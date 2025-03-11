using System;
using System.Collections.Generic;
using UnityEngine;

public class FriendListView : BaseListView
{
	internal class FriendLine
	{
		private const int LOI_UNKNOWN = 2040221;

		private const int LOI_BEFORE_X_MINUTES = 2040222;

		private const int LOI_BEFORE_X_HOURS = 2040223;

		private const int LOI_YESTERDAY = 2040224;

		private const int LOI_X_DAYS_AGO = 2040225;

		private OnFriendSelect m_OnFriendSelectDelegate;

		private string m_FriendName;

		private GUIBase_Widget m_Line;

		private GUIBase_Label m_Name;

		private GUIBase_Label m_Level;

		private GUIBase_Label m_Missions;

		private GUIBase_Label m_LastOnline;

		public Vector3 spritePos
		{
			get
			{
				return m_Line.transform.position;
			}
		}

		public FriendLine(GUIBase_Widget inLine, OnFriendSelect inFriendSelect)
			: this(inLine)
		{
			m_OnFriendSelectDelegate = inFriendSelect;
			GUIBase_Button component = inLine.GetComponent<GUIBase_Button>();
			if (component != null)
			{
				component.RegisterTouchDelegate2(Delegate_OnFriendSelect);
			}
		}

		public FriendLine(GUIBase_Widget inLine)
		{
			m_Line = inLine;
			m_Name = inLine.transform.GetChildComponent<GUIBase_Label>("01_name");
			m_Level = inLine.transform.GetChildComponent<GUIBase_Label>("02_level");
			m_Missions = inLine.transform.GetChildComponent<GUIBase_Label>("03_missions");
			m_LastOnline = inLine.transform.GetChildComponent<GUIBase_Label>("04_last_online");
		}

		public void Update(FriendList.FriendInfo inFriend)
		{
			m_FriendName = inFriend.m_Name;
			m_Name.SetNewText(inFriend.m_Name);
			m_Level.SetNewText((inFriend.Level >= 0) ? inFriend.Level.ToString() : "?");
			m_Missions.SetNewText((inFriend.Missions >= 0) ? inFriend.Missions.ToString() : "?");
			string lastOnlineInfo = GetLastOnlineInfo(inFriend);
			m_LastOnline.SetNewText(lastOnlineInfo);
		}

		public void Show()
		{
			m_Line.Show(true, true);
		}

		public void Hide()
		{
			m_Line.Show(false, true);
		}

		private string GetLastOnlineInfo(FriendList.FriendInfo inFriend)
		{
			return TextDatabase.instance[2040221];
		}

		private void Delegate_OnFriendSelect(GUIBase_Widget inInstigator)
		{
			if (m_OnFriendSelectDelegate != null)
			{
				m_OnFriendSelectDelegate(m_FriendName);
			}
		}
	}

	public delegate void OnFriendSelect(string inFriendName);

	public OnFriendSelect m_OnFriendSelectDelegate;

	private FriendLine[] m_GuiLines;

	private GUIBase_Button m_PrevButton;

	private GUIBase_Button m_NextButton;

	private GUIBase_Layout m_View;

	private int m_FirstVisibleIndex;

	private bool isUpdateNeccesary { get; set; }

	public void GUIView_Init(GUIBase_Layout inView, GUIBase_List inList, GUIBase_Button inPrev, GUIBase_Button inNext)
	{
		m_View = inView;
		m_PrevButton = inPrev;
		m_NextButton = inNext;
		InitGuiLines(inList);
	}

	public override void GUIView_Show()
	{
		isUpdateNeccesary = true;
		m_PrevButton.RegisterReleaseDelegate2(Delegate_Prev);
		m_NextButton.RegisterReleaseDelegate2(Delegate_Next);
		MFGuiManager.Instance.ShowLayout(m_View, true);
		GameCloudManager.friendList.FriendListChanged += OnFriendListChanged;
	}

	public override void GUIView_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_View, false);
		GameCloudManager.friendList.FriendListChanged -= OnFriendListChanged;
	}

	public override void GUIView_Update()
	{
		if (isUpdateNeccesary)
		{
			UpdateView();
			isUpdateNeccesary = false;
		}
	}

	private void InitGuiLines(GUIBase_List inParent)
	{
		if (inParent.numOfLines <= 0)
		{
			Debug.LogError("inParent.numOfLines = " + inParent.numOfLines);
			return;
		}
		m_GuiLines = new FriendLine[inParent.numOfLines];
		for (int i = 0; i < inParent.numOfLines; i++)
		{
			m_GuiLines[i] = new FriendLine(inParent.GetWidgetOnLine(i), Delegate_OnSelect);
		}
	}

	private void UpdateView()
	{
		List<FriendList.FriendInfo> friends = GameCloudManager.friendList.friends;
		for (int i = 0; i < m_GuiLines.Length; i++)
		{
			int num = m_FirstVisibleIndex + i;
			if (num < friends.Count)
			{
				m_GuiLines[i].Show();
				m_GuiLines[i].Update(friends[num]);
			}
			else
			{
				m_GuiLines[i].Hide();
			}
		}
		m_PrevButton.Show(m_FirstVisibleIndex != 0);
		m_NextButton.Show(m_FirstVisibleIndex + m_GuiLines.Length < friends.Count);
	}

	private void OnFriendListChanged(object sender, EventArgs e)
	{
		isUpdateNeccesary = true;
	}

	private void Delegate_Prev(GUIBase_Widget inInstigator)
	{
		m_FirstVisibleIndex -= m_GuiLines.Length;
		isUpdateNeccesary = true;
		if (m_FirstVisibleIndex < 0)
		{
			m_FirstVisibleIndex = 0;
			Debug.LogError("Internal error, inform alex");
		}
	}

	private void Delegate_Next(GUIBase_Widget inInstigator)
	{
		m_FirstVisibleIndex += m_GuiLines.Length;
		isUpdateNeccesary = true;
		List<FriendList.FriendInfo> friends = GameCloudManager.friendList.friends;
		if (m_FirstVisibleIndex % m_GuiLines.Length != 0 || m_FirstVisibleIndex >= friends.Count)
		{
			m_FirstVisibleIndex = m_GuiLines.Length * (friends.Count / m_GuiLines.Length);
			Debug.LogError("Internal error, inform alex");
		}
	}

	private void Delegate_OnSelect(string inFriendName)
	{
		if (m_OnFriendSelectDelegate != null)
		{
			m_OnFriendSelectDelegate(inFriendName);
		}
	}
}
