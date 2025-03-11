using System;
using UnityEngine;

public class BackupView : BaseListView
{
	internal class SaveInfoLine
	{
		private Color HIGHLIGHT_COLOR = new Color(2f / 3f, 40f / 51f, 0.11764706f);

		private Color DEFAULT_COLOR = Color.white;

		private GUIBase_Widget m_Line;

		private GUIBase_Label m_Name;

		private GUIBase_Label m_LocalValue;

		private GUIBase_Label m_CloudValue;

		public Vector3 spritePos
		{
			get
			{
				return m_Line.transform.position;
			}
		}

		public SaveInfoLine(GUIBase_Widget inLine)
		{
			m_Line = inLine;
			m_Name = inLine.transform.GetChildComponent<GUIBase_Label>("01_name");
			m_LocalValue = inLine.transform.GetChildComponent<GUIBase_Label>("02_local");
			m_CloudValue = inLine.transform.GetChildComponent<GUIBase_Label>("03_cloud");
		}

		public void Update(int inTextID, int inVal1, int inVal2, bool inTime = false)
		{
			if (!inTime)
			{
				Update(inTextID, inVal1.ToString(), inVal2.ToString(), inVal1 - inVal2);
				return;
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(inVal1);
			string text = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
			timeSpan = TimeSpan.FromSeconds(inVal2);
			string text2 = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
			Update(inTextID, text.ToString(), text2.ToString(), inVal1 - inVal2);
		}

		public void Update(int inTextID, int inVal1, string inVal2, bool inTime = false)
		{
			if (!inTime)
			{
				Update(inTextID, inVal1.ToString(), inVal2, 0);
				return;
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(inVal1);
			string text = ((int)timeSpan.TotalHours).ToString("00") + ":" + timeSpan.Minutes.ToString("00");
			Update(inTextID, text.ToString(), inVal2, 0);
		}

		public void Show()
		{
			m_Line.Show(true, true);
		}

		public void Hide()
		{
			m_Line.Show(false, true);
		}

		private void Update(int inTextID, string inVal1, string inVal2, int inHighlight)
		{
			m_Name.SetNewText(inTextID);
			m_LocalValue.SetNewText(inVal1);
			m_CloudValue.SetNewText(inVal2);
			m_LocalValue.Widget.m_Color = ((inHighlight <= 0) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
			m_CloudValue.Widget.m_Color = ((inHighlight >= 0) ? DEFAULT_COLOR : HIGHLIGHT_COLOR);
		}
	}

	public delegate int IntExtractor(PlayerPersistentInfoData inData);

	private const int TEXT_ID_TOTAL_TIME = 2040321;

	private const int TEXT_ID_MISSIONS_PLAYED = 2040315;

	private const int TEXT_ID_EXPERIENCE = 2040311;

	private const int TEXT_ID_GOLD = 2040318;

	private const int TEXT_ID_MONEY = 2040312;

	private SaveInfoLine[] m_GuiLines;

	private GUIBase_Layout m_View;

	private bool isUpdateNeccesary { get; set; }

	public bool retrivingInfoFromCloud { get; set; }

	public void GUIView_Init(GUIBase_Layout inView, GUIBase_List inList, GUIBase_Button inPrev, GUIBase_Button inNext)
	{
		m_View = inView;
		InitGuiLines(inList);
	}

	public override void GUIView_Show()
	{
		isUpdateNeccesary = true;
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

	public void ForceUpdateView()
	{
		isUpdateNeccesary = true;
	}

	private void InitGuiLines(GUIBase_List inParent)
	{
		if (inParent.numOfLines <= 0)
		{
			Debug.LogError("inParent.numOfLines = " + inParent.numOfLines);
			return;
		}
		m_GuiLines = new SaveInfoLine[inParent.numOfLines];
		for (int i = 0; i < inParent.numOfLines; i++)
		{
			m_GuiLines[i] = new SaveInfoLine(inParent.GetWidgetOnLine(i));
		}
	}

	private void UpdateView()
	{
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		PlayerPersistantInfo cloudPPI = GameCloudManager.cloudPPI;
		UpdateGUILine(0, 2040321, playerPersistentInfo, cloudPPI, (PlayerPersistentInfoData val) => (int)val.Params.GameTime, retrivingInfoFromCloud, true);
		UpdateGUILine(1, 2040315, playerPersistentInfo, cloudPPI, (PlayerPersistentInfoData val) => val.Params.MissionCount, retrivingInfoFromCloud);
		UpdateGUILine(2, 2040311, playerPersistentInfo, cloudPPI, (PlayerPersistentInfoData val) => val.Params.Experience, retrivingInfoFromCloud);
		UpdateGUILine(3, 2040318, playerPersistentInfo, cloudPPI, (PlayerPersistentInfoData val) => val.Params.TotalGold, retrivingInfoFromCloud);
		UpdateGUILine(4, 2040312, playerPersistentInfo, cloudPPI, (PlayerPersistentInfoData val) => val.Params.TotalMoney, retrivingInfoFromCloud);
	}

	private void OnFriendListChanged(object sender, EventArgs e)
	{
		ForceUpdateView();
	}

	private void UpdateGUILine(int inLineIndex, int inTextID, PlayerPersistantInfo inLocalPPI, PlayerPersistantInfo inCloudPPI, IntExtractor inExtractor, bool inSyncInProgress, bool inTime = false)
	{
		if (m_GuiLines[inLineIndex] != null)
		{
			m_GuiLines[inLineIndex].Show();
			if (inCloudPPI != null)
			{
				int inVal = inExtractor(inLocalPPI.GetPlayerData_ForStatistics());
				int inVal2 = inExtractor(inCloudPPI.GetPlayerData_ForStatistics());
				m_GuiLines[inLineIndex].Update(inTextID, inVal, inVal2, inTime);
			}
			else
			{
				string inVal3 = TextDatabase.instance[(!inSyncInProgress) ? 2040613 : 2040612];
				int inVal4 = inExtractor(inLocalPPI.GetPlayerData_ForStatistics());
				m_GuiLines[inLineIndex].Update(inTextID, inVal4, inVal3, inTime);
			}
		}
	}
}
