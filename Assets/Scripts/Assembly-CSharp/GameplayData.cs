using UnityEngine;

public class GameplayData
{
	private static GameplayData m_Instance;

	private PlayerLevelData m_PlayerLevelData;

	private EnemySettings m_EnemySettings;

	private PlayerLevelData.PlayerLevelInfo m_PlayerLevelInfo;

	private float m_MoneyModifierByItems = 1f;

	private int m_CurrentRank = -1;

	private int m_StrongerWave;

	public static GameplayData Instance
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

	public PlayerLevelData playerLevelData
	{
		get
		{
			return m_PlayerLevelData;
		}
	}

	public EnemySettings enemySettings
	{
		get
		{
			return m_EnemySettings;
		}
	}

	public int XpPerZombie()
	{
		float num = GetPlayerInfo().xpPerZombie;
		return Mathf.CeilToInt(num * GetXpModifier());
	}

	public int XpCompletionBonus()
	{
		float num = GetPlayerInfo().xpMissionBonus;
		return Mathf.CeilToInt(num * GetXpModifier());
	}

	public int MoneyPerZombie()
	{
		int moneyPerZombie = GetPlayerInfo().moneyPerZombie;
		return Mathf.CeilToInt((float)moneyPerZombie * GetMoneyModifier());
	}

	public int MoneyCompletionBonus()
	{
		float num = GetPlayerInfo().moneyMissionBonus;
		return Mathf.CeilToInt(num * GetMoneyModifier());
	}

	public int MoneyPerSpecialEnemy()
	{
		int num = MoneyPerZombie();
		return Mathf.CeilToInt((float)num * 0.5f);
	}

	public int MoneyPerHead()
	{
		int moneyPerHead = PlayerLevelData.PlayerLevelInfo.moneyPerHead;
		return Mathf.CeilToInt((float)moneyPerHead * GetMoneyModifier());
	}

	public int MoneyPerLimb()
	{
		int moneyPerLimb = PlayerLevelData.PlayerLevelInfo.moneyPerLimb;
		return Mathf.CeilToInt((float)moneyPerLimb * GetMoneyModifier());
	}

	public int MoneyPerCarnage()
	{
		int moneyPerCarnage = PlayerLevelData.PlayerLevelInfo.moneyPerCarnage;
		return Mathf.CeilToInt((float)moneyPerCarnage * GetMoneyModifier());
	}

	public int EnemyHealth()
	{
		float num = GetPlayerInfo().enemyHealth;
		return Mathf.RoundToInt(num * GetEnemyHealthModifier());
	}

	public int EnemyAttack()
	{
		float num = GetPlayerInfo().enemyAttack;
		return Mathf.RoundToInt(num * GetEnemyAttackModifier());
	}

	public void RecomputeItemModifiers()
	{
		m_MoneyModifierByItems = 1f;
		foreach (PPIItemData item in Game.Instance.PlayerPersistentInfo.EquipList.Items)
		{
			ItemSettings itemSettings = ItemSettingsManager.Instance.Get(item.ID);
			if (itemSettings.ItemBehaviour == E_ItemBehaviour.IncreaseMoney)
			{
				m_MoneyModifierByItems *= itemSettings.PowerUpModifier;
			}
		}
	}

	public void ResetStrongerWave()
	{
		m_StrongerWave = 0;
	}

	public void SetStrongerWave()
	{
		m_StrongerWave++;
	}

	private PlayerLevelData.PlayerLevelInfo GetPlayerInfo()
	{
		if (m_PlayerLevelInfo == null || m_CurrentRank < 0 || m_CurrentRank != Game.Instance.PlayerPersistentInfo.rank)
		{
			m_CurrentRank = Game.Instance.PlayerPersistentInfo.rank;
			m_PlayerLevelInfo = playerLevelData.GetInfo(m_CurrentRank);
		}
		return m_PlayerLevelInfo;
	}

	private float GetMoneyModifier()
	{
		float num = m_MoneyModifierByItems;
		if (Game.Instance.Difficulty == MissionFlowData.Difficulty.Easy)
		{
			num *= 0.75f;
		}
		else if (Game.Instance.Difficulty == MissionFlowData.Difficulty.Hard)
		{
			num *= 1.25f;
		}
		return num * 0.85f;
	}

	private float GetXpModifier()
	{
		return 1f;
	}

	private float GetEnemyHealthModifier()
	{
		float num = 1f;
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			num = ((m_StrongerWave != 1) ? (num + 0.3f * (float)m_StrongerWave) : (num + 0.2f * (float)m_StrongerWave));
		}
		else if (Game.Instance.Difficulty == MissionFlowData.Difficulty.Easy)
		{
			num *= 0.75f;
		}
		else if (Game.Instance.Difficulty == MissionFlowData.Difficulty.Hard)
		{
			num *= 1.25f;
		}
		return num;
	}

	private float GetEnemyAttackModifier()
	{
		float num = 1f;
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			num = ((m_StrongerWave != 1) ? (num + 0.25f * (float)m_StrongerWave) : (num + 0.2f * (float)m_StrongerWave));
		}
		else if (Game.Instance.Difficulty == MissionFlowData.Difficulty.Hard)
		{
			num *= 1.25f;
		}
		return num;
	}

	private static void CreateInstance()
	{
		m_Instance = new GameplayData();
		m_Instance.Init();
	}

	private void Init()
	{
		GameObject gameObject = Resources.Load("Player/GameplayData") as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Can't find object Player/GameplayData in Resources folder!");
			return;
		}
		m_PlayerLevelData = gameObject.GetComponent<PlayerLevelData>();
		if (m_PlayerLevelData == null)
		{
			Debug.LogError("Can't find component PlayerLevelData in prefab Player/GameplayData in Resources folder!");
		}
		m_EnemySettings = gameObject.GetComponent<EnemySettings>();
		if (m_EnemySettings == null)
		{
			Debug.LogError("Can't find component EnemySettings in prefab Player/GameplayData in Resources folder!");
		}
	}
}
