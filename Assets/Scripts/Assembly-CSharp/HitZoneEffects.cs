using UnityEngine;

public class HitZoneEffects : HitZone
{
	public bool MustDieToDestroy;

	public float DestroyCumulativePercentage = 0.4f;

	public float DestroyBashPercentage = 0.3f;

	public ParticleSystem DestroyParticle;

	private float _cumulativeDamage;

	public float CumulativeDamage
	{
		get
		{
			return _cumulativeDamage;
		}
	}

	private void Start()
	{
		_cumulativeDamage = 0f;
	}

	public override void Reset()
	{
		base.Reset();
		_cumulativeDamage = 0f;
	}

	public override void OnProjectileHit(Projectile projectile)
	{
		bool flag = false;
		float num = ((projectile.ProjectileType == E_ProjectileType.Melee) ? 1f : DamageModifier);
		ComponentEnemy componentEnemy = ((base.HitZoneOwner == null) ? null : (base.HitZoneOwner as ComponentEnemy));
		if (componentEnemy != null && (bool)componentEnemy.Owner && componentEnemy.Owner.IsInvulnerable)
		{
			flag = true;
		}
		if (!flag)
		{
			_cumulativeDamage += projectile.Damage() * num * projectile.BodyPartDamageModif;
		}
		if (base.HitZoneOwner != null)
		{
			base.HitZoneOwner.OnHitZoneProjectileHit(this, projectile);
		}
	}

	public override void OnReceiveRangeDamage(Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType)
	{
		bool flag = false;
		ComponentEnemy componentEnemy = ((base.HitZoneOwner == null) ? null : (base.HitZoneOwner as ComponentEnemy));
		if (componentEnemy != null && (bool)componentEnemy.Owner && componentEnemy.Owner.IsInvulnerable)
		{
			flag = true;
		}
		if (!flag)
		{
			_cumulativeDamage += damage * DamageModifier;
		}
		if (base.HitZoneOwner == null)
		{
			return;
		}
		if ((bool)componentEnemy)
		{
			if (componentEnemy.GetBodyPart(this) == E_BodyPart.Body)
			{
				base.HitZoneOwner.OnHitZoneRangeDamage(this, attacker, damage, impulse, weaponID, weaponType);
			}
		}
		else
		{
			base.HitZoneOwner.OnHitZoneRangeDamage(this, attacker, damage, impulse, weaponID, weaponType);
		}
	}
}
