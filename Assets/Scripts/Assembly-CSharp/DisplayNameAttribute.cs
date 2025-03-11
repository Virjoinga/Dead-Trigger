using System;

[AttributeUsage(AttributeTargets.Class)]
public class DisplayNameAttribute : Attribute
{
	public string DisplayName { get; private set; }

	public string ToolTip { get; private set; }

	public DisplayNameAttribute(string DisplayName)
	{
		this.DisplayName = DisplayName;
		ToolTip = string.Empty;
	}

	public DisplayNameAttribute(string DisplayName, string ToolTip)
	{
		this.DisplayName = DisplayName;
		this.ToolTip = ToolTip;
	}
}
