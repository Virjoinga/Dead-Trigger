using System.Runtime.CompilerServices;
using UnityEngine;

public class SafeHaven : BaseMenu, IScreenOwner
{
	public delegate void MenuDelegate();

	private static SafeHaven sInstance;

	private bool m_IsInitialized;

	private int m_HACK_FrameCount = 3;

	private MenuController MenuCtrl = new MenuController();

	private SafeHavenMainScreen m_MainScreen;

	private SafeHavenScreen m_SafeHavenScreen;

	private BaseMenuScreen m_LoginScreen;

	private RegisterScreen m_RegisterScreen;

	private StatisticsScreen m_StatisticsScreen;

	private FriendsScreen m_FriendsScreen;

	private BackupScreen m_BackupScreen;

	private AuthenticationDialog m_AuthenticationDialog;

	private SelectFriendDialog m_SelectFriendDialog;

	private NewFriendDialog m_NewFriendDialog;

	private ForgotPasswordDialog m_ForgotPasswordDialog;

	private ConfirmDialog m_ConfirmDialog;

	private MessageBoxDialog m_MessageBox;

	public static SafeHaven Instance
	{
		get
		{
			return sInstance;
		}
	}

	[method: MethodImpl(32)]
	public static event MenuDelegate OnSafeHavenShow;

	[method: MethodImpl(32)]
	public static event MenuDelegate OnSafeHavenHide;

	private void Awake()
	{
		sInstance = this;
	}

	private void Start()
	{
		m_SafeHavenScreen = base.gameObject.AddComponent<SafeHavenScreen>();
		m_LoginScreen = base.gameObject.AddComponent<LoginScreen>();
		m_MainScreen = base.gameObject.AddComponent<SafeHavenMainScreen>();
		m_RegisterScreen = base.gameObject.AddComponent<RegisterScreen>();
		m_StatisticsScreen = base.gameObject.AddComponent<StatisticsScreen>();
		m_FriendsScreen = base.gameObject.AddComponent<FriendsScreen>();
		m_BackupScreen = base.gameObject.AddComponent<BackupScreen>();
		m_AuthenticationDialog = base.gameObject.AddComponent<AuthenticationDialog>();
		m_SelectFriendDialog = base.gameObject.AddComponent<SelectFriendDialog>();
		m_NewFriendDialog = base.gameObject.AddComponent<NewFriendDialog>();
		m_ForgotPasswordDialog = base.gameObject.AddComponent<ForgotPasswordDialog>();
		m_ConfirmDialog = base.gameObject.AddComponent<ConfirmDialog>();
		m_MessageBox = base.gameObject.AddComponent<MessageBoxDialog>();
		_RegisterScreen("MainBar", m_SafeHavenScreen);
		_RegisterScreen("MainScreen", m_MainScreen);
		_RegisterScreen("Login", m_LoginScreen);
		_RegisterScreen("CreateNewUser", m_RegisterScreen);
		_RegisterScreen("Statistics", m_StatisticsScreen);
		_RegisterScreen("Friends", m_FriendsScreen);
		_RegisterScreen("Backup", m_BackupScreen);
		_RegisterScreen("Authentication", m_AuthenticationDialog);
		_RegisterScreen("SelectFriend", m_SelectFriendDialog);
		_RegisterScreen("NewFriend", m_NewFriendDialog);
		_RegisterScreen("ForgotPassword", m_ForgotPasswordDialog);
		_RegisterScreen("Confirm", m_ConfirmDialog);
		_RegisterScreen("MessageBox", m_MessageBox);
	}

	private void LateUpdate()
	{
		if (!m_IsInitialized)
		{
			if (--m_HACK_FrameCount < 0)
			{
				_InitScreens(this);
				m_IsInitialized = true;
			}
		}
		else
		{
			_UpdateVisibleScreens();
			ProcessInput();
		}
	}

	public void Show()
	{
		if (SafeHaven.OnSafeHavenShow != null)
		{
			SafeHaven.OnSafeHavenShow();
		}
		ShowScreen("MainScreen");
	}

	public void Close()
	{
		_ClearStack();
		if (SafeHaven.OnSafeHavenHide != null)
		{
			SafeHaven.OnSafeHavenHide();
		}
	}

	public override void ShowScreen(string inScreenName, bool inClearStack = false)
	{
		if (!(activeScreenName == inScreenName))
		{
			if (inScreenName == "Main")
			{
				inClearStack = true;
				inScreenName = "MainScreen";
			}
			if (inClearStack)
			{
				_ClearStack();
			}
			else
			{
				_HideTopScreen();
			}
			_ShowScreen(inScreenName);
			UpdateSafeHavenMainBarEnableedStatus();
		}
	}

	public override void ShowPopup(string inPopupName, string inCaption, string inText, PopupHandler inHandler = null)
	{
		if (activeScreenName == inPopupName)
		{
			return;
		}
		_DisableTopScreen();
		_ShowScreen(inPopupName);
		if (activeScreenName == inPopupName)
		{
			BasePopupScreen basePopupScreen = base.activeScreen as BasePopupScreen;
			if (basePopupScreen != null)
			{
				basePopupScreen.SetCaption(inCaption);
				basePopupScreen.SetText(inText);
				basePopupScreen.SetHandler(inHandler);
			}
		}
		UpdateSafeHavenMainBarEnableedStatus();
	}

	public override void Back()
	{
		_Back();
		UpdateSafeHavenMainBarEnableedStatus();
	}

	public override void DoCommand(string inCommand)
	{
		Debug.LogError("Unknown command " + inCommand);
	}

	public override void Exit()
	{
		Close();
	}

	private void ProcessInput()
	{
		if (base.screenStackDepth <= 0)
		{
			return;
		}
		MenuCtrl.Update();
		if (MenuCtrl.PressedBack() && !CloudUser.instance.authenticationInProggres)
		{
			if (base.screenStackDepth > 1)
			{
				Back();
			}
			else
			{
				Close();
			}
		}
	}

	private void UpdateSafeHavenMainBarEnableedStatus()
	{
	}

	private void AuthenticationResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		ShowScreen("Main");
	}
}
