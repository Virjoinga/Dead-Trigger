using System.Collections.Generic;

internal class GuiShopCategoryTabs
{
	private class CatInfo
	{
		public GUIBase_Button m_Button;

		public ShopItemId m_LastSelection;
	}

	public delegate void CategoryDelegate(E_ShopCategory cat, ShopItemId id);

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Widget m_ShopButtons_Widget;

	private GUIBase_Widget m_FundsButtons_Widget;

	private GUIBase_Widget m_TutorialShop;

	private Dictionary<string, E_ShopCategory> m_CategoryDict = new Dictionary<string, E_ShopCategory>();

	private Dictionary<E_ShopCategory, CatInfo> m_TabButtonsDict = new Dictionary<E_ShopCategory, CatInfo>();

	public CategoryDelegate m_CategoryDelegate;

	public void GuiInit()
	{
		m_Pivot = MFGuiManager.Instance.GetPivot("ShopMenu");
		m_Layout = m_Pivot.GetLayout("Main_Layout");
		m_ShopButtons_Widget = m_Layout.GetWidget("ShopButtons");
		m_FundsButtons_Widget = m_Layout.GetWidget("FundsButtons");
		m_TutorialShop = m_Layout.GetWidget("Tutorial_Shop");
		AddCategory("Guns_Button", E_ShopCategory.Weapons);
		AddCategory("Items_Button", E_ShopCategory.Items);
		AddCategory("Upgrades_Button", E_ShopCategory.Upgrade);
		AddCategory("Funds_Button", E_ShopCategory.Funds);
		GuiBaseUtils.RegisterButtonDelegate(m_Layout, "FreeGold_Button", OnFreeGoldTouch, null, null);
	}

	public void ShowShop()
	{
		m_ShopButtons_Widget.Show(true, true);
		m_FundsButtons_Widget.Show(false, true);
	}

	public void ShowFunds()
	{
		m_ShopButtons_Widget.Show(false, true);
		m_FundsButtons_Widget.Show(true, true);
	}

	public void DisableByTutorial(bool dis)
	{
		GUIBase_Button button = m_TabButtonsDict[E_ShopCategory.Weapons].m_Button;
		button.SetDisabled(dis);
		button.Widget.Show(!dis, true);
		GUIBase_Button button2 = m_TabButtonsDict[E_ShopCategory.Upgrade].m_Button;
		button2.SetDisabled(dis);
		button2.Widget.Show(!dis, true);
		m_TutorialShop.Show(dis, true);
	}

	private void AddCategory(string btnName, E_ShopCategory cat)
	{
		m_CategoryDict.Add(btnName, cat);
		GUIBase_Button button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, btnName, OnCategoryTouch, null, null);
		m_TabButtonsDict.Add(cat, new CatInfo
		{
			m_Button = button,
			m_LastSelection = ShopItemId.EmptyId
		});
	}

	private void ClearHighlight()
	{
		foreach (KeyValuePair<E_ShopCategory, CatInfo> item in m_TabButtonsDict)
		{
			item.Value.m_Button.SetDisabled(false);
		}
	}

	private void OnCategoryTouch(GUIBase_Widget w)
	{
		if (m_CategoryDict.ContainsKey(w.name) && !GuiShopMenu.Instance.TutorialMode)
		{
			E_ShopCategory e_ShopCategory = m_CategoryDict[w.name];
			Highlight(e_ShopCategory);
			if (m_CategoryDelegate != null)
			{
				m_CategoryDelegate(e_ShopCategory, m_TabButtonsDict[e_ShopCategory].m_LastSelection);
			}
		}
	}

	private void OnFreeGoldTouch(GUIBase_Widget w)
	{
		TapjoyPlugin.ShowOffers();
	}

	public void Highlight(E_ShopCategory c)
	{
		ClearHighlight();
		m_TabButtonsDict[c].m_Button.SetDisabled(true);
	}

	public void SetLastSelection(E_ShopCategory c, ShopItemId selId)
	{
		m_TabButtonsDict[c].m_LastSelection = selId;
	}

	public ShopItemId GetLastSelection(E_ShopCategory c)
	{
		return m_TabButtonsDict[c].m_LastSelection;
	}
}
