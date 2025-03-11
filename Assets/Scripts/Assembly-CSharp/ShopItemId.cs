using System;
using UnityEngine;

public class ShopItemId : IComparable<ShopItemId>, IEquatable<ShopItemId>
{
	private const uint GoldToMoney = 200u;

	public static ShopItemId EmptyId = new ShopItemId(-1, GuiShop.E_ItemType.None);

	public int Id { get; private set; }

	public GuiShop.E_ItemType ItemType { get; private set; }

	internal ShopItemId(int id, GuiShop.E_ItemType type)
	{
		Id = id;
		ItemType = type;
	}

	public bool IsEmpty()
	{
		return this == EmptyId;
	}

	public override string ToString()
	{
		return "ShopItemId: " + Id + " " + ItemType;
	}

	public int CompareTo(ShopItemId other)
	{
		return CompareByPrice(other);
	}

	public int CompareByPrice(ShopItemId other)
	{
		int num = ItemType.CompareTo(other.ItemType);
		if (num != 0)
		{
			return num;
		}
		switch (ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			num = CompareWeapon_ByPrice(other.Id);
			break;
		case GuiShop.E_ItemType.Item:
			num = CompareItem_ByPrice(other.Id);
			break;
		case GuiShop.E_ItemType.Upgrade:
			num = CompareUpgrade(other.Id);
			break;
		case GuiShop.E_ItemType.Fund:
			num = CompareFund(other.Id);
			break;
		default:
			Debug.LogError("TODO: Unhandled type");
			break;
		}
		return num;
	}

	public int CompareByType(ShopItemId other)
	{
		int num = ItemType.CompareTo(other.ItemType);
		if (num != 0)
		{
			return num;
		}
		switch (ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			num = CompareWeapon_ByType(other.Id);
			break;
		case GuiShop.E_ItemType.Item:
			num = CompareItem_ByType(other.Id);
			break;
		case GuiShop.E_ItemType.Upgrade:
			num = CompareUpgrade(other.Id);
			break;
		case GuiShop.E_ItemType.Fund:
			num = CompareFund(other.Id);
			break;
		default:
			Debug.LogError("TODO: Unhandled type");
			break;
		}
		return num;
	}

	private int CompareWeapon_ByPrice(int otherId)
	{
		WeaponSettings weaponSettings = WeaponSettingsManager.Instance.Get((E_WeaponID)otherId);
		WeaponSettings weaponSettings2 = WeaponSettingsManager.Instance.Get((E_WeaponID)Id);
		bool flag = weaponSettings2.Rank <= ShopDataBridge.Instance.PlayerLevel;
		bool flag2 = weaponSettings.Rank <= ShopDataBridge.Instance.PlayerLevel;
		if (flag && flag2)
		{
			ShopItemInfo shopWeaponInfo = ShopDataBridge.Instance.GetShopWeaponInfo(Id);
			ShopItemInfo shopWeaponInfo2 = ShopDataBridge.Instance.GetShopWeaponInfo(otherId);
			int num = (!shopWeaponInfo.Owned).CompareTo(!shopWeaponInfo2.Owned);
			if (num != 0)
			{
				return num;
			}
			num = (!shopWeaponInfo.UpgradeMaxed).CompareTo(!shopWeaponInfo2.UpgradeMaxed);
			if (num != 0)
			{
				return num;
			}
			num = (!shopWeaponInfo.UpgradeReady).CompareTo(!shopWeaponInfo2.UpgradeReady);
			if (num != 0)
			{
				return num;
			}
			uint weaponCost = GetWeaponCost(shopWeaponInfo);
			uint weaponCost2 = GetWeaponCost(shopWeaponInfo2);
			num = weaponCost.CompareTo(weaponCost2);
			if (num != 0)
			{
				return num;
			}
			return weaponSettings2.Rank.CompareTo(weaponSettings.Rank);
		}
		return weaponSettings2.Rank.CompareTo(weaponSettings.Rank);
	}

	private uint GetWeaponCost(ShopItemInfo inf)
	{
		if (inf.Owned)
		{
			return (uint)((!inf.UpgradeGoldCurrency) ? inf.UpgradeCost : (inf.UpgradeCost * 200));
		}
		return (uint)((!inf.GoldCurrency) ? inf.Cost : (inf.Cost * 200));
	}

	private int CompareItem_ByPrice(int otherId)
	{
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get((E_ItemID)otherId);
		ItemSettings itemSettings2 = ItemSettingsManager.Instance.Get((E_ItemID)Id);
		bool flag = itemSettings2.Rank <= ShopDataBridge.Instance.PlayerLevel;
		bool flag2 = itemSettings.Rank <= ShopDataBridge.Instance.PlayerLevel;
		if (flag && flag2)
		{
			ShopItemInfo shopItemInfo = ShopDataBridge.Instance.GetShopItemInfo(Id);
			ShopItemInfo shopItemInfo2 = ShopDataBridge.Instance.GetShopItemInfo(otherId);
			uint num = (uint)((!shopItemInfo.GoldCurrency) ? shopItemInfo.Cost : (shopItemInfo.Cost * 200));
			uint value = (uint)((!shopItemInfo2.GoldCurrency) ? shopItemInfo2.Cost : (shopItemInfo2.Cost * 200));
			int num2 = num.CompareTo(value);
			if (num2 != 0)
			{
				return num2;
			}
			return itemSettings2.Rank.CompareTo(itemSettings.Rank);
		}
		return itemSettings2.Rank.CompareTo(itemSettings.Rank);
	}

