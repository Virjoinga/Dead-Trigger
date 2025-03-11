public class MainScreen : BaseMenuScreen
{
	private static string s_PivotName = "Main";

	private static string s_ScreenLayoutName = "01Buttons_Layout";

	private static string s_PlayButtonName = "Play_Button";

	private static string s_EquipButtonName = "Equip_Button";

	private static string s_OptButtonName = "Opt_Button";

	private static string s_HelpButtonName = "Help_Button";

	private static string s_ExitButtonName = "Exit_Button";

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot(s_PivotName);
			m_ScreenLayout = GetLayout(s_PivotName, s_ScreenLayoutName);
			PrepareButton(m_ScreenLayout, s_PlayButtonName, null, OnPlay);
			PrepareButton(m_ScreenLayout, s_OptButtonName, null, OnTouchButtonOpt);
			PrepareButton(m_ScreenLayout, s_EquipButtonName, null, OnTouchButtonEquip);
			ButtonDisable(m_ScreenLayout, "Opt_Button", true);
			PrepareButton(m_ScreenLayout, "HiddenOptions_Demo", null, OnTouchButtonOpt);
			ButtonDisable(m_ScreenLayout, "Charts_Button", true);
			PrepareButton(m_ScreenLayout, s_HelpButtonName, null, OnTouchButtonHelp);
			PrepareButton(m_ScreenLayout, s_ExitButtonName, null, OnTouchButtonExit);
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, true);
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	private void OnPlay(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.ShowScreen("PlayServer");
	}

	private void OnTouchButtonOpt(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.ShowScreen("Options");
	}

	private void OnTouchButtonEquip(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.ShowScreen("Equip");
	}

	private void OnTouchButtonHelp(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.ShowScreen("Help");
	}

	private void OnTouchButtonExit(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Exit();
	}
}
