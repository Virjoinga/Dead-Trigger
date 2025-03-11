using System;

public class PlayerPersistentInfoData
{
	public struct S_Params
	{
		public int Experience;

		public int Money;

		public int Gold;

		public int m_StoryId;

		public int m_ChapterProgress;

		public int m_TotalMissionsPlayed;

		public int m_StoryMissionsPlayed;

		public int Kills;

		public int Deaths;

		public int HeadShots;

		public int Limbs;

		public int Carnage;

		public int TotalGold;

		public int TotalMoney;

		public int MissionCount;

		public int MissionSuccess;

		public float GameTime;

		public int Arena1Played;

		public int Arena1HiMoney;

		public int Arena1HiScore;

		public float Arena1Time;

		public int Arena2Played;

		public int Arena2HiMoney;

		public int Arena2HiScore;

		public float Arena2Time;

		public int Arena3Played;

		public int Arena3HiMoney;

		public int Arena3HiScore;

		public float Arena3Time;

		public int Arena4Played;

		public int Arena4HiMoney;

		public int Arena4HiScore;

		public float Arena4Time;

		public DateTime LastBonusTime;

		public int GoldForGoldMission;

		public int Tickets;

		public bool ProtectObjectsTutorial;
	}

	public S_Params Params;

	public PPIInventoryList InventoryList = new PPIInventoryList();

	public PPIEquipList EquipList = new PPIEquipList();

	public PPIUpgradeList Upgrades = new PPIUpgradeList();

	public PPIBankData BankData = new PPIBankData();

	public int InternalDataVersion;

	public DateTime InternalCloudSyncTime;

	public string AccountName;
}
