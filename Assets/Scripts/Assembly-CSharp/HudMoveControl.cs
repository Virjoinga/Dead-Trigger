using UnityEngine;

public class HudMoveControl : HudComponent
{
	private GUIBase_Sprite m_DPad;

	private GUIBase_Sprite m_DPadjoy;

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "HUD_Layout";

	private string s_DPadName = "Dpad";

	private string s_DPadJoyName = "Dpadjoy";

	public override bool VisibleOnStart()
	{
		return true;
	}

	public override void Init()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(s_PivotMainName);
		if (!pivot)
		{
			Debug.LogError("'" + s_PivotMainName + "' not found!!! Assert should come now");
			return;
		}
		GUIBase_Layout layout = pivot.GetLayout(s_LayoutMainName);
		if (!layout)
		{
			Debug.LogError("'" + s_LayoutMainName + "' not found!!! Assert should come now");
			return;
		}
		m_DPad = layout.GetWidget(s_DPadName).GetComponent<GUIBase_Sprite>();
		m_DPadjoy = layout.GetWidget(s_DPadJoyName).GetComponent<GUIBase_Sprite>();
	}

	public override void Reset()
	{
	}

	public override void LateUpdate(float deltaTime)
	{
		base.LateUpdate(deltaTime);
	}

	protected override void ShowWidgets(bool on)
	{
		switch (GuiOptions.m_ControlScheme)
		{
		case GuiOptions.E_ControlScheme.Scheme1:
			if ((bool)m_DPad)
			{
				m_DPad.Widget.Show(false, true);
			}
			if ((bool)m_DPadjoy)
			{
				m_DPadjoy.Widget.Show(false, true);
			}
			break;
		case GuiOptions.E_ControlScheme.Scheme2:
			if ((bool)m_DPad)
			{
				m_DPad.Widget.Show(on, true);
			}
			if ((bool)m_DPadjoy)
			{
				m_DPadjoy.Widget.Show(false, true);
			}
			break;
		}
	}

	public override void StoreControlsOrigPositions()
	{
		base.StoreControlsOrigPositions();
		GuiOptions.MoveStick.OrigPos = new Vector2(m_DPad.transform.position.x, m_DPad.transform.position.y);
	}

	public override void UpdateControlsPosition()
	{
		base.UpdateControlsPosition();
		m_DPad.transform.position = GuiOptions.MoveStick.Positon;
	}

	public void JoystickBaseShow(Vector2 center)
	{
		if ((bool)m_DPad)
		{
			m_DPad.Widget.Show(true, true);
			Vector3 position = ScreenToWidget(center);
			m_DPad.transform.position = position;
		}
	}

	public void JoystickBaseHide()
	{
		if ((bool)m_DPad)
		{
			m_DPad.Widget.Show(false, true);
		}
	}

	public void JoystickDown(Vector2 center)
	{
		if ((bool)m_DPadjoy)
		{
			m_DPadjoy.Widget.Show(true, true);
			Vector3 position = ScreenToWidget(center);
			m_DPadjoy.transform.position = position;
		}
	}

	public void JoystickUpdate(Vector2 center)
	{
		if ((bool)m_DPadjoy)
		{
			Vector3 position = ScreenToWidget(center);
			m_DPadjoy.transform.position = position;
		}
	}

	public void JoystickUp()
	{
		if ((bool)m_DPadjoy)
		{
			m_DPadjoy.Widget.Show(false, true);
		}
	}

	private static Vector3 ScreenToWidget(Vector2 pos)
	{
		pos.y = (float)Screen.height - pos.y;
		return new Vector3(pos.x, pos.y, 0f);
	}
}
