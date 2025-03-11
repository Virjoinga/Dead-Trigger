using System.Collections.Generic;

public class PPIUpgradeList
{
	public class UpgradeData
	{
		public E_UpgradeID ID;
	}

	public List<UpgradeData> Upgrades = new List<UpgradeData>();

	public bool ContainsUpgrade(E_UpgradeID id)
	{
		foreach (UpgradeData upgrade in Upgrades)
		{
			if (upgrade.ID == id)
			{
				return true;
			}
		}
		return false;
	}
}
