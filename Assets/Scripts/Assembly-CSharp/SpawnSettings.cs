using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnSettings : MonoBehaviour
{
	public enum AgentType
	{
		Walker = 0,
		Normal_Zombie = 1,
		Fast_Zombie = 2,
		Berserker = 3,
		Vomitter = 4,
		Swat = 5,
		Boss1 = 6,
		Boss1_small = 7,
		Athlete = 8,
		BossSanta = 9,
		Last = 10
	}

	[Serializable]
	public enum SpawnProbability
	{
		None = 0,
		Low = 1,
		Medium = 2,
		High = 3,
		VeryHigh = 4
	}

	[Serializable]
	public class SpawnSetting
	{
		public SpawnProbability[] probability = new SpawnProbability[10];
	}

	private class SkinValue
	{
		public string skin;

		public float value;
	}

	private class EnemyTypeSkinsInfo
	{
		private List<SkinValue> skinValues = new List<SkinValue>();

		public void Clear()
		{
			skinValues.Clear();
		}

		public bool HasSkinInList(string name)
		{
			foreach (SkinValue skinValue in skinValues)
			{
				if (skinValue.skin == name)
				{
					return true;
				}
			}
			return false;
		}

		public void AddSkinValue(string name, float value)
		{
			foreach (SkinValue skinValue2 in skinValues)
			{
				if (skinValue2.skin == name)
				{
					skinValue2.value += value;
					return;
				}
			}
			SkinValue skinValue = new SkinValue();
			skinValue.skin = name;
			skinValue.value = value;
			skinValues.Add(skinValue);
		}

		public string GetRandomSkinWithBestValue(out int skinCount)
		{
			List<string> list = new List<string>();
			float num = 0f;
			foreach (SkinValue skinValue in skinValues)
			{
				if (num < skinValue.value || Mathf.Abs(num - skinValue.value) < 0.01f)
				{
					num = skinValue.value;
					list.Add(skinValue.skin);
				}
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			skinCount = list.Count;
			return list[index];
		}

		public IList<SkinValue> GetList()
		{
			return skinValues;
		}

		public IList<SkinValue> GetSortedList()
		{
			Sort();
			return GetList();
		}

		private void Sort()
		{
			skinValues.Sort(delegate(SkinValue value1, SkinValue value2)
			{
				if (value1.value > value2.value)
				{
					return -1;
				}
				return (value1.value != value2.value) ? 1 : 0;
			});
		}
	}

	private const int MAX_SKINS_IN_MISSION = 5;

	private const int MAX_ENEMY_TYPES_IN_MISSION = 6;

	private const float MAX_RANK_FOR_PCT = 30f;

	public bool useBoss;

	public AgentType bosssType = AgentType.Boss1;

	public List<SpawnSetting> settings = new List<SpawnSetting>();

	public int SpawnSettingsCount()
	{
		return settings.Count;
	}

	public void CreateSpawnManager()
	{
		EnemyTypeSkinsInfo enemyTypeSkinsInfo = new EnemyTypeSkinsInfo();
		EnemyTypeSkinsInfo[] array = new EnemyTypeSkinsInfo[10];
		EnemyTypeSkinsInfo[] array2 = new EnemyTypeSkinsInfo[10];
		float[] array3 = new float[10];
		int num = 0;
		List<int> list = new List<int>();
		for (int i = 0; i < settings.Count; i++)
		{
			float[] spawnProbability = GetSpawnProbability(i);
			for (int j = 0; j < array3.Length; j++)
			{
				if (array3[j] < spawnProbability[j])
				{
					array3[j] = spawnProbability[j];
				}
			}
		}
		for (int k = 0; k < array3.Length; k++)
		{
			if (array3[k] > 1E-05f)
			{
				num++;
				if (IsOptionalEnemy((AgentType)k) && !list.Contains(k))
				{
					list.Add(k);
				}
			}
		}
		bool flag = false;
		if (num > 6)
		{
			int num2 = 6 - (num - list.Count);
			if (num2 <= 0)
			{
				Debug.LogWarning("STRANGE ERROR - can't remove any special enemy type when too many are requested!");
			}
			else
			{
				list.Shuffle();
				for (int l = 0; l < list.Count; l++)
				{
					if (l >= num2)
					{
						for (int m = 0; m < settings.Count; m++)
						{
							settings[m].probability[list[l]] = SpawnProbability.None;
						}
						array3[list[l]] = 0f;
						flag = true;
					}
				}
			}
		}
		if (flag)
		{
			for (int n = 0; n < settings.Count; n++)
			{
				float[] spawnProbability2 = GetSpawnProbability(n);
				for (int num3 = 0; num3 < array3.Length; num3++)
				{
					if (array3[num3] < spawnProbability2[num3])
					{
						array3[num3] = spawnProbability2[num3];
					}
				}
			}
		}
		bool flag2 = false;
		for (int num4 = 0; num4 < array3.Length; num4++)
		{
			if (!(array3[num4] > 0f))
			{
				continue;
			}
			flag2 = true;
			E_AgentType agentType = Convert((AgentType)num4);
			EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(agentType);
			array[num4] = new EnemyTypeSkinsInfo();
			foreach (EnemySettings.Prefab prefab in enemyInfo.prefabs)
			{
				if (prefab.playerLevel <= Game.Instance.PlayerPersistentInfo.rank || Game.Instance.GameplayType == GameplayType.Arena)
				{
					enemyTypeSkinsInfo.AddSkinValue(prefab.prefab, 10f * array3[num4]);
					array[num4].AddSkinValue(prefab.prefab, 10f * array3[num4]);
				}
			}
		}
		if (!flag2)
		{
			CreateSpawnManagerWithDefaultData();
			return;
		}
		for (int num5 = 0; num5 < array.Length; num5++)
		{
			if (array[num5] != null)
			{
				int skinCount;
				string randomSkinWithBestValue = array[num5].GetRandomSkinWithBestValue(out skinCount);
				if (skinCount == 1)
				{
					enemyTypeSkinsInfo.AddSkinValue(randomSkinWithBestValue, 200f);
				}
				else
				{
					enemyTypeSkinsInfo.AddSkinValue(randomSkinWithBestValue, 100f);
				}
			}
		}
		IList<SkinValue> sortedList = enemyTypeSkinsInfo.GetSortedList();
		int num6 = 0;
		foreach (SkinValue item in sortedList)
		{
			num6++;
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				if (array[num7] != null)
				{
					if (array2[num7] == null)
					{
						array2[num7] = new EnemyTypeSkinsInfo();
					}
					if (array[num7].HasSkinInList(item.skin))
					{
						array2[num7].AddSkinValue(item.skin, item.value);
					}
				}
			}
			if (num6 >= 5)
			{
				break;
			}
		}
		CreateSpawnManager(array3, array2, useBoss, bosssType);
	}

	public static void CreateSpawnManagerWithDefaultData()
	{
		float[] defaultSpawnProbability = GetDefaultSpawnProbability();
		EnemyTypeSkinsInfo[] array = new EnemyTypeSkinsInfo[10];
		for (int i = 0; i < defaultSpawnProbability.Length; i++)
		{
			if (!(defaultSpawnProbability[i] > 0f))
			{
				continue;
			}
			E_AgentType agentType = Convert((AgentType)i);
			EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(agentType);
			array[i] = new EnemyTypeSkinsInfo();
			int num = 0;
			foreach (EnemySettings.Prefab prefab in enemyInfo.prefabs)
			{
				if (prefab.playerLevel <= Game.Instance.PlayerPersistentInfo.rank || Game.Instance.GameplayType == GameplayType.Arena)
				{
					array[i].AddSkinValue(prefab.prefab, 1f);
					num++;
					if (num >= 5)
					{
						break;
					}
				}
			}
			break;
		}
		CreateSpawnManager(defaultSpawnProbability, array, false, AgentType.Last);
		SendSpawnDataToSpawnManager(defaultSpawnProbability);
	}

	public void SendSpawnDataToSpawnManager(int settingIndex)
	{
		float[] spawnProbability = GetSpawnProbability(settingIndex);
		SendSpawnDataToSpawnManager(spawnProbability);
	}

	public void FillAgentData(int settingIndex, List<AgentHuman.ModData> modData)
	{
		float[] spawnProbability = GetSpawnProbability(settingIndex);
		FillAgentData(spawnProbability, modData);
	}

	private void Awake()
	{
		bosssType = AgentType.Boss1;
		foreach (SpawnSetting setting in settings)
		{
			if (setting.probability.Length < 10)
			{
				SpawnProbability[] array = new SpawnProbability[10];
				for (int i = 0; i < setting.probability.Length; i++)
				{
					array[i] = setting.probability[i];
				}
				setting.probability = array;
			}
		}
	}

	private static void CreateSpawnManager(float[] sumPct, EnemyTypeSkinsInfo[] processedSkins, bool useBoss, AgentType bossType)
	{
		SpawnManager.InitData initData = new SpawnManager.InitData();
		initData.m_Enemies = new List<SpawnManager.EnemyInitData>();
		for (int i = 0; i < sumPct.Length; i++)
		{
			if (!(sumPct[i] > 0f))
			{
				continue;
			}
			SpawnManager.EnemyInitData enemyInitData = new SpawnManager.EnemyInitData();
			E_AgentType e_AgentType = Convert((AgentType)i);
			EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(e_AgentType);
			enemyInitData.m_MaxSpawnProb = sumPct[i];
			enemyInitData.m_Type = e_AgentType;
			enemyInitData.m_Mods = new AgentHuman.ModData();
			enemyInitData.m_Mods.m_Health = GameplayData.Instance.EnemyHealth();
			enemyInitData.m_Mods.m_AttackDamage = GameplayData.Instance.EnemyAttack();
			enemyInitData.m_Mods.m_Health *= enemyInfo.healthModif;
			enemyInitData.m_Mods.m_AttackDamage *= enemyInfo.attackModif;
			IList<SkinValue> list = processedSkins[i].GetList();
			enemyInitData.m_Prefabs = new List<string>();
			foreach (SkinValue item in list)
			{
				enemyInitData.m_Prefabs.Add(item.skin);
			}
			initData.m_Enemies.Add(enemyInitData);
		}
		if (useBoss)
		{
			E_AgentType e_AgentType2 = Convert(bossType);
			EnemySettings.Info enemyInfo2 = GameplayData.Instance.enemySettings.GetEnemyInfo(e_AgentType2);
			if (enemyInfo2.sincePlayerRank <= Game.Instance.PlayerPersistentInfo.rank || Game.Instance.GameplayType == GameplayType.Arena)
			{
				SpawnManager.EnemyInitData enemyInitData2 = new SpawnManager.EnemyInitData();
				EnemySettings.Info enemyInfo3 = GameplayData.Instance.enemySettings.GetEnemyInfo(e_AgentType2);
				enemyInitData2.m_MaxSpawnProb = 0f;
				enemyInitData2.m_Type = e_AgentType2;
				enemyInitData2.m_Mods = new AgentHuman.ModData();
				enemyInitData2.m_Mods.m_Type = e_AgentType2;
				enemyInitData2.m_Mods.m_Health = GameplayData.Instance.EnemyHealth();
				enemyInitData2.m_Mods.m_AttackDamage = GameplayData.Instance.EnemyAttack();
				enemyInitData2.m_Mods.m_Health *= enemyInfo3.healthModif;
				enemyInitData2.m_Mods.m_AttackDamage *= enemyInfo3.attackModif;
				IList<EnemySettings.Prefab> prefabs = enemyInfo3.prefabs;
				enemyInitData2.m_Prefabs = new List<string>();
				foreach (EnemySettings.Prefab item2 in prefabs)
				{
					if (item2.playerLevel <= Game.Instance.PlayerPersistentInfo.rank || Game.Instance.GameplayType == GameplayType.Arena)
					{
						enemyInitData2.m_Prefabs.Add(item2.prefab);
					}
				}
				initData.m_Enemies.Add(enemyInitData2);
			}
		}
		SpawnManager.CreateInstance(initData);
	}

	private static void SendSpawnDataToSpawnManager(float[] pct)
	{
		List<SpawnManager.EnemySpawnData> list = new List<SpawnManager.EnemySpawnData>();
		for (int i = 0; i < pct.Length; i++)
		{
			if (pct[i] > 0f)
			{
				SpawnManager.EnemySpawnData enemySpawnData = new SpawnManager.EnemySpawnData();
				enemySpawnData.m_Prob = pct[i];
				enemySpawnData.m_Type = Convert((AgentType)i);
				list.Add(enemySpawnData);
			}
		}
		SpawnManager.Instance.SetSpawnProbability(list);
	}

	private static void FillAgentData(float[] pct, List<AgentHuman.ModData> modList)
	{
		for (int i = 0; i < pct.Length; i++)
		{
			if (pct[i] > 0f)
			{
				E_AgentType e_AgentType = Convert((AgentType)i);
				EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(e_AgentType);
				AgentHuman.ModData modData = new AgentHuman.ModData();
				modData.m_Type = e_AgentType;
				modData.m_Health = GameplayData.Instance.EnemyHealth();
				modData.m_AttackDamage = GameplayData.Instance.EnemyAttack();
				modData.m_Health *= enemyInfo.healthModif;
				modData.m_AttackDamage *= enemyInfo.attackModif;
				modList.Add(modData);
			}
		}
	}

	private float[] GetSpawnProbability(int settingIndex)
	{
		if (settingIndex >= settings.Count)
		{
			Debug.LogWarning("Invalid index! Requested: " + settingIndex + ", available: " + settings.Count);
			return GetDefaultSpawnProbability();
		}
		float[] array = new float[10];
		float num = 0f;
		for (int i = 0; i < settings[settingIndex].probability.Length; i++)
		{
			EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(Convert((AgentType)i));
			if (enemyInfo.sincePlayerRank > Game.Instance.PlayerPersistentInfo.rank && Game.Instance.GameplayType == GameplayType.Missions)
			{
				array[i] = 0f;
				continue;
			}
			float num3;
			if (Game.Instance.GameplayType == GameplayType.Missions)
			{
				float num2 = Mathf.Clamp(Game.Instance.PlayerPersistentInfo.rank - enemyInfo.sincePlayerRank, 0f, 30f);
				num3 = num2 / (30f + (float)enemyInfo.sincePlayerRank);
			}
			else
			{
				num3 = 1f;
			}
			num3 = ((enemyInfo.sincePlayerRank > 1) ? Mathf.Clamp(num3 + 0.02f, 0f, 1f) : 1f);
			switch (settings[settingIndex].probability[i])
			{
			case SpawnProbability.None:
				array[i] = 0f * num3;
				break;
			case SpawnProbability.Low:
				array[i] = 0.25f * num3;
				break;
			case SpawnProbability.Medium:
				array[i] = 0.5f * num3;
				break;
			case SpawnProbability.High:
				array[i] = 0.75f * num3;
				break;
			case SpawnProbability.VeryHigh:
				array[i] = 1f * num3;
				break;
			}
			if (!Mathf.Approximately(array[i], 0f) && array[i] < 0.04f)
			{
				array[i] = 0.04f;
			}
			num += array[i];
		}
		if (Mathf.Approximately(num, 0f))
		{
			return GetDefaultSpawnProbability();
		}
		num = 1f / num;
		for (int j = 0; j < array.Length; j++)
		{
			array[j] *= num;
		}
		return array;
	}

	private static float[] GetDefaultSpawnProbability()
	{
		float[] array = new float[10];
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0)
			{
				array[i] = 1f;
			}
			else
			{
				array[i] = 0f;
			}
		}
		return array;
	}

	private static bool IsOptionalEnemy(AgentType type)
	{
		switch (type)
		{
		case AgentType.Walker:
			return false;
		case AgentType.Normal_Zombie:
			return false;
		case AgentType.Fast_Zombie:
			return false;
		case AgentType.Boss1:
			return false;
		case AgentType.Athlete:
			if (Game.Instance.GameplayType == GameplayType.Arena)
			{
				return false;
			}
			break;
		case AgentType.BossSanta:
			return false;
		}
		return true;
	}

	private static E_AgentType Convert(AgentType type)
	{
		switch (type)
		{
		case AgentType.Walker:
			return E_AgentType.Walker1;
		case AgentType.Normal_Zombie:
			return E_AgentType.ZombieSlow1;
		case AgentType.Fast_Zombie:
			return E_AgentType.ZombieFast1;
		case AgentType.Berserker:
			return E_AgentType.Berserker1;
		case AgentType.Vomitter:
			return E_AgentType.Vomitter1;
		case AgentType.Swat:
			return E_AgentType.Swat;
		case AgentType.Boss1:
			return E_AgentType.Boss1;
		case AgentType.Boss1_small:
			return E_AgentType.Boss1_small;
		case AgentType.Athlete:
			return E_AgentType.Athlete;
		case AgentType.BossSanta:
			return E_AgentType.BossSanta;
		default:
			Debug.LogWarning("Unknown type: " + type);
			return E_AgentType.ZombieSlow1;
		}
	}

	private static AgentType Convert(E_AgentType type)
	{
		switch (type)
		{
		case E_AgentType.Walker1:
			return AgentType.Walker;
		case E_AgentType.ZombieSlow1:
			return AgentType.Normal_Zombie;
		case E_AgentType.ZombieFast1:
			return AgentType.Fast_Zombie;
		case E_AgentType.Berserker1:
			return AgentType.Berserker;
		case E_AgentType.Vomitter1:
			return AgentType.Vomitter;
		case E_AgentType.Swat:
			return AgentType.Swat;
		case E_AgentType.Boss1:
			return AgentType.Boss1;
		case E_AgentType.Boss1_small:
			return AgentType.Boss1_small;
		case E_AgentType.Athlete:
			return AgentType.Athlete;
		case E_AgentType.BossSanta:
			return AgentType.BossSanta;
		default:
			Debug.LogWarning("Unknown type: " + type);
			return AgentType.Normal_Zombie;
		}
	}
}
