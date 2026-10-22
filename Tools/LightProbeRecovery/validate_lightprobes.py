"""Validate recovered assets in an isolated Unity 2017 project, never the game project."""
import argparse
import hashlib
import json
from pathlib import Path
import shutil
import subprocess


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--unity', type=Path, default=Path('C:/Program Files/Unity/Hub/Editor/2017.4.40f1/Editor/Unity.exe'))
    args = parser.parse_args()
    root = Path(__file__).resolve().parents[2]
    tools = root / 'Tools/LightProbeRecovery'
    harness = root / 'Temp/LightProbeRecoveryHarness'
    marker = harness / 'LightProbeRecoveryHarness.marker'
    if harness.exists() and not marker.exists():
        raise RuntimeError('Refusing to modify an unmarked harness directory')
    harness.mkdir(parents=True, exist_ok=True)
    marker.write_text('Isolated LightProbe recovery validation\n')
    for directory in ('Assets/Editor', 'ProjectSettings'):
        (harness / directory).mkdir(parents=True, exist_ok=True)
    shutil.copyfile(root / 'ProjectSettings/ProjectVersion.txt', harness / 'ProjectSettings/ProjectVersion.txt')
    for source in (tools / 'ValidateLightProbeRecovery.cs', tools / 'TestSceneLightProbes.cs', root / 'Assets/Editor/AttachRecoveredLightProbes.cs'):
        shutil.copyfile(source, harness / 'Assets/Editor' / source.name)
    cases = []
    hashes = {}
    for asset in sorted((root / 'Assets/RecoveredLightProbes').glob('*/LightProbes.asset')):
        relative = asset.relative_to(root)
        target = harness / relative
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(asset, target)
        shutil.copyfile(asset.with_suffix('.recovery.json'), target.with_suffix('.recovery.json'))
        # Unity can mark imported metadata read-only. Existing deterministic GUIDs are retained.
        if not target.with_suffix('.asset.meta').exists():
            shutil.copyfile(asset.with_suffix('.asset.meta'), target.with_suffix('.asset.meta'))
        cases.append(relative.as_posix())
        hashes[relative.as_posix()] = hashlib.sha256(asset.read_bytes()).hexdigest()
    if len(cases) != 14:
        raise RuntimeError('Expected the 14 original exported probe assets')
    (harness / 'LightProbeCases.json').write_text(json.dumps({'assets': cases}))
    shutil.copyfile(tools / 'LightingTemplates/mall.yaml', harness / 'LightingTemplate.yaml')
    for method, report_name in [('ValidateLightProbeRecovery.Run', 'LightProbeRecoveryResults.json'), ('TestSceneLightProbes.Run', 'NativeProbeSceneResult.json')]:
        report = harness / report_name
        if report.exists():
            report.unlink()
        log = harness / (method.split('.')[0] + '.log')
        result = subprocess.run([str(args.unity), '-batchmode', '-nographics', '-quit', '-projectPath', str(harness), '-executeMethod', method, '-logFile', str(log)])
        if result.returncode or not report.exists():
            raise RuntimeError('Native validation failed; inspect ' + str(log))
        content = json.loads(report.read_text())
        if report_name == 'LightProbeRecoveryResults.json':
            if not content['basisCalibrationPassed'] or not all(item['passed'] for item in content['results']):
                raise RuntimeError('Native numerical validation failed')
        elif not content['passed']:
            raise RuntimeError('Native scene attachment failed')
        shutil.copyfile(report, tools / report_name)
    (tools / 'ValidatedAssetHashes.json').write_text(json.dumps(hashes, indent=2) + '\n')
    print('All 14 assets passed native numerical checks; mall passed scene save/reopen using the editor attachment helper.')


if __name__ == '__main__':
    main()
