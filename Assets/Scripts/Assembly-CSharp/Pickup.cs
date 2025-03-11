using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	private enum E_Status
	{
		Activating = 0,
		Active = 1,
		Deactivating = 2,
		Inactive = 3
	}

	[Serializable]
	public class Effects
	{
		public string m_Animation;

		public ParticleSystem m_Particle;
	}

	private GameObject m_Obj;

	private Transform m_ObjXFrom;

	private Animation m_Anim;

	private RotateObject m_Rotator;

	private float m_Timer;

	private E_Status m_Status;

	private float m_ExpirationTime = -1f;

	public Effects m_OnActivationEffects = new Effects();

	public Effects m_OnPickupEffects = new Effects();

	public Effects m_OnExpirationEffects = new Effects();

	private Effects m_Effects;

	public GameObject GameObj
	{
		get
		{
			return m_Obj;
		}
	}

	public Vector3 Position
	{
		get
		{
			return m_ObjXFrom.position;
		}
	}

	public float LifeTime
	{
		get
		{
			return m_Timer;
		}
	}

	public bool IsInvalid
	{
		get
		{
			return m_Status == E_Status.Inactive;
		}
	}

	public bool IsMarked { get; set; }

	private void Awake()
	{
		m_Obj = base.gameObject;
		m_ObjXFrom = m_Obj.transform;
		m_Anim = m_Obj.GetComponent<Animation>();
		m_Rotator = m_Obj.GetComponent<RotateObject>();
		m_Obj._SetActiveRecursively(false);
	}

	public void DroppedOut(Vector3 Pos, float Expiration)
	{
		m_Status = E_Status.Activating;
		m_Timer = 0f;
		m_ExpirationTime = Expiration;
		m_Obj._SetActiveRecursively(true);
		m_ObjXFrom.position = Pos;
		StartEffects(m_OnActivationEffects);
		IsMarked = false;
	}

	private void Update()
	{
		if (Time.deltaTime <= float.Epsilon)
		{
			return;
		}
		float realDeltaTime = TimeManager.Instance.GetRealDeltaTime();
		float ratio = realDeltaTime / Time.deltaTime;
		m_Timer += realDeltaTime;
		switch (m_Status)
		{
		case E_Status.Activating:
			if (AreEffectsFinished())
			{
				m_Status = E_Status.Active;
			}
			break;
		case E_Status.Active:
			if (m_ExpirationTime > 0f && m_Timer >= m_ExpirationTime)
			{
				Expired();
			}
			break;
		case E_Status.Deactivating:
			if (AreEffectsFinished())
			{
				m_Status = E_Status.Inactive;
				m_Obj._SetActiveRecursively(false);
			}
			break;
		}
		CompensateSlowMotion(ratio);
	}

	public void PickedUp()
	{
		m_Status = E_Status.Deactivating;
		StartEffects(m_OnPickupEffects);
	}

	public bool CanBePickedUp(bool EndOfMission)
	{
		return m_Status == E_Status.Active || (m_Status == E_Status.Activating && EndOfMission);
	}

	public void Expired()
	{
		m_Status = E_Status.Deactivating;
		OnExpiration();
		StartEffects(m_OnExpirationEffects);
	}

	protected virtual void OnExpiration()
	{
	}

	private void StartEffects(Effects E)
	{
		m_Effects = E;
		if (m_Effects.m_Animation != string.Empty && m_Anim != null)
		{
			m_Anim.Play(m_Effects.m_Animation);
		}
		if (m_Effects.m_Particle != null)
		{
			m_Effects.m_Particle.Play();
		}
	}

	private bool AreEffectsFinished()
	{
		if (m_Effects.m_Animation != string.Empty && m_Anim != null && m_Anim.isPlaying)
		{
			return false;
		}
		if (m_Effects.m_Particle != null && (m_Effects.m_Particle.isPlaying || m_Effects.m_Particle.IsAlive()))
		{
			return false;
		}
		m_Effects = null;
		return true;
	}

	private void CompensateSlowMotion(float Ratio)
	{
		if (m_Rotator != null)
		{
			m_Rotator.SpeedMultiplier = Ratio;
		}
		if (m_Effects == null)
		{
			return;
		}
		if (m_Anim.isPlaying && m_Effects.m_Animation != string.Empty)
		{
			foreach (AnimationState item in m_Anim)
			{
				item.speed = Ratio;
			}
		}
		if (m_Effects.m_Particle != null)
		{
			m_Effects.m_Particle.playbackSpeed = Ratio;
		}
	}

	public virtual E_PickupID GetID()
	{
		return E_PickupID.None;
	}
}
