using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Items/Cutter - Mechanical")]
public class CutterMechanical : MonoBehaviour
{
	private enum State
	{
		Activating = 0,
		Active = 1,
		Deactivating = 2,
		Inactive = 3
	}

	[Serializable]
	public class DeactivationSettings
	{
		public float m_Delay;

		public GameObject m_Destroy;

		public Explosion m_Explosion;

		public Transform m_ExplosionOrigin;

		public float m_ExplosionDamageCoef;

		public float m_ExplosionRadius;

		public float m_ExplosionRadiusMax;
	}

	private const float TiltChangeDuration = 0.2f;

	private const float TiltDamageThreshold = 0.1f;

	private const float RaycastOffset = 0.3f;

	private AgentHuman m_Owner;

	private Transform m_Transform;

	public Transform m_SpinningPart;

	private AudioSource m_Audio;

	private State m_State;

	private MappingChange m_LifeBar;

	public int m_HitsLimit = 10;

	private int m_HitsRemaining;

	public float m_AfterHitHealth = 0.5f;

	private static BladesChopperDamage m_DmgRLeg = new BladesChopperDamage(E_BodyPart.RightLeg);

	private static BladesChopperDamage m_DmgLLeg = new BladesChopperDamage(E_BodyPart.LeftLeg);

	public AudioClip m_SndActivating;

	public AudioClip m_SndActive;

	public AudioClip m_SndDeactivating;

	public Transform[] m_Blades;

	public float m_BladesLength;

	public float m_SpinDuration = 1.5f;

	private float m_BladesHeightMin;

	private float m_BladesHeightMax;

	private float m_BladesTiltCoef;

	private UnityEngine.AI.NavMeshAgent m_NavMeshAgent;

	public DeactivationSettings m_Deactivation = new DeactivationSettings();

	private void Awake()
	{
		m_Transform = base.transform;
	}

	public void Init(Item.InitData Data)
	{
		m_Owner = Data.Owner;
		m_Transform.position = Data.Pos;
		m_Transform.rotation.SetLookRotation(Data.Dir);
		SemanticMaterialManager.Instance.SpawnPlacementEffect(Data.Pos);
	}

