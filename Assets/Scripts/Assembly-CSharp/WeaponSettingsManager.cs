using UnityEngine;

public class WeaponSettingsManager : SettingsManager<WeaponSettings, E_WeaponID>
{
	private static WeaponSettingsManager s_Instance;

	public static WeaponSettingsManager Instance
	{
		get
		{
			return GetInstance();
		}
	}

	private static WeaponSettingsManager GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = ScriptableObject.CreateInstance<WeaponSettingsManager>();
			s_Instance.Init("_Settings/_WeaponSettings");
			Object.DontDestroyOnLoad(s_Instance);
		}
		return s_Instance;
	}
}
