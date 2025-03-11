using UnityEngine;

public class CitySiteSlot : MonoBehaviour
{
	public enum Type
	{
		Normal = 0,
		Money = 1,
		ChopperMission = 2
	}

	public Type m_Type;

	public int m_EnableSinceStoryId = 1;

	public string m_SlotName = string.Empty;

	public int m_UID = -1;

	private bool m_Occupied;

	public bool occupied
	{
		get
		{
			return m_Occupied;
		}
	}

	public void OccupySlot()
	{
		m_Occupied = true;
	}

	public void FreeSlot()
	{
		m_Occupied = false;
	}

	public Vector3 GetPos()
	{
		return base.transform.position;
	}
}
