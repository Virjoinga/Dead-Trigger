using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AgentsCache2
{
	public class EnemyData
	{
		public E_AgentType m_Type;

		public float m_MaxSpawnProb;

		public List<string> m_Prefabs;
	}

	private class PrefabData
	{
		public string m_Name;

		public int m_Count;

		public ResourceCache m_Cache;
	}

	public string m_PlayerResName = "Player/PlayerFPV";

	private List<PrefabData> m_Prefabs;

	private Dictionary<E_AgentType, ResourceCache[]> m_CacheDict;

	private Hashtable m_HashTable;

	public static bool UsesRagdolls { get; private set; }

	public AgentsCache2()
	{
		m_CacheDict = new Dictionary<E_AgentType, ResourceCache[]>();
		m_HashTable = new Hashtable(9, 0.7f);
	}

	public void InitPlayer()
	{
		ResourceCache resourceCache = new ResourceCache(m_PlayerResName, 1, 0);
		m_CacheDict.Add(E_AgentType.Player, new ResourceCache[1] { resourceCache });
	}

	public void InitEnemies(int MaxEnemiesNum, List<EnemyData> Data)
	{
		UsesRagdolls = DeviceInfo.PerformanceGrade >= DeviceInfo.Performance.UltraHigh;
		if (UsesRagdolls)
		{
			foreach (EnemyData Datum in Data)
			{
				for (int i = 0; i < Datum.m_Prefabs.Count; i++)
				{
					List<string> prefabs;
					List<string> list = (prefabs = Datum.m_Prefabs);
					int index;
					int index2 = (index = i);
					string text = prefabs[index];
					list[index2] = text + "_R";
				}
			}
		}
		m_Prefabs = new List<PrefabData>();
		foreach (EnemyData Datum2 in Data)
		{
			if (Datum2.m_Prefabs.Count > 0)
			{
				int num = (int)Mathf.Round(0.51f + (float)MaxEnemiesNum * Datum2.m_MaxSpawnProb / (float)Datum2.m_Prefabs.Count);
				if (Datum2.m_Type == E_AgentType.BossSanta || Datum2.m_Type == E_AgentType.Boss1)
				{
					num++;
				}
				for (int j = 0; j < Datum2.m_Prefabs.Count; j++)
				{
					AddPrefab(Datum2.m_Prefabs[j], num);
				}
			}
		}
		int num2 = 0;
		int num3 = 0;
		PrefabData prefabData = null;
		foreach (PrefabData prefab in m_Prefabs)
		{
			num3++;
			num2 += prefab.m_Count;
			prefab.m_Cache = new ResourceCache(prefab.m_Name, prefab.m_Count, 0);
		}
		List<ResourceCache> list2 = new List<ResourceCache>();
		foreach (EnemyData Datum3 in Data)
		{
			list2.Clear();
			for (int k = 0; k < Datum3.m_Prefabs.Count; k++)
			{
				prefabData = FindPrefab(Datum3.m_Prefabs[k]);
				list2.Add(prefabData.m_Cache);
			}
			m_CacheDict.Add(Datum3.m_Type, list2.ToArray());
		}
		InitGpuResources();
	}

	private void InitGpuResources()
	{
		int num = (UsesRagdolls ? 2 : 0);
		Vector3 position = new Vector3(0f, 300f, 0f);
		foreach (PrefabData prefab in m_Prefabs)
		{
			string text = prefab.m_Name.Substring(0, prefab.m_Name.Length - num);
			text = text.Insert(text.LastIndexOf("/") + 1, "loaders/Loader_");
			UnityEngine.Object @object = Resources.Load(text);
			if (@object != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(@object) as GameObject;
				gameObject.transform.position = position;
				position.x += 1f;
			}
		}
	}

	public GameObject SpitOut(E_AgentType Type)
	{
		ResourceCache[] array = m_CacheDict[Type];
		if (array != null)
		{
			int max = array.Length;
			int num = UnityEngine.Random.Range(0, max);
			ResourceCache resourceCache = null;
			while (max-- > 0)
			{
				resourceCache = array[num];
				if (resourceCache.GetFreeNum() > 0)
				{
					GameObject gameObject = resourceCache.Get();
					m_HashTable.Add(gameObject, resourceCache);
					return gameObject;
				}
				num = (num + 1) % array.Length;
			}
		}
		return null;
	}

	public void Swallow(GameObject Obj)
	{
		ResourceCache resourceCache = m_HashTable[Obj] as ResourceCache;
		if (resourceCache != null)
		{
			resourceCache.Return(Obj);
			m_HashTable.Remove(Obj);
		}
	}

	private PrefabData FindPrefab(string ResName)
	{
		foreach (PrefabData prefab in m_Prefabs)
		{
			if (prefab.m_Name == ResName)
			{
				return prefab;
			}
		}
		return null;
	}

	private void AddPrefab(string ResName, int Count)
	{
		PrefabData prefabData = FindPrefab(ResName);
		if (prefabData != null)
		{
			prefabData.m_Count += Count;
			return;
		}
		prefabData = new PrefabData();
		prefabData.m_Name = ResName;
		prefabData.m_Count = Count;
		prefabData.m_Cache = null;
		m_Prefabs.Add(prefabData);
	}
}
