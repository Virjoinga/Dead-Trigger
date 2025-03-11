public class HelpScreen : BaseMenuScreen
{
	private GUIBase_Layout m_ScreenLayout;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenLayout = GetLayout("MainHelp", "01Buttons_Layout");
			if (Game.Instance.IsXperiaPlay)
			{
				m_ScreenLayout = GetLayout("MainHelp", "Xperia_Layout");
			}
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnHelpButtonBack);
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, true);
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, false);
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

	private void OnHelpButtonBack(GUIBase_Widget inInstigator)
	{
		m_OwnerMenu.Back();
	}
}
