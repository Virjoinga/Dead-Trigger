using System;
using System.Collections.Generic;
using UnityEngine;

internal class GuiScroller<Key> where Key : IComparable<Key>
{
	private class Item<TKey> : IComparable<Item<TKey>> where TKey : IComparable<TKey>
	{
		public TKey m_UID;

		public GUIBase_Widget m_Widget;

		public IScrollItem m_ItemGui;

		public Item(TKey id, GUIBase_Widget w, IScrollItem itemGui)
		{
			m_UID = id;
			m_Widget = w;
			m_ItemGui = itemGui;
		}

		public int CompareTo(Item<TKey> other)
		{
			return m_UID.CompareTo(other.m_UID);
		}
	}

	private class Transition
	{
		private float m_From;

		private float m_To;

		private float m_BeginTime;

		private float m_Duration;

		public Transition(float From, float To, float Duration)
		{
			m_From = From;
			m_To = To;
			m_BeginTime = Time.time;
			m_Duration = Duration;
		}

		public float GetTransition()
		{
			float num = Time.time - m_BeginTime;
			return Mathfx.Sinerp(m_From, m_To, Mathf.Clamp(num / m_Duration, 0f, 1f));
		}

		public bool IsDone()
		{
			return Time.time > m_BeginTime + m_Duration;
		}
	}

	private enum E_ScrollMode
	{
		Drag = 0,
		Momentum = 1,
		Anim = 2,
		Idle = 3
	}

	public delegate void ChangeDelegate();

	public delegate void HoldDelegate(int itemIndex, Key itemId);

	public delegate void HoldEndDelegate();

	public ChangeDelegate m_OnSelectionChange;

	public HoldDelegate m_OnHoldBegin;

	public HoldEndDelegate m_OnHoldEnd;

	public string m_DebugName;

	private int m_LastItem = -1;

	private List<Item<Key>> m_Items = new List<Item<Key>>();

	private GUIBase_Pivot m_ScrollPivot;

	private GUIBase_Layout m_BackgroundLayout;

	public int m_CurrentItemIndex = -1;

	private float m_ScrollLimitMin;

	private float m_ScrollLimitMax;

	private float m_ScrollLimitHardMin;

	private float m_ScrollLimitHardMax;

	private GuiDragInput m_DragInput = new GuiDragInput();

	private Transition m_Transition;

	private E_ScrollMode m_ScrollMode = E_ScrollMode.Idle;

	private bool m_WasHolding;

	public float ItemOffset { get; private set; }

	public void Show()
	{
		MFGuiManager.Instance.ShowPivot(m_ScrollPivot, true);
		MFGuiManager.Instance.ShowLayout(m_BackgroundLayout, true);
		UpdateItemsViews();
	}

