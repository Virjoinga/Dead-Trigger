#define DEBUG
using UnityEngine;

public class NewFriendDialog : BasePopupScreen
{
	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_OKButton;

	private GUIBase_Button m_NameButton;

	private string m_FriendName = string.Empty;

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
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("AddNewFriend_Dialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("AddNewFriend_Layout");
			m_OKButton = PrepareButton(m_ScreenLayout, "OK_Button", null, null);
			m_NameButton = PrepareButton(m_ScreenLayout, "Username_Button", null, null);
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
		m_FriendName = string.Empty;
		m_NameButton.SetNewText(m_FriendName);
		m_OKButton.RegisterReleaseDelegate2(Delegate_OK);
		m_NameButton.RegisterReleaseDelegate2(Delegate_FriendName);
		UpdateOKButton();
	}

	protected override void OnGUI_Hide()
	{
		m_FriendName = string.Empty;
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
		if (!string.IsNullOrEmpty(m_FriendName))
		{
			GameCloudManager.friendList.AddNewFriend(m_FriendName);
			SendResult(E_PopupResultCode.Ok);
		}
		m_OwnerMenu.Back();
	}

	private void Delegate_FriendName(GUIBase_Widget inInstigator)
	{
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
		m_FriendName = inKeyboardText.ToLower();
		m_NameButton.SetNewText(m_FriendName);
		UpdateOKButton();
	}

	private void UpdateOKButton()
	{
		bool flag = false;
		m_OKButton.SetDisabled(flag);
		m_OKButton.Widget.m_Color = ((!flag) ? Color.white : Color.gray);
	}
}
