using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : IEnumerable, IEnumerable<Statistics.Item>
{
	public enum E_Mode
	{
		None = 0,
		Player = 1,
		CompareWithFriend = 2,
		CompareWithBest = 3
	}

	public enum E_Better
	{
		None = 0,
		Smaller = 1,
		Bigger = 2
	}

	public abstract class Item
	{
		public int m_NameIndex;

		public string m_NameText;
	}

	public class BaseItem<T> : Item
	{
		public T m_PlayerValue;

		public T m_SecondValue;

		public string m_SecondValueFriendName;

		public bool m_HighlightPlayer;

		public bool m_HighlightFriend;
	}

	public class IntItem : BaseItem<int>
	{
	}

	public class FloatItem : BaseItem<float>
	{
	}

	public class StringItem : BaseItem<string>
	{
	}

	public delegate int IntExtractor(PlayerPersistentInfoData inData);

	private const int TEXT_ID_EXPERIENCE = 2040311;

	private const int TEXT_ID_MONEY = 2040312;

	private const int TEXT_ID_KILLS = 2040313;

	private const int TEXT_ID_DEATHS = 2040314;

	private const int TEXT_ID_MISSIONS_PLAYED = 2040315;

	private const int TEXT_ID_SUCCESSED_MISSIONS = 2040316;

	private const int TEXT_ID_CARNAGES = 2040317;

	private const int TEXT_ID_GOLD = 2040318;

	private const int TEXT_ID_HEADSHOTS = 2040319;

	private const int TEXT_ID_LIMBS = 2040320;

	private const int TEXT_ID_TOTAL_TIME = 2040321;

	private const int TEXT_ID_ARENA_1 = 2040391;

	private const int TEXT_ID_ARENA_1_HIGH_SCORE = 2040322;

	private const int TEXT_ID_ARENA_1_TOTAL_TIME = 2040323;

	private const int TEXT_ID_ARENA_1_PLAYED = 2040324;

	private const int TEXT_ID_ARENA_2 = 2040392;

	private const int TEXT_ID_ARENA_2_HIGH_SCORE = 2040325;

	private const int TEXT_ID_ARENA_2_TOTAL_TIME = 2040326;

	private const int TEXT_ID_ARENA_2_PLAYED = 2040327;

	private const int TEXT_ID_ARENA_3 = 2040393;

	private const int TEXT_ID_ARENA_3_HIGH_SCORE = 2040328;

	private const int TEXT_ID_ARENA_3_TOTAL_TIME = 2040329;

	private const int TEXT_ID_ARENA_3_PLAYED = 2040330;

	private const int TEXT_ID_ARENA_4 = 2040394;

	private const int TEXT_ID_ARENA_4_HIGH_SCORE = 2040331;

	private const int TEXT_ID_ARENA_4_TOTAL_TIME = 2040332;

	private const int TEXT_ID_ARENA_4_PLAYED = 2040333;

	private E_Mode m_CurrentMode;

	private List<Item> m_StatisticsItems = new List<Item>();

	public int Count
	{
		get
		{
			return m_StatisticsItems.Count;
		}
	}

	public E_Mode Mode
	{
		get
		{
			return m_CurrentMode;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public Item GetItem(int index)
	{
		return m_StatisticsItems[index];
	}

	public void PrepareFor(E_Mode inMode, string inFriendName)
	{
		PrepareFor_Internal(inMode, inFriendName);
	}

	public void Clear()
	{
		m_CurrentMode = E_Mode.None;
		m_StatisticsItems.Clear();
	}

	public IEnumerable<Item> Range(int inFrom, int inTo)
	{
		if (inFrom >= 0 && inFrom < m_StatisticsItems.Count && inTo >= 0 && inTo < m_StatisticsItems.Count)
		{
			for (int i = inFrom; i < inTo; i++)
			{
				yield return m_StatisticsItems[i];
			}
		}
	}

	public IEnumerator<Item> GetEnumerator()
	{
		for (int i = 0; i < m_StatisticsItems.Count; i++)
		{
			yield return m_StatisticsItems[i];
		}
	}

	private void PrepareFor_Internal(E_Mode inMode, string inFriendName)
	{
		m_CurrentMode = inMode;
		m_StatisticsItems.Clear();
		PlayerPersistentInfoData playerPersistentInfoData = null;
		try
		{
			playerPersistentInfoData = Game.Instance.PlayerPersistentInfo.GetPlayerData_ForStatistics();
		}
		catch
		{
			PlayerPersistantInfo playerPersistantInfo = new PlayerPersistantInfo();
			playerPersistantInfo.InitPlayerDataFromStr(GameSaveLoadUtl.OpenReadPlayerData().GetString("PPI", string.Empty));
			playerPersistentInfoData = playerPersistantInfo.GetPlayerData_ForStatistics();
		}
		List<FriendList.FriendInfo> list = new List<FriendList.FriendInfo>();
		switch (inMode)
		{
		case E_Mode.CompareWithFriend:
			if (!string.IsNullOrEmpty(inFriendName))
			{
				FriendList.FriendInfo friendInfo = GameCloudManager.friendList.friends.Find((FriendList.FriendInfo f) => f.m_Name == inFriendName);
				if (friendInfo != null && friendInfo.m_PPIData != null)
				{
					list.Add(friendInfo);
				}
			}
			else
			{
				Debug.LogError("Invalid friend name: " + inFriendName);
			}
			break;
		case E_Mode.CompareWithBest:
			list = GameCloudManager.friendList.friends;
			break;
		}
		m_StatisticsItems = HarvestStatistics(playerPersistentInfoData, list);
	}

	private static List<Item> HarvestStatistics(PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		List<Item> list = new List<Item>();
		list.Add(Get_Experiance(2040311, inPlayerData, inFriendsData));
		list.Add(Get_Money(2040312, inPlayerData, inFriendsData));
		list.Add(Get_Gold(2040318, inPlayerData, inFriendsData));
		list.Add(Get_Kills(2040313, inPlayerData, inFriendsData));
		list.Add(Get_Deaths(2040314, inPlayerData, inFriendsData));
		list.Add(Get_HeadShots(2040319, inPlayerData, inFriendsData));
		list.Add(Get_Carnages(2040317, inPlayerData, inFriendsData));
		list.Add(Get_Limbs(2040320, inPlayerData, inFriendsData));
		list.Add(Get_PlayedMissions(2040315, inPlayerData, inFriendsData));
		list.Add(Get_MissionSuccess(2040316, inPlayerData, inFriendsData));
		list.Add(Get_GameTime(2040321, inPlayerData, inFriendsData, E_Better.Bigger));
		string newValue = TextDatabase.instance[2040391];
		string inItemNameText = TextDatabase.instance[2040322].Replace("<ARENA_1_NAME>", newValue);
		string inItemNameText2 = TextDatabase.instance[2040323].Replace("<ARENA_1_NAME>", newValue);
		string inItemNameText3 = TextDatabase.instance[2040324].Replace("<ARENA_1_NAME>", newValue);
		list.Add(Get_Arena1_HighScore(inItemNameText, inPlayerData, inFriendsData));
		list.Add(Get_Arena1_GameTime(inItemNameText2, inPlayerData, inFriendsData, E_Better.Bigger));
		list.Add(Get_Arena1_Played(inItemNameText3, inPlayerData, inFriendsData));
		string newValue2 = TextDatabase.instance[2040392];
		string inItemNameText4 = TextDatabase.instance[2040325].Replace("<ARENA_2_NAME>", newValue2);
		string inItemNameText5 = TextDatabase.instance[2040326].Replace("<ARENA_2_NAME>", newValue2);
		string inItemNameText6 = TextDatabase.instance[2040327].Replace("<ARENA_2_NAME>", newValue2);
		list.Add(Get_Arena2_HighScore(inItemNameText4, inPlayerData, inFriendsData));
		list.Add(Get_Arena2_GameTime(inItemNameText5, inPlayerData, inFriendsData, E_Better.Bigger));
		list.Add(Get_Arena2_Played(inItemNameText6, inPlayerData, inFriendsData));
		string newValue3 = TextDatabase.instance[2040393];
		string inItemNameText7 = TextDatabase.instance[2040328].Replace("<ARENA_3_NAME>", newValue3);
		string inItemNameText8 = TextDatabase.instance[2040329].Replace("<ARENA_3_NAME>", newValue3);
		string inItemNameText9 = TextDatabase.instance[2040330].Replace("<ARENA_3_NAME>", newValue3);
		list.Add(Get_Arena3_HighScore(inItemNameText7, inPlayerData, inFriendsData));
		list.Add(Get_Arena3_GameTime(inItemNameText8, inPlayerData, inFriendsData, E_Better.Bigger));
		list.Add(Get_Arena3_Played(inItemNameText9, inPlayerData, inFriendsData));
		string newValue4 = TextDatabase.instance[2040394];
		string inItemNameText10 = TextDatabase.instance[2040331].Replace("<ARENA_4_NAME>", newValue4);
		string inItemNameText11 = TextDatabase.instance[2040332].Replace("<ARENA_4_NAME>", newValue4);
		string inItemNameText12 = TextDatabase.instance[2040333].Replace("<ARENA_4_NAME>", newValue4);
		list.Add(Get_Arena4_HighScore(inItemNameText10, inPlayerData, inFriendsData));
		list.Add(Get_Arena4_GameTime(inItemNameText11, inPlayerData, inFriendsData, E_Better.Bigger));
		list.Add(Get_Arena4_Played(inItemNameText12, inPlayerData, inFriendsData));
		return list;
	}

	private static Item Get_Experiance(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Experience);
	}

	private static Item Get_Money(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.TotalMoney);
	}

	private static Item Get_Gold(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.TotalGold);
	}

	private static Item Get_Kills(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Kills);
	}

	private static Item Get_Deaths(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Smaller, (PlayerPersistentInfoData val) => val.Params.Deaths);
	}

	private static Item Get_HeadShots(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.HeadShots);
	}

	private static Item Get_Carnages(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Carnage);
	}

	private static Item Get_Limbs(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Limbs);
	}

	private static Item Get_PlayedMissions(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.MissionCount);
	}

	private static Item Get_MissionSuccess(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameTextID, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.MissionSuccess);
	}

	private static Item _GetIntItem(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter, IntExtractor inExtractor)
	{
		IntItem intItem = new IntItem();
		intItem.m_NameIndex = inItemNameTextID;
		return _GetIntItem(intItem, inPlayerData, inFriendsData, inBetter, inExtractor);
	}

	private static Item _GetIntItem(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter, IntExtractor inExtractor)
	{
		IntItem intItem = new IntItem();
		intItem.m_NameText = inItemNameText;
		intItem.m_NameIndex = 0;
		return _GetIntItem(intItem, inPlayerData, inFriendsData, inBetter, inExtractor);
	}

	private static Item _GetIntItem(IntItem inItem, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter, IntExtractor inExtractor)
	{
		inItem.m_PlayerValue = inExtractor(inPlayerData);
		inItem.m_SecondValue = -1;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			int num = inExtractor(inFriendsDatum.m_PPIData);
			if (num > inItem.m_SecondValue)
			{
				inItem.m_SecondValue = num;
				inItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			inItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && inItem.m_PlayerValue > inItem.m_SecondValue) || (inBetter == E_Better.Smaller && inItem.m_PlayerValue < inItem.m_SecondValue);
			inItem.m_HighlightFriend = (inBetter == E_Better.Bigger && inItem.m_PlayerValue < inItem.m_SecondValue) || (inBetter == E_Better.Smaller && inItem.m_PlayerValue > inItem.m_SecondValue);
		}
		return inItem;
	}

	private static Item Get_GameTime(int inItemNameTextID, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter)
	{
		StringItem stringItem = new StringItem();
		stringItem.m_NameIndex = inItemNameTextID;
		float gameTime = inPlayerData.Params.GameTime;
		float num = -1f;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			float gameTime2 = inFriendsDatum.m_PPIData.Params.GameTime;
			if (gameTime2 > num)
			{
				num = gameTime2;
				stringItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			stringItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && gameTime > num) || (inBetter == E_Better.Smaller && gameTime < num);
			stringItem.m_HighlightFriend = (inBetter == E_Better.Bigger && gameTime < num) || (inBetter == E_Better.Smaller && gameTime > num);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(gameTime);
		stringItem.m_PlayerValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		timeSpan = TimeSpan.FromSeconds(num);
		stringItem.m_SecondValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		return stringItem;
	}

	private static Item Get_Arena1_HighScore(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena1HiScore);
	}

	private static Item Get_Arena1_Played(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena1Played);
	}

	private static Item Get_Arena1_GameTime(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter)
	{
		StringItem stringItem = new StringItem();
		stringItem.m_NameIndex = 0;
		stringItem.m_NameText = inItemNameText;
		float arena1Time = inPlayerData.Params.Arena1Time;
		float num = -1f;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			float arena1Time2 = inFriendsDatum.m_PPIData.Params.Arena1Time;
			if (arena1Time2 > num)
			{
				num = arena1Time2;
				stringItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			stringItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && arena1Time > num) || (inBetter == E_Better.Smaller && arena1Time < num);
			stringItem.m_HighlightFriend = (inBetter == E_Better.Bigger && arena1Time < num) || (inBetter == E_Better.Smaller && arena1Time > num);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(arena1Time);
		stringItem.m_PlayerValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		timeSpan = TimeSpan.FromSeconds(num);
		stringItem.m_SecondValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		return stringItem;
	}

	private static Item Get_Arena2_HighScore(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena2HiScore);
	}

	private static Item Get_Arena2_Played(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena2Played);
	}

	private static Item Get_Arena2_GameTime(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter)
	{
		StringItem stringItem = new StringItem();
		stringItem.m_NameIndex = 0;
		stringItem.m_NameText = inItemNameText;
		float arena2Time = inPlayerData.Params.Arena2Time;
		float num = -1f;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			float arena2Time2 = inFriendsDatum.m_PPIData.Params.Arena2Time;
			if (arena2Time2 > num)
			{
				num = arena2Time2;
				stringItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			stringItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && arena2Time > num) || (inBetter == E_Better.Smaller && arena2Time < num);
			stringItem.m_HighlightFriend = (inBetter == E_Better.Bigger && arena2Time < num) || (inBetter == E_Better.Smaller && arena2Time > num);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(arena2Time);
		stringItem.m_PlayerValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		timeSpan = TimeSpan.FromSeconds(num);
		stringItem.m_SecondValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		return stringItem;
	}

	private static Item Get_Arena3_HighScore(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena3HiScore);
	}

	private static Item Get_Arena3_Played(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena3Played);
	}

	private static Item Get_Arena3_GameTime(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter)
	{
		StringItem stringItem = new StringItem();
		stringItem.m_NameIndex = 0;
		stringItem.m_NameText = inItemNameText;
		float arena3Time = inPlayerData.Params.Arena3Time;
		float num = -1f;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			float arena3Time2 = inFriendsDatum.m_PPIData.Params.Arena3Time;
			if (arena3Time2 > num)
			{
				num = arena3Time2;
				stringItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			stringItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && arena3Time > num) || (inBetter == E_Better.Smaller && arena3Time < num);
			stringItem.m_HighlightFriend = (inBetter == E_Better.Bigger && arena3Time < num) || (inBetter == E_Better.Smaller && arena3Time > num);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(arena3Time);
		stringItem.m_PlayerValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		timeSpan = TimeSpan.FromSeconds(num);
		stringItem.m_SecondValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		return stringItem;
	}

	private static Item Get_Arena4_HighScore(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena4HiScore);
	}

	private static Item Get_Arena4_Played(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData)
	{
		return _GetIntItem(inItemNameText, inPlayerData, inFriendsData, E_Better.Bigger, (PlayerPersistentInfoData val) => val.Params.Arena4Played);
	}

	private static Item Get_Arena4_GameTime(string inItemNameText, PlayerPersistentInfoData inPlayerData, List<FriendList.FriendInfo> inFriendsData, E_Better inBetter)
	{
		StringItem stringItem = new StringItem();
		stringItem.m_NameIndex = 0;
		stringItem.m_NameText = inItemNameText;
		float arena4Time = inPlayerData.Params.Arena4Time;
		float num = -1f;
		foreach (FriendList.FriendInfo inFriendsDatum in inFriendsData)
		{
			float arena4Time2 = inFriendsDatum.m_PPIData.Params.Arena4Time;
			if (arena4Time2 > num)
			{
				num = arena4Time2;
				stringItem.m_SecondValueFriendName = inFriendsDatum.m_Name;
			}
		}
		if (inBetter != 0 && inFriendsData.Count > 0)
		{
			stringItem.m_HighlightPlayer = (inBetter == E_Better.Bigger && arena4Time > num) || (inBetter == E_Better.Smaller && arena4Time < num);
			stringItem.m_HighlightFriend = (inBetter == E_Better.Bigger && arena4Time < num) || (inBetter == E_Better.Smaller && arena4Time > num);
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(arena4Time);
		stringItem.m_PlayerValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		timeSpan = TimeSpan.FromSeconds(num);
		stringItem.m_SecondValue = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
		return stringItem;
	}
}
