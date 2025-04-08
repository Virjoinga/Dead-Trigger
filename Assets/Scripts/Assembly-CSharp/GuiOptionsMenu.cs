using UnityEngine;

[AddComponentMenu("GUI/Menu/GuiOptionsMenu")]
public class GuiOptionsMenu : MonoBehaviour
{
	public delegate void OptionsHideDelegate();

	public static GuiOptionsMenu Instance;

	private bool m_IsInitialized;

	private int MAX_OPT_PAGE_IDX = 1;

	private int m_CurrentOptPage;

	private static string s_PivotOptName = "MainOpt";

	private static string s_LayoutOptSoundsName = "00Sounds_Layout";

	private static string s_LayoutOptControlsName = "00Controls_Layout";

	private static string s_LayoutOptButtonsName = "01Buttons_Layout";

	private static string s_ButtonOptPrevButtonName = "Prev_Button";

	private static string s_ButtonOptNextButtonName = "Next_Button";

	private static string s_ButtonOptResetButtonName = "Reset_Button";

	private static string s_SliderMusicName = "Music_Slider";

	private static string s_SliderSensitivityName = "Sensitivity_Slider";

	private static string s_SwitchInvertYName = "InvertY_Switch";

	private static string s_SwitchLefthandedName = "Lefthanded_Switch";

	private static string s_ControlsSchemeName = "ControlScheme_Enum";

	private static string s_CustomiseButtonName = "CustomiseButton";

	private static string s_GamepadButtonName = "GamepadButton";

	private static string s_ButtonBackName = "Back_Button";

	private GUIBase_Pivot m_PivotOpt;

	private GUIBase_Layout m_LayoutOptButtons;

	private GUIBase_Layout m_LayoutOptSounds;

	private GUIBase_Layout m_LayoutOptControls;

	private GUIBase_Switch m_SwitchYAxis;

	private GUIBase_Switch m_SwitchLefthanded;

	private GUIBase_Slider m_SliderMusic;

	private GUIBase_Slider m_SliderSensitivity;

	private GUIBase_Button m_CustomiseButton;

	private GUIBase_Switch m_MusicOn_Switch;

	private GUIBase_Enum m_ControlSchemeEnum;

	private GUIBase_Enum m_GraphicEnum;

	private GUIBase_Pivot m_Graphic_Pivot;

	private GUIBase_Button m_GpadButton;

	private GUIBase_Layout m_HelpLayout;

	private GUIBase_Button m_HelpButton;

	public OptionsHideDelegate m_OnHideDelegate;

	private bool m_RunDelegateAfterPivotHide;

	public bool CustomiseEnabled { get; set; }

	public bool IsIngameMenu { get; set; }

	private void Awake()
	{
		Instance = this;
		m_IsInitialized = false;
		CustomiseEnabled = true;
	}

	private void Start()
	{
		GuiOptions.Load();
		if ((bool)GuiCustomizeControls.Instance)
		{
			GuiCustomizeControls.Instance.m_OnHideDelegate = OnCustomizeHide;
		}
	}

	private void LateUpdate()
	{
		if (!m_IsInitialized)
		{
			return;
		}
		if (m_RunDelegateAfterPivotHide && !m_PivotOpt.IsVisible())
		{
			m_RunDelegateAfterPivotHide = false;
			if (m_OnHideDelegate != null)
			{
				m_OnHideDelegate();
			}
		}
		else if (m_PivotOpt.IsVisible() && m_PivotOpt.IsControlEnabled() && Input.GetKeyDown(KeyCode.Escape))
		{
			OnTouchOptionsButtonBack(true);
		}
		else if (m_HelpLayout != null && m_HelpLayout.IsVisible() && Input.GetKeyDown(KeyCode.Escape))
		{
			OnBackFromHelp(true);
		}
	}

	public void LateInitialize()
	{
		PrecacheObjects();
		RegisterDelegates();
		ApplyOptionsToWidgets();
		ApplyGraphicsOptions();
		m_IsInitialized = true;
	}

	private void PrecacheObjects()
	{
		m_PivotOpt = MFGuiManager.Instance.GetPivot(s_PivotOptName);
		m_LayoutOptButtons = GuiBaseUtils.GetLayout(s_LayoutOptButtonsName, m_PivotOpt);
		m_LayoutOptSounds = GuiBaseUtils.GetLayout(s_LayoutOptSoundsName, m_PivotOpt);
		m_LayoutOptControls = GuiBaseUtils.GetLayout(s_LayoutOptControlsName, m_PivotOpt);
	}

