#define DEBUG
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
	[SerializeField]
	private float m_DamageRadius = 5f;

	[SerializeField]
	private float m_MaxDamageRadius;

	[SerializeField]
	private float m_MaxDamage = 5f;

	[SerializeField]
	private E_WeaponType m_WeaponType = E_WeaponType.Explosion;

	public bool m_GenerateWaveFX;

	public float m_WaveFXDelay;

	public float m_WaveFXAmplitudeMin = 1f;

	public float m_WaveFXAmplitudeMax = 1f;

	public float m_WaveFXFreq = 20f;

	public float m_WaveFXDuration = 1.5f;

	public float m_WaveFXRadiusMin = 0.1f;

	public float m_WaveFXRadiusMax = 1f;

	public float m_WaveFXSpeed = 1.3f;

	public float m_WaveFXMaxWrldDist = 30f;

	public float m_ParticleCriticalDistance = 5f;

	private ParticleSystem[] m_Emitters;

	private GameObject m_GameObject;

	private bool m_Exploded;

	public Agent causer { get; set; }

	public float damage { get; set; }

	public float damageRadius { get; set; }

	public float damageMaxRadius { get; set; }

	public Transform[] noBlocking { get; set; }

	public E_WeaponID FromWeapon { get; set; }

	public Explosion cacheKey { get; set; }

	private void Awake()
	{
		m_GameObject = base.gameObject;
		m_Emitters = m_GameObject.GetComponentsInChildren<ParticleSystem>();
		damage = m_MaxDamage;
		damageRadius = m_DamageRadius;
		damageMaxRadius = m_MaxDamageRadius;
	}

	private void OnDestroy()
	{
		m_Emitters = null;
	}

	private void Update()
	{
		if (!m_Exploded)
		{
			Explode();
			m_Exploded = true;
			return;
		}
		bool flag = false;
		ParticleSystem[] emitters = m_Emitters;
		foreach (ParticleSystem particleSystem in emitters)
		{
			if (particleSystem.isPlaying || particleSystem.IsAlive())
			{
				flag = true;
				break;
			}
		}
		if (!flag && !base.GetComponent<AudioSource>().isPlaying)
		{
			CancelInvoke("HACK_CancelEffects");
			CleanUp();
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDrawGizmos()
	{
	}

	private void HACK_CancelEffects()
	{
		ParticleSystem[] emitters = m_Emitters;
		foreach (ParticleSystem particleSystem in emitters)
		{
			particleSystem.Stop();
		}
	}

	private void HACK_StopEmittersOnEnd()
	{
		float num = 0f;
		ParticleSystem[] emitters = m_Emitters;
		foreach (ParticleSystem particleSystem in emitters)
		{
			if (particleSystem.duration > num)
			{
				num = particleSystem.duration;
			}
		}
		num += 0.02f;
		CancelInvoke("HACK_CancelEffects");
		Invoke("HACK_CancelEffects", num);
	}

	public void Explode()
	{
		DebugUtils.Assert(!m_Exploded);
		HACK_StopEmittersOnEnd();
		ParticleSystem[] emitters = m_Emitters;
		foreach (ParticleSystem particleSystem in emitters)
		{
			particleSystem.Play();
		}
		if (base.GetComponent<AudioSource>() != null && base.GetComponent<AudioSource>().clip != null)
		{
			base.GetComponent<AudioSource>().Play();
		}
		if (m_GenerateWaveFX && (bool)CamExplosionFXMgr.Instance)
		{
			GenerateWaveFX();
		}
		if (damageRadius > 0f && damage > 0f)
		{
			ApplyDamage();
		}
	}

	public void CleanUp()
	{
		if (cacheKey != null)
		{
			Mission.Instance.ExplosionCache.Return(this);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Reset()
	{
		ParticleSystem[] emitters = m_Emitters;
		foreach (ParticleSystem particleSystem in emitters)
		{
			particleSystem.Stop();
		}
		base.GetComponent<AudioSource>().Stop();
		m_Exploded = false;
		causer = null;
		noBlocking = null;
		damage = m_MaxDamage;
		damageRadius = m_DamageRadius;
		damageMaxRadius = m_MaxDamageRadius;
	}

	private void ApplyDamage()
	{
		DebugUtils.Assert(damageMaxRadius <= damageRadius);
		Vector3 vector = base.transform.position + new Vector3(0f, 0.5f, 0f);
		int layerMask = ~ObjectLayerMask.EnemyBox;
		Collider[] array = Physics.OverlapSphere(vector, damageRadius, layerMask);
		List<Agent> list = new List<Agent>();
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Agent firstComponentUpward = collider.gameObject.GetFirstComponentUpward<Agent>();
			Vector3 outInpuls;
			if (firstComponentUpward != null)
			{
				if (!list.Contains(firstComponentUpward))
				{
					float num = ComputeDamage(true, collider, vector, out outInpuls);
					if (num >= 0f)
					{
						list.Add(firstComponentUpward);
						firstComponentUpward.OnReceiveRangeDamage(causer, num, outInpuls, FromWeapon, m_WeaponType);
					}
				}
				continue;
			}
			HitZone component = collider.GetComponent<HitZone>();
			if (component != null)
			{
				float num2 = ComputeDamage(false, collider, vector, out outInpuls);
				if (num2 >= 0f)
				{
					component.OnReceiveRangeDamage(causer, num2, outInpuls, FromWeapon, m_WeaponType);
				}
				continue;
			}
			DestroyObject component2 = collider.GetComponent<DestroyObject>();
			if (component2 != null)
			{
				float num3 = ComputeDamage(false, collider, vector, out outInpuls);
				if (num3 >= 0f)
				{
					component2.OnReceiveRangeDamage(causer, num3, outInpuls, m_WeaponType);
				}
			}
		}
	}

	private Vector3 GetTargetPoint(bool TargetIsAgent, Collider Target, Vector3 ExplosionPos)
	{
		if (TargetIsAgent && AgentsCache2.UsesRagdolls)
		{
			return ClosestPoint.PointBounds(ExplosionPos, Target.bounds);
		}
		return ClosestPoint.PointBoundsCenter(ExplosionPos, Target.bounds);
	}

	private float ComputeDamage(bool inAgent, Collider inVictim, Vector3 inExplosionPos, out Vector3 outInpuls)
	{
		Vector3 targetPoint = GetTargetPoint(inAgent, inVictim, inExplosionPos);
		float num = Vector3.Distance(targetPoint, inExplosionPos);
		outInpuls = Vector3.zero;
		if (num >= damageRadius || IsCollisionBetween(inExplosionPos, targetPoint, inVictim))
		{
			return -1f;
		}
		float num2 = damage;
		if (num > damageMaxRadius)
		{
			num2 *= (damageRadius - num) / (damageRadius - damageMaxRadius);
		}
		outInpuls = inVictim.bounds.center - inExplosionPos;
		outInpuls.y = targetPoint.y - inExplosionPos.y;
		outInpuls.Normalize();
		return num2;
	}

	private bool IsCollisionBetween(Vector3 inFrom, Vector3 inTo, Collider inVictim)
	{
		Vector3 vector = inTo - inFrom;
		float magnitude = vector.magnitude;
		vector /= magnitude;
		inFrom -= vector * 0.04f;
		int layersMask = ~(ObjectLayerMask.Enemy | ObjectLayerMask.EnemyBox | ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.IgnorePlayer);
		List<HitInfo> list = HitDetection.RayCast(inFrom, vector, magnitude, layersMask);
		foreach (HitInfo item in list)
		{
			if (item.data.collider == inVictim || item.data.collider.isTrigger || (noBlocking != null && IgnoreInBlockingTest(item.data.collider.transform)))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private bool IgnoreInBlockingTest(Transform inTransform)
	{
		Transform[] array = noBlocking;
		foreach (Transform transform in array)
		{
			if (transform == inTransform)
			{
				return true;
			}
		}
		return false;
	}

	private void GenerateWaveFX()
	{
		if (!(Camera.main == null))
		{
			float magnitude = (base.transform.position - Camera.main.transform.position).magnitude;
			float t = Mathf.Min(magnitude / m_WaveFXMaxWrldDist, 1f);
			MFExplosionPostFX.S_WaveParams waveParams = default(MFExplosionPostFX.S_WaveParams);
			waveParams.m_Amplitude = Mathf.Lerp(m_WaveFXAmplitudeMax, m_WaveFXAmplitudeMin, t);
			waveParams.m_Duration = m_WaveFXDuration;
			waveParams.m_Freq = m_WaveFXFreq;
			waveParams.m_Speed = m_WaveFXSpeed;
			waveParams.m_Radius = Mathf.Lerp(m_WaveFXRadiusMax, m_WaveFXRadiusMin, t);
			waveParams.m_Delay = m_WaveFXDelay;
			CamExplosionFXMgr.Instance.SpawnExplosionWaveFX(base.transform.position, waveParams, m_WaveFXDelay);
		}
	}
}
