using System;
using System.Collections;
using UnityEngine;

[AddComponentMenu("Weapons/WeaponBase")]
[RequireComponent(typeof(AudioSource))]
public abstract class WeaponBase : MonoBehaviour
{
	[Serializable]
	public class SoundsInfo
	{
		public AudioClip Arm;

		public AudioClip Disarm;

		public AudioClip Reload;

		public AudioClip[] SoundFire = new AudioClip[0];
	}

	[Serializable]
	public class PlayerAnimsInfo
	{
		public AnimationClip Run;

		public AnimationClip Walk;

		public AnimationClip Idle;

		public AnimationClip Fire;

		public AnimationClip Reload;

		public AnimationClip Arm;

		public AnimationClip Disarm;

		public AnimationClip InjuryL;

		public AnimationClip InjuryR;

		public AnimationClip InjuryFrontL;

		public AnimationClip InjuryFrontR;

		public AnimationClip Death;

		public AnimationClip StrafeL;

		public AnimationClip StrafeR;

		public AnimationClip AimIdle;

		public AnimationClip AimFire;

		public AnimationClip AimWalk;
	}

	[Serializable]
	public class WeaponAnimsInfo
	{
		public AnimationClip Run;

		public AnimationClip Walk;

		public AnimationClip Idle;

		public AnimationClip Fire;

		public AnimationClip Reload;

		public AnimationClip Arm;

		public AnimationClip Disarm;
	}

	public const float ShotPosOffset = 0.55f;

	protected WeaponSettings Settings;

	protected WeaponSettings.Data SettingsData;

	public PlayerAnimsInfo PlayerAnimsSetup = new PlayerAnimsInfo();

	public WeaponAnimsInfo WeaponAnimsSetup = new WeaponAnimsInfo();

	public SoundsInfo SoundsSetup = new SoundsInfo();

	[SerializeField]
	private GameObject Muzzle;

	public ParticleSystem Shells;

	protected Animation Animation;

	protected AgentHuman Owner;

	protected float _MoveDispersion;

	protected float _ShootDispersion;

	protected int _AmmoInClip;

	protected int _AmmoInWeapon;

	protected float AmmoModificator;

	protected GameObject GameObject;

	protected Transform Transform;

	protected AudioSource Audio;

	protected Quaternion Temp = default(Quaternion);

	protected float TimeReload;

	protected float TimeArm;

	protected float TimeDisarm;

	private int UpdateAnimations;

	protected ProjectileInitSettings InitProjSettings;

	private float Busy;

	private float Firing;

	public E_WeaponID WeaponID
	{
		get
		{
			return Settings.ID;
		}
	}

	public E_WeaponType WeaponType
	{
		get
		{
			return Settings.WeaponType;
		}
	}

	public float FinalDispersion
	{
		get
		{
			if (Owner.BlackBoard.Desires.WeaponIronSight)
			{
				return (SettingsData.Dispersion + MoveDispersion + ShootDispersion) * 0.5f;
			}
			return SettingsData.Dispersion + MoveDispersion + ShootDispersion;
		}
	}

	public float MoveDispersion
	{
		private get
		{
			return _MoveDispersion;
		}
		set
		{
			_MoveDispersion = value * SettingsData.DispersionAddMove;
		}
	}

	public float ShootDispersion
	{
		private get
		{
			return _ShootDispersion;
		}
		set
		{
			_ShootDispersion = value * SettingsData.DispersionAddShoot;
		}
	}

	public int ClipAmmo
	{
		get
		{
			return _AmmoInClip;
		}
		protected set
		{
			_AmmoInClip = Mathf.Clamp(value, 0, MaxAmmoInClip);
		}
	}

	public int WeaponAmmo
	{
		get
		{
			return _AmmoInWeapon;
		}
		protected set
		{
			_AmmoInWeapon = Mathf.Clamp(value, 0, MaxAmmoInWeapon);
		}
	}

	public int MaxAmmoInClip
	{
		get
		{
			return SettingsData.MaxAmmoInClip;
		}
	}

	public int MaxAmmoInWeapon
	{
		get
		{
			if (SettingsData.MaxAmmoInWeapon == -1 || SettingsData.MaxAmmoInWeapon == 0)
			{
				return SettingsData.MaxAmmoInWeapon;
			}
			return Mathf.FloorToInt((float)SettingsData.MaxAmmoInWeapon * AmmoModificator + (float)SettingsData.MaxAmmoInClip * (AmmoModificator - 1f));
		}
	}

	public bool HasAnyAmmo
	{
		get
		{
			return ClipAmmo + WeaponAmmo > 0;
		}
	}

