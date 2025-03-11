using System.Collections.Generic;
using UnityEngine;

public class CitySiteManager
{
	private const int m_MaxOverallMissions = 5;

	private const int m_MaxStoryMissions = 3;

	private const int m_MaxGeneratedMissions = 4;

	public CitySiteIcon iconArena;

	private GameObject m_GameObject;

	private GameObject m_MissionIconPrefab;

	private GameObject m_StoryMissionIconPrefab;

	private GameObject m_ChopperMissionIconPrefab;

	private GameObject m_DailyRewardMissionIconPrefab;

	private GameObject m_SpecialIconPrefab;

	private GameObject m_MoneyIconPrefab;

	private CitySiteSlotsManager m_SlotManager;

	private MissionManager m_MissionManager;

	private List<CitySiteIcon> m_Spawned = new List<CitySiteIcon>();

	public IList<CitySiteIcon> SpawnedIcons
	{
		get
		{
			return m_Spawned.AsReadOnly();
		}
	}

	public void Init(GameObject gameObject, MissionManager manager)
	{
		m_GameObject = gameObject;
		m_MissionManager = manager;
		m_SlotManager = new CitySiteSlotsManager();
		m_SlotManager.Init(gameObject.transform);
		m_SpecialIconPrefab = Resources.Load("City/CitySpecialIcon") as GameObject;
		if (m_SpecialIconPrefab == null)
		{
			Debug.LogError("Can't find object City/CitySpecialIcon in Resources folder!");
		}
		m_MissionIconPrefab = Resources.Load("City/CityMissionIcon") as GameObject;
		if (m_MissionIconPrefab == null)
		{
			Debug.LogError("Can't find object City/CityMissionIcon in Resources folder!");
		}
		m_ChopperMissionIconPrefab = Resources.Load("City/CityChopperMissionIcon") as GameObject;
		if (m_ChopperMissionIconPrefab == null)
		{
			Debug.LogError("Can't find object City/CityChopperMissionIcon in Resources folder!");
		}
		m_DailyRewardMissionIconPrefab = Resources.Load("City/DailyRewardMissionIcon") as GameObject;
		if (m_DailyRewardMissionIconPrefab == null)
		{
			Debug.LogError("Can't find object City/DailyRewardMissionIcon in Resources folder!");
		}
		m_StoryMissionIconPrefab = Resources.Load("City/CityStoryMissionIcon") as GameObject;
		if (m_StoryMissionIconPrefab == null)
		{
			Debug.LogError("Can't find object City/CityStoryMissionIcon in Resources folder!");
		}
		m_MoneyIconPrefab = Resources.Load("City/CityBonusIcon") as GameObject;
		if (m_MoneyIconPrefab == null)
		{
			Debug.LogError("Can't find object City/CityBonusIcon in Resources folder!");
		}
	}

