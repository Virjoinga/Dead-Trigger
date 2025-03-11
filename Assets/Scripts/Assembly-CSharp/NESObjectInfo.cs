using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NESObjectInfo
{
	public GameObject m_InObject;

	public List<NESEventInfo> m_Events = new List<NESEventInfo>();

	public bool isValid
	{
		get
		{
			if (m_InObject == null || m_Events == null || m_Events.Count == 0)
			{
				return false;
			}
			return true;
		}
	}
}