	public void Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_ScrollPivot, false);
		MFGuiManager.Instance.ShowLayout(m_BackgroundLayout, false);
	}

	public void UpdateItemsViews()
	{
		foreach (Item<Key> item in m_Items)
		{
			item.m_ItemGui.UpdateItemInfo();
			item.m_ItemGui.Show();
		}
	}

	public void InitGui(GUIBase_Layout bgLayout, GUIBase_Pivot scrollPivot)
	{
		m_ScrollPivot = scrollPivot;
		m_BackgroundLayout = bgLayout;
		GUIBase_Sprite gUIBase_Sprite = GuiBaseUtils.PrepareSprite(m_BackgroundLayout, "ActiveArea_Sprite");
		Rect rectInScreenCoords = gUIBase_Sprite.Widget.GetRectInScreenCoords();
		rectInScreenCoords.center = new Vector2(rectInScreenCoords.center.x, (float)Screen.height - rectInScreenCoords.center.y);
		m_DragInput.SetActiveArea(rectInScreenCoords);
		ItemOffset = 312f;
		m_DragInput.isHorizontal = true;
	}

	public void Clear()
	{
		m_Items.Clear();
		m_CurrentItemIndex = -1;
	}

	public void HideItems()
	{
		foreach (Item<Key> item in m_Items)
		{
			item.m_ItemGui.Hide();
		}
	}

	public void AddItem(Key uid, GUIBase_Widget w, IScrollItem itemGui)
	{
		Item<Key> item = new Item<Key>(uid, w, itemGui);
		int count = m_Items.Count;
		w.transform.localPosition = new Vector2(ItemOffset * (float)count, w.transform.localPosition.y);
		m_Items.Add(item);
		ComputeScrollLimits();
	}

	private void ComputeScrollLimits()
	{
		float num = ItemOffset * 0.25f;
		m_ScrollLimitMax = 0f + num;
		m_ScrollLimitMin = 0f - (ItemOffset * (float)(m_Items.Count - 1) + num);
		float num2 = (m_ScrollLimitHardMax = ItemOffset * 0.75f);
		m_ScrollLimitHardMin = 0f - (ItemOffset * (float)(m_Items.Count - 1) + num2);
	}

	public bool HasSelection()
	{
		return m_CurrentItemIndex >= 0 && m_CurrentItemIndex < m_Items.Count;
	}

	public Key GetSelectedItem()
	{
		return m_Items[m_CurrentItemIndex].m_UID;
	}

	public void SetSelectedItem(Key uid)
	{
		int num = 0;
		num = FindItemIndex(uid);
		if (num == -1)
		{
			num = 0;
		}
		ScrollToItem(num);
	}

	private int GetNearestItem(float lastScrollDir)
	{
		float num = ItemOffset * 0.5f;
		if (lastScrollDir > 0f)
		{
			num = ItemOffset * 0.1f;
		}
		else if (lastScrollDir < 0f)
		{
			num = ItemOffset * 0.9f;
		}
		float num2 = 0f - m_ScrollPivot.transform.localPosition.x;
		return (int)((num2 + num) / ItemOffset);
	}

	private int GetCurrentItem()
	{
		float num = ItemOffset * 0.5f;
		float num2 = 0f - m_ScrollPivot.transform.localPosition.x;
		int value = (int)((num2 + num) / ItemOffset);
		return Mathf.Clamp(value, 0, m_Items.Count - 1);
	}

	private int GetItemOnPos(float pos)
	{
		float num = 0f - m_ScrollPivot.transform.localPosition.x;
		float x = pos - (float)(Screen.width / 2);
		return (int)((num + m_BackgroundLayout.ScreenPosToLayoutSpace(new Vector2(x, 0f)).x + ItemOffset / 2f) / ItemOffset);
	}

	private void ScrollToItem(int index)
	{
		m_ScrollPivot.transform.localPosition = new Vector2((float)(-index) * ItemOffset, 0f);
		m_CurrentItemIndex = index;
		if (m_OnSelectionChange != null)
		{
			m_OnSelectionChange();
		}
	}

	private int FindItemIndex(Key uid)
	{
		for (int i = 0; i < m_Items.Count; i++)
		{
			if (m_Items[i].m_UID.Equals(uid))
			{
				return i;
			}
		}
		return -1;
	}

	private void MoveScrollerPivot(Vector2 delta)
	{
		Vector2 vector = m_BackgroundLayout.ScreenDeltaToLayoutSpace(delta);
		Vector3 localPosition = m_ScrollPivot.transform.localPosition;
		localPosition.x += vector.x;
		localPosition.y += vector.y;
		localPosition.x = Mathf.Clamp(localPosition.x, m_ScrollLimitHardMin, m_ScrollLimitHardMax);
		m_ScrollPivot.transform.localPosition = localPosition;
	}

	private bool AdjustScrollToLimits()
	{
		Vector3 localPosition = m_ScrollPivot.transform.localPosition;
		if (localPosition.x > m_ScrollLimitMax)
		{
			return true;
		}
		if (localPosition.x < m_ScrollLimitMin)
		{
			return true;
		}
		return false;
	}

	public void Update()
	{
		if (m_ScrollPivot == null)
		{
			return;
		}
		m_DragInput.Update();
		DetectTap();
		DetectHold();
		if (m_DragInput.IsDragging)
		{
			m_ScrollMode = E_ScrollMode.Drag;
			m_Transition = null;
			UpdateStateDrag();
		}
		else
		{
			switch (m_ScrollMode)
			{
			case E_ScrollMode.Idle:
				UpdateStateIdle();
				break;
			case E_ScrollMode.Drag:
				UpdateStateDrag();
				break;
			case E_ScrollMode.Momentum:
				UpdateStateMomentum();
				break;
			case E_ScrollMode.Anim:
				UpdateStateAnim();
				break;
			}
		}
		int currentItem = GetCurrentItem();
		if (m_LastItem != currentItem)
		{
			m_LastItem = currentItem;
			m_CurrentItemIndex = currentItem;
			if (m_OnSelectionChange != null)
			{
				m_OnSelectionChange();
			}
		}
	}

	private void DetectTap()
	{
		if (m_DragInput.tapEvent)
		{
			int itemOnPos = GetItemOnPos(m_DragInput.tapEventPos);
			m_DragInput.ClearTapEvent();
			m_ScrollMode = E_ScrollMode.Anim;
			itemOnPos = Mathf.Clamp(itemOnPos, 0, m_Items.Count - 1);
			int num = Mathf.Abs(m_CurrentItemIndex - itemOnPos);
			float duration = 0.5f + (float)(num - 1) * 0.15f;
			m_Transition = new Transition(m_ScrollPivot.transform.localPosition.x, 0f - (float)itemOnPos * ItemOffset, duration);
			m_CurrentItemIndex = itemOnPos;
		}
	}

	private void DetectHold()
	{
		if (!m_WasHolding && m_DragInput.isHolding)
		{
			int itemOnPos = GetItemOnPos(m_DragInput.holdingPos);
			if (itemOnPos >= 0 && itemOnPos < m_Items.Count)
			{
				Key uID = m_Items[itemOnPos].m_UID;
				if (m_OnHoldBegin != null)
				{
					m_OnHoldBegin(itemOnPos, uID);
				}
				m_WasHolding = true;
			}
		}
		else if (m_WasHolding && !m_DragInput.isHolding)
		{
			if (m_OnHoldEnd != null)
			{
				m_OnHoldEnd();
			}
			m_WasHolding = false;
		}
	}

	private void UpdateStateIdle()
	{
		if (m_DragInput.IsDragging)
		{
			m_ScrollMode = E_ScrollMode.Drag;
			MoveScrollerPivot(m_DragInput.ScrollDelta);
		}
	}

	private void UpdateStateDrag()
	{
		if (m_DragInput.IsDragging)
		{
			MoveScrollerPivot(m_DragInput.ScrollDelta);
			if (AdjustScrollToLimits())
			{
				m_ScrollMode = E_ScrollMode.Anim;
				int nearestItem = GetNearestItem(m_DragInput.ScrollDelta.x);
				m_CurrentItemIndex = Mathf.Clamp(nearestItem, 0, m_Items.Count - 1);
				m_Transition = new Transition(m_ScrollPivot.transform.localPosition.x, 0f - (float)m_CurrentItemIndex * ItemOffset, 0.35f);
			}
		}
		else
		{
			m_ScrollMode = E_ScrollMode.Momentum;
		}
	}

	private void UpdateStateMomentum()
	{
		MoveScrollerPivot(m_DragInput.ScrollDelta);
		if (!m_DragInput.HasMomentum() || AdjustScrollToLimits())
		{
			m_ScrollMode = E_ScrollMode.Anim;
			int nearestItem = GetNearestItem(m_DragInput.ScrollDelta.x);
			m_CurrentItemIndex = Mathf.Clamp(nearestItem, 0, m_Items.Count - 1);
			float num = Mathf.Abs(m_ScrollPivot.transform.localPosition.x + (float)m_CurrentItemIndex * ItemOffset);
			float num2 = Mathf.Max(m_DragInput.MoveSpeed, m_DragInput.MinSpeed);
			float duration = num / num2;
			m_Transition = new Transition(m_ScrollPivot.transform.localPosition.x, 0f - (float)m_CurrentItemIndex * ItemOffset, duration);
		}
	}

	private void UpdateStateAnim()
	{
		Vector2 vector = m_ScrollPivot.transform.localPosition;
		vector.x = m_Transition.GetTransition();
		m_ScrollPivot.transform.localPosition = vector;
		if (m_Transition.IsDone())
		{
			m_Transition = null;
			m_ScrollMode = E_ScrollMode.Idle;
		}
	}
}
