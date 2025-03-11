using System;
using System.Collections.Generic;
using UnityEngine;

public class GuiShopMenu : BaseMenuScreen
{
	public static GuiShopMenu Instance;

	public GameObject ScrollBarPrefab;

	private ItemSelectionPanel m_ItemSelectionPanel;

	private GuiShopCategoryTabs m_CategoryTabs = new GuiShopCategoryTabs();

	private E_ShopCategory m_CurrentCategory;

	private E_ShopCategory m_LastCategory;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Button m_BackButton;

	private float m_PulseBackButton;

	public bool IsShown { get; private set; }

	public bool TutorialMode { get; set; }

	public static float PulseAlpha(float startTime)
	{
		float num = 1f - Mathf.PingPong(Time.time - startTime, 1f) / 1f;
		return num * 0.6f + 0.4f;
	}

	private void Awake()
	{
		Instance = this;
		IsShown = false;
		m_ItemSelectionPanel = new ItemSelectionPanel(ScrollBarPrefab);
		m_ItemSelectionPanel.m_DebugName = "Shop panel";
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_Pivot = MFGuiManager.Instance.GetPivot("ShopMenu");
			m_Layout = m_Pivot.GetLayout("Main_Layout");
			m_BackButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Back_Button", null, OnButtonBack);
			m_ItemSelectionPanel.InitGui();
			m_CategoryTabs.GuiInit();
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		if (ShopDataBridge.Instance == null)
		{
			ShopDataBridge.CreateInstance();
		}
		if (!ShopDataBridge.Instance.IAPServiceAvailable())
		{
			ShopDataBridge.Instance.IAPInit();
		}
		MFGuiManager.Instance.ShowLayout(m_Layout, true);
		m_ItemSelectionPanel.Show(OnBuyButton, OnUpgradeButton, OnEquipButton, OnFreeGoldButton);
		GuiShopCategoryTabs categoryTabs = m_CategoryTabs;
		categoryTabs.m_CategoryDelegate = (GuiShopCategoryTabs.CategoryDelegate)Delegate.Combine(categoryTabs.m_CategoryDelegate, new GuiShopCategoryTabs.CategoryDelegate(SwitchToCategory));
		if (TutorialMode)
		{
			Debug.Log("Tutorial:" + TutorialMode);
			SwitchToCategory(E_ShopCategory.Items, new ShopItemId(1, GuiShop.E_ItemType.Item));
			m_CategoryTabs.DisableByTutorial(true);
			TutorialBackButton();
		}
		else
		{
			m_CategoryTabs.DisableByTutorial(false);
			SwitchToCategory(m_LastCategory, ShopItemId.EmptyId);
		}
		IsShown = true;
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, false);
		m_ItemSelectionPanel.Hide();
		GuiShopCategoryTabs categoryTabs = m_CategoryTabs;
		categoryTabs.m_CategoryDelegate = (GuiShopCategoryTabs.CategoryDelegate)Delegate.Remove(categoryTabs.m_CategoryDelegate, new GuiShopCategoryTabs.CategoryDelegate(SwitchToCategory));
		IsShown = false;
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		if (IsShown)
		{
			m_ItemSelectionPanel.Update();
			if (TutorialMode && m_PulseBackButton > 0f)
			{
				m_BackButton.Widget.m_FadeAlpha = PulseAlpha(m_PulseBackButton);
			}
		}
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		Debug.Log("Enable Shop menu");
		m_Pivot.EnableControls(true);
		m_ItemSelectionPanel.EnableControls(true);
		m_ItemSelectionPanel.UpdateItemsViews();
		m_ItemSelectionPanel.HideScrollbar(false);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_Pivot.EnableControls(false);
		m_ItemSelectionPanel.EnableControls(false);
		m_ItemSelectionPanel.HideScrollbar(true);
		base.OnGUI_Disable();
	}

	public void SwitchToCategory(E_ShopCategory cat, ShopItemId selId)
	{
		bool flag = ((!TutorialMode) ? true : false);
		List<ShopItemId> items = null;
		switch (cat)
		{
		case E_ShopCategory.Weapons:
			m_CategoryTabs.ShowShop();
			items = ShopDataBridge.Instance.GetWeapons();
			break;
		case E_ShopCategory.Items:
			m_CategoryTabs.ShowShop();
			items = ((!TutorialMode) ? ShopDataBridge.Instance.GetItems() : ShopDataBridge.Instance.GetItemsTutorial());
			break;
		case E_ShopCategory.Funds:
			flag = false;
			m_CategoryTabs.ShowFunds();
			items = ShopDataBridge.Instance.GetFunds();
			break;
		case E_ShopCategory.Upgrade:
			m_CategoryTabs.ShowShop();
			items = ShopDataBridge.Instance.GetUpgrades();
			break;
		default:
			Debug.LogError("TODO: support type " + cat);
			break;
		}
		m_CategoryTabs.SetLastSelection(m_CurrentCategory, m_ItemSelectionPanel.GetSelectedItem());
		m_CurrentCategory = cat;
		if (flag)
		{
			m_LastCategory = cat;
		}
		m_ItemSelectionPanel.Insert(items);
		m_ItemSelectionPanel.Show(OnBuyButton, OnUpgradeButton, OnEquipButton, OnFreeGoldButton);
		if (selId.IsEmpty())
		{
			ShopItemId lastSelection = m_CategoryTabs.GetLastSelection(m_CurrentCategory);
			m_ItemSelectionPanel.SelectItem((!lastSelection.IsEmpty()) ? lastSelection : m_ItemSelectionPanel.GetSelectedItem());
		}
		else
		{
			m_ItemSelectionPanel.SelectItem(selId);
		}
		m_CategoryTabs.Highlight(m_CurrentCategory);
	}

	private void DisableBackButton(bool dis)
	{
		Debug.Log("Back button disabled:" + dis);
		m_BackButton.Widget.Show(!dis, true);
	}

	private void StartHighlightBackButton()
	{
		m_PulseBackButton = Time.time;
		m_BackButton.Widget.m_FadeAlpha = 1f;
	}

	private void StopHighlightBackButton()
	{
		m_PulseBackButton = 0f;
		m_BackButton.Widget.m_FadeAlpha = 1f;
	}

	private void OnEquipButton(bool inside)
	{
		if (inside)
		{
			GuiMainMenu.Instance.RemoveTopFromStackHack();
			m_OwnerMenu.ShowScreen("Equip", true);
		}
	}

	private void OnBuyButton(bool inside)
	{
		if (!inside)
		{
			return;
		}
		ShopItemId selectedItem = m_ItemSelectionPanel.GetSelectedItem();
		if (ShopDataBridge.Instance.IsIAPFund(selectedItem))
		{
			if (ShopDataBridge.Instance.IAPServiceAvailable())
			{
				m_OwnerMenu.ShowPopup("ShopStatusIAP", TextDatabase.instance[2030044], TextDatabase.instance[2030045]);
				ShopDataBridge.Instance.IAPRequestPurchase(selectedItem, delegate(IAP.E_Buy result)
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
				m_OwnerMenu.ShowPopup("ShopMessageBox", TextDatabase.instance[2030046], TextDatabase.instance[2030047], WaitForIAPurchaseHandler);
			}
		}
		else
		{
			GuiShopBuyPopup.Instance.SetCaptionID(GetConfirmType());
			GuiShopBuyPopup.Instance.SetBuyItem(selectedItem);
			m_OwnerMenu.ShowPopup("ShopBuyPopup", string.Empty, string.Empty, BuyResultHandler);
		}
	}

	private void WaitForIAPurchaseHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		m_ItemSelectionPanel.UpdateItemsViews();
	}

	private void OnUpgradeButton(bool inside)
	{
		if (inside)
		{
			ShopItemId selectedItem = m_ItemSelectionPanel.GetSelectedItem();
			GuiShopUpgradePopup.Instance.SetBuyItem(selectedItem);
			m_OwnerMenu.ShowPopup("ShopUpgradePopup", string.Empty, string.Empty, UpgradeResultHandler);
		}
	}

	private void OnFreeGoldButton(bool inside)
	{
		if (inside)
		{
			ShopItemId selectedItem = m_ItemSelectionPanel.GetSelectedItem();
			ShowFreeGoldOffer(selectedItem);
		}
	}

	public static void ShowFreeGoldOffer(ShopItemId goldId)
	{
		if (ShopDataBridge.Instance.IsFreeGold(goldId))
		{
			switch ((E_FundID)goldId.Id)
			{
			case E_FundID.TapJoyWeb:
				Debug.Log("Free gold: Tapjoy Web");
				Etcetera.ShowWeb("https://www.tapjoy.com/earn?eid=ab8c8296e98e6ef62a40221a7a17a6dd394f7d0dbdc5948f4828ae02c65220f03a1be4186eb3e2531a52c2a0c033b9c0&referral=madfinger_deadtrigger");
				break;
			case E_FundID.TapJoyInApp:
				Debug.Log("Free gold: Tapjoy Offerwall");
				TapjoyPlugin.ShowOffers();
				break;
			}
		}
	}

	private void BuyResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			ShopItemId selectedItem = m_ItemSelectionPanel.GetSelectedItem();
			if (selectedItem.ItemType != GuiShop.E_ItemType.Fund)
			{
				ShopDataBridge.Instance.SynchroniseBoughtItem(selectedItem);
				GuiEquipMenu.Instance.TryEquipBoughtItem(selectedItem);
				Game.Instance.PlayerPersistentInfo.Save();
			}
			m_ItemSelectionPanel.UpdateItemsViews();
			TutorialBackButton();
		}
	}

	private void UpgradeResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			ShopItemId selectedItem = m_ItemSelectionPanel.GetSelectedItem();
			ShopDataBridge.Instance.UpgradeWeapon(selectedItem);
			Game.Instance.PlayerPersistentInfo.Save();
			m_ItemSelectionPanel.UpdateItemsViews();
		}
	}

	private string GetDBText(int textId)
	{
		return TextDatabase.instance[textId];
	}

	private void MessageBoxResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			m_OwnerMenu.Back();
		}
	}

	private GuiShopBuyPopup.E_BuyType GetConfirmType()
	{
		if (m_CurrentCategory != E_ShopCategory.Funds)
		{
			return GuiShopBuyPopup.E_BuyType.Buy;
		}
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(m_ItemSelectionPanel.GetSelectedItem());
		if (itemInfo != null && itemInfo.Cost > 0)
		{
			return GuiShopBuyPopup.E_BuyType.ConvertFunds;
		}
		return GuiShopBuyPopup.E_BuyType.AddFunds;
	}

	public void OnButtonBack(bool inside)
	{
		if (inside)
		{
			Game.Instance.PlayerPersistentInfo.Save();
			m_OwnerMenu.Back();
		}
	}

	private void TutorialBackButton()
	{
		if (TutorialMode)
		{
			ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(new ShopItemId(1, GuiShop.E_ItemType.Item));
			if (!itemInfo.Owned || itemInfo.OwnedCount < itemInfo.MissionMaxCount)
			{
				Instance.DisableBackButton(true);
				GuiMainMenu.Instance.DisableBack = true;
				return;
			}
			TutorialMode = false;
			Instance.DisableBackButton(false);
			Instance.OnButtonBack(true);
			GuiMainMenu.Instance.DisableBack = false;
		}
	}
}
