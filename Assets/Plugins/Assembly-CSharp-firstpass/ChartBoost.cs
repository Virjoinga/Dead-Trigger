using System;
using ChartboostSDK;
using UnityEngine;

public class ChartBoost : MonoBehaviour
{
	private static ChartBoost m_Instance;

	private static bool m_IsEnabled;

	public static Action<bool> EnableInput;

	private static ChartBoost Instance
	{
		get
		{
			if (m_Instance == null)
			{
				SpawnInstance();
			}
			return m_Instance;
		}
	}

	public static bool IsEnabled
	{
		get
		{
			return m_IsEnabled;
		}
		private set
		{
			m_IsEnabled = value;
		}
	}

	private static void SpawnInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject(typeof(ChartBoost).Name);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<ChartBoost>();
			gameObject.AddComponent<Chartboost>();
		}
	}

	public static bool IsAvailable()
	{
		return true;
	}

	public static void Init(bool isChartboostEnabled)
	{
		IsEnabled = isChartboostEnabled;
		if (IsEnabled)
		{
			SpawnInstance();
			Chartboost.didDisplayInterstitial += CBDisableInput;
			Chartboost.didDisplayMoreApps += CBDisableInput;
			Chartboost.didDismissInterstitial += CBEnableInput;
			Chartboost.didDismissMoreApps += CBEnableInput;
		}
	}

	private static void CBEnableInput(CBLocation location)
	{
		if (EnableInput != null)
		{
			EnableInput(true);
		}
	}

	private static void CBDisableInput(CBLocation location)
	{
		if (EnableInput != null)
		{
			EnableInput(false);
		}
	}

	public static void CacheInterstitial(string location)
	{
		if (IsEnabled)
		{
			Chartboost.cacheInterstitial(CBLocation.locationFromName(location));
		}
	}

	public static bool HasCachedInterstitial(string location)
	{
		if (!IsEnabled)
		{
			return false;
		}
		return Chartboost.hasInterstitial(CBLocation.locationFromName(location));
	}

	public static void ShowInterstitial(string location)
	{
		if (IsEnabled)
		{
			Chartboost.showInterstitial(CBLocation.locationFromName(location));
		}
	}

	public static void CacheMoreApps()
	{
		if (IsEnabled)
		{
			Chartboost.cacheMoreApps(CBLocation.Default);
		}
	}

	public static void ShowMoreApps()
	{
		if (IsEnabled)
		{
			Chartboost.showMoreApps(CBLocation.Default);
			Chartboost.cacheMoreApps(CBLocation.Default);
		}
	}
}
