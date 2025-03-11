public class GuiExitConfirm : BasePopupScreen
{
	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Label m_StatusLabel;

	private GUIBase_Label m_CaptionLabel;

	public override void SetCaption(string inCaption)
	{
		m_CaptionLabel.SetNewText(inCaption);
	}

	public override void SetText(string inText)
	{
		m_StatusLabel.SetNewText(inText);
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("ExitConfirm");
			m_ScreenLayout = m_ScreenPivot.GetLayout("ExitConfirm_Layout");
			PrepareButton(m_ScreenLayout, "OK_Button", null, OnButtonOK);
			PrepareButton(m_ScreenLayout, "Cancel_Button", null, OnButtonCancel);
			m_StatusLabel = PrepareLabel(m_ScreenLayout, "Text_Label");
			m_CaptionLabel = PrepareLabel(m_ScreenLayout, "Caption_Label");
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		base.OnGUI_Show();
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, true);
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

	private void OnButtonOK(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
		SendResult(E_PopupResultCode.Ok);
	}

	private void OnButtonCancel(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
		SendResult(E_PopupResultCode.Cancel);
	}
}
