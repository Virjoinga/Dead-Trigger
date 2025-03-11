public abstract class IAPExtensionRequestPurchase : IAP.Extension
{
	public delegate void RequestPurchaseCallback(string requestId, string request, string signature);

	public abstract void MakeRequest(string productId, RequestPurchaseCallback callback);
}
