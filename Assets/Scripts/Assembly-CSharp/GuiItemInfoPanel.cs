using UnityEngine;

internal class GuiItemInfoPanel
{
	private GUIBase_Layout m_Layout;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Label m_NameLabel;

	private GUIBase_Sprite m_BigSprite;

	private GUIBase_Button m_BuyButton;

	private GUIBase_Label m_Buy_Button_Label;

	private GUIBase_Sprite m_Buy_Button_BG;

	private GUIBase_Button m_UpgradeButton;

	private GUIBase_Label m_UpgradeBtn_Label;

	private GUIBase_Sprite m_UpgradeButton_BG;

	private GUIBase_Sprite m_Upgrage_Locked;

	private GUIBase_Label m_UpgradeLockedRank_Label;

	private GUIBase_Sprite m_Cost_Sprite;

	private GUIBase_Label m_Cost_Label;

	private GUIBase_Label m_Cost_Label2;

	private GUIBase_Sprite m_CostSpriteGold;

	private GUIBase_Sprite m_CostSpriteMoney;

	private GUIBase_Sprite m_LockedSprite;

	private GUIBase_Label m_RankLabel;

	private GUIBase_Button m_EarnGoldButton;

	private GUIBase_Label m_SaleLabel;

	private GUIBase_Label m_IAP_Label;

	private GUIBase_Pivot m_WeaponInfo_Pivot;

	private GUIBase_TextArea m_WeaponDesc;

	private GUIBase_Label m_DamageLabel;

	private GUIBase_Label m_AccuracyLabel;

	private GUIBase_Label m_RangeLabel;

	private GUIBase_Label m_ClipLabel;

	private GuiShopUpgradeSprite m_UpgradeSprite;

	private GUIBase_Pivot m_ItemInfo_Pivot;

	private GUIBase_TextArea m_ItemDesc;

	private GUIBase_Label m_OwnedCount;

	private GUIBase_Sprite m_InfiniteSprite;

	private GUIBase_Pivot m_FundInfo_Pivot;

	private GUIBase_TextArea m_FundDesc;

	private GUIBase_Pivot m_CapInfo_Pivot;

	private GUIBase_Label m_CapDesc;

	private GUIBase_Pivot m_SkinInfo_Pivot;

	private GUIBase_Label m_SkinDesc;

	private GUIBase_Pivot m_UpgradeInfo_Pivot;

	private GUIBase_TextArea m_UpgradeDesc;

	private GUIBase_Sprite m_OwnedSprite;

	public bool EnableUpgradeButton = true;

	private float m_PulseUpgradeStartTime;

	private float m_PulseBuyStartTime;

	public void InitGui(GUIBase_Layout layout, GUIBase_Pivot pivot)
	{
		m_Layout = layout;
		m_Pivot = pivot;
		m_NameLabel = GuiBaseUtils.PrepareLabel(m_Layout, "Name_Label");
		m_BigSprite = GuiBaseUtils.PrepareSprite(m_Layout, "BigThumbnail");
		m_Cost_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Cost_Label");
		m_Cost_Label2 = GuiBaseUtils.PrepareLabel(m_Layout, "Cost_Label2");
		m_Cost_Sprite = GuiBaseUtils.PrepareSprite(m_Layout, "Cost_Sprite");
		m_CostSpriteGold = GuiBaseUtils.PrepareSprite(m_Layout, "Gold_Sprite");
		m_CostSpriteMoney = GuiBaseUtils.PrepareSprite(m_Layout, "Money_Sprite");
		m_LockedSprite = GuiBaseUtils.PrepareSprite(m_Layout, "LockedSprite");
		m_RankLabel = GuiBaseUtils.GetChildLabel(m_LockedSprite.Widget, "Required_Label");
		m_SaleLabel = GuiBaseUtils.PrepareLabel(m_Layout, "Sale_Label");
		m_IAP_Label = GuiBaseUtils.PrepareLabel(m_Layout, "IAP_Label");
		m_WeaponInfo_Pivot = MFGuiManager.Instance.GetPivot("WeaponInfo_Pivot");
		m_WeaponDesc = GuiBaseUtils.PrepareTextArea(m_Layout, "WeapDesc_Label");
		m_DamageLabel = GuiBaseUtils.PrepareLabel(m_Layout, "DamageInfo_Label");
		m_AccuracyLabel = GuiBaseUtils.PrepareLabel(m_Layout, "AccuracyInfo_Label");
		m_RangeLabel = GuiBaseUtils.PrepareLabel(m_Layout, "RangeInfo_Label");
		m_ClipLabel = GuiBaseUtils.PrepareLabel(m_Layout, "ClipInfo_Label");
		m_UpgradeSprite = new GuiShopUpgradeSprite(GuiBaseUtils.PrepareSprite(m_Layout, "Upgrade_Sprite"));
		m_ItemInfo_Pivot = MFGuiManager.Instance.GetPivot("ItemInfo_Pivot");
		m_ItemDesc = GuiBaseUtils.PrepareTextArea(m_Layout, "ItemDesc_Label");
		m_OwnedCount = GuiBaseUtils.PrepareLabel(m_Layout, "OwnedCount_Label");
		m_InfiniteSprite = GuiBaseUtils.PrepareSprite(m_Layout, "Infinite_Sprite");
		m_FundInfo_Pivot = MFGuiManager.Instance.GetPivot("FundInfo_Pivot");
		m_FundDesc = GuiBaseUtils.PrepareTextArea(m_Layout, "FundDesc_Label");
		m_UpgradeInfo_Pivot = MFGuiManager.Instance.GetPivot("UpgradeInfo_Pivot");
		m_UpgradeDesc = GuiBaseUtils.PrepareTextArea(m_Layout, "UpgradeDesc_Label");
		m_OwnedSprite = GuiBaseUtils.PrepareSprite(m_Layout, "OwnedSprite");
	}

