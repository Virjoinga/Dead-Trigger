using System;
using System.Collections.Generic;
using UnityEngine;

public class PendingFriendListView : BaseListView
{
	internal class FriendLine
	{
		private const int FS_REQUEST = 2040218;

		private const int FS_PENDING = 2040219;

		private string m_FriendName;

		private GUIBase_Widget m_Line;

		private GUIBase_Label m_Name;

		private GUIBase_Label m_Status;

		private GUIBase_Button m_Accept;

		private GUIBase_Button m_Reject;

		public Vector3 spritePos
		{
			get
			{
				return m_Line.transform.position;
			}
		}

		public FriendLine(GUIBase_Widget inLine)
		{
			m_Line = inLine;
			m_Name = inLine.transform.GetChildComponent<GUIBase_Label>("01_name");
			m_Status = inLine.transform.GetChildComponent<GUIBase_Label>("02_status");
			m_Accept = inLine.transform.GetChildComponent<GUIBase_Button>("03_accept");
			m_Reject = inLine.transform.GetChildComponent<GUIBase_Button>("04_reject");
			m_Accept.RegisterTouchDelegate(Delegate_Accept);
			m_Reject.RegisterTouchDelegate(Delegate_Reject);
		}

		public void Update(FriendList.PendingFriendInfo inFriend)
		{
			m_FriendName = inFriend.m_Name;
			m_Name.SetNewText(inFriend.m_Name);
			bool isItRequest = inFriend.isItRequest;
			m_Status.SetNewText((!isItRequest) ? 2040219 : 2040218);
			m_Accept.Widget.Show(isItRequest ? true : false, true);
			m_Reject.Widget.Show(isItRequest ? true : false, true);
		}

		public void Show()
		{
			m_Line.Show(true, true);
		}

		public void Hide()
		{
			m_Line.Show(false, true);
		}

		private void Delegate_Accept()
		{
			GameCloudManager.friendList.AcceptFriendRequest(m_FriendName);
		}

		private void Delegate_Reject()
		{
			GameCloudManager.friendList.RejectFriendRequest(m_FriendName);
		}
	}

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
		GameCloudManager.friendList.PendingFriendListChanged += OnFriendListChanged;
	}

	public override void GUIView_Hide()
	{
		GameCloudManager.friendList.PendingFriendListChanged -= OnFriendListChanged;
		MFGuiManager.Instance.ShowLayout(m_View, false);
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
			m_GuiLines[i] = new FriendLine(inParent.GetWidgetOnLine(i));
		}
	}

	private void UpdateView()
	{
		List<FriendList.PendingFriendInfo> pendingFriends = GameCloudManager.friendList.pendingFriends;
		for (int i = 0; i < m_GuiLines.Length; i++)
		{
			int num = m_FirstVisibleIndex + i;
			if (num < pendingFriends.Count)
			{
				m_GuiLines[i].Show();
				m_GuiLines[i].Update(pendingFriends[num]);
			}
			else
			{
				m_GuiLines[i].Hide();
			}
		}
		m_PrevButton.Show(m_FirstVisibleIndex != 0);
		m_NextButton.Show(m_FirstVisibleIndex + m_GuiLines.Length < pendingFriends.Count);
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
		List<FriendList.PendingFriendInfo> pendingFriends = GameCloudManager.friendList.pendingFriends;
		if (m_FirstVisibleIndex % m_GuiLines.Length != 0 || m_FirstVisibleIndex >= pendingFriends.Count)
		{
			m_FirstVisibleIndex = m_GuiLines.Length * (pendingFriends.Count / m_GuiLines.Length);
			Debug.LogError("Internal error, inform alex");
		}
	}
}
