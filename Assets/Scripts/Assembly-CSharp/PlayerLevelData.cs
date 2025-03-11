using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerLevelData : MonoBehaviour
{
	[Serializable]
	public class PlayerLevelInfo
	{
		public int playerLevel = 1;

		public int playerXp;

		public E_WeaponID recommendedWeapon;

		public int xpPerZombie;

		public int xpMissionBonus;

		public int moneyPerZombie;

		public int moneyMissionBonus;

		public int enemyHealth = 100;

		public int enemyAttack = 25;

		[NonSerialized]
		public static int moneyPerHead = 10;

		[NonSerialized]
		public static int moneyPerLimb = 5;

		[NonSerialized]
		public static int moneyPerCarnage = 10;
	}

	[HideInInspector]
	[SerializeField]
	private List<PlayerLevelInfo> m_Data = new List<PlayerLevelInfo>();

	public List<PlayerLevelInfo> data
	{
		get
		{
			return m_Data;
		}
	}

	public int GetRankCount()
	{
		return m_Data.Count;
	}

	public PlayerLevelInfo GetInfo(int playerLevel)
	{
		for (int i = 0; i < m_Data.Count; i++)
		{
			if (m_Data[i].playerLevel == playerLevel)
			{
				return m_Data[i];
			}
		}
		Debug.LogError("PlayerLevelData: Can't find record for difficulty: " + playerLevel);
		return null;
	}

	public PlayerLevelInfo GetInfoByPlayerXp(int playerXp)
	{
		for (int i = 0; i < m_Data.Count; i++)
		{
			if (m_Data[i].playerXp <= playerXp && (i == m_Data.Count - 1 || m_Data[i + 1].playerXp > playerXp))
			{
				return m_Data[i];
			}
		}
		Debug.LogError("PlayerLevelData: Can't find record for playerXP: " + playerXp);
		return null;
	}

	private void Sort()
	{
		m_Data.Sort((PlayerLevelInfo d1, PlayerLevelInfo d2) => d1.playerLevel.CompareTo(d2.playerLevel));
	}

	private void Start()
	{
		Sort();
	}
}
