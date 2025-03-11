#define DEBUG
using UnityEngine;

public class RegisterScreen : BaseMenuScreen
{
	private const int TEXT_ID_USERNAME_HEADER = 2040503;

	private const int TEXT_ID_PASSWORD_HEADER = 2040504;

	private const int ERROR_CANT_CONTACT_SERVER = 2040517;

	private const int ERROR_USERNAME_EXIST = 2040518;

	private const int ERROR_USERNAME_TOO_SHORT = 2040519;

	private const int ERROR_PASSWORD_NOT_MATCH = 2040520;

	private const int ERROR_EMAIL_NOT_VALID = 2040521;

	private const int ERROR_PASSWORD_TOO_SHORT = 2040522;

	private const int ERROR_USERNAME_INVALID_FORMAT = 2040523;

	private const string SCREEN_LOGIN = "Login";

	private static Color DefaultColor = Color.white;

	private static Color ErrorColor = Color.red;

	private UserNameAlreadyExist m_CloudActionUserName;

	private CreateNewMFAccount m_CloudActionCreateUser;

	private string m_NickName;

	private string m_UserName;

	private bool m_IsUserNameFree;

	private string m_FreeUserName;

	private bool m_UserNameHasValidFormat;

	private string m_PasswordHash;

	private string m_ConfirmPasswordHash;

	private int m_PasswordLength;

	private int m_ConfirmPasswordLength;

	private string m_Email;

	private bool m_IWantNews = true;

	private bool m_IAgreeWithLicence = true;

	private bool m_AccountDataError;

	private static string s_RegisterPivotName = "Register_Screen";

	private static string s_ScreenLayoutName = "Register_Layout";

	private static string s_UserNameButtonName = "Username_Button";

	private static string s_PasswordButtonName = "Pass_Button";

	private static string s_ConfirmPasswordButtonName = "PassConfirm_Button";

	private static string s_EmailButtonName = "Email_Button";

	private static string s_NewsToggleName = "WantNews_Switch";

	private static string s_CreateAccountButtonName = "Create_Button";

	private static string s_HintLabelName = "Hint_Label";

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_UserNameButton;

	private GUIBase_Button m_PasswordButton;

	private GUIBase_Button m_ConfirmPasswordButton;

	private GUIBase_Button m_EmailButton;

	private GUIBase_Switch m_NewsToggle;

	private GUIBase_Button m_CreateAccountButton;

	private GUIBase_Label m_HintLabel;

	private GUIBase_Label m_UserNameHeader;

	private GUIBase_Label m_PasswordHeader;

	private GUIBase_Label m_ConfirmPasswordHeader;

	private GUIBase_Label m_EmailHeader;

	private bool isUserNmaeValid
	{
		get
		{
			return !string.IsNullOrEmpty(m_UserName) && m_UserName.Length >= CloudUser.MIN_ACCOUNT_NAME_LENGTH && m_UserNameHasValidFormat;
		}
	}

	private bool isPasswordValid
	{
		get
		{
			return !string.IsNullOrEmpty(m_PasswordHash) && m_PasswordHash == m_ConfirmPasswordHash;
		}
	}

	private string passwordGUIText
	{
		get
		{
			return (m_PasswordLength <= 0) ? string.Empty : new string('*', m_PasswordLength);
		}
	}

	private string confirmGUIText
	{
		get
		{
			return (m_ConfirmPasswordLength <= 0) ? string.Empty : new string('*', m_ConfirmPasswordLength);
		}
	}

	private bool isEmailValid
	{
		get
		{
			return !string.IsNullOrEmpty(m_Email);
		}
	}

