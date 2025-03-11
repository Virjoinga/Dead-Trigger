using UnityEngine;

namespace OnePF
{
	public class OpenIAB
	{
		private static IOpenIAB _billing;

		public static GameObject EventManager
		{
			get
			{
				return GameObject.Find(typeof(OpenIABEventManager).ToString());
			}
		}

		static OpenIAB()
		{
			_billing = new OpenIAB_Android();
			Debug.Log("********** Android OpenIAB plugin initialized **********");
		}

		public static void mapSku(string sku, string storeName, string storeSku)
		{
			_billing.mapSku(sku, storeName, storeSku);
		}

		public static void init(Options options)
		{
			_billing.init(options);
		}

		public static void unbindService()
		{
			_billing.unbindService();
		}

		public static bool areSubscriptionsSupported()
		{
			return _billing.areSubscriptionsSupported();
		}

		public static void queryInventory()
		{
			_billing.queryInventory();
		}

		public static void queryInventory(string[] skus)
		{
			_billing.queryInventory(skus);
		}

		public static void purchaseProduct(string sku, string developerPayload = "")
		{
			_billing.purchaseProduct(sku, developerPayload);
		}

		public static void purchaseSubscription(string sku, string developerPayload = "")
		{
			_billing.purchaseSubscription(sku, developerPayload);
		}

		public static void consumeProduct(Purchase purchase)
		{
			_billing.consumeProduct(purchase);
		}

		public static void restoreTransactions()
		{
			_billing.restoreTransactions();
		}

		public static bool isDebugLog()
		{
			return _billing.isDebugLog();
		}

		public static void enableDebugLogging(bool enabled)
		{
			_billing.enableDebugLogging(enabled);
		}

		public static void enableDebugLogging(bool enabled, string tag)
		{
			_billing.enableDebugLogging(enabled, tag);
		}
	}
}
