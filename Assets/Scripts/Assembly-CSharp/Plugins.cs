using System.Collections;
using UnityEngine;

public class Plugins : MonoBehaviour
{
	private static Plugins m_Instance;

	public static void Initialize(bool hasSpentRealMoney)
	{
		if (!(m_Instance != null))
		{
			GameObject gameObject = new GameObject("MFPlugins");
			Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<Plugins>();
			m_Instance.StartCoroutine(m_Instance.Initialize_Coroutine(hasSpentRealMoney));
		}
	}

	private IEnumerator Initialize_Coroutine(bool hasSpentRealMoney)
	{
		GetAppSettings settings = new GetAppSettings();
		GameCloudManager.AddAction(settings);
		while (!settings.isDone)
		{
			yield return null;
		}
		if (!hasSpentRealMoney)
		{
			//Advertisement.Initialize(settings.chartboostWeightAndroid, settings.appLovinWeightAndroid, settings.adsDelayInSeconds);
		}
		if (settings.useUnityAnalyticsAndroid)
		{
			//UnityAnalyticsWrapper.Initialize();
		}
	}
}
