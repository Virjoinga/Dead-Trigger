using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionGraphicData : MonoBehaviour
{
	[Serializable]
	public class Info
	{
		public string level = string.Empty;

		public int sinceStoryId;

		public List<string> preview = new List<string>();
	}

	public class ShortInfo
	{
		public string level;

		public string preview;
	}

	[HideInInspector]
	[SerializeField]
	private List<Info> m_Data = new List<Info>();

	private System.Random m_Random = new System.Random((int)DateTime.Now.Ticks);

	private System.Random m_Random2 = new System.Random((int)DateTime.Now.Ticks / 2);

	private List<Info> availableLevels = new List<Info>();

	public List<Info> data
	{
		get
		{
			return m_Data;
		}
	}

	public bool AnyMatchingLevelAvailable(List<string> levels, int storyId)
	{
		if (levels.Count == 0)
		{
			foreach (Info datum in m_Data)
			{
				if (datum.sinceStoryId <= storyId)
				{
					return true;
				}
			}
		}
		else
		{
			foreach (string level in levels)
			{
				Info info = FindInfo(level);
				if (info != null)
				{
					return true;
				}
				Debug.LogError(" MissionLevelData: Can't find info for graphics level:" + level);
			}
		}
		return false;
	}

	public ShortInfo GetRandomMatchingLevel(List<string> levels, int storyId)
	{
		availableLevels.Clear();
		if (levels.Count == 0)
		{
			foreach (Info datum in m_Data)
			{
				if (datum.sinceStoryId <= storyId)
				{
					availableLevels.Add(datum);
				}
			}
		}
		else
		{
			foreach (string level in levels)
			{
				Info info = FindInfo(level);
				if (info != null)
				{
					availableLevels.Add(info);
				}
				else
				{
					Debug.LogError(" MissionLevelData: Can't find info for graphics level:" + level);
				}
			}
		}
		if (availableLevels.Count == 0)
		{
			Debug.LogError(string.Concat(" MissionLevelData: Can't find available level for ", levels, " and story Id ", storyId));
			return null;
		}
		ShortInfo shortInfo = new ShortInfo();
		int index = m_Random.Next(0, availableLevels.Count);
		shortInfo.level = availableLevels[index].level;
		if (availableLevels[index].preview.Count > 0)
		{
			int index2 = m_Random2.Next(0, availableLevels[index].preview.Count);
			shortInfo.preview = availableLevels[index].preview[index2];
		}
		return shortInfo;
	}

	public Info FindInfo(string level)
	{
		foreach (Info datum in m_Data)
		{
			if (datum.level == level)
			{
				return datum;
			}
		}
		return null;
	}
}