	public virtual bool IsFull
	{
		get
		{
			return ClipAmmo + WeaponAmmo == MaxAmmoInWeapon;
		}
	}

	public bool IsFullyLoaded
	{
		get
		{
			return ClipAmmo == MaxAmmoInClip;
		}
	}

	public bool IsFullAuto
	{
		get
		{
			return Settings.FireType == WeaponSettings.E_FireType.Auto;
		}
	}

	public bool IsCriticalAmmo
	{
		get
		{
			return SettingsData.CriticalAmmo >= (float)ClipAmmo;
		}
	}

	public float FireTime
	{
		get
		{
			return SettingsData.FireTime;
		}
	}

	public Vector3 ShotPos
	{
		get
		{
			return Camera.main.transform.position - ShotDir * 0.55f;
		}
	}

	public Vector3 ShotDir
	{
		get
		{
			return Camera.main.transform.forward;
		}
	}

	public float MaxRange
	{
		get
		{
			return SettingsData.MaxRange;
		}
	}

	public float EffectiveRange
	{
		get
		{
			return SettingsData.EffectiveRange;
		}
	}

	public bool IsBusy()
	{
		return Busy > TimeManager.Instance.timeSinceLevelLoad;
	}

	public void SetBusy(float busyTime)
	{
		Busy = TimeManager.Instance.timeSinceLevelLoad + busyTime;
	}

	protected bool IsFiring()
	{
		return Firing > TimeManager.Instance.timeSinceLevelLoad;
	}

	protected void SetFiring(float busyTime)
	{
		Firing = TimeManager.Instance.timeSinceLevelLoad + busyTime + 0.066f;
	}

	public float DamageByRangeRatio(float distance)
	{
		if (distance <= SettingsData.EffectiveRange)
		{
			return 1f;
		}
		if (distance >= SettingsData.MaxRange)
		{
			return 0f;
		}
		float num = Mathf.Clamp((distance - SettingsData.EffectiveRange) / (SettingsData.MaxRange - SettingsData.EffectiveRange), 0f, 1f);
		return 1f - num;
	}

	public static bool IsProperHit(RaycastHit hit, HitZone zoneHit, bool checkAlsoDead = false)
	{
		if (hit.collider.isTrigger)
		{
			return false;
		}
		GameObject gameObject = hit.collider.gameObject;
		HitZone component = gameObject.GetComponent<HitZone>();
		HitZoneEffects component2 = gameObject.GetComponent<HitZoneEffects>();
		Agent firstComponentUpward = GameObjectUtils.GetFirstComponentUpward<Agent>(gameObject);
		if ((((bool)zoneHit && zoneHit.ForPlayer) || ((bool)component && component.ForPlayer) || ((bool)component2 && component2.ForPlayer)) && (firstComponentUpward == null || ((firstComponentUpward.IsAlive || checkAlsoDead) && !firstComponentUpward.IsPlayer)))
		{
			return true;
		}
		return false;
	}

	private static float GetDistance(Ray ray, HitUtils.HitData hitData)
	{
		return (ray.origin - hitData.hitPos).magnitude;
	}

