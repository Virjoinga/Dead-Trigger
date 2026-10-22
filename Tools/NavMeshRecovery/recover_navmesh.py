"""Recover supported Unity 4.5 text NavMeshes as native Unity 2017.4.40f1 assets.

No rebaking. Original files are read-only. A real binary 2017 NavMeshData is
required as a serialization template. Native validation is a separate step.
"""
import argparse
from collections import defaultdict
import hashlib
import json
import math
from pathlib import Path
import re
import struct

from legacy_format import U32, F32, VEC, legacy_tiles, convert_tile, read_2017_template


def vec(value):
    return dict(zip(('x', 'y', 'z'), value))


def build_bv_tree(bounds, minimum, quant, original):
    """Refit the original BV topology conservatively around detailed surfaces."""
    if original:
        nodes = [list(struct.unpack_from('<6Hi', original, p)) for p in range(0, len(original), 16)]

        def refit(index):
            node = nodes[index]
            value = node[6]
            if value >= 0:
                if value >= len(bounds):
                    raise ValueError('BV leaf refers to a removed or invalid polygon')
                lo, hi = bounds[value]
                if any(not math.isfinite(v) for v in lo + hi) or any((hi[a] - minimum[a]) * quant > 65534 for a in range(3)):
                    raise ValueError('Invalid surface or BV quantization range')
                end = index + 1
            else:
                left_lo, left_hi, middle = refit(index + 1)
                right_lo, right_hi, end = refit(middle)
                if end != index - value:
                    raise ValueError('Unsupported BV escape topology')
                lo = [min(a, b) for a, b in zip(left_lo, right_lo)]
                hi = [max(a, b) for a, b in zip(left_hi, right_hi)]
            for axis in range(3):
                node[axis] = min(node[axis], max(0, math.floor((lo[axis] - minimum[axis]) * quant) - 1))
                node[axis + 3] = max(node[axis + 3], min(65535, math.ceil((hi[axis] - minimum[axis]) * quant) + 1))
            return lo, hi, end

        refit(0)
        return b''.join(struct.pack('<6Hi', *node) for node in nodes)
    nodes = []

    def build(indices):
        lo = [min(bounds[i][0][a] for i in indices) for a in range(3)]
        hi = [max(bounds[i][1][a] for i in indices) for a in range(3)]
        qlo = [max(0, math.floor((lo[a] - minimum[a]) * quant) - 1) for a in range(3)]
        qhi = [min(65535, math.ceil((hi[a] - minimum[a]) * quant) + 1) for a in range(3)]
        if any(n > 65535 for n in qlo) or any(n < 0 for n in qhi):
            raise ValueError('Detail surface exceeds BV quantization range')
        index = len(nodes)
        nodes.append(None)
        if len(indices) == 1:
            value = indices[0]
        else:
            axis = max(range(3), key=lambda a: hi[a] - lo[a])
            indices.sort(key=lambda i: (bounds[i][0][axis] + bounds[i][1][axis], i))
            middle = len(indices) // 2
            build(indices[:middle])
            build(indices[middle:])
            value = -(len(nodes) - index)
        nodes[index] = struct.pack('<6Hi', *(qlo + qhi), value)

    if bounds:
        build(list(range(len(bounds))))
    return b''.join(nodes)


def store(path, content):
    if path.exists() and path.read_bytes() != content:
        raise FileExistsError('Refusing to replace a different file: ' + str(path))
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_bytes(content)


def read_container(path):
    text = path.read_text('utf-8-sig')
    match = re.search(r'^\s*m_MeshData:\s*([0-9a-fA-F]+)', text, re.M)
    if not match:
        raise ValueError('Expected Unity 4.5 YAML NavMesh.m_MeshData')
    blob = bytes.fromhex(match.group(1))
    if len(blob) < 40 or blob[:4] != b'TESM' or U32(blob, 4) != 1:
        raise ValueError('Unsupported legacy container; expected TESM version 1')
    if not re.search(r'^\s*m_Heightmaps:\s*\[\]\s*$', text, re.M):
        raise ValueError('Nonempty or absent legacy heightmaps are not supported')
    origin = VEC(blob, 12)
    width, depth = struct.unpack_from('<2f', blob, 24)
    if not all(math.isfinite(v) for v in (*origin, width, depth)) or width <= 0 or not math.isclose(width, depth, rel_tol=1e-6):
        raise ValueError('Expected a finite square legacy tile grid')
    tiles = list(legacy_tiles(path))
    if not tiles:
        raise ValueError('Empty legacy NavMesh')
    return origin, width, tiles


