using UnityEngine;

public class FundSettingsManager : SettingsManager<FundSettings, E_FundID>
{
	private static FundSettingsManager s_Instance;

	public static FundSettingsManager Instance
	{
		get
		{
			return GetInstance();
		}
	}

	private static FundSettingsManager GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = ScriptableObject.CreateInstance<FundSettingsManager>();
			s_Instance.Init("_Settings/_FundSettings");
			Object.DontDestroyOnLoad(s_Instance);
		}
		return s_Instance;
	}
}
