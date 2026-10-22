# Zombie shader translation fix

Production shader: `Assets/Shader/Mobile-TexPseudoBRDF-WrapAround-customgloss-zombie.shader`.

The supplied original Unity 4.x GLES program was read from the untouched Dead Trigger 1.9.5 export used for LightProbe recovery. Two translation/platform issues explain directional lighting and highlights pointing the wrong way:

1. GLSL's `matrix[index]` indexes a column, while Cg/HLSL indexes a row. The original explicitly built an object-to-tangent matrix whose rows are tangent, bitangent and normal. Copying the element assignments literally into Cg transposed that matrix. The corrected Cg matrix assigns those three vectors to rows, so multiplying a direction computes the required three dot products. Both the dominant probe-light direction and view/half-vector calculation use this matrix.
2. The GLES shader decoded ordinary RGB normal maps. The current zombie textures are imported as Unity Normal Maps (`textureType: 1`), allowing desktop normal-map packing. Raw `.xyz * 2 - 1` cannot decode packed desktop textures correctly. The shader now calls Unity 2017's `UnpackNormal`, which handles the platform encoding.

Existing probe SH evaluation, gloss weights, BRDF lookup, material properties, projection and wound controls are retained. The fix changes the shared shader; existing materials need no reassignment. The supplied zombie normal textures were not reimported or modified in the game project.

## Validation

Run from the project root:

```powershell
python Tools/ShaderRecovery/validate_zombie_shader.py
```

The runner creates a marked isolated project under `Temp/ZombieShaderHarness` and uses Unity 2017.4.40f1 with Direct3D 11 and a real graphics device. It does not open or save a game scene. A graphics-capable Windows session and the Unity installation at the script's configured path are required.

The fixture copies the actual production shader and changes only its shader name and SH uniform names so automatic scene ambient updates cannot override controlled inputs. An independently written reference expresses the original GLSL TBN multiplication as explicit dot products and uses Unity's normal decoder for the desktop import format. The pre-fix baseline is reconstructed by reversing the two targeted changes. The unmodified production shader is imported separately and its pass is bound as a support check.

The 18 cases cover three rotations, two tangent handedness values, and three normal textures: neutral, tilted, and the actual `zombie01_n.png`. All textures use Unity's native compressed normal-map import. The comparison includes nonconstant SH terms, camera half-vectors, a BRDF lookup and gloss contribution. It compares 4,608 central render pixels against the reference, rejecting empty reference renders.

`ZombieShaderValidation.json` records the graphics device, error bounds, production shader hash and result. The corrected shader has zero pixel error in the measured cases; the baseline differs beyond the 0.02 threshold in all 18 cases, with maximum channel error about 0.529. Logs remain in the harness.

This validates the translation and normal-map handling under controlled rendering on Direct3D 11. It does not constitute an in-game screenshot comparison or cover other character shaders, all graphics APIs, skinning variants, or the shader's wound/FPV projection branches.
