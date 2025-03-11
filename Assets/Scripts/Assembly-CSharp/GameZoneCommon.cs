using System.Collections.Generic;
using UnityEngine;

public class GameZoneCommon : MonoBehaviour
{
	private List<IGameZoneChild> m_ControlledChilds = new List<IGameZoneChild>();

	protected void CollectChilds(bool inIncludeInactive)
	{
		IGameZoneChild_AutoRegister[] componentsInChildrenWithInterface = base.gameObject.GetComponentsInChildrenWithInterface<IGameZoneChild_AutoRegister>(inIncludeInactive);
		if (componentsInChildrenWithInterface != null)
		{
			IGameZoneChild_AutoRegister[] array = componentsInChildrenWithInterface;
			foreach (IGameZoneChild inObject in array)
			{
				RegisterControllableObject(inObject);
			}
		}
	}

	protected void EnableChilds()
	{
		foreach (IGameZoneChild controlledChild in m_ControlledChilds)
		{
			if (controlledChild.IsActivatedWithGameZone())
			{
				controlledChild.Enable();
			}
		}
	}

	protected void DisableChilds()
	{
		foreach (IGameZoneChild controlledChild in m_ControlledChilds)
		{
			controlledChild.Disable();
		}
	}

	protected void ResetChilds()
	{
		foreach (IGameZoneChild controlledChild in m_ControlledChilds)
		{
			controlledChild.Reset();
		}
	}

	public virtual void RegisterControllableObject(IGameZoneChild inObject)
	{
		if (!m_ControlledChilds.Contains(inObject))
		{
			m_ControlledChilds.Add(inObject);
		}
	}

	public virtual void UnRegisterControllableObject(IGameZoneChild inObject)
	{
		m_ControlledChilds.Remove(inObject);
	}
}
