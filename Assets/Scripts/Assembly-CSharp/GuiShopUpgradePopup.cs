using UnityEngine;

internal class GuiShopUpgradePopup : BasePopupScreen
{
	public static GuiShopUpgradePopup Instance;

	private GUIBase_Layout m_Layout;

	private GUIBase_Label m_Caption_Label;

	private GUIBase_Sprite m_BigThumbnail;

	private GUIBase_Label m_Sale_Label;

	private GuiShopFunds m_Cost;

	private GUIBase_Label m_DamageLabel;

	private GUIBase_Label m_AccuracyLabel;

	private GUIBase_Label m_RangeLabel;

	private GUIBase_Label m_ClipLabel;

	private GUIBase_Label m_DamageLabel2;

	private GUIBase_Label m_AccuracyLabel2;

	private GUIBase_Label m_RangeLabel2;

	private GUIBase_Label m_ClipLabel2;

	private GuiShopUpgradeSprite m_UpgradeSprite;

	private GuiShopUpgradeSprite m_UpgradeSprite2;

	private GUIBase_Sprite m_LockedSprite;

	private GUIBase_Label m_RankLabel;

	private GUIBase_Button m_AcceptButton;

	private ShopItemId m_BuyItemId = ShopItemId.EmptyId;

	public bool IsShown { get; private set; }

	private void Awake()
	{
		Instance = this;
		IsShown = false;
	}

	public override void SetCaption(string inCaption)
	{
	}

