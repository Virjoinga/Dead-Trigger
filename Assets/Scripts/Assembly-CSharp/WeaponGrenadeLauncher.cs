using UnityEngine;

[AddComponentMenu("Weapons/GrenadeLauncher")]
public class WeaponGrenadeLauncher : WeaponBase
{
	protected override void SpawnProjectile()
	{
		InitProjSettings.Agent = Owner;
		bool targetFound;
		HitUtils.HitData hitData;
		ComputeAimAssistDir(out targetFound, out hitData);
		float num = Mathf.Clamp(hitData.distance / 8f, 0f, 1f);
		ProjectileManager.Instance.SpawnProjectile(Settings.ProjectileType, base.ShotPos + base.ShotDir * 0.5f - Camera.main.transform.up * 0.1f * num, ShotDirWithDispersion((Camera.main.transform.forward + Camera.main.transform.up * 0.22f * num).normalized), InitProjSettings);
	}
}
