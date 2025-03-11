using System.Collections.Generic;

internal static class Kontagent
{
	public const string Root_User = "User";

	public const string Root_Game = "Game";

	public const string Root_iAP = "iAP";

	public const string Branch_Game_Equip = "Equip";

	public const string Branch_Game_Death = "Death";

	public const string Branch_Game_UseConsumable = "Consumable";

	public const string Branch_Game_Arena = "Equip";

	public const string Branch_Game_Equip_Upgrades = "Upgrades";

	public const string Branch_Game_Equip_Weapons = "Weapons";

	public const string Branch_Game_Equip_Gadgets = "Gadgets";

	public const string UserRankPos = "User_Rank_Position";

	public const string UserRankComplete = "User_Rank_";

	public const string Weapon = "Weapon";

	public const string Gadgets = "Gadget";

	public const string Upgrades = "Upgrade";

	public const string MissionTime = "MissionTime";

	public const string ArenaTime = "ArenaTime";

	public const string DeadByEnemy = "ByEnemy";

	public const string Branch_iAP_Buy = "Buy";

	public const string iApp = "iAPP ";

	private const string Rank = "l";

	private const string Default = "v";

	private const string Root = "st1";

	private const string Branch = "st2";

	private const string SubBranch = "st3";

	private static bool test;

	private static bool log;

	private static bool SessionRunning;

	public static string API_KEY
	{
		get
		{
			if (test)
			{
				return "13b334dd9693475980c1548360f89624";
			}
			return "0a47602f7cb2411e9d7e0867a27c3cd6";
		}
	}

	public static void StartSession()
	{
		if (!SessionRunning)
		{
			KontagentBinding.startSession(API_KEY, log);
			SessionRunning = true;
		}
	}

	public static void StopSession()
	{
		if (SessionRunning)
		{
			KontagentBinding.stopSession();
			SessionRunning = false;
		}
	}

	public static void Monetization(int cents)
	{
		KontagentBinding.revenueTracking(cents, null);
	}

	public static void SendEquipToKontagent(PlayerPersistantInfo ppi)
	{
		PPIEquipList equipList = ppi.EquipList;
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			SendCustomEvent(weapon.ID.ToString(), "Game", "Equip", "Weapons", ppi.rank, weapon.EquipSlotIdx);
		}
		foreach (PPIItemData item in equipList.Items)
		{
			SendCustomEvent(item.ID.ToString(), "Game", "Equip", "Gadgets", ppi.rank, item.EquipSlotIdx);
		}
		foreach (PPIUpgradeList.UpgradeData upgrade in ppi.Upgrades.Upgrades)
		{
			SendCustomEvent(upgrade.ID.ToString(), "Game", "Equip", "Upgrades", ppi.rank);
		}
	}

	public static void SendCustomEvent(string name, string root, string branch, string subBranch, int rank, int defaultValue = 1)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (root != null)
		{
			dictionary.Add("st1", root);
			if (branch != null)
			{
				dictionary.Add("st2", branch);
			}
			if (subBranch != null)
			{
				dictionary.Add("st3", subBranch);
			}
		}
		dictionary.Add("l", rank.ToString());
		dictionary.Add("v", defaultValue.ToString());
		KontagentBinding.customEvent(name, dictionary);
	}
}
