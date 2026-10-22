"""Fresh Unity 4.5 LightProbes -> Unity 2017.4.40f1 conversion, without rebaking."""
import argparse
import hashlib
import json
import math
from pathlib import Path
import re
import struct
import uuid

# Unity 4.x stores raw real-SH coefficients in RGB interleaved order. Unity
# 2017 stores channel-major, basis-weighted, cosine-convolved coefficients.
FACTORS = (1 / (2 * math.sqrt(math.pi)),
           -math.sqrt(3 / math.pi) / 3, math.sqrt(3 / math.pi) / 3, -math.sqrt(3 / math.pi) / 3,
           math.sqrt(15 / math.pi) / 8, -math.sqrt(15 / math.pi) / 8,
           math.sqrt(5 / math.pi) / 16, -math.sqrt(15 / math.pi) / 8,
           math.sqrt(15 / math.pi) / 16)


def f32(value):
    return struct.unpack('<f', struct.pack('<f', float(value)))[0]


def blocks(text):
    fields = list(re.finditer(r'^  ([A-Za-z_][\w]*):[^\n]*\n', text, re.M))
    return {m.group(1): text[m.end():fields[i + 1].start() if i + 1 < len(fields) else len(text)]
            for i, m in enumerate(fields)}


def vectors(block):
    return [[f32(v) for v in match] for match in re.findall(r'\{x: ([^,]+), y: ([^,]+), z: ([^}]+)\}', block)]


def store(path, content):
    encoded = content.encode('utf-8')
    if path.exists() and path.read_bytes() != encoded:
        raise FileExistsError('Refusing to replace a different file: ' + str(path))
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_bytes(encoded)


