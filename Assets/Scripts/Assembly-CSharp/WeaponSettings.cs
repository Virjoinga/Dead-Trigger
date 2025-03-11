using System;
using UnityEngine;

[AddComponentMenu("Weapons/WeaponSettings")]
public class WeaponSettings : Settings<E_WeaponID>
{
	public enum E_FireType
	{
		Once = 0,
		Auto = 1
	}

	[Serializable]
	public class Data
	{
		public int MoneyCost;

		public int GoldCost;

		public int MaxAmmoInClip;

		public int MaxAmmoInWeapon;

		public float CriticalAmmo;

		public float FireTime;

		public float Damage;

		public float BodyPartDamageModif;

		public float MaxRangeDamage;

		public float EffectiveRange;

		public float MaxRange;

		public float Speed;

		public float Impuls;

		public float Dispersion;

		public float DispersionAddMove;

		public float DispersionAddShoot;

		public int UpgradeRank;
	}

	public E_WeaponType WeaponType;

	public E_ProjectileType ProjectileType;

	public E_FireType FireType;

	public bool AimCross = true;

	public bool CanAim = true;

	public float AimFov = 50f;

	public Data[] Upgrades = new Data[2]
	{
		new Data(),
		new Data()
	};

	public int MoneyCost
	{
		get
		{
			return Upgrades[0].MoneyCost;
		}
	}

	public int GoldCost
	{
		get
		{
			return Upgrades[0].GoldCost;
		}
	}

	public int UpgradeMoneyCost
	{
		get
		{
			return Upgrades[1].MoneyCost;
		}
	}

	public int UpgradeGoldCost
	{
		get
		{
			return Upgrades[1].GoldCost;
		}
	}

	public override string GetSettingsClass()
	{
		return "weapon";
	}
}
