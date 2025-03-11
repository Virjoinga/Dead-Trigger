using UnityEngine;

public class MFGooglePlayBillingV3
{
	public const int RESULT_OK = 0;

	public const int RESULT_USER_CANCELED = 1;

	public const int RESULT_BILLING_UNAVAILABLE = 3;

	public const int RESULT_ITEM_UNAVAILABLE = 4;

	public const int RESULT_DEVELOPER_ERROR = 5;

	public const int RESULT_ERROR = 6;

	public const int RESULT_ITEM_ALREADY_OWNED = 7;

	public const int RESULT_ITEM_NOT_OWNED = 8;

	private static AndroidJavaClass m_plugin;

	private static string ITEM_TYPE;

	static MFGooglePlayBillingV3()
	{
		ITEM_TYPE = "inapp";
		//m_plugin = new AndroidJavaClass("com.madfingergames.billing.googleplay.v3.UnityPlugin");
	}

	public static void Start()
	{
		//m_plugin.CallStatic("start");
	}

	public static void RequestPurchase(string productId, string developerPayload)
	{
		m_plugin.CallStatic("requestPurchase", ITEM_TYPE, productId, developerPayload);
	}

	public static void ConsumePurchase(MFGooglePlayBillingTransactionV3 transaction)
	{
		m_plugin.CallStatic("consumePurchase", transaction.Receipt);
	}

	public static void QueryPurchases()
	{
		m_plugin.CallStatic("queryPurchases");
	}

	public static void QueryProductsDetails(string[] productIds)
	{
		m_plugin.CallStatic("queryProductsDetails", ITEM_TYPE, productIds);
	}
}