	private void Start()
	{
		GameObject gameObject = base.gameObject;
		MappingChange[] componentsInChildren = gameObject.GetComponentsInChildren<MappingChange>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			m_LifeBar = componentsInChildren[0];
			m_LifeBar.Value = 1f;
			m_LifeBar.MappingChangeType = MappingChange.Type.BasedOnValue;
		}
		m_State = State.Activating;
		m_Audio = gameObject.GetComponent<AudioSource>();
		m_Audio.clip = m_SndActivating;
		m_Audio.loop = false;
		m_Audio.Play();
		m_BladesHeightMin = float.MaxValue;
		m_BladesHeightMax = float.MinValue;
		Transform[] blades = m_Blades;
		foreach (Transform transform in blades)
		{
			m_BladesHeightMin = Mathf.Min(m_BladesHeightMin, transform.position.y);
			m_BladesHeightMax = Mathf.Max(m_BladesHeightMax, transform.position.y);
		}
		m_BladesHeightMin -= 0.3f;
		m_BladesHeightMax += 0.3f;
		m_HitsRemaining = m_HitsLimit;
		SetBladesTilt(1f);
		m_NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (m_NavMeshAgent != null)
		{
			m_NavMeshAgent.updatePosition = false;
			m_NavMeshAgent.updateRotation = false;
			m_NavMeshAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
			m_NavMeshAgent.enabled = true;
		}
	}

	private void Update()
	{
		if (m_State == State.Inactive)
		{
			return;
		}
		float num = (float)Math.PI * 2f / m_SpinDuration;
		float tiltingDuration = 0.2f;
		switch (m_State)
		{
		case State.Activating:
			if (m_Audio.isPlaying)
			{
				num *= m_Audio.time / m_Audio.clip.length;
				break;
			}
			m_State = State.Active;
			m_Audio.clip = m_SndActive;
			m_Audio.loop = true;
			m_Audio.Play();
			break;
		case State.Active:
			if (m_HitsRemaining <= 0)
			{
				m_State = State.Deactivating;
				if (m_SndDeactivating != null)
				{
					m_Audio.clip = m_SndDeactivating;
					m_Audio.loop = false;
					m_Audio.Play();
					tiltingDuration = m_SndDeactivating.length / 2f;
				}
			}
			break;
		case State.Deactivating:
			if (m_Audio.isPlaying)
			{
				num *= 1f - m_Audio.time / m_Audio.clip.length;
				tiltingDuration = m_SndDeactivating.length / 2f;
			}
			else
			{
				m_State = State.Inactive;
				Invoke("OnDeactivation", m_Deactivation.m_Delay);
			}
			break;
		}
		m_SpinningPart.Rotate(m_SpinningPart.up, num * Time.deltaTime);
		UpdateBlades(Time.deltaTime, tiltingDuration);
	}

	private void UpdateBlades(float DeltaTime, float TiltingDuration)
	{
		bool flag = m_State == State.Active;
		if (m_Owner != null && m_Owner.IsAlive && m_State == State.Active)
		{
			Vector3 position = m_Owner.Position;
			Vector3 vector = position - m_Transform.position;
			float num = vector.x * vector.x + vector.z * vector.z;
			float num2 = m_BladesLength + 0.3f;
			if (num <= num2 * num2)
			{
				flag = position.y + 2f < m_BladesHeightMin || position.y > m_BladesHeightMax;
			}
		}
		float bladesTiltCoef = m_BladesTiltCoef;
		float num3 = DeltaTime / TiltingDuration;
		bladesTiltCoef = ((!flag) ? Mathf.Min(1f, m_BladesTiltCoef + num3) : Mathf.Max(0f, m_BladesTiltCoef - num3));
		SetBladesTilt(bladesTiltCoef);
		if (bladesTiltCoef > 0.1f || m_HitsRemaining <= 0)
		{
			return;
		}
		Transform[] blades = m_Blades;
		foreach (Transform transform in blades)
		{
			List<HitInfo> list = HitDetection.RayCast(transform.position - 0.3f * transform.forward, transform.forward, m_BladesLength + 0.3f);
			foreach (HitInfo item in list)
			{
				if (!item.data.collider.isTrigger)
				{
					int num4 = ProcessHit(item);
					m_HitsRemaining = Mathf.Max(0, m_HitsRemaining - num4);
					if (m_LifeBar != null)
					{
						m_LifeBar.Value = (float)m_HitsRemaining / (float)m_HitsLimit;
					}
					if (m_HitsRemaining == 0)
					{
						break;
					}
				}
			}
		}
	}

	private int ProcessHit(HitInfo Hit)
	{
		int result = 0;
		if (Hit.hitZone != null)
		{
			ComponentEnemy firstComponentUpward = Hit.hitZone.GameObj.GetFirstComponentUpward<ComponentEnemy>();
			if (firstComponentUpward != null && firstComponentUpward.Owner.BlackBoard.ReactOnHits && !firstComponentUpward.IsLimbDecapitated(E_BodyPart.LeftLeg) && !firstComponentUpward.IsLimbDecapitated(E_BodyPart.RightLeg))
			{
				int numDecapitatedLimbs = firstComponentUpward.GetNumDecapitatedLimbs();
				Hit.hitZone.OnMeleeDamage(GetDamageDesc(firstComponentUpward.Owner));
				if (numDecapitatedLimbs < firstComponentUpward.GetNumDecapitatedLimbs())
				{
					result = 1;
				}
				if (firstComponentUpward.Owner.IsBoss)
				{
					result = Mathf.CeilToInt((float)m_HitsLimit * 0.25f);
				}
			}
		}
		return result;
	}

	private BladesChopperDamage GetDamageDesc(AgentHuman Victim)
	{
		BladesChopperDamage bladesChopperDamage = ((!(Vector3.Dot(Victim.Transform.right, m_Transform.position - Victim.Position) >= 0f)) ? m_DmgLLeg : m_DmgRLeg);
		if (!Victim.IsBoss)
		{
			bladesChopperDamage.Attacker = m_Owner;
			bladesChopperDamage.Damage = 0f;
			bladesChopperDamage.ReducedHealthCoef = m_AfterHitHealth;
		}
		else
		{
			bladesChopperDamage.Attacker = m_Owner;
			bladesChopperDamage.Damage = Victim.BlackBoard.RealMaxHealth * 0.22f;
			bladesChopperDamage.ReducedHealthCoef = 1f;
		}
		return bladesChopperDamage;
	}

	private void SetBladesTilt(float Value)
	{
		float angle = (Value - m_BladesTiltCoef) * 90f;
		Transform[] blades = m_Blades;
		foreach (Transform transform in blades)
		{
			transform.RotateAround(transform.position, transform.right, angle);
		}
		m_BladesTiltCoef = Value;
	}

	private void OnDeactivation()
	{
		if (m_Deactivation.m_Explosion != null)
		{
			Vector3 inPosition = ((!(m_Deactivation.m_ExplosionOrigin != null)) ? m_Transform.position : m_Deactivation.m_ExplosionOrigin.position);
			Quaternion inRotation = ((!(m_Deactivation.m_ExplosionOrigin != null)) ? m_Transform.rotation : m_Deactivation.m_ExplosionOrigin.rotation);
			Explosion explosion = Mission.Instance.ExplosionCache.Get(m_Deactivation.m_Explosion, inPosition, inRotation);
			if (explosion != null)
			{
				explosion.causer = m_Owner;
				if (m_Deactivation.m_ExplosionDamageCoef > 0f)
				{
					explosion.damage = m_Deactivation.m_ExplosionDamageCoef * (float)GameplayData.Instance.EnemyHealth();
				}
				if (m_Deactivation.m_ExplosionRadius > 0f)
				{
					explosion.damageRadius = m_Deactivation.m_ExplosionRadiusMax;
				}
				if (m_Deactivation.m_ExplosionRadiusMax > 0f)
				{
					explosion.damageMaxRadius = m_Deactivation.m_ExplosionRadius;
				}
			}
		}
		if (m_Deactivation.m_Destroy != null)
		{
			UnityEngine.Object.Destroy(m_Deactivation.m_Destroy);
		}
	}
}
