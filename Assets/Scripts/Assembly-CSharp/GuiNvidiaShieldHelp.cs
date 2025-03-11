using UnityEngine;

public class GuiNvidiaShieldHelp : MonoBehaviour
{
	public static GuiNvidiaShieldHelp Instance;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Button m_CloseButton;

	private GUIBase_Button m_BackButton;

	private GUIBase_Sprite m_BlackBg;

	private GUIBase_Button.ReleaseDelegate m_OnClose;

	private bool m_Initialised;

	public bool IsShown { get; private set; }

	private void Awake()
	{
		Instance = this;
		IsShown = false;
	}

	private void OnDestroy()
	{
		Hide();
		Instance = null;
	}

	public void Init()
	{
		m_Pivot = MFGuiManager.Instance.GetPivot("NvidiaShieldHelp");
		if (!(m_Pivot == null))
		{
			m_Layout = m_Pivot.GetLayout("NvidiaShieldHelp_Layout");
			if (!(m_Layout == null))
			{
				m_BlackBg = GuiBaseUtils.PrepareSprite(m_Layout, "BlackBgScreen");
				m_BackButton = GuiBaseUtils.GetButton(m_Layout, "Back_Button");
				m_CloseButton = GuiBaseUtils.GetButton(m_Layout, "Close_Button");
				m_BackButton.RegisterReleaseDelegate(OnCloseButton);
				m_CloseButton.RegisterReleaseDelegate(OnCloseButton);
				m_Initialised = true;
			}
		}
	}

	public void Show(GUIBase_Button.ReleaseDelegate onClose, bool showBlackBg)
	{
		if (m_Initialised && MFGuiManager.Instance != null)
		{
			IsShown = true;
			m_OnClose = onClose;
			MFGuiManager.Instance.ShowLayout(m_Layout, true);
			if ((bool)m_BlackBg && showBlackBg)
			{
				m_BlackBg.Widget.Show(true, true);
			}
		}
	}

	public void Hide()
	{
		if (m_Initialised && MFGuiManager.Instance != null)
		{
			IsShown = false;
			MFGuiManager.Instance.ShowLayout(m_Layout, false);
		}
	}

	public void LateUpdate()
	{
		if (!m_Initialised)
		{
			Init();
		}
		else if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Escape))
		{
			OnCloseButton(true);
		}
	}

	private void OnCloseButton(bool inside)
	{
		if (inside)
		{
			Hide();
			if (m_OnClose != null)
			{
				m_OnClose(true);
				m_OnClose = null;
			}
		}
	}
}
