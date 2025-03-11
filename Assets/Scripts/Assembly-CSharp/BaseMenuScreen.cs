using System.Collections;
using UnityEngine;

public abstract class BaseMenuScreen : MonoBehaviour
{
	public enum E_KeyBoardMode
	{
		Default = 0,
		Email = 1,
		Password = 2
	}

	public delegate void KeyboardClose(GUIBase_Button inInput, string inText, bool inInputCanceled);

	protected IScreenOwner m_OwnerMenu;

	private GUIBase_Button m_ActiveInput;

	public bool isInitialized { get; protected set; }

	public bool isVisible { get; private set; }

	public bool isEnabled { get; private set; }

	public bool isInputActive
	{
		get
		{
			return m_ActiveInput != null;
		}
	}

	protected virtual void OnGUI_Init()
	{
	}

	protected virtual void OnGUI_Show()
	{
	}

	protected virtual void OnGUI_Hide()
	{
	}

	protected virtual void OnGUI_Update()
	{
	}

	protected virtual void OnGUI_Enable()
	{
	}

	protected virtual void OnGUI_Disable()
	{
	}

	protected virtual void OnGUI_Destroy()
	{
	}

	public void Screen_Init(IScreenOwner inOwner)
	{
		if (isInitialized)
		{
			Debug.LogWarning("Screen " + base.name + " is already initialized");
		}
		else
		{
			m_OwnerMenu = inOwner;
			isEnabled = true;
			try
			{
				OnGUI_Init();
			}
			catch
			{
				isInitialized = false;
				Debug.LogWarning("Screen component " + GetType().Name + " on GameObject " + base.name + " is not properly initialized");
			}
		}
		if (!isInitialized)
		{
			Debug.LogWarning("Screen " + base.name + " was not initialized correctly");
		}
	}

	public void Screen_Show()
	{
		if (isInitialized && !isVisible)
		{
			OnGUI_Show();
			isVisible = true;
		}
	}

	public void Screen_Hide()
	{
		if (isInitialized && isVisible)
		{
			OnGUI_Hide();
			isVisible = false;
		}
	}

	public void Screen_Update()
	{
		if (isInitialized)
		{
			OnGUI_Update();
		}
	}

	public void Screen_Enable()
	{
		if (isInitialized && !isEnabled)
		{
			OnGUI_Enable();
			isEnabled = true;
		}
	}

	public void Screen_Disable()
	{
		if (isInitialized && isEnabled)
		{
			OnGUI_Disable();
			isEnabled = false;
		}
	}

	public void Screen_Destroy()
	{
		OnGUI_Destroy();
	}