	public void SpawnSpecialSites()
	{
		GameObject gameObject = null;
		if (Game.Instance.PlayerPersistentInfo.totalMissionsPlayed >= 4 && iconArena == null)
		{
			CitySiteInfo citySiteInfo = new CityArenaInfo();
			citySiteInfo.slot = m_SlotManager.OccupySpecialSlot("ARENA");
			Vector3 pos = citySiteInfo.slot.GetPos();
			gameObject = Object.Instantiate(m_SpecialIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject;
			gameObject.transform.parent = m_GameObject.transform;
			iconArena = gameObject.GetComponent<CitySiteIcon>();
			iconArena.Init(citySiteInfo);
		}
	}

	public void SpawnAvailableMissions(int storyId, int playerXp)
	{
		for (bool flag = true; flag && IsPossibleSpawnStoryMission(); flag = SpawnMissionGenerate(storyId, playerXp, true))
		{
		}
		for (bool flag = true; flag && IsPossibleSpawnGeneratedMission(); flag = SpawnMissionGenerate(storyId, playerXp, false))
		{
		}
	}

	public void SpawnDailyRewardMission(int storyId, int playerXp)
	{
		if (Game.Instance.PlayerPersistentInfo.totalMissionsPlayed >= 3 && IsPossibleSpawnDailyRewardMission())
		{
			SpawnDailyRewardMissionInternal(storyId, playerXp);
		}
	}

	public CitySiteSlot GetSlotForHelicopter()
	{
		if (IsPossibleSpawnChopperMission())
		{
			return m_SlotManager.OccupyChopperSlot(string.Empty);
		}
		return null;
	}

	public void SpawnHelicopterMissions(int storyId, int playerXp, CitySiteSlot slot)
	{
		SpawnChopperMission(storyId, playerXp, slot);
	}

	public void SpawnMoney(int storyId)
	{
		CityMoneyInfo info = new CityMoneyInfo(new Funds(Funds.Type.Money, 100), 0.1f);
		SpawnMoneyInternal(info, storyId);
	}

	public void DestroySpawnedSite(CitySiteIcon icon)
	{
		m_Spawned.Remove(icon);
		if (icon.missionInfo != null)
		{
			m_MissionManager.MissionRemovedFromActiveList(icon.missionInfo);
		}
		m_SlotManager.FreeOccupiedSlot(icon.siteInfo.slot);
		icon.PrepareForDestroy();
		Object.Destroy(icon.gameObject);
	}

	public void Save(IDataFile file)
	{
		SaveInternal(file);
	}

	public void Load(IDataFile file, int storyId, int playerXP, int version)
	{
		LoadInternal(file, storyId, playerXP, version);
	}

	private void SaveInternal(IDataFile file)
	{
		int num = 1;
		num = 1;
		foreach (CitySiteIcon spawnedIcon in SpawnedIcons)
		{
			CityMissionInfo missionInfo = spawnedIcon.missionInfo;
			if (missionInfo != null)
			{
				string text = "Icon" + num + "_";
				file.SetInt(text, spawnedIcon.siteInfo.slot.m_UID);
				missionInfo.Save(file, text, m_MissionManager);
				num++;
			}
		}
		if (iconArena != null && iconArena.siteInfo is CityArenaInfo)
		{
			(iconArena.siteInfo as CityArenaInfo).Save(file, "Arena");
		}
	}

	private void LoadInternal(IDataFile file, int storyId, int playerXP, int version)
	{
		List<CitySiteIcon> list = new List<CitySiteIcon>();
		foreach (CitySiteIcon spawnedIcon in SpawnedIcons)
		{
			CityMissionInfo missionInfo = spawnedIcon.missionInfo;
			if (missionInfo != null)
			{
				list.Add(spawnedIcon);
			}
		}
		foreach (CitySiteIcon item in list)
		{
			DestroySpawnedSite(item);
		}
		int num = 1;
		while (true)
		{
			string text = "Icon" + num + "_";
			if (!file.KeyExists(text))
			{
				break;
			}
			int @int = file.GetInt(text);
			CityMissionInfo cityMissionInfo = new CityMissionInfo();
			cityMissionInfo.Load(file, text, m_MissionManager);
			SpawnMission(cityMissionInfo, storyId, playerXP, @int);
			num++;
		}
		if (iconArena != null && iconArena.siteInfo is CityArenaInfo)
		{
			(iconArena.siteInfo as CityArenaInfo).Load(file, "Arena", version);
		}
	}

	private bool IsPossibleSpawnStoryMission()
	{
		if (m_Spawned.Count >= 5)
		{
			return false;
		}
		int num = 0;
		foreach (CitySiteIcon item in m_Spawned)
		{
			if (item.missionInfo != null && item.missionInfo.story == MissionFlowData.StoryBound.Story)
			{
				num++;
			}
		}
		if (num >= 3)
		{
			return false;
		}
		return true;
	}

	private bool IsPossibleSpawnGeneratedMission()
	{
		if (m_Spawned.Count >= 5)
		{
			return false;
		}
		int num = 0;
		foreach (CitySiteIcon item in m_Spawned)
		{
			if (item.missionInfo != null && item.missionInfo.story != 0)
			{
				num++;
			}
		}
		if (num >= 4)
		{
			return false;
		}
		return true;
	}

	private bool IsPossibleSpawnChopperMission()
	{
		foreach (CitySiteIcon item in m_Spawned)
		{
			if (item.siteInfo.slot.m_Type == CitySiteSlot.Type.ChopperMission)
			{
				return false;
			}
		}
		return true;
	}

	private bool IsPossibleSpawnDailyRewardMission()
	{
		foreach (CitySiteIcon item in m_Spawned)
		{
			if (item.missionInfo != null && item.missionInfo.story == MissionFlowData.StoryBound.DailyReward)
			{
				return false;
			}
		}
		return true;
	}

	private MissionFlowData.Difficulty ComputeDifficulty()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (CitySiteIcon item in m_Spawned)
		{
			if (item.missionInfo.story != 0)
			{
				switch (item.missionInfo.difficulty)
				{
				case MissionFlowData.Difficulty.Easy:
					num++;
					break;
				case MissionFlowData.Difficulty.Normal:
					num2++;
					break;
				case MissionFlowData.Difficulty.Hard:
					num3++;
					break;
				}
			}
		}
		if (num2 > 0 && num3 == 0)
		{
			return MissionFlowData.Difficulty.Hard;
		}
		if (num2 > 0 && num == 0)
		{
			return MissionFlowData.Difficulty.Easy;
		}
		return MissionFlowData.Difficulty.Normal;
	}

