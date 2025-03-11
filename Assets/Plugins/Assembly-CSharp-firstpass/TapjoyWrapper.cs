public class TapjoyWrapper
{
	private static bool m_RewardedVideoLoaded;

	public static void GetFullScreenAd()
	{
		m_RewardedVideoLoaded = false;
		TapjoyPlugin.GetFullScreenAdWithCurrencyID("4c40cc63-5475-45e3-80a0-945624ba9ead");
	}

	public static void ShowFullScreenAd()
	{
		TapjoyPlugin.ShowFullScreenAd();
	}

	public static bool IsFullScreenAdLoaded()
	{
		return m_RewardedVideoLoaded;
	}

	public static void SetFullScreenAdLoaded()
	{
		m_RewardedVideoLoaded = true;
	}
}
