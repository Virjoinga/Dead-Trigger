using System;
using System.Collections.Generic;
using UnityEngine;

public class CitySiteSlotsManager
{
	private List<CitySiteSlot> m_MissionSlots;

	private List<CitySiteSlot> availableSlots = new List<CitySiteSlot>();

	private System.Random m_Random = new System.Random((int)DateTime.Now.Ticks);

	public void Init(Transform scene)
	{
		m_MissionSlots = new List<CitySiteSlot>();
		foreach (Transform item in scene)
		{
			CitySiteSlot component = item.GetComponent<CitySiteSlot>();
			if (component != null)
			{
				m_MissionSlots.Add(component);
			}
		}
	}

	public CitySiteSlot OccupyMoneySlot(string slotName)
	{
		return OccupySiteSlot(slotName, 10000, CitySiteSlot.Type.Money);
	}

	public CitySiteSlot OccupyChopperSlot(string slotName)
	{
		return OccupySiteSlot(slotName, 10000, CitySiteSlot.Type.ChopperMission);
	}

	public CitySiteSlot OccupySpecialSlot(string slotName)
	{
		return OccupySiteSlot(slotName, 0);
	}

	public CitySiteSlot ForceOccupySiteSlot(int slotUid)
	{
		foreach (CitySiteSlot missionSlot in m_MissionSlots)
		{
			if (missionSlot.m_UID == slotUid)
			{
				missionSlot.OccupySlot();
				return missionSlot;
			}
		}
		Debug.LogWarning("CityMissionSlotManager: Can't find slot with UID: " + slotUid);
		return null;
	}

	public CitySiteSlot OccupySiteSlot(string slotName, int storyId, CitySiteSlot.Type slotType = CitySiteSlot.Type.Normal)
	{
		availableSlots.Clear();
		if (slotName == string.Empty)
		{
			foreach (CitySiteSlot missionSlot in m_MissionSlots)
			{
				if (missionSlot.m_EnableSinceStoryId <= storyId && !missionSlot.occupied && slotType == missionSlot.m_Type && missionSlot.m_SlotName == string.Empty)
				{
					availableSlots.Add(missionSlot);
				}
			}
		}
		else
		{
			foreach (CitySiteSlot missionSlot2 in m_MissionSlots)
			{
				if (missionSlot2.m_SlotName == slotName && !missionSlot2.occupied && slotType == missionSlot2.m_Type)
				{
					availableSlots.Add(missionSlot2);
				}
			}
		}
		if (availableSlots.Count == 0)
		{
			Debug.LogError("CityMissionSlotManager: Can't find any free slot! Requested name: " + slotName);
			return null;
		}
		int index = m_Random.Next(0, availableSlots.Count);
		availableSlots[index].OccupySlot();
		return availableSlots[index];
	}

	public void FreeOccupiedSlot(CitySiteSlot slot)
	{
		slot.FreeSlot();
	}
}
