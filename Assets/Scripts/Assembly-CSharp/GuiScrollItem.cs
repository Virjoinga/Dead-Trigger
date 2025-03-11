public class GuiScrollItem : IScrollItem
{
	private GUIBase_Widget m_Widget;

	private ShopItemId m_Id;

	private ShopItemInfo m_Inf;

	private GUIBase_Sprite m_Equiped_Sprite;

	private GUIBase_Sprite m_Owned_Sprite;

	private GUIBase_Sprite m_New_Sprite;

	private GUIBase_Sprite m_Sale_Sprite;

	private GUIBase_Label m_Sale_Label;

	private GUIBase_Sprite m_Unlocked_Sprite;

	private GUIBase_Sprite m_NewUpgrade_Sprite;

	private GuiShopUpgradeSprite m_UpgradeSprite;

	private GUIBase_Sprite m_Gold_Sprite;

	private GUIBase_Sprite m_Name_Sprite;

	private GUIBase_Label m_Name_Label;

	private GUIBase_Sprite m_Thumbnail;

	private GUIBase_Label m_DiscountLabel;

	private GUIBase_Sprite m_LockedOn;

	private GUIBase_Label m_RequiredRank_Label;

	private GUIBase_Label m_CountLabel;

	private bool m_EquipMenu;

	public GuiScrollItem(ShopItemId id, GUIBase_Widget w, bool equipMenu)
	{
		m_Widget = w;
		m_Id = id;
		m_EquipMenu = equipMenu;
		UpdateItemInfo();
		InitGui();
	}

	public override void UpdateItemInfo()
	{
		m_Inf = ShopDataBridge.Instance.GetItemInfo(m_Id);
	}

	public override void Show()
	{
		if (m_Inf.EmptySlot)
		{
			m_Widget.Show(false, true);
			return;
		}
		m_Widget.Show(true, false);
		m_Thumbnail.Widget.Show(true, false);
		m_Name_Sprite.Widget.Show(true, true);
		m_Name_Label.SetNewText(m_Inf.NameTextId);
		if (m_Inf.Locked && !m_EquipMenu)
		{
			m_LockedOn.Widget.Show(true, true);
			m_RequiredRank_Label.SetNewText(m_Inf.RequiredLevel.ToString());
		}
		else
		{
			m_LockedOn.Widget.Show(false, true);
		}
		bool on = m_Inf.Owned && m_Inf.Upgrade > 0;
		m_UpgradeSprite.Show(on, m_Inf.Upgrade);
		m_Gold_Sprite.Widget.Show(m_Inf.GoldCurrency && !m_EquipMenu, true);
		bool flag = ShopDataBridge.Instance.IsEquiped(m_Id);
		bool flag2 = m_Id.ItemType == GuiShop.E_ItemType.Item && m_Inf.OwnedCount <= 0;
		bool v = false;
		bool flag3 = false;
		bool v2 = false;
		bool v3 = false;
		bool v4 = false;
		bool v5 = false;
		if (flag && !m_EquipMenu && !m_Inf.Locked)
		{
			v3 = true;
		}
		else if (m_Inf.Owned && !flag2 && !m_EquipMenu && !m_Inf.Locked)
		{
			v2 = true;
		}
		bool flag4 = m_Id.ItemType == GuiShop.E_ItemType.Item && !m_Inf.InfiniteUse;
		bool flag5 = m_Id.ItemType == GuiShop.E_ItemType.Weapon && m_Inf.UpgradeReady && !m_EquipMenu;
		if (!m_EquipMenu)
		{
			if (m_Inf.Locked)
			{
				if (m_Inf.NewInShop)
				{
					v = true;
				}
			}
			else if (m_Inf.Owned && flag5)
			{
				v5 = true;
			}
			else if (m_Inf.PriceSale && (!m_Inf.Owned || flag4))
			{
				flag3 = true;
			}
			else if (!m_Inf.Owned && m_Inf.JustUnlocked)
			{
				v4 = true;
			}
		}
		m_Equiped_Sprite.Widget.Show(v3, true);
		m_Owned_Sprite.Widget.Show(v2, true);
		m_Sale_Sprite.Widget.Show(flag3, true);
		if (flag3)
		{
			m_Sale_Label.SetNewText(m_Inf.DiscountTagSmall);
		}
		m_New_Sprite.Widget.Show(v, true);
		m_Unlocked_Sprite.Widget.Show(v4, true);
		m_NewUpgrade_Sprite.Widget.Show(v5, true);
		bool flag6 = m_Id.ItemType == GuiShop.E_ItemType.Item && !m_Inf.InfiniteUse;
		if (flag6)
		{
			m_CountLabel.SetNewText((!m_EquipMenu) ? ("+" + m_Inf.ShopCount) : m_Inf.OwnedCount.ToString());
		}
		m_CountLabel.Widget.Show(flag6, true);
	}

	public override void Hide()
	{
		m_Widget.Show(false, true);
	}

	private void InitGui()
	{
		m_Thumbnail = GuiBaseUtils.GetChildSprite(m_Widget, "SmallThumbnail");
		if (m_Inf.SpriteWidget != null)
		{
			m_Thumbnail.Widget.CopyMaterialSettings(m_Inf.SpriteWidget);
		}
		m_Equiped_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Equiped_Sprite");
		m_Owned_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Owned_Sprite");
		m_New_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "New_Sprite");
		m_Sale_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Sale_Sprite");
		m_Sale_Label = GuiBaseUtils.GetChildLabel(m_Sale_Sprite.Widget, "Label");
		m_Unlocked_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Unlocked_Sprite");
		m_NewUpgrade_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "NewUpgrade_Sprite");
		m_UpgradeSprite = new GuiShopUpgradeSprite(GuiBaseUtils.GetChildSprite(m_Widget, "Upgrade_Sprite"));
		m_Gold_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Gold_Sprite");
		m_Name_Sprite = GuiBaseUtils.GetChildSprite(m_Widget, "Name_Sprite");
		m_Name_Label = GuiBaseUtils.GetChildLabel(m_Widget, "Name_Label");
		m_LockedOn = GuiBaseUtils.GetChildSprite(m_Widget, "LockedOn");
		m_RequiredRank_Label = GuiBaseUtils.GetChildLabel(m_Widget, "RequiredRank_Label");
		m_CountLabel = GuiBaseUtils.GetChildLabel(m_Widget, "Count_Label");
	}
}
