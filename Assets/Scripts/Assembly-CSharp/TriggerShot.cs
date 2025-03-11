using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[NESEvent(new string[] { "On Hit" })]
[AddComponentMenu("Triggers/Trigger - Shot")]
public class TriggerShot : MonoBehaviour, IGameZoneChild, IGameZoneChild_AutoRegister, IHitZoneOwner
{
	private enum E_Event
	{
		Hit = 0,
		_MAX_ = 1
	}

	[Serializable]
	public class EventData
	{
		public readonly string m_Name;

		public float m_TimeToTrigger;

		public float m_ReTriggerDelay = 0.1f;

		public int m_TriggerCount;

		public int m_MaxTriggerCount;

		public EventData(string Name)
		{
			m_Name = Name;
		}
	}

	private static readonly Color Col = new Color(0.8f, 0.8f, 0.2f, 0.4f);

	public static readonly string[] DefaultInstigators = new string[1] { "Player" };

	private NESController m_NESController;

	private Transform m_XForm;

	private BoxCollider m_Box;

	public List<string> m_InstigatorTags = new List<string>(DefaultInstigators);

	public EventData[] m_Data = new EventData[1]
	{
		new EventData("On Hit")
	};

	private void Awake()
	{
		m_XForm = base.gameObject.transform;
		m_Box = base.GetComponent<Collider>() as BoxCollider;
		if (m_Box != null)
		{
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
		EventData[] data = m_Data;
		foreach (EventData eventData in data)
		{
			eventData.m_TimeToTrigger -= Time.deltaTime;
		}
	}

	private void TriggerEvent(E_Event E)
	{
		EventData eventData = m_Data[(int)E];
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
			Gizmos.matrix = m_XForm.localToWorldMatrix;
			Gizmos.color = Col;
			Gizmos.DrawCube(m_Box.center, m_Box.size);
			Gizmos.matrix = Matrix4x4.identity;
		}
	}

	public void OnHitZoneProjectileHit(HitZone zone, Projectile projectile)
	{
		TriggerEvent(E_Event.Hit);
	}

	public void OnHitZoneRangeDamage(HitZone zone, Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType)
	{
		TriggerEvent(E_Event.Hit);
	}

	public void OnHitZoneMeleeDamage(HitZone zone, MeleeDamageData Data)
	{
		TriggerEvent(E_Event.Hit);
	}
}
