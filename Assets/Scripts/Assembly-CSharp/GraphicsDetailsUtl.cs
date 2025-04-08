using UnityEngine;

public class GraphicsDetailsUtl
{
	public enum Quality
	{
		Low = 0,
		Medium = 1,
		High = 2,
		VeryHigh = 3
	}

	public static void AutoSetupShaderQuality()
    {
        SetShaderQuality(Quality.High);
    }

	public static void SetShaderQuality(Quality quality)
	{
		DisableShaderKeyword("UNITY_SHADER_DETAIL_LOW");
		DisableShaderKeyword("UNITY_SHADER_DETAIL_MEDIUM");
		DisableShaderKeyword("UNITY_SHADER_DETAIL_HIGH");
		DisableShaderKeyword("UNITY_SHADER_DETAIL_VERY_HIGH");
		switch (quality)
		{
		case Quality.Low:
			EnableShaderKeyword("UNITY_SHADER_DETAIL_LOW");
			break;
		case Quality.Medium:
			EnableShaderKeyword("UNITY_SHADER_DETAIL_MEDIUM");
			break;
		case Quality.High:
			EnableShaderKeyword("UNITY_SHADER_DETAIL_HIGH");
			break;
		case Quality.VeryHigh:
			EnableShaderKeyword("UNITY_SHADER_DETAIL_VERY_HIGH");
			break;
		}
	}

	public static void EnableShaderKeyword(string keyword)
	{
		Debug.Log("EnableShaderKeyword: " + keyword);
		Shader.EnableKeyword(keyword);
	}

	public static void DisableShaderKeyword(string keyword)
	{
		Shader.DisableKeyword(keyword);
	}
}
