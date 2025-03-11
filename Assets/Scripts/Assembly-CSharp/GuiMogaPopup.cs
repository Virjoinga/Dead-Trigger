using System.Collections.Generic;
using UnityEngine;

public class GuiMogaPopup : MonoBehaviour
{
	public static GuiMogaPopup Instance;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Label m_TextLabel;

	private bool m_initialised;

	private GUIBase_Layout m_HelpLayout;

	private GUIBase_Button m_HelpCloseButton;

	private GUIBase_Switch m_HelpSwitch;

	private List<GUIBase_Widget> m_TouchWidgets;

	private GUIBase_Widget m_Moga_Widget;

	private GUIBase_Widget m_MogaPro_Widget;

	private bool m_IsHelpOn;

	private float m_PopupHideTime;

	public bool IsShown()
	{
		return m_PopupHideTime > 0f;
	}

	public bool IsReady()
	{
		return m_initialised && MFGuiManager.Instance != null;
	}

	public bool IsHelpOn()
	{
		return m_IsHelpOn;
	}

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		Hide();
		CancelInvoke();
		Instance = null;
	}

	public void Init()
	{
		m_Pivot = MFGuiManager.Instance.GetPivot("MogaGui");
		if (m_Pivot == null)
		{
			return;
		}
		m_Layout = m_Pivot.GetLayout("Connection_Layout");
		if (m_Layout == null)
		{
			return;
		}
		m_TextLabel = GuiBaseUtils.PrepareLabel(m_Layout, "Text_Label");
		if (!(m_TextLabel == null))
		{
			m_HelpLayout = m_Pivot.GetLayout("MogaHelp_Layout");
			m_HelpCloseButton = GuiBaseUtils.GetButton(m_HelpLayout, "Close_Button");
			m_HelpSwitch = GuiBaseUtils.GetSwitch(m_HelpLayout, "ShowHelp_Switch");
			m_Moga_Widget = m_HelpLayout.GetWidget("Moga");
			m_MogaPro_Widget = m_HelpLayout.GetWidget("MogaPro");
			m_TouchWidgets = new List<GUIBase_Widget>();
			m_TouchWidgets.Add(m_HelpCloseButton.Widget);
			GUIBase_Button[] buttons = m_HelpSwitch.m_Buttons;
			foreach (GUIBase_Button gUIBase_Button in buttons)
			{
				m_TouchWidgets.Add(gUIBase_Button.Widget);
			}
			m_initialised = true;
		}
	}

	public void Show(int msgId, float hideTime)
	{
		if (m_initialised && MFGuiManager.Instance != null)
		{
			string newText = TextDatabase.instance[msgId];
			MFGuiManager.Instance.ShowLayout(m_Layout, true);
			m_TextLabel.SetNewText(newText);
			m_PopupHideTime = Time.realtimeSinceStartup + hideTime;
		}
	}

	public void Hide()
	{
		if (m_initialised && MFGuiManager.Instance != null)
		{
			MFGuiManager.Instance.ShowLayout(m_Layout, false);
			m_PopupHideTime = 0f;
		}
	}

	public void LateUpdate()
	{
		if (!m_initialised)
		{
			Init();
		}
	}

	public void ShowHelp(bool mogaPro)
	{
		if (m_initialised && MFGuiManager.Instance != null)
		{
			m_HelpSwitch.SetValue(GuiOptions.showMogaHelp);
			m_HelpCloseButton.RegisterReleaseDelegate(OnCloseButton);
			m_HelpSwitch.RegisterDelegate(OnHelpSwitch);
			MFGuiManager.Instance.ShowLayout(m_HelpLayout, true);
			if (mogaPro)
			{
				m_MogaPro_Widget.Show(true, true);
			}
			else
			{
				m_Moga_Widget.Show(true, true);
			}
			MFGuiManager.ControlEnabled = false;
			m_IsHelpOn = true;
		}
	}

	public void HideHelp()
	{
		if (m_initialised && MFGuiManager.Instance != null)
		{
			m_HelpCloseButton.RegisterReleaseDelegate(null);
			m_HelpSwitch.RegisterDelegate(null);
			MFGuiManager.Instance.ShowLayout(m_HelpLayout, false);
			MFGuiManager.ControlEnabled = true;
			m_IsHelpOn = false;
		}
	}

	private void OnCloseButton(bool inside)
	{
		if (inside)
		{
			HideHelp();
		}
	}

	private void OnHelpSwitch(bool val)
	{
		GuiOptions.showMogaHelp = val;
	}

	private void Update()
	{
		if (m_IsHelpOn)
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				Vector2 position = touch.position;
				position.y = (float)Screen.height - touch.position.y;
				if (touch.phase != 0 && touch.phase != TouchPhase.Ended)
				{
					continue;
				}
				foreach (GUIBase_Widget touchWidget in m_TouchWidgets)
				{
					if (touchWidget.IsVisible() && touchWidget.IsMouseOver(position))
					{
						if (touch.phase == TouchPhase.Began)
						{
							touchWidget.HandleTouchEvent(GUIBase_Widget.E_TouchPhase.E_TP_CLICK_BEGIN);
						}
						else if (touch.phase == TouchPhase.Ended)
						{
							touchWidget.HandleTouchEvent(GUIBase_Widget.E_TouchPhase.E_TP_CLICK_RELEASE);
						}
					}
				}
			}
		}
		if (m_PopupHideTime > 0f && m_PopupHideTime < Time.realtimeSinceStartup)
		{
			Hide();
		}
	}
}
