# Supplied batch validation — 2026-10-21

Target: Unity 2017.4.40f1. Native scene data is saved and reloaded for every
case; no runtime loader object is used. All 15 supplied files pass ground
surface, adjacency, portal, and original attached baked-link route checks.

| Original filename | Tiles | Coarse triangles | Detail samples | Adjacent paths | Attached link routes |
| --- | ---: | ---: | ---: | ---: | ---: |
| NavMesh.asset | 4 | 1288 | 2710 | 1308 | 106 |
| NavMesh(1).asset | 1 | 890 | 1229 | 904 | 76 |
| NavMesh(2).asset | 5 | 1649 | 1964 | 1898 | 62 |
| NavMesh(20261021-054020).asset | 1 | 1614 | 7255 | 1810 | 34 |
| NavMesh(20261021-054020-1).asset | 1 | 942 | 947 | 1032 | 66 |
| NavMesh(20261021-054020-2).asset | 4 | 770 | 842 | 800 | 118 |
| NavMesh(20261021-054020-3).asset | 4 | 1288 | 2710 | 1308 | 106 |
| NavMesh(20261021-054024).asset | 1 | 1642 | 7345 | 1834 | 32 |
| NavMesh(20261021-054026).asset | 1 | 161 | 171 | 162 | 18 |
| NavMesh(3).asset | 2 | 618 | 1425 | 696 | 44 |
| NavMesh(4).asset | 3 | 663 | 791 | 1058 | 22 |
| NavMesh(5).asset | 4 | 1227 | 4145 | 1296 | 37 |
| NavMesh(6).asset | 8 | 1057 | 3157 | 1120 | 36 |
| NavMesh(7).asset | 1 | 675 | 1914 | 722 | 40 |
| NavMesh(8).asset | 2 | 556 | 637 | 582 | 76 |

Full machine-readable evidence is in `RecoveryBatchResults.json`. Original
uploaded filenames are retained; no level-name mapping is assumed.

Acceptance uses 0.25-unit samples at every original detail-triangle center,
not only NavMesh.CalculateTriangulation's coarse polygon triangles. Six
original detail centers have 0.108–0.132-unit differences from Unity's returned
surface and therefore miss the additional, stricter 0.1-unit check; all pass
the stated acceptance radius. Seven coarse centers in NavMesh(5) lie more than
two units below the detailed hill. Their coarse samples are informational.
All original detail coordinates and records remain preserved.

The BV topology and polygon indices are retained while bounds are expanded
conservatively to include the actual detailed surface. This matters for
uneven terrain; omitting the BV tree can omit candidates above coarse bounds.

Binary checks independently verified unchanged source hashes, ground polygon
records (excluding the removed firstLink field), detail records and triangle
indices. All 35,108 original vertices, including detail vertices, remain at
the same world positions with maximum roundoff 0.0000076294 units.

## Link distinctions

The legacy runtime graph has seven incompletely attached links across the
files. Two in NavMesh(4) have no nearby original ground and remain unavailable.
This is recorded instead of generating extra geometry. Other legacy-orphaned
links can find routes in Unity 2017 and are reported separately.

Before endpoint normalization in the isolated harness, 878/884 raw endpoint
routes complete. The six unavailable routes comprise NavMesh(4)'s two orphaned
one-way links and the forward/reverse routes of two NavMesh(5) links. The latter
store raw endpoints about 0.9 units above their snapped baked endpoints.

All 873 routes for fully attached source links complete using the original
baked endpoint positions. The targeted editor repair command was exercised on
the two affected NavMesh(5) links in the harness, followed by save/reload.
No original gameplay scene, animation, or endpoint was modified by batch tests.
Actual scene-level enemy animations require normal gameplay testing.

## Portability

The converter accepts the tested TESM1/VAND13 text-serialized layout, square
grids, empty heightmaps, and a matching binary Unity 2017.4.40f1 template. These
results do not establish support for all Unity 4.5 binary/heightmap variations.
No original source is replaced, and different existing outputs are refused.
