using UnityEngine;

public class SafeHavenScreen : BaseMenuScreen
{
	private const int TEXT_ID_OFFLINE = 2040108;

	private const int TEXT_ID_ONLINE = 2040109;

	private const int TEXT_ID_LOGIN = 2040105;

	private const int TEXT_ID_LOGOUT = 2040113;

	private const int TEXT_ID_LOGOUT_CONFIRM = 2040030;

	private const string SCREEN_FRIENDS = "Friends";

	private const string SCREEN_STATISTICS = "Statistics";

	private const string SCREEN_LOGIN = "Login";

	private const string SCREEN_BACKUP = "Backup";

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_FriendsButton;

	private GUIBase_Button m_ArchievementsButton;

	private GUIBase_Button m_StatisticsButton;

	private GUIBase_Button m_LoginButton;

	private GUIBase_Button m_BackupButton;

	private GUIBase_Button m_BackButton;

	public void OnScreenShow(string inShowScreenName)
	{
		switch (inShowScreenName)
		{
		case "Statistics":
			UpdateButtonDownState(m_StatisticsButton.Widget);
			break;
		case "Friends":
			UpdateButtonDownState(m_FriendsButton.Widget);
			break;
		case "Login":
			UpdateButtonDownState(m_LoginButton.Widget);
			break;
		case "Backup":
			UpdateButtonDownState(m_BackupButton.Widget);
			break;
		}
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("SafeHaven_Screen");
			m_ScreenLayout = GetLayout("SafeHaven_Screen", "SafeHaven_Layout");
			m_FriendsButton = PrepareButton(m_ScreenLayout, "Friends_Button", OnFriends, null);
			m_ArchievementsButton = PrepareButton(m_ScreenLayout, "Archievements_Button", OnArchievements, null);
			m_StatisticsButton = PrepareButton(m_ScreenLayout, "Statistics_Button", OnStatistics, null);
			m_LoginButton = PrepareButton(m_ScreenLayout, "Login_Button", OnLogin, null);
			m_BackupButton = PrepareButton(m_ScreenLayout, "Backup_Button", OnBackup, null);
			m_BackButton = PrepareButton(m_ScreenLayout, "Back_Button", OnBackButton, null);
			m_FriendsButton.autoColorLabels = true;
			m_ArchievementsButton.autoColorLabels = true;
			m_StatisticsButton.autoColorLabels = true;
			m_LoginButton.autoColorLabels = true;
			m_BackupButton.autoColorLabels = true;
			m_BackButton.autoColorLabels = true;
			m_FriendsButton.stayDown = true;
			m_ArchievementsButton.stayDown = true;
			m_StatisticsButton.stayDown = true;
			m_LoginButton.stayDown = true;
			m_BackupButton.stayDown = true;
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
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		CancelInvoke("UpdateGUIByConnectionStatus");
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
		UpdateButtonDownState(inWidget);
		m_OwnerMenu.ShowScreen("Friends", true);
	}

	private void OnArchievements(GUIBase_Widget inWidget)
	{
		UpdateButtonDownState(inWidget);
	}

	private void OnStatistics(GUIBase_Widget inWidget)
	{
		UpdateButtonDownState(inWidget);
		m_OwnerMenu.ShowScreen("Statistics", true);
	}

	private void OnLogin(GUIBase_Widget inWidget)
	{
		if (CloudUser.instance.isUserAuthenticated)
		{
			m_OwnerMenu.ShowPopup("Confirm", TextDatabase.instance[2040113], TextDatabase.instance[2040030], OnLogoutConfirmation);
			return;
		}
		UpdateButtonDownState(inWidget);
		m_OwnerMenu.ShowScreen("Login", true);
	}

	private void OnBackup(GUIBase_Widget inWidget)
	{
		UpdateButtonDownState(inWidget);
		m_OwnerMenu.ShowScreen("Backup", true);
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
			m_OwnerMenu.ShowScreen("Login", true);
			UpdateGUIByConnectionStatus();
		}
	}

	private void UpdateButtonDownState(GUIBase_Widget inWidget)
	{
		m_FriendsButton.ForceDownStatus(m_FriendsButton.Widget == inWidget);
		m_ArchievementsButton.ForceDownStatus(m_ArchievementsButton.Widget == inWidget);
		m_StatisticsButton.ForceDownStatus(m_StatisticsButton.Widget == inWidget);
		m_LoginButton.ForceDownStatus(m_LoginButton.Widget == inWidget);
		m_BackupButton.ForceDownStatus(m_BackupButton.Widget == inWidget);
	}

	private void UpdateGUIByConnectionStatus()
	{
		UpdateGUIByConnectionStatus(!base.isEnabled);
	}

	private void UpdateGUIByConnectionStatus(bool inDisable)
	{
		bool flag = Application.internetReachability == NetworkReachability.NotReachable;
		bool isUserAuthenticated = CloudUser.instance.isUserAuthenticated;
		m_FriendsButton.SetDisabled(flag || !isUserAuthenticated || inDisable);
		m_StatisticsButton.SetDisabled(inDisable);
		m_LoginButton.SetDisabled(flag || inDisable);
		m_BackButton.SetDisabled(inDisable);
		m_BackupButton.SetDisabled(flag || !isUserAuthenticated || inDisable);
		if (flag)
		{
			m_LoginButton.SetNewText(2040108);
		}
		else if (!isUserAuthenticated)
		{
			m_LoginButton.SetNewText(2040105);
		}
		else
		{
			m_LoginButton.SetNewText(2040113);
		}
		m_ArchievementsButton.Widget.Show(false, true);
	}
}
