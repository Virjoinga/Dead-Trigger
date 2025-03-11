using UnityEngine;

public class GuiBaseUtils
{
	public static void RegisterButtonDelegate(GUIBase_Pivot pivot, string layoutName, string buttonName, GUIBase_Button.TouchDelegate touch, GUIBase_Button.ReleaseDelegate release)
	{
		if ((bool)pivot)
		{
			GUIBase_Layout layout = GetLayout(layoutName, pivot);
			if ((bool)layout)
			{
				RegisterButtonDelegate(layout, buttonName, touch, release);
			}
			else
			{
				Debug.LogError("Can't find layout '" + layoutName);
			}
		}
	}

	public static GUIBase_Layout GetLayout(string layoutName, GUIBase_Pivot pivot)
	{
		if ((bool)pivot)
		{
			GUIBase_Layout[] componentsInChildren = pivot.GetComponentsInChildren<GUIBase_Layout>();
			GUIBase_Layout[] array = componentsInChildren;
			foreach (GUIBase_Layout gUIBase_Layout in array)
			{
				if (gUIBase_Layout.name == layoutName)
				{
					return gUIBase_Layout;
				}
			}
		}
		Debug.LogError("Can't find layout '" + layoutName + "'");
		return null;
	}

	public static GUIBase_Button RegisterButtonDelegate(GUIBase_Layout layout, string buttonName, GUIBase_Button.TouchDelegate touch, GUIBase_Button.ReleaseDelegate release)
	{
		GUIBase_Widget widget = layout.GetWidget(buttonName);
		if ((bool)widget)
		{
			GUIBase_Button component = widget.GetComponent<GUIBase_Button>();
			if ((bool)component)
			{
				component.RegisterTouchDelegate(touch);
				component.RegisterReleaseDelegate(release);
				return component;
			}
			Debug.LogError("Widget '" + buttonName + "' is not GUIBase_Button");
		}
		else
		{
			Debug.LogError("Can't find widget '" + buttonName);
		}
		return null;
	}

	public static GUIBase_Button RegisterButtonDelegate(GUIBase_Layout layout, string buttonName, GUIBase_Button.TouchDelegate2 touch, GUIBase_Button.ReleaseDelegate2 release, GUIBase_Button.CancelDelegate2 cancel)
	{
		GUIBase_Widget widget = layout.GetWidget(buttonName);
		if ((bool)widget)
		{
			GUIBase_Button component = widget.GetComponent<GUIBase_Button>();
			if ((bool)component)
			{
				component.RegisterTouchDelegate2(touch);
				component.RegisterReleaseDelegate2(release);
				component.RegisterCancelDelegate2(cancel);
				return component;
			}
			Debug.LogError("Widget '" + buttonName + "' is not GUIBase_Button");
		}
		else
		{
			Debug.LogError("Can't find widget '" + buttonName);
		}
		return null;
	}

	public static GUIBase_Button GetButton(GUIBase_Layout layout, string buttonName)
	{
		GUIBase_Widget widget = layout.GetWidget(buttonName);
		if ((bool)widget)
		{
			return widget.GetComponent<GUIBase_Button>();
		}
		Debug.LogWarning("Can't find widget '" + buttonName);
		return null;
	}

	public static GUIBase_Switch GetSwitch(GUIBase_Layout layout, string switchName)
	{
		GUIBase_Widget widget = layout.GetWidget(switchName);
		if ((bool)widget)
		{
			return widget.GetComponent<GUIBase_Switch>();
		}
		Debug.LogWarning("Can't find widget '" + switchName);
		return null;
	}

	public static GUIBase_Slider RegisterSliderDelegate(GUIBase_Layout layout, string sliderName, GUIBase_Slider.ChangeValueDelegate d)
	{
		GUIBase_Widget widget = layout.GetWidget(sliderName);
		if ((bool)widget)
		{
			GUIBase_Slider component = widget.GetComponent<GUIBase_Slider>();
			if ((bool)component)
			{
				component.RegisterChangeValueDelegate(d);
				return component;
			}
		}
		else
		{
			Debug.LogError("Can't find widget '" + sliderName);
		}
		return null;
	}

	public static GUIBase_Switch RegisterSwitchDelegate(GUIBase_Layout layout, string switchName, GUIBase_Switch.SwitchDelegate d)
	{
		GUIBase_Widget widget = layout.GetWidget(switchName);
		if ((bool)widget)
		{
			GUIBase_Switch component = widget.GetComponent<GUIBase_Switch>();
			if ((bool)component)
			{
				component.RegisterDelegate(d);
				return component;
			}
		}
		else
		{
			Debug.LogError("Can't find widget '" + switchName);
		}
		return null;
	}

