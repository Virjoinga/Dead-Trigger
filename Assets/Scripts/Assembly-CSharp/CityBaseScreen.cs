public class CityBaseScreen
{
	private GUIBase_Button.TouchDelegate m_CloseDelegate;

	public CityBaseScreen(CityScreen screen)
	{
	}

	public virtual void Show(GUIBase_Button.TouchDelegate inCloseDelegate = null)
	{
		m_CloseDelegate = inCloseDelegate;
		CityManager.Instance.ScreenTracker.ScreenActivated(this);
	}

	public virtual void Hide()
	{
		m_CloseDelegate = null;
		CityManager.Instance.ScreenTracker.ScreenDeactivated(this);
	}

	public virtual void ProcessBackButton()
	{
		if (m_CloseDelegate != null)
		{
			m_CloseDelegate();
		}
	}

	public GUIBase_Button.TouchDelegate GetCloseDelegate()
	{
		return m_CloseDelegate;
	}
}
