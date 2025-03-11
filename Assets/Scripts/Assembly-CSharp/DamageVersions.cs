using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Interaction/Damage Versions")]
public class DamageVersions : MonoBehaviour, IGameZoneControledObject, IHitZoneOwner
{
	[Serializable]
	public class Version
	{
		public List<GameObject> GameObjects = new List<GameObject>();

		public AudioClip Audio;

		public float MaxHealth;

		public float Health { get; set; }

		public GameObject GameObject { get; set; }

		public void Activate()
		{
			Health = MaxHealth;
			GameObject = GameObjects[UnityEngine.Random.Range(0, GameObjects.Count)];
			GameObject._SetActiveRecursively(true);
		}

		public void DeActivate()
		{
			if (GameObject != null)
			{
				GameObject._SetActiveRecursively(false);
			}
			GameObject = null;
		}

		public void Initialize()
		{
			foreach (GameObject gameObject in GameObjects)
			{
				gameObject._SetActiveRecursively(false);
			}
		}
	}

	public List<Version> Versions = new List<Version>();

	private int CurrentVersion;

	private AudioSource Audio;

	private float Health;

	private void Awake()
	{
		Audio = base.gameObject.GetComponent<AudioSource>();
		GameZone firstComponentUpward = base.gameObject.GetFirstComponentUpward<GameZone>();
		firstComponentUpward.RegisterControllableObject(this);
		foreach (Version version in Versions)
		{
			version.Initialize();
		}
	}

	public void Enable()
	{
		Versions[CurrentVersion].Activate();
	}

	public void Reset()
	{
		foreach (Version version in Versions)
		{
			version.DeActivate();
		}
		CurrentVersion = 0;
	}

	public void Disable()
	{
		Versions[CurrentVersion].Activate();
	}

	public void OnProjectileHit(Projectile p)
	{
	}

	public void OnHitZoneProjectileHit(HitZone zone, Projectile projectile)
	{
		if (CurrentVersion != Versions.Count - 1)
		{
			if (projectile.Agent != null && !projectile.Agent.IsPlayer)
			{
				DoDamage(projectile.Damage() * 2f);
			}
			else
			{
				DoDamage(projectile.Damage());
			}
		}
	}

	public void OnHitZoneRangeDamage(HitZone Zone, Agent Attacker, float Damage, Vector3 Impulse, E_WeaponID weaponID, E_WeaponType WeaponType)
	{
		if (CurrentVersion != Versions.Count - 1)
		{
			DoDamage(Damage);
		}
	}

	public void OnHitZoneMeleeDamage(HitZone Zone, MeleeDamageData Data)
	{
	}

	private void DoDamage(float damage)
	{
		Version version = Versions[CurrentVersion];
		version.Health -= damage;
		if (version.Health <= 0f)
		{
			if ((bool)version.Audio)
			{
				Audio.PlayOneShot(version.Audio);
			}
			version.DeActivate();
			CurrentVersion++;
			Versions[CurrentVersion].Activate();
		}
	}
}
