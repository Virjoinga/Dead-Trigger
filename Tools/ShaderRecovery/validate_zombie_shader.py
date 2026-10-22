"""Native GPU comparison in an isolated Unity 2017.4.40f1 project."""
import hashlib
import json
from pathlib import Path
import shutil
import struct
import subprocess
import zlib

ROOT = Path(__file__).resolve().parents[2]
HERE = Path(__file__).resolve().parent
SHADER = Path('Assets/Shader/Mobile-TexPseudoBRDF-WrapAround-customgloss-zombie.shader')


def png(path, rgba):
    def chunk(kind, data):
        return struct.pack('>I', len(data)) + kind + data + struct.pack('>I', zlib.crc32(kind + data))
    path.write_bytes(b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>2I5B', 8, 8, 8, 6, 0, 0, 0))
                     + chunk(b'IDAT', zlib.compress((b'\0' + bytes(rgba) * 8) * 8)) + chunk(b'IEND', b''))


def fixture_shader(text, name):
    # Retain every shader operation, substituting only controlled probe inputs.
    # Native per-renderer ambient updates must not override the fixture values.
    text = text.replace('Shader "MADFINGER/Characters/BRDFLit (Supports Backlight) - custom glossingess mask - zombie"', 'Shader "Hidden/' + name + '"')
    declarations = []
    for suffix in ('Ar', 'Ag', 'Ab', 'Br', 'Bg', 'Bb', 'C'):
        text = text.replace('unity_SH' + suffix, '_FixtureSH' + suffix)
        declarations.append('float4 _FixtureSH' + suffix + ';')
    return text.replace('#include "UnityCG.cginc"', '#include "UnityCG.cginc"\n' + '\n'.join(declarations))


def main():
    harness = ROOT / 'Temp/ZombieShaderHarness'
    marker = harness / 'ZombieShaderHarness.marker'
    if harness.exists() and not marker.exists():
        raise RuntimeError('Refusing to modify unmarked harness')
    for folder in ('Assets/Editor', 'ProjectSettings'):
        (harness / folder).mkdir(parents=True, exist_ok=True)
    marker.write_text('Isolated zombie shader validation\n')
    shutil.copyfile(ROOT / 'ProjectSettings/ProjectVersion.txt', harness / 'ProjectSettings/ProjectVersion.txt')
    shutil.copyfile(HERE / 'ValidateZombieShader.cs', harness / 'Assets/Editor/ValidateZombieShader.cs')
    shutil.copyfile(HERE / 'ZombieReference.shader', harness / 'Assets/ZombieReference.shader')
    candidate = (ROOT / SHADER).read_text()
    # Reconstruct the pre-fix translation, retaining all other user shader edits.
    baseline = candidate.replace('tmpvar_18[0] = tmpvar_16;\n                  tmpvar_18[1] = tmpvar_17;\n                  tmpvar_18[2] = tmpvar_2;',
                                 'tmpvar_18[0] = float3(tmpvar_16.x,tmpvar_17.x,tmpvar_2.x);\n'
                                 'tmpvar_18[1] = float3(tmpvar_16.y,tmpvar_17.y,tmpvar_2.y);\n'
                                 'tmpvar_18[2] = float3(tmpvar_16.z,tmpvar_17.z,tmpvar_2.z);')
    baseline = baseline.replace('UnpackNormal(tex2D (_BumpMap, i.texcoord0))', '((tex2D (_BumpMap, i.texcoord0).xyz * 2.0) - 1.0)')
    if baseline == candidate:
        raise RuntimeError('Could not reconstruct the pre-fix baseline')
    (harness / 'Assets/Candidate.shader').write_text(fixture_shader(candidate, 'ZombieCandidate'))
    (harness / 'Assets/Baseline.shader').write_text(fixture_shader(baseline, 'ZombieBaseline'))
    # Import the unmodified production shader as well to catch ordinary compile errors.
    shutil.copyfile(ROOT / SHADER, harness / 'Assets/Production.shader')
    png(harness / 'Assets/FlatNormal.png', (128,128,255,255))
    png(harness / 'Assets/TiltedNormal.png', (204,153,226,255))
    shutil.copyfile(ROOT / 'Assets/Texture2D/zombie01_n.png', harness / 'Assets/ZombieNormal.png')
    report = harness / 'ZombieShaderValidation.json'
    if report.exists():
        report.unlink()
    unity = Path('C:/Program Files/Unity/Hub/Editor/2017.4.40f1/Editor/Unity.exe')
    result = subprocess.run([str(unity), '-batchmode', '-force-d3d11', '-quit', '-projectPath', str(harness), '-executeMethod', 'ValidateZombieShader.Run', '-logFile', str(harness / 'validation.log')])
    if result.returncode or not report.exists():
        raise RuntimeError('GPU validation failed; inspect ' + str(harness / 'validation.log'))
    content = json.loads(report.read_text())
    if not content['passed']:
        raise RuntimeError('GPU shader comparison failed')
    content['productionShaderSHA256'] = hashlib.sha256((ROOT / SHADER).read_bytes()).hexdigest()
    (HERE / report.name).write_text(json.dumps(content, indent=2) + '\n')
    print(json.dumps(content, indent=2))


if __name__ == '__main__':
    main()
