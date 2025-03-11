using System.Collections;
using UnityEngine;

public class HudPause : HudComponent
{
	private GUIBase_Button m_PauseButton;

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "HUD_Layout";

	private string s_PauseButtonName = "PauseButton";

	public override bool VisibleOnStart()
	{
		return true;
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
		}
		else
		{
			m_PauseButton = GuiBaseUtils.RegisterButtonDelegate(layout, s_PauseButtonName, null, PauseButtonDelegate);
		}
	}

	public override void Reset()
	{
	}

	public override void LateUpdate(float deltaTime)
	{
		base.LateUpdate(deltaTime);
	}

	protected override void ShowWidgets(bool on)
	{
		m_PauseButton.Widget.Show(on, true);
	}

	public override void StoreControlsOrigPositions()
	{
		base.StoreControlsOrigPositions();
		GuiOptions.PauseButton.OrigPos = new Vector2(m_PauseButton.transform.position.x, m_PauseButton.transform.position.y);
	}

	public override void UpdateControlsPosition()
	{
		base.UpdateControlsPosition();
	}

	public void StartHighlight()
	{
		m_PauseButton.StartCoroutine(HighlightObject(m_PauseButton.Widget));
	}

	public void StopHighlight()
	{
		m_PauseButton.StopAllCoroutines();
		m_PauseButton.Widget.m_FadeAlpha = 1f;
	}

	private IEnumerator HighlightObject(GUIBase_Widget sprite)
	{
		while (true)
		{
			sprite.m_FadeAlpha = 0.25f;
			yield return new WaitForSeconds(0.25f);
			sprite.m_FadeAlpha = 1f;
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void PauseButtonDelegate(bool inside)
	{
		if (inside)
		{
			GuiHUD.Instance.SwitchToIngameMenu();
		}
	}
}
