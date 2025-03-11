using UnityEngine;

public class UpgradeSettingsManager : SettingsManager<UpgradeSettings, E_UpgradeID>
{
	private static UpgradeSettingsManager s_Instance;

	public static UpgradeSettingsManager Instance
	{
		get
		{
			return GetInstance();
		}
	}

	private static UpgradeSettingsManager GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = ScriptableObject.CreateInstance<UpgradeSettingsManager>();
			s_Instance.Init("_Settings/_UpgradesSettings");
			Object.DontDestroyOnLoad(s_Instance);
		}
		return s_Instance;
	}
}
