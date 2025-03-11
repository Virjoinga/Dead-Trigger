using System;
using LitJson;

public class MFGooglePlayBillingTransactionV3
{
	public const int STATE_PURCHASED = 0;

	public const int STATE_CANCELED = 1;

	public const int STATE_REFUNDED = 2;

	public string OrderId { get; private set; }

	public string ProductId { get; private set; }

	public int State { get; private set; }

	public string DeveloperPayload { get; private set; }

	public DateTime Date { get; private set; }

	public string Receipt { get; private set; }

	public string Signature { get; private set; }

	public MFGooglePlayBillingTransactionV3(string receiptJSON, string signature)
	{
		JsonData jsonData = JsonMapper.ToObject(receiptJSON);
		OrderId = (string)jsonData["orderId"];
		ProductId = (string)jsonData["productId"];
		State = (int)jsonData["purchaseState"];
		DeveloperPayload = (string)jsonData["developerPayload"];
		if (jsonData["purchaseTime"].GetJsonType().Equals(JsonType.Int))
		{
			Date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)(int)jsonData["purchaseTime"] / 1000.0);
		}
		else
		{
			Date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double)(long)jsonData["purchaseTime"] / 1000.0);
		}
		Receipt = receiptJSON;
		Signature = signature;
	}

	public MFGooglePlayBillingTransactionV3()
	{
	}
}