	protected GUIBase_Pivot GetPivot(string inPivotName)
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(inPivotName);
		if (pivot == null)
		{
			throw new MFScreenInitException("Can't find pivot with name [ " + inPivotName + " }");
		}
		return pivot;
	}

	protected GUIBase_Layout GetLayout(string inPivotName, string inLayoutName)
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(inPivotName);
		if (pivot == null)
		{
			throw new MFScreenInitException("Can't find pivot with name [ " + inPivotName + " }");
		}
		GUIBase_Layout layout = pivot.GetLayout(inLayoutName);
		if (layout == null)
		{
			throw new MFScreenInitException("Can't find layout with name [ " + inLayoutName + " }");
		}
		return layout;
	}

	protected GUIBase_Widget GetWidget(GUIBase_Layout inLayout, string inName)
	{
		GUIBase_Widget widget = inLayout.GetWidget(inName);
		if (widget == null)
		{
			throw new MFScreenInitException("Can't find widget with name [ " + inName + " }");
		}
		return widget;
	}

	protected GUIBase_Button PrepareButton(GUIBase_Layout inLayout, string inName, GUIBase_Button.TouchDelegate2 inTouchDlgt, GUIBase_Button.ReleaseDelegate2 inRreleaseDlgt)
	{
		GUIBase_Button component = GetWidget(inLayout, inName).GetComponent<GUIBase_Button>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have button component");
		}
		component.RegisterTouchDelegate2(inTouchDlgt);
		component.RegisterReleaseDelegate2(inRreleaseDlgt);
		return component;
	}

	protected GUIBase_Switch PrepareSwitch(GUIBase_Layout inLayout, string inName, GUIBase_Switch.SwitchDelegate inSwitchDlgt)
	{
		GUIBase_Switch component = GetWidget(inLayout, inName).GetComponent<GUIBase_Switch>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have switch component");
		}
		component.RegisterDelegate(inSwitchDlgt);
		return component;
	}

	protected GUIBase_Label PrepareLabel(GUIBase_Layout inLayout, string inName)
	{
		GUIBase_Label component = GetWidget(inLayout, inName).GetComponent<GUIBase_Label>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have label component");
		}
		return component;
	}

	protected GUIBase_TextArea PrepareTextArea(GUIBase_Layout inLayout, string inName)
	{
		GUIBase_TextArea component = GetWidget(inLayout, inName).GetComponent<GUIBase_TextArea>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have TextArea component");
		}
		return component;
	}

	protected GUIBase_Number PrepareNumber(GUIBase_Layout inLayout, string inName)
	{
		GUIBase_Number component = GetWidget(inLayout, inName).GetComponent<GUIBase_Number>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have number component");
		}
		return component;
	}

	protected void ButtonDisable(GUIBase_Layout inLayout, string inName, bool inDisable)
	{
		GUIBase_Button component = GetWidget(inLayout, inName).GetComponent<GUIBase_Button>();
		if (component == null)
		{
			throw new MFScreenInitException("Widget [ " + inName + " } dosn't have button component");
		}
		component.SetDisabled(inDisable);
	}

	protected bool ShowKeyboard(GUIBase_Button inInput, E_KeyBoardMode inMode, KeyboardClose inCloseDelegate, string inText, int inMaxTextLength = -1)
	{
		return ShowKeyboard(inInput, inMode, inCloseDelegate, inText, string.Empty, inMaxTextLength);
	}

	protected bool ShowKeyboard(GUIBase_Button inInput, E_KeyBoardMode inMode, KeyboardClose inCloseDelegate, string inText, string inPlaceholder, int inMaxTextLength = -1)
	{
		return ShowTouchScreenKeyboard(inInput, inMode, inCloseDelegate, inText, inPlaceholder, inMaxTextLength);
	}

	private bool ShowTouchScreenKeyboard(GUIBase_Button inInput, E_KeyBoardMode inMode, KeyboardClose inCloseDelegate, string inText, string inPlaceholder, int inMaxTextLength)
	{
		if (m_ActiveInput != null || inInput == null || inCloseDelegate == null)
		{
			return false;
		}
		int num;
		switch (inMode)
		{
		case E_KeyBoardMode.Email:
			num = 7;
			break;
		case E_KeyBoardMode.Password:
			num = 0;
			break;
		default:
			num = 1;
			break;
		}
		TouchScreenKeyboardType keyboardType = (TouchScreenKeyboardType)num;
		bool autocorrection = false;
		bool secure = inMode == E_KeyBoardMode.Password;
		TouchScreenKeyboard touchScreenKeyboard = TouchScreenKeyboard.Open(inText, keyboardType, autocorrection, false, secure, false, inPlaceholder);
		if (touchScreenKeyboard == null)
		{
			return false;
		}
		m_ActiveInput = inInput;
		StartCoroutine(ProcessKeyboardInput(touchScreenKeyboard, inCloseDelegate, inText, inMaxTextLength));
		return true;
	}

	private IEnumerator ProcessKeyboardInput(TouchScreenKeyboard inKeyboard, KeyboardClose inCloseDelegate, string inText, int inMaxTextLength)
	{
		bool canceled = false;
		while (!inKeyboard.done)
		{
			if (!inKeyboard.active)
			{
				canceled = true;
				break;
			}
			if (inMaxTextLength > 0 && inKeyboard.text.Length > inMaxTextLength)
			{
				inKeyboard.text = inKeyboard.text.Substring(0, inMaxTextLength);
			}
			yield return new WaitForEndOfFrame();
		}
		string keyboardText = inKeyboard.text;
		if (canceled || inKeyboard.wasCanceled)
		{
			canceled = true;
			keyboardText = inText;
		}
		inCloseDelegate(m_ActiveInput, keyboardText, canceled);
		m_ActiveInput = null;
	}
}
