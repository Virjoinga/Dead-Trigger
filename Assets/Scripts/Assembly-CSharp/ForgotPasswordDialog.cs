#define DEBUG
using UnityEngine;

public class ForgotPasswordDialog : BasePopupScreen
{
	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_OKButton;

	private GUIBase_Button m_CancelButton;

	private GUIBase_Button m_NameButton;

	private string m_UserName = string.Empty;

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
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("ForgotPasword_Dialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("ForgotPasword_Layout");
			m_OKButton = PrepareButton(m_ScreenLayout, "OK_Button", null, null);
			m_NameButton = PrepareButton(m_ScreenLayout, "Username_Button", null, null);
			m_CancelButton = PrepareButton(m_ScreenLayout, "Cancel_Button", null, null);
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
		m_OKButton.RegisterReleaseDelegate2(Delegate_OK);
		m_CancelButton.RegisterReleaseDelegate2(Delegate_Cancel);
		m_NameButton.RegisterReleaseDelegate2(Delegate_UserName);
		if (CloudUser.instance.authenticationDataPresent)
		{
			m_UserName = CloudUser.instance.userName;
		}
		else
		{
			m_UserName = string.Empty;
		}
		m_NameButton.SetNewText(m_UserName);
		UpdateOKButton();
	}

	protected override void OnGUI_Hide()
	{
		m_UserName = string.Empty;
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

	private void Delegate_OK(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_OKButton.Widget);
		bool flag = !string.IsNullOrEmpty(m_UserName);
		if (flag)
		{
			GameCloudManager.AddAction(new ForgotPassword(m_UserName));
		}
		m_OwnerMenu.Back();
		SendResult((!flag) ? E_PopupResultCode.Cancel : E_PopupResultCode.Ok);
	}

	private void Delegate_Cancel(GUIBase_Widget inInstigator)
	{
		m_OwnerMenu.Back();
		SendResult(E_PopupResultCode.Cancel);
	}

	private void Delegate_UserName(GUIBase_Widget inInstigator)
	{
		Debug.Log("Delegate_FriendName: ");
		DebugUtils.Assert(inInstigator == m_NameButton.Widget);
		if (base.isInputActive)
		{
			Debug.LogWarning("Internal error. Interaction has to be disabled if Keyboard is active");
			return;
		}
		GUIBase_Button component = inInstigator.GetComponent<GUIBase_Button>();
		if (component == null)
		{
			Debug.LogError("Internal error !!! ");
		}
		else
		{
			ShowKeyboard(component, E_KeyBoardMode.Default, Delegate_OnKeyboardClose, component.GetText(), "Enter username");
		}
	}

	private void Delegate_OnKeyboardClose(GUIBase_Button inInput, string inKeyboardText, bool inInputCanceled)
	{
		DebugUtils.Assert(m_NameButton == inInput);
		m_UserName = inKeyboardText.ToLower();
		m_NameButton.SetNewText(m_UserName);
		UpdateOKButton();
	}

	private void UpdateOKButton()
	{
		bool flag = false;
		m_OKButton.SetDisabled(flag);
		m_OKButton.Widget.m_Color = ((!flag) ? Color.white : Color.gray);
	}
}
