using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudGadgets : HudComponent
{
	private class GadgetInfo
	{
		public GUIBase_Button m_GadgetButton;

		public GUIBase_Widget m_GadgetImage;

		public GUIBase_Label m_GadgetCount;

		public Vector2 m_OrigPos;

		public int m_Instances;
	}

	private List<E_ItemID> Gadgets = new List<E_ItemID>();

	private GadgetInfo[] m_GadgetInfos = new GadgetInfo[5];

	private bool m_ActivePowerupExpiring;

	private GUIBase_Widget m_SelectionImage;

	private int m_SelectedIndex;

	public override bool VisibleOnStart()
	{
		return true;
	}

	public override void Init()
	{
		GUIBase_Button.TouchDelegate[] array = new GUIBase_Button.TouchDelegate[5] { TouchDelegate1, TouchDelegate2, TouchDelegate3, TouchDelegate4, TouchDelegate5 };
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("MainHUD");
		if (!pivot)
		{
			Debug.LogError("'MainHUD' not found!!! Assert should come now");
			return;
		}
		GUIBase_Layout layout = pivot.GetLayout("HUD_Layout");
		if (!layout)
		{
			Debug.LogError("'HUD_Layout' not found!!! Assert should come now");
			return;
		}
		for (int i = 0; i < m_GadgetInfos.Length; i++)
		{
			m_GadgetInfos[i] = new GadgetInfo();
			m_GadgetInfos[i].m_GadgetButton = GuiBaseUtils.RegisterButtonDelegate(layout, "Gadget" + (i + 1), array[i], null);
			m_GadgetInfos[i].m_GadgetImage = GetChildByName<GUIBase_Widget>(m_GadgetInfos[i].m_GadgetButton, "Gadget_Image");
			m_GadgetInfos[i].m_GadgetCount = GetChildByName<GUIBase_Label>(m_GadgetInfos[i].m_GadgetButton, "Gadget_Text");
			m_GadgetInfos[i].m_OrigPos = m_GadgetInfos[i].m_GadgetButton.transform.position;
			m_GadgetInfos[i].m_Instances = -1;
		}
		m_SelectionImage = GuiBaseUtils.PrepareSprite(layout, "SelectedGadged_Image").Widget;
	}

	public override void Reset()
	{
	}

	public override void LateUpdate(float deltaTime)
	{
		bool flag = false;
		for (int i = 0; i < Gadgets.Count; i++)
		{
			Item gadget = Player.Instance.Owner.GadgetsComponent.GetGadget(Gadgets[i]);
			if (gadget == null)
			{
				continue;
			}
			if (gadget.Settings.ID == Player.Instance.GetActivePowerup() && Player.Instance.IsActivePowerupExpiring())
			{
				flag = true;
				if (!m_ActivePowerupExpiring)
				{
					m_ActivePowerupExpiring = true;
					StartHighlight(i);
				}
			}
			if (m_GadgetInfos[i].m_Instances != gadget.Count)
			{
				m_GadgetInfos[i].m_Instances = gadget.Count;
				m_GadgetInfos[i].m_GadgetCount.SetNewText(gadget.Count.ToString());
			}
			if (gadget.IsAvailableForUse() && gadget.Settings.ItemUse == E_ItemUse.Activate)
			{
				m_GadgetInfos[i].m_GadgetButton.SetDisabled(false);
				m_GadgetInfos[i].m_GadgetImage.m_FadeAlpha = 1f;
				m_GadgetInfos[i].m_GadgetCount.Widget.m_FadeAlpha = 1f;
			}
			else if (gadget.Settings.ItemUse == E_ItemUse.Passive)
			{
				m_GadgetInfos[i].m_GadgetButton.SetDisabled(true);
				if (gadget.IsAvailableForUse())
				{
					m_GadgetInfos[i].m_GadgetImage.m_FadeAlpha = 1f;
					m_GadgetInfos[i].m_GadgetCount.Widget.m_FadeAlpha = 1f;
					if (gadget.Settings.ItemBehaviour == E_ItemBehaviour.ReviveAndKillAll)
					{
						m_GadgetInfos[i].m_GadgetCount.Widget.Show(true, false);
					}
					else
					{
						m_GadgetInfos[i].m_GadgetCount.Widget.Show(false, false);
					}
				}
				else
				{
					m_GadgetInfos[i].m_GadgetImage.m_FadeAlpha = 0.1f;
					m_GadgetInfos[i].m_GadgetCount.Widget.m_FadeAlpha = 0.1f;
				}
			}
			else
			{
				m_GadgetInfos[i].m_GadgetButton.SetDisabled(true);
				m_GadgetInfos[i].m_GadgetImage.m_FadeAlpha = 0.1f;
				m_GadgetInfos[i].m_GadgetCount.Widget.m_FadeAlpha = 0.1f;
			}
		}
		if (!flag && m_ActivePowerupExpiring)
		{
			m_ActivePowerupExpiring = false;
			StopHighlight();
		}
	}

	public void SetGadgets(List<E_ItemID> gadgets)
	{
		Gadgets = gadgets;
		for (int i = 0; i < gadgets.Count; i++)
		{
			SetGadgetHudInfo(gadgets[i], m_GadgetInfos[i]);
		}
		ShowGadgetSprites(IsVisible());
	}

	private void SetGadgetHudInfo(E_ItemID id, GadgetInfo widget)
	{
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get(id);
		widget.m_GadgetImage.CopyMaterialSettings(itemSettings.HudWidget);
		widget.m_GadgetImage.SetModify();
	}

	public E_ItemID GetGadgetID(int index)
	{
		if (index < 0 || index >= Gadgets.Count)
		{
			return E_ItemID.None;
		}
		return Gadgets[index];
	}

	public void SetSelected(int index)
	{
	}

	public int GetSelected()
	{
		return m_SelectedIndex;
	}

	public void SelectNext()
	{
		int num = m_SelectedIndex;
		Item gadget;
		do
		{
			num = ++num % Gadgets.Count;
			gadget = Player.Instance.Owner.GadgetsComponent.GetGadget(Gadgets[num]);
		}
		while ((gadget == null || gadget.Settings.ItemUse != E_ItemUse.Activate) && num != m_SelectedIndex);
		m_SelectedIndex = num;
		ShowGadgetSprites(IsVisible());
	}

	public void SelectPrev()
	{
		int num = m_SelectedIndex;
		Item gadget;
		do
		{
			num = ((num > 0) ? (num - 1) : (Gadgets.Count - 1));
			gadget = Player.Instance.Owner.GadgetsComponent.GetGadget(Gadgets[num]);
		}
		while ((gadget == null || gadget.Settings.ItemUse != E_ItemUse.Activate) && num != m_SelectedIndex);
		m_SelectedIndex = num;
		ShowGadgetSprites(IsVisible());
	}

	public void StartHighlight(int index)
	{
		m_GadgetInfos[index].m_GadgetImage.StartCoroutine(HighlightObject(m_GadgetInfos[index].m_GadgetImage));
		m_GadgetInfos[index].m_GadgetButton.StartCoroutine(HighlightObject(m_GadgetInfos[index].m_GadgetButton.Widget));
	}

	public void StopHighlight()
	{
		for (int i = 0; i < Gadgets.Count; i++)
		{
			m_GadgetInfos[i].m_GadgetImage.StopAllCoroutines();
			m_GadgetInfos[i].m_GadgetImage.m_FadeAlpha = 1f;
			m_GadgetInfos[i].m_GadgetButton.StopAllCoroutines();
			m_GadgetInfos[i].m_GadgetButton.Widget.m_FadeAlpha = 1f;
		}
	}

	private IEnumerator HighlightObject(GUIBase_Widget sprite)
	{
		while (true)
		{
			sprite.m_FadeAlpha = 0.25f;
			yield return new WaitForSeconds(0.25f);
			sprite.m_FadeAlpha = 1f;
			yield return new WaitForSeconds(0.25f);
		}
	}

	protected override void ShowWidgets(bool on)
	{
		ShowGadgetSprites(false);
		if (on)
		{
			ShowGadgetSprites(true);
		}
	}

	public override bool Enable(EnableLayer layer, bool enable)
	{
		return base.Enable(layer, enable);
	}

	public bool FingerIdInUse(int fingerId)
	{
		return false;
	}

	public override void StoreControlsOrigPositions()
	{
		base.StoreControlsOrigPositions();
		for (int i = 0; i < m_GadgetInfos.Length; i++)
		{
			GuiOptions.GadgetButtons[i].OrigPos = new Vector2(m_GadgetInfos[i].m_GadgetButton.transform.position.x, m_GadgetInfos[i].m_GadgetButton.transform.position.y);
		}
	}

	public override void UpdateControlsPosition()
	{
		base.UpdateControlsPosition();
		for (int i = 0; i < m_GadgetInfos.Length; i++)
		{
			m_GadgetInfos[i].m_GadgetButton.transform.position = GuiOptions.GadgetButtons[i].Positon;
		}
	}

	private T GetChildByName<T>(GUIBase_Button btn, string name) where T : Component
	{
		Transform transform = btn.transform.Find(name);
		return (!(transform != null)) ? ((T)null) : transform.GetComponent<T>();
	}

	private void ShowGadgetSprites(bool show)
	{
		if (Gadgets.Count != 0)
		{
			for (int i = 0; i < m_GadgetInfos.Length; i++)
			{
				bool flag = Gadgets.Count > i && Gadgets[i] != E_ItemID.None;
				m_GadgetInfos[i].m_GadgetButton.Widget.Show(show && flag, true);
			}
			if (m_SelectedIndex != -1)
			{
				m_SelectionImage.SetSpritePos(m_GadgetInfos[m_SelectedIndex].m_GadgetButton.transform.position);
				m_SelectionImage.Show(show, true);
			}
			else
			{
				m_SelectionImage.Show(false, true);
			}
		}
	}

	private void UseGadget(int gadgetIdx)
	{
		ShowGadgetSprites(true);
		Player.Instance.Controls.UseGadgetDelegate(Gadgets[gadgetIdx]);
	}

	private void TouchDelegate1()
	{
		UseGadget(0);
	}

	private void TouchDelegate2()
	{
		UseGadget(1);
	}

	private void TouchDelegate3()
	{
		UseGadget(2);
	}

	private void TouchDelegate4()
	{
		UseGadget(3);
	}

	private void TouchDelegate5()
	{
		UseGadget(4);
	}
}
