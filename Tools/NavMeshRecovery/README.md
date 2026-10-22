# Legacy NavMesh recovery

Direct recovery of the supplied Unity 4.5 NavMeshes into **Unity 2017.4.40f1**
NavMeshData. Uses Python's standard library; no OBJ rebake, UnityPy or pip
dependencies. The original files and gameplay scenes are not overwritten.

## Ready-to-use results

`Assets/RecoveredNavMeshes/` contains all **15 supplied files**. Each subfolder
retains the original uploaded filename and contains `NavMesh.asset` plus a
`NavMesh.recovery.json` provenance/validation sidecar. Original numbered and
timestamped names have not been guessed into level names. The original
`NavMesh.asset` and `NavMesh(20261021-054020-3).asset` contain the same level.

Open the corresponding gameplay scene, select its recovered **asset**, then
run **Tools > Navigation > Attach Selected NavMesh To Scene**. Save and reopen
the scene, then test gameplay. This uses the scene's native data reference,
without a loader GameObject. Keep the scene's existing OffMeshLink and
ActionPoint objects; the converted ground asset does not embed them.

Do not Bake or Clear navigation as part of attaching recovered data.

## Convert more files

From the repository root:

```powershell
python Tools/NavMeshRecovery/recover_navmesh.py --supplied
python Tools/NavMeshRecovery/recover_navmesh.py 'D:/OriginalProject/NavMesh.asset' --reference 'docs/source/reference/NavMesh(10).asset' --output-dir 'Assets/RecoveredOtherProject'
```

Inputs can be individual files or directories, searched recursively. A real,
populated **binary Unity 2017.4.40f1 NavMeshData** with an identity transform is
required as the serialization template. The supplied template is in the local
handoff's `docs/source/reference/` folder (the repository ignores `docs/`).
Supply `--reference` if using these tools elsewhere.

The converter reads each legacy container's origin and tile width, derives
tile/voxel size, preserves tile coordinates/layers, native height/radius/climb,
ground flags, areas, polygon neighbors, detail meshes and triangle records.
It translates vertices/bounds into the correct local grid and stores the
compensating NavMeshData position. Quantized BV bounds are conservatively
refitted around original coarse/detail vertices, preserving original topology.
No ground polygons are added, removed or rebaked.

Default agent type is 0; `--agent-type` can set a different existing target
agent type. Slope 45 and region area 2 are metadata defaults, since those
settings are not available in the native tile header. Rebuilding or baking
from those defaults is not part of recovery.

Existing outputs must be byte-identical to be reused; a different output is
refused. Use a new output directory when changing source files or converter
versions. Sidecars include source/output SHA-256 and the maximum vertex
world-coordinate roundoff (under 0.000008 units in the supplied batch).

## Native validation

Check exact binary/geometry preservation without launching Unity:

```powershell
python Tools/NavMeshRecovery/verify_recovery.py
```

```powershell
python Tools/NavMeshRecovery/validate_recovery.py --unity 'C:/Program Files/Unity/Hub/Editor/2017.4.40f1/Editor/Unity.exe'
```

Add `--assets 'Assets/RecoveredOtherProject'` for another output directory.
The script creates a marked, isolated project under `Temp/NavMeshRecoveryBatch`.
It imports the assets, attaches each to a new native scene, saves/reopens,
tests every detailed-surface triangle center, original adjacent ground
polygons, matched tile portals, and original attached baked link endpoints.
Gameplay scenes are never loaded or modified by this test.

`RecoveryBatchResults.json` records full results. Fifteen out of fifteen
supplied files pass: 37,242 detailed samples, 16,530 adjacency paths (including
246 portal paths), and 873 original attached baked-link routes. Source hashes,
polygon/detail records and all 35,108 stored vertices were independently
verified. The acceptance sample radius is 0.25 units; tighter
0.1-unit samples are recorded separately. Native CalculateTriangulation returns
coarse polygon triangles, so their Y coordinates need not lie on a detailed
terrain hill. Coarse samples are informational, not the only surface check.

The script also tests raw original link-transform positions before testing
baked snapped endpoints. Those results are separate: mesh recovery cannot
guarantee the correctness of every original gameplay link or animation.

## Original link limitations

`NavMesh(4)` contains two links with no attachments in the original runtime
graph and no nearby ground at their endpoints. The converter records them;
it does not invent terrain to connect them. Seven links across the supplied
files lack complete original graph attachments; some can reconnect in 2017.

`NavMesh(5)` has two links whose raw endpoints sit about 0.9 units above their
original snapped ground positions. Native ground and baked link connectivity
pass, but those raw scene transforms need a targeted integration adjustment.
Outside Play Mode, with the recovered data attached, select an affected scene
OffMeshLink and run **Tools > Navigation > Restore Selected Link Baked Endpoints**.
This command requires a unique coordinate match in the asset's sidecar and
empty endpoint helper objects, uses the original baked world positions, and
is undoable. It preserves the link component, ActionPoint, animation settings,
area, activation and direction. It never runs automatically.

## Supported scope

Validated inputs are **text-serialized Unity 4.5 NavMesh**, class 194,
**TESM container version 1 / VAND tile version 13**, with square grids and empty
heightmap lists. The target template uses serialized file version 17 and the
tested Unity 2017.4.40f1 NavMeshData layout. All supplied 42 tiles fit this scope.

This generalizes the recovery across the supplied files and should apply to
other projects with this same layout; it is not a guarantee for every Unity
4.5 project. Binary legacy assets, nonempty heightmaps, other tile layouts,
non-square grids, incompatible templates and unsupported structures are
rejected. Native validation and scene-level enemy/animation tests remain the
acceptance checks for new sources.
