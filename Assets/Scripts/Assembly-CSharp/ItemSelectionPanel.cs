using System.Collections.Generic;
using UnityEngine;

internal class ItemSelectionPanel
{
	private GuiShopItemScroller m_ShopScroller;

	private GuiItemInfoPanel m_ItemInfoPanel = new GuiItemInfoPanel();

	private GUIBase_Layout m_Layout;

	private bool m_IsInitialized;

	private bool IsShown;

	public string m_DebugName;

	private GUIBase_Button.ReleaseDelegate m_OnBuyButton;

	private GUIBase_Button.ReleaseDelegate m_OnUpgradeButton;

	private GUIBase_Button.ReleaseDelegate m_OnEquipButton;

	private GUIBase_Button.ReleaseDelegate m_OnFreeGoldButton;

	public ItemSelectionPanel(GameObject itemTemplate)
	{
		m_ShopScroller = new GuiShopItemScroller(itemTemplate, "Shop scroller");
		m_ShopScroller.m_DebugName = "Shop scroller";
	}

	public void Update()
	{
		if (IsShown)
		{
			m_ShopScroller.Update();
			m_ItemInfoPanel.Update();
		}
	}

	public void InitGui()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("ShopMenu");
		m_Layout = pivot.GetLayout("InfoPanel_Layout");
		m_ShopScroller.InitGui(OnSelectionChange);
		GUIBase_Pivot pivot2 = MFGuiManager.Instance.GetPivot("ShopFocused_Pivot");
		m_ItemInfoPanel.InitGui(m_Layout, pivot2);
		m_IsInitialized = true;
	}

	public void Insert(List<ShopItemId> items)
	{
		m_ShopScroller.Insert(items, false, ShopDataBridge.CompareShop);
	}

	public void Hide()
	{
		m_ShopScroller.Hide();
		m_ItemInfoPanel.Hide();
		IsShown = false;
	}

	public void Show(GUIBase_Button.ReleaseDelegate OnBuyButton, GUIBase_Button.ReleaseDelegate OnUpgradeButton, GUIBase_Button.ReleaseDelegate OnEquipButton, GUIBase_Button.ReleaseDelegate OnFreeGoldButton)
	{
		m_OnBuyButton = OnBuyButton;
		m_OnUpgradeButton = OnUpgradeButton;
		m_OnEquipButton = OnEquipButton;
		m_OnFreeGoldButton = OnFreeGoldButton;
		m_ShopScroller.Show();
		ShopItemId selectedItem = GetSelectedItem();
		m_ItemInfoPanel.Show(selectedItem, m_OnBuyButton, m_OnUpgradeButton, m_OnEquipButton, m_OnFreeGoldButton);
		IsShown = true;
	}

	public void SelectItem(ShopItemId id)
	{
		m_ShopScroller.SetSelectedItem(id);
	}

	public ShopItemId GetSelectedItem()
	{
		return m_ShopScroller.GetSelectedItem();
	}

	private void OnSelectionChange()
	{
		if (m_IsInitialized && IsShown && m_ShopScroller != null)
		{
			if (m_ShopScroller.HasSelection())
			{
				ShopItemId selectedItem = GetSelectedItem();
				m_ItemInfoPanel.Show(selectedItem, m_OnBuyButton, m_OnUpgradeButton, m_OnEquipButton, m_OnFreeGoldButton);
			}
			else
			{
				m_ItemInfoPanel.Hide();
			}
		}
	}

	public void EnableControls(bool on)
	{
		m_Layout.EnableControls(on);
		m_ShopScroller.EnableGui(on);
	}

	public void UpdateItemsViews()
	{
		m_ShopScroller.UpdateItemsViews();
		ShopItemId selectedItem = GetSelectedItem();
		m_ItemInfoPanel.Show(selectedItem, m_OnBuyButton, m_OnUpgradeButton, m_OnEquipButton, m_OnFreeGoldButton);
	}

	public void HideScrollbar(bool hide)
	{
		if (hide)
		{
			m_ShopScroller.Hide();
			return;
		}
		m_ShopScroller.Show();
		m_ShopScroller.SetSelectedItem(GetSelectedItem());
	}
}
