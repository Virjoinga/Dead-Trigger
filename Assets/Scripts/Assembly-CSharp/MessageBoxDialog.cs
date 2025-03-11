public class MessageBoxDialog : BasePopupScreen
{
	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Label m_Caption;

	private GUIBase_Label m_Message;

	public override void SetCaption(string inCaption)
	{
		m_Caption.SetNewText(inCaption);
	}

	public override void SetText(string inText)
	{
		m_Message.SetNewText(inText);
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("MessageBox_Dialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("MessageBox_Layout");
			PrepareButton(m_ScreenLayout, "OK_Button", null, Delegate_OK);
			m_Caption = PrepareLabel(m_ScreenLayout, "Caption_Label");
			m_Message = PrepareLabel(m_ScreenLayout, "Text_Label");
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
		if (base.isVisible)
		{
			base.OnGUI_Update();
		}
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	private void Delegate_OK(GUIBase_Widget inInstigator)
	{
		m_OwnerMenu.Back();
		SendResult(E_PopupResultCode.Ok);
	}
}
