using UnityEngine;

public class CitySiteInfo
{
	public enum InfoType
	{
		Normal = 0,
		Shop = 1,
		SafeHaven = 2,
		Bank = 3,
		Equip = 4,
		Arena = 5,
		Money = 6,
		Casino = 7,
		None = 8
	}

	protected CitySiteSlot m_Slot;

	protected InfoType m_InfoType;

	public InfoType infoType
	{
		get
		{
			return m_InfoType;
		}
	}

	public CitySiteSlot slot
	{
		get
		{
			return m_Slot;
		}
		set
		{
			m_Slot = value;
		}
	}

	public CitySiteInfo(InfoType type)
	{
		m_InfoType = type;
		if (type == InfoType.Normal)
		{
			Debug.LogError("CitySiteInfo constructor - wrong type, you can't use 'Normal', create class CityMissionInfo instead!");
		}
	}

	protected CitySiteInfo()
	{
	}

	public override string ToString()
	{
		return "CitySiteInfo - Type: " + infoType;
	}
}
