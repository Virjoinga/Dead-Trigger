public struct PPIItemData
{
	public E_ItemID ID;

	public int EquipSlotIdx;

	public int StatsUseCount;

	public int StatsKills;

	public int Count;

	public bool IsValid()
	{
		return ID != E_ItemID.None;
	}
}
