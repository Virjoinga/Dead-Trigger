using UnityEngine;

[AddComponentMenu("GUI/Menu/HelpMenu")]
public class GuiHelpMenu : MonoBehaviour
{
	public static GuiHelpMenu Instance;

	private GUIBase_Pivot m_HelpPivot;

	private GUIBase_Layout m_HelpLayout;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
	}

	public void Init()
	{
		m_HelpPivot = MFGuiManager.Instance.GetPivot("MainHelpFirst");
		if (!m_HelpPivot)
		{
			Debug.LogError("'MainHelpFirst' not found!");
			return;
		}
		m_HelpLayout = GuiBaseUtils.GetLayout("01Buttons_Layout", m_HelpPivot);
		GuiBaseUtils.RegisterButtonDelegate(m_HelpLayout, "Back_Button", null, OnBackButtonDelegate);
	}

	public void Show()
	{
		Debug.Log("Show help");
		MFGuiManager.Instance.ShowLayout(m_HelpLayout, true);
	}

	public void Hide()
	{
		Debug.Log("Hidehelp");
		MFGuiManager.Instance.ShowLayout(m_HelpLayout, false);
	}

	private void OnBackButtonDelegate(bool inside)
	{
		if (inside)
		{
			GuiHUD.Instance.SwitchBackFromFirstHelp();
		}
	}

	private void LateUpdate()
	{
		if (m_HelpLayout != null && m_HelpLayout.IsVisible() && m_HelpPivot != null && m_HelpPivot.IsControlEnabled() && Input.GetKeyDown(KeyCode.Escape))
		{
			OnBackButtonDelegate(true);
		}
	}
}