def convert(source, output, level=None, refresh_matrices=True):
    if source.resolve() == output.resolve():
        raise ValueError('Source and output must be different')
    original = source.read_bytes()
    text = original.decode('utf-8-sig').replace('\r\n', '\n')
    if not re.search(r'^--- !u!197 &19700000\s*$', text, re.M):
        raise ValueError('Expected original Unity 4.x class 197 LightProbes')
    sections = blocks(text)
    required = {'bakedPositions', 'bakedCoefficients', 'tetrahedra', 'hullRays'}
    if not required.issubset(sections):
        raise ValueError('Missing original probe geometry or coefficient fields')
    positions = vectors(sections['bakedPositions'])
    hull = vectors(sections['hullRays'])
    values = [(int(k), f32(v)) for k, v in re.findall(r'sh\[\s*(\d+)\]:\s*([^\s]+)', sections['bakedCoefficients'])]
    if not positions or len(values) != len(positions) * 27 or len(hull) != len(positions):
        raise ValueError('Position/coefficient/hull count mismatch')
    if [k for k, _ in values] != list(range(27)) * len(positions):
        raise ValueError('Missing, duplicate or unordered SH coefficients')
    if not all(math.isfinite(v) for _, v in values) or not all(math.isfinite(v) for p in positions + hull for v in p):
        raise ValueError('Nonfinite source data')
    source_coefficients = [[v for _, v in values[i:i + 27]] for i in range(0, len(values), 27)]
    tetrahedra = []
    tetra_blocks = []
    for tetra in re.split(r'(?=^  - indices\[0\]:)', sections['tetrahedra'], flags=re.M):
        if not tetra.strip():
            continue
        indices = [int(i) for i in re.findall(r'indices\[\d\]: (-?\d+)', tetra)]
        if len(indices) != 4 or any(i < -2 or i >= len(positions) for i in indices):
            raise ValueError('Invalid tetrahedron vertex references')
        tetrahedra.append({'indices': indices})
        if refresh_matrices and min(indices) >= 0:
            p = [positions[i] for i in indices]
            columns = [[p[j][k] - p[3][k] for k in range(3)] for j in range(3)]
            def cross(a, b):
                return [a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]]
            rows = [cross(columns[1], columns[2]), cross(columns[2], columns[0]), cross(columns[0], columns[1])]
            determinant = sum(a * b for a, b in zip(columns[0], rows[0]))
            if determinant != 0:
                for a in range(3):
                    for b in range(3):
                        value = f32(rows[a][b] / determinant)
                        tetra = re.sub(r'(\be%d%d: )[^\s]+' % (a, b), lambda m: m.group(1) + ('%.9g' % value), tetra)
        tetra_blocks.append(tetra.rstrip())
    if not tetrahedra:
        raise ValueError('No original tetrahedralization')
    level = level or source.parent.name
    source_sha = hashlib.sha256(original).hexdigest()
    probe_hash = source_sha[:32]
    lines = ['%YAML 1.1', '%TAG !u! tag:unity3d.com,2011:', '--- !u!258 &25800000', 'LightProbes:',
             '  m_ObjectHideFlags: 0', '  m_PrefabParentObject: {fileID: 0}', '  m_PrefabInternal: {fileID: 0}',
             '  m_Name: LightProbes_' + level + '_Recovered', '  m_Data:', '    m_Tetrahedralization:', '      m_Tetrahedra:']
    lines += ['    ' + line for block in tetra_blocks for line in block.splitlines()]
    lines += ['      m_HullRays:'] + ['    ' + line for line in sections['hullRays'].rstrip().splitlines()]
    lines += ['    m_ProbeSets:', '    - m_Hash:', '        serializedVersion: 2', '        Hash: ' + probe_hash,
              '      m_Offset: 0', '      m_Size: ' + str(len(positions)), '    m_Positions:']
    lines += ['  ' + line for line in sections['bakedPositions'].rstrip().splitlines()]
    lines += ['    m_NonTetrahedralizedProbeSetIndexMap: []', '  m_BakedCoefficients:']
    for coefficients in source_coefficients:
        for c in range(3):
            for k in range(9):
                index = c * 9 + k
                value = f32(coefficients[k * 3 + c] * FACTORS[k])
                lines.append(('  - ' if index == 0 else '    ') + 'sh[%2d]: %.9g' % (index, value))
    lines += ['  m_BakedLightOcclusion: []']
    yaml = '\n'.join(lines) + '\n'
    guid = uuid.uuid5(uuid.NAMESPACE_URL, 'LightProbeRecovery/v1/' + level + '/' + source_sha).hex
    store(output, yaml)
    store(output.with_suffix('.asset.meta'), 'fileFormatVersion: 2\nguid: ' + guid + '\nNativeFormatImporter:\n  externalObjects: {}\n  mainObjectFileID: 25800000\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n')
    manifest = {'level': level, 'source': str(source.resolve()), 'sourceSHA256': source_sha,
                'outputSHA256': hashlib.sha256(yaml.encode()).hexdigest(), 'probeCount': len(positions),
                'cellCount': len(tetrahedra), 'coefficientCount': len(values),
                'basisFactors': FACTORS, 'status': 'UNVERIFIED UNTIL NATIVE UNITY TESTS PASS',
                'refreshedInteriorInverseMatrices': refresh_matrices,
                'probes': [{'position': dict(zip(('x', 'y', 'z'), p)), 'legacyCoefficients': sh}
                           for p, sh in zip(positions, source_coefficients)], 'tetrahedra': tetrahedra}
    store(output.with_suffix('.recovery.json'), json.dumps(manifest, indent=2) + '\n')
    return manifest


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('source_assets', type=Path, help='Untouched export Assets directory')
    parser.add_argument('--output', type=Path, default=Path('Assets/RecoveredLightProbes'))
    parser.add_argument('--keep-legacy-matrices', action='store_true', help='Diagnostic only: keep old inverse matrices, including their precision errors')
    args = parser.parse_args()
    sources = sorted(args.source_assets.glob('Levels/Test/*/LightProbes.asset'))
    if not sources:
        parser.error('No original LightProbes assets found under Levels/Test')
    for source in sources:
        result = convert(source, args.output / source.parent.name / 'LightProbes.asset', refresh_matrices=not args.keep_legacy_matrices)
        print('%s: %d probes, %d tetrahedra, %d coefficients' % (source.parent.name, result['probeCount'], result['cellCount'], result['coefficientCount']))
