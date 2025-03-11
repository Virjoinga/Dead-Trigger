public class GuiShopStatusIAP : BasePopupScreen
{
	private const float minShowTime = 0.5f;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Label m_StatusLabel;

	private GUIBase_Label m_CaptionLabel;

	private E_PopupResultCode m_Result;

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
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("BuyWaitDialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("BuyWait_Layout");
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

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}
}
