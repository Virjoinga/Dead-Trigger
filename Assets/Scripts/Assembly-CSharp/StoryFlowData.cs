using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StoryFlowData : MonoBehaviour
{
	[Serializable]
	public class Story
	{
		public int id;

		public int storyCaption;

		public List<int> storyPages = new List<int>();

		public string storyPicture = string.Empty;

		public int storyText;

		public List<int> debriefPages = new List<int>();

		public string debriefPicture = string.Empty;

		public int objectiveText;

		public int missionsRequired;

		public int rankToProgress;

		[SerializeField]
		internal int m_UID;

		public int uid
		{
			get
			{
				return m_UID;
			}
		}

		internal Story()
		{
		}
	}

	[HideInInspector]
	[SerializeField]
	private List<Story> m_Data = new List<Story>();

	[SerializeField]
	private int m_UIDGenerator;

	public List<Story> data
	{
		get
		{
			return m_Data;
		}
	}

	public Story GetStory(int storyId)
	{
		foreach (Story datum in m_Data)
		{
			if (datum.id == storyId)
			{
				return datum;
			}
		}
		return null;
	}

	public Story CreateNewStory()
	{
		Story story = new Story();
		story.m_UID = m_UIDGenerator++;
		Debug.Log("New story UID: " + story.m_UID);
		return story;
	}

	private void Sort()
	{
		m_Data.Sort(delegate(Story s1, Story s2)
		{
			if (s1 != s2 && s1.id == s2.id)
			{
				Debug.Log("StoryFlowData: Duplicite ID! " + s1.id);
			}
			return s1.id.CompareTo(s2.id);
		});
	}

	private void Start()
	{
		Sort();
	}
}
