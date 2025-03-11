using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager
{
	public class MissionData
	{
		public MissionFlowData.Mission mission;

		public string level;

		public string levelPreview;

		public MissionTexts texts;

		public MissionFlowData.Difficulty difficulty;
	}

	private class MissionTypeHistory
	{
		public E_MissionType m_MissionType;

		public int m_ActuallyOnMap = 1;
	}

	private const int RandomIterations = 10;

	private const int MaxMissionTypeHistorySize = 6;

	private const int MaxLevelHistorySize = 5;

	private const int MaxTextHistorySize = 6;

	private LinkedList<MissionTypeHistory> m_MissionTypeHistory = new LinkedList<MissionTypeHistory>();

	private LinkedList<int> m_TextHistory = new LinkedList<int>();

	private LinkedList<string> m_LevelHistory = new LinkedList<string>();

	private List<MissionFlowData.Mission> availableMissions = new List<MissionFlowData.Mission>();

	private List<MissionTexts>[] m_GeneratedTexts = new List<MissionTexts>[4];

	private List<MissionTexts>[] m_GeneratedChopperTexts = new List<MissionTexts>[4];

	private System.Random m_RandomText = new System.Random((int)DateTime.Now.Ticks);

	private StoryFlowData m_StoryData;

	private MissionFlowData m_MissionData;

	private MissionGraphicData m_LevelData;

	public void Init(GameObject gameObject)
	{
		InitInternal(gameObject);
	}

	public int GetNumOfRequiredMissions(int storyId)
	{
		return m_StoryData.GetStory(storyId).missionsRequired;
	}

	public IList<MissionTexts> GetGeneratedTexts(E_MissionType missionType, bool chopperMission)
	{
		if (chopperMission)
		{
			return m_GeneratedChopperTexts[(int)missionType].AsReadOnly();
		}
		return m_GeneratedTexts[(int)missionType].AsReadOnly();
	}

	public StoryFlowData.Story GetStoryChapterInfo(int storyId)
	{
		return m_StoryData.GetStory(storyId);
	}

	public MissionData GenerateMission(int storyId, bool story, MissionFlowData.Difficulty nonstoryDifficulty)
	{
		return GenerateMissionInternal(storyId, story, MissionFlowData.StoryBound.None, nonstoryDifficulty);
	}

	public MissionData GenerateChopperMission(int storyId, bool story, MissionFlowData.Difficulty nonstoryDifficulty)
	{
		return GenerateMissionInternal(storyId, story, MissionFlowData.StoryBound.ChopperMission, nonstoryDifficulty);
	}

	public MissionData GenerateDailyRewardMission(int storyId, bool story, MissionFlowData.Difficulty nonstoryDifficulty)
	{
		return GenerateMissionInternal(storyId, story, MissionFlowData.StoryBound.DailyReward, nonstoryDifficulty);
	}

	public void SaveMissionData(MissionData data, IDataFile file, string keyPrefix)
	{
		file.SetInt(keyPrefix + "MissionUID", data.mission.uid);
		file.SetString(keyPrefix + "Level", data.level);
		file.SetString(keyPrefix + "LevelPreview", data.levelPreview);
		file.SetInt(keyPrefix + "Difficulty", (int)data.difficulty);
		data.texts.Save(file, keyPrefix);
	}

	public void LoadMissionData(MissionData data, IDataFile file, string keyPrefix)
	{
		data.mission = m_MissionData.GetMission(file.GetInt(keyPrefix + "MissionUID"));
		data.level = file.GetString(keyPrefix + "Level", string.Empty);
		data.levelPreview = file.GetString(keyPrefix + "LevelPreview", string.Empty);
		data.texts = MissionTexts.CreateNew();
		data.difficulty = (MissionFlowData.Difficulty)file.GetInt(keyPrefix + "Difficulty", 1);
		data.texts.Load(file, keyPrefix);
	}

	public void Save(IDataFile file)
	{
		int num = 1;
		foreach (MissionTypeHistory item in m_MissionTypeHistory)
		{
			string key = "MissionTypeHistory_Type" + num;
			file.SetInt(key, (int)item.m_MissionType);
			key = "MissionTypeHistory_OnMap" + num;
			file.SetInt(key, item.m_ActuallyOnMap);
			num++;
		}
		num = 1;
		foreach (int item2 in m_TextHistory)
		{
			string key = "MissionTextHistory" + num;
			file.SetInt(key, item2);
			num++;
		}
		num = 1;
		foreach (string item3 in m_LevelHistory)
		{
			string key = "MissionLevelHistory" + num;
			file.SetString(key, item3);
			num++;
		}
		m_MissionData.Save(file);
	}

	public void Load(IDataFile file)
	{
		m_MissionTypeHistory.Clear();
		m_TextHistory.Clear();
		m_LevelHistory.Clear();
		int num = 1;
		while (true)
		{
			string key = "MissionTypeHistory_Type" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			MissionTypeHistory missionTypeHistory = new MissionTypeHistory();
			missionTypeHistory.m_MissionType = (E_MissionType)file.GetInt(key);
			key = "MissionTypeHistory_OnMap" + num;
			missionTypeHistory.m_ActuallyOnMap = file.GetInt(key);
			m_MissionTypeHistory.AddLast(missionTypeHistory);
			num++;
		}
		num = 1;
		while (true)
		{
			string key = "MissionTextHistory" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			m_TextHistory.AddLast(file.GetInt(key));
			num++;
		}
		num = 1;
		while (true)
		{
			string key = "MissionLevelHistory" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			m_LevelHistory.AddLast(file.GetString(key, string.Empty));
			num++;
		}
		m_MissionData.Load(file);
	}

	public void InitInternal(GameObject gameObject)
	{
		m_StoryData = CityGameplayData.Instance.storyFlowData;
		m_MissionData = CityGameplayData.Instance.missionFlowData;
		m_LevelData = CityGameplayData.Instance.missionGraphicData;
		if (availableMissions.Capacity < m_MissionData.data.Count)
		{
			availableMissions.Capacity = m_MissionData.data.Count;
		}
		for (int i = 0; i < 4; i++)
		{
			m_GeneratedTexts[i] = new List<MissionTexts>();
			for (int j = 1050000 + (i + 1) * 1000; TextDatabase.instance.Exists(j); j += 10)
			{
				MissionTexts missionTexts = MissionTexts.CreateNew();
				missionTexts.caption = j;
				missionTexts.objective = j + 2;
				missionTexts.description = j + 4;
				missionTexts.success = j + 6;
				missionTexts.fail = j + 8;
				m_GeneratedTexts[i].Add(missionTexts);
			}
			m_GeneratedChopperTexts[i] = new List<MissionTexts>();
			for (int j = 1060000 + (i + 1) * 1000; TextDatabase.instance.Exists(j); j += 10)
			{
				MissionTexts missionTexts2 = MissionTexts.CreateNew();
				missionTexts2.caption = j;
				missionTexts2.objective = j + 2;
				missionTexts2.description = j + 4;
				missionTexts2.success = j + 6;
				missionTexts2.fail = j + 8;
				m_GeneratedChopperTexts[i].Add(missionTexts2);
			}
		}
	}

	public void MissionRemovedFromActiveList(CityMissionInfo info)
	{
		if (info == null)
		{
			return;
		}
		MissionTypeHistory missionTypeHistory = null;
		foreach (MissionTypeHistory item in m_MissionTypeHistory)
		{
			if (item.m_MissionType == info.missionType && item.m_ActuallyOnMap > 0)
			{
				item.m_ActuallyOnMap = 0;
				missionTypeHistory = item;
				break;
			}
		}
		if (missionTypeHistory != null)
		{
			m_MissionTypeHistory.Remove(missionTypeHistory);
			m_MissionTypeHistory.AddFirst(missionTypeHistory);
		}
	}

	private MissionData GenerateMissionInternal(int storyId, bool story, MissionFlowData.StoryBound specFilter, MissionFlowData.Difficulty nonstoryDifficulty)
	{
		availableMissions.Clear();
		StoryFlowData.Story storyChapterInfo = GetStoryChapterInfo(Game.Instance.PlayerPersistentInfo.storyId);
		foreach (MissionFlowData.Mission datum in m_MissionData.data)
		{
			if (specFilter != MissionFlowData.StoryBound.None)
			{
				if (datum.storyBound == specFilter && m_LevelData.AnyMatchingLevelAvailable(datum.levels, storyId))
				{
					availableMissions.Add(datum);
				}
			}
			else if (datum.storyBound != MissionFlowData.StoryBound.ChopperMission && datum.storyBound != MissionFlowData.StoryBound.DailyReward && datum.storyId == storyId && ((story && datum.storyBound == MissionFlowData.StoryBound.Story && (!datum.unique || datum.used == 0)) || (!story && datum.storyBound != 0)) && (datum.storyBound != MissionFlowData.StoryBound.Dependent || (Game.Instance.PlayerPersistentInfo.chapterProgress >= storyChapterInfo.missionsRequired && storyChapterInfo.rankToProgress != 0)) && m_LevelData.AnyMatchingLevelAvailable(datum.levels, storyId))
			{
				availableMissions.Add(datum);
			}
		}
		if (availableMissions.Count == 0)
		{
			return null;
		}
		MissionData missionData = new MissionData();
		int num = 0;
		int num2 = 1000;
		int index = 0;
		for (int i = 0; i < availableMissions.Count; i++)
		{
			num = i;
			int num3 = MissionTypeInHistoryValue(availableMissions[num].missionType);
			if (num3 == 0 || num2 > num3)
			{
				num2 = num3;
				index = num;
			}
			if (num3 == 0)
			{
				break;
			}
		}
		missionData.mission = availableMissions[index];
		missionData.level = string.Empty;
		missionData.texts = null;
		num2 = 100;
		MissionGraphicData.ShortInfo shortInfo = null;
		for (int j = 0; j < 10; j++)
		{
			MissionGraphicData.ShortInfo randomMatchingLevel = m_LevelData.GetRandomMatchingLevel(missionData.mission.levels, storyId);
			int num4 = LevelInHistoryValue(randomMatchingLevel.level);
			if (num4 == 0 || num2 > num4)
			{
				num2 = num4;
				shortInfo = randomMatchingLevel;
			}
			if (num4 == 0)
			{
				break;
			}
		}
		missionData.level = shortInfo.level;
		missionData.levelPreview = shortInfo.preview;
		IList<MissionTexts> list = missionData.mission.texts;
		if (list.Count == 0)
		{
			list = GetGeneratedTexts(missionData.mission.missionType, specFilter == MissionFlowData.StoryBound.ChopperMission);
			if (list.Count == 0)
			{
				Debug.LogError("CityMissionManager.FindNewMission: No text found for missionType : " + MissionTypeNames.name[(int)missionData.mission.missionType]);
				return null;
			}
		}
		int num5 = 0;
		num2 = 100;
		index = 0;
		for (int k = 0; k < 10; k++)
		{
			num5 = m_RandomText.Next(0, list.Count);
			int num6 = TextInHistoryValue(list[num5].caption);
			if (num6 == 0 || num2 > num6)
			{
				num2 = num6;
				index = num5;
			}
			if (num6 == 0)
			{
				break;
			}
		}
		missionData.texts = list[index];
		if (story)
		{
			missionData.difficulty = missionData.mission.difficulty;
		}
		else
		{
			missionData.difficulty = nonstoryDifficulty;
		}
		MissionTypeHistory missionTypeHistory = new MissionTypeHistory();
		missionTypeHistory.m_MissionType = missionData.mission.missionType;
		missionTypeHistory.m_ActuallyOnMap = 1;
		m_MissionTypeHistory.AddLast(missionTypeHistory);
		if (m_MissionTypeHistory.Count > 6)
		{
			m_MissionTypeHistory.RemoveFirst();
		}
		m_TextHistory.AddLast(missionData.texts.caption);
		if (m_TextHistory.Count > 6)
		{
			m_TextHistory.RemoveFirst();
		}
		m_LevelHistory.AddLast(missionData.level);
		if (m_LevelHistory.Count > 5)
		{
			m_LevelHistory.RemoveFirst();
		}
		return missionData;
	}

	private int MissionTypeInHistoryValue(E_MissionType missionType)
	{
		int num = 0;
		foreach (MissionTypeHistory item in m_MissionTypeHistory)
		{
			if (item.m_MissionType == missionType)
			{
				num = ((item.m_ActuallyOnMap <= 0) ? ((num != 0) ? (num + 1) : (num + 1)) : (num + 3));
			}
		}
		return num;
	}

	private int TextInHistoryValue(int text)
	{
		int num = 1;
		foreach (int item in m_TextHistory)
		{
			if (item == text)
			{
				return num;
			}
			num++;
		}
		return 0;
	}

	private int LevelInHistoryValue(string level)
	{
		int num = 1;
		foreach (string item in m_LevelHistory)
		{
			if (item == level)
			{
				return num;
			}
			num++;
		}
		return 0;
	}
}
