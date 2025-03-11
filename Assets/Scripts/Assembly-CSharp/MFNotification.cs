using System.Collections.Generic;

public class MFNotification
{
	public enum Source
	{
		LOCAL = 0,
		REMOTE = 1
	}

	public class ExtendedStyle
	{
	}

	public class BigTextStyle : ExtendedStyle
	{
		public string BigText { get; set; }

		public string BigTitle { get; set; }

		public string Summary { get; set; }

		public BigTextStyle(string bigText, string bigTitle = "", string summary = "")
		{
			BigText = bigText;
			BigTitle = bigTitle;
			Summary = summary;
		}

		public BigTextStyle()
		{
		}
	}

	public class InboxStyle : ExtendedStyle
	{
		public List<string> Lines { get; private set; }

		public string InboxTitle { get; set; }

		public string Summary { get; set; }

		public InboxStyle(string[] lines, string inboxTitle = "", string summary = "")
		{
			Lines = new List<string>(lines);
			InboxTitle = inboxTitle;
			Summary = summary;
		}

		public InboxStyle()
		{
		}
	}

	public string Title { get; set; }

	public string Text { get; set; }

	public string Icon { get; set; }

	public ExtendedStyle Style { get; set; }

	public int Counter { get; set; }

	public string Sound { get; set; }

	public Source Origin { get; internal set; }

	public int Id { get; internal set; }

	public MFNotification(string title, string text, string icon, string sound = "", int counter = 0, ExtendedStyle style = null)
	{
		Title = title;
		Text = text;
		Icon = icon;
		Counter = counter;
		Style = style;
		Sound = sound;
		Origin = Source.LOCAL;
		Id = -1;
	}

	public MFNotification()
	{
	}
}
