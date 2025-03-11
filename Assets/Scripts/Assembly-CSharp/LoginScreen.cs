#define DEBUG
using UnityEngine;

public class LoginScreen : BasePopupScreen
{
	private string m_LoadedNickName;

	private static string s_DefaultUserNameText = "USER NAME";

	private string m_LoadedUserName = s_DefaultUserNameText;

	private string m_UserName = s_DefaultUserNameText;

	private static string s_DefaultPasswordText = "PASSWORD";

	private string m_LoadedPassword = s_DefaultPasswordText;

	private string m_PasswordHash = s_DefaultPasswordText;

	private int m_LoadedPasswordLength = -1;

	private int m_PasswordLength = -1;

	private bool m_LoadedRememberMe = true;

	private bool m_RememberMe = true;

	private bool m_LoadedAutoLogin = true;

	private bool m_AutoLogin = true;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_CreateNewUserButton;

	private GUIBase_Button m_LoginButton;

	private GUIBase_Button m_UserNameButton;

	private GUIBase_Button m_PasswordButton;

	private GUIBase_Button m_ForgotButton;

	private GUIBase_Switch m_RememberMeButton;

	private GUIBase_Switch m_AutoLoginButton;

	private bool userNameChanged
	{
		get
		{
			return m_LoadedUserName != m_UserName;
		}
	}

	private bool inUserNmaeValid
	{
		get
		{
			return !string.IsNullOrEmpty(m_UserName) && m_UserName != s_DefaultUserNameText;
		}
	}

	private bool inPasswordValid
	{
		get
		{
			return !string.IsNullOrEmpty(m_PasswordHash) && m_PasswordHash != s_DefaultPasswordText && m_PasswordLength > 0;
		}
	}

	private bool passwordChanged
	{
		get
		{
			return m_LoadedPassword != m_PasswordHash;
		}
	}

	private string passwordGUIText
	{
		get
		{
			return (!inPasswordValid) ? s_DefaultPasswordText : new string('*', m_PasswordLength);
		}
	}

	private bool rememberMeChanged
	{
		get
		{
			return m_LoadedRememberMe != m_RememberMe;
		}
	}

	private bool autoLoginChanged
	{
		get
		{
			return m_LoadedAutoLogin != m_AutoLogin;
		}
	}

	private bool logindataChanged
	{
		get
		{
			return userNameChanged || passwordChanged || rememberMeChanged || autoLoginChanged;
		}
	}

	private bool logindataValid
	{
		get
		{
			return inUserNmaeValid && inPasswordValid;
		}
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("Login_Screen");
			m_ScreenLayout = GetLayout("Login_Screen", "Login_Layout");
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			m_CreateNewUserButton = PrepareButton(m_ScreenLayout, "Create_Button", null, Delegate_CreateUser);
			m_LoginButton = PrepareButton(m_ScreenLayout, "Login_Button", null, Delegate_Login);
			m_UserNameButton = PrepareButton(m_ScreenLayout, "Username_Button", null, Delegate_UserName);
			m_PasswordButton = PrepareButton(m_ScreenLayout, "Pass_Button", null, Delegate_Password);
			m_ForgotButton = PrepareButton(m_ScreenLayout, "Forgot_Button", null, Delegate_ForgotPassword);
			m_RememberMeButton = PrepareSwitch(m_ScreenLayout, "Remember_Switch", Delegate_RememberMe);
			m_AutoLoginButton = PrepareSwitch(m_ScreenLayout, "AutoLogin_Switch", Delegate_AutoLogin);
			m_LoginButton.autoColorLabels = true;
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		m_PasswordLength = -1;
		CloudUser instance = CloudUser.instance;
		if (!instance.GetLoginData(ref m_LoadedNickName, ref m_LoadedUserName, ref m_LoadedPassword, ref m_LoadedPasswordLength, ref m_LoadedRememberMe, ref m_LoadedAutoLogin))
		{
			m_LoadedUserName = (m_UserName = s_DefaultUserNameText);
			m_LoadedPassword = (m_PasswordHash = s_DefaultPasswordText);
			m_LoadedPasswordLength = (m_PasswordLength = 0);
			m_LoadedRememberMe = (m_RememberMe = true);
			m_LoadedAutoLogin = (m_AutoLogin = false);
		}
		else
		{
			m_UserName = m_LoadedUserName.ToLower();
			m_PasswordHash = m_LoadedPassword;
			m_RememberMe = m_LoadedRememberMe;
			m_PasswordLength = m_LoadedPasswordLength;
			m_AutoLogin = m_LoadedAutoLogin;
		}
		m_UserNameButton.SetNewText(m_UserName);
		m_PasswordButton.SetNewText(passwordGUIText);
		m_RememberMeButton.SetValue(m_RememberMe);
		m_AutoLoginButton.SetValue(m_AutoLogin);
		UpdateLoginButton();
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

	protected override void OnGUI_Enable()
	{
		m_ScreenPivot.EnableControls(true);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_ScreenPivot.EnableControls(false);
		base.OnGUI_Disable();
	}

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void Delegate_CreateUser(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_CreateNewUserButton.Widget);
		m_OwnerMenu.Back();
		m_OwnerMenu.ShowScreen("CreateNewUser");
	}

