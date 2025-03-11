using UnityEngine;

public class AuthenticationDialog : BasePopupScreen
{
	private CloudUser.E_AuthenticationStatus lastAuthStatus;

	private static int AUTHENTICATION_IN_PROGRESS = 2040011;

	private static int AUTHENTICATION_GET_PPI = 2040012;

	private static int AUTHENTICATION_FAILED = 2040013;

	private static int AUTHENTICATION_OK = 2040014;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_OKButton;

	private GUIBase_Label m_ProggresLabel;

	public override void SetCaption(string inCaption)
	{
	}

	public override void SetText(string inText)
	{
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("Authentication_Dialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("Authentication_Layout");
			m_OKButton = PrepareButton(m_ScreenLayout, "OK_Button", null, Delegate_OK);
			m_ProggresLabel = PrepareLabel(m_ScreenLayout, "Text_Label");
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
		CloudUser.instance.AuthenticateLocalUser();
		lastAuthStatus = CloudUser.E_AuthenticationStatus.None;
		UpdateOKButton();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		if (base.isVisible && lastAuthStatus != CloudUser.instance.authenticationStatus)
		{
			switch (CloudUser.instance.authenticationStatus)
			{
			case CloudUser.E_AuthenticationStatus.InProgress:
				m_ProggresLabel.SetNewText(AUTHENTICATION_IN_PROGRESS);
				break;
			case CloudUser.E_AuthenticationStatus.RetrievingPPI:
				m_ProggresLabel.SetNewText(AUTHENTICATION_GET_PPI);
				break;
			case CloudUser.E_AuthenticationStatus.Failed:
				m_ProggresLabel.SetNewText(AUTHENTICATION_FAILED);
				break;
			case CloudUser.E_AuthenticationStatus.Ok:
				m_ProggresLabel.SetNewText(AUTHENTICATION_OK);
				CloseDialog(E_PopupResultCode.Success);
				break;
			}
			lastAuthStatus = CloudUser.instance.authenticationStatus;
			UpdateOKButton();
		}
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	private void CloseDialog(E_PopupResultCode inResult)
	{
		m_OwnerMenu.Back();
		SendResult(inResult);
	}

	private void Delegate_OK(GUIBase_Widget inInstigator)
	{
		CloseDialog((!CloudUser.instance.isUserAuthenticated) ? E_PopupResultCode.Failed : E_PopupResultCode.Success);
	}

	private void UpdateOKButton()
	{
		bool flag = lastAuthStatus != CloudUser.E_AuthenticationStatus.Ok && lastAuthStatus != CloudUser.E_AuthenticationStatus.Failed;
		m_OKButton.SetDisabled(flag);
		m_OKButton.Widget.m_Color = ((!flag) ? Color.white : Color.gray);
	}
}
