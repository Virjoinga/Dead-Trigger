using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ComponentWeapons : MonoBehaviour
{
	private struct MoveDispersionTimer
	{
		public float Increase;

		public float Decrease;

		public float Dispersion;

		public bool Increasing;

		public void Tick(float deltaTime)
		{
			if (Increasing && Dispersion < 1f)
			{
				Dispersion = Mathf.Clamp(Dispersion + Increase * Time.deltaTime, 0f, 1f);
			}
			else if (!Increasing && Dispersion > 0f)
			{
				if (Dispersion > 0.25f)
				{
					Dispersion = Mathf.Clamp(Dispersion - Decrease * Time.deltaTime, 0.25f, 1f);
				}
				else
				{
					Dispersion = Mathf.Clamp(Dispersion - Decrease * 0.12f * Time.deltaTime, 0f, 0.25f);
				}
			}
		}
	}

	private struct ShootDispersionTimer
	{
		public float Increase;

		public float Decrease;

		public float Dispersion;

		public float LastFireTime;

		public float FireOnInterval;

		public float DispersionHoldTime;

		public void TickDecrease(float deltaTime)
		{
			if (Dispersion > 0f)
			{
				if (Dispersion > 0.25f)
				{
					Dispersion = Mathf.Clamp(Dispersion - Decrease * Time.deltaTime, 0.25f, Decrease / 3f);
				}
				else
				{
					Dispersion = Mathf.Clamp(Dispersion - Decrease * 0.15f * Time.deltaTime, 0f, 0.25f);
				}
			}
		}

		public void TickIncrease(float deltaTime)
		{
			if (Dispersion < 1f)
			{
				Dispersion = Mathf.Clamp(Dispersion + Increase * Time.deltaTime, 0f, Decrease / 3f);
			}
		}
	}

	public Transform Hand;

	protected AgentHuman Owner;

	private MoveDispersionTimer MoveDispersion;

	private ShootDispersionTimer ShootDispersion;

	public Dictionary<E_WeaponID, WeaponBase> Weapons { get; protected set; }

	public E_WeaponID CurrentWeapon { get; protected set; }

	public WeaponBase GetCurrentWeapon()
	{
		return (CurrentWeapon == E_WeaponID.None) ? null : Weapons[CurrentWeapon];
	}

	public WeaponBase GetWeapon(E_WeaponID t)
	{
		return (!Weapons.ContainsKey(t)) ? null : Weapons[t];
	}

	private void Awake()
	{
		Owner = GetComponent<AgentHuman>();
		BlackBoard blackBoard = Owner.BlackBoard;
		blackBoard.ActionHandler = (BlackBoard.AgentActionHandler)Delegate.Combine(blackBoard.ActionHandler, new BlackBoard.AgentActionHandler(HandleAction));
		Weapons = new Dictionary<E_WeaponID, WeaponBase>();
	}

	protected virtual void Initialize()
	{
		CurrentWeapon = E_WeaponID.None;
		MoveDispersion.Increase = 5f;
		MoveDispersion.Decrease = 10f;
		MoveDispersion.Dispersion = 0f;
		MoveDispersion.Increasing = false;
		ShootDispersion.Increase = 0.4f;
		ShootDispersion.Decrease = 5f;
		ShootDispersion.Dispersion = 0f;
		ShootDispersion.LastFireTime = 0f;
		ShootDispersion.FireOnInterval = 0.1f;
		ShootDispersion.DispersionHoldTime = 0f;
	}

	protected virtual void LateUpdate()
	{
		MoveDispersion.Tick(Time.deltaTime);
		if (Time.timeSinceLevelLoad - ShootDispersion.LastFireTime <= ShootDispersion.FireOnInterval)
		{
			ShootDispersion.TickIncrease(Time.deltaTime);
		}
		else if (Time.timeSinceLevelLoad - ShootDispersion.LastFireTime > ShootDispersion.DispersionHoldTime)
		{
			ShootDispersion.TickDecrease(Time.deltaTime);
		}
		if (Owner.BlackBoard.MotionType == E_MotionType.Run || Owner.BlackBoard.MotionType == E_MotionType.Walk)
		{
			MoveDispersion.Increasing = true;
		}
		else
		{
			MoveDispersion.Increasing = false;
		}
		if (CurrentWeapon == E_WeaponID.None)
		{
			return;
		}
		WeaponBase weaponBase = Weapons[CurrentWeapon];
		weaponBase.MoveDispersion = MoveDispersion.Dispersion;
		weaponBase.ShootDispersion = Mathf.Clamp(ShootDispersion.Dispersion, 0f, 1f);
		if (weaponBase.IsBusy() || !Owner.BlackBoard.Desires.WeaponTriggerOn)
		{
			return;
		}
		if (weaponBase.ClipAmmo > 0)
		{
			if (weaponBase.FireStartupDone())
			{
				AgentActionAttack agentActionAttack = AgentActionFactory.Create(AgentActionFactory.E_Type.Attack) as AgentActionAttack;
				agentActionAttack.AttackDir = Owner.BlackBoard.FireDir;
				Owner.BlackBoard.ActionAdd(agentActionAttack);
			}
			else
			{
				HandleFire();
			}
		}
		if (!weaponBase.IsFullAuto)
		{
			Owner.BlackBoard.Desires.WeaponTriggerOn = false;
		}
	}

	protected void SetDefaultWeaponToHand(E_WeaponID weapon)
	{
		CurrentWeapon = weapon;
		WeaponBase weaponBase = Weapons[weapon];
		Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, weaponBase.ClipAmmo > 0);
		weaponBase.WeaponShow(Hand);
	}

	public void SwitchWeapons(E_WeaponID weaponID)
	{
		if (Weapons.ContainsKey(weaponID))
		{
			Weapons[CurrentWeapon].WeaponHide();
			CurrentWeapon = weaponID;
			Weapons[weaponID].WeaponShow(Hand);
			Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, Weapons[weaponID].ClipAmmo > 0);
		}
	}

	public void HandleFire()
	{
		WeaponBase weaponBase = Weapons[CurrentWeapon];
		weaponBase.Fire();
		if (ShootDispersion.Dispersion == 0f)
		{
			ShootDispersion.Increase = 5f;
		}
		else
		{
			ShootDispersion.Increase = 5f;
		}
		ShootDispersion.LastFireTime = Time.timeSinceLevelLoad;
		ShootDispersion.FireOnInterval = 0.1f;
		ShootDispersion.DispersionHoldTime = weaponBase.FireTime;
		if (weaponBase.ClipAmmo == 0)
		{
			Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, false);
		}
		if (Owner.IsPlayer)
		{
			Game.Instance.MissionResultData.WeaponUsed(CurrentWeapon, weaponBase.FireTime);
		}
	}

	public virtual void HandleAction(AgentAction action)
	{
		if (!action.IsFailed())
		{
			if (action is AgentActionAttack)
			{
				HandleFire();
			}
			else if (action is AgentActionReload)
			{
				WeaponBase weaponBase = Weapons[CurrentWeapon];
				weaponBase.Reload();
				Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, true);
			}
		}
	}

	public bool AddAmmoToWeapon(E_WeaponID WpnID, int AmmoAmount)
	{
		WeaponBase weapon = GetWeapon(WpnID);
		if (weapon != null && !weapon.IsFull)
		{
			weapon.AddAmmo(AmmoAmount);
			return true;
		}
		return false;
	}

	public void DisableCurrentWeapon(float disableTime)
	{
		if (CurrentWeapon != 0)
		{
			Weapons[CurrentWeapon].SetBusy(disableTime);
		}
	}
}
