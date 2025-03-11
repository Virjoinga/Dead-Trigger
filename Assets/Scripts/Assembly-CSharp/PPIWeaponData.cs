public struct PPIWeaponData
{
	public E_WeaponID ID;

	public E_UpgradeLevel UpgradeLevel;

	public int EquipSlotIdx;

	public int StatsFire;

	public int StatsHits;

	public int StatsKills;

	public bool IsValid()
	{
		return ID != E_WeaponID.None;
	}
}
