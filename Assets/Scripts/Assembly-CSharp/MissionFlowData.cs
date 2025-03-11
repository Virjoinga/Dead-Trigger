using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MissionFlowData : MonoBehaviour
{
	public enum StoryBound
	{
		Story = 0,
		Dependent = 1,
		None = 2,
		ChopperMission = 3,
		DailyReward = 4
	}

	public enum Difficulty
	{
		Easy = 0,
		Normal = 1,
		Hard = 2
	}

	[Serializable]
	public class Mission
	{
		public int storyId;

		public E_MissionType missionType;

		public string missionSubtype = string.Empty;

		public StoryBound storyBound = StoryBound.None;

		public bool unique;

		public List<string> levels = new List<string>();

		public string specialIcon = string.Empty;

		public List<MissionTexts> texts = new List<MissionTexts>();

		public Difficulty difficulty = Difficulty.Normal;

		public string specialSlot = string.Empty;

		public SpecialReward.Type specialBonus;

		public List<string> specialZombies = new List<string>();

		private int m_Used;

		[SerializeField]
		internal int m_UID;

		public int used
		{
			get
			{
				return m_Used;
			}
			set
			{
				m_Used = value;
			}
		}

		public int uid
		{
			get
			{
				return m_UID;
			}
		}

		internal Mission()
		{
		}
	}

	[SerializeField]
	[HideInInspector]
	private List<Mission> m_Data = new List<Mission>();

	[SerializeField]
	private int m_UIDGenerator;

	public List<Mission> data
	{
		get
		{
			return m_Data;
		}
	}

	public Mission GetMission(int uid)
	{
		foreach (Mission datum in data)
		{
			if (datum.uid == uid)
			{
				return datum;
			}
		}
		return null;
	}

	public Mission CreateNewMission()
	{
		Mission mission = new Mission();
		mission.m_UID = m_UIDGenerator++;
		Debug.Log("New mission UID: " + mission.m_UID);
		return mission;
	}

	public void Save(IDataFile file)
	{
		int num = 1;
		foreach (Mission datum in data)
		{
			if (datum.storyId == Game.Instance.PlayerPersistentInfo.storyId && datum.used > 0 && datum.unique)
			{
				file.SetInt("MissionFlowDataUid" + num, datum.uid);
				file.SetInt("MissionFlowData" + num, datum.used);
				num++;
			}
		}
	}

	public void Load(IDataFile file)
	{
		int num = 1;
		foreach (Mission datum in data)
		{
			datum.used = 0;
		}
		while (true)
		{
			string key = "MissionFlowData" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			int @int = file.GetInt(key);
			int int2 = file.GetInt("MissionFlowDataUid" + num);
			Mission mission = GetMission(int2);
			if (mission != null)
			{
				mission.used = @int;
			}
			else
			{
				Debug.LogWarning("Can't find mission with loaded uid: " + int2);
			}
			num++;
		}
	}

	private void Sort()
	{
		m_Data.Sort(delegate(Mission m1, Mission m2)
		{
			if (m1.storyId == m2.storyId)
			{
				int storyBound = (int)m1.storyBound;
				return storyBound.CompareTo((int)m2.storyBound);
			}
			return m1.storyId.CompareTo(m2.storyId);
		});
	}

	private void Start()
	{
		Sort();
	}
}
