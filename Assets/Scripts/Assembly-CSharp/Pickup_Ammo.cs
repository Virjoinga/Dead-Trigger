public class Pickup_Ammo : Pickup
{
	public int m_Amount;

	public E_WeaponID m_WeaponID;

	public override E_PickupID GetID()
	{
		return E_PickupID.Ammo;
	}

	protected override void OnExpiration()
	{
		Game.Instance.MissionResultData.WastedMoneyBags++;
	}
}
