using System;
using System.Collections.Generic;
using UnityEngine;

internal class GuiShopItemScroller
{
	private const int maxScrollItems = 40;

	private GameObject m_ScrollBarPrefab;

	private GuiScroller<ShopItemId> m_ScrollInventory = new GuiScroller<ShopItemId>();

	private List<GUIBase_Widget> m_ScrollCache = new List<GUIBase_Widget>();

	private GuiShopInfoPopup m_InfoPopup = new GuiShopInfoPopup();

	private bool m_EnableControls = true;

	public string m_DebugName;

	public GuiShopItemScroller(GameObject ScrollBarPrefab, string dbgName)
	{
		m_ScrollBarPrefab = ScrollBarPrefab;
		CreateItemsCache();
		m_ScrollInventory.m_DebugName = dbgName;
	}

	private void CreateItemsCache()
	{
		m_ScrollCache.Clear();
		for (int i = 0; i < 40; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_ScrollBarPrefab) as GameObject;
			GUIBase_Widget component = gameObject.GetComponent<GUIBase_Widget>();
			component.transform.parent = m_ScrollBarPrefab.transform.parent;
			component.transform.localPosition = m_ScrollBarPrefab.transform.localPosition;
			m_ScrollCache.Add(component);
		}
	}

	public void Update()
	{
		if (m_EnableControls)
		{
			m_ScrollInventory.Update();
		}
	}

	public void InitGui(GuiScroller<ShopItemId>.ChangeDelegate onSelectionChange)
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("ShopMenu");
		GUIBase_Layout layout = pivot.GetLayout("Scroller_Layout");
		GUIBase_Pivot pivot2 = MFGuiManager.Instance.GetPivot("Scroll_Pivot");
		m_ScrollInventory.InitGui(layout, pivot2);
		m_ScrollInventory.m_OnSelectionChange = onSelectionChange;
		m_InfoPopup.GuiInit();
		m_ScrollInventory.m_OnHoldBegin = ShowInfoPopup;
		m_ScrollInventory.m_OnHoldEnd = HideInfoPopup;
	}

	public void Insert(List<ShopItemId> items, bool hideOwnedHack, Comparison<ShopItemId> compareDlgt)
	{
		m_ScrollInventory.HideItems();
		m_ScrollInventory.Clear();
		if (items.Count > m_ScrollCache.Count)
		{
			Debug.LogError("Scroll cache too small: size " + m_ScrollCache.Count + ", required " + items.Count);
		}
		items.Sort();
		for (int i = 0; i < items.Count; i++)
		{
			m_ScrollInventory.AddItem(items[i], m_ScrollCache[i], new GuiScrollItem(items[i], m_ScrollCache[i], hideOwnedHack));
		}
	}

	public void Hide()
	{
		m_ScrollInventory.Hide();
	}

	public void Show()
	{
		m_ScrollInventory.Show();
	}

	public void SetSelectedItem(ShopItemId id)
	{
		if (id != null)
		{
			m_ScrollInventory.SetSelectedItem(id);
		}
	}

	public ShopItemId GetSelectedItem()
	{
		if (m_ScrollInventory.HasSelection())
		{
			return m_ScrollInventory.GetSelectedItem();
		}
		return ShopItemId.EmptyId;
	}

	public bool HasSelection()
	{
		return m_ScrollInventory.HasSelection();
	}

	private void ShowInfoPopup(int itemIndex, ShopItemId itemId)
	{
	}

	private void HideInfoPopup()
	{
		m_InfoPopup.Hide();
	}

	public void EnableGui(bool on)
	{
		m_EnableControls = on;
	}

	public void UpdateItemsViews()
	{
		m_ScrollInventory.UpdateItemsViews();
	}
}
