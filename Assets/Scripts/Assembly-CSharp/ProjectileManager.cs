using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Weapons/ProjectileManager")]
public class ProjectileManager : MonoBehaviour
{
	[Serializable]
	public class SoundInfo
	{
		public AudioClip[] DefaultHits = new AudioClip[0];

		public AudioClip[] MetalHits = new AudioClip[0];

		public AudioClip[] BloodHits = new AudioClip[0];

		public AudioClip[] WaterHits = new AudioClip[0];

		public AudioClip HitDefault
		{
			get
			{
				if (DefaultHits.Length == 0)
				{
					return null;
				}
				return DefaultHits[UnityEngine.Random.Range(0, DefaultHits.Length)];
			}
		}

		public AudioClip HitMetal
		{
			get
			{
				if (MetalHits.Length == 0)
				{
					return null;
				}
				return MetalHits[UnityEngine.Random.Range(0, MetalHits.Length)];
			}
		}

		public AudioClip HitBlood
		{
			get
			{
				if (BloodHits.Length == 0)
				{
					return null;
				}
				return BloodHits[UnityEngine.Random.Range(0, BloodHits.Length)];
			}
		}

		public AudioClip HitWater
		{
			get
			{
				if (WaterHits.Length == 0)
				{
					return null;
				}
				return WaterHits[UnityEngine.Random.Range(0, WaterHits.Length)];
			}
		}
	}

	public static ProjectileManager Instance;

	public SoundInfo ProjectileSounds = new SoundInfo();

	public SoundInfo GrenadeSounds = new SoundInfo();

	private GameObject Audio;

	private AudioSource AudioSource;

	private Dictionary<E_ProjectileType, ProjectileCacheEx> CacheOfProjectiles = new Dictionary<E_ProjectileType, ProjectileCacheEx>();

	private List<Projectile> ActiveProjectiles = new List<Projectile>();

	private void Awake()
	{
		Instance = this;
		CacheOfProjectiles[E_ProjectileType.Pistol] = new ProjectileCacheEx("Weapons/Projectiles/ProjectilePistol", E_ProjectileType.Pistol, 10);
		CacheOfProjectiles[E_ProjectileType.SMG] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileSMGWithTrace", E_ProjectileType.SMG, 10);
		CacheOfProjectiles[E_ProjectileType.AR] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileSMGWithTrace", E_ProjectileType.AR, 10);
		CacheOfProjectiles[E_ProjectileType.MG] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileSMGWithTrace", E_ProjectileType.MG, 10);
		CacheOfProjectiles[E_ProjectileType.Shotgun] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileShotgun", E_ProjectileType.Shotgun, 10);
		CacheOfProjectiles[E_ProjectileType.VomitRed] = new ProjectileCacheEx("Weapons/Projectiles/VomitRed", E_ProjectileType.VomitRed, 3);
		CacheOfProjectiles[E_ProjectileType.VomitGreen] = new ProjectileCacheEx("Weapons/Projectiles/VomitGreen", E_ProjectileType.VomitGreen, 3);
		CacheOfProjectiles[E_ProjectileType.Melee] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileMelee", E_ProjectileType.Melee, 3);
		CacheOfProjectiles[E_ProjectileType.AlienGun] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileAlienGun", E_ProjectileType.AlienGun, 2);
		CacheOfProjectiles[E_ProjectileType.Crossbow] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileCrossbow", E_ProjectileType.Crossbow, 4);
		CacheOfProjectiles[E_ProjectileType.GrenadeLauncher] = new ProjectileCacheEx("Weapons/Projectiles/ProjectileGrenadeLauncher", E_ProjectileType.GrenadeLauncher, 4);
		CacheOfProjectiles[E_ProjectileType.SantaPresent] = new ProjectileCacheEx("Weapons/Projectiles/Present", E_ProjectileType.SantaPresent, 4);
		Audio = new GameObject("ProjectilesAudio", typeof(AudioSource));
		AudioSource = Audio.GetComponent<AudioSource>();
		AudioSource.playOnAwake = false;
		AudioSource.minDistance = 1f;
		AudioSource.maxDistance = 30f;
		AudioSource.rolloffMode = AudioRolloffMode.Linear;
	}

	private void OnDestroy()
	{
		ProjectileSounds = null;
		GrenadeSounds = null;
		Audio = null;
		AudioSource = null;
		CacheOfProjectiles.Clear();
		ActiveProjectiles.Clear();
	}

	private void FixedUpdate()
	{
		UpdateProjectiles(Time.fixedDeltaTime);
	}

	private void LateUpdate()
	{
		CollectUnusedProjectiles();
	}

	private void UpdateProjectiles(float deltaTime)
	{
		if (Game.Instance.GameState != E_GameState.Game || Time.deltaTime <= 0f)
		{
			return;
		}
		foreach (Projectile activeProjectile in ActiveProjectiles)
		{
			if (!activeProjectile.IsFinished())
			{
				activeProjectile.ProjectileUpdate(deltaTime);
			}
		}
	}

	private void CollectUnusedProjectiles()
	{
		for (int i = 0; i < ActiveProjectiles.Count; i++)
		{
			if (ActiveProjectiles[i].IsFinished())
			{
				ReturnProjectile(ActiveProjectiles[i]);
				ActiveProjectiles.RemoveAt(i);
			}
		}
	}

	public void SpawnProjectile(E_ProjectileType inProjeType, Vector3 inPos, Vector3 inDir, ProjectileInitSettings inSettings)
	{
		if (!CacheOfProjectiles.ContainsKey(inProjeType))
		{
			Debug.LogError("ProjectileFactory: unknown type " + inProjeType);
			return;
		}
		if (CacheOfProjectiles[inProjeType] == null)
		{
			Debug.LogError(string.Concat("ProjectileFactory: For this type ", inProjeType, " we don't have resource"));
			return;
		}
		Projectile projectile = CacheOfProjectiles[inProjeType].Get();
		if (projectile == null)
		{
			Debug.LogError("ProjectileFactory: Can't create projectile for type " + inProjeType);
			return;
		}
		projectile.ProjectileInit(inPos, inDir.normalized, inSettings);
		ActiveProjectiles.Add(projectile);
	}

	public void ReturnProjectile(Projectile inProjectile)
	{
		if (inProjectile == null)
		{
			Debug.LogError("ProjectileFactory: sombody is trying return null object to cache");
			return;
		}
		if (!CacheOfProjectiles.ContainsKey(inProjectile.ProjectileType))
		{
			Debug.LogError("ProjectileFactory: unknown type " + inProjectile.ProjectileType);
			return;
		}
		if (CacheOfProjectiles[inProjectile.ProjectileType] == null)
		{
			Debug.LogError("ProjectileFactory: We don't have cache for this projectile type. This object was not created by this manager");
			return;
		}
		inProjectile.ProjectileDeinit();
		CacheOfProjectiles[inProjectile.ProjectileType].Return(inProjectile);
	}

	public void Reset()
	{
		for (int i = 0; i < ActiveProjectiles.Count; i++)
		{
			ActiveProjectiles[i].ProjectileDeinit();
			ReturnProjectile(ActiveProjectiles[i]);
		}
		ActiveProjectiles.Clear();
	}
}
