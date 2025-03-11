using System;
using UnityEngine;

[Serializable]
public abstract class Agent : MonoBehaviour
{
	public delegate int ModifierInt(int data);

	public delegate float ModifierFloat(float data);

	public ModifierInt ModifierMoney;

	public ModifierInt ModifierExperience;

	public ModifierFloat ModifierDamage;

	public ModifierFloat ModifierSpeed;

	public E_AgentType AgentType { get; set; }

	public Transform Transform { get; private set; }

	public GameObject GameObject { get; private set; }

	public AudioSource Audio { get; private set; }

	public Animation Animation { get; private set; }

	public Vector3 Position
	{
		get
		{
			return Transform.position;
		}
	}

	public Vector3 Forward
	{
		get
		{
			return Transform.forward;
		}
	}

	public Vector3 Right
	{
		get
		{
			return Transform.right;
		}
	}

	public abstract bool IsAlive { get; }

	public virtual bool IsPlayer
	{
		get
		{
			return false;
		}
	}

	public abstract bool IsVisible { get; }

	public abstract bool IsInvulnerable { get; }

	public abstract Vector3 ChestPosition { get; }

	public virtual float HealthPercent
	{
		get
		{
			return 1f;
		}
	}

	public virtual void OnReceiveRangeDamage(Agent attacker, float damage, Vector3 impuls, E_WeaponID weaponID, E_WeaponType weaponType)
	{
	}

	public virtual void OnShootSound(AgentHuman inAgent)
	{
	}

	protected void Initialize()
	{
		Transform = base.transform;
		GameObject = base.gameObject;
		Audio = base.GetComponent<AudioSource>();
		Animation = base.GetComponent<Animation>();
	}
}
