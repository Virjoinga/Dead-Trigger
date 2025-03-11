public class FundSettings : Settings<E_FundID>
{
	public int GoldCost;

	public int MoneyCost;

	public bool ConvertGold;

	public int AddGold;

	public int AddMoney;

	public int GetPriceInCents()
	{
		switch (ID)
		{
		case E_FundID.Gold099:
			return 99;
		case E_FundID.Gold299:
			return 299;
		case E_FundID.Gold499:
			return 499;
		case E_FundID.Gold999:
			return 999;
		case E_FundID.Gold1299:
			return 1299;
		case E_FundID.Money99:
			return 99;
		case E_FundID.Money299:
			return 299;
		case E_FundID.Money499:
			return 499;
		case E_FundID.Money999:
			return 999;
		default:
			return 0;
		}
	}

	public override string GetSettingsClass()
	{
		return "fund";
	}
}
