public static class AppLovinWrapper
{
	private static bool m_IsAvailable;

	public static bool IsAvailable()
	{
		return m_IsAvailable;
	}

	public static void Initialize(bool isEnabled)
	{
		m_IsAvailable = isEnabled;
		if (m_IsAvailable)
		{
			AppLovin.InitializeSdk();
		}
	}

	public static bool HasPreloadedInterstitial()
	{
		if (m_IsAvailable)
		{
			return AppLovin.HasPreloadedInterstitial();
		}
		return false;
	}

	public static void PreloadInterstitial()
	{
		if (m_IsAvailable)
		{
			AppLovin.PreloadInterstitial();
		}
	}

	public static void ShowInterstitial()
	{
		if (m_IsAvailable)
		{
			AppLovin.ShowInterstitial();
		}
	}
}