	protected bool GetAimTarget(Ray ray, out HitUtils.HitData hitData, out bool hitOnlyWithAimAssistent)
	{
		bool[] array = new bool[3];
		HitUtils.HitData[] array2 = new HitUtils.HitData[3];
		array[0] = HitUtils.FirstCollisionOnRay(ray, 40f, Player.Instance.Owner.GameObject, IsProperHit, out array2[0]);
		array[1] = HitUtils.SphereCollisionOnRayAccurate(ray, 13f, 0.4f, Player.Instance.Owner.GameObject, IsProperHit, out array2[1], 0f);
		if (!array[1])
		{
			array[2] = HitUtils.SphereCollisionOnRayAccurate(ray, 26f, 0.52f, Player.Instance.Owner.GameObject, IsProperHit, out array2[2], 12.9f);
		}
		int num = 0;
		float num2 = 1000000f;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array[i])
			{
				float distance = GetDistance(ray, array2[i]);
				if (num2 > distance)
				{
					num = i;
					num2 = distance;
				}
			}
		}
		hitData = array2[num];
		hitOnlyWithAimAssistent = num > 0;
		return array[num];
	}

	public virtual Vector3 ComputeAimAssistDir(out bool targetFound, out HitUtils.HitData hitData)
	{
		Ray ray = new Ray(ShotPos, ShotDir);
		bool hitOnlyWithAimAssistent;
		Vector3 result;
		if (GetAimTarget(ray, out hitData, out hitOnlyWithAimAssistent))
		{
			result = hitData.hitPos - ShotPos;
			targetFound = true;
		}
		else
		{
			result = ShotDir;
			targetFound = false;
		}
		return result;
	}

	public virtual void Initialize(E_WeaponID id, E_UpgradeLevel upgrade, float ammoModificator)
	{
		Settings = WeaponSettingsManager.Instance.Get(id);
		SettingsData = Settings.Upgrades[(int)upgrade];
		AmmoModificator = ammoModificator;
		_AmmoInClip = MaxAmmoInClip;
		_AmmoInWeapon = MaxAmmoInWeapon;
		GameObject = base.gameObject;
		Audio = base.GetComponent<AudioSource>();
		Transform = GameObject.transform;
		Animation = base.GetComponent<Animation>();
		InitProjSettings = new ProjectileInitSettings();
		InitProjSettings.Weapon = Settings.ID;
		InitProjSettings.WeaponType = Settings.WeaponType;
		InitProjSettings.Speed = SettingsData.Speed;
		InitProjSettings.Damage = SettingsData.Damage;
		InitProjSettings.BodyPartDamageModif = SettingsData.BodyPartDamageModif;
		InitProjSettings.Impuls = SettingsData.Impuls;
		InitProjSettings.EffectiveRange = SettingsData.EffectiveRange;
		InitProjSettings.MaxRange = SettingsData.MaxRange;
		InitProjSettings.MaxRangeDamage = SettingsData.MaxRangeDamage;
		if ((bool)WeaponAnimsSetup.Run)
		{
			Animation.AddClip(WeaponAnimsSetup.Run, WeaponAnimsSetup.Run.name);
		}
		if ((bool)WeaponAnimsSetup.Walk)
		{
			Animation.AddClip(WeaponAnimsSetup.Walk, WeaponAnimsSetup.Walk.name);
		}
		if ((bool)WeaponAnimsSetup.Idle)
		{
			Animation.AddClip(WeaponAnimsSetup.Idle, WeaponAnimsSetup.Idle.name);
		}
		if ((bool)WeaponAnimsSetup.Fire)
		{
			Animation.AddClip(WeaponAnimsSetup.Fire, WeaponAnimsSetup.Fire.name);
		}
		if ((bool)WeaponAnimsSetup.Reload)
		{
			Animation.AddClip(WeaponAnimsSetup.Reload, WeaponAnimsSetup.Reload.name);
		}
		if ((bool)WeaponAnimsSetup.Arm)
		{
			Animation.AddClip(WeaponAnimsSetup.Arm, WeaponAnimsSetup.Arm.name);
		}
		if ((bool)WeaponAnimsSetup.Disarm)
		{
			Animation.AddClip(WeaponAnimsSetup.Disarm, WeaponAnimsSetup.Disarm.name);
		}
		if (DeviceInfo.PerformanceGrade != DeviceInfo.Performance.High && DeviceInfo.PerformanceGrade != DeviceInfo.Performance.UltraHigh)
		{
			Shells = null;
		}
	}

	private void OnDestroy()
	{
		SoundsSetup = null;
		Muzzle = null;
		Shells = null;
	}

	public virtual void Reset()
	{
		Busy = 0f;
		Firing = 0f;
		_AmmoInClip = MaxAmmoInClip;
		_AmmoInWeapon = MaxAmmoInWeapon;
		ShowWeapon(false);
	}

	public virtual void WeaponShow(Transform linkTo, bool relink = true)
	{
		if (relink)
		{
			Transform.parent = linkTo;
			Transform.localPosition = Vector3.zero;
			Transform.localRotation = Quaternion.identity;
		}
		ShowWeapon(true);
	}

	public virtual void WeaponHide(bool unlink = true)
	{
		if (unlink)
		{
			Transform.parent = null;
			Transform.localPosition = Vector3.zero;
			Transform.localRotation = Quaternion.identity;
		}
		ShowWeapon(false);
	}

	public virtual void AddToOwner(AgentHuman owner)
	{
		Owner = owner;
		TimeReload = PlayerAnimsSetup.Reload.length;
		TimeArm = PlayerAnimsSetup.Arm.length;
		TimeDisarm = PlayerAnimsSetup.Disarm.length;
		Owner.Animation.AddClip(PlayerAnimsSetup.Run, PlayerAnimsSetup.Run.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Walk, PlayerAnimsSetup.Walk.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Idle, PlayerAnimsSetup.Idle.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Fire, PlayerAnimsSetup.Fire.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Reload, PlayerAnimsSetup.Reload.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Arm, PlayerAnimsSetup.Arm.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Disarm, PlayerAnimsSetup.Disarm.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.InjuryL, PlayerAnimsSetup.InjuryL.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.InjuryR, PlayerAnimsSetup.InjuryR.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.InjuryFrontL, PlayerAnimsSetup.InjuryFrontL.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.InjuryFrontR, PlayerAnimsSetup.InjuryFrontR.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.Death, PlayerAnimsSetup.Death.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.StrafeL, PlayerAnimsSetup.StrafeL.name);
		Owner.Animation.AddClip(PlayerAnimsSetup.StrafeR, PlayerAnimsSetup.StrafeR.name);
		if ((bool)PlayerAnimsSetup.AimFire)
		{
			Owner.Animation.AddClip(PlayerAnimsSetup.AimFire, PlayerAnimsSetup.AimFire.name);
		}
		if ((bool)PlayerAnimsSetup.AimIdle)
		{
			Owner.Animation.AddClip(PlayerAnimsSetup.AimIdle, PlayerAnimsSetup.AimIdle.name);
		}
		if ((bool)PlayerAnimsSetup.AimWalk)
		{
			Owner.Animation.AddClip(PlayerAnimsSetup.AimWalk, PlayerAnimsSetup.AimWalk.name);
		}
	}

	public virtual void RemoveFromOwner()
	{
		if (Owner.Animation.Contains(PlayerAnimsSetup.Run))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Run);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Walk))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Walk);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Idle))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Idle);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Fire))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Fire);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Reload))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Reload);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Arm))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Arm);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Disarm))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Disarm);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.InjuryL))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.InjuryL);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.InjuryR))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.InjuryR);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.InjuryFrontL))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.InjuryFrontL);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.InjuryFrontR))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.InjuryFrontR);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.Death))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.Death);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.StrafeL))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.StrafeL);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.StrafeR))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.StrafeR);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.AimFire))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.AimFire);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.AimIdle))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.AimIdle);
		}
		if (Owner.Animation.Contains(PlayerAnimsSetup.AimWalk))
		{
			Owner.Animation.RemoveClip(PlayerAnimsSetup.AimWalk);
		}
		Owner = null;
	}

	public virtual bool FireStartupDone()
	{
		return true;
	}

	public virtual void Fire()
	{
		if (ClipAmmo == 0 || IsBusy())
		{
			return;
		}
		PlaySoundFire();
		SpawnProjectile();
		if ((bool)Muzzle)
		{
			Muzzle.transform.localEulerAngles = new Vector3(Muzzle.transform.localEulerAngles.x, Muzzle.transform.localEulerAngles.y, UnityEngine.Random.Range(0, 50));
			Muzzle._SetActiveRecursively(true);
		}
		if ((bool)Shells)
		{
			Shells.Emit(1);
		}
		if ((bool)WeaponAnimsSetup.Fire)
		{
			string text = WeaponAnimsSetup.Fire.name;
			if (Animation.IsPlaying(text))
			{
				Animation.Stop(text);
			}
			Animation.Play(text);
		}
		DecreaseAmmo();
		SetBusy(SettingsData.FireTime);
		SetFiring(SettingsData.FireTime);
		if (Mission.Instance.CurrentGameZone != null && Owner.IsPlayer)
		{
			Mission.Instance.CurrentGameZone.SendFactToEnemies(Owner, Owner, E_EventTypes.EnemyFire, 30f, 10f, false);
		}
		if (Owner.IsPlayer)
		{
			Game.Instance.PlayerPersistentInfo.AddWeaponUse(Settings.ID);
		}
	}

	private void ReloadClip()
	{
		if (MaxAmmoInWeapon == -1)
		{
			_AmmoInClip = SettingsData.MaxAmmoInClip;
		}
		else if (_AmmoInWeapon > 0)
		{
			int num = Mathf.Min(SettingsData.MaxAmmoInClip - _AmmoInClip, _AmmoInWeapon);
			_AmmoInClip += num;
			_AmmoInWeapon -= num;
		}
	}

	public virtual void Reload()
	{
		Owner.SoundPlay(SoundsSetup.Reload);
		ReloadClip();
		if ((bool)WeaponAnimsSetup.Reload)
		{
			Animation.Play(WeaponAnimsSetup.Reload.name);
		}
		SetBusy(TimeReload);
	}

	public virtual void WeaponArm()
	{
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		if ((bool)WeaponAnimsSetup.Arm)
		{
			Animation.Play(WeaponAnimsSetup.Arm.name);
		}
		Owner.SoundPlay(SoundsSetup.Arm);
		SetBusy(TimeArm / num);
	}

	public virtual void WeaponDisArm()
	{
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		if ((bool)WeaponAnimsSetup.Disarm)
		{
			Animation.Play(WeaponAnimsSetup.Disarm.name);
		}
		Owner.SoundPlay(SoundsSetup.Disarm);
		SetBusy(TimeArm / num);
	}

	public virtual void DecreaseAmmo()
	{
		_AmmoInClip--;
	}

	public virtual void AddAmmo(int ammo)
	{
		if (ammo == -1)
		{
			_AmmoInClip = MaxAmmoInClip;
			_AmmoInWeapon = MaxAmmoInWeapon;
			return;
		}
		if (MaxAmmoInWeapon <= 0)
		{
			if (_AmmoInClip + ammo > MaxAmmoInClip)
			{
				_AmmoInClip = MaxAmmoInClip;
			}
			else
			{
				_AmmoInClip += ammo;
			}
		}
		else if (_AmmoInWeapon + ammo > MaxAmmoInWeapon)
		{
			_AmmoInWeapon = MaxAmmoInWeapon;
		}
		else
		{
			_AmmoInWeapon += ammo;
		}
		if (Owner.IsPlayer && Player.Instance.Owner.WeaponComponent.CurrentWeapon != WeaponID)
		{
			ReloadClip();
		}
	}

	public void SetAmmo(int ammoInClip, int ammoInWeapon)
	{
		_AmmoInClip = ammoInClip;
		_AmmoInWeapon = ammoInWeapon;
	}

	protected virtual IEnumerator UpdateFireEffect()
	{
		while (true)
		{
			if ((bool)Muzzle && Muzzle.activeInHierarchy)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.05f));
				Muzzle._SetActiveRecursively(false);
			}
			yield return new WaitForEndOfFrame();
			if ((bool)Shells && Shells.isPlaying)
			{
				yield return new WaitForEndOfFrame();
				Shells.Stop();
			}
		}
	}

	protected virtual Vector3 ShotDirWithDispersion(Vector3 baseDir)
	{
		return MathUtils.RandomVectorInsideCone(baseDir, FinalDispersion);
	}

	protected virtual void SpawnProjectile()
	{
		InitProjSettings.Agent = Owner;
		bool targetFound;
		HitUtils.HitData hitData;
		Vector3 baseDir = ComputeAimAssistDir(out targetFound, out hitData);
		ProjectileManager.Instance.SpawnProjectile(Settings.ProjectileType, ShotPos, ShotDirWithDispersion(baseDir), InitProjSettings);
	}

	protected virtual void PlaySoundFire()
	{
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		if (soundFire.Length <= 0)
		{
			return;
		}
		AudioClip audioClip = soundFire[UnityEngine.Random.Range(0, soundFire.Length)];
		Audio.loop = false;
		if (Audio.isPlaying)
		{
			Audio.Stop();
			if (Audio.clip != audioClip)
			{
				Audio.clip = audioClip;
			}
			Audio.Play();
		}
		else
		{
			Audio.clip = audioClip;
			Audio.Play();
		}
	}

	private void ShowWeapon(bool show)
	{
		GameObject._SetActiveRecursively(show);
		if ((bool)Muzzle)
		{
			Muzzle._SetActiveRecursively(false);
		}
		if (show)
		{
			StartCoroutine(UpdateFireEffect());
		}
		else
		{
			StopCoroutine("UpdateFireEffect");
		}
		if ((bool)Shells)
		{
			if (show)
			{
				Shells.Play();
			}
			else
			{
				Shells.Stop();
			}
		}
	}

	protected void LateUpdate()
	{
		if (Mathf.Approximately(Time.deltaTime, 0f))
		{
			return;
		}
		if (UpdateAnimations > 0)
		{
			float speed = ((UpdateAnimations > 1) ? (TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime) : 1f);
			if ((bool)Animation)
			{
				foreach (AnimationState item in Animation)
				{
					item.speed = speed;
				}
			}
			if ((bool)Owner && (bool)Owner.Animation)
			{
				foreach (AnimationState item2 in Owner.Animation)
				{
					item2.speed = speed;
				}
			}
		}
		if (Mathf.Abs(TimeManager.Instance.GetRealDeltaTime() - Time.deltaTime) > 0.001f)
		{
			UpdateAnimations = 2;
		}
		else if (UpdateAnimations > 0)
		{
			UpdateAnimations--;
		}
	}

	public bool CanAim()
	{
		return Settings.CanAim;
	}

	public float GetIronSightFov()
	{
		return Settings.AimFov;
	}

	public bool IronSightCrosshair()
	{
		return Settings.AimCross;
	}
}
