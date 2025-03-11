using System;
using LitJson;
using UnityEngine;

public class GetAppSettings : DefaultCloudAction
{
	private string productID
	{
		get
		{
			return GameCloudManager.productID;
		}
	}

	public float chartboostWeightAndroid { get; private set; }

	public float chartboostWeightIOS { get; private set; }

	public float appLovinWeightAndroid { get; private set; }

	public float appLovinWeightIOS { get; private set; }

	public float adsDelayInSeconds { get; private set; }

	public bool useUnityAnalyticsAndroid { get; private set; }

	public bool useUnityAnalyticsIOS { get; private set; }

	public GetAppSettings(float inTimeOut = -1f)
		: base(null, inTimeOut)
	{
		chartboostWeightAndroid = 1f;
		chartboostWeightIOS = 1f;
		appLovinWeightAndroid = 1f;
		appLovinWeightIOS = 1f;
		adsDelayInSeconds = 300f;
		useUnityAnalyticsAndroid = true;
		useUnityAnalyticsIOS = true;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().ProductGetParam(productID, "_GenericSettings", string.Empty);
	}

	protected override void OnSuccess()
	{
		try
		{
			JsonData jsonData = JsonMapper.ToObject(base.result);
			JsonData jsonData2 = ((!jsonData.HasValue("advertisementSettings")) ? null : jsonData["advertisementSettings"]);
			if (jsonData2 != null)
			{
				chartboostWeightAndroid = ((!jsonData2.HasValue("chartboostWeightAndroid")) ? chartboostWeightAndroid : ((float)(double)jsonData2["chartboostWeightAndroid"]));
				chartboostWeightIOS = ((!jsonData2.HasValue("chartboostWeightIOS")) ? chartboostWeightIOS : ((float)(double)jsonData2["chartboostWeightIOS"]));
				appLovinWeightAndroid = ((!jsonData2.HasValue("appLovinWeightAndroid")) ? appLovinWeightAndroid : ((float)(double)jsonData2["appLovinWeightAndroid"]));
				appLovinWeightIOS = ((!jsonData2.HasValue("appLovinWeightIOS")) ? appLovinWeightIOS : ((float)(double)jsonData2["appLovinWeightIOS"]));
				adsDelayInSeconds = ((!jsonData2.HasValue("adsDelayInSeconds")) ? adsDelayInSeconds : ((float)(double)jsonData2["adsDelayInSeconds"]));
			}
			jsonData2 = ((!jsonData.HasValue("analyticsSettings")) ? null : jsonData["analyticsSettings"]);
			if (jsonData2 != null)
			{
				useUnityAnalyticsAndroid = ((!jsonData2.HasValue("useUnityAnalyticsAndroid")) ? useUnityAnalyticsAndroid : ((bool)jsonData2["useUnityAnalyticsAndroid"]));
				useUnityAnalyticsIOS = ((!jsonData2.HasValue("useUnityAnalyticsIOS")) ? useUnityAnalyticsIOS : ((bool)jsonData2["useUnityAnalyticsIOS"]));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to parse result data when obtaining app settings -> " + ex.ToString());
		}
	}
}
