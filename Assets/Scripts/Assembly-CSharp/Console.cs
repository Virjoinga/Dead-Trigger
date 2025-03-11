using UnityEngine;

[NESEvent(new string[] { "Done" })]
[AddComponentMenu("Entities/Console")]
public class Console : MonoBehaviour
{
	public int m_Caption;

	public int m_Message;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Label m_LabelCaption;

	private GUIBase_Button m_ButtonClose;

	private NESController m_NESController;

	private void Awake()
	{
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (!(m_NESController == null))
		{
		}
	}

	private void Destroy()
	{
	}

	private void Update()
	{
	}

	[NESAction]
	public void Init()
	{
		m_ScreenPivot = MFGuiManager.Instance.GetPivot("Computer");
		if (m_ScreenPivot == null)
		{
			Debug.LogError("Console::Init() ... Screen-pivot 'Computer' not found!");
			return;
		}
		m_ScreenLayout = MFGuiManager.Instance.GetLayout("Computer_Layout");
		if (m_ScreenLayout == null)
		{
			Debug.LogError("Console::Init() ... Screen-layout 'Computer_Layout' not found!");
			return;
		}
		string text = TextDatabase.instance[m_Caption];
		string text2 = TextDatabase.instance[m_Message];
		GuiUtils.PrepareLabel(m_ScreenLayout, "Caption", text);
		GuiUtils.PrepareTextArea(m_ScreenLayout, "Description", text2);
		m_ButtonClose = GuiUtils.PrepareButton(m_ScreenLayout, "ButtonClose", null, OnButtonReleased);
		if (m_ButtonClose == null)
		{
			Debug.LogError("Console::Init() ... 'ButtonClose' not found!");
			return;
		}
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, true);
		Game.Instance.GameState = E_GameState.IngameMenu;
		AudioListener.pause = true;
		TimeManager.Instance.PauseTime();
		GuiHUD.Instance.Hide();
	}

	private void Done()
	{
		GuiHUD.Instance.Show();
		GuiUtils.PrepareButton(m_ScreenLayout, "ButtonClose", null, null);
		Game.Instance.GameState = E_GameState.Game;
		AudioListener.pause = false;
		TimeManager.Instance.UnpauseTime();
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, false);
		if (m_NESController != null)
		{
			m_NESController.SendGameEvent(this, "Done");
		}
	}

	private void OnButtonReleased(GUIBase_Widget W)
	{
		if (m_ButtonClose.Widget == W)
		{
			Done();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(base.gameObject.transform.position, "Console.tif", true);
	}
}