	public override void SetText(string inText)
	{
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("ShopPopups");
			m_Layout = pivot.GetLayout("Upgrade_Layout");
			m_Caption_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Caption_Label");
			m_BigThumbnail = GuiBaseUtils.PrepareSprite(m_Layout, "BigThumbnail");
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Back_Button", null, OnCloseButton);
			m_AcceptButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Accept_Button", null, OnAcceptButton);
			m_Sale_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Sale_Label");
			m_Cost = new GuiShopFunds(GuiBaseUtils.PrepareSprite(m_Layout, "Cost_Sprite"));
			m_DamageLabel = GuiBaseUtils.PrepareLabel(m_Layout, "DamageInfo_Label");
			m_AccuracyLabel = GuiBaseUtils.PrepareLabel(m_Layout, "AccuracyInfo_Label");
			m_RangeLabel = GuiBaseUtils.PrepareLabel(m_Layout, "RangeInfo_Label");
			m_ClipLabel = GuiBaseUtils.PrepareLabel(m_Layout, "ClipInfo_Label");
			m_DamageLabel2 = GuiBaseUtils.PrepareLabel(m_Layout, "DamageInfo_Label2");
			m_AccuracyLabel2 = GuiBaseUtils.PrepareLabel(m_Layout, "AccuracyInfo_Label2");
			m_RangeLabel2 = GuiBaseUtils.PrepareLabel(m_Layout, "RangeInfo_Label2");
			m_ClipLabel2 = GuiBaseUtils.PrepareLabel(m_Layout, "ClipInfo_Label2");
			m_UpgradeSprite = new GuiShopUpgradeSprite(GuiBaseUtils.PrepareSprite(m_Layout, "Upgrade_Sprite"));
			m_UpgradeSprite2 = new GuiShopUpgradeSprite(GuiBaseUtils.PrepareSprite(m_Layout, "Upgrade2_Sprite"));
			m_LockedSprite = GuiBaseUtils.PrepareSprite(m_Layout, "LockedSprite");
			m_RankLabel = GuiBaseUtils.GetChildLabel(m_LockedSprite.Widget, "Required_Label");
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		base.OnGUI_Show();
		if (m_BuyItemId == ShopItemId.EmptyId)
		{
			Debug.LogError("Call SetBuyItem with valid item id first!");
			return;
		}
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(m_BuyItemId);
		string newText = TextDatabase.instance[2030026] + " " + itemInfo.NextUpgrade + " " + TextDatabase.instance[2030029] + " " + itemInfo.MaxUpgrade + "/ " + TextDatabase.instance[itemInfo.NameTextId];
		m_Caption_Label.SetNewText(newText);
		m_BigThumbnail.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		if (itemInfo.PriceSale)
		{
			m_Sale_Label.SetNewText(itemInfo.DiscountTag);
		}
		m_Sale_Label.Widget.Show(itemInfo.PriceSale, true);
		m_Cost.Show(true);
		m_Cost.SetValue(itemInfo.UpgradeCost, itemInfo.UpgradeGoldCurrency);
		m_DamageLabel.SetNewText(ShopDataBridge.FormatDamage(itemInfo.WeaponDamage));
		m_AccuracyLabel.SetNewText(itemInfo.WeaponAccuracy.ToString());
		m_RangeLabel.SetNewText(itemInfo.WeaponRange.ToString());
		m_ClipLabel.SetNewText(itemInfo.WeaponClip.ToString());
		int dmg = itemInfo.WeaponDamageMax - itemInfo.WeaponDamage;
		int num = itemInfo.WeaponAccuracyMax - itemInfo.WeaponAccuracy;
		int num2 = itemInfo.WeaponRangeMax - itemInfo.WeaponRange;
		int num3 = itemInfo.WeaponClipMax - itemInfo.WeaponClip;
		m_DamageLabel2.SetNewText(ShopDataBridge.FormatDamage(dmg));
		m_AccuracyLabel2.SetNewText(num.ToString());
		m_RangeLabel2.SetNewText(num2.ToString());
		m_ClipLabel2.SetNewText(num3.ToString());
		bool on = itemInfo.Owned && itemInfo.Upgrade > 0;
		m_UpgradeSprite.Show(on, itemInfo.Upgrade);
		m_UpgradeSprite2.Show(true, itemInfo.NextUpgrade);
		bool flag = itemInfo.UpgradeRank > ShopDataBridge.Instance.PlayerLevel;
		if (itemInfo.UpgradeRank > ShopDataBridge.Instance.PlayerLevel)
		{
			m_LockedSprite.Widget.Show(true, true);
			m_RankLabel.SetNewText(itemInfo.UpgradeRank.ToString());
		}
		else
		{
			m_LockedSprite.Widget.Show(false, true);
		}
		m_AcceptButton.SetDisabled(flag);
		m_AcceptButton.Widget.Show(!flag, true);
		MFGuiManager.Instance.ShowLayout(m_Layout, true);
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		if (base.isVisible)
		{
		}
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		m_Layout.EnableControls(true);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_Layout.EnableControls(false);
		base.OnGUI_Disable();
	}

	public void SetBuyItem(ShopItemId itemId)
	{
		m_BuyItemId = itemId;
	}

	public ShopItemId GetBuyItem()
	{
		return m_BuyItemId;
	}

	private void OnCloseButton(bool inside)
	{
		if (inside)
		{
			m_OwnerMenu.Back();
			SendResult(E_PopupResultCode.Cancel);
		}
	}

	private void OnAcceptButton(bool inside)
	{
		if (inside)
		{
			if (!ShopDataBridge.Instance.HaveEnoughMoneyForUpgrade(m_BuyItemId))
			{
				GuiShopNotFundsPopup.Instance.DesiredItem = m_BuyItemId;
				GuiShopNotFundsPopup.Instance.IsUpgrade = true;
				m_OwnerMenu.ShowPopup("NotFundsPopup", string.Empty, string.Empty, NoFundsResultHandler);
			}
			else
			{
				m_OwnerMenu.Back();
				SendResult(E_PopupResultCode.Success);
			}
		}
	}

	private void NoFundsResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success && ShopDataBridge.Instance.HaveEnoughMoneyForUpgrade(m_BuyItemId))
		{
			m_OwnerMenu.Back();
			SendResult(E_PopupResultCode.Success);
		}
	}
}
