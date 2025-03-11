using System;
using UnityEngine;

public class StatisticsView : BaseListView
{
	internal class StatisticsLine
	{
		private Color HIGHLIGHT_COLOR = new Color(2f / 3f, 40f / 51f, 0.11764706f);

		private Color DEFAULT_COLOR = Color.white;

		private GUIBase_Widget m_Line;

		private GUIBase_Label m_Name;

		private GUIBase_Label m_PlayerValue;

		private GUIBase_Label m_FriendValue;

		private GUIBase_Label m_FriendName;

		public Vector3 spritePos
		{
			get
			{
				return m_Line.transform.position;
			}
		}

		public StatisticsLine(GUIBase_Widget inLine)
		{
			m_Line = inLine;
			m_Name = inLine.transform.GetChildComponent<GUIBase_Label>("01_name");
			m_PlayerValue = inLine.transform.GetChildComponent<GUIBase_Label>("02_player_value");
			m_FriendValue = inLine.transform.GetChildComponent<GUIBase_Label>("03_friend_value");
			m_FriendName = inLine.transform.GetChildComponent<GUIBase_Label>("04_friend_name");
		}

		public void Update(Statistics.Item inItem, Statistics.E_Mode inStatisticMode)
		{
			bool flag = inStatisticMode == Statistics.E_Mode.CompareWithFriend;
			bool flag2 = inStatisticMode == Statistics.E_Mode.CompareWithBest;
			m_FriendName.Widget.Show(flag2, true);
			m_FriendValue.Widget.Show(flag || flag2, true);
			if (inItem is Statistics.IntItem)
			{
				Update(inItem as Statistics.IntItem, inStatisticMode);
			}
			else if (inItem is Statistics.FloatItem)
			{
				Update(inItem as Statistics.FloatItem, inStatisticMode);
			}
			else if (inItem is Statistics.StringItem)
			{
				Update(inItem as Statistics.StringItem, inStatisticMode);
			}
			else
			{
				Debug.LogWarning("Unknown Statistics item type" + inItem.GetType().Name);
			}
		}

		public void Show()
		{
			m_Line.Show(true, true);
		}

		public void Hide()
		{
			m_Line.Show(false, true);
		}

		private void Update(Statistics.IntItem inItem, Statistics.E_Mode inStatisticMode)
		{
			if (inItem.m_NameIndex == 0 && !string.IsNullOrEmpty(inItem.m_NameText))
			{
				UpdateLine(inItem.m_NameText, inItem.m_PlayerValue.ToString(), inItem.m_SecondValue.ToString(), inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
			else
			{
				UpdateLine(inItem.m_NameIndex, inItem.m_PlayerValue.ToString(), inItem.m_SecondValue.ToString(), inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
		}

		private void Update(Statistics.FloatItem inItem, Statistics.E_Mode inStatisticMode)
		{
			if (inItem.m_NameIndex == 0 && !string.IsNullOrEmpty(inItem.m_NameText))
			{
				UpdateLine(inItem.m_NameText, inItem.m_PlayerValue.ToString(), inItem.m_SecondValue.ToString(), inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
			else
			{
				UpdateLine(inItem.m_NameIndex, inItem.m_PlayerValue.ToString(), inItem.m_SecondValue.ToString(), inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
		}

		private void Update(Statistics.StringItem inItem, Statistics.E_Mode inStatisticMode)
		{
			if (inItem.m_NameIndex == 0 && !string.IsNullOrEmpty(inItem.m_NameText))
			{
				UpdateLine(inItem.m_NameText, inItem.m_PlayerValue, inItem.m_SecondValue, inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
			else
			{
				UpdateLine(inItem.m_NameIndex, inItem.m_PlayerValue, inItem.m_SecondValue, inItem.m_SecondValueFriendName, inItem.m_HighlightPlayer, inItem.m_HighlightFriend);
			}
		}

		private void UpdateLine(string inText, string inVal1, string inVal2, string inVal3, bool inHighlightPlayer, bool inHighlightFriend)
		{
			m_Name.SetNewText(inText);
			m_PlayerValue.SetNewText(inVal1);
			m_FriendValue.SetNewText(inVal2);
			m_FriendName.SetNewText(inVal3);
			m_PlayerValue.Widget.m_Color = ((!inHighlightPlayer) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
			m_FriendValue.Widget.m_Color = ((!inHighlightFriend) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
		}

		private void UpdateLine(int inTextID, string inVal1, string inVal2, string inVal3, bool inHighlightPlayer, bool inHighlightFriend)
		{
			m_Name.SetNewText(inTextID);
			m_PlayerValue.SetNewText(inVal1);
			m_FriendValue.SetNewText(inVal2);
			m_FriendName.SetNewText(inVal3);
			m_PlayerValue.Widget.m_Color = ((!inHighlightPlayer) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
			m_FriendValue.Widget.m_Color = ((!inHighlightFriend) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
		}
	}

	private StatisticsLine[] m_GuiLines;

	private GUIBase_Button m_PrevButton;

	private GUIBase_Button m_NextButton;

	private GUIBase_Layout m_View;

	private Statistics m_Statistics = new Statistics();

	private Statistics.E_Mode m_Mode = Statistics.E_Mode.Player;

	private string m_FriendName = string.Empty;

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
		m_Statistics.Clear();
		isUpdateNeccesary = true;
		m_PrevButton.RegisterReleaseDelegate2(Delegate_Prev);
		m_NextButton.RegisterReleaseDelegate2(Delegate_Next);
		MFGuiManager.Instance.ShowLayout(m_View, true);
	}

	public override void GUIView_Hide()
	{
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

	public void SetStatisticsMode(Statistics.E_Mode inMode, string inFriendName)
	{
		isUpdateNeccesary |= inMode != m_Mode;
		isUpdateNeccesary |= inFriendName != m_FriendName;
		m_Mode = inMode;
		m_FriendName = inFriendName;
	}

	private void InitGuiLines(GUIBase_List inParent)
	{
		if (inParent.numOfLines <= 0)
		{
			Debug.LogError("inParent.numOfLines = " + inParent.numOfLines);
			return;
		}
		m_GuiLines = new StatisticsLine[inParent.numOfLines];
		for (int i = 0; i < inParent.numOfLines; i++)
		{
			m_GuiLines[i] = new StatisticsLine(inParent.GetWidgetOnLine(i));
		}
	}

	private void UpdateView()
	{
		m_Statistics.PrepareFor(m_Mode, m_FriendName);
		for (int i = 0; i < m_GuiLines.Length; i++)
		{
			int num = m_FirstVisibleIndex + i;
			if (num < m_Statistics.Count)
			{
				m_GuiLines[i].Show();
				m_GuiLines[i].Update(m_Statistics.GetItem(num), m_Statistics.Mode);
			}
			else
			{
				m_GuiLines[i].Hide();
			}
		}
		m_PrevButton.Show(m_FirstVisibleIndex != 0);
		m_NextButton.Show(m_FirstVisibleIndex + m_GuiLines.Length < m_Statistics.Count);
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
		if (m_FirstVisibleIndex % m_GuiLines.Length != 0 || m_FirstVisibleIndex >= m_Statistics.Count)
		{
			m_FirstVisibleIndex = m_GuiLines.Length * (m_Statistics.Count / m_GuiLines.Length);
			Debug.LogError("Internal error, inform alex");
		}
	}
}
