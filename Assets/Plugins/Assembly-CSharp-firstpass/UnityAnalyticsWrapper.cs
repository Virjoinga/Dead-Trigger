using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsWrapper : MonoBehaviour
{
	private static bool m_Initialized;

	public static void Initialize()
	{
		if (!m_Initialized)
		{
			m_Initialized = true;
			UnityAnalytics.StartSDK("45863072-426b-43e6-bbc3-85f8566e3ec7");
		}
	}

	public static void ReportInAppPurchase(string productID, float price, string currencyCode, string receipt, string signature = null)
	{
		if (m_Initialized)
		{
			UnityAnalytics.Transaction(productID, (decimal)price, currencyCode, receipt, signature);
		}
	}

	public static void ReportCustomEvent(string eventName, IDictionary<string, object> eventData)
	{
		if (m_Initialized)
		{
			UnityAnalytics.CustomEvent(eventName, eventData);
		}
	}
}