	private void RegisterDelegates()
	{
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptButtons, s_ButtonBackName, null, OnTouchOptionsButtonBack);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptButtons, s_ButtonOptPrevButtonName, null, OnTouchButtonOptPrev);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptButtons, s_ButtonOptNextButtonName, null, OnTouchButtonOptNext);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptButtons, s_ButtonOptResetButtonName, null, OnTouchButtonOptReset);
		m_SliderMusic = GuiBaseUtils.RegisterSliderDelegate(m_LayoutOptSounds, s_SliderMusicName, OnMusicSliderChange);
		m_SliderSensitivity = GuiBaseUtils.RegisterSliderDelegate(m_LayoutOptControls, s_SliderSensitivityName, OnSensitivitySliderChange);
		m_SwitchYAxis = GuiBaseUtils.RegisterSwitchDelegate(m_LayoutOptControls, s_SwitchInvertYName, OnInvertYChange);
		m_SwitchLefthanded = GuiBaseUtils.RegisterSwitchDelegate(m_LayoutOptControls, s_SwitchLefthandedName, OnLefthandedChange);
		m_ControlSchemeEnum = GuiBaseUtils.PrepareEnum(m_LayoutOptControls, s_ControlsSchemeName, OnControlSchemeChanged);
		m_ControlSchemeEnum.SetValue((int)GuiOptions.m_ControlScheme);
		m_Graphic_Pivot = MFGuiManager.Instance.GetPivot("GraphicDetails_Pivot");
		m_GraphicEnum = GuiBaseUtils.PrepareEnum(m_LayoutOptSounds, "GraphDetails_Enum", OnGraphicChanged);
		m_GraphicEnum.SetValue(GuiOptions.graphicDetail);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptSounds, "DetectGraphic_Button", null, OnDetectGraphicButton);
		m_MusicOn_Switch = GuiBaseUtils.RegisterSwitchDelegate(m_LayoutOptSounds, "MusicOn_Switch", OnMusicOnChange);
		m_CustomiseButton = GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptControls, s_CustomiseButtonName, null, CustomiseButtonDelegate);
		m_GpadButton = GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptControls, s_GamepadButtonName, null, GamepadButtonDelegate);
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("MainHelp");
		if (!IsIngameMenu)
		{
			m_HelpLayout = pivot.GetLayout("01Buttons_Layout");
			if (Game.Instance.IsXperiaPlay)
			{
				m_HelpLayout = pivot.GetLayout("Xperia_Layout");
			}
			GuiBaseUtils.RegisterButtonDelegate(m_HelpLayout, "Back_Button", null, OnBackFromHelp);
			m_HelpButton = GuiBaseUtils.RegisterButtonDelegate(m_LayoutOptControls, "Help_Button", null, HelpButtonDelegate);
		}
	}

	private void ApplyOptionsToWidgets()
	{
		m_SliderMusic.SetValue(GuiOptions.musicVolume);
		m_SliderSensitivity.SetValue(GuiOptions.sensitivity);
		m_SwitchYAxis.SetValue(GuiOptions.invertYAxis);
		m_SwitchLefthanded.SetValue(GuiOptions.leftHandAiming);
		m_ControlSchemeEnum.SetValue((int)GuiOptions.m_ControlScheme);
		m_GraphicEnum.SetValue(GuiOptions.graphicDetail);
		if (m_MusicOn_Switch != null)
		{
			m_MusicOn_Switch.SetValue(GuiOptions.musicOn);
		}
		if ((bool)Player.Instance)
		{
			Player.Instance.Controls.TouchControls.OnControlSchemeChange();
		}
	}

	private void ShowCurrentOptPage()
	{
		MFGuiManager.Instance.ShowPivot(m_PivotOpt, true);
		if (m_CurrentOptPage > MAX_OPT_PAGE_IDX)
		{
			m_CurrentOptPage = 0;
		}
		else if (m_CurrentOptPage < 0)
		{
			m_CurrentOptPage = MAX_OPT_PAGE_IDX;
		}
		MFGuiManager.Instance.ShowLayout(m_LayoutOptControls, m_CurrentOptPage == 0);
		MFGuiManager.Instance.ShowLayout(m_LayoutOptSounds, m_CurrentOptPage == 1);
		if (m_CurrentOptPage == 0)
		{
			m_CustomiseButton.Widget.Show(CustomiseEnabled, true);
			if (!IsIngameMenu)
			{
				m_HelpButton.Widget.Show(true, true);
			}
		}
		if (m_CurrentOptPage == 0)
		{
			m_GpadButton.Widget.Show(Game.IsGamepadConnected(), true);
		}
		if (m_CurrentOptPage == 1 && IsIngameMenu)
		{
			GuiBaseUtils.ShowPivotWidgets(m_Graphic_Pivot, false);
		}
	}

	private void OnTouchButtonOptPrev(bool inside)
	{
		if (inside)
		{
			m_CurrentOptPage--;
			ShowCurrentOptPage();
		}
	}

	private void OnTouchButtonOptNext(bool inside)
	{
		if (inside)
		{
			m_CurrentOptPage++;
			ShowCurrentOptPage();
		}
	}

	private void OnMusicSliderChange(float val)
	{
		GuiOptions.musicVolume = val;
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.ApplyOptionsChange();
		}
	}

	private void OnSensitivitySliderChange(float val)
	{
		GuiOptions.sensitivity = val;
	}

	private void OnInvertYChange(bool switchValue)
	{
		GuiOptions.invertYAxis = switchValue;
	}

	private void OnMusicOnChange(bool switchValue)
	{
		GuiOptions.musicOn = switchValue;
		if (MusicManager.Instance != null)
		{
			MusicManager.Instance.ApplyOptionsChange();
		}
	}

	private void OnLefthandedChange(bool switchValue)
	{
		GuiOptions.SetNewLeftHandAiming(switchValue);
	}

	private void OnTouchButtonOptReset(bool inside)
	{
		if (inside)
		{
			GuiOptions.ResetToDefaults();
			if (MusicManager.Instance != null)
			{
				MusicManager.Instance.ApplyOptionsChange();
			}
			ApplyOptionsToWidgets();
		}
	}

	private void OnTouchOptionsButtonBack(bool inside)
	{
		if (inside)
		{
			GuiOptions.Save();
			if (MusicManager.Instance != null)
			{
				MusicManager.Instance.ApplyOptionsChange();
			}
			Hide();
		}
	}

	private void OnControlSchemeChanged(int val)
	{
		GuiOptions.m_ControlScheme = (GuiOptions.E_ControlScheme)val;
		if ((bool)Player.Instance)
		{
			Player.Instance.Controls.TouchControls.OnControlSchemeChange();
		}
	}

	private void OnGraphicChanged(int val)
	{
		/*if (!GraphicsDetailsUtl.IsTegra3() && val == 3)
		{
			int num = 0;
			num = ((GuiOptions.graphicDetail == 0) ? 2 : 0);
			val = num;
			if (m_GraphicEnum != null)
			{
				m_GraphicEnum.SetValue(val);
			}
		}*/
		GuiOptions.graphicDetail = val;
		ApplyGraphicsOptions();
	}

	private void ApplyGraphicsOptions()
	{
		DeviceInfo.Initialize((DeviceInfo.Performance)GuiOptions.graphicDetail);
	}

	private void OnDetectGraphicButton(bool inside)
	{
		if (inside)
		{
			OnGraphicChanged(GuiOptions.GetDefaultGraphics());
			m_GraphicEnum.SetValue(GuiOptions.graphicDetail);
		}
	}

	private void CustomiseButtonDelegate(bool inside)
	{
		if (inside)
		{
			MFGuiManager.Instance.ShowPivot(m_PivotOpt, false);
			GuiCustomizeControls.Instance.Show();
		}
	}

	private void GamepadButtonDelegate(bool inside)
	{
		if (inside)
		{
			MFGuiManager.Instance.ShowPivot(m_PivotOpt, false);
			custom_inputs.Instance.m_OnHideDelegate = OnGamepadConfigHide;
			custom_inputs.Instance.ShowConfig();
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
				GuiNvidiaShieldHelp.Instance.Show(OnShieldHelpClose, false);
				m_PivotOpt.EnableControls(false);
			}
			else
			{
				MFGuiManager.Instance.ShowLayout(m_HelpLayout, true);
				m_PivotOpt.EnableControls(false);
			}
		}
	}

	private void OnBackFromHelp(bool inside)
	{
		if (inside)
		{
			MFGuiManager.Instance.ShowLayout(m_HelpLayout, false);
			m_PivotOpt.EnableControls(true);
		}
	}

	private void OnShieldHelpClose(bool inside)
	{
		m_PivotOpt.EnableControls(true);
	}

	private void OnCustomizeHide()
	{
		Show();
	}

	private void OnGamepadConfigHide()
	{
		Show();
	}

	private void Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_PivotOpt, false);
		m_RunDelegateAfterPivotHide = true;
	}

	public void Show()
	{
		ShowCurrentOptPage();
	}
}
