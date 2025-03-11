using UnityEngine;

public class ItemSettingsManager : SettingsManager<ItemSettings, E_ItemID>
{
	private static ItemSettingsManager s_Instance;

	public static ItemSettingsManager Instance
	{
		get
		{
			return GetInstance();
		}
	}

	private static ItemSettingsManager GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = ScriptableObject.CreateInstance<ItemSettingsManager>();
			s_Instance.Init("_Settings/_ItemSettings");
			Object.DontDestroyOnLoad(s_Instance);
		}
		return s_Instance;
	}
}
