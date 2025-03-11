using UnityEngine;

[AddComponentMenu("Weapons/Shotgun")]
public class WeaponShotgun : WeaponBase
{
	public int ProjectilesPerShot = 8;

	public override void Initialize(E_WeaponID id, E_UpgradeLevel upgrade, float ammoModifier)
	{
		base.Initialize(id, upgrade, ammoModifier);
		InitProjSettings.Damage /= ProjectilesPerShot;
		InitProjSettings.MaxRangeDamage /= ProjectilesPerShot;
	}

	protected override void SpawnProjectile()
	{
		InitProjSettings.Agent = Owner;
		bool targetFound;
		HitUtils.HitData hitData;
		Vector3 vector = ComputeAimAssistDir(out targetFound, out hitData);
		for (int i = 0; i < ProjectilesPerShot; i++)
		{
			if (i == 0)
			{
				ProjectileManager.Instance.SpawnProjectile(Settings.ProjectileType, base.ShotPos, vector, InitProjSettings);
			}
			else
			{
				ProjectileManager.Instance.SpawnProjectile(Settings.ProjectileType, base.ShotPos, ShotDirWithDispersion(vector), InitProjSettings);
			}
		}
	}
}
