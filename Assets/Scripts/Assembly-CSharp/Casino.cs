using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casino : MonoBehaviour
{
	public enum PrizeType
	{
		Money = 0,
		Gold = 1,
		Chip = 2,
		Gadget = 3
	}

	public class Prize
	{
		private CasinoSlotMachine.Symbols m_Id;

		private PrizeType m_PrizeType;

		private GUIBase_Widget m_Icon;

		private int m_Name;

		private bool m_IsJackpot;

		private float m_Probability;

		private int m_Amount;

		private E_ItemID m_Item;

		public GUIBase_Widget icon
		{
			get
			{
				return m_Icon;
			}
		}

		public int name
		{
			get
			{
				return m_Name;
			}
		}

		public int amount
		{
			get
			{
				return m_Amount;
			}
		}

		public CasinoSlotMachine.Symbols id
		{
			get
			{
				return m_Id;
			}
		}

		public int coolDown { get; set; }

		public Prize(CasinoSlotMachine.Symbols id, PrizeType prizeType, GUIBase_Widget icon, int name, float probability, int coolDownInit, int amount, E_ItemID item = E_ItemID.None)
		{
			m_Id = id;
			m_PrizeType = prizeType;
			m_Name = name;
			m_Icon = icon;
			m_Probability = probability;
			coolDown = coolDownInit;
			m_Amount = amount;
			m_Item = item;
			if (m_PrizeType == PrizeType.Gadget)
			{
				Settings<E_ItemID> settings = ItemSettingsManager.Instance.Get(item);
				m_Name = settings.Name;
				m_Icon = settings.ShopWidget;
			}
		}

		public void ApplyReward()
		{
			switch (m_PrizeType)
			{
			case PrizeType.Gold:
				Game.Instance.PlayerPersistentInfo.AddGold(amount);
				break;
			case PrizeType.Money:
				Game.Instance.PlayerPersistentInfo.AddMoney(amount);
				break;
			case PrizeType.Chip:
				Game.Instance.PlayerPersistentInfo.AddTicket(amount);
				break;
			case PrizeType.Gadget:
				Game.Instance.PlayerPersistentInfo.InventoryAddItem(m_Item, m_Amount);
				if ((bool)GuiEquipMenu.Instance)
				{
					GuiEquipMenu.Instance.TryEquipBoughtItem(new ShopItemId((int)m_Item, GuiShop.E_ItemType.Item));
				}
				break;
			default:
				Debug.LogWarning("Unknown enum");
				break;
			}
			if (id == CasinoSlotMachine.Symbols.Jackpot)
			{
				coolDown += 8;
			}
			else if (id == CasinoSlotMachine.Symbols.Big1)
			{
				coolDown += 4;
			}
			else if (id == CasinoSlotMachine.Symbols.Medium1 || id == CasinoSlotMachine.Symbols.Medium2)
			{
				coolDown += 2;
			}
		}

		public float GetProbability()
		{
			if (coolDown > 0)
			{
				return 0f;
			}
			return m_Probability;
		}

		public void DecreaseCollDown()
		{
			if (coolDown > 0)
			{
				coolDown--;
			}
		}
	}

	private static CasinoGui m_CasinoGui;

	private static CasinoSlotMachine m_SlotMachine;

	private static bool m_IsVisible = false;

	private static bool m_IsInitialized = false;

	private static Prize m_Jackpot;

	private static Prize m_Winning;

	private static System.Random m_Random = new System.Random();

	private static bool m_PlaysFirstTime = true;

	private static bool m_DailyBonusActive = false;

	private static List<Prize> m_Prizes = new List<Prize>();

	public void Init()
	{
		m_CasinoGui = new CasinoGui();
		m_IsInitialized = true;
	}

	public void InitializeRewards()
	{
		m_Prizes.Clear();
		E_FundID key = E_FundID.Gold099;
		Settings<E_FundID> settings = FundSettingsManager.Instance.Get(key);
		E_FundID key2 = E_FundID.Money299;
		Settings<E_FundID> settings2 = FundSettingsManager.Instance.Get(key2);
		string text = "Items/GuiCasinoChip";
		GameObject gameObject = Resources.Load(text) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("Cant find prefab: " + text);
		}
		float value = UnityEngine.Random.value;
		E_ItemID item;
		int amount;
		if (value <= 0.5f)
		{
			item = E_ItemID.Bandage;
			amount = 4;
		}
		else
		{
			item = E_ItemID.MedKit;
			amount = 2;
		}
		value = UnityEngine.Random.value;
		E_ItemID item2;
		int amount2;
		if (value <= 0.5f)
		{
			item2 = E_ItemID.CutterLaser;
			amount2 = 2;
		}
		else
		{
			item2 = E_ItemID.CutterMechanical;
			amount2 = 2;
		}
		value = UnityEngine.Random.value;
		E_ItemID item3;
		int amount3;
		if (value <= 0.5f)
		{
			item3 = E_ItemID.ReviveKit;
			amount3 = 2;
		}
		else
		{
			item3 = E_ItemID.Turret;
			amount3 = 2;
		}
		Prize item4 = new Prize(CasinoSlotMachine.Symbols.First, PrizeType.Money, settings2.ShopWidget, 1010140, 17f, 0, GameplayData.Instance.MoneyPerZombie() * 30);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Small2, PrizeType.Money, settings2.ShopWidget, 1010140, 28f, 0, GameplayData.Instance.MoneyPerZombie() * ((!(UnityEngine.Random.value <= 0.5f)) ? 25 : 20));
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Small3, PrizeType.Gadget, null, 0, 11f, 0, amount, item);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Small4, PrizeType.Gadget, null, 0, 8f, 0, amount2, item2);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Small5, PrizeType.Gadget, null, 0, 8f, 0, amount3, item3);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Medium1, PrizeType.Money, settings2.ShopWidget, 1010140, 16f, 0, GameplayData.Instance.MoneyPerZombie() * 50);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Medium2, PrizeType.Gold, settings.ShopWidget, 1010145, 10f, 1, 5);
		m_Prizes.Add(item4);
		item4 = new Prize(CasinoSlotMachine.Symbols.Big1, PrizeType.Gold, settings.ShopWidget, 1010145, 2.5f, 3, 10);
		m_Prizes.Add(item4);
		item4 = (m_Jackpot = new Prize(CasinoSlotMachine.Symbols.Jackpot, PrizeType.Gold, settings.ShopWidget, 1010145, 0.25f, 5, 200));
		m_Prizes.Add(item4);
	}

	public void Show(GUIBase_Button.TouchDelegate closeDelegate)
	{
		if (!m_IsVisible)
		{
			m_IsVisible = true;
			InitializeRewards();
			m_CasinoGui.ShowMainHud(BuyChips, Spin, closeDelegate, m_Jackpot);
			CheckOutOfChips();
		}
	}

	public void Hide()
	{
		if (m_IsVisible)
		{
			m_IsVisible = false;
			m_CasinoGui.HideMainHud();
			m_SlotMachine.Reset();
		}
	}

	public void ActivateDailyReward()
	{
		m_DailyBonusActive = true;
	}

	public void Save(IDataFile file)
	{
		file.SetInt("CASINO_played", m_PlaysFirstTime ? 1 : 0);
		file.SetInt("CASINO_dailyBonus", m_DailyBonusActive ? 1 : 0);
		int num = 1;
		foreach (Prize prize in m_Prizes)
		{
			string key = "Casino_RewardId_" + num;
			file.SetInt(key, (int)prize.id);
			key = "Casino_RewardCooldown_" + num;
			file.SetInt(key, prize.coolDown);
			num++;
		}
	}

	public void Load(IDataFile file)
	{
		m_PlaysFirstTime = file.GetInt("CASINO_played", 1) > 0;
		m_DailyBonusActive = file.GetInt("CASINO_dailyBonus") > 0;
		int num = 1;
		while (true)
		{
			string key = "Casino_RewardId_" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			CasinoSlotMachine.Symbols @int = (CasinoSlotMachine.Symbols)file.GetInt(key);
			key = "Casino_RewardCooldown_" + num;
			int int2 = file.GetInt(key);
			foreach (Prize prize in m_Prizes)
			{
				if (prize.id == @int)
				{
					prize.coolDown = int2;
					break;
				}
			}
			num++;
		}
	}

	private static bool SpinEnabled()
	{
		return !m_SlotMachine.IsBusy();
	}

	private static float GetLoseLimit()
	{
		if (m_DailyBonusActive)
		{
			m_DailyBonusActive = false;
			return 0.5f;
		}
		return 0.75f;
	}

	private static Prize GetRandomPrize()
	{
		float num = 0f;
		foreach (Prize prize in m_Prizes)
		{
			num += prize.GetProbability();
		}
		float num2 = (float)m_Random.NextDouble() * num;
		num = 0f;
		foreach (Prize prize2 in m_Prizes)
		{
			float num3 = num;
			num += prize2.GetProbability();
			if (num2 >= num3 && num2 < num && prize2.GetProbability() > 1E-05f)
			{
				return prize2;
			}
		}
		return null;
	}

	private static CasinoSlotMachine.Symbols GenerateReward(out bool win)
	{
		float num = (float)m_Random.NextDouble();
		if (!m_PlaysFirstTime && num < GetLoseLimit())
		{
			win = false;
			return (CasinoSlotMachine.Symbols)UnityEngine.Random.Range(0, 9);
		}
		win = true;
		m_PlaysFirstTime = false;
		m_Winning = GetRandomPrize();
		return m_Winning.id;
	}

	private void BuyChips()
	{
		if (Game.Instance.PlayerPersistentInfo.gold < 5)
		{
			m_CasinoGui.DisableHudControls(true);
			GuiShopNotFundsPopup.Instance.RequiredFunds = ShopDataBridge.Instance.FindFundsItem(5, true);
			GuiShopNotFundsPopup.Instance.RemoveFromTopHack = true;
			GuiMainMenu.Instance.ShowPopup("NotFundsPopup", string.Empty, string.Empty, NoFundsResultHandler);
		}
		else
		{
			AddChips();
		}
	}

	private void NoFundsResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			AddChips();
		}
		m_CasinoGui.DisableHudControls(false);
	}

	public static void AddChips(bool afterRewordVideo = false)
	{
		if (afterRewordVideo)
		{
			CheckOutOfChips(true);
			//Advertisement.Instance.StartCoroutine(WaitAndAddChips(0.45f));
		}
		else if (Game.Instance.PlayerPersistentInfo.gold >= 5)
		{
			Game.Instance.PlayerPersistentInfo.RemoveGold(5);
			Game.Instance.PlayerPersistentInfo.AddTicket(5);
			CheckOutOfChips();
			m_CasinoGui.RefreshChipsBuyState();
		}
	}

	public static IEnumerator WaitAndAddChips(float time)
	{
		yield return new WaitForSeconds(time);
		Game.Instance.PlayerPersistentInfo.AddTicket(1);
		CheckOutOfChips();
		m_CasinoGui.RefreshChipsBuyState();
	}

	private static void CheckOutOfChips(bool afterRewordVideo = false)
	{
		bool outOfChips = Game.Instance.PlayerPersistentInfo.numTickets <= 0;
		if (afterRewordVideo)
		{
			outOfChips = false;
		}
		m_CasinoGui.OutOfChips(outOfChips);
	}

	public static void Spin()
	{
		if (Game.Instance.PlayerPersistentInfo.numTickets <= 0 || !SpinEnabled())
		{
			return;
		}
		m_CasinoGui.SetBusy(true);
		bool win;
		CasinoSlotMachine.Symbols symbol = GenerateReward(out win);
		foreach (Prize prize in m_Prizes)
		{
			prize.DecreaseCollDown();
		}
		Game.Instance.PlayerPersistentInfo.RemoveTicket(1);
		CheckOutOfChips();
		CityManager.Instance.Save();
		m_SlotMachine.Spin(symbol, win);
	}

	private void Awake()
	{
		GameObject gameObject = GameObject.Find("slot_machine");
		m_SlotMachine = gameObject.GetComponentInChildren<CasinoSlotMachine>();
		CasinoSlotMachine slotMachine = m_SlotMachine;
		slotMachine.m_RewardCallback = (CasinoSlotMachine.ShowReward)Delegate.Combine(slotMachine.m_RewardCallback, new CasinoSlotMachine.ShowReward(ShowReward));
	}

	private void ShowReward()
	{
		if (m_Winning != null)
		{
			m_CasinoGui.DisableHudControls(true);
			m_CasinoGui.ShowSpecialReward(CloseReward, m_Winning);
		}
	}

	private void CloseReward()
	{
		if (m_Winning != null)
		{
			m_Winning.ApplyReward();
		}
		m_CasinoGui.HideSpecialReward();
		m_CasinoGui.DisableHudControls(false);
		CheckOutOfChips();
		CityManager.Instance.Save();
		m_Winning = null;
	}

	private void Update()
	{
		if (m_IsInitialized && m_IsVisible)
		{
			m_CasinoGui.SetBusy(!SpinEnabled());
		}
	}
}
