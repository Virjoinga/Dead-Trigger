using UnityEngine;

public class HitZone : MonoBehaviour
{
	public float DamageModifier = 1f;

	public bool ForPlayer = true;

	public GameObject GameObj { get; private set; }

	public IHitZoneOwner HitZoneOwner { get; private set; }

	private void Awake()
	{
		GameObj = base.gameObject;
		HitZoneOwner = GameObj.GetFirstComponentUpwardWithInterface<IHitZoneOwner>();
	}

	public virtual void Reset()
	{
	}

	public void SetOwner(IHitZoneOwner NewOwner)
	{
		HitZoneOwner = NewOwner;
	}

	public virtual void OnProjectileHit(Projectile projectile)
	{
		if (HitZoneOwner != null)
		{
			HitZoneOwner.OnHitZoneProjectileHit(this, projectile);
		}
	}

	public virtual void OnReceiveRangeDamage(Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType)
	{
		if (HitZoneOwner != null)
		{
			HitZoneOwner.OnHitZoneRangeDamage(this, attacker, damage, impulse, weaponID, weaponType);
		}
	}

	public virtual void OnMeleeDamage(MeleeDamageData Data)
	{
		if (HitZoneOwner != null)
		{
			HitZoneOwner.OnHitZoneMeleeDamage(this, Data);
		}
	}

	public static void SetRecursivelyPlayerRelevancy(GameObject Obj, bool Enable)
	{
		HitZone[] componentsInChildren = Obj.GetComponentsInChildren<HitZone>(true);
		HitZone[] array = componentsInChildren;
		foreach (HitZone hitZone in array)
		{
			hitZone.ForPlayer = Enable;
		}
	}
}
