# LightProbe recovery restart

Target: Unity 2017.4.40f1. Source: untouched Unity 4.x class-197 LightProbes from the export supplied on 22 October 2026:

`C:\Users\Ethan\Documents\GitHub\ULegacy-Ripper\Source\0Bins\AssetRipper\Debug\Export\Dead Trigger\1.9.5\AssetRipperExport639276928146067146\Assets`

The new assets are under `Assets/RecoveredLightProbes/<level>/LightProbes.asset`. Original project probe assets, lighting assets, scenes and shaders were not replaced by this recovery. This handles the supplied export's layout; other Unity versions and exporters require their own format checks.

## Try a scene

1. Open one matching, saved level scene, for example `Assets/Levels/Test/mall.unity`, outside Play mode.
2. Select `Assets/RecoveredLightProbes/mall/LightProbes.asset` in the Project window.
3. Run **Tools > Light Probe Recovery > Attach Selected Probes to Scene**.
4. Save and reopen the scene, then test in Play mode.

The command creates a separate `LightingData_Recovered.asset` beside the recovered probes and assigns it through `Lightmapping.lightingDataAsset`. It removes obsolete `RestoreLightProbes` components from that scene with Undo support, because that script would transpose the corrected coefficients again. No runtime recovery component is required. Do not add the old restore script to recovered scenes.

The command copies the scene's existing text lighting asset, changing only its probe and scene references. When the original lighting asset is binary, it uses the corresponding text snapshot under `LightingTemplates`. These snapshots preserve this project's existing lighting data as captured during recovery. A different binary lighting asset is rejected so later edits are not discarded. Only the matching original scene GUID is accepted; duplicate scenes need an explicitly adapted lighting template.

Unity 2017 refreshes native probes on scene reload; the immediate `LightmapSettings.lightProbes` value is not a reliable attachment check. Scene persistence is tested after saving, opening an empty scene and reopening the test scene.

## What was wrong

The old conversion generated coefficient field names with an extra space for two-digit indices, such as `sh[ 10]`. Unity's canonical spelling is `sh[10]`; it silently ignored those fields. All 14 current project probe assets consequently have zeros in slots 10–26. The original supplied export retains all 27 values.

Reordering alone is also insufficient. The legacy values are RGB-interleaved real SH coefficients. The native Unity 2017 representation is channel-major and incorporates basis and cosine-convolution factors. The converter uses:

```
new[channel * 9 + k] = float32(old[k * 3 + channel] * factor[k])
factor = [
  0.28209479177387814,
 -0.32573500793527993, 0.32573500793527993, -0.32573500793527993,
  0.2731371076480198, -0.2731371076480198, 0.07884789131313001,
 -0.2731371076480198, 0.1365685538240099
]
```

These factors were independently calibrated against Unity 2017's native `SphericalHarmonicsL2.AddAmbientLight` and `AddDirectionalLight` methods, including axis and mixed directions. Reference layouts: [legacy LightProbes coefficients](https://docs.unity3d.com/jp/460/ScriptReference/LightProbes-coefficients.html) and [Unity 2017 SH indexer](https://docs.unity3d.com/2017.4/Documentation/ScriptReference/Rendering.SphericalHarmonicsL2.Index_operator.html).

The original serialized interior inverse matrices also contained rounding inconsistencies with the stored probe positions. Keeping them produced six probe-position query failures. Interior inverses are now recalculated from the original float32 positions, using double precision before storing float32 results. Probe positions, tetrahedron indices, neighbors, hull rays and exterior matrices are retained. No lighting bake or new tetrahedralization is performed. `--keep-legacy-matrices` exists only to reproduce that diagnostic comparison.

## Reproduce

Run from the game project root:

```powershell
python Tools/LightProbeRecovery/recover_lightprobes.py "<untouched export Assets directory>"
python Tools/LightProbeRecovery/validate_lightprobes.py
```

The converter refuses to overwrite different existing outputs. Each `.recovery.json` records the original SHA-256, generated asset SHA-256, original positions and coefficients, counts and conversion factors. `outputSHA256` describes the generated text before Unity imports it; Unity may subsequently normalize that text, including the asset name. `ValidatedAssetHashes.json` records the actual files submitted to native validation after import. Its unverified status is deliberate: generating an asset alone does not validate it. Native results are recorded separately below. Preserve asset GUIDs if recreating files already referenced by a scene. To reproduce generation after Unity has rewritten outputs, use `--output` with a fresh directory instead of replacing the imported assets.

The validation runner builds a marked, isolated project under `Temp/LightProbeRecoveryHarness`; it never opens the game project in batch mode. It requires the installed Unity 2017.4.40f1 editor; `--unity` accepts a different executable location. Full logs remain in the harness.

## Native validation

`LightProbeRecoveryResults.json` contains all 14 native results; `ValidatedAssetHashes.json` identifies the tested assets. Checks cover:

- 6,403 original positions with zero stored position error.
- 172,881 stored coefficients with zero error against the independently implemented conversion.
- 6,403/6,403 probe-position interpolation queries, normalized coefficient error below 0.0001.
- 25,664/25,664 nondegenerate interior centroid interpolation queries, normalized coefficient error below 0.001. Of these, 25,653 pass the tighter 0.0001 bound; the largest error is approximately 0.000544 in a thin cell.
- Native ambient and directional basis calibration.

Normalized error is `abs(actual - expected) / (1 + abs(expected))`. The centroid test compares against the average SH of the four vertices; this is a numerical interpolation check, not a visual similarity metric. Exterior-space rendering and every possible sample point are not exhaustively tested.

`NativeProbeSceneResult.json` records the mall scene attachment test. It calls the actual editor helper, saves/reopens a minimal scene and checks all 647 stored probes plus an interpolated sample. The test substitutes only the template's scene GUID for its isolated scene; it does not load or save a game scene.

The lighting template still produces Unity's existing incompatible realtime GI/Enlighten warning. Baked probes load and interpolate despite that warning. This recovery does not repair realtime GI, and Generate Lighting would replace the recovered baked results. Visual comparison of characters in the actual game scenes remains to be done; native coefficient and scene-persistence checks alone cannot prove final shader appearance.
