using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaskManager : MonoBehaviour, IHitZoneOwner
{
	private const int MaxAmount = 12;

	public float m_LifeTime = 5f;

	public float m_FadeOutDuration = 1.5f;

	public float m_Probability = 0.5f;

	private float m_ProbabilityAccum;

	public float m_HeadCollisionScale = 1.4f;

	public string[] m_HalloweenMasks;

	public string[] m_ChristmasMasks;

	private ResourceCache[] m_Caches;

	public static EnemyMaskManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private void Start()
	{
		if (Game.Instance == null || m_Probability <= 0f)
		{
			return;
		}
		string[] array = null;
		if (Game.Instance.UsePumpkins())
		{
			array = m_HalloweenMasks;
		}
		else if (Game.Instance.UseChristmasMasks())
		{
			array = m_ChristmasMasks;
		}
		if (array == null)
		{
			return;
		}
		int initSize = Mathf.CeilToInt(12f / (float)array.Length);
		List<ResourceCache> list = new List<ResourceCache>(array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = Resources.Load(array[i]) as GameObject;
			if (!(gameObject == null))
			{
				EnemyMask component = gameObject.GetComponent<EnemyMask>();
				if (!(component == null))
				{
					component.m_CacheIndex = list.Count;
					list.Add(new ResourceCache(gameObject, initSize, 0));
				}
			}
		}
		m_Caches = list.ToArray();
		m_Probability = Mathf.Clamp01(m_Probability);
	}

	public void Add(AgentHuman Enemy)
	{
		if (Enemy == null || m_Caches == null || (m_ProbabilityAccum += m_Probability) < 1f)
		{
			return;
		}
		m_ProbabilityAccum -= 1f;
		Transform transform = Enemy.Transform.FindChildByName("Head");
		if (transform == null)
		{
			return;
		}
		GameObject gameObject = null;
		int maxValue = m_Caches.Length;
		int num = MiscUtils.SysRandom.Next(0, maxValue);
		while (maxValue-- > 0)
		{
			if (m_Caches[num].GetFreeNum() > 0)
			{
				gameObject = m_Caches[num].Get();
				break;
			}
			num = (num + 1) % m_Caches.Length;
		}
		if (!(gameObject == null))
		{
			DummyCollider component = transform.GetComponent<DummyCollider>();
			if (component != null)
			{
				component.Scale(m_HeadCollisionScale);
			}
			Enemy.Mask = gameObject;
			gameObject.transform.parent = transform.transform;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
			gameObject.GetComponent<Rigidbody>().useGravity = false;
			gameObject.GetComponent<Collider>().enabled = false;
			gameObject.GetComponent<Renderer>().probeAnchor = Enemy.TransformTarget;
			gameObject.SetActive(true);
			if (Enemy.TransformEye != null)
			{
				Enemy.TransformEyeOriginal = Enemy.TransformEye;
				Enemy.TransformEyeOriginal.gameObject.SetActive(false);
			}
			Enemy.TransformEye = gameObject.transform.Find("eye");
			HitZone component2 = gameObject.GetComponent<HitZone>();
			if (component2 != null)
			{
				component2.ForPlayer = false;
				component2.SetOwner(Enemy.EnemyComponent);
			}
		}
	}

	public bool Drop(AgentHuman Enemy, Vector3 Impulse)
	{
		GameObject mask = Enemy.Mask;
		if (mask == null)
		{
			return false;
		}
		DummyCollider component = mask.transform.parent.GetComponent<DummyCollider>();
		if (component != null)
		{
			component.Scale(1f / m_HeadCollisionScale);
		}
		Enemy.Mask = null;
		mask.transform.parent = null;
		mask.GetComponent<Collider>().enabled = true;
		mask.GetComponent<Rigidbody>().isKinematic = false;
		mask.GetComponent<Rigidbody>().useGravity = true;
		mask.GetComponent<Rigidbody>().velocity = mask.GetComponent<Rigidbody>().velocity + 3f * Impulse;
		mask.GetComponent<Rigidbody>().angularVelocity = mask.GetComponent<Rigidbody>().angularVelocity + 6f * Random.insideUnitSphere;
		mask.GetComponent<Renderer>().probeAnchor = null;
		Enemy.TransformEye = Enemy.TransformEyeOriginal;
		Enemy.TransformEye.gameObject.SetActive(false);
		HitZone component2 = mask.GetComponent<HitZone>();
		if (component2 != null)
		{
			component2.SetOwner(this);
		}
		StartCoroutine(RemoveMask(mask));
		return true;
	}

	private IEnumerator RemoveMask(GameObject MaskObj)
	{
		yield return new WaitForSeconds(m_LifeTime);
		EnemyMask maskComp = MaskObj.GetComponent<EnemyMask>();
		if (maskComp.m_FadeOutMaterial != null && m_FadeOutDuration > 0f)
		{
			Renderer maskRenderer = MaskObj.GetComponent<Renderer>();
			Material maskOrigMat = maskRenderer.material;
			maskRenderer.material = maskComp.m_FadeOutMaterial;
			maskRenderer.material.SetFloat("_TimeOffs", 0f - Time.time);
			maskRenderer.material.SetFloat("_Invert", 0f);
			maskRenderer.material.SetFloat("_Duration", m_FadeOutDuration);
			yield return new WaitForSeconds(m_FadeOutDuration);
			maskRenderer.material = maskOrigMat;
		}
		MaskObj.SetActive(false);
		m_Caches[maskComp.m_CacheIndex].Return(MaskObj);
	}

	public void OnHitZoneProjectileHit(HitZone zone, Projectile projectile)
	{
		GameObject gameObject = zone.gameObject;
		gameObject.GetComponent<Rigidbody>().velocity = 1.5f * gameObject.GetComponent<Rigidbody>().velocity + projectile.transform.forward;
	}

	public void OnHitZoneRangeDamage(HitZone zone, Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType)
	{
	}

	public void OnHitZoneMeleeDamage(HitZone zone, MeleeDamageData Data)
	{
	}
}
