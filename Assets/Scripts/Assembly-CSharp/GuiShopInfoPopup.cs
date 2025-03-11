using UnityEngine;

public class GuiShopInfoPopup
{
	private GUIBase_Layout m_WeaponLayout;

	private GUIBase_Layout m_ItemLayout;

	private GUIBase_Pivot m_WeaponPositionPivot;

	private GUIBase_Pivot m_ItemPositionPivot;

	private GUIBase_Label m_Name_Label;

	public void GuiInit()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("ShopPopups");
		m_WeaponLayout = pivot.GetLayout("InfoWeapon_Layout");
		m_ItemLayout = pivot.GetLayout("InfoItem_Layout");
		m_WeaponPositionPivot = MFGuiManager.Instance.GetPivot("WeaponPosition_Pivot");
		m_ItemPositionPivot = MFGuiManager.Instance.GetPivot("ItemPosition_Pivot");
	}

	public void Show(ShopItemId item, float desiredPos)
	{
		Vector2 pos = new Vector2(Mathf.Clamp(desiredPos, -555f, 555f), 0f);
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(item);
		if (item.ItemType == GuiShop.E_ItemType.Weapon)
		{
			ShowWeaponInfo(pos, itemInfo);
		}
		else if (item.ItemType == GuiShop.E_ItemType.Item)
		{
			ShowItemInfo(pos, itemInfo);
		}
	}

	private void ShowWeaponInfo(Vector2 pos, ShopItemInfo inf)
	{
		m_WeaponPositionPivot.transform.localPosition = pos;
		MFGuiManager.Instance.ShowLayout(m_WeaponLayout, true);
	}

	private void ShowItemInfo(Vector2 pos, ShopItemInfo inf)
	{
		m_ItemPositionPivot.transform.localPosition = pos;
		MFGuiManager.Instance.ShowLayout(m_ItemLayout, true);
	}

	public void Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_ItemLayout, false);
		MFGuiManager.Instance.ShowLayout(m_WeaponLayout, false);
	}
}