	private bool accountDataValid
	{
		get
		{
			return m_IsUserNameFree && !m_AccountDataError && isUserNmaeValid && isPasswordValid && isEmailValid && m_IAgreeWithLicence;
		}
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot(s_RegisterPivotName);
			m_ScreenLayout = GetLayout(s_RegisterPivotName, s_ScreenLayoutName);
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			m_UserNameButton = PrepareButton(m_ScreenLayout, s_UserNameButtonName, null, Delegate_UserName);
			m_PasswordButton = PrepareButton(m_ScreenLayout, s_PasswordButtonName, null, Delegate_Password);
			m_ConfirmPasswordButton = PrepareButton(m_ScreenLayout, s_ConfirmPasswordButtonName, null, Delegate_ConfirmPassword);
			m_EmailButton = PrepareButton(m_ScreenLayout, s_EmailButtonName, null, Delegate_Email);
			m_NewsToggle = PrepareSwitch(m_ScreenLayout, s_NewsToggleName, Delegate_News);
			m_CreateAccountButton = PrepareButton(m_ScreenLayout, s_CreateAccountButtonName, null, Delegate_CreateAccount);
			m_CreateAccountButton.autoColorLabels = true;
			m_HintLabel = PrepareLabel(m_ScreenLayout, s_HintLabelName);
			m_UserNameHeader = PrepareLabel(m_ScreenLayout, "UserName_Header");
			m_PasswordHeader = PrepareLabel(m_ScreenLayout, "Pass_Header");
			m_ConfirmPasswordHeader = PrepareLabel(m_ScreenLayout, "PassConfirm_Header");
			m_EmailHeader = PrepareLabel(m_ScreenLayout, "Email_Header");
			string format = TextDatabase.instance[2040503];
			string newText = string.Format(format, CloudUser.MIN_ACCOUNT_NAME_LENGTH, CloudUser.MAX_ACCOUNT_NAME_LENGTH);
			m_UserNameHeader.SetNewText(newText);
			string format2 = TextDatabase.instance[2040504];
			string newText2 = string.Format(format2, CloudUser.MIN_PASSWORD_LENGTH);
			m_PasswordHeader.SetNewText(newText2);
			DefaultColor = m_UserNameHeader.Widget.m_Color;
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		m_UserName = null;
		m_NickName = null;
		m_PasswordHash = null;
		m_ConfirmPasswordHash = null;
		m_PasswordLength = 0;
		m_ConfirmPasswordLength = 0;
		m_Email = null;
		m_IWantNews = true;
		m_UserNameButton.SetNewText(string.Empty);
		m_PasswordButton.SetNewText(string.Empty);
		m_ConfirmPasswordButton.SetNewText(string.Empty);
		m_EmailButton.SetNewText(string.Empty);
		m_NewsToggle.SetValue(true);
		m_HintLabel.SetNewText(null);
		m_UserNameHeader.Widget.m_Color = DefaultColor;
		m_PasswordHeader.Widget.m_Color = DefaultColor;
		m_ConfirmPasswordHeader.Widget.m_Color = DefaultColor;
		m_EmailHeader.Widget.m_Color = DefaultColor;
		UpdateCreateAccountButton();
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
		if (m_CloudActionUserName != null && m_CloudActionUserName.isDone)
		{
			if (m_CloudActionUserName.status == BaseCloudAction.E_Status.Failed)
			{
				ShowHint(2040517);
				m_AccountDataError = true;
			}
			else if (m_CloudActionUserName.userExist)
			{
				ShowHint(2040518);
				m_UserNameHeader.Widget.m_Color = ErrorColor;
				m_AccountDataError = true;
			}
			else
			{
				m_IsUserNameFree = true;
				m_FreeUserName = m_CloudActionUserName.userName;
				UpdateCreateAccountButton();
			}
			m_CloudActionUserName = null;
		}
		if (m_CloudActionCreateUser != null && m_CloudActionCreateUser.isDone)
		{
			if (m_CloudActionCreateUser.status == BaseCloudAction.E_Status.Failed)
			{
				ShowHint(2040517);
				Debug.LogWarning("Can't create new account. Reason: " + m_CloudActionCreateUser.failInfo);
			}
			else
			{
				CloudUser.instance.SetLoginData(m_NickName, m_UserName, m_PasswordHash, m_PasswordLength, false, false);
				m_OwnerMenu.ShowPopup("Authentication", "Authentication", string.Empty, AuthenticationResultHandler);
			}
			m_CloudActionCreateUser = null;
			UpdateCreateAccountButton();
		}
		base.OnGUI_Update();
	}

