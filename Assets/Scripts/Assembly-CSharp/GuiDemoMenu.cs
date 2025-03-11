using UnityEngine;

public class GuiDemoMenu : MonoBehaviour
{
	public static GuiDemoMenu Instance;

	private bool m_IsInitialized;

	private bool m_Shown;

	private GUIBase_Pivot m_PivotMain;

	private GUIBase_Pivot m_PivotLoading;

	private GUIBase_Layout m_LayoutPlay;

	private float TimeSinceStartup;

	private string m_Level = string.Empty;

	private void Awake()
	{
		Instance = this;
		m_IsInitialized = false;
	}

	private void LateUpdate()
	{
		if (TimeSinceStartup == 0f)
		{
			TimeSinceStartup = Time.timeSinceLevelLoad;
		}
		if (!m_IsInitialized && Time.timeSinceLevelLoad - TimeSinceStartup > 0.2f)
		{
			InitGui();
			m_IsInitialized = true;
			if (!m_Shown)
			{
				MFGuiManager.Instance.ShowPivot(m_PivotMain, true);
				m_Shown = true;
			}
		}
	}

	private void InitGui()
	{
		m_PivotMain = MFGuiManager.Instance.GetPivot("MenuPivot");
		m_PivotLoading = MFGuiManager.Instance.GetPivot("Loading");
		m_LayoutPlay = GuiBaseUtils.GetLayout("Play_Layout", m_PivotMain);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel1_1Button", null, OnTouchPlayLevel1_1);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel1_2Button", null, OnTouchPlayLevel1_2);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel1_3Button", null, OnTouchPlayLevel1_3);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel2_1Button", null, OnTouchPlayLevel2_1);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel2_2Button", null, OnTouchPlayLevel2_2);
		GuiBaseUtils.RegisterButtonDelegate(m_LayoutPlay, "PlayLevel2_3Button", null, OnTouchPlayLevel2_3);
	}

	private void OnTouchPlayLevel1_1(bool inside)
	{
		if (inside)
		{
			m_Level = "underpass";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.CarryResources;
			Game.Instance.MissionSubtype = "GamePlay1";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void OnTouchPlayLevel1_2(bool inside)
	{
		if (inside)
		{
			m_Level = "underpass";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.KillZombies;
			Game.Instance.MissionSubtype = "GamePlay1";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void OnTouchPlayLevel1_3(bool inside)
	{
		if (inside)
		{
			m_Level = "ulicka_01";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.KillZombies;
			Game.Instance.MissionSubtype = "GamePlay1_kill";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void OnTouchPlayLevel2_1(bool inside)
	{
		if (inside)
		{
			m_Level = "mall";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.TimeDefense;
			Game.Instance.MissionSubtype = "GamePlay2_def";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void OnTouchPlayLevel2_2(bool inside)
	{
		if (inside)
		{
			m_Level = "mall";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.CarryResources;
			Game.Instance.MissionSubtype = "GamePlay3_carry";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void OnTouchPlayLevel2_3(bool inside)
	{
		if (inside)
		{
			m_Level = "ulicka_01";
			Game.Instance.StandaloneMission = false;
			Game.Instance.MissionType = E_MissionType.CarryResources;
			Game.Instance.MissionSubtype = "GamePlay1";
			MFGuiManager.Instance.ShowPivot(m_PivotMain, false);
			MFGuiManager.Instance.ShowPivot(m_PivotLoading, true);
			Invoke("LoadDemoLevel", 0.5f);
		}
	}

	private void LoadDemoLevel()
	{
		Debug.Log("Loading demo level " + m_Level);
		Application.LoadLevel(m_Level);
	}
}
