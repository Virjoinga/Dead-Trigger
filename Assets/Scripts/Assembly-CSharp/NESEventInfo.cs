using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NESEventInfo
{
	public Component m_InComponent;

	public string m_Event;

	public List<NESActionInfo> m_Actions = new List<NESActionInfo>();

	public bool isValid
	{
		get
		{
			if (m_InComponent == null || m_Event == null || m_Event == string.Empty || m_Actions == null || m_Actions.Count == 0)
			{
				return false;
			}
			return true;
		}
	}
}
