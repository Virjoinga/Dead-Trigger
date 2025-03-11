public class Pickup_Money : Pickup
{
	public int m_Amount = 1;

	public override E_PickupID GetID()
	{
		return E_PickupID.Money;
	}

	protected override void OnExpiration()
	{
		Game.Instance.MissionResultData.WastedMoneyBags += m_Amount;
	}
}
