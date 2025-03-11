using UnityEngine;

public class SafeHavenMainScreen : BaseMenuScreen
{
	private const int TEXT_ID_OFFLINE = 2040108;

	private const int TEXT_ID_ONLINE = 2040109;

	private const int TEXT_ID_LOGIN = 2040105;

	private const int TEXT_ID_LOGOUT = 2040113;

	private const int TEXT_ID_LOGOUT_CONFIRM = 2040030;

	private const int TEXT_ID_SIGN_IN = 2040037;

	private const int TEXT_ID_SIGN_OUT = 2040039;

	private const int TEXT_ID_GUEST = 2040010;

	private const int TEXT_ID_HINT_DEFAULT = 2040031;

	private const int TEXT_ID_HINT_BACKUP = 2040032;

	private const string SCREEN_FRIENDS = "Friends";

	private const string SCREEN_STATISTICS = "Statistics";

	private const string SCREEN_LOGIN = "Login";

	private const string SCREEN_BACKUP = "Backup";

	private string m_RequestedScreen = string.Empty;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_FriendsButton;

	private GUIBase_Button m_ArchievementsButton;

	private GUIBase_Button m_StatisticsButton;

	private GUIBase_Button m_LoginButton;

	private GUIBase_Button m_BackupButton;

	private GUIBase_Button m_BackButton;

	private GUIBase_Label m_UserNameLabel;

	private GUIBase_Label m_HintLabel;

	public void OnScreenShow(string inShowScreenName)
	{
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("Main_Screen");
			m_ScreenLayout = GetLayout("Main_Screen", "Main_Layout");
			PrepareButton(m_ScreenLayout, "Friends_Button", OnFriends, null);
			m_ArchievementsButton = PrepareButton(m_ScreenLayout, "Archievements_Button", OnArchievements, null);
			PrepareButton(m_ScreenLayout, "Stats_Button", OnStatistics, null);
			m_LoginButton = PrepareButton(m_ScreenLayout, "Login_Button", OnLogin, null);
			PrepareButton(m_ScreenLayout, "Save_Button", OnBackup, null);
			m_BackButton = PrepareButton(m_ScreenLayout, "Back_Button", OnBackButton, null);
			m_UserNameLabel = PrepareLabel(m_ScreenLayout, "User_Label");
			m_HintLabel = PrepareLabel(m_ScreenLayout, "Hint_Label");
			m_LoginButton.autoColorLabels = true;
			m_BackButton.autoColorLabels = true;
			m_LoginButton.stayDown = true;
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
		UpdateGUIByConnectionStatus(!base.isEnabled);
		if (!IsInvoking("UpdateGUIByConnectionStatus"))
		{
			InvokeRepeating("UpdateGUIByConnectionStatus", 2f, 2f);
		}
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		m_RequestedScreen = string.Empty;
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		CancelInvoke("UpdateGUIByConnectionStatus");
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		base.OnGUI_Update();
		if (CloudUser.instance.isUserAuthenticated)
		{
			if (m_RequestedScreen == "Friends")
			{
				m_RequestedScreen = string.Empty;
				m_OwnerMenu.ShowScreen("Friends");
			}
			else if (m_RequestedScreen == "Backup")
			{
				m_RequestedScreen = string.Empty;
				m_OwnerMenu.ShowScreen("Backup");
			}
		}
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		m_ScreenPivot.EnableControls(true);
		UpdateGUIByConnectionStatus(false);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_ScreenPivot.EnableControls(false);
		UpdateGUIByConnectionStatus(true);
		base.OnGUI_Disable();
	}

	private void OnFriends(GUIBase_Widget inWidget)
	{
		if (CloudUser.instance.isUserAuthenticated)
		{
			m_OwnerMenu.ShowScreen("Friends");
			return;
		}
		m_RequestedScreen = "Friends";
		m_OwnerMenu.ShowPopup("Login", string.Empty, string.Empty);
	}

	private void OnArchievements(GUIBase_Widget inWidget)
	{
	}

	private void OnStatistics(GUIBase_Widget inWidget)
	{
		m_RequestedScreen = string.Empty;
		m_OwnerMenu.ShowScreen("Statistics");
	}

	private void OnLogin(GUIBase_Widget inWidget)
	{
		if (CloudUser.instance.isUserAuthenticated)
		{
			m_OwnerMenu.ShowPopup("Confirm", TextDatabase.instance[2040113], TextDatabase.instance[2040030], OnLogoutConfirmation);
			return;
		}
		m_RequestedScreen = string.Empty;
		m_OwnerMenu.ShowPopup("Login", string.Empty, string.Empty);
	}

	private void OnBackup(GUIBase_Widget inWidget)
	{
		if (CloudUser.instance.isUserAuthenticated)
		{
			m_OwnerMenu.ShowScreen("Backup");
			return;
		}
		m_RequestedScreen = "Backup";
		m_OwnerMenu.ShowPopup("Login", string.Empty, string.Empty);
	}

	private void OnBackButton(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Exit();
	}

	private void OnLogoutConfirmation(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			CloudUser.instance.LogoutLocalUser();
			UpdateGUIByConnectionStatus();
		}
	}

	private void UpdateGUIByConnectionStatus()
	{
		UpdateGUIByConnectionStatus(!base.isEnabled);
	}

	private void UpdateGUIByConnectionStatus(bool inDisable)
	{
		bool flag = Application.internetReachability == NetworkReachability.NotReachable;
		bool isUserAuthenticated = CloudUser.instance.isUserAuthenticated;
		m_LoginButton.SetDisabled(flag || inDisable);
		m_BackButton.SetDisabled(inDisable);
		if (CloudUser.instance.authenticationDataPresent)
		{
			m_UserNameLabel.SetNewText(CloudUser.instance.userName);
			m_HintLabel.SetNewText(2040032);
		}
		else
		{
			m_UserNameLabel.SetNewText(2040010);
			m_HintLabel.SetNewText(2040031);
		}
		if (flag)
		{
			m_LoginButton.SetNewText(2040108);
		}
		else if (!isUserAuthenticated)
		{
			m_LoginButton.SetNewText(2040037);
		}
		else
		{
			m_LoginButton.SetNewText(2040039);
		}
		m_ArchievementsButton.Widget.Show(false, true);
	}
}
