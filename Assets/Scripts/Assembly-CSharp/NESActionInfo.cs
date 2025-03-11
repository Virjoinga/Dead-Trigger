using System;
using System.Reflection;
using UnityEngine;

[Serializable]
public class NESActionInfo
{
	public delegate void DelegateNoArgs();

	public bool m_Disabled;

	public GameObject m_Target;

	public Component m_TargetComponent;

	public string m_TargetAction;

	public NESActionParamInfo m_1stParam;

	public float m_Delay;

	public MethodInfo m_Method;

	public DelegateNoArgs m_Delegate;

	public bool isValid
	{
		get
		{
			if (m_Target == null || m_TargetComponent == null || m_TargetAction == null || m_TargetAction == string.Empty)
			{
				return false;
			}
			return true;
		}
	}
}
