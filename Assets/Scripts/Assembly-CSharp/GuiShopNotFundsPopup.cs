using UnityEngine;

public class GuiShopNotFundsPopup : BasePopupScreen
{
	public static GuiShopNotFundsPopup Instance;

	public bool RemoveFromTopHack;

	private GUIBase_Layout m_Layout;

	private GUIBase_Sprite m_Funds_Sprite;

	private GuiShopFunds m_FundsAdd;

	private GUIBase_Button m_AddFunds_Button;

	private ShopItemId m_BuyFundId;

	public ShopItemId DesiredItem { get; set; }

	public bool IsUpgrade { get; set; }

	public ShopItemId RequiredFunds { get; set; }

	private void Awake()
	{
		Instance = this;
		RequiredFunds = ShopItemId.EmptyId;
		RemoveFromTopHack = false;
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
			m_Layout = pivot.GetLayout("NotFunds_Layout");
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Back_Button", null, OnButtonBack);
			m_FundsAdd = new GuiShopFunds(GuiBaseUtils.PrepareSprite(m_Layout, "FundsAdd_Sprite"));
			m_Funds_Sprite = GuiBaseUtils.PrepareSprite(m_Layout, "Funds_Sprite");
			m_AddFunds_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "AddFunds_Button", null, OnAddFunds);
			GuiBaseUtils.PrepareLabel(m_Layout, "Caption_Label");
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
		MFGuiManager.Instance.ShowLayout(m_Layout, true);
		m_BuyFundId = GetRequiredFunds();
		ShowFundInfo(m_BuyFundId);
	}

	private ShopItemId GetRequiredFunds()
	{
		if (RequiredFunds != null && !RequiredFunds.IsEmpty())
		{
			return RequiredFunds;
		}
		int fundsNeeded;
		bool isGold;
		if (IsUpgrade)
		{
			ShopDataBridge.Instance.RequiredFundsForUpgrade(DesiredItem, out fundsNeeded, out isGold);
		}
		else
		{
			ShopDataBridge.Instance.RequiredFunds(DesiredItem, out fundsNeeded, out isGold);
		}
		return ShopDataBridge.Instance.FindFundsItem(fundsNeeded, isGold);
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, false);
		RequiredFunds = ShopItemId.EmptyId;
		RemoveFromTopHack = false;
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
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

	private void ShowFundInfo(ShopItemId id)
	{
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(id);
		if (itemInfo.AddGold > 0)
		{
			m_FundsAdd.SetValue(itemInfo.AddGold, true, true);
		}
		else
		{
			m_FundsAdd.SetValue(itemInfo.AddMoney, false, true);
		}
		m_FundsAdd.Show(!ShopDataBridge.Instance.IsFreeGold(id));
		m_Funds_Sprite.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		m_AddFunds_Button.Widget.Show(true, true);
		GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(m_AddFunds_Button.Widget, "GUIBase_Label");
		if ((bool)childLabel)
		{
			if (ShopDataBridge.Instance.IsFreeGold(id))
			{
				childLabel.SetNewText(2030058);
			}
			else if (itemInfo.Cost > 0)
			{
				childLabel.SetNewText(2030040);
			}
			else
			{
				childLabel.SetNewText(2030036);
			}
		}
		bool disabled = (itemInfo.GoldCurrency && itemInfo.Cost > ShopDataBridge.Instance.PlayerGold) || (!itemInfo.GoldCurrency && itemInfo.Cost > ShopDataBridge.Instance.PlayerMoney);
		m_AddFunds_Button.SetDisabled(disabled);
	}

	private void OnButtonBack(bool inside)
	{
		if (inside)
		{
			Close(E_PopupResultCode.Cancel);
		}
	}

	private void OnAddFunds(bool inside)
	{
		if (!inside)
		{
			return;
		}
		if (ShopDataBridge.Instance.IsIAPFund(m_BuyFundId))
		{
			if (ShopDataBridge.Instance.IAPServiceAvailable())
			{
				m_OwnerMenu.ShowPopup("ShopStatusIAP", TextDatabase.instance[2030044], TextDatabase.instance[2030045]);
				ShopDataBridge.Instance.IAPRequestPurchase(m_BuyFundId, delegate(IAP.E_Buy result)
				{
					int i = 2030049;
					switch (result)
					{
					case IAP.E_Buy.Success:
						i = 2030048;
						break;
					case IAP.E_Buy.UserCancelled:
						i = 2030050;
						break;
					case IAP.E_Buy.Failure:
						i = 2030049;
						break;
					case IAP.E_Buy.Fatal:
						i = 2030049;
						break;
					}
					m_OwnerMenu.Back();
					m_OwnerMenu.ShowPopup("ShopMessageBox", TextDatabase.instance[2030046], TextDatabase.instance[i], WaitForIAPurchaseHandler);
				});
			}
			else
			{
				m_OwnerMenu.ShowPopup("ShopMessageBox", TextDatabase.instance[2030046], TextDatabase.instance[2030047], NoIAPServiceHandler);
			}
		}
		else
		{
			Debug.LogError("Fund item is not IAP: " + m_BuyFundId);
		}
	}

	private void WaitForIAPurchaseHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		Close(inResult);
	}

	private void NoIAPServiceHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		Close(E_PopupResultCode.Failed);
	}

	private void Close(E_PopupResultCode result)
	{
		if (RemoveFromTopHack)
		{
			GuiMainMenu.Instance.ClosePopupScreenHack();
		}
		else
		{
			m_OwnerMenu.Back();
		}
		RequiredFunds = ShopItemId.EmptyId;
		RemoveFromTopHack = false;
		SendResult(result);
	}
}
