using UnityEngine;

public interface IHitZoneOwner
{
	void OnHitZoneProjectileHit(HitZone zone, Projectile projectile);

	void OnHitZoneRangeDamage(HitZone zone, Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType);

	void OnHitZoneMeleeDamage(HitZone zone, MeleeDamageData Data);
}