	private void Delegate_Login(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_LoginButton.Widget);
		if (logindataValid)
		{
			if (logindataChanged)
			{
				CloudUser instance = CloudUser.instance;
				instance.SetLoginData(m_UserName, m_UserName, m_PasswordHash, m_PasswordLength, m_RememberMe, m_AutoLogin);
			}
			CloudUser.instance.AuthenticateLocalUser();
			m_OwnerMenu.ShowPopup("Authentication", "Authentication", string.Empty, AuthenticationResultHandler);
		}
		else
		{
			Debug.LogError("Internal error. Login button has to be disabled if Login Data are not valid");
		}
	}

	private void Delegate_UserName(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_UserNameButton.Widget);
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

	private void Delegate_Password(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_PasswordButton.Widget);
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
			ShowKeyboard(component, E_KeyBoardMode.Password, Delegate_OnKeyboardClose, string.Empty, "Enter password");
		}
	}

	private void Delegate_RememberMe(bool switchValue)
	{
		m_RememberMe = switchValue;
	}

	private void Delegate_AutoLogin(bool switchValue)
	{
		m_AutoLogin = switchValue;
	}

	private void Delegate_ForgotPassword(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_ForgotButton.Widget);
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
			m_OwnerMenu.ShowPopup("ForgotPassword", string.Empty, string.Empty, ForgotPasswordHandler);
		}
	}

	private void ForgotPasswordHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			m_OwnerMenu.Back();
			m_OwnerMenu.ShowPopup("MessageBox", TextDatabase.instance[2040020], TextDatabase.instance[2040021]);
		}
	}

	private void Delegate_OnKeyboardClose(GUIBase_Button inInput, string inKeyboardText, bool inInputCanceled)
	{
		if (inInput == m_UserNameButton)
		{
			if (m_UserName != inKeyboardText)
			{
				string userName = m_LoadedUserName;
				if (!string.IsNullOrEmpty(inKeyboardText))
				{
					userName = inKeyboardText.ToLower();
				}
				m_UserName = userName;
				m_UserNameButton.SetNewText(m_UserName);
				UpdateLoginButton();
			}
		}
		else if (inInput == m_PasswordButton)
		{
			if (string.IsNullOrEmpty(inKeyboardText))
			{
				m_PasswordHash = m_LoadedPassword;
				m_PasswordLength = m_LoadedPasswordLength;
			}
			else
			{
				m_PasswordHash = CloudServices.CalcPasswordHash(inKeyboardText);
				m_PasswordLength = inKeyboardText.Length;
			}
			m_PasswordButton.SetNewText(passwordGUIText);
			UpdateLoginButton();
		}
		else
		{
			Debug.LogError("Unknown input widget !!!");
		}
	}

	private void AuthenticationDeadLoopFixer()
	{
	}

	private void UpdateLoginButton()
	{
		m_LoginButton.SetDisabled(!logindataValid);
	}

	private void AuthenticationResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			m_OwnerMenu.Back();
		}
	}

	public override void SetCaption(string inCaption)
	{
	}

	public override void SetText(string inText)
	{
	}
}
