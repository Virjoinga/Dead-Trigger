using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class ShopDataBridge
{
	private class IAPTransactionVerifier : IAP.Verifier
	{
		private const string IAP_TRANSACTIONS = "IAPTRANSACTIONS";

		private string ProcessedTransactionIds;

		private byte[] modulus = new byte[257]
		{
			0, 171, 116, 135, 152, 106, 109, 242, 224, 142,
			64, 46, 175, 184, 34, 115, 239, 144, 84, 50,
			160, 128, 250, 201, 213, 252, 155, 68, 49, 200,
			68, 243, 194, 6, 26, 116, 221, 203, 117, 94,
			220, 166, 97, 137, 59, 105, 127, 216, 16, 118,
			241, 208, 187, 61, 23, 2, 0, 169, 155, 19,
			191, 104, 15, 38, 33, 64, 145, 128, 226, 34,
			37, 101, 11, 106, 66, 188, 1, 251, 6, 196,
			121, 198, 155, 33, 182, 203, 78, 82, 159, 2,
			58, 188, 92, 125, 204, 66, 183, 114, 111, 165,
			34, 20, 20, 79, 25, 172, 8, 89, 231, 74,
			129, 173, 102, 33, 146, 128, 104, 209, 109, 123,
			94, 186, 199, 244, 119, 14, 98, 75, 117, 10,
			224, 57, 224, 83, 185, 33, 7, 95, 63, 123,
			133, 66, 56, 240, 178, 239, 162, 85, 99, 8,
			96, 21, 177, 250, 73, 231, 246, 10, 65, 6,
			196, 18, 194, 118, 163, 134, 26, 169, 190, 240,
			147, 174, 72, 26, 16, 169, 249, 174, 132, 21,
			52, 18, 46, 185, 142, 94, 189, 36, 159, 97,
			163, 115, 128, 168, 105, 19, 191, 208, 231, 144,
			186, 174, 157, 163, 184, 212, 233, 0, 244, 4,
			119, 120, 122, 155, 57, 17, 27, 165, 166, 167,
			15, 172, 32, 216, 107, 180, 99, 5, 170, 233,
			182, 95, 78, 177, 5, 140, 170, 173, 159, 22,
			227, 253, 79, 101, 44, 103, 161, 83, 160, 162,
			196, 12, 200, 23, 18, 95, 201
		};

		private byte[] exponent = new byte[3] { 1, 0, 1 };

		public IAPTransactionVerifier()
		{
			ProcessedTransactionIds = PlayerPrefs.GetString("IAPTRANSACTIONS", string.Empty);
		}

		public void Process(IAP.Transaction txn, Action<IAP.E_ProcessingState> callback)
		{
			VerifyTransaction(txn, delegate(IAP.E_ProcessingState result)
			{
				if (result == IAP.E_ProcessingState.Accepted && !ProcessedTransactionIds.Contains(txn.Id))
				{
					try
					{
						int guid = Convert.ToInt32(txn.ProductId);
						Instance.IAPProcessBoughtItem(guid);
					}
					catch
					{
						result = IAP.E_ProcessingState.Rejected;
					}
					finally
					{
						IAPTransactionVerifier iAPTransactionVerifier = this;
						iAPTransactionVerifier.ProcessedTransactionIds = iAPTransactionVerifier.ProcessedTransactionIds + txn.Id + ";";
						PlayerPrefs.SetString("IAPTRANSACTIONS", ProcessedTransactionIds);
					}
				}
				callback(result);
			});
		}

		private void ReportGoogleTransactionToAnalytics(IAPShopGooglePlay.GooglePlayTransaction txn)
		{
			//UnityAnalyticsWrapper.ReportInAppPurchase(txn.Native.ProductId, txn.Product.Price, txn.Product.CurrencyCode, txn.Native.Receipt, txn.Native.Signature);
		}

		private void VerifyTransaction(IAP.Transaction txn, Action<IAP.E_ProcessingState> callback)
		{
			IAPShopGooglePlay.GooglePlayTransaction googlePlayTransaction = txn as IAPShopGooglePlay.GooglePlayTransaction;
			if (googlePlayTransaction == null || googlePlayTransaction.Native == null || string.IsNullOrEmpty(googlePlayTransaction.Native.Receipt) || string.IsNullOrEmpty(googlePlayTransaction.Native.Signature))
			{
				callback(IAP.E_ProcessingState.Rejected);
				return;
			}
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(googlePlayTransaction.Native.Receipt);
				byte[] rgbSignature = Convert.FromBase64String(googlePlayTransaction.Native.Signature);
				SHA1 sHA = SHA1.Create();
				byte[] rgbHash = sHA.ComputeHash(bytes);
				RSAParameters parameters = default(RSAParameters);
				parameters.Modulus = modulus;
				parameters.Exponent = exponent;
				RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
				rSACryptoServiceProvider.ImportParameters(parameters);
				RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
				rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
				if (rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature))
				{
					callback(IAP.E_ProcessingState.Accepted);
					ReportGoogleTransactionToAnalytics(googlePlayTransaction);
				}
				else
				{
					callback(IAP.E_ProcessingState.Rejected);
				}
			}
			catch
			{
				callback(IAP.E_ProcessingState.CantVerify);
			}
		}
	}

	public struct UpgradeInfo
	{
		public int Value;

		public int Level;

		public int MaxLevel;
	}

	public static ShopDataBridge Instance;

	private bool m_IAPInitInProgress;

	private IAPTransactionVerifier m_Verifier = new IAPTransactionVerifier();

	public PlayerPersistantInfo PPI
	{
		get
		{
			return Game.Instance.PlayerPersistentInfo;
		}
	}

	public GUIBase_Widget MissingWidget { get; private set; }

	public int PlayerXP
	{
		get
		{
			return PPI.experience;
		}
	}

	public int PlayerLevel
	{
		get
		{
			return PPI.rank;
		}
	}

	public int PlayerGold
	{
		get
		{
			return PPI.gold;
		}
	}

	public int PlayerMoney
	{
		get
		{
			return PPI.money;
		}
	}

	private ShopDataBridge()
	{
		IAPInit();
	}

	public static void CreateInstance()
	{
		Instance = new ShopDataBridge();
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("Gui/EmptyWidget")) as GameObject;
		Instance.MissingWidget = gameObject.GetComponent<GUIBase_Widget>();
	}

	private ShopItemInfo CreateEmptySlotInfo()
	{
		ShopItemInfo shopItemInfo = new ShopItemInfo();
		shopItemInfo.NameTextId = 210000;
		shopItemInfo.EmptySlot = true;
		return shopItemInfo;
	}

	public List<ShopItemId> GetOwnedWeapons()
	{
		List<ShopItemId> list = new List<ShopItemId>();
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIWeaponData> weapons = inventoryList.Weapons;
		foreach (PPIWeaponData item2 in weapons)
		{
			ShopItemId item = new ShopItemId((int)item2.ID, GuiShop.E_ItemType.Weapon);
			list.Add(item);
		}
		return list;
	}

	public List<ShopItemId> GetOwnedItems()
	{
		List<ShopItemId> list = new List<ShopItemId>();
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIItemData> items = inventoryList.Items;
		foreach (PPIItemData item2 in items)
		{
			ShopItemId item = new ShopItemId((int)item2.ID, GuiShop.E_ItemType.Item);
			list.Add(item);
		}
		return list;
	}

	public List<ShopItemId> GetWeapons()
	{
		WeaponSettings[] all = WeaponSettingsManager.Instance.GetAll();
		List<ShopItemId> list = new List<ShopItemId>();
		WeaponSettings[] array = all;
		foreach (WeaponSettings weaponSettings in array)
		{
			if (!weaponSettings.DISABLED)
			{
				list.Add(new ShopItemId((int)weaponSettings.ID, GuiShop.E_ItemType.Weapon));
			}
		}
		return list;
	}

	public List<ShopItemId> GetItems()
	{
		ItemSettings[] all = ItemSettingsManager.Instance.GetAll();
		List<ShopItemId> list = new List<ShopItemId>();
		ItemSettings[] array = all;
		foreach (ItemSettings itemSettings in array)
		{
			if (!itemSettings.DISABLED)
			{
				list.Add(new ShopItemId((int)itemSettings.ID, GuiShop.E_ItemType.Item));
			}
		}
		return list;
	}

	public List<ShopItemId> GetItemsTutorial()
	{
		List<ShopItemId> list = new List<ShopItemId>();
		list.Add(new ShopItemId(1, GuiShop.E_ItemType.Item));
		return list;
	}

	public List<ShopItemId> GetFunds()
	{
		FundSettings[] all = FundSettingsManager.Instance.GetAll();
		List<ShopItemId> list = new List<ShopItemId>();
		FundSettings[] array = all;
		foreach (FundSettings fundSettings in array)
		{
			if (!fundSettings.DISABLED && fundSettings.ID != E_FundID.TapJoyInApp && fundSettings.ID != E_FundID.TapJoyWeb)
			{
				list.Add(new ShopItemId((int)fundSettings.ID, GuiShop.E_ItemType.Fund));
			}
		}
		return list;
	}

	public List<ShopItemId> GetUpgrades()
	{
		UpgradeSettings[] all = UpgradeSettingsManager.Instance.GetAll();
		List<ShopItemId> list = new List<ShopItemId>();
		UpgradeSettings[] array = all;
		foreach (UpgradeSettings upgradeSettings in array)
		{
			if (!upgradeSettings.DISABLED && upgradeSettings.ID != E_UpgradeID.ItemSlot1 && upgradeSettings.ID != 0)
			{
				list.Add(new ShopItemId((int)upgradeSettings.ID, GuiShop.E_ItemType.Upgrade));
			}
		}
		return list;
	}

	public ShopItemInfo GetItemInfo(ShopItemId itemId)
	{
		if (itemId == ShopItemId.EmptyId)
		{
			return CreateEmptySlotInfo();
		}
		switch (itemId.ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			return GetShopWeaponInfo(itemId.Id);
		case GuiShop.E_ItemType.Item:
			return GetShopItemInfo(itemId.Id);
		case GuiShop.E_ItemType.Fund:
			return GetShopFundInfo(itemId.Id);
		case GuiShop.E_ItemType.Upgrade:
			return GetShopUpgradeInfo(itemId.Id);
		default:
			Debug.LogError("TODO: Unsupported type" + itemId.ItemType);
			return CreateEmptySlotInfo();
		}
	}

	private PPIWeaponData GetOwnedWeaponData(E_WeaponID id)
	{
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIWeaponData> weapons = inventoryList.Weapons;
		foreach (PPIWeaponData item in weapons)
		{
			if (item.ID == id)
			{
				return item;
			}
		}
		return default(PPIWeaponData);
	}

	private void SetOwnedWeaponUpgrade(E_WeaponID id, int upgLevel)
	{
		bool flag = false;
		List<PPIWeaponData> weapons = PPI.InventoryList.Weapons;
		for (int i = 0; i < weapons.Count; i++)
		{
			PPIWeaponData value = weapons[i];
			if (value.ID == id)
			{
				value.UpgradeLevel = (E_UpgradeLevel)upgLevel;
				weapons[i] = value;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogError("Weapon not found in inventory: " + id);
			return;
		}
		List<PPIWeaponData> weapons2 = PPI.EquipList.Weapons;
		for (int j = 0; j < weapons2.Count; j++)
		{
			PPIWeaponData value2 = weapons2[j];
			if (value2.ID == id)
			{
				value2.UpgradeLevel = (E_UpgradeLevel)upgLevel;
				weapons2[j] = value2;
				break;
			}
		}
	}

	private void SetWeaponSlot(E_WeaponID id, int slot)
	{
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIWeaponData> weapons = inventoryList.Weapons;
		for (int i = 0; i < weapons.Count; i++)
		{
			PPIWeaponData value = weapons[i];
			if (value.ID == id)
			{
				value.EquipSlotIdx = slot;
				weapons[i] = value;
				return;
			}
		}
		Debug.LogError("Weapon not found in inventory: " + id);
	}

	internal PPIItemData GetOwnedItemData(E_ItemID id)
	{
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIItemData> items = inventoryList.Items;
		foreach (PPIItemData item in items)
		{
			if (item.ID == id)
			{
				return item;
			}
		}
		return default(PPIItemData);
	}

	private void SetOwnedItemCount(E_ItemID id, int newCount)
	{
		List<PPIItemData> items = PPI.InventoryList.Items;
		bool flag = false;
		for (int i = 0; i < items.Count; i++)
		{
			PPIItemData value = items[i];
			if (value.ID == id)
			{
				value.Count = newCount;
				items[i] = value;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogError("Item not found in inventory: " + id);
			return;
		}
		List<PPIItemData> items2 = PPI.EquipList.Items;
		for (int j = 0; j < items2.Count; j++)
		{
			PPIItemData value2 = items2[j];
			if (value2.ID == id)
			{
				value2.Count = newCount;
				items2[j] = value2;
				break;
			}
		}
	}

	private void SetItemSlot(E_ItemID id, int slot)
	{
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIItemData> items = inventoryList.Items;
		for (int i = 0; i < items.Count; i++)
		{
			PPIItemData value = items[i];
			if (value.ID == id)
			{
				value.EquipSlotIdx = slot;
				items[i] = value;
				return;
			}
		}
		Debug.LogError("Item not found in inventory: " + id);
	}

	public bool HasOwnedUpgrade(E_UpgradeID id)
	{
		PPIUpgradeList upgrades = PPI.Upgrades;
		List<PPIUpgradeList.UpgradeData> upgrades2 = upgrades.Upgrades;
		foreach (PPIUpgradeList.UpgradeData item in upgrades2)
		{
			if (item.ID == id)
			{
				return true;
			}
		}
		return false;
	}

	private int DmgPerSec(WeaponSettings.Data wpnData)
	{
		float num = ((!(wpnData.FireTime > 1f)) ? (wpnData.Damage / wpnData.FireTime) : wpnData.Damage);
		if (wpnData.FireTime > 0.4f)
		{
			num *= 1.3f;
		}
		int num2 = (int)num;
		if (num2 < 100)
		{
			return (num2 + 2) / 5 * 5;
		}
		return (num2 + 5) / 10 * 10;
	}

	private int Accuracy(WeaponSettings.Data wpnData)
	{
		float num = wpnData.Dispersion + wpnData.DispersionAddShoot + wpnData.DispersionAddMove / 2f;
		float num2 = Mathf.Clamp(0.1f + (1f - num / 22f) * 0.9f, 0.1f, 1f);
		return (int)(num2 * 100f);
	}

	private int Range(WeaponSettings.Data wpnData)
	{
		return (int)(wpnData.EffectiveRange + (wpnData.MaxRange - wpnData.EffectiveRange) * 0.33f);
	}

	internal ShopItemInfo GetShopWeaponInfo(int id)
	{
		WeaponSettings weaponSettings = WeaponSettingsManager.Instance.Get((E_WeaponID)id);
		if (weaponSettings != null)
		{
			ShopItemInfo shopItemInfo = new ShopItemInfo();
			shopItemInfo.NameTextId = weaponSettings.Name;
			shopItemInfo.Description = weaponSettings.Description;
			shopItemInfo.SpriteWidget = ((!(weaponSettings.ShopWidget == null)) ? weaponSettings.ShopWidget : MissingWidget);
			shopItemInfo.Locked = weaponSettings.Rank > PlayerLevel;
			shopItemInfo.RequiredLevel = weaponSettings.Rank;
			shopItemInfo.JustUnlocked = shopItemInfo.RequiredLevel == PlayerLevel;
			shopItemInfo.NewInShop = weaponSettings.NewInShop;
			PPIWeaponData ownedWeaponData = GetOwnedWeaponData((E_WeaponID)id);
			if (ownedWeaponData.IsValid())
			{
				shopItemInfo.Upgrade = (int)ownedWeaponData.UpgradeLevel;
				shopItemInfo.Owned = true;
			}
			shopItemInfo.MaxUpgrade = weaponSettings.Upgrades.Length - 1;
			shopItemInfo.NextUpgrade = Mathf.Min(shopItemInfo.Upgrade + 1, shopItemInfo.MaxUpgrade);
			WeaponSettings.Data data = weaponSettings.Upgrades[shopItemInfo.Upgrade];
			WeaponSettings.Data data2 = weaponSettings.Upgrades[shopItemInfo.NextUpgrade];
			shopItemInfo.GoldCurrency = weaponSettings.GoldCost > 0;
			shopItemInfo.Cost = ((!shopItemInfo.GoldCurrency) ? weaponSettings.MoneyCost : weaponSettings.GoldCost);
			shopItemInfo.UpgradeGoldCurrency = data2.GoldCost > 0;
			shopItemInfo.UpgradeCost = ((!shopItemInfo.UpgradeGoldCurrency) ? data2.MoneyCost : data2.GoldCost);
			shopItemInfo.UpgradeRank = data2.UpgradeRank;
			bool flag = shopItemInfo.UpgradeRank > Instance.PlayerLevel;
			shopItemInfo.UpgradeMaxed = shopItemInfo.Upgrade >= shopItemInfo.MaxUpgrade;
			shopItemInfo.UpgradeReady = shopItemInfo.Owned && !shopItemInfo.Locked && !shopItemInfo.UpgradeMaxed && !flag;
			shopItemInfo.WeaponDamage = DmgPerSec(data);
			shopItemInfo.WeaponDamageMax = DmgPerSec(data2);
			shopItemInfo.WeaponClip = data.MaxAmmoInClip;
			shopItemInfo.WeaponClipMax = data2.MaxAmmoInClip;
			shopItemInfo.WeaponAccuracy = Accuracy(data);
			shopItemInfo.WeaponAccuracyMax = Accuracy(data2);
			shopItemInfo.WeaponRange = Range(data);
			shopItemInfo.WeaponRangeMax = Range(data2);
			if (weaponSettings.SaleInPercent > 0)
			{
				shopItemInfo.PriceSale = true;
				shopItemInfo.DiscountTag = TextDatabase.instance[2030065];
				shopItemInfo.DiscountTag = shopItemInfo.DiscountTag.Replace("%d1", weaponSettings.SaleInPercent.ToString());
				shopItemInfo.DiscountTagSmall = TextDatabase.instance[2030066];
				shopItemInfo.DiscountTagSmall = shopItemInfo.DiscountTagSmall.Replace("%d1", weaponSettings.SaleInPercent.ToString());
				shopItemInfo.CostBeforeSale = shopItemInfo.Cost;
				int b = (int)((float)shopItemInfo.Cost * (1f - (float)weaponSettings.SaleInPercent * 0.01f));
				shopItemInfo.Cost = Mathf.Max(0, b);
			}
			return shopItemInfo;
		}
		Debug.LogError("Weapon not found: " + id);
		return new ShopItemInfo();
	}

	internal ShopItemInfo GetShopItemInfo(int id)
	{
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get((E_ItemID)id);
		if (itemSettings != null)
		{
			ShopItemInfo shopItemInfo = new ShopItemInfo();
			shopItemInfo.NameTextId = itemSettings.Name;
			shopItemInfo.Description = itemSettings.Description;
			shopItemInfo.SpriteWidget = ((!(itemSettings.ShopWidget == null)) ? itemSettings.ShopWidget : MissingWidget);
			shopItemInfo.MaxUpgrade = 0;
			shopItemInfo.GoldCurrency = itemSettings.GoldCost > 0;
			shopItemInfo.Cost = ((!shopItemInfo.GoldCurrency) ? itemSettings.MoneyCost : itemSettings.GoldCost);
			shopItemInfo.Locked = itemSettings.Rank > PlayerLevel;
			shopItemInfo.RequiredLevel = itemSettings.Rank;
			shopItemInfo.ShopCount = itemSettings.Count;
			shopItemInfo.MissionMaxCount = itemSettings.MaxCountInMisson;
			shopItemInfo.InfiniteUse = itemSettings.InfiniteUse;
			shopItemInfo.JustUnlocked = shopItemInfo.RequiredLevel == PlayerLevel;
			shopItemInfo.NewInShop = itemSettings.NewInShop;
			if (itemSettings.SaleInPercent > 0)
			{
				shopItemInfo.PriceSale = true;
				shopItemInfo.DiscountTag = TextDatabase.instance[2030065];
				shopItemInfo.DiscountTag = shopItemInfo.DiscountTag.Replace("%d1", itemSettings.SaleInPercent.ToString());
				shopItemInfo.DiscountTagSmall = TextDatabase.instance[2030066];
				shopItemInfo.DiscountTagSmall = shopItemInfo.DiscountTagSmall.Replace("%d1", itemSettings.SaleInPercent.ToString());
				shopItemInfo.CostBeforeSale = shopItemInfo.Cost;
				int b = (int)((float)shopItemInfo.Cost * (1f - (float)itemSettings.SaleInPercent * 0.01f));
				shopItemInfo.Cost = Mathf.Max(0, b);
			}
			PPIItemData ownedItemData = GetOwnedItemData((E_ItemID)id);
			if (ownedItemData.IsValid())
			{
				shopItemInfo.OwnedCount = ownedItemData.Count;
				shopItemInfo.Owned = true;
			}
			return shopItemInfo;
		}
		Debug.LogError("Item not found: " + id);
		return new ShopItemInfo();
	}

	internal ShopItemInfo GetShopUpgradeInfo(int id)
	{
		UpgradeSettings upgradeSettings = UpgradeSettingsManager.Instance.Get((E_UpgradeID)id);
		if (upgradeSettings != null)
		{
			ShopItemInfo shopItemInfo = new ShopItemInfo();
			shopItemInfo.NameTextId = upgradeSettings.Name;
			shopItemInfo.Description = upgradeSettings.Description;
			shopItemInfo.SpriteWidget = ((!(upgradeSettings.ShopWidget == null)) ? upgradeSettings.ShopWidget : MissingWidget);
			shopItemInfo.MaxUpgrade = 0;
			shopItemInfo.GoldCurrency = upgradeSettings.GoldCost > 0;
			shopItemInfo.Cost = ((!shopItemInfo.GoldCurrency) ? upgradeSettings.MoneyCost : upgradeSettings.GoldCost);
			shopItemInfo.Locked = upgradeSettings.Rank > PlayerLevel;
			shopItemInfo.RequiredLevel = upgradeSettings.Rank;
			shopItemInfo.JustUnlocked = shopItemInfo.RequiredLevel == PlayerLevel;
			shopItemInfo.NewInShop = upgradeSettings.NewInShop;
			if (upgradeSettings.SaleInPercent > 0)
			{
				shopItemInfo.PriceSale = true;
				shopItemInfo.DiscountTag = TextDatabase.instance[2030065];
				shopItemInfo.DiscountTag = shopItemInfo.DiscountTag.Replace("%d1", upgradeSettings.SaleInPercent.ToString());
				shopItemInfo.DiscountTagSmall = TextDatabase.instance[2030066];
				shopItemInfo.DiscountTagSmall = shopItemInfo.DiscountTagSmall.Replace("%d1", upgradeSettings.SaleInPercent.ToString());
				shopItemInfo.CostBeforeSale = shopItemInfo.Cost;
				int b = (int)((float)shopItemInfo.Cost * (1f - (float)upgradeSettings.SaleInPercent * 0.01f));
				shopItemInfo.Cost = Mathf.Max(0, b);
			}
			shopItemInfo.Owned = HasOwnedUpgrade((E_UpgradeID)id);
			return shopItemInfo;
		}
		Debug.LogError("Upgrade not found: " + id);
		return new ShopItemInfo();
	}

	private ShopItemInfo GetShopFundInfo(int id)
	{
		FundSettings fundSettings = FundSettingsManager.Instance.Get((E_FundID)id);
		if (fundSettings != null)
		{
			ShopItemInfo shopItemInfo = new ShopItemInfo();
			shopItemInfo.NameTextId = fundSettings.Name;
			shopItemInfo.Description = fundSettings.Description;
			shopItemInfo.Owned = false;
			shopItemInfo.SpriteWidget = ((!(fundSettings.ShopWidget == null)) ? fundSettings.ShopWidget : MissingWidget);
			shopItemInfo.MaxUpgrade = 0;
			shopItemInfo.AddGold = fundSettings.AddGold;
			shopItemInfo.AddMoney = fundSettings.AddMoney;
			shopItemInfo.NewInShop = fundSettings.NewInShop;
			string productId = GetShopItemGUID(new ShopItemId(id, GuiShop.E_ItemType.Fund)).ToString();
			IAP.Product product = IAP.AvailableProducts.FirstOrDefault((IAP.Product p) => p.Id.Equals(productId));
			if (product != null)
			{
				shopItemInfo.IAPCost = product.FormattedPrice;
			}
			if (fundSettings.SaleInPercent > 0)
			{
				shopItemInfo.PriceSale = true;
				shopItemInfo.DiscountTag = TextDatabase.instance[2030064];
				shopItemInfo.DiscountTag = shopItemInfo.DiscountTag.Replace("%d1", fundSettings.SaleInPercent.ToString());
				shopItemInfo.DiscountTagSmall = shopItemInfo.DiscountTag;
			}
			if (fundSettings.GoldCost > 0 || fundSettings.MoneyCost > 0)
			{
				shopItemInfo.GoldCurrency = fundSettings.GoldCost > 0;
				shopItemInfo.Cost = ((!shopItemInfo.GoldCurrency) ? fundSettings.MoneyCost : fundSettings.GoldCost);
			}
			return shopItemInfo;
		}
		Debug.LogError("Fund not found: " + id);
		return new ShopItemInfo();
	}

	public bool IsWeaponSlotLocked(int slotIndex)
	{
		int num = 2;
		if (HasOwnedUpgrade(E_UpgradeID.WeaponSlot2))
		{
			num++;
		}
		if (HasOwnedUpgrade(E_UpgradeID.WeaponSlotExclusive))
		{
			num++;
		}
		return slotIndex >= num;
	}

	public bool IsItemSlotLocked(int slotIndex)
	{
		int num = 2;
		if (HasOwnedUpgrade(E_UpgradeID.ItemSlot2))
		{
			num++;
		}
		if (HasOwnedUpgrade(E_UpgradeID.ItemSlotExclusive))
		{
			num++;
		}
		return slotIndex >= num;
	}

	public bool HaveEnoughMoney(ShopItemId itemId, float discount = 0f)
	{
		int fundsMissing;
		bool isGold;
		MissingFunds(itemId, out fundsMissing, out isGold, discount);
		return fundsMissing <= 0;
	}

	public bool HaveEnoughMoneyForUpgrade(ShopItemId itemId)
	{
		int fundsMissing;
		bool isGold;
		MissingFundsForUpgrade(itemId, out fundsMissing, out isGold);
		return fundsMissing <= 0;
	}

	public void RequiredFunds(ShopItemId itemId, out int fundsNeeded, out bool isGold)
	{
		ShopItemInfo itemInfo = GetItemInfo(itemId);
		isGold = itemInfo.GoldCurrency;
		fundsNeeded = itemInfo.Cost;
	}

	public void RequiredFundsForUpgrade(ShopItemId itemId, out int fundsNeeded, out bool isGold)
	{
		ShopItemInfo itemInfo = GetItemInfo(itemId);
		isGold = itemInfo.UpgradeGoldCurrency;
		fundsNeeded = itemInfo.UpgradeCost;
	}

	private void MissingFunds(ShopItemId itemId, out int fundsMissing, out bool isGold, float discount = 0f)
	{
		int fundsNeeded;
		RequiredFunds(itemId, out fundsNeeded, out isGold);
		fundsNeeded = Mathf.RoundToInt((float)fundsNeeded - (float)fundsNeeded * discount);
		int num = ((!isGold) ? Instance.PlayerMoney : Instance.PlayerGold);
		fundsMissing = fundsNeeded - num;
	}

	private void MissingFundsForUpgrade(ShopItemId itemId, out int fundsMissing, out bool isGold)
	{
		int fundsNeeded;
		RequiredFundsForUpgrade(itemId, out fundsNeeded, out isGold);
		fundsMissing = fundsNeeded - ((!isGold) ? Instance.PlayerMoney : Instance.PlayerGold);
	}

	public ShopItemId FindFundsItem(int fundsRequest, bool isGold)
	{
		E_FundID e_FundID = E_FundID.Item_None;
		int num = 0;
		FundSettings[] all = FundSettingsManager.Instance.GetAll();
		FundSettings[] array = all;
		foreach (FundSettings fundSettings in array)
		{
			bool flag = fundSettings.AddGold > 0;
			int num2 = ((!flag) ? fundSettings.AddMoney : fundSettings.AddGold);
			if (fundSettings.MoneyCost <= 0 && fundSettings.GoldCost <= 0 && isGold == flag)
			{
				if (num < fundsRequest && num2 > num)
				{
					num = num2;
					e_FundID = fundSettings.ID;
				}
				else if (num > fundsRequest && num2 >= fundsRequest && num2 < num)
				{
					num = num2;
					e_FundID = fundSettings.ID;
				}
			}
		}
		Debug.Log(string.Concat("Best funds found: ", e_FundID, " requested ", fundsRequest));
		if (e_FundID != 0)
		{
			return new ShopItemId((int)e_FundID, GuiShop.E_ItemType.Fund);
		}
		return ShopItemId.EmptyId;
	}

	private void AddToPlayerInventory(ShopItemId itemId)
	{
		PPIInventoryList inventoryList = PPI.InventoryList;
		switch (itemId.ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			inventoryList.Weapons.Add(new PPIWeaponData
			{
				ID = (E_WeaponID)itemId.Id,
				EquipSlotIdx = -1
			});
			break;
		case GuiShop.E_ItemType.Item:
		{
			PPIItemData ownedItemData = GetOwnedItemData((E_ItemID)itemId.Id);
			ItemSettings itemSettings = ItemSettingsManager.Instance.Get((E_ItemID)itemId.Id);
			if (!ownedItemData.IsValid())
			{
				inventoryList.Items.Add(new PPIItemData
				{
					ID = (E_ItemID)itemId.Id,
					Count = itemSettings.Count,
					EquipSlotIdx = -1
				});
			}
			else
			{
				int newCount = ownedItemData.Count + itemSettings.Count;
				SetOwnedItemCount(ownedItemData.ID, newCount);
			}
			break;
		}
		case GuiShop.E_ItemType.Upgrade:
		{
			PPIUpgradeList upgrades = PPI.Upgrades;
			List<PPIUpgradeList.UpgradeData> upgrades2 = upgrades.Upgrades;
			PPIUpgradeList.UpgradeData upgradeData = new PPIUpgradeList.UpgradeData();
			upgradeData.ID = (E_UpgradeID)itemId.Id;
			upgrades2.Add(upgradeData);
			GuiEquipMenu.Instance.UpdateLockedSlots();
			break;
		}
		case GuiShop.E_ItemType.Fund:
			Debug.LogError("Invalid operation: Trying to add Funds to Inventory!");
			break;
		default:
			Debug.LogError("Invalid or unhandled item type: " + itemId.ItemType);
			break;
		}
	}

	public void SynchroniseBoughtItem(ShopItemId itemId, float discount = 0f)
	{
		RemoveFunds(itemId, discount);
		AddToPlayerInventory(itemId);
	}

	public void UpgradeWeapon(ShopItemId itemId)
	{
		if (itemId.ItemType != GuiShop.E_ItemType.Weapon)
		{
			Debug.LogError("TODO: Unsupported type for upgrade");
			return;
		}
		PPIWeaponData ownedWeaponData = GetOwnedWeaponData((E_WeaponID)itemId.Id);
		if (!ownedWeaponData.IsValid())
		{
			Debug.LogError("Not in inventory!");
			return;
		}
		int upgradeLevel = (int)ownedWeaponData.UpgradeLevel;
		if (upgradeLevel >= 3)
		{
			Debug.LogError("Already has maximum upgrades!");
			return;
		}
		upgradeLevel++;
		WeaponSettings weaponSettings = WeaponSettingsManager.Instance.Get((E_WeaponID)itemId.Id);
		if (upgradeLevel >= weaponSettings.Upgrades.Length)
		{
			Debug.LogError("Trying upgrade more then this weapon have set!");
			return;
		}
		RemoveUpgradeFunds(itemId);
		SetOwnedWeaponUpgrade((E_WeaponID)itemId.Id, upgradeLevel);
	}

	private void RemoveFunds(ShopItemId itemId, float discount = 0f)
	{
		ShopItemInfo itemInfo = GetItemInfo(itemId);
		if (itemInfo.GoldCurrency)
		{
			PPI.RemoveGold(Mathf.RoundToInt((float)itemInfo.Cost - (float)itemInfo.Cost * discount));
		}
		else
		{
			PPI.RemoveMoney(Mathf.RoundToInt((float)itemInfo.Cost - (float)itemInfo.Cost * discount));
		}
	}

	private void RemoveUpgradeFunds(ShopItemId itemId)
	{
		ShopItemInfo itemInfo = GetItemInfo(itemId);
		if (itemInfo.UpgradeGoldCurrency)
		{
			PPI.RemoveGold(itemInfo.UpgradeCost);
		}
		else
		{
			PPI.RemoveMoney(itemInfo.UpgradeCost);
		}
	}

	public int EquippedWeaponsCount()
	{
		return PPI.EquipList.Weapons.Count;
	}

	public void Debug_LogEquipedWeapons()
	{
		Debug.Log("Equiped Weapons");
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			Debug.Log(string.Concat(" - ", weapon.ID, " , slot: ", weapon.EquipSlotIdx));
		}
	}

	public void Debug_LogOwnedWeapons()
	{
		Debug.Log("Owned Weapons:");
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIWeaponData> weapons = inventoryList.Weapons;
		foreach (PPIWeaponData item in weapons)
		{
			Debug.Log(" - " + item.ID);
		}
	}

	public void Debug_LogOwnedItems()
	{
		Debug.Log("Owned Items:");
		PPIInventoryList inventoryList = PPI.InventoryList;
		List<PPIItemData> items = inventoryList.Items;
		foreach (PPIItemData item in items)
		{
			Debug.Log(string.Concat(" - ", item.ID, ": ", item.Count));
		}
	}

	public void Debug_LogEquipedItems()
	{
		Debug.Log("Equiped Items");
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIItemData item in equipList.Items)
		{
			Debug.Log(string.Concat(" - ", item.ID, " , slot: ", item.EquipSlotIdx, ", count: ", item.Count));
		}
	}

	private bool IsWeaponEquiped(int id)
	{
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			if (weapon.ID == (E_WeaponID)id)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsItemEquiped(int id)
	{
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIItemData item in equipList.Items)
		{
			if (item.ID == (E_ItemID)id)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsEquiped(ShopItemId itemId)
	{
		switch (itemId.ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			return IsWeaponEquiped(itemId.Id);
		case GuiShop.E_ItemType.Item:
			return IsItemEquiped(itemId.Id);
		default:
			return false;
		}
	}

	public int GetShopItemGUID(ShopItemId itm)
	{
		GuiShop.E_ItemType itemType = itm.ItemType;
		if (itemType == GuiShop.E_ItemType.Fund)
		{
			FundSettings fundSettings = FundSettingsManager.Instance.Get((E_FundID)itm.Id);
			return fundSettings.GUID;
		}
		Debug.LogError("TODO: unhandled item type");
		return 0;
	}

	private FundSettings FindFundSettings(int guid)
	{
		FundSettings[] all = FundSettingsManager.Instance.GetAll();
		FundSettings[] array = all;
		foreach (FundSettings fundSettings in array)
		{
			if (!fundSettings.DISABLED && fundSettings.GUID == guid)
			{
				return fundSettings;
			}
		}
		return null;
	}

	public ShopItemId GetWeaponInSlot(int slotIdx)
	{
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			if (weapon.EquipSlotIdx == slotIdx)
			{
				return new ShopItemId((int)weapon.ID, GuiShop.E_ItemType.Weapon);
			}
		}
		return ShopItemId.EmptyId;
	}

	public ShopItemId GetItemInSlot(int slotIdx)
	{
		PPIEquipList equipList = PPI.EquipList;
		foreach (PPIItemData item in equipList.Items)
		{
			if (item.EquipSlotIdx == slotIdx)
			{
				return new ShopItemId((int)item.ID, GuiShop.E_ItemType.Item);
			}
		}
		return ShopItemId.EmptyId;
	}

	public void Action_Equip(ShopItemId item, int slot)
	{
		if (item.ItemType == GuiShop.E_ItemType.Weapon)
		{
			EquipWeapon((E_WeaponID)item.Id, slot);
		}
		else if (item.ItemType == GuiShop.E_ItemType.Item)
		{
			EquipItem((E_ItemID)item.Id, slot);
		}
	}

	public void Action_UnEquip(ShopItemId item, int slot)
	{
		if (item.ItemType == GuiShop.E_ItemType.Weapon)
		{
			UnEquipWeapon((E_WeaponID)item.Id, slot);
		}
		else if (item.ItemType == GuiShop.E_ItemType.Item)
		{
			UnEquipItem((E_ItemID)item.Id, slot);
		}
	}

	private static int CompareWeaponData(PPIWeaponData x, PPIWeaponData y)
	{
		return x.EquipSlotIdx.CompareTo(y.EquipSlotIdx);
	}

	private static int CompareItemData(PPIItemData x, PPIItemData y)
	{
		return x.EquipSlotIdx.CompareTo(y.EquipSlotIdx);
	}

	private void EquipWeapon(E_WeaponID id, int slot)
	{
		PPIWeaponData ownedWeaponData = GetOwnedWeaponData(id);
		if (!ownedWeaponData.IsValid())
		{
			Debug.LogError("Trying to equip weapon that is not owned: " + id);
			return;
		}
		PPIEquipList equipList = PPI.EquipList;
		List<PPIWeaponData> list = new List<PPIWeaponData>();
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			if (weapon.EquipSlotIdx == slot)
			{
				list.Add(weapon);
			}
			else if (weapon.ID == id)
			{
				list.Add(weapon);
			}
		}
		PPIWeaponData remIt;
		foreach (PPIWeaponData item in list)
		{
			remIt = item;
			SetWeaponSlot(remIt.ID, -1);
			equipList.Weapons.RemoveAll((PPIWeaponData ps) => ps.ID == remIt.ID);
		}
		SetWeaponSlot(ownedWeaponData.ID, slot);
		ownedWeaponData.EquipSlotIdx = slot;
		equipList.Weapons.Add(ownedWeaponData);
		equipList.Weapons.Sort(CompareWeaponData);
	}

	private void EquipItem(E_ItemID id, int slot)
	{
		PPIItemData ownedItemData = GetOwnedItemData(id);
		if (!ownedItemData.IsValid())
		{
			Debug.LogError("Trying to equip item that is not owned: " + id);
			return;
		}
		PPIEquipList equipList = PPI.EquipList;
		List<PPIItemData> list = new List<PPIItemData>();
		foreach (PPIItemData item in equipList.Items)
		{
			if (item.EquipSlotIdx == slot)
			{
				list.Add(item);
			}
			else if (item.ID == id)
			{
				list.Add(item);
			}
		}
		PPIItemData remIt;
		foreach (PPIItemData item2 in list)
		{
			remIt = item2;
			SetItemSlot(remIt.ID, -1);
			equipList.Items.RemoveAll((PPIItemData ps) => ps.ID == remIt.ID);
		}
		ownedItemData.EquipSlotIdx = slot;
		equipList.Items.Add(ownedItemData);
		SetItemSlot(ownedItemData.ID, slot);
		equipList.Items.Sort(CompareItemData);
	}

	private void UnEquipWeapon(E_WeaponID id, int slot)
	{
		if (!GetOwnedWeaponData(id).IsValid())
		{
			Debug.LogError("Trying to unequip weapon that is not owned: " + id);
			return;
		}
		PPIEquipList equipList = PPI.EquipList;
		PPIWeaponData removeItem = default(PPIWeaponData);
		foreach (PPIWeaponData weapon in equipList.Weapons)
		{
			Debug.Log(string.Concat(weapon.ID, " ", id));
			if (weapon.ID == id)
			{
				if (weapon.EquipSlotIdx == slot)
				{
					removeItem = weapon;
					break;
				}
				Debug.LogError("Found item with same id but in different slot ");
			}
		}
		if (removeItem.IsValid())
		{
			SetWeaponSlot(removeItem.ID, -1);
			equipList.Weapons.RemoveAll((PPIWeaponData ps) => ps.ID == removeItem.ID);
		}
		else
		{
			Debug.Log("Item not found in equip list");
		}
		equipList.Weapons.Sort(CompareWeaponData);
	}

	private void UnEquipItem(E_ItemID id, int slot)
	{
		if (!GetOwnedItemData(id).IsValid())
		{
			Debug.LogError("Trying to unequip item that is not owned: " + id);
			return;
		}
		PPIEquipList equipList = PPI.EquipList;
		PPIItemData removeItem = default(PPIItemData);
		foreach (PPIItemData item in equipList.Items)
		{
			if (item.ID == id)
			{
				if (item.EquipSlotIdx == slot)
				{
					removeItem = item;
					break;
				}
				Debug.LogWarning("Founf item with same id but in different slot ");
			}
		}
		if (removeItem.IsValid())
		{
			SetItemSlot(removeItem.ID, -1);
			equipList.Items.RemoveAll((PPIItemData ps) => ps.ID == removeItem.ID);
		}
		else
		{
			Debug.Log("Item not found in equip list");
		}
		equipList.Items.Sort(CompareItemData);
	}

	public void IAPInit(Action<bool> callback = null)
	{
		if (!IAP.IsReady && !m_IAPInitInProgress)
		{
			FundSettings[] all = FundSettingsManager.Instance.GetAll();
			List<IAP.Product> list = new List<IAP.Product>();
			FundSettings[] array = all;
			foreach (FundSettings fundSettings in array)
			{
				if (fundSettings.AddGold > 0 || fundSettings.AddMoney > 0)
				{
					list.Add(new IAP.Product
					{
						Id = fundSettings.GUID.ToString(),
						Type = IAP.Product.E_Type.Consumable
					});
				}
			}
			m_IAPInitInProgress = true;
			IAP.Init(list.ToArray(), m_Verifier, delegate(IAP.E_Init result)
			{
				m_IAPInitInProgress = false;
				if (callback != null)
				{
					callback(result == IAP.E_Init.Success);
				}
			});
		}
		else if (callback != null)
		{
			callback(IAP.IsReady);
		}
	}

	public bool IAPServiceAvailable()
	{
		return IAP.IsReady;
	}

	public void IAPRequestPurchase(ShopItemId item, Action<IAP.E_Buy> callback)
	{
		string guid = GetShopItemGUID(item).ToString();
		IAP.Product product = IAP.AvailableProducts.FirstOrDefault((IAP.Product p) => p.Id.Equals(guid));
		IAP.Buy(product, callback);
	}

	private void IAPProcessBoughtItem(int guid)
	{
		FundSettings fundSettings = FindFundSettings(guid);
		if ((bool)fundSettings)
		{
			if (fundSettings.GoldCost > 0 || fundSettings.MoneyCost > 0)
			{
				PPI.RemoveGold(fundSettings.GoldCost);
				PPI.RemoveMoney(fundSettings.MoneyCost);
			}
			PPI.AddGold(fundSettings.AddGold);
			PPI.AddMoney(fundSettings.AddMoney);
			Game.Instance.PlayerPersistentInfo.Save();
			if (CityManager.Instance != null)
			{
				CityManager.Instance.hasSpentRealMoney = true;
			}
			Kontagent.Monetization(fundSettings.GetPriceInCents());
			Kontagent.SendCustomEvent(fundSettings.ID.ToString(), "iAP", "Buy", string.Empty, PPI.rank, fundSettings.GetPriceInCents());
		}
		else
		{
			Debug.LogError("IAP Fund not found. guid " + guid);
		}
	}

	public bool IsIAPFund(ShopItemId itemId)
	{
		if (itemId.ItemType == GuiShop.E_ItemType.Fund && !IsFreeGold(itemId))
		{
			return true;
		}
		return false;
	}

	public UpgradeInfo AmmoUpgInfo()
	{
		UpgradeInfo result = default(UpgradeInfo);
		int num = 0;
		if (HasOwnedUpgrade(E_UpgradeID.Ammo1))
		{
			num++;
		}
		if (HasOwnedUpgrade(E_UpgradeID.AmmoExclusive))
		{
			num++;
		}
		result.Value = num * 25;
		result.Level = num;
		result.MaxLevel = 2;
		return result;
	}

	public UpgradeInfo HealthUpgInfo()
	{
		UpgradeInfo result = default(UpgradeInfo);
		int num = 0;
		if (HasOwnedUpgrade(E_UpgradeID.Health1))
		{
			num++;
		}
		if (HasOwnedUpgrade(E_UpgradeID.Health2))
		{
			num++;
		}
		if (HasOwnedUpgrade(E_UpgradeID.HealthExclusive))
		{
			num++;
		}
		result.Value = (int)(100f + (float)num * Player.HealthSegment);
		result.Level = num;
		result.MaxLevel = 3;
		return result;
	}

	public UpgradeInfo RadarUpgInfo()
	{
		UpgradeInfo result = default(UpgradeInfo);
		int num = 0;
		if (HasOwnedUpgrade(E_UpgradeID.ImproveRadar))
		{
			num++;
		}
		result.Value = (int)((num != 0) ? 15f : 10f);
		result.Level = num;
		result.MaxLevel = 1;
		return result;
	}

	public UpgradeInfo AutohealUpgInfo()
	{
		UpgradeInfo result = default(UpgradeInfo);
		int num = 0;
		if (HasOwnedUpgrade(E_UpgradeID.AutohealKit))
		{
			num++;
		}
		result.Value = ((num != 0) ? 1 : 0);
		result.Level = num;
		result.MaxLevel = 1;
		return result;
	}

	public ShopItemId NextAmmoUpg()
	{
		if (!HasOwnedUpgrade(E_UpgradeID.AmmoExclusive))
		{
			return new ShopItemId(10, GuiShop.E_ItemType.Upgrade);
		}
		if (!HasOwnedUpgrade(E_UpgradeID.Ammo1))
		{
			return new ShopItemId(9, GuiShop.E_ItemType.Upgrade);
		}
		return ShopItemId.EmptyId;
	}

	public ShopItemId NextHealthUpg()
	{
		if (!HasOwnedUpgrade(E_UpgradeID.HealthExclusive))
		{
			return new ShopItemId(8, GuiShop.E_ItemType.Upgrade);
		}
		if (!HasOwnedUpgrade(E_UpgradeID.Health1))
		{
			return new ShopItemId(6, GuiShop.E_ItemType.Upgrade);
		}
		if (!HasOwnedUpgrade(E_UpgradeID.Health2))
		{
			return new ShopItemId(7, GuiShop.E_ItemType.Upgrade);
		}
		return ShopItemId.EmptyId;
	}

	public ShopItemId NextRadarUpg()
	{
		if (!HasOwnedUpgrade(E_UpgradeID.ImproveRadar))
		{
			return new ShopItemId(12, GuiShop.E_ItemType.Upgrade);
		}
		return ShopItemId.EmptyId;
	}

	public ShopItemId NextAutohealUpg()
	{
		if (!HasOwnedUpgrade(E_UpgradeID.AutohealKit))
		{
			return new ShopItemId(11, GuiShop.E_ItemType.Upgrade);
		}
		return ShopItemId.EmptyId;
	}

	public static string FormatDamage(int dmg)
	{
		if (dmg >= 1000)
		{
			return ((float)dmg / 1000f).ToString("F1") + "k";
		}
		return dmg.ToString();
	}

	public bool IsFreeGold(ShopItemId item)
	{
		if (item.ItemType != GuiShop.E_ItemType.Fund)
		{
			return false;
		}
		E_FundID id = (E_FundID)item.Id;
		return id == E_FundID.TapJoyWeb || id == E_FundID.TapJoyInApp;
	}

	public bool NewItemsUnlocked(int rank)
	{
		int num = 0;
		WeaponSettings[] all = WeaponSettingsManager.Instance.GetAll();
		WeaponSettings[] array = all;
		foreach (WeaponSettings weaponSettings in array)
		{
			if (weaponSettings.Rank == rank)
			{
				num++;
			}
		}
		ItemSettings[] all2 = ItemSettingsManager.Instance.GetAll();
		ItemSettings[] array2 = all2;
		foreach (ItemSettings itemSettings in array2)
		{
			if (itemSettings.Rank == rank)
			{
				num++;
			}
		}
		UpgradeSettings[] all3 = UpgradeSettingsManager.Instance.GetAll();
		UpgradeSettings[] array3 = all3;
		foreach (UpgradeSettings upgradeSettings in array3)
		{
			if (upgradeSettings.Rank == rank)
			{
				num++;
			}
		}
		return num > 0;
	}

	public ShopItemId GetUnequipedItem()
	{
		foreach (PPIItemData item in PPI.InventoryList.Items)
		{
			if (item.Count <= 0 || item.EquipSlotIdx >= 0)
			{
				continue;
			}
			return new ShopItemId((int)item.ID, GuiShop.E_ItemType.Item);
		}
		return ShopItemId.EmptyId;
	}

	public ShopItemId GetUnequipedWeapon()
	{
		foreach (PPIWeaponData weapon in PPI.InventoryList.Weapons)
		{
			if (weapon.EquipSlotIdx >= 0)
			{
				continue;
			}
			return new ShopItemId((int)weapon.ID, GuiShop.E_ItemType.Weapon);
		}
		return ShopItemId.EmptyId;
	}

	public bool HasWeaponUpgradeAvailable(ShopItemId weaponId)
	{
		if (weaponId.ItemType != GuiShop.E_ItemType.Weapon)
		{
			return false;
		}
		ShopItemInfo itemInfo = Instance.GetItemInfo(weaponId);
		return itemInfo.UpgradeReady;
	}

	public bool BuyMoreAdvised(ShopItemId itemId)
	{
		if (itemId.ItemType != GuiShop.E_ItemType.Item)
		{
			return false;
		}
		ShopItemInfo itemInfo = Instance.GetItemInfo(itemId);
		return !itemInfo.InfiniteUse && itemInfo.OwnedCount < itemInfo.MissionMaxCount;
	}

	public static int CompareEquip(ShopItemId first, ShopItemId second)
	{
		return first.CompareByType(second);
	}

	public static int CompareShop(ShopItemId first, ShopItemId second)
	{
		return first.CompareByPrice(second);
	}
}
