"""Independently check preservation of source polygon/detail records and coordinates."""
import argparse
import hashlib
import json
from pathlib import Path
import struct

from legacy_format import legacy_tiles, read_2017_template


def verify(asset):
    manifest = json.loads(asset.with_suffix('.recovery.json').read_text())
    source = Path(manifest['source'])
    assert hashlib.sha256(source.read_bytes()).hexdigest() == manifest['sourceSHA256'], 'Source changed'
    assert hashlib.sha256(asset.read_bytes()).hexdigest() == manifest['outputSHA256'], 'Output changed'
    blob, prefix, tail, _, data_offset, _ = read_2017_template(asset)
    position = struct.unpack_from('<3f', tail, 108)
    offset = data_offset + len(prefix)
    count = struct.unpack_from('<I', blob, offset)[0]
    offset += 4
    originals = list(legacy_tiles(source))
    assert count == len(originals)
    error = 0
    vertices_checked = 0
    for _, old in originals:
        length = struct.unpack_from('<I', blob, offset)[0]
        tile = blob[offset + 4:offset + 4 + length]
        offset += 4 + length + 16
        pc, vc, links, dm, dv, dt = struct.unpack_from('<6I', old, 24)
        assert struct.unpack_from('<3i', old, 8) == struct.unpack_from('<3i', tile, 8), 'Tile coordinates changed'
        old_polys = 104 + 12 * vc
        new_polys = 72 + 12 * vc
        for poly in range(dm):
            assert old[old_polys + 36 * poly + 4:old_polys + 36 * (poly + 1)] == tile[new_polys + 32 * poly:new_polys + 32 * (poly + 1)], 'Ground polygon record changed'
        old_details = old_polys + 36 * pc + 12 * links
        new_details = new_polys + 32 * dm
        assert old[old_details:old_details + 12 * dm] == tile[new_details:new_details + 12 * dm], 'Detail mesh record changed'
        old_triangles = old_details + 12 * dm + 12 * dv
        new_triangles = new_details + 12 * dm + 12 * dv
        assert old[old_triangles:old_triangles + 8 * dt] == tile[new_triangles:new_triangles + 8 * dt], 'Detail triangles changed'
        for old_base, new_base, n in ((104, 72, vc), (old_details + 12 * dm, new_details + 12 * dm, dv)):
            for i in range(n):
                a = struct.unpack_from('<3f', old, old_base + 12 * i)
                b = struct.unpack_from('<3f', tile, new_base + 12 * i)
                error = max(error, *(abs(a[k] - (b[k] + position[k])) for k in range(3)))
                vertices_checked += 1
        assert error < 0.0001, 'World geometry changed beyond floating-point tolerance'
    return vertices_checked, error


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--assets', type=Path, default=Path(__file__).resolve().parents[2] / 'Assets/RecoveredNavMeshes')
    args = parser.parse_args()
    assets = sorted(args.assets.rglob('*.asset'))
    if not assets:
        parser.error('No recovered assets found')
    totals = [verify(asset) for asset in assets]
    print('Verified %d assets, %d vertices; polygons/detail records unchanged; maximum world error %.10f' % (len(assets), sum(n for n, _ in totals), max(e for _, e in totals)))
