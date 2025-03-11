using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySettings : MonoBehaviour
{
	[Serializable]
	public class Prefab
	{
		public int playerLevel = 1;

		public string prefab = string.Empty;
	}

	[Serializable]
	public class Info
	{
		public E_AgentType agentType = E_AgentType.ZombieSlow1;

		public int sincePlayerRank;

		public float healthModif;

		public float attackModif;

		public List<Prefab> prefabs = new List<Prefab>();
	}

	[SerializeField]
	[HideInInspector]
	private List<Info> m_Data = new List<Info>();

	public List<Info> data
	{
		get
		{
			return m_Data;
		}
	}

	public Info GetEnemyInfo(E_AgentType agentType)
	{
		foreach (Info datum in m_Data)
		{
			if (datum.agentType == agentType)
			{
				return datum;
			}
		}
		Debug.LogWarning("Can't find enemy info: " + agentType);
		return null;
	}
}
