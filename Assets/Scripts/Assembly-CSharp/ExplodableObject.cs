using UnityEngine;

public class ExplodableObject : MonoBehaviour, IGameZoneControledObject, IHitZoneOwner
{
	private float m_DefaultHitPoints;

	public float m_HitPoints = 100f;

	public Explosion m_Explosion;

	public Transform m_ExplosionOrigin;

	public float m_ExplosionDamage;

	public GameObject[] m_HideObjects;

	public GameObject[] m_ShowObjects;

	private GameObject m_GameObject;

	private void Awake()
	{
		m_GameObject = base.gameObject;
		m_DefaultHitPoints = m_HitPoints;
		m_ExplosionOrigin = ((!(m_ExplosionOrigin != null)) ? base.transform : m_ExplosionOrigin);
		GameZone firstComponentUpward = base.gameObject.GetFirstComponentUpward<GameZone>();
		firstComponentUpward.RegisterControllableObject(this);
	}

	private void OnDestroy()
	{
		m_Explosion = null;
	}

	public void OnHitZoneProjectileHit(HitZone zone, Projectile projectile)
	{
		if (!(m_HitPoints <= 0f))
		{
			m_HitPoints -= projectile.Damage() * zone.DamageModifier;
			if (m_HitPoints <= 0f)
			{
				Break();
			}
			projectile.hitProcessed = true;
		}
	}

	public void OnHitZoneRangeDamage(HitZone Zone, Agent Attacker, float Damage, Vector3 Impulse, E_WeaponID weaponID, E_WeaponType WeaponType)
	{
		if (!(m_HitPoints <= 0f))
		{
			m_HitPoints -= Damage * Zone.DamageModifier;
			if (m_HitPoints <= 0f)
			{
				Break();
			}
		}
	}

	public void OnHitZoneMeleeDamage(HitZone Zone, MeleeDamageData Data)
	{
	}

	public virtual void Reset()
	{
		m_HitPoints = m_DefaultHitPoints;
		GameObject[] hideObjects = m_HideObjects;
		foreach (GameObject gameObject in hideObjects)
		{
			gameObject.SetActive(true);
		}
		GameObject[] showObjects = m_ShowObjects;
		foreach (GameObject gameObject2 in showObjects)
		{
			gameObject2.SetActive(false);
		}
	}

	public void Enable()
	{
		m_GameObject._SetActiveRecursively(true);
		GameObject[] showObjects = m_ShowObjects;
		foreach (GameObject gameObject in showObjects)
		{
			gameObject.SetActive(false);
		}
	}

	public void Disable()
	{
	}

	protected virtual void Break()
	{
		if (m_Explosion != null)
		{
			Explosion explosion = Mission.Instance.ExplosionCache.Get(m_Explosion, m_ExplosionOrigin.position, m_ExplosionOrigin.rotation);
			if (explosion != null)
			{
				if (m_ShowObjects.Length > 0)
				{
					Transform[] array = new Transform[m_ShowObjects.Length];
					for (int i = 0; i < m_ShowObjects.Length; i++)
					{
						array[i] = m_ShowObjects[i].transform;
					}
					explosion.noBlocking = array;
				}
				if (m_ExplosionDamage > 0f)
				{
					explosion.damage = m_ExplosionDamage;
				}
				else if (m_ExplosionDamage < 0f)
				{
					explosion.damage = (0f - m_ExplosionDamage) * (float)GameplayData.Instance.EnemyHealth();
				}
			}
		}
		GameObject[] hideObjects = m_HideObjects;
		foreach (GameObject gameObject in hideObjects)
		{
			gameObject.SetActive(false);
		}
		GameObject[] showObjects = m_ShowObjects;
		foreach (GameObject gameObject2 in showObjects)
		{
			gameObject2.SetActive(true);
		}
	}
}
