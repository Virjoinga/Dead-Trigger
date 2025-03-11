using System;
using System.Collections.Generic;

[Serializable]
public class ComponentWeaponsPlayer : ComponentWeapons
{
	private void Activate(SpawnPoint spawn)
	{
	}

	protected override void Initialize()
	{
		base.Initialize();
		PPIEquipList equipList = Game.Instance.PlayerPersistentInfo.EquipList;
		List<E_WeaponID> list = new List<E_WeaponID>();
		float num = 1f;
		if (Game.Instance.PlayerPersistentInfo.Upgrades.Upgrades.Find((PPIUpgradeList.UpgradeData ps) => ps.ID == E_UpgradeID.Ammo1) != null)
		{
			num += 0.25f;
		}
		if (Game.Instance.PlayerPersistentInfo.Upgrades.Upgrades.Find((PPIUpgradeList.UpgradeData ps) => ps.ID == E_UpgradeID.AmmoExclusive) != null)
		{
			num += 0.25f;
		}
		foreach (PPIWeaponData weapon2 in equipList.Weapons)
		{
			if (!base.Weapons.ContainsKey(weapon2.ID))
			{
				WeaponBase weapon = WeaponManager.Instance.GetWeapon(weapon2.ID, weapon2.UpgradeLevel, num);
				base.Weapons.Add(weapon2.ID, weapon);
				weapon.AddToOwner(Owner);
				if (base.CurrentWeapon == E_WeaponID.None)
				{
					base.CurrentWeapon = weapon2.ID;
				}
				list.Add(weapon2.ID);
			}
		}
		GuiHUD.Instance.SetWeapons(list);
		SetDefaultWeaponToHand(base.CurrentWeapon);
		Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, GetCurrentWeapon().ClipAmmo > 0);
		GuiHUD.Instance.OnCurrentWeaponChanged(base.CurrentWeapon);
	}

	protected void DbgInitialize()
	{
		base.Initialize();
		base.CurrentWeapon = E_WeaponID.None;
		List<E_WeaponID> list = new List<E_WeaponID>();
		foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in base.Weapons)
		{
			weapon.Value.AddToOwner(Owner);
			if (base.CurrentWeapon == E_WeaponID.None)
			{
				base.CurrentWeapon = weapon.Key;
			}
			list.Add(weapon.Key);
		}
		GuiHUD.Instance.SetWeapons(list);
		SetDefaultWeaponToHand(base.CurrentWeapon);
		Owner.WorldState.SetWSProperty(E_PropKey.WeaponLoaded, GetCurrentWeapon().ClipAmmo > 0);
		GuiHUD.Instance.OnCurrentWeaponChanged(base.CurrentWeapon);
	}

	private void Deactivate()
	{
		foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in base.Weapons)
		{
			WeaponManager.Instance.Return(weapon.Value);
		}
		base.Weapons.Clear();
		base.CurrentWeapon = E_WeaponID.None;
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (base.CurrentWeapon == E_WeaponID.None)
		{
			return;
		}
		WeaponBase weaponBase = base.Weapons[base.CurrentWeapon];
		if (weaponBase.WeaponAmmo + weaponBase.ClipAmmo != 0 || weaponBase.IsBusy())
		{
			return;
		}
		WeaponBase weaponBase2 = null;
		int num = 0;
		foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in base.Weapons)
		{
			WeaponBase value = weapon.Value;
			if (value.WeaponAmmo + value.ClipAmmo > num)
			{
				num = value.WeaponAmmo + value.ClipAmmo;
				weaponBase2 = value;
			}
		}
		if (weaponBase2 != null && Player.Instance.CanChangeWeapon())
		{
			Player.Instance.Controls.ChangeWeaponDelegate(weaponBase2.WeaponID);
			GuiHUD.Instance.OnCurrentWeaponChanged(weaponBase2.WeaponID);
		}
	}
}
