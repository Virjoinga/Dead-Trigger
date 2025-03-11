using UnityEngine;

public class GuiMissionResult
{
	private GUIBase_Pivot m_PivotMissionResult;

	private GUIBase_Layout m_LayoutMissionResult;

	private GUIBase_Label m_MissionResultText;

	private GUIBase_Label m_MissionResultText2;

	private GUIBase_Sprite m_MissionResultBackground2;

	private GUIBase_Button.TouchDelegate m_OnMissionResultDelegate;

	private int m_ArenaWave;

	private int m_ArenaScore;

	private static string s_PivotMissionResultName = "MissionResult";

	private static string s_LayoutMissionResultName = "Result";

	private static string s_MissionResultText = "Text";

	private static string s_MissionResultText2 = "Text2";

	private static string s_MissionResultBackground2 = "Background2";

	public void Init()
	{
		m_PivotMissionResult = MFGuiManager.Instance.GetPivot(s_PivotMissionResultName);
		if (!m_PivotMissionResult)
		{
			Debug.LogError("'" + s_PivotMissionResultName + "' not found!");
			return;
		}
		m_LayoutMissionResult = GuiBaseUtils.GetLayout(s_LayoutMissionResultName, m_PivotMissionResult);
		m_MissionResultText = m_LayoutMissionResult.GetWidget(s_MissionResultText).GetComponent<GUIBase_Label>();
		m_MissionResultText2 = m_LayoutMissionResult.GetWidget(s_MissionResultText2).GetComponent<GUIBase_Label>();
		m_MissionResultBackground2 = m_LayoutMissionResult.GetWidget(s_MissionResultBackground2).GetComponent<GUIBase_Sprite>();
	}

	public void ShowMissionResult(Mission.E_MissionResult result, GUIBase_Button.TouchDelegate touchDelegate)
	{
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			m_MissionResultText.SetNewText(3000025);
			string text = TextDatabase.instance[3000026];
			text = text.Replace("%d", m_ArenaWave.ToString());
			text = text.Replace("%f", m_ArenaScore.ToString());
			m_MissionResultText2.SetNewText(text);
			m_MissionResultText2.Widget.Show(true, true);
			m_MissionResultBackground2.Widget.Show(true, true);
		}
		else if (result == Mission.E_MissionResult.Success)
		{
			m_MissionResultText.SetNewText(3000010);
			m_MissionResultText2.Widget.Show(false, true);
		}
		else
		{
			m_MissionResultText2.Widget.Show(true, true);
			m_MissionResultBackground2.Widget.Show(true, true);
			m_MissionResultText.SetNewText(3000020);
			switch (result)
			{
			case Mission.E_MissionResult.DoorDestroyed:
				m_MissionResultText2.SetNewText(3000024);
				break;
			case Mission.E_MissionResult.PlayerKilled:
				m_MissionResultText2.SetNewText(3000025);
				break;
			}
		}
		MFGuiManager.Instance.ShowLayout(m_LayoutMissionResult, true);
		Screen.lockCursor = false;
		m_OnMissionResultDelegate = touchDelegate;
	}

	public void HideMissionResult()
	{
		MFGuiManager.Instance.ShowLayout(m_LayoutMissionResult, false);
		Screen.lockCursor = true;
		m_OnMissionResultDelegate = null;
	}

	public void SetArenaScore(int wave, int score)
	{
		m_ArenaWave = wave;
		m_ArenaScore = score;
	}

	private void OnMissionSuccessTouch()
	{
		if (m_OnMissionResultDelegate != null)
		{
			m_OnMissionResultDelegate();
			Screen.lockCursor = true;
			m_OnMissionResultDelegate = null;
		}
		else
		{
			Debug.LogError("No delegate for Mission success!");
		}
	}
}
