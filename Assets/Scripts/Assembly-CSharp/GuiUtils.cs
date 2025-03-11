internal static class GuiUtils
{
	public static GUIBase_Label PrepareLabel(GUIBase_Layout Layout, string Name, string Text)
	{
		GUIBase_Label component = Layout.GetWidget(Name).GetComponent<GUIBase_Label>();
		if (component != null)
		{
			component.SetNewText(Text);
		}
		return component;
	}

	public static GUIBase_Button PrepareButton(GUIBase_Layout Layout, string Name, GUIBase_Button.TouchDelegate2 Touched, GUIBase_Button.ReleaseDelegate2 Released)
	{
		GUIBase_Button component = Layout.GetWidget(Name).GetComponent<GUIBase_Button>();
		if (component != null)
		{
			component.RegisterTouchDelegate2(Touched);
			component.RegisterReleaseDelegate2(Released);
		}
		return component;
	}

	public static GUIBase_TextArea PrepareTextArea(GUIBase_Layout Layout, string Name, string Text)
	{
		GUIBase_TextArea component = Layout.GetWidget(Name).GetComponent<GUIBase_TextArea>();
		if (component != null)
		{
			component.SetNewText(Text);
		}
		return component;
	}
}
