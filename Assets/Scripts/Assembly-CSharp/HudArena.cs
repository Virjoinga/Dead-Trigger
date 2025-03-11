using UnityEngine;

public class HudArena : HudComponent
{
	private GUIBase_Widget m_Arena;

	private bool m_ShowArena;

	private AnimatedNumber m_Score;

	private int m_ScoreVal;

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "HUD_Layout";

	private string s_ArenaParent = "Arena";

	public override bool VisibleOnStart()
	{
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			return true;
		}
		return false;
	}

	public override void Init()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(s_PivotMainName);
		if (!pivot)
		{
			Debug.LogError("'" + s_PivotMainName + "' not found!!! Assert should come now");
			return;
		}
		GUIBase_Layout layout = pivot.GetLayout(s_LayoutMainName);
		if (!layout)
		{
			Debug.LogError("'" + s_LayoutMainName + "' not found!!! Assert should come now");
			return;
		}
		m_Arena = layout.GetWidget(s_ArenaParent).GetComponent<GUIBase_Widget>();
		m_Score = new AnimatedNumber(GetChildByName<GUIBase_Label>(m_Arena, "Score"));
		m_ScoreVal = 0;
	}

	public override void Reset()
	{
	}

	public override void LateUpdate(float deltaTime)
	{
		base.LateUpdate(deltaTime);
		if (IsVisible())
		{
		}
	}

	protected override void ShowWidgets(bool on)
	{
		if (m_ShowArena)
		{
			m_Arena.Show(on, true);
		}
		else
		{
			m_Arena.Show(false, true);
		}
	}

	public void SetScore(int val)
	{
		m_Score.Label.StartCoroutine(CityGUIResources.AnimateNumber(m_ScoreVal, val, m_Score, 1f, string.Empty));
		m_ScoreVal = val;
	}

	public void ShowArena(bool show)
	{
		if (IsVisible())
		{
			m_Arena.Show(show, true);
		}
		m_ShowArena = show;
	}

	private T GetChildByName<T>(GUIBase_Widget widget, string name) where T : Component
	{
		Transform transform = widget.transform.Find(name);
		return (!(transform != null)) ? ((T)null) : transform.GetComponent<T>();
	}
}
