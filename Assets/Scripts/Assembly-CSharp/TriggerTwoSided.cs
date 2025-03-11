using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[NESEvent(new string[] { "On Enter [G]", "On Enter [R]", "On Exit [G]", "On Exit [R]", "On Empty" })]
[AddComponentMenu("Triggers/Trigger - Two Sided")]
public class TriggerTwoSided : MonoBehaviour, IGameZoneChild, IGameZoneChild_AutoRegister
{
	private enum E_Side
	{
		Positive = 0,
		Negative = 1
	}

	private enum E_Event
	{
		PosEnter = 0,
		PosExit = 1,
		NegEnter = 2,
		NegExit = 3,
		Empty = 4,
		_MAX_ = 5
	}

	private static readonly Color PosSideCol = new Color(0.2f, 1f, 0.2f, 0.4f);

	private static readonly Color NegSideCol = new Color(1f, 0.2f, 0.2f, 0.4f);

	private NESController m_NESController;

	private Transform m_XForm;

	private BoxCollider m_Box;

	private int m_InsideCounter;

	public List<string> m_InstigatorTags = new List<string>(Trigger.DefaultInstigators);

	public Trigger.EventData[] m_Data = new Trigger.EventData[5]
	{
		new Trigger.EventData("On Enter [G]"),
		new Trigger.EventData("On Enter [R]"),
		new Trigger.EventData("On Exit [G]"),
		new Trigger.EventData("On Exit [R]"),
		new Trigger.EventData("On Empty")
	};

	private void Awake()
	{
		m_XForm = base.gameObject.transform;
		m_Box = base.GetComponent<Collider>() as BoxCollider;
		if (m_Box != null)
		{
			m_Box.isTrigger = true;
		}
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (!(m_NESController == null))
		{
		}
	}

	private void Destroy()
	{
		m_Data = null;
	}

	private void Update()
	{
		Trigger.EventData[] data = m_Data;
		foreach (Trigger.EventData eventData in data)
		{
			eventData.m_TimeToTrigger -= Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider Other)
	{
		GameObject obj = Other.gameObject;
		if (IsValidInstigator(obj))
		{
			E_Side side = GetSide(obj);
			TriggerEvent((side != 0) ? E_Event.NegEnter : E_Event.PosEnter);
			m_InsideCounter++;
		}
	}

	private void OnTriggerExit(Collider Other)
	{
		GameObject obj = Other.gameObject;
		if (IsValidInstigator(obj))
		{
			E_Side side = GetSide(obj);
			TriggerEvent((side == E_Side.Positive) ? E_Event.PosExit : E_Event.NegExit);
			if (--m_InsideCounter == 0)
			{
				TriggerEvent(E_Event.Empty);
			}
		}
	}

	private void TriggerEvent(E_Event E)
	{
		Trigger.EventData eventData = m_Data[(int)E];
		if (!(eventData.m_TimeToTrigger > 0f) && (eventData.m_TriggerCount < eventData.m_MaxTriggerCount || eventData.m_MaxTriggerCount == 0))
		{
			eventData.m_TriggerCount++;
			eventData.m_TimeToTrigger = eventData.m_ReTriggerDelay;
			m_NESController.SendGameEvent(this, eventData.m_Name);
		}
	}

	private bool IsValidInstigator(GameObject Obj)
	{
		if (m_InstigatorTags.Count == 0 || m_InstigatorTags.Contains(Obj.tag))
		{
			return true;
		}
		return false;
	}

	private E_Side GetSide(GameObject Obj)
	{
		Vector3 rhs = Obj.transform.position - m_XForm.position;
		float num = Vector3.Dot(m_XForm.forward, rhs);
		return (!(num >= 0f)) ? E_Side.Negative : E_Side.Positive;
	}

	[NESAction]
	public void Enable()
	{
		base.gameObject._SetActiveRecursively(true);
	}

	[NESAction]
	public void Disable()
	{
		base.gameObject._SetActiveRecursively(false);
	}

	public void Reset()
	{
	}

	public bool IsActivatedWithGameZone()
	{
		return false;
	}

	private void OnDrawGizmos()
	{
		m_Box = base.GetComponent<Collider>() as BoxCollider;
		m_XForm = base.gameObject.transform;
		if (m_Box != null)
		{
			Vector3 size = new Vector3(m_Box.size.x, m_Box.size.y, m_Box.size.z * 0.48f);
			Vector3 vector = Vector3.forward * (m_Box.size.z - size.z) * 0.5f;
			Gizmos.matrix = m_XForm.localToWorldMatrix;
			Gizmos.color = PosSideCol;
			Gizmos.DrawCube(m_Box.center + vector, size);
			Gizmos.color = NegSideCol;
			Gizmos.DrawCube(m_Box.center - vector, size);
			Gizmos.matrix = Matrix4x4.identity;
			vector = Vector3.forward * (m_Box.size.z * 0.5f);
			DebugDraw.LineOriented(PosSideCol, m_Box.center + vector, m_Box.center + 3f * vector, m_XForm.localToWorldMatrix);
			DebugDraw.LineOriented(NegSideCol, m_Box.center - vector, m_Box.center - 3f * vector, m_XForm.localToWorldMatrix);
		}
	}
}