	private void AuthenticationResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			m_OwnerMenu.Back();
		}
	}

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
		m_OwnerMenu.ShowPopup("Login", string.Empty, string.Empty);
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
			ShowKeyboard(component, E_KeyBoardMode.Default, Delegate_OnKeyboardClose, component.GetText(), "Enter username", CloudUser.MAX_ACCOUNT_NAME_LENGTH);
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

	private void Delegate_ConfirmPassword(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_ConfirmPasswordButton.Widget);
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

	private void Delegate_Email(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_EmailButton.Widget);
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
			ShowKeyboard(component, E_KeyBoardMode.Email, Delegate_OnKeyboardClose, component.GetText(), "Enter email address");
		}
	}

	private void Delegate_News(bool switchValue)
	{
		m_IWantNews = switchValue;
	}

	private void Delegate_CreateAccount(GUIBase_Widget inInstigator)
	{
		DebugUtils.Assert(inInstigator == m_CreateAccountButton.Widget);
		if (accountDataValid)
		{
			m_CloudActionCreateUser = CloudUser.instance.CreateNewUser(m_UserName, m_PasswordHash, m_NickName, m_Email, m_IWantNews);
			UpdateCreateAccountButton();
		}
		else
		{
			Debug.LogError("Internal error. Create account button has to be disabled if Account Data are not valid");
		}
	}

	private void Delegate_OnKeyboardClose(GUIBase_Button inInput, string inKeyboardText, bool inInputCanceled)
	{
		if (inInput == m_UserNameButton)
		{
			if (string.IsNullOrEmpty(inKeyboardText))
			{
				m_NickName = string.Empty;
				m_UserName = string.Empty;
				m_FreeUserName = null;
				VerifyAccountData();
				UpdateCreateAccountButton();
				return;
			}
			inKeyboardText.Trim();
			if (m_NickName != inKeyboardText)
			{
				m_NickName = inKeyboardText;
				m_UserName = inKeyboardText.ToLower();
				m_UserNameButton.SetNewText(m_NickName);
				m_FreeUserName = null;
				VerifyAccountData();
				UpdateCreateAccountButton();
			}
		}
		else if (inInput == m_PasswordButton)
		{
			if (string.IsNullOrEmpty(inKeyboardText))
			{
				m_PasswordHash = null;
				m_PasswordLength = 0;
			}
			else
			{
				m_PasswordHash = CloudServices.CalcPasswordHash(inKeyboardText);
				m_PasswordLength = inKeyboardText.Length;
			}
			m_PasswordButton.SetNewText(passwordGUIText);
			VerifyAccountData();
			UpdateCreateAccountButton();
		}
		else if (inInput == m_ConfirmPasswordButton)
		{
			if (string.IsNullOrEmpty(inKeyboardText))
			{
				m_ConfirmPasswordHash = null;
				m_ConfirmPasswordLength = 0;
			}
			else
			{
				m_ConfirmPasswordHash = CloudServices.CalcPasswordHash(inKeyboardText);
				m_ConfirmPasswordLength = inKeyboardText.Length;
			}
			m_ConfirmPasswordButton.SetNewText(confirmGUIText);
			VerifyAccountData();
			UpdateCreateAccountButton();
		}
		else if (inInput == m_EmailButton)
		{
			if (m_Email != inKeyboardText)
			{
				m_Email = inKeyboardText;
				m_EmailButton.SetNewText(m_Email);
				VerifyAccountData();
				UpdateCreateAccountButton();
			}
		}
		else
		{
			Debug.LogError("Unknown input widget !!!");
		}
	}

	private void UpdateCreateAccountButton()
	{
		m_CreateAccountButton.SetDisabled(!accountDataValid || (m_CloudActionCreateUser != null && !m_CloudActionCreateUser.isDone));
	}

	private void CheckUserName(string inName)
	{
		m_IsUserNameFree = false;
		m_CloudActionUserName = null;
		if (!string.IsNullOrEmpty(inName))
		{
			m_CloudActionUserName = CloudUser.instance.CheckIfUserNameExist(inName);
		}
	}

	private void VerifyAccountData()
	{
		m_AccountDataError = false;
		if (!string.IsNullOrEmpty(m_Email))
		{
			m_Email.Trim();
			int num = m_Email.IndexOf('@');
			if (num <= 0 || num >= m_Email.Length - 1)
			{
				m_AccountDataError = true;
				m_HintLabel.SetNewText(2040521);
				m_EmailHeader.Widget.m_Color = ErrorColor;
			}
			else
			{
				m_EmailHeader.Widget.m_Color = DefaultColor;
			}
		}
		if (!string.IsNullOrEmpty(m_PasswordHash) && !string.IsNullOrEmpty(m_ConfirmPasswordHash))
		{
			if (m_PasswordHash != m_ConfirmPasswordHash)
			{
				m_AccountDataError = true;
				m_HintLabel.SetNewText(2040520);
				m_PasswordHeader.Widget.m_Color = ErrorColor;
				m_ConfirmPasswordHeader.Widget.m_Color = ErrorColor;
			}
			else if (m_PasswordLength < CloudUser.MIN_PASSWORD_LENGTH)
			{
				m_AccountDataError = true;
				m_HintLabel.SetNewText(2040522);
				m_PasswordHeader.Widget.m_Color = ErrorColor;
				m_ConfirmPasswordHeader.Widget.m_Color = ErrorColor;
			}
			else
			{
				m_PasswordHeader.Widget.m_Color = DefaultColor;
				m_ConfirmPasswordHeader.Widget.m_Color = DefaultColor;
			}
		}
		if (!string.IsNullOrEmpty(m_UserName))
		{
			if (m_UserName.Length < CloudUser.MIN_ACCOUNT_NAME_LENGTH)
			{
				m_AccountDataError = true;
				m_HintLabel.SetNewText(2040519);
				m_UserNameHeader.Widget.m_Color = ErrorColor;
			}
			else
			{
				m_UserNameHasValidFormat = UserNameHasValidFormat(m_UserName);
				if (!m_UserNameHasValidFormat)
				{
					m_AccountDataError = true;
					m_HintLabel.SetNewText(2040523);
					m_UserNameHeader.Widget.m_Color = ErrorColor;
				}
				else if (m_FreeUserName != m_UserName)
				{
					m_UserNameHeader.Widget.m_Color = DefaultColor;
					CheckUserName(m_UserName);
				}
			}
		}
		if (!m_AccountDataError)
		{
			ClearHint();
		}
	}

	private void ClearHint()
	{
		m_HintLabel.Clear();
	}

	private void ShowHint(int inHintText)
	{
		m_HintLabel.SetNewText(inHintText);
	}

	public static bool UserNameHasValidFormat(string inUserName)
	{
		if (string.IsNullOrEmpty(inUserName))
		{
			return false;
		}
		for (int i = 0; i < inUserName.Length; i++)
		{
			if (!char.IsLetter(inUserName[i]) && !char.IsNumber(inUserName[i]))
			{
				return false;
			}
		}
		return true;
	}
}
