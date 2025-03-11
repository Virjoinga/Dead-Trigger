using System.Collections.Generic;

public class CityScreenTracker
{
	private List<CityBaseScreen> m_ActiveScreens = new List<CityBaseScreen>();

	public bool AnyScreenActive()
	{
		return m_ActiveScreens.Count > 0;
	}

	public void ScreenActivated(CityBaseScreen screen)
	{
		m_ActiveScreens.Add(screen);
	}

	public void ScreenDeactivated(CityBaseScreen screen)
	{
		m_ActiveScreens.Remove(screen);
	}

	public void ProcessBackButton()
	{
		if (m_ActiveScreens.Count > 0 && m_ActiveScreens[m_ActiveScreens.Count - 1] != null)
		{
			m_ActiveScreens[m_ActiveScreens.Count - 1].ProcessBackButton();
		}
	}
}
