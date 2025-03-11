public class Funds
{
	public enum Type
	{
		Money = 0,
		Gold = 1,
		XP = 2
	}

	public Type FundsType;

	public int Value;

	public Funds(Type fundsType, int value)
	{
		FundsType = fundsType;
		Value = value;
	}

	public Funds(Funds funds)
	{
		FundsType = funds.FundsType;
		Value = funds.Value;
	}
}
