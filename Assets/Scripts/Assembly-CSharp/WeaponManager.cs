using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Weapons/WeaponManager")]
internal class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance;

	private Dictionary<E_WeaponID, ResourceCache> WeaponsCache = new Dictionary<E_WeaponID, ResourceCache>();

	private Dictionary<E_WeaponID, ResourceCache> AmmoCache = new Dictionary<E_WeaponID, ResourceCache>();

	private void Start()
	{
		Instance = this;
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Colt1911))
		{
			WeaponsCache[E_WeaponID.Colt1911] = new ResourceCache("Weapons/WeaponColt1911", 1);
			AmmoCache[E_WeaponID.Colt1911] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.M4))
		{
			WeaponsCache[E_WeaponID.M4] = new ResourceCache("Weapons/WeaponM4", 1);
			AmmoCache[E_WeaponID.M4] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Striker))
		{
			WeaponsCache[E_WeaponID.Striker] = new ResourceCache("Weapons/WeaponStriker", 1);
			AmmoCache[E_WeaponID.Striker] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.KSG))
		{
			WeaponsCache[E_WeaponID.KSG] = new ResourceCache("Weapons/WeaponKSG", 1);
			AmmoCache[E_WeaponID.KSG] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Remington870))
		{
			WeaponsCache[E_WeaponID.Remington870] = new ResourceCache("Weapons/WeaponRemington870", 1);
			AmmoCache[E_WeaponID.Remington870] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.RemingtonTactics))
		{
			WeaponsCache[E_WeaponID.RemingtonTactics] = new ResourceCache("Weapons/WeaponRemingtonTactics", 1);
			AmmoCache[E_WeaponID.RemingtonTactics] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Uzi))
		{
			WeaponsCache[E_WeaponID.Uzi] = new ResourceCache("Weapons/WeaponUzi", 1);
			AmmoCache[E_WeaponID.Uzi] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Minigun))
		{
			WeaponsCache[E_WeaponID.Minigun] = new ResourceCache("Weapons/WeaponMinigun", 1);
			AmmoCache[E_WeaponID.Minigun] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.WaltherP99))
		{
			WeaponsCache[E_WeaponID.WaltherP99] = new ResourceCache("Weapons/WeaponWaltherP99", 1);
			AmmoCache[E_WeaponID.WaltherP99] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Scorpion))
		{
			WeaponsCache[E_WeaponID.Scorpion] = new ResourceCache("Weapons/WeaponScorpion", 1);
			AmmoCache[E_WeaponID.Scorpion] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.P90))
		{
			WeaponsCache[E_WeaponID.P90] = new ResourceCache("Weapons/WeaponP90", 1);
			AmmoCache[E_WeaponID.P90] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Bren))
		{
			WeaponsCache[E_WeaponID.Bren] = new ResourceCache("Weapons/WeaponBren", 1);
			AmmoCache[E_WeaponID.Bren] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.AK47))
		{
			WeaponsCache[E_WeaponID.AK47] = new ResourceCache("Weapons/WeaponAK47", 1);
			AmmoCache[E_WeaponID.AK47] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.LeeEnfield303))
		{
			WeaponsCache[E_WeaponID.LeeEnfield303] = new ResourceCache("Weapons/WeaponLeeEnfield303", 1);
			AmmoCache[E_WeaponID.LeeEnfield303] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Lupara))
		{
			WeaponsCache[E_WeaponID.Lupara] = new ResourceCache("Weapons/WeaponLupara", 1);
			AmmoCache[E_WeaponID.Lupara] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Chainsaw))
		{
			WeaponsCache[E_WeaponID.Chainsaw] = new ResourceCache("Weapons/WeaponChainsaw", 1);
			AmmoCache[E_WeaponID.Chainsaw] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Propeller))
		{
			WeaponsCache[E_WeaponID.Propeller] = new ResourceCache("Weapons/WeaponPropeller", 1);
			AmmoCache[E_WeaponID.Propeller] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Winchester))
		{
			WeaponsCache[E_WeaponID.Winchester] = new ResourceCache("Weapons/WeaponWinchester", 1);
			AmmoCache[E_WeaponID.Winchester] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Lewis))
		{
			WeaponsCache[E_WeaponID.Lewis] = new ResourceCache("Weapons/WeaponLewis", 1);
			AmmoCache[E_WeaponID.Lewis] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.AlienGun))
		{
			WeaponsCache[E_WeaponID.AlienGun] = new ResourceCache("Weapons/WeaponAlienGun", 1);
			AmmoCache[E_WeaponID.AlienGun] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.Crossbow))
		{
			WeaponsCache[E_WeaponID.Crossbow] = new ResourceCache("Weapons/WeaponCrossbow", 1);
			AmmoCache[E_WeaponID.Crossbow] = new ResourceCache("Weapons/Ammo", 1);
		}
		if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsWeapon(E_WeaponID.GrenadeLauncher))
		{
			WeaponsCache[E_WeaponID.GrenadeLauncher] = new ResourceCache("Weapons/WeaponGrenadeLauncher", 1);
			AmmoCache[E_WeaponID.GrenadeLauncher] = new ResourceCache("Weapons/Ammo", 1);
		}
	}

	public WeaponBase GetWeapon(E_WeaponID id, E_UpgradeLevel upgrade, float ammoModifier)
	{
		if (!WeaponsCache.ContainsKey(id))
		{
			Debug.LogError("WeaponManager: unknown type " + id);
			return null;
		}
		if (WeaponsCache[id] == null)
		{
			Debug.LogError(string.Concat("WeaponManager: For this type ", id, " we don't have resource"));
			return null;
		}
		GameObject gameObject = WeaponsCache[id].Get();
		if ((bool)gameObject)
		{
			WeaponBase component = gameObject.GetComponent<WeaponBase>();
			component.Initialize(id, upgrade, ammoModifier);
			return component;
		}
		return null;
	}

	public void Return(WeaponBase weapon)
	{
		if (weapon == null)
		{
			Debug.LogError("WeaponManager: sombody is trying return null object to cache");
			return;
		}
		if (!WeaponsCache.ContainsKey(weapon.WeaponID))
		{
			Debug.LogError("WeaponManager: unknown type " + weapon.WeaponType);
			return;
		}
		if (WeaponsCache[weapon.WeaponID] == null)
		{
			Debug.LogError("WeaponManager: We don't have cache for this weapon type. This object was not created by this manager");
			return;
		}
		weapon.RemoveFromOwner();
		WeaponsCache[weapon.WeaponID].Return(weapon.gameObject);
	}

	public AmmoBox GetAmmo(E_WeaponID type)
	{
		if (!AmmoCache.ContainsKey(type))
		{
			Debug.LogError("AmmoFactory: unknown type " + type);
			return null;
		}
		if (AmmoCache[type] == null)
		{
			Debug.LogError(string.Concat("AmmoFactory: For this type ", type, " we don't have resource"));
			return null;
		}
		GameObject gameObject = AmmoCache[type].Get();
		AmmoBox ammoBox = ((!gameObject) ? null : gameObject.GetComponent<AmmoBox>());
		if (ammoBox != null)
		{
			ammoBox.cacheKey = type;
		}
		return ammoBox;
	}

	public void Return(AmmoBox ammo)
	{
		if (ammo == null)
		{
			Debug.LogError("AmmoFactory: sombody is trying return null object to cache");
			return;
		}
		if (!AmmoCache.ContainsKey(ammo.cacheKey))
		{
			Debug.LogError("AmmoFactory: unknown type " + ammo.cacheKey);
			return;
		}
		if (AmmoCache[ammo.cacheKey] == null)
		{
			Debug.LogError("AmmoFactory: We don't have cache for this weapon type. This object was not created by this manager");
			return;
		}
		ammo.Reset();
		AmmoCache[ammo.cacheKey].Return(ammo.gameObject);
	}

	public void Clear()
	{
		WeaponsCache.Clear();
		AmmoCache.Clear();
	}
}