	public void Show(ShopItemId itemId, GUIBase_Button.ReleaseDelegate OnBuyButton, GUIBase_Button.ReleaseDelegate OnUpgradeButton, GUIBase_Button.ReleaseDelegate OnEquipButton, GUIBase_Button.ReleaseDelegate OnEarnGoldButton)
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, true);
		m_BuyButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Buy_Button", null, OnBuyButton);
		m_Buy_Button_Label = GuiBaseUtils.GetChildLabel(m_BuyButton.Widget, "Buy_Button_Label");
		m_Buy_Button_BG = GuiBaseUtils.GetChildSprite(m_BuyButton.Widget, "BG");
		m_UpgradeButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Upgrade_Button", null, OnUpgradeButton);
		m_UpgradeBtn_Label = GuiBaseUtils.GetChildLabel(m_UpgradeButton.Widget, "UpgradeBtn_Label");
		m_UpgradeButton_BG = GuiBaseUtils.GetChildSprite(m_UpgradeButton.Widget, "BG");
		m_Upgrage_Locked = GuiBaseUtils.GetChildSprite(m_UpgradeButton.Widget, "Upgrage_Locked");
		m_UpgradeLockedRank_Label = GuiBaseUtils.GetChildLabel(m_Upgrage_Locked.Widget, "UpgradeLockedRank_Label");
		m_EarnGoldButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "FreeGold_Button", null, OnEarnGoldButton);
		if (itemId == ShopItemId.EmptyId)
		{
			ShowEmptySelection();
			return;
		}
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(itemId);
		m_NameLabel.SetNewText(itemInfo.NameTextId);
		m_BigSprite.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		if (itemInfo.Locked)
		{
			m_LockedSprite.Widget.Show(true, true);
			m_RankLabel.SetNewText(itemInfo.RequiredLevel.ToString());
		}
		else
		{
			m_LockedSprite.Widget.Show(false, true);
		}
		bool flag = itemId.ItemType == GuiShop.E_ItemType.Item && !itemInfo.InfiniteUse;
		bool flag2 = itemInfo.PriceSale && (!itemInfo.Owned || flag);
		m_SaleLabel.Widget.Show(flag2, true);
		if (flag2)
		{
			m_SaleLabel.SetNewText(itemInfo.DiscountTag);
		}
		if (ShopDataBridge.Instance.IsIAPFund(itemId) && itemInfo.IAPCost != null && itemInfo.IAPCost.Length > 0)
		{
			m_IAP_Label.Widget.Show(true, true);
			m_IAP_Label.SetNewText(itemInfo.IAPCost);
		}
		else
		{
			m_IAP_Label.Widget.Show(false, true);
		}
		m_OwnedSprite.Widget.Show(itemInfo.Owned, true);
		bool flag3 = ShopDataBridge.Instance.IsFreeGold(itemId);
		bool flag4 = itemId.ItemType == GuiShop.E_ItemType.Weapon && itemInfo.Owned && !itemInfo.Locked && EnableUpgradeButton;
		bool flag5 = !flag3 && !flag4;
		m_EarnGoldButton.Widget.Show(flag3, true);
		bool flag6 = flag4 && itemInfo.Upgrade < itemInfo.MaxUpgrade;
		m_UpgradeButton.Widget.Show(flag4, true);
		m_UpgradeButton.SetDisabled(!flag6);
		if (flag4)
		{
			if (itemInfo.UpgradeReady || itemInfo.UpgradeMaxed)
			{
				m_Upgrage_Locked.Widget.Show(false, true);
				m_UpgradeBtn_Label.Widget.Show(true, true);
				if (itemInfo.Upgrade >= itemInfo.MaxUpgrade)
				{
					m_UpgradeBtn_Label.SetNewText(2030052);
				}
				else
				{
					m_UpgradeBtn_Label.SetNewText(2030021);
				}
			}
			else
			{
				m_UpgradeBtn_Label.Widget.Show(false, true);
				m_Upgrage_Locked.Widget.Show(true, true);
				m_UpgradeLockedRank_Label.SetNewText(itemInfo.UpgradeRank.ToString());
			}
		}
		bool flag7 = itemId.ItemType == GuiShop.E_ItemType.Item && !itemInfo.InfiniteUse;
		bool flag8 = (!itemInfo.Owned || flag7) && !itemInfo.Locked;
		m_BuyButton.Widget.Show(flag5, true);
		m_BuyButton.SetDisabled(!flag8);
		if (flag5)
		{
			int num = 2030020;
			num = ((itemId.ItemType == GuiShop.E_ItemType.Fund && itemInfo.Cost > 0) ? 2030040 : ((itemInfo.Owned && !flag8) ? 2030054 : ((!itemInfo.Locked) ? 2030020 : 2030053)));
			m_Buy_Button_Label.SetNewText(num);
		}
		if (flag8)
		{
			if (ShopDataBridge.Instance.BuyMoreAdvised(itemId))
			{
				StartHighlightBuy();
			}
			else
			{
				StopHighlightBuy();
			}
		}
		if (itemInfo.UpgradeReady)
		{
			m_PulseUpgradeStartTime = Time.time;
			m_UpgradeButton.Widget.m_FadeAlpha = 1f;
			m_UpgradeButton_BG.Widget.m_FadeAlpha = 1f;
		}
		else
		{
			m_PulseUpgradeStartTime = 0f;
			m_UpgradeButton.Widget.m_FadeAlpha = 1f;
			m_UpgradeButton_BG.Widget.m_FadeAlpha = 1f;
		}
		bool flag9 = itemInfo.Cost > 0;
		if (itemId.ItemType == GuiShop.E_ItemType.Weapon && itemInfo.Upgrade == itemInfo.MaxUpgrade)
		{
			flag9 = false;
		}
		m_Cost_Sprite.Widget.Show(flag9, true);
		if (flag9)
		{
			int num2 = ((!flag4) ? itemInfo.Cost : itemInfo.UpgradeCost);
			bool flag10 = ((!flag4) ? itemInfo.GoldCurrency : itemInfo.UpgradeGoldCurrency);
			m_Cost_Label.SetNewText(num2.ToString());
			m_CostSpriteGold.Widget.Show(flag10, false);
			m_CostSpriteMoney.Widget.Show(!flag10, false);
			if (flag2)
			{
				m_Cost_Label2.SetNewText(itemInfo.CostBeforeSale.ToString());
				m_Cost_Label2.Widget.Show(true, true);
			}
			else
			{
				m_Cost_Label2.Widget.Show(false, true);
			}
		}
		HideSelectionSpecificInfo();
		switch (itemId.ItemType)
		{
		case GuiShop.E_ItemType.Weapon:
			ShowWeaponInfo(itemInfo);
			break;
		case GuiShop.E_ItemType.Item:
			ShowItemInfo(itemInfo);
			break;
		case GuiShop.E_ItemType.Fund:
			ShowFundInfo(itemInfo);
			break;
		case GuiShop.E_ItemType.Upgrade:
			ShowUpgradeInfo(itemInfo);
			break;
		default:
			Debug.LogError("Unsupported type: " + itemId.ItemType);
			break;
		}
	}

	private void ShowEmptySelection()
	{
		HideSelectionSpecificInfo();
		m_BuyButton.Widget.Show(false, true);
		m_UpgradeButton.Widget.Show(false, true);
		m_Cost_Sprite.Widget.Show(false, true);
		m_CostSpriteGold.Widget.Show(false, false);
		m_CostSpriteMoney.Widget.Show(false, false);
		m_NameLabel.SetNewText(210000);
		m_ItemDesc.SetNewText(210001);
		m_ItemDesc.Widget.Show(true, true);
	}

	private void HideSelectionSpecificInfo()
	{
		HideWeaponInfo();
		HideItemInfo();
		HideFundInfo();
		HideUpgradeInfo();
	}

	private void ShowWeaponInfo(ShopItemInfo inf)
	{
		GuiBaseUtils.ShowPivotWidgets(m_WeaponInfo_Pivot, true);
		m_WeaponDesc.SetNewText(inf.Description);
		m_DamageLabel.SetNewText(ShopDataBridge.FormatDamage(inf.WeaponDamage));
		m_AccuracyLabel.SetNewText(inf.WeaponAccuracy.ToString());
		m_RangeLabel.SetNewText(inf.WeaponRange.ToString());
		m_ClipLabel.SetNewText(inf.WeaponClip.ToString());
		bool on = inf.MaxUpgrade > 0 && inf.Upgrade > 0;
		m_UpgradeSprite.Show(on, inf.Upgrade);
	}

	private void HideWeaponInfo()
	{
		GuiBaseUtils.ShowPivotWidgets(m_WeaponInfo_Pivot, false);
	}

	private void ShowItemInfo(ShopItemInfo inf)
	{
		GuiBaseUtils.ShowPivotWidgets(m_ItemInfo_Pivot, true);
		m_ItemDesc.SetNewText(inf.Description);
		m_OwnedCount.SetNewText(inf.OwnedCount.ToString());
		if (GuiShopMenu.Instance.TutorialMode)
		{
			m_Layout.GetWidget("OwnedCount_Sprite").Show(false, true);
		}
		m_InfiniteSprite.Widget.Show(inf.InfiniteUse, true);
	}

	private void HideItemInfo()
	{
		GuiBaseUtils.ShowPivotWidgets(m_ItemInfo_Pivot, false);
	}

	private void ShowFundInfo(ShopItemInfo inf)
	{
		GuiBaseUtils.ShowPivotWidgets(m_FundInfo_Pivot, true);
		m_FundDesc.SetNewText(inf.Description);
	}

	private void HideFundInfo()
	{
		GuiBaseUtils.ShowPivotWidgets(m_FundInfo_Pivot, false);
	}

	private void ShowUpgradeInfo(ShopItemInfo inf)
	{
		GuiBaseUtils.ShowPivotWidgets(m_UpgradeInfo_Pivot, true);
		m_UpgradeDesc.SetNewText(inf.Description);
	}

	private void HideUpgradeInfo()
	{
		GuiBaseUtils.ShowPivotWidgets(m_UpgradeInfo_Pivot, false);
	}

	public void Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, false);
		m_Pivot.Show(false);
	}

	public void Update()
	{
		if (m_PulseUpgradeStartTime > 0f)
		{
			float fadeAlpha = GuiShopMenu.PulseAlpha(m_PulseUpgradeStartTime);
			m_UpgradeButton.Widget.m_FadeAlpha = fadeAlpha;
			m_UpgradeButton_BG.Widget.m_FadeAlpha = fadeAlpha;
		}
		if (m_PulseBuyStartTime > 0f)
		{
			float fadeAlpha2 = GuiShopMenu.PulseAlpha(m_PulseUpgradeStartTime);
			m_BuyButton.Widget.m_FadeAlpha = fadeAlpha2;
			m_Buy_Button_BG.Widget.m_FadeAlpha = fadeAlpha2;
		}
	}

	private void StartHighlightBuy()
	{
		m_PulseBuyStartTime = Time.time;
		m_BuyButton.Widget.m_FadeAlpha = 1f;
		m_Buy_Button_BG.Widget.m_FadeAlpha = 1f;
	}

	private void StopHighlightBuy()
	{
		m_PulseBuyStartTime = 0f;
		m_BuyButton.Widget.m_FadeAlpha = 1f;
		m_Buy_Button_BG.Widget.m_FadeAlpha = 1f;
	}
}
