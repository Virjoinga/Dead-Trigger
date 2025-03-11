using System.Collections;
using UnityEngine;

[AddComponentMenu("GUI/Menu/GuiIngameMenu")]
public class GuiIngameMenu : MonoBehaviour
{
	public static GuiIngameMenu Instance;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Pivot m_PivotLoading;

	private GUIBase_Layout m_HelpLayout;

	private static string s_PivotName = "IngameMenu";

	private static string s_PivotLoadingName = "Loading";

	private static string s_PivotHelpName = "MainHelp";

	private static string s_LayoutName = "Menu_Layout";

	private static string s_ResumeButtonName = "ResumeButton";

	private static string s_OptButtonName = "OptButton";

	private static string s_HelpButtonName = "HelpButton";

	private static string s_ExitButtonName = "ExitButton";

	private bool exiting;

	private bool m_IsInitialized;

	private bool m_IsVisible;

	private bool HACK_disableInputForOneTick;

	private bool m_PauseDown = true;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		GuiOptionsMenu.Instance.m_OnHideDelegate = OnOptionsHide;
	}

	public void Init(Mission mission)
	{
		m_Pivot = MFGuiManager.Instance.GetPivot(s_PivotName);
		if (!m_Pivot)
		{
			Debug.LogError("'" + s_PivotName + "' not found!!! Assert should come now");
			return;
		}
		m_Layout = m_Pivot.GetLayout(s_LayoutName);
		if (!m_Layout)
		{
			Debug.LogError("'" + s_LayoutName + "' not found!!! Assert should come now");
			return;
		}
		m_PivotLoading = MFGuiManager.Instance.GetPivot(s_PivotLoadingName);
		if (!m_PivotLoading)
		{
			Debug.LogError("'" + s_PivotLoadingName + "' not found!");
			return;
		}
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(s_PivotHelpName);
		if (!pivot)
		{
			Debug.LogError("'" + s_PivotHelpName + "' not found!");
			return;
		}
		m_HelpLayout = GuiBaseUtils.GetLayout("01Buttons_Layout", pivot);
		if (Game.Instance.IsXperiaPlay)
		{
			m_HelpLayout = GuiBaseUtils.GetLayout("Xperia_Layout", pivot);
		}
		GuiBaseUtils.RegisterButtonDelegate(m_HelpLayout, "Back_Button", null, OnBackFromHelp);
		PrepareButton(m_Layout, s_ResumeButtonName, null, ResumeButtonDelegate);
		PrepareButton(m_Layout, s_OptButtonName, null, OptButtonDelegate);
		PrepareButton(m_Layout, s_HelpButtonName, null, HelpButtonDelegate);
		PrepareButton(m_Layout, s_ExitButtonName, null, ExitButtonDelegate);
		GuiOptionsMenu.Instance.IsIngameMenu = true;
		GuiOptionsMenu.Instance.LateInitialize();
		GuiCustomizeControls.Instance.LateInitialize();
		m_IsInitialized = true;
	}

	public void LateUpdate()
	{
		if (m_IsInitialized && m_IsVisible)
		{
			if (HACK_disableInputForOneTick)
			{
				HACK_disableInputForOneTick = false;
				return;
			}
			if (Game.Instance.IsMogaConnected && Game.Instance.GetMogaGpad() != null)
			{
				if (Game.Instance.GetMogaGpad().getKeyCode(108) == 1)
				{
					m_PauseDown = false;
				}
				if (Input.GetKeyDown(KeyCode.Escape) || (Game.Instance.GetMogaGpad().getKeyCode(108) == 0 && !m_PauseDown))
				{
					m_PauseDown = true;
					if (m_Pivot.IsControlEnabled())
					{
						ResumeButtonDelegate(true);
					}
				}
			}
			if (m_Pivot.IsControlEnabled() && Input.GetKeyDown(KeyCode.Escape))
			{
				ResumeButtonDelegate(true);
			}
		}
		else if (m_HelpLayout != null && m_HelpLayout.IsVisible() && Input.GetKeyDown(KeyCode.Escape))
		{
			OnBackFromHelp(true);
		}
	}

	public void Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_Pivot, false);
		DisableRestartButton(false);
		m_IsVisible = false;
	}

	public void Show()
	{
		MFGuiManager.Instance.ShowPivot(m_Pivot, true);
		m_IsVisible = true;
		HACK_disableInputForOneTick = true;
		m_PauseDown = true;
	}

	public void DisableRestartButton(bool disable)
	{
	}

	private GUIBase_Button PrepareButton(GUIBase_Layout layout, string name, GUIBase_Button.TouchDelegate touch, GUIBase_Button.ReleaseDelegate release)
	{
		GUIBase_Button gUIBase_Button = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			gUIBase_Button = widget.GetComponent<GUIBase_Button>();
			if ((bool)gUIBase_Button)
			{
				gUIBase_Button.RegisterTouchDelegate(touch);
				gUIBase_Button.RegisterReleaseDelegate(release);
			}
		}
		return gUIBase_Button;
	}

	private void ResumeButtonDelegate(bool inside)
	{
		if (inside)
		{
			GuiHUD.Instance.SwitchBackFromIngameMenu();
		}
	}

	private void OptButtonDelegate(bool inside)
	{
		if (inside)
		{
			m_Pivot.EnableControls(false);
			GuiOptionsMenu.Instance.Show();
		}
	}

	private void OnOptionsHide()
	{
		m_Pivot.EnableControls(true);
		Show();
	}

	private void CustomiseButtonDelegate(bool inside)
	{
		if (inside)
		{
			Hide();
			GuiCustomizeControls.Instance.Show();
		}
	}

	private void ExitButtonDelegate(bool inside)
	{
		if (inside && !exiting)
		{
			exiting = true;
			ExitToMainMenu();
		}
	}

	private void HelpButtonDelegate(bool inside)
	{
		if (inside)
		{
			if (Game.Instance.IsMogaConnected)
			{
				GuiMogaPopup.Instance.ShowHelp(Game.Instance.IsMogaPro);
			}
			else if (custom_inputs.IsNVidiaShield())
			{
				GuiNvidiaShieldHelp.Instance.Show(OnShieldHelpBack, false);
				Hide();
			}
			else
			{
				Hide();
				MFGuiManager.Instance.ShowLayout(m_HelpLayout, true);
			}
		}
	}

	private void OnBackFromHelp(bool inside)
	{
		if (inside)
		{
			MFGuiManager.Instance.ShowLayout(m_HelpLayout, false);
			Show();
		}
	}

	private void OnShieldHelpBack(bool inside)
	{
		if (inside)
		{
			Show();
		}
	}

	private void RestartButtonDelegate(bool inside)
	{
		if (inside)
		{
			StartCoroutine(RestartCheckpoint());
		}
	}

	private IEnumerator RestartCheckpoint()
	{
		MFGuiManager.Instance.FadeOut(0.1f);
		TimeManager.Instance.TimeScale = 1f;
		yield return new WaitForSeconds(0.1f);
		GuiHUD.Instance.SwitchBackFromIngameMenu();
		Mission.Instance.RestartCheckpoint();
	}

	private void ExitToMainMenu()
	{
		Hide();
		Game.Instance.LoadMainMenu();
	}

	public void ShowLoadingScreen()
	{
		MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
	}
}
