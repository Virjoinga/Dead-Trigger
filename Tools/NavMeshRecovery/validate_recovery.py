"""Prepare a dedicated minimal Unity project and validate recovered assets."""
import argparse
import json
from pathlib import Path
import shutil
import subprocess
import time


def main():
    root = Path(__file__).resolve().parents[2]
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--assets', type=Path, default=root / 'Assets/RecoveredNavMeshes')
    parser.add_argument('--unity', type=Path)
    args = parser.parse_args()
    harness = root / 'Temp/NavMeshRecoveryBatch'
    asset_target = harness / 'Assets/RecoveredNavMeshes'
    editor = harness / 'Assets/Editor'
    settings = harness / 'ProjectSettings'
    for directory in (asset_target, editor, settings):
        directory.mkdir(parents=True, exist_ok=True)
    (harness / 'NavMeshRecoveryHarness.marker').write_text('Isolated native NavMesh recovery tests\n')
    cases = []
    # No deletion or modification of any source or gameplay project asset.
    for source in args.assets.rglob('*'):
        if source.is_file() and source.suffix in ('.asset', '.json'):
            destination = asset_target / source.relative_to(args.assets)
            destination.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(source, destination)
            if source.suffix == '.asset':
                cases.append(destination.relative_to(harness).as_posix())
    (harness / 'RecoveryCases.json').write_text(json.dumps({'assets': sorted(cases)}, indent=2))
    shutil.copy2(root / 'Tools/NavMeshRecovery/ValidateRecoveredNavMeshes.cs', editor)
    shutil.copy2(root / 'Assets/Editor/AttachSceneNavMesh.cs', editor)
    shutil.copy2(root / 'Assets/Editor/RestoreBakedLinkEndpoints.cs', editor)
    shutil.copy2(root / 'ProjectSettings/ProjectVersion.txt', settings)
    shutil.copy2(root / 'ProjectSettings/NavMeshAreas.asset', settings)
    print('Prepared', harness)
    if args.unity:
        started = time.time()
        process = subprocess.run([str(args.unity), '-batchmode', '-nographics', '-quit', '-projectPath', str(harness),
                                  '-executeMethod', 'ValidateRecoveredNavMeshes.Run', '-logFile', str(harness / 'validation.log')])
        report = harness / 'RecoveryBatchResults.json'
        if not report.exists() or report.stat().st_mtime < started - 2:
            raise SystemExit('No fresh native report; inspect ' + str(harness / 'validation.log'))
        output = root / 'Tools/NavMeshRecovery/RecoveryBatchResults.json'
        shutil.copy2(report, output)
        results = json.loads(report.read_text())['results']
        print('%d/%d native cases passed; report: %s' % (sum(r['passed'] for r in results), len(results), output))
        if process.returncode or not all(r['passed'] for r in results):
            raise SystemExit(1)


if __name__ == '__main__':
    main()
