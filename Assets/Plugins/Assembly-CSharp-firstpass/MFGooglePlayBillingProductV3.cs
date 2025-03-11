using LitJson;

public class MFGooglePlayBillingProductV3
{
	public string productId;

	public string type;

	public string price;

	public string title;

	public string description;

	public string price_currency_code;

	public long price_amount_micros;

	public MFGooglePlayBillingProductV3()
	{
	}

	public MFGooglePlayBillingProductV3(JsonData productJson)
	{
		productId = ((!productJson.HasValue("productId")) ? string.Empty : ((string)productJson["productId"]));
		type = ((!productJson.HasValue("type")) ? string.Empty : ((string)productJson["type"]));
		price = ((!productJson.HasValue("price")) ? string.Empty : ((string)productJson["price"]));
		title = ((!productJson.HasValue("title")) ? string.Empty : ((string)productJson["title"]));
		description = ((!productJson.HasValue("description")) ? string.Empty : ((string)productJson["description"]));
		price_currency_code = ((!productJson.HasValue("price_currency_code")) ? string.Empty : ((string)productJson["price_currency_code"]));
		price_amount_micros = ((!productJson.HasValue("price_amount_micros")) ? 0 : ((!productJson["price_amount_micros"].IsLong) ? ((int)productJson["price_amount_micros"]) : ((long)productJson["price_amount_micros"])));
	}
}
