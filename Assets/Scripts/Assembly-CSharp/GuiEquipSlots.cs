using System.Collections.Generic;
using UnityEngine;

internal class GuiEquipSlots
{
	private class CSlot
	{
		public ShopItemId m_ItemId;

		public ISlotView m_SlotView;

		public GUIBase_Button m_Button;

		public bool m_Locked;

		public E_Team m_Team;
	}

	public delegate void SlotSelectionDelegate(int slotIndex);

	private List<CSlot> m_Items = new List<CSlot>();

	private GuiShop.E_ItemType m_SlotItemType;

	private int m_PressedSlotIndex;

	private bool m_IsShown;

	public SlotSelectionDelegate m_OnSlotSelectionDone;

	public GuiEquipSlots(GuiShop.E_ItemType type)
	{
		m_SlotItemType = type;
	}

	public void AddSlot(ISlotView view, GUIBase_Button btn, E_Team team = E_Team.None)
	{
		CSlot cSlot = new CSlot();
		cSlot.m_ItemId = ShopItemId.EmptyId;
		cSlot.m_SlotView = view;
		cSlot.m_Button = btn;
		cSlot.m_Team = team;
		cSlot.m_Button.RegisterTouchDelegate2(OnWeaponSlot);
		m_Items.Add(cSlot);
	}

	public Vector2 GetHighlightPos(int index)
	{
		return m_Items[index].m_Button.transform.position;
	}

	private int FindSlotIndex(GUIBase_Widget w)
	{
		for (int i = 0; i < m_Items.Count; i++)
		{
			if (m_Items[i].m_Button.Widget == w)
			{
				return i;
			}
		}
		return -1;
	}

	private CSlot GetSlot(int index)
	{
		return m_Items[index];
	}

	private void OnWeaponSlot(GUIBase_Widget w)
	{
		OnWeaponSlot_ByIndex(FindSlotIndex(w));
	}

	private void OnWeaponSlot_ByIndex(int index)
	{
		m_PressedSlotIndex = index;
		if (m_PressedSlotIndex != -1 && !GetSlot(m_PressedSlotIndex).m_Locked)
		{
			GuiEquipSelection.Instance.m_OnEquipDelegate = OnEquipDone;
			ShopItemId itemId = GetSlot(m_PressedSlotIndex).m_ItemId;
			if (!GuiEquipSelection.Instance.IsShown || GuiEquipSelection.Instance.GetCurrentSelection().ItemType != m_SlotItemType)
			{
				GuiEquipSelection.Instance.Show(m_SlotItemType, itemId, GetSlot(m_PressedSlotIndex).m_Team);
			}
			else
			{
				GuiEquipSelection.Instance.SetEquipedItem(itemId);
			}
			GuiEquipMenu.Instance.HighlightSlot(m_SlotItemType, m_PressedSlotIndex);
		}
	}

	public void SelectSlotHACK(int index)
	{
		OnWeaponSlot_ByIndex(index);
	}

	private void OnEquipDone(ShopItemId selItem, bool cancel)
	{
		if (!cancel)
		{
			CSlot slot = GetSlot(m_PressedSlotIndex);
			if (selItem.IsEmpty())
			{
				ShopDataBridge.Instance.Action_UnEquip(slot.m_ItemId, m_PressedSlotIndex);
			}
			else
			{
				ShopDataBridge.Instance.Action_Equip(selItem, m_PressedSlotIndex);
			}
			GuiEquipMenu.Instance.OnUpdatePPIInfo();
			Show();
		}
	}

	public void Show()
	{
		m_IsShown = true;
		foreach (CSlot item in m_Items)
		{
			item.m_SlotView.Show(item.m_ItemId, item.m_Locked);
		}
	}

	public void Hide()
	{
		m_IsShown = false;
		foreach (CSlot item in m_Items)
		{
			item.m_SlotView.Hide();
		}
	}

	public void InitItemInSlot(ShopItemId uid, int slot)
	{
		m_Items[slot].m_ItemId = uid;
	}

	public void SetSlotLocked(int slot, bool locked)
	{
		m_Items[slot].m_Locked = locked;
		m_Items[slot].m_Button.SetDisabled(locked);
		if (m_IsShown)
		{
			m_Items[slot].m_SlotView.Show(m_Items[slot].m_ItemId, m_Items[slot].m_Locked);
		}
	}

	public bool IsSlotLocked(int slot)
	{
		return m_Items[slot].m_Locked;
	}

	public ShopItemId GetSlotItem(int slot)
	{
		if (slot < 0 || slot >= m_Items.Count)
		{
			Debug.LogError("Invalid index: " + slot + " range " + m_Items.Count);
		}
		return m_Items[slot].m_ItemId;
	}

	public E_Team GetSlotTeam(int slot)
	{
		return GetSlot(slot).m_Team;
	}

	public int GetFreeSlotIndex()
	{
		return m_Items.FindIndex((CSlot p) => !p.m_Locked && p.m_ItemId.IsEmpty());
	}
}
