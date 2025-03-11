using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("Game/NES/Controller")]
public class NESController : MonoBehaviour
{
	[SerializeField]
	private bool m_EnableLogging;

	[SerializeField]
	private bool m_UseDelegatesIfCan = true;

	[SerializeField]
	private List<NESObjectInfo> m_GameLogicInteractions = new List<NESObjectInfo>();

	private int m_ActiveEvents;

	private void Start()
	{
		m_ActiveEvents = 0;
	}

	public void SendGameEvent(MonoBehaviour inFrom, string inEvent)
	{
		if (m_EnableLogging)
		{
			Debug.Log("SendGameEvent :: " + inFrom.name + " " + inEvent);
		}
		if (m_ActiveEvents > 200)
		{
			Debug.LogError("Too many active events, event ignored !!!");
			return;
		}
		NESEventInfo gameLogicEventInfo = GetGameLogicEventInfo(inFrom, inEvent);
		if (gameLogicEventInfo == null)
		{
			return;
		}
		foreach (NESActionInfo action in gameLogicEventInfo.m_Actions)
		{
			if (!action.m_Disabled)
			{
				StartCoroutine(InvokeAction_Corutine(action));
			}
		}
	}

	private IEnumerator InvokeAction_Corutine(NESActionInfo inActionInfo)
	{
		m_ActiveEvents++;
		yield return new WaitForSeconds(inActionInfo.m_Delay);
		m_ActiveEvents--;
		InvokeAction(inActionInfo);
	}

	private void InvokeAction(NESActionInfo inActionInfo)
	{
		if (m_EnableLogging)
		{
			Debug.Log(string.Concat("SendGameEvent :: ", inActionInfo.m_Target, " ", inActionInfo.m_TargetAction));
		}
		if (inActionInfo.m_Method == null)
		{
			inActionInfo.m_Method = inActionInfo.m_TargetComponent.GetType().GetMethod(inActionInfo.m_TargetAction, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (inActionInfo.m_Method == null)
			{
				Debug.LogError(string.Concat("Can't invoke action :: ", inActionInfo.m_Target, " ", inActionInfo.m_TargetAction, " Target action was not found in Target object ", inActionInfo.m_TargetComponent));
				return;
			}
		}
		object[] array = null;
		if (inActionInfo.m_1stParam != null && inActionInfo.m_1stParam.GetValueType() != null)
		{
			array = new object[1] { inActionInfo.m_1stParam.GetValue() };
		}
		if (array == null && inActionInfo.m_Delegate == null && m_UseDelegatesIfCan)
		{
			if (m_EnableLogging)
			{
				Debug.Log("SendGameEvent :: Prepare delegate for interaction");
			}
			Delegate @delegate = Delegate.CreateDelegate(typeof(NESActionInfo.DelegateNoArgs), inActionInfo.m_TargetComponent, inActionInfo.m_Method, false);
			if (@delegate != null)
			{
				inActionInfo.m_Delegate = (NESActionInfo.DelegateNoArgs)@delegate;
			}
		}
		try
		{
			if (inActionInfo.m_Delegate != null && m_UseDelegatesIfCan)
			{
				if (m_EnableLogging)
				{
					Debug.Log(string.Concat("Calling delegate :: ", inActionInfo.m_Target, " ", inActionInfo.m_TargetAction));
				}
				inActionInfo.m_Delegate();
			}
			else
			{
				if (m_EnableLogging)
				{
					Debug.Log(string.Concat("invoke action :: ", inActionInfo.m_Target, " ", inActionInfo.m_TargetAction));
				}
				inActionInfo.m_Method.Invoke(inActionInfo.m_TargetComponent, array);
			}
		}
		catch
		{
			Debug.LogError(string.Concat("Invoke action problem :: ", inActionInfo.m_Target, " ", inActionInfo.m_TargetAction, "\n See next error if problem was in TargetAction"));
			throw;
		}
	}

	private NESEventInfo GetGameLogicEventInfo(Component inComponent, string inEvent)
	{
		foreach (NESObjectInfo gameLogicInteraction in m_GameLogicInteractions)
		{
			if (!(gameLogicInteraction.m_InObject == inComponent.gameObject))
			{
				continue;
			}
			foreach (NESEventInfo @event in gameLogicInteraction.m_Events)
			{
				if (@event.m_InComponent == inComponent && @event.m_Event == inEvent)
				{
					return @event;
				}
			}
			break;
		}
		return null;
	}
}
