public class UpgradeSettings : Settings<E_UpgradeID>
{
	public int MoneyCost;

	public int GoldCost;

	public E_UpgradeType UpgradeType;

	public override string GetSettingsClass()
	{
		return "upgrade";
	}
}