	public static GUIBase_Sprite PrepareSprite(GUIBase_Layout layout, string name)
	{
		GUIBase_Sprite result = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			result = widget.GetComponent<GUIBase_Sprite>();
		}
		else
		{
			Debug.LogWarning("Can't find widget '" + name);
		}
		return result;
	}

	public static GUIBase_Label PrepareLabel(GUIBase_Layout layout, string name)
	{
		GUIBase_Label result = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			result = widget.GetComponent<GUIBase_Label>();
		}
		else
		{
			Debug.LogWarning("Can't find widget '" + name);
		}
		return result;
	}

	public static GUIBase_TextArea PrepareTextArea(GUIBase_Layout layout, string name)
	{
		GUIBase_TextArea result = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			result = widget.GetComponent<GUIBase_TextArea>();
		}
		else
		{
			Debug.LogWarning("Can't find widget '" + name);
		}
		return result;
	}

	public static void RegisterFocusDelegate(GUIBase_Pivot pivot, string layoutName, GUIBase_Layout.FocusDelegate d)
	{
		if ((bool)pivot)
		{
			GUIBase_Layout layout = GetLayout(layoutName, pivot);
			if ((bool)layout)
			{
				RegisterFocusDelegate(layout, d);
			}
			else
			{
				Debug.LogError("Can't find layout '" + layoutName);
			}
		}
	}

	public static void RegisterFocusDelegate(GUIBase_Layout layout, GUIBase_Layout.FocusDelegate d)
	{
		if ((bool)layout)
		{
			layout.RegisterFocusDelegate(d);
		}
	}

	public static GUIBase_Number PrepareNumber(GUIBase_Layout layout, string name)
	{
		GUIBase_Number result = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			result = widget.GetComponent<GUIBase_Number>();
		}
		else
		{
			Debug.LogError("Can't find widget '" + name);
		}
		return result;
	}

	public static GUIBase_ProgressBar PrepareProgressBar(GUIBase_Layout layout, string name)
	{
		GUIBase_ProgressBar result = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			result = widget.GetComponent<GUIBase_ProgressBar>();
		}
		else
		{
			Debug.LogError("Can't find widget '" + name);
		}
		return result;
	}

	public static GUIBase_Enum PrepareEnum(GUIBase_Layout layout, string name, GUIBase_Enum.ChangeValueDelegate d)
	{
		GUIBase_Enum gUIBase_Enum = null;
		GUIBase_Widget widget = layout.GetWidget(name);
		if ((bool)widget)
		{
			gUIBase_Enum = widget.GetComponent<GUIBase_Enum>();
			gUIBase_Enum.RegisterDelegate(d);
		}
		else
		{
			Debug.LogError("Can't find widget '" + name);
		}
		return gUIBase_Enum;
	}

	public static GUIBase_Label GetChildLabel(GUIBase_Widget w, string name)
	{
		GUIBase_Label[] componentsInChildren = w.GetComponentsInChildren<GUIBase_Label>();
		GUIBase_Label[] array = componentsInChildren;
		foreach (GUIBase_Label gUIBase_Label in array)
		{
			if (gUIBase_Label.name == name)
			{
				return gUIBase_Label;
			}
		}
		Debug.LogWarning("Can't find widget '" + name);
		return null;
	}

	public static GUIBase_Sprite GetChildSprite(GUIBase_Widget w, string name)
	{
		GUIBase_Sprite[] componentsInChildren = w.GetComponentsInChildren<GUIBase_Sprite>();
		GUIBase_Sprite[] array = componentsInChildren;
		foreach (GUIBase_Sprite gUIBase_Sprite in array)
		{
			if (gUIBase_Sprite.name == name)
			{
				return gUIBase_Sprite;
			}
		}
		Debug.LogWarning("Can't find widget '" + name);
		return null;
	}

	public static GUIBase_Counter GetChildCounter(GUIBase_Widget w, string name)
	{
		GUIBase_Counter[] componentsInChildren = w.GetComponentsInChildren<GUIBase_Counter>();
		GUIBase_Counter[] array = componentsInChildren;
		foreach (GUIBase_Counter gUIBase_Counter in array)
		{
			if (gUIBase_Counter.name == name)
			{
				return gUIBase_Counter;
			}
		}
		Debug.LogWarning("Can't find widget '" + name);
		return null;
	}

	public static GUIBase_Button GetChildButton(GUIBase_Widget w, string name)
	{
		GUIBase_Button[] componentsInChildren = w.GetComponentsInChildren<GUIBase_Button>();
		GUIBase_Button[] array = componentsInChildren;
		foreach (GUIBase_Button gUIBase_Button in array)
		{
			if (gUIBase_Button.name == name)
			{
				return gUIBase_Button;
			}
		}
		Debug.LogWarning("Can't find widget '" + name);
		return null;
	}

	public static GUIBase_Number GetChildNumber(GUIBase_Button btn, string name)
	{
		Transform transform = btn.transform.Find(name);
		if (transform == null)
		{
			Debug.LogWarning("Can't find widget '" + name);
		}
		return (!(transform != null)) ? null : transform.GetComponent<GUIBase_Number>();
	}

	public static GUIBase_Number GetChildNumber(GUIBase_Widget w, string name)
	{
		GUIBase_Number[] componentsInChildren = w.GetComponentsInChildren<GUIBase_Number>();
		GUIBase_Number[] array = componentsInChildren;
		foreach (GUIBase_Number gUIBase_Number in array)
		{
			if (gUIBase_Number.name == name)
			{
				return gUIBase_Number;
			}
		}
		Debug.LogWarning("Can't find widget '" + name);
		return null;
	}

	public static void ShowPivotWidgets(GUIBase_Pivot p, bool show)
	{
		p.Show(show);
		GUIBase_Widget[] componentsInChildren = p.GetComponentsInChildren<GUIBase_Widget>();
		GUIBase_Widget[] array = componentsInChildren;
		foreach (GUIBase_Widget gUIBase_Widget in array)
		{
			gUIBase_Widget.Show(show, true);
		}
	}
}
