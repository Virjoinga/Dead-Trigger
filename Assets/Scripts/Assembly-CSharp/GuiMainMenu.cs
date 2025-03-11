using UnityEngine;

public class GuiMainMenu : BaseMenu
{
	public delegate void MenuDelegate();

	private bool m_IsInitialized;

	public static GuiMainMenu Instance;

	public static MenuDelegate m_OnCityMapSuspend;

	public static MenuDelegate m_OnCityMapResume;

	private GuiShopMenu m_ShopScreen;

	private GuiEquipMenu m_EquipScreen;

	private GuiShopBuyPopup m_ShopBuyPopup;

	private GuiShopUpgradePopup m_ShopUpgradePopup;

	private GuiShopNotFundsPopup m_NotFundsPopup;

	private GuiShopStatusIAP m_ShopStatusIAP;

	private GuiShopMessageBox m_ShopMessageBox;

	private GuiBankScreen m_BankScreen;

	private GuiOkDialog m_OkDialog;

	private GuiExitConfirm m_GuiExitConfirm;

	private GuiShopFreeGold m_FreeGoldScreen;

	private MenuController MenuCtrl = new MenuController();

	private bool HACK_disableInputForOneTick;

	public bool DisableBack;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
		}
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void Start()
	{
		m_ShopScreen = GuiShopMenu.Instance;
		m_EquipScreen = base.gameObject.AddComponent<GuiEquipMenu>();
		m_ShopBuyPopup = base.gameObject.AddComponent<GuiShopBuyPopup>();
		m_ShopUpgradePopup = base.gameObject.AddComponent<GuiShopUpgradePopup>();
		m_NotFundsPopup = GuiShopNotFundsPopup.Instance;
		m_ShopStatusIAP = base.gameObject.AddComponent<GuiShopStatusIAP>();
		m_ShopMessageBox = base.gameObject.AddComponent<GuiShopMessageBox>();
		m_BankScreen = base.gameObject.AddComponent<GuiBankScreen>();
		m_OkDialog = base.gameObject.AddComponent<GuiOkDialog>();
		m_GuiExitConfirm = base.gameObject.AddComponent<GuiExitConfirm>();
		m_FreeGoldScreen = base.gameObject.AddComponent<GuiShopFreeGold>();
		base.gameObject.AddComponent<GuiOptionsMenu>();
		_RegisterScreen("Shop", m_ShopScreen);
		_RegisterScreen("Equip", m_EquipScreen);
		_RegisterScreen("ShopBuyPopup", m_ShopBuyPopup);
		_RegisterScreen("ShopUpgradePopup", m_ShopUpgradePopup);
		_RegisterScreen("NotFundsPopup", m_NotFundsPopup);
		_RegisterScreen("ShopStatusIAP", m_ShopStatusIAP);
		_RegisterScreen("ShopMessageBox", m_ShopMessageBox);
		_RegisterScreen("Bank", m_BankScreen);
		_RegisterScreen("OkDialog", m_OkDialog);
		_RegisterScreen("ExitConfirm", m_GuiExitConfirm);
		_RegisterScreen("FreeGold", m_FreeGoldScreen);
	}

	private void InitGui()
	{
		_InitScreens(this);
		GuiOptionsMenu.Instance.IsIngameMenu = false;
		GuiOptionsMenu.Instance.LateInitialize();
		GuiOptionsMenu.Instance.CustomiseEnabled = false;
		m_IsInitialized = true;
	}

	private void LateUpdate()
	{
		if (m_IsInitialized)
		{
			_UpdateVisibleScreens();
			if (base.screenStackDepth > 0)
			{
				if (HACK_disableInputForOneTick)
				{
					HACK_disableInputForOneTick = false;
				}
				else
				{
					ProcessInput();
				}
			}
		}
		else
		{
			InitGui();
		}
	}

	public override void ShowScreen(string inScreenName, bool inClearStack = false)
	{
		if (!(activeScreenName == inScreenName))
		{
			int num = base.screenStackDepth;
			if (inClearStack)
			{
				_ClearStack();
			}
			else
			{
				_HideTopScreen();
			}
			_ShowScreen(inScreenName);
			if (num == 0 && base.screenStackDepth > 0)
			{
				m_OnCityMapSuspend();
				HACK_disableInputForOneTick = true;
			}
		}
	}

	public override void ShowPopup(string inPopupName, string inCaption, string inText, PopupHandler inHandler = null)
	{
		if (activeScreenName == inPopupName)
		{
			return;
		}
		int num = base.screenStackDepth;
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
		if (num == 0 && base.screenStackDepth > 0)
		{
			m_OnCityMapSuspend();
			HACK_disableInputForOneTick = true;
		}
	}

	public override void Back()
	{
		if (base.screenStackDepth > 1)
		{
			_Back();
			return;
		}
		_ClearStack();
		if (m_OnCityMapResume != null)
		{
			m_OnCityMapResume();
		}
	}

	public void RemoveTopFromStackHack()
	{
		_Back();
	}

	public void ClosePopupScreenHack()
	{
		if (base.screenStackDepth > 1)
		{
			_Back();
		}
		else
		{
			_ClearStack();
		}
	}

	public override void DoCommand(string inCommand)
	{
		Debug.LogError("Unknown command " + inCommand);
	}

	public override void Exit()
	{
		Debug.Log("Quiting application");
		Application.Quit();
	}

	public bool IsScreenActive(string screenName)
	{
		return activeScreenName == screenName;
	}

	private void ProcessInput()
	{
		if (base.screenStackDepth > 0)
		{
			MenuCtrl.Update();
			if (!DisableBack && MenuCtrl.PressedBack())
			{
				Back();
			}
		}
	}
}
