using System;
using UnityEngine;

public class FPVShaderUtils
{
	public static void SetFPVProjectionParams(float mainCamFOVRad, float FPVCamFOVRad, float aspectRatio, float depthOffs)
	{
		float num = 1f / Mathf.Tan(mainCamFOVRad * ((float)Math.PI / 180f) * 0.5f);
		float num2 = 1f / Mathf.Tan(FPVCamFOVRad * ((float)Math.PI / 180f) * 0.5f);
		Vector4 vec = default(Vector4);
		vec.y = (vec.x = num2 / num);
		vec.z = 0.1f;
		vec.w = depthOffs;
		Shader.SetGlobalVector("_ProjParams", vec);
	}
}
