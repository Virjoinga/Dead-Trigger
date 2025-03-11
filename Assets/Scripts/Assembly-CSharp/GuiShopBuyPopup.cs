using UnityEngine;

internal class GuiShopBuyPopup : BasePopupScreen
{
	public enum E_BuyType
	{
		Buy = 0,
		Upgrade = 1,
		AddFunds = 2,
		ConvertFunds = 3
	}

	public static GuiShopBuyPopup Instance;

	private GUIBase_Layout m_Layout;

	private GUIBase_Label m_Caption_Label;

	private GUIBase_Sprite m_BigThumbnail;

	private GUIBase_Label m_Sale_Label;

	private GuiShopFunds m_Cost;

	private ShopItemId m_BuyItemId = ShopItemId.EmptyId;

	private int m_CaptionID;

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
			m_Layout = pivot.GetLayout("Buy_Layout");
			m_Caption_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Caption_Label");
			m_BigThumbnail = GuiBaseUtils.PrepareSprite(m_Layout, "BigThumbnail");
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Back_Button", null, OnCloseButton);
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Accept_Button", null, OnAcceptButton);
			m_Sale_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Sale_Label");
			m_Cost = new GuiShopFunds(GuiBaseUtils.PrepareSprite(m_Layout, "Cost_Sprite"));
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
		string newText = TextDatabase.instance[m_CaptionID] + " " + TextDatabase.instance[itemInfo.NameTextId];
		m_Caption_Label.SetNewText(newText);
		m_BigThumbnail.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		if (itemInfo.PriceSale)
		{
			m_Sale_Label.SetNewText(itemInfo.DiscountTag);
		}
		m_Sale_Label.Widget.Show(itemInfo.PriceSale, true);
		m_Cost.Show(true);
		if (m_BuyItemId.ItemType == GuiShop.E_ItemType.Fund)
		{
			bool flag = itemInfo.AddGold > 0;
			m_Cost.SetValue((!flag) ? itemInfo.AddMoney : itemInfo.AddGold, flag, true);
		}
		else
		{
			m_Cost.SetValue(itemInfo.Cost, itemInfo.GoldCurrency);
		}
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

	private int CaptionId(E_BuyType t)
	{
		switch (t)
		{
		case E_BuyType.Buy:
			return 2030025;
		case E_BuyType.Upgrade:
			return 2030026;
		case E_BuyType.AddFunds:
			return 2030027;
		case E_BuyType.ConvertFunds:
			return 2030028;
		default:
			Debug.LogError("Unhandled case: " + t);
			return 0;
		}
	}

	public void SetCaptionID(E_BuyType type)
	{
		Debug.Log("SetCaptionID: " + m_CaptionID);
		m_CaptionID = CaptionId(type);
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
			if (!ShopDataBridge.Instance.HaveEnoughMoney(m_BuyItemId))
			{
				GuiShopNotFundsPopup.Instance.DesiredItem = m_BuyItemId;
				GuiShopNotFundsPopup.Instance.IsUpgrade = false;
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
		if (inResult == E_PopupResultCode.Success && ShopDataBridge.Instance.HaveEnoughMoney(m_BuyItemId))
		{
			m_OwnerMenu.Back();
			SendResult(E_PopupResultCode.Success);
		}
	}
}
