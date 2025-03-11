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

	public static bool IsTegra3()
	{
		string graphicsDeviceName = SystemInfo.graphicsDeviceName;
		string graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
		if (SystemInfo.processorCount >= 4)
		{
			string text = graphicsDeviceVendor.ToUpper();
			if (text.IndexOf("NVIDIA") != -1)
			{
				string text2 = graphicsDeviceName.ToUpper();
				if (text2.IndexOf("TEGRA") != -1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static void AutoSetupShaderQuality()
	{
		Debug.Log("IsTegra3 : " + IsTegra3());
		if (IsTegra3())
		{
			SetShaderQuality(Quality.High);
		}
		else
		{
			SetShaderQuality(Quality.Low);
		}
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