	private int CompareWeapon_ByType(int otherId)
	{
		WeaponSettings weaponSettings = WeaponSettingsManager.Instance.Get((E_WeaponID)otherId);
		WeaponSettings weaponSettings2 = WeaponSettingsManager.Instance.Get((E_WeaponID)Id);
		bool flag = weaponSettings2.Rank <= ShopDataBridge.Instance.PlayerLevel;
		bool flag2 = weaponSettings.Rank <= ShopDataBridge.Instance.PlayerLevel;
		if (flag && flag2)
		{
			int num = weaponSettings2.WeaponType.CompareTo(weaponSettings.WeaponType);
			if (num != 0)
			{
				return num;
			}
			return weaponSettings2.Rank.CompareTo(weaponSettings.Rank);
		}
		return weaponSettings2.Rank.CompareTo(weaponSettings.Rank);
	}

	private int CompareItem_ByType(int otherId)
	{
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get((E_ItemID)otherId);
		ItemSettings itemSettings2 = ItemSettingsManager.Instance.Get((E_ItemID)Id);
		bool flag = itemSettings2.Rank <= ShopDataBridge.Instance.PlayerLevel;
		bool flag2 = itemSettings.Rank <= ShopDataBridge.Instance.PlayerLevel;
		if (flag && flag2)
		{
			int num = itemSettings2.ItemType.CompareTo(itemSettings.ItemType);
			if (num != 0)
			{
				return num;
			}
			num = itemSettings2.ItemBehaviour.CompareTo(itemSettings.ItemBehaviour);
			if (num != 0)
			{
				return num;
			}
			return itemSettings2.Rank.CompareTo(itemSettings.Rank);
		}
		return itemSettings2.Rank.CompareTo(itemSettings.Rank);
	}

	private int CompareUpgrade(int otherId)
	{
		UpgradeSettings upgradeSettings = UpgradeSettingsManager.Instance.Get((E_UpgradeID)otherId);
		UpgradeSettings upgradeSettings2 = UpgradeSettingsManager.Instance.Get((E_UpgradeID)Id);
		bool flag = upgradeSettings2.Rank <= ShopDataBridge.Instance.PlayerLevel;
		bool flag2 = upgradeSettings.Rank <= ShopDataBridge.Instance.PlayerLevel;
		if (flag && flag2)
		{
			int num = UpgradeTypeOrder(upgradeSettings2.UpgradeType).CompareTo(UpgradeTypeOrder(upgradeSettings.UpgradeType));
			if (num != 0)
			{
				return num;
			}
			ShopItemInfo shopUpgradeInfo = ShopDataBridge.Instance.GetShopUpgradeInfo(Id);
			ShopItemInfo shopUpgradeInfo2 = ShopDataBridge.Instance.GetShopUpgradeInfo(otherId);
			uint num2 = (uint)((!shopUpgradeInfo.GoldCurrency) ? shopUpgradeInfo.Cost : (shopUpgradeInfo.Cost * 200));
			uint value = (uint)((!shopUpgradeInfo2.GoldCurrency) ? shopUpgradeInfo2.Cost : (shopUpgradeInfo2.Cost * 200));
			num = num2.CompareTo(value);
			if (num != 0)
			{
				return num;
			}
			return upgradeSettings2.Rank.CompareTo(upgradeSettings.Rank);
		}
		return upgradeSettings2.Rank.CompareTo(upgradeSettings.Rank);
	}

	private int UpgradeTypeOrder(E_UpgradeType type)
	{
		switch (type)
		{
		case E_UpgradeType.Radar:
			return -1;
		case E_UpgradeType.AutoHeal:
			return 99;
		default:
			return (int)type;
		}
	}

	private int CompareFund(int otherId)
	{
		FundSettings fundSettings = FundSettingsManager.Instance.Get((E_FundID)otherId);
		FundSettings fundSettings2 = FundSettingsManager.Instance.Get((E_FundID)Id);
		int num = fundSettings2.GoldCost.CompareTo(fundSettings.GoldCost);
		if (num != 0)
		{
			return num;
		}
		num = fundSettings2.MoneyCost.CompareTo(fundSettings.MoneyCost);
		if (num != 0)
		{
			return num;
		}
		num = fundSettings2.AddGold.CompareTo(fundSettings.AddGold);
		if (num != 0)
		{
			return num;
		}
		return fundSettings2.AddMoney.CompareTo(fundSettings.AddMoney);
	}

	public bool Equals(ShopItemId other)
	{
		if (other == null)
		{
			return false;
		}
		return other.Id == Id && other.ItemType == ItemType;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return base.Equals(obj);
		}
		ShopItemId shopItemId = obj as ShopItemId;
		if (shopItemId == null)
		{
			return false;
		}
		return Equals(shopItemId);
	}

	public override int GetHashCode()
	{
		return Id ^ (int)ItemType;
	}
}