	private bool SpawnMission(CityMissionInfo info, int storyId, int playerXP, int forcedSlotUid = -1)
	{
		bool flag = info.story == MissionFlowData.StoryBound.ChopperMission;
		bool flag2 = info.story == MissionFlowData.StoryBound.DailyReward;
		PlayerLevelData.PlayerLevelInfo infoByPlayerXp = GameplayData.Instance.playerLevelData.GetInfoByPlayerXp(playerXP);
		info.AssignPlayerLevelInfo(infoByPlayerXp);
		if (forcedSlotUid < 0)
		{
			if (flag)
			{
				info.slot = m_SlotManager.OccupyChopperSlot(string.Empty);
			}
			else
			{
				info.slot = m_SlotManager.OccupySiteSlot(info.slotSuggestion, storyId);
			}
		}
		else
		{
			info.slot = m_SlotManager.ForceOccupySiteSlot(forcedSlotUid);
		}
		if (info.slot != null)
		{
			Vector3 pos = info.slot.GetPos();
			GameObject gameObject = null;
			gameObject = (flag ? (Object.Instantiate(m_ChopperMissionIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject) : (flag2 ? (Object.Instantiate(m_DailyRewardMissionIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject) : ((info.story != 0) ? (Object.Instantiate(m_MissionIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject) : (Object.Instantiate(m_StoryMissionIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject))));
			gameObject.transform.parent = m_GameObject.transform;
			CitySiteIcon component = gameObject.GetComponent<CitySiteIcon>();
			component.Init(info);
			m_Spawned.Add(component);
			info.MarkAsUsed();
			return true;
		}
		return false;
	}

	private bool SpawnChopperMission(int storyId, int playerXP, CitySiteSlot slot)
	{
		MissionManager.MissionData missionData = m_MissionManager.GenerateChopperMission(storyId, false, ComputeDifficulty());
		if (missionData != null)
		{
			CityMissionInfo info = new CityMissionInfo(missionData);
			return SpawnMission(info, storyId, playerXP, slot.m_UID);
		}
		return false;
	}

	private bool SpawnDailyRewardMissionInternal(int storyId, int playerXP)
	{
		MissionManager.MissionData missionData = m_MissionManager.GenerateDailyRewardMission(storyId, false, MissionFlowData.Difficulty.Easy);
		if (missionData != null)
		{
			CityMissionInfo info = new CityMissionInfo(missionData);
			return SpawnMission(info, storyId, playerXP);
		}
		return false;
	}

	private bool SpawnMissionGenerate(int storyId, int playerXP, bool story)
	{
		MissionManager.MissionData missionData = m_MissionManager.GenerateMission(storyId, story, story ? MissionFlowData.Difficulty.Normal : ComputeDifficulty());
		if (missionData != null)
		{
			CityMissionInfo info = new CityMissionInfo(missionData);
			return SpawnMission(info, storyId, playerXP);
		}
		return false;
	}

	private bool SpawnMoneyInternal(CityMoneyInfo info, int storyId, int forcedSlotUid = -1)
	{
		if (forcedSlotUid < 0)
		{
			info.slot = m_SlotManager.OccupyMoneySlot(string.Empty);
		}
		else
		{
			info.slot = m_SlotManager.ForceOccupySiteSlot(forcedSlotUid);
		}
		if (info.slot != null)
		{
			Vector3 pos = info.slot.GetPos();
			GameObject gameObject = null;
			gameObject = Object.Instantiate(m_MoneyIconPrefab, pos, Quaternion.Euler(270f, 0f, 0f)) as GameObject;
			gameObject.transform.parent = m_GameObject.transform;
			CitySiteIcon component = gameObject.GetComponent<CitySiteIcon>();
			component.Init(info);
			m_Spawned.Add(component);
			return true;
		}
		return false;
	}
}
