using System.Collections.Generic;
using UnityEngine;

public class GuiEquipSelection : MonoBehaviour
{
	public delegate void HideDelegate(ShopItemId selWpn, bool cancel);

	public static GuiEquipSelection Instance;

	public GameObject ScrollBarPrefab;

	private bool m_IsInitialized;

	private GUIBase_Layout m_Layout;

	private GUIBase_Button m_Equip_Button;

	private GUIBase_Label m_Equip_Label;

	private GuiShopItemScroller m_ItemScroller;

	private ShopItemId m_EquipedItem = ShopItemId.EmptyId;

	public HideDelegate m_OnEquipDelegate;

	public bool IsShown { get; private set; }

	public ShopItemId LastSelectedItem { get; private set; }

	private void Awake()
	{
		Instance = this;
		m_IsInitialized = false;
		IsShown = false;
		m_ItemScroller = new GuiShopItemScroller(ScrollBarPrefab, "Equip scroller");
		m_ItemScroller.m_DebugName = "Equip scroller";
		LastSelectedItem = ShopItemId.EmptyId;
	}

	private void LateUpdate()
	{
		if (m_IsInitialized && IsShown)
		{
			m_ItemScroller.Update();
		}
	}

	public void InitGui()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("EquipMenu");
		m_Layout = pivot.GetLayout("Main_Layout");
		m_Equip_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Equip_Button", null, OnEquipButton);
		m_Equip_Label = GuiBaseUtils.PrepareLabel(m_Layout, "Equip_Label");
		m_ItemScroller.InitGui(OnSelectionChange);
		m_IsInitialized = true;
	}

	public void Hide()
	{
		m_ItemScroller.Hide();
		m_Equip_Button.Widget.Show(false, true);
		IsShown = false;
	}

	public void Show(GuiShop.E_ItemType type, ShopItemId equipedId, E_Team skinTeam)
	{
		List<ShopItemId> items = null;
		switch (type)
		{
		case GuiShop.E_ItemType.Weapon:
			items = ShopDataBridge.Instance.GetOwnedWeapons();
			break;
		case GuiShop.E_ItemType.Item:
			items = ShopDataBridge.Instance.GetOwnedItems();
			break;
		default:
			Debug.LogError("TODO: support type " + type);
			break;
		}
		m_ItemScroller.Insert(items, true, ShopDataBridge.CompareEquip);
		m_ItemScroller.Show();
		m_Equip_Button.Widget.Show(true, true);
		IsShown = true;
		SetEquipedItem(equipedId);
		SelectItem(equipedId);
	}

	public void SetEquipedItem(ShopItemId equipedId)
	{
		m_EquipedItem = equipedId;
		UpdateItemButtons();
	}

	public void SelectItem(ShopItemId id)
	{
		m_ItemScroller.SetSelectedItem(id);
	}

	private void OnEquipButton(bool inside)
	{
		if (!inside)
		{
			return;
		}
		ShopItemId shopItemId = m_ItemScroller.GetSelectedItem();
		if (shopItemId.ItemType == GuiShop.E_ItemType.Item && ShopDataBridge.Instance.GetItemInfo(shopItemId).OwnedCount == 0)
		{
			GuiMainMenu.Instance.RemoveTopFromStackHack();
			GuiMainMenu.Instance.ShowScreen("Shop");
			GuiShopMenu.Instance.SwitchToCategory((shopItemId.ItemType == GuiShop.E_ItemType.Item) ? E_ShopCategory.Items : E_ShopCategory.Weapons, shopItemId);
			return;
		}
		if (m_EquipedItem.Equals(shopItemId))
		{
			shopItemId = ShopItemId.EmptyId;
		}
		m_OnEquipDelegate(shopItemId, false);
		SetEquipedItem(shopItemId);
		m_ItemScroller.UpdateItemsViews();
	}

	public void UpdateScrollerView()
	{
		m_ItemScroller.UpdateItemsViews();
	}

	private void OnSelectionChange()
	{
		if (m_IsInitialized && IsShown && m_ItemScroller != null)
		{
			LastSelectedItem = m_ItemScroller.GetSelectedItem();
			UpdateItemButtons();
		}
	}

	public void UpdateItemButtons()
	{
		if (!m_IsInitialized || !IsShown)
		{
			return;
		}
		ShopItemId selectedItem = m_ItemScroller.GetSelectedItem();
		if (selectedItem != ShopItemId.EmptyId)
		{
			ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(selectedItem);
			bool flag = selectedItem.ItemType == GuiShop.E_ItemType.Weapon && m_EquipedItem.Equals(selectedItem) && ShopDataBridge.Instance.EquippedWeaponsCount() == 1;
			bool flag2 = itemInfo != null && !flag;
			m_Equip_Button.SetDisabled(!flag2);
			int num = 2050001;
			int num2 = 2050002;
			int num3 = 2050035;
			int num4 = 2030059;
			int newText = num;
			if (selectedItem.ItemType == GuiShop.E_ItemType.Item && itemInfo.OwnedCount == 0)
			{
				newText = num3;
			}
			else if (m_EquipedItem.Equals(selectedItem))
			{
				newText = ((!flag) ? num2 : num4);
			}
			m_Equip_Label.SetNewText(newText);
		}
		else
		{
			m_Equip_Button.SetDisabled(true);
		}
	}

	public ShopItemId GetCurrentSelection()
	{
		return m_ItemScroller.GetSelectedItem();
	}
}
