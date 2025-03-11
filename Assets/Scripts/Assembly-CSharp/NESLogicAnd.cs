using System;
using System.Collections.Generic;
using UnityEngine;

[NESEvent(new string[] { "OnSatisfied" })]
[AddComponentMenu("Game/NES/Logic And")]
public class NESLogicAnd : MonoBehaviour
{
	[Serializable]
	public class Property
	{
		public static string m_DefaultName = "[noname]";

		public string m_Name = m_DefaultName;

		public bool m_InitValue;

		public bool m_TargetValue = true;

		public bool m_ActualValue;
	}

	public List<Property> m_Properties = new List<Property>();

	private NESController m_Controller;

	private void Awake()
	{
		m_Controller = base.gameObject.GetFirstComponentUpward<NESController>();
		ResetProperties();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Gizmos.DrawCube(Vector3.zero, new Vector3(1.5f, 0.2f, 0.2f));
	}

	public void Reset()
	{
		ResetProperties();
	}

	[NESAction(Argument1 = "GetAvailibleNames")]
	public void SetValue(string inName)
	{
		ChangeValue(inName, true);
	}

	[NESAction(Argument1 = "GetAvailibleNames")]
	public void ClearValue(string inName)
	{
		ChangeValue(inName, false);
	}

	private string[] GetAvailibleNames()
	{
		List<string> list = new List<string>();
		foreach (Property property in m_Properties)
		{
			if (property.m_Name != Property.m_DefaultName)
			{
				list.Add(property.m_Name);
			}
		}
		return list.ToArray();
	}

	private void ChangeValue(string inName, bool inValue)
	{
		Property property = m_Properties.Find((Property item) => item.m_Name == inName);
		if (property != null)
		{
			property.m_ActualValue = inValue;
			CheckCondition();
		}
	}

	private void CheckCondition()
	{
		if (m_Controller == null)
		{
			return;
		}
		foreach (Property property in m_Properties)
		{
			if (property.m_ActualValue != property.m_TargetValue)
			{
				return;
			}
		}
		m_Controller.SendGameEvent(this, "OnSatisfied");
	}

	public void ResetProperties()
	{
		foreach (Property property in m_Properties)
		{
			property.m_ActualValue = property.m_InitValue;
		}
	}
}
