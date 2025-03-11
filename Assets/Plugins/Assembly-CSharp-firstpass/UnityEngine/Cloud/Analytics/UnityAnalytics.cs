using System.Collections.Generic;

namespace UnityEngine.Cloud.Analytics
{
	public static class UnityAnalytics
	{
		private static SessionImpl s_Implementation;

		public static AnalyticsResult StartSDK(string appId)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.StartWithAppId(appId);
		}

		public static void SetLogLevel(LogLevel logLevel, bool enableLogging = true)
		{
			Logger.EnableLogging = enableLogging;
			Logger.SetLogLevel((int)logLevel);
		}

		public static AnalyticsResult SetUserId(string userId)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.SetUserId(userId);
		}

		public static AnalyticsResult SetUserGender(SexEnum gender)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			object userGender;
			switch (gender)
			{
			case SexEnum.M:
				userGender = "M";
				break;
			case SexEnum.F:
				userGender = "F";
				break;
			default:
				userGender = "U";
				break;
			}
			return (AnalyticsResult)singleton.SetUserGender((string)userGender);
		}

		public static AnalyticsResult SetUserBirthYear(int birthYear)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.SetUserBirthYear(birthYear);
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.Transaction(productId, amount, currency, null, null);
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.Transaction(productId, amount, currency, receiptPurchaseData, signature);
		}

		public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			IUnityAnalyticsSession singleton = GetSingleton();
			return (AnalyticsResult)singleton.CustomEvent(customEventName, eventData);
		}

		private static IUnityAnalyticsSession GetSingleton()
		{
			if (s_Implementation == null)
			{
				Logger.loggerInstance = new UnityLogger();
				IPlatformWrapper platform = PlatformWrapper.platform;
				IFileSystem fileSystem = new FileSystem();
				ICoroutineManager coroutineManager = new UnityCoroutineManager();
				s_Implementation = new SessionImpl(platform, coroutineManager, fileSystem);
				GameObserver.CreateComponent(platform, s_Implementation);
			}
			return s_Implementation;
		}
	}
}
