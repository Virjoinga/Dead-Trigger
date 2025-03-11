internal class SlotViewWeapon : ISlotView
{
	private GUIBase_Widget m_RootWidget;

	private GUIBase_Label m_NameLabel;

	private GUIBase_Sprite m_WeaponSprite;

	private GuiShopUpgradeSprite m_UpgradeSprite;

	private GUIBase_Sprite m_LockSprite;

	private GUIBase_Label m_EmptyLabel;

	private GUIBase_Button m_UpgradeButton;

	public void InitGui(GUIBase_Layout layout, GUIBase_Button btn, int slotId)
	{
		m_RootWidget = btn.Widget;
		m_NameLabel = GuiBaseUtils.GetChildLabel(btn.Widget, "Gun_Label");
		m_WeaponSprite = GuiBaseUtils.GetChildSprite(btn.Widget, "Gun_Sprite");
		m_LockSprite = GuiBaseUtils.GetChildSprite(btn.Widget, "Lock_Sprite");
		m_EmptyLabel = GuiBaseUtils.GetChildLabel(btn.Widget, "Empty_Label");
		m_UpgradeSprite = new GuiShopUpgradeSprite(GuiBaseUtils.GetChildSprite(btn.Widget, "Upgrade_Sprite"));
		m_UpgradeButton = GuiBaseUtils.GetChildButton(btn.Widget, "Upgrade_Button");
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
		m_WeaponSprite.Widget.CopyMaterialSettings(itemInfo.SpriteWidget);
		bool on = itemInfo.Owned && itemInfo.Upgrade > 0;
		m_UpgradeSprite.Show(on, itemInfo.Upgrade);
		m_EmptyLabel.Widget.Show(false, true);
		m_LockSprite.Widget.Show(false, true);
		bool v = ShopDataBridge.Instance.HasWeaponUpgradeAvailable(id);
		m_UpgradeButton.Widget.Show(v, true);
	}

	private void ShowEmpty(bool locked)
	{
		m_NameLabel.Widget.Show(false, true);
		m_WeaponSprite.Widget.Show(false, true);
		m_UpgradeSprite.Show(false, 0);
		m_LockSprite.Widget.Show(locked, true);
		m_EmptyLabel.Widget.Show(!locked, true);
		m_UpgradeButton.Widget.Show(false, true);
	}

	public override void Hide()
	{
		m_RootWidget.Show(false, true);
	}
}
