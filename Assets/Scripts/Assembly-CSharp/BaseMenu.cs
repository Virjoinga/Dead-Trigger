using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMenu : MonoBehaviour, IScreenOwner
{
	private Dictionary<string, BaseMenuScreen> m_Screens = new Dictionary<string, BaseMenuScreen>();

	private Stack<BaseMenuScreen> m_ActiveScreens = new Stack<BaseMenuScreen>();

	private Dictionary<BaseMenuScreen, string> m_ScreensToNames_TODO = new Dictionary<BaseMenuScreen, string>();

	protected string activeScreenName = string.Empty;

	protected BaseMenuScreen activeScreen
	{
		get
		{
			return (m_ActiveScreens.Count <= 0) ? null : m_ActiveScreens.Peek();
		}
	}

	protected int screenStackDepth
	{
		get
		{
			return m_ActiveScreens.Count;
		}
	}

	public abstract void DoCommand(string inCommand);

	public abstract void ShowScreen(string inScreenName, bool inClearStack = false);

	public abstract void ShowPopup(string inPopupName, string inCaption, string inText, PopupHandler inHandler = null);

	public abstract void Back();

	public abstract void Exit();

	public bool IsThisScreenTop(BaseMenuScreen inScreen)
	{
		return activeScreen != null && activeScreen == inScreen;
	}

	protected void _RegisterScreen(string inScreenName, BaseMenuScreen inScreen)
	{
		m_Screens[inScreenName] = inScreen;
		m_ScreensToNames_TODO[inScreen] = inScreenName;
	}

	protected void _InitScreens(IScreenOwner inOwner)
	{
		foreach (BaseMenuScreen value in m_Screens.Values)
		{
			value.Screen_Init(inOwner);
		}
	}

	protected void _UpdateVisibleScreens()
	{
		if (m_ActiveScreens == null || m_ActiveScreens.Count <= 0)
		{
			return;
		}
		List<BaseMenuScreen> list = new List<BaseMenuScreen>();
		foreach (BaseMenuScreen activeScreen in m_ActiveScreens)
		{
			if (!(activeScreen == null))
			{
				if (!activeScreen.isVisible)
				{
					break;
				}
				list.Add(activeScreen);
			}
		}
		foreach (BaseMenuScreen item in list)
		{
			item.Screen_Update();
		}
	}

	protected void _ClearStack()
	{
		while (m_ActiveScreens.Count > 0)
		{
			BaseMenuScreen baseMenuScreen = m_ActiveScreens.Pop();
			if (!baseMenuScreen.isEnabled)
			{
				baseMenuScreen.Screen_Enable();
			}
			if (baseMenuScreen != null && baseMenuScreen.isVisible)
			{
				baseMenuScreen.Screen_Hide();
			}
		}
		activeScreenName = null;
	}

	protected void _HideTopScreen()
	{
		if (m_ActiveScreens.Count > 0)
		{
			BaseMenuScreen baseMenuScreen = m_ActiveScreens.Peek();
			if (baseMenuScreen != null)
			{
				baseMenuScreen.Screen_Hide();
			}
		}
	}

	protected void _DisableTopScreen()
	{
		if (m_ActiveScreens.Count > 0)
		{
			BaseMenuScreen baseMenuScreen = m_ActiveScreens.Peek();
			if (baseMenuScreen != null)
			{
				baseMenuScreen.Screen_Disable();
			}
		}
	}

	protected void _ShowScreen(string inScreenName)
	{
		BaseMenuScreen value;
		if (m_Screens.TryGetValue(inScreenName, out value))
		{
			activeScreenName = inScreenName;
			m_ActiveScreens.Push(value);
			value.Screen_Show();
		}
		else
		{
			Debug.LogError("Screen with name [ " + inScreenName + " ] doesn't exist !!!");
		}
	}

	protected void _Back()
	{
		if (m_ActiveScreens.Count > 1)
		{
			BaseMenuScreen baseMenuScreen = m_ActiveScreens.Pop();
			baseMenuScreen.Screen_Hide();
			baseMenuScreen = m_ActiveScreens.Peek();
			if (!baseMenuScreen.isEnabled)
			{
				baseMenuScreen.Screen_Enable();
			}
			else
			{
				baseMenuScreen.Screen_Show();
			}
			activeScreenName = m_ScreensToNames_TODO[baseMenuScreen];
		}
	}
}
