using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GUI/Menu/GuiCustomizeControls")]
public class GuiCustomizeControls : MonoBehaviour
{
	private class CustomControl
	{
		public string m_Name;

		public GUIBase_Widget m_WidgetDummy;

		public Transform m_Transform;

		public Vector2 m_TempOffset;

		public GuiOptions.ControlPos m_OptionsPos;
	}

	public delegate void CustomizeHideDelegate();

	public static GuiCustomizeControls Instance;

	private GUIBase_Pivot m_CutomizePivot;

	private GUIBase_Layout m_CutomizeLayout;

	private static string s_CutomizePivotName = "Customize";

	private static string s_CutomizeLayoutName = "Customize_Layout";

	private static string s_ResetButtonName = "ResetButton";

	private static string s_ConfirmButtonName = "BackButton";

	private List<CustomControl> m_Controls = new List<CustomControl>();

	private Vector2 DragBeginPos;

	private Vector2 DragBeginOffset;

	private CustomControl DraggingControl;

	private bool isOn;

	public CustomizeHideDelegate m_OnHideDelegate;

	private void Awake()
	{
		Instance = this;
	}

	private void InitControlsReferences()
	{
		m_Controls.Add(new CustomControl
		{
			m_Name = "FireDummy",
			m_OptionsPos = GuiOptions.FireUseButton
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "ReloadDummy",
			m_OptionsPos = GuiOptions.ReloadButton
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "AimDummy",
			m_OptionsPos = GuiOptions.AimButton
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "DpadMoveDummy",
			m_OptionsPos = GuiOptions.MoveStick
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "GadgetDummy1",
			m_OptionsPos = GuiOptions.GadgetButtons[0]
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "GadgetDummy2",
			m_OptionsPos = GuiOptions.GadgetButtons[1]
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "GadgetDummy3",
			m_OptionsPos = GuiOptions.GadgetButtons[2]
		});
		m_Controls.Add(new CustomControl
		{
			m_Name = "GadgetDummy4",
			m_OptionsPos = GuiOptions.GadgetButtons[3]
		});
	}

	public void LateInitialize()
	{
		m_CutomizePivot = MFGuiManager.Instance.GetPivot(s_CutomizePivotName);
		if (!m_CutomizePivot)
		{
			Debug.LogError("Pivot '" + s_CutomizePivotName + "'  not found");
		}
		m_CutomizeLayout = m_CutomizePivot.GetLayout(s_CutomizeLayoutName);
		if (!m_CutomizeLayout)
		{
			Debug.LogError("Layout '" + s_CutomizeLayoutName + "'  not found");
		}
		GuiBaseUtils.RegisterButtonDelegate(m_CutomizeLayout, s_ResetButtonName, null, ResetButtonDelegate);
		GuiBaseUtils.RegisterButtonDelegate(m_CutomizeLayout, s_ConfirmButtonName, null, ConfirmButtonDelegate);
		InitControlsReferences();
		foreach (CustomControl control in m_Controls)
		{
			GUIBase_Sprite gUIBase_Sprite = GuiBaseUtils.PrepareSprite(m_CutomizeLayout, control.m_Name);
			control.m_WidgetDummy = gUIBase_Sprite.Widget;
			control.m_Transform = control.m_WidgetDummy.gameObject.transform;
		}
	}

	public void Show()
	{
		isOn = true;
		InitTempPositions();
		MFGuiManager.Instance.ShowPivot(m_CutomizePivot, true);
		ShowSchemeSpecificSprites();
	}

	private void Hide()
	{
		isOn = false;
		MFGuiManager.Instance.ShowPivot(m_CutomizePivot, false);
		HideSchemeSpecificSprites();
	}

	private void InitTempPositions()
	{
		foreach (CustomControl control in m_Controls)
		{
			control.m_TempOffset = control.m_OptionsPos.Offset;
		}
		UpdateSpritesPos();
	}

	private void CancelButtonDelegate(bool inside)
	{
		if (inside)
		{
			Hide();
			if (m_OnHideDelegate != null)
			{
				m_OnHideDelegate();
			}
		}
	}

	private void ResetButtonDelegate(bool inside)
	{
		if (!inside)
		{
			return;
		}
		foreach (CustomControl control in m_Controls)
		{
			control.m_TempOffset = Vector2.zero;
			if (GuiOptions.leftHandAiming && control.m_OptionsPos.Side != 0)
			{
				control.m_TempOffset.x = (float)Screen.width - control.m_OptionsPos.OrigPos.x - control.m_OptionsPos.OrigPos.x;
			}
		}
		UpdateSpritesPos();
		RedrawSprites();
	}

	private void ConfirmButtonDelegate(bool inside)
	{
		if (!inside)
		{
			return;
		}
		foreach (CustomControl control in m_Controls)
		{
			control.m_OptionsPos.Offset = control.m_TempOffset;
		}
		GuiHUD.Instance.UpdateControlsPosition();
		Player.Instance.Controls.TouchControls.OnControlSchemeChange();
		GuiOptions.Save();
		Hide();
		if (m_OnHideDelegate != null)
		{
			m_OnHideDelegate();
		}
	}

	private void MouseTouchEvent()
	{
		if (Input.GetMouseButtonDown(0))
		{
			TouchBegin(Input.mousePosition);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			TouchEnd(Input.mousePosition);
		}
		else if (Input.GetMouseButton(0))
		{
			TouchUpdate(Input.mousePosition);
		}
	}

	private void TouchBegin(Vector2 pos)
	{
		pos.y = (float)Screen.height - pos.y;
		foreach (CustomControl control in m_Controls)
		{
			if (control.m_WidgetDummy.IsVisible() && control.m_WidgetDummy.IsMouseOver(pos))
			{
				DraggingControl = control;
				DragBeginOffset = control.m_TempOffset;
				DragBeginPos = pos;
				break;
			}
		}
	}

	private void TouchUpdate(Vector2 pos)
	{
		pos.y = (float)Screen.height - pos.y;
		Vector2 vector = pos - DragBeginPos;
		if (DraggingControl != null)
		{
			DraggingControl.m_TempOffset = DragBeginOffset + vector;
			UpdateSpritesPos();
			RedrawSprites();
		}
	}

	private void TouchEnd(Vector2 pos)
	{
		DraggingControl = null;
	}

	private void Update()
	{
		if (isOn && Input.touchCount != 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				TouchBegin(touch.position);
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				TouchUpdate(touch.position);
			}
			else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
			{
				TouchEnd(touch.position);
			}
		}
	}

	private void UpdateSpritesPos()
	{
		foreach (CustomControl control in m_Controls)
		{
			control.m_Transform.position = control.m_OptionsPos.OrigPos + control.m_TempOffset;
		}
	}

	private void RedrawSprites()
	{
		foreach (CustomControl control in m_Controls)
		{
			control.m_WidgetDummy.ShowImmediate(true, true);
		}
	}

	private void ShowSchemeSpecificSprites()
	{
		if (GuiOptions.m_ControlScheme != 0 && GuiOptions.m_ControlScheme != GuiOptions.E_ControlScheme.Scheme2)
		{
		}
	}

	private void HideSchemeSpecificSprites()
	{
	}
}
