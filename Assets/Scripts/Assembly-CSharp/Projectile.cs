using UnityEngine;

[AddComponentMenu("Weapons/Projectile")]
public abstract class Projectile : MonoBehaviour
{
	protected ProjectileInitSettings Settings;

	public E_ProjectileType ProjectileType { get; set; }

	public GameObject GameObject { get; private set; }

	public Transform Transform { get; private set; }

	public bool ignoreThisHit { get; set; }

	public bool hitProcessed { get; set; }

	public float Speed
	{
		get
		{
			return Settings.Speed;
		}
	}

	public float Impuls
	{
		get
		{
			return Settings.Impuls;
		}
	}

	public AgentHuman Agent
	{
		get
		{
			return Settings.Agent;
		}
	}

	public E_WeaponID Weapon
	{
		get
		{
			return Settings.Weapon;
		}
	}

	public E_WeaponType WeaponType
	{
		get
		{
			return Settings.WeaponType;
		}
	}

	public float BodyPartDamageModif
	{
		get
		{
			return Settings.BodyPartDamageModif;
		}
	}

	public virtual float Damage()
	{
		return Settings.Damage;
	}

	public virtual void Awake()
	{
		GameObject = base.gameObject;
		Transform = GameObject.transform;
	}

	public virtual void ProjectileInit(Vector3 pos, Vector3 dir, ProjectileInitSettings inSettings)
	{
		Transform.position = pos;
		Transform.forward = dir;
		Settings = inSettings;
		ignoreThisHit = false;
		hitProcessed = false;
	}

	public abstract void ProjectileUpdate(float deltaTime);

	public abstract void ProjectileDeinit();

	public abstract bool IsFinished();
}
