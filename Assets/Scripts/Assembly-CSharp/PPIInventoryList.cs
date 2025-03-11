using System.Collections.Generic;

public class PPIInventoryList
{
	private enum E_DatatType
	{
		None = 0,
		Item = 1,
		Weapon = 2
	}

	public List<PPIItemData> Items = new List<PPIItemData>();

	public List<PPIWeaponData> Weapons = new List<PPIWeaponData>();

	public bool ContainsWeapon(E_WeaponID weapon)
	{
		foreach (PPIWeaponData weapon2 in Weapons)
		{
			if (weapon2.ID == weapon)
			{
				return true;
			}
		}
		return false;
	}

	public int ContainsItem(E_ItemID item)
	{
		foreach (PPIItemData item2 in Items)
		{
			if (item2.ID == item)
			{
				return item2.Count;
			}
		}
		return 0;
	}
}