def recover(source, reference_path, output, agent_type=0):
    if source.resolve() == output.resolve() or reference_path.resolve() == output.resolve():
        raise ValueError('Output must differ from source and template')
    origin, width, old_tiles = read_container(source)
    shift = (origin[0], 0.0, origin[2])
    parameters = [struct.unpack_from('<3f', old, 64) for _, old in old_tiles]
    height, radius, climb = parameters[0]
    if any(any(abs(a - b) > 1e-5 for a, b in zip(p, parameters[0])) for p in parameters):
        raise ValueError('Tiles have inconsistent native agent dimensions')
    if not all(math.isfinite(v) and v >= 0 for v in (height, radius, climb)) or height <= 0:
        raise ValueError('Invalid native agent dimensions')
    quant = F32(old_tiles[0][1], 100)
    if not math.isfinite(quant) or quant <= 0:
        raise ValueError('Invalid BV quantization factor')
    tile_size = max(1, int(round(width * quant)))
    voxel = width / tile_size
    reference, prefix, tail, _, data_offset, object_size_at = read_2017_template(reference_path)
    if struct.unpack_from('<4f', tail, 92) != (0, 0, 0, 1) or VEC(tail, 108) != (0, 0, 0):
        raise ValueError('Serialization template must have an identity transform')
    tiles = []
    rows = []
    centers = {}
    pairs = set()
    portals = defaultdict(list)
    links = []
    surface_samples = []
    expected_triangles = 0
    max_error = 0.0
    coordinates = set()
    for index, old in old_tiles:
        converted, row = convert_tile(old)
        x, z, layer = row['x'], row['z'], row['layer']
        if (x, z, layer) in coordinates:
            raise ValueError('Duplicate tile coordinates')
        coordinates.add((x, z, layer))
        for axis, coordinate in ((0, x), (2, z)):
            expected_min = origin[axis] + coordinate * width
            if abs(row['bounds_min'][axis] - expected_min) > max(0.001, width * 1e-5):
                raise ValueError('Tile bounds do not match container grid')
        pc, vc, dm, dv, dt, bv = struct.unpack_from('<6I', converted, 20)
        bv_start = 72 + 12 * vc + 32 * pc + 12 * dm + 12 * dv + 8 * dt
        # BV bounds are quantized relative to each tile's minimum. Translating
        # the tile minimum and geometry together leaves these values unchanged.
        # Keep them: coarse polygon bounds can omit large detail-surface hills.
        tile = bytearray(converted)
        if len(tile) != bv_start + 16 * bv:
            raise ValueError('Invalid BV section size')

        def translate(offset, verify=False):
            nonlocal max_error
            before = VEC(tile, offset)
            struct.pack_into('<3f', tile, offset, *(a - b for a, b in zip(before, shift)))
            if verify:
                after = VEC(tile, offset)
                max_error = max(max_error, *(abs(before[k] - (after[k] + shift[k])) for k in range(3)))

        translate(44)
        translate(56)
        for i in range(vc):
            translate(72 + 12 * i, True)
        details = 72 + 12 * vc + 32 * pc + 12 * dm
        for i in range(dv):
            translate(details + 12 * i, True)
        vertices = [VEC(old, 104 + 12 * i) for i in range(vc)]
        old_pc, _, old_links, old_dm, old_dv, old_dt = struct.unpack_from('<6I', old, 24)
        detail_base = 104 + 12 * vc + 36 * old_pc + 12 * old_links
        detail_vertices = detail_base + 12 * old_dm
        detail_triangles = detail_vertices + 12 * old_dv
        poly_bounds = []
        for poly in range(pc):
            p = 104 + 12 * vc + 36 * poly
            count = old[p + 32]
            points = [vertices[v] for v in struct.unpack_from('<6H', old, p + 4)[:count]]
            center = tuple(sum(v[a] for v in points) / count for a in range(3))
            vb, tb, vn, tn = struct.unpack_from('<IIHH', old, detail_base + poly * 12)
            surface = points + [VEC(old, detail_vertices + 12 * (vb + j)) for j in range(vn)]
            if any(not math.isfinite(value) for point in surface for value in point):
                raise ValueError('Nonfinite detail geometry')
            poly_bounds.append(([min(v[a] for v in surface) for a in range(3)], [max(v[a] for v in surface) for a in range(3)]))
            for triangle in range(tn):
                ids = struct.unpack_from('<4H', old, detail_triangles + 8 * (tb + triangle))[:3]
                if any(v >= len(surface) for v in ids):
                    raise ValueError('Invalid detail triangle index')
                a, b, c = [surface[v] for v in ids]
                surface_samples.append(vec(tuple((a[k] + b[k] + c[k]) / 3 for k in range(3))))
                # Coarse polygon Y can differ substantially from the actual
                # detailed surface. Lift adjacency probes to the original surface.
                denominator = (b[2] - c[2]) * (a[0] - c[0]) + (c[0] - b[0]) * (a[2] - c[2])
                if abs(denominator) > 1e-12:
                    wa = ((b[2] - c[2]) * (center[0] - c[0]) + (c[0] - b[0]) * (center[2] - c[2])) / denominator
                    wb = ((c[2] - a[2]) * (center[0] - c[0]) + (a[0] - c[0]) * (center[2] - c[2])) / denominator
                    wc = 1 - wa - wb
                    if min(wa, wb, wc) >= -1e-5:
                        center = (center[0], wa * a[1] + wb * b[1] + wc * c[1], center[2])
            centers[index, poly] = center
            expected_triangles += count - 2
            for edge, neighbor in enumerate(struct.unpack_from('<6H', old, p + 16)[:count]):
                if neighbor & 0x8000:
                    ends = [tuple(round(v, 3) for v in points[j]) for j in (edge, (edge + 1) % count)]
                    portals[layer, tuple(sorted(ends))].append((index, poly))
                elif neighbor:
                    pairs.add(tuple(sorted(((index, poly), (index, neighbor - 1)))))
        off_count = U32(old, 52)
        connections = len(old) - 40 * off_count
        old_pc = U32(old, 24)
        for i in range(off_count):
            p = connections + 40 * i
            poly = struct.unpack_from('<H', old, p + 32)[0]
            if poly < pc or poly >= old_pc:
                raise ValueError('Invalid embedded link polygon index')
            area = old[104 + 12 * vc + 36 * poly + 33] & 63
            poly_offset = 104 + 12 * vc + 36 * poly
            endpoint_ids = struct.unpack_from('<2H', old, poly_offset + 4)
            attachment_edges = set()
            link_index = U32(old, poly_offset)
            visited = set()
            runtime_links = 104 + 12 * vc + 36 * old_pc
            while link_index != 0xffffffff:
                if link_index >= old_links or link_index in visited:
                    raise ValueError('Invalid legacy runtime link list')
                visited.add(link_index)
                link_offset = runtime_links + 12 * link_index
                attachment_edges.add(old[link_offset + 8])
                link_index = U32(old, link_offset + 4)
            links.append({'id': '%d:%d:%d' % (index, i, U32(old, p + 36)),
                          'start': vec(VEC(old, p)), 'end': vec(VEC(old, p + 12)),
                          'bakedStart': vec(vertices[endpoint_ids[0]]), 'bakedEnd': vec(vertices[endpoint_ids[1]]),
                          'sourceAttached': 0 in attachment_edges and 1 in attachment_edges,
                          'bidirectional': bool(old[p + 34] & 1), 'area': area,
                          'radius': F32(old, p + 24)})
        bvtree = build_bv_tree(poly_bounds, row['bounds_min'], F32(old, 100), converted[bv_start:])
        tile = tile[:bv_start] + bvtree
        struct.pack_into('<I', tile, 40, len(bvtree) // 16)
        row.update(index=index, expectedTriangles=sum(old[104 + 12 * vc + 36 * p + 32] - 2 for p in range(pc)),
                   refittedBVNodes=len(bvtree) // 16, new_size=len(tile))
        rows.append(row)
        tiles.append(bytes(tile))
    for nodes in portals.values():
        if len(nodes) == 2 and nodes[0][0] != nodes[1][0]:
            pairs.add(tuple(sorted(nodes)))
    probes = []
    for a, b in sorted(pairs):
        for start, end in ((a, b), (b, a)):
            probes.append({'start': vec(centers[start]), 'end': vec(centers[end]),
                           'crossTile': start[0] != end[0], 'label': '%s -> %s' % (start, end)})
    tail = bytearray(tail)
    struct.pack_into('<i7f', tail, 0, agent_type, radius, height, 45, climb, 0, 0, 2)
    struct.pack_into('<ifii', tail, 32, 1, voxel, 1, tile_size)
    mins = [min(r['bounds_min'][a] for r in rows) for a in range(3)]
    maxs = [max(r['bounds_max'][a] for r in rows) for a in range(3)]
    center = [(a + b) / 2 - s for a, b, s in zip(mins, maxs, shift)]
    extent = [(b - a) / 2 for a, b in zip(mins, maxs)]
    struct.pack_into('<6f', tail, 68, *(center + extent))
    struct.pack_into('<3f', tail, 108, *shift)
    name = (source.stem + '_Recovered').encode('utf-8')
    if len(name) > 256:
        raise ValueError('Recovered object name exceeds supported serialization limit')
    prefix = prefix[:28] + struct.pack('<I', len(name)) + name + b'\0' * (-len(name) % 4)
    obj = prefix + struct.pack('<I', len(tiles)) + b''.join(struct.pack('<I', len(t)) + t + bytes(16) for t in tiles) + tail
    result = bytearray(reference[:data_offset]) + obj
    struct.pack_into('>I', result, 4, len(result))
    struct.pack_into('<I', result, object_size_at, len(obj))
    manifest = {'status': 'STRUCTURALLY CHECKED; NATIVE UNITY VALIDATION REQUIRED',
                'source': str(source.resolve()), 'sourceSHA256': hashlib.sha256(source.read_bytes()).hexdigest(),
                'output': output.name, 'format': 'TESM1/VAND13 -> Unity2017.4.40f1/VAND16',
                'outputSHA256': hashlib.sha256(result).hexdigest(),
                'origin': vec(origin), 'position': vec(shift), 'tileWidth': width,
                'voxelSize': voxel, 'tileSize': tile_size, 'agentTypeID': agent_type,
                'agentRadius': radius, 'agentHeight': height, 'agentClimb': climb,
                'maxVertexWorldError': max_error, 'expectedTriangles': expected_triangles,
                'groundPolygons': sum(r['polygons'] for r in rows), 'tiles': rows,
                'probes': probes, 'surfaceSamples': surface_samples, 'links': links,
                'limitations': 'Embedded links are exported for validation; retain original scene OffMeshLink/ActionPoint objects. Heightmaps and other binary layouts are unsupported. Slope/region settings use defaults; no rebake is performed.'}
    store(output, result)
    store(output.with_suffix('.recovery.json'), (json.dumps(manifest, indent=2) + '\n').encode())
    # Confirm serialization can be parsed after writing.
    read_2017_template(output)
    return manifest


def main():
    root = Path(__file__).resolve().parents[2]
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('inputs', nargs='*', type=Path, help='Legacy .asset files or directories (recursive)')
    parser.add_argument('--supplied', action='store_true', help='All 15 supplied files in docs/source/legacy')
    parser.add_argument('--reference', type=Path, default=root / 'docs/source/reference/NavMesh(10).asset')
    parser.add_argument('--output-dir', type=Path, default=root / 'Assets/RecoveredNavMeshes')
    parser.add_argument('--agent-type', type=int, default=0)
    args = parser.parse_args()
    paths = set()
    for path in args.inputs + ([root / 'docs/source/legacy'] if args.supplied else []):
        if path.is_dir():
            paths.update(p.resolve() for p in path.rglob('*.asset'))
        else:
            paths.add(path.resolve())
    if not paths:
        parser.error('Supply legacy assets/directories or --supplied')
    names = [p.stem.casefold() for p in paths]
    if len(names) != len(set(names)):
        parser.error('Inputs have duplicate filenames; convert those separately into different output directories')
    failures = []
    for source in sorted(paths):
        try:
            output = args.output_dir / source.stem / 'NavMesh.asset'
            report = recover(source, args.reference, output, args.agent_type)
            print('%s: %d tiles, %d triangles, %d links -> %s' % (source.name, len(report['tiles']), report['expectedTriangles'], len(report['links']), output))
        except (ValueError, OSError, struct.error) as error:
            failures.append((str(source), str(error)))
            print('FAILED %s: %s' % (source, error))
    if failures:
        raise SystemExit('%d source(s) failed; original assets were not changed' % len(failures))


if __name__ == '__main__':
    main()
