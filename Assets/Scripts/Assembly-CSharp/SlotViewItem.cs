internal class SlotViewItem : ISlotView
{
	private GUIBase_Widget m_RootWidget;

	private GUIBase_Label m_NameLabel;

	private GUIBase_Sprite m_ItemSprite;

	private GUIBase_Label m_CountLabel;

	private GUIBase_Sprite m_LockSprite;

	private GUIBase_Label m_EmptyLabel;

	private GUIBase_Button m_BuyButton;

	public void InitGui(GUIBase_Layout layout, GUIBase_Button btn, int slotId)
	{
		m_RootWidget = btn.Widget;
		m_NameLabel = GuiBaseUtils.GetChildLabel(btn.Widget, "Item_Label");
		m_ItemSprite = GuiBaseUtils.GetChildSprite(btn.Widget, "Item_Sprite");
		m_CountLabel = GuiBaseUtils.GetChildLabel(btn.Widget, "Count_Label");
		m_LockSprite = GuiBaseUtils.GetChildSprite(btn.Widget, "Lock_Sprite");
		m_EmptyLabel = GuiBaseUtils.GetChildLabel(btn.Widget, "Empty_Label");
		m_BuyButton = GuiBaseUtils.GetChildButton(btn.Widget, "BuyMore_Button");
	}

	public override void Show(ShopItemId id, bool locked)
	{
		m_RootWidget.Show(true, true);
		if (id == ShopItemId.EmptyId)
		{
			ShowEmpty(locked);
			return;
		}
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(id);
		m_NameLabel.SetNewText(itemInfo.NameTextId);
		m_ItemSprite.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		bool flag = !itemInfo.InfiniteUse;
		if (flag)
		{
			m_CountLabel.SetNewText(itemInfo.OwnedCount.ToString());
		}
		m_CountLabel.Widget.Show(flag, true);
		m_EmptyLabel.Widget.Show(false, true);
		m_LockSprite.Widget.Show(false, true);
		bool v = ShopDataBridge.Instance.BuyMoreAdvised(id);
		m_BuyButton.Widget.Show(v, true);
	}

	private void ShowEmpty(bool locked)
	{
		m_NameLabel.Widget.Show(false, true);
		m_ItemSprite.Widget.Show(false, true);
		m_CountLabel.Widget.Show(false, true);
		m_LockSprite.Widget.Show(locked, true);
		m_EmptyLabel.Widget.Show(!locked, true);
		m_BuyButton.Widget.Show(false, true);
	}

	public override void Hide()
	{
		m_RootWidget.Show(false, true);
	}
}
