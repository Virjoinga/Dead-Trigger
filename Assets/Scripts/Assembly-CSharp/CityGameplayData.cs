using UnityEngine;

public class CityGameplayData
{
	private static CityGameplayData m_Instance;

	private MissionFlowData m_MissionFlowData;

	private MissionGraphicData m_MissionGraphicData;

	private StoryFlowData m_StoryFlowData;

	public static CityGameplayData Instance
	{
		get
		{
			if (m_Instance == null)
			{
				CreateInstance();
			}
			return m_Instance;
		}
	}

	public MissionFlowData missionFlowData
	{
		get
		{
			return m_MissionFlowData;
		}
	}

	public MissionGraphicData missionGraphicData
	{
		get
		{
			return m_MissionGraphicData;
		}
	}

	public StoryFlowData storyFlowData
	{
		get
		{
			return m_StoryFlowData;
		}
	}

	private static void CreateInstance()
	{
		m_Instance = new CityGameplayData();
		m_Instance.Init();
	}

	private void Init()
	{
		GameObject gameObject = Resources.Load("City/CityGameplayData") as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Can't find object City/CityGameplayData in Resources folder!");
			return;
		}
		m_MissionFlowData = gameObject.GetComponent<MissionFlowData>();
		if (m_MissionFlowData == null)
		{
			Debug.LogError("Can't find component MissionFlowData in prefab City/CityGameplayData in Resources folder!");
		}
		m_MissionGraphicData = gameObject.GetComponent<MissionGraphicData>();
		if (m_MissionGraphicData == null)
		{
			Debug.LogError("Can't find component MissionGraphicData in prefab City/CityGameplayDataa in Resources folder!");
		}
		m_StoryFlowData = gameObject.GetComponent<StoryFlowData>();
		if (m_StoryFlowData == null)
		{
			Debug.LogError("Can't find component StoryFlowData in prefab City/CityGameplayData in Resources folder!");
		}
	}
}
