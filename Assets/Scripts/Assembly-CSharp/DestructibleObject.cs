using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NESEvent(new string[] { "Version Destroyed", "Object Destroyed" })]
[AddComponentMenu("Entities/Destructable Object")]
[RequireComponent(typeof(AudioSource))]
public class DestructibleObject : MonoBehaviour, IGameZoneChild, IGameZoneChild_AutoRegister, IImportantObject, IHitZoneOwner
{
	private struct GfxRecord
	{
		public GameObject m_GameObject;

		public ParticleSystem[] m_Effects;

		public float m_ActivationTime;

		public GfxRecord(GameObject Obj, ParticleSystem[] Particles)
		{
			m_GameObject = Obj;
			m_Effects = Particles;
			m_ActivationTime = Time.timeSinceLevelLoad;
		}
	}

	[Serializable]
	public class Version
	{
		public GameObject m_Object;

		public float m_Health;

		public AudioClip m_Sfx;

		public List<GameObject> m_Gfx;

		public Explosion m_Explosion;

		public Transform m_ExplosionOrigin;

		public float m_ExplosionDamage;

		public float m_ExplosionRadius;

		public float m_ExplosionRadiusMax;

		public void Initialize()
		{
			DeActivate();
			if (m_Gfx == null)
			{
				return;
			}
			for (int num = m_Gfx.Count - 1; num >= 0; num--)
			{
				if (m_Gfx[num] != null)
				{
					m_Gfx[num]._SetActiveRecursively(false);
				}
				else
				{
					m_Gfx.RemoveAt(num);
				}
			}
		}

		public void Activate()
		{
			if (m_Object != null)
			{
				m_Object._SetActiveRecursively(true);
			}
		}

		public void DeActivate()
		{
			if (m_Object != null)
			{
				m_Object._SetActiveRecursively(false);
			}
		}
	}

	[Serializable]
	public class SoundInfo
	{
		public AudioClip[] HitSounds = new AudioClip[0];
	}

	private GameObject m_GameObj;

	public bool m_IsImportantObject;

	public List<Version> m_Versions = new List<Version>();

	public SoundInfo m_SoundSetup = new SoundInfo();

	public AnimationClip[] m_AttackAnims = new AnimationClip[0];

	private int m_CurrVersion;

	private float m_CurrVersionHP;

	private float m_TotalHP;

	private float m_RemainingHP;

	private MappingChange m_LifeBar;

	private int m_IgnoreDamage;

	private float m_DmgMultiplier = 1f;

	public int m_TotalHits = 7;

	private AudioSource m_Audio;

	private List<GfxRecord> m_Gfx = new List<GfxRecord>();

	private NESController m_NESController;

	[NonSerialized]
	private ArrayList RegisteredAgents;

	[NonSerialized]
	private ArrayList AttackPoints = new ArrayList();

	public float HealthCoef
	{
		get
		{
			return m_RemainingHP / m_TotalHP;
		}
		set
		{
			SetState(value);
		}
	}

	public bool IsAlive
	{
		get
		{
			return m_RemainingHP > 0f && GetGameObject() != null;
		}
	}

	private void Awake()
	{
		m_GameObj = base.gameObject;
		int count = m_Versions.Count;
		if (count == 0)
		{
			return;
		}
		Version version = m_Versions[count - 1];
		if (version.m_Health != 0f)
		{
			version.m_Health = 0f;
		}
		m_Audio = m_GameObj.GetComponent<AudioSource>();
		if (m_Audio == null)
		{
		}
		m_NESController = m_GameObj.GetFirstComponentUpward<NESController>();
		if (m_NESController == null)
		{
		}
		MappingChange[] componentsInChildren = m_GameObj.GetComponentsInChildren<MappingChange>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			m_LifeBar = componentsInChildren[0];
			m_LifeBar.Value = 1f;
			m_LifeBar.MappingChangeType = MappingChange.Type.BasedOnValue;
		}
		bool flag = false;
		m_TotalHP = 0f;
		foreach (Version version2 in m_Versions)
		{
			version2.Initialize();
			m_TotalHP += version2.m_Health;
			flag |= version2.m_Gfx != null && version2.m_Gfx.Count > 0;
		}
		m_CurrVersion = 0;
		m_CurrVersionHP = m_Versions[0].m_Health;
		m_RemainingHP = m_TotalHP;
		if (flag)
		{
			m_Gfx = new List<GfxRecord>();
		}
		if (!m_IsImportantObject)
		{
			return;
		}
		AttackPoints = new ArrayList();
		RegisteredAgents = new ArrayList();
		m_DmgMultiplier = m_TotalHP / (float)(m_TotalHits * GameplayData.Instance.EnemyAttack());
		Transform transform = null;
		while ((transform = m_GameObj.transform.FindChildByName("AttackPoint" + AttackPoints.Count)) != null)
		{
			AttackPoint component = transform.GetComponent<AttackPoint>();
			if (component != null)
			{
				AttackPoints.Add(component);
			}
		}
	}

	private void Init(Item.InitData Data)
	{
		m_GameObj.transform.position = Data.Pos;
		m_GameObj.transform.forward = Data.Dir;
		foreach (Version version in m_Versions)
		{
			version.DeActivate();
		}
		m_Versions[0].Activate();
	}

	private void Start()
	{
		Enable();
	}

	private void OnDestroy()
	{
		if (m_IsImportantObject && Mission.Instance != null)
		{
			Mission.Instance.CurrentGameZone.UnregisterImportantObject(this);
		}
		m_GameObj = null;
	}

	[NESAction]
	public void Enable()
	{
		m_Versions[m_CurrVersion].Activate();
		if (m_IsImportantObject)
		{
			Mission.Instance.CurrentGameZone.RegisterImportantObject(this);
		}
	}

	public void Reset()
	{
		m_TotalHP = 0f;
		foreach (Version version in m_Versions)
		{
			version.DeActivate();
			m_TotalHP += version.m_Health;
		}
		m_CurrVersion = 0;
		m_CurrVersionHP = m_Versions[0].m_Health;
		m_RemainingHP = m_TotalHP;
		UnregisterAllAgents();
	}

	[NESAction]
	public void Disable()
	{
		m_Versions[m_CurrVersion].DeActivate();
		if (m_IsImportantObject)
		{
			Mission.Instance.CurrentGameZone.UnregisterImportantObject(this);
		}
	}

	public void Update()
	{
		if (m_CurrVersion != -1 && m_Gfx != null)
		{
			for (int num = m_Gfx.Count - 1; num >= 0; num--)
			{
				bool flag = false;
				ParticleSystem[] effects = m_Gfx[num].m_Effects;
				foreach (ParticleSystem particleSystem in effects)
				{
					flag |= particleSystem.isPlaying;
				}
				if (!flag)
				{
					m_Gfx[num].m_GameObject._SetActiveRecursively(false);
					m_Gfx.RemoveAt(num);
				}
			}
		}
		m_IgnoreDamage--;
		if (m_IsImportantObject)
		{
			UpdateRegisteredAgents();
		}
	}

	public bool IsActivatedWithGameZone()
	{
		return false;
	}

	public E_ImportantObjectType GetImportantObjectType()
	{
		return E_ImportantObjectType.DestructibleObject;
	}

	public GameObject GetGameObject()
	{
		return m_GameObj;
	}

	public void OnHitZoneProjectileHit(HitZone Zone, Projectile Proj)
	{
		if (m_CurrVersion < m_Versions.Count - 1)
		{
			Proj.hitProcessed = true;
			OnDamage(Proj.Agent, Proj.Damage() * Zone.DamageModifier, false);
		}
	}

	public void OnHitZoneRangeDamage(HitZone Zone, Agent Attacker, float Damage, Vector3 Impulse, E_WeaponID weaponID, E_WeaponType WeaponType)
	{
		if (m_CurrVersion < m_Versions.Count - 1)
		{
			OnDamage(Attacker, Damage * Zone.DamageModifier, true);
		}
	}

	public void OnHitZoneMeleeDamage(HitZone Zone, MeleeDamageData Data)
	{
		if (m_CurrVersion < m_Versions.Count - 1)
		{
			OnDamage(null, Data.Damage * Zone.DamageModifier, false);
		}
	}

	public void TakeDamage(Agent attacker, float damage)
	{
		if (m_CurrVersion < m_Versions.Count - 1)
		{
			OnDamage(attacker, damage, true);
		}
	}

	private void OnDamage(Agent Instigator, float Damage, bool ShouldIgnore)
	{
		if (Game.Instance.MissionResultData.Result == MissionResult.Type.SUCCESS || (ShouldIgnore && m_IgnoreDamage > 0) || (m_IsImportantObject && Instigator != null && Instigator.IsPlayer))
		{
			return;
		}
		Damage *= m_DmgMultiplier;
		m_RemainingHP -= Damage;
		m_CurrVersionHP -= Damage;
		if (m_CurrVersionHP <= 0f)
		{
			m_RemainingHP -= m_CurrVersionHP;
			Version version = m_Versions[m_CurrVersion];
			OnDestruction(Instigator, version);
			version.DeActivate();
			version = m_Versions[++m_CurrVersion];
			version.Activate();
			if (m_CurrVersion < m_Versions.Count - 1)
			{
				m_CurrVersionHP = version.m_Health;
				if (m_NESController != null)
				{
					m_NESController.SendGameEvent(this, "Version Destroyed");
				}
			}
			else if (m_NESController != null)
			{
				m_NESController.SendGameEvent(this, "Object Destroyed");
			}
		}
		else
		{
			AudioClip audioClip = MiscUtils.RandomValue(m_SoundSetup.HitSounds);
			if (audioClip != null)
			{
				m_Audio.PlayOneShot(audioClip);
			}
		}
		if (m_LifeBar != null)
		{
			m_LifeBar.Value = HealthCoef;
		}
	}

	private void OnDestruction(Agent Instigator, Version V)
	{
		if (V.m_Sfx != null)
		{
			m_Audio.PlayOneShot(V.m_Sfx);
		}
		if (V.m_Gfx != null)
		{
			foreach (GameObject item in V.m_Gfx)
			{
				ParticleSystem[] componentsInChildren = item.GetComponentsInChildren<ParticleSystem>(true);
				if (componentsInChildren != null && componentsInChildren.Length != 0)
				{
					item._SetActiveRecursively(true);
					ParticleSystem[] array = componentsInChildren;
					foreach (ParticleSystem particleSystem in array)
					{
						particleSystem.Play();
					}
					m_Gfx.Add(new GfxRecord(item, componentsInChildren));
				}
			}
		}
		if (!(V.m_Explosion != null))
		{
			return;
		}
		Vector3 inPosition = ((!(V.m_ExplosionOrigin != null)) ? V.m_Object.transform.position : V.m_ExplosionOrigin.position);
		Quaternion inRotation = ((!(V.m_ExplosionOrigin != null)) ? V.m_Object.transform.rotation : V.m_ExplosionOrigin.rotation);
		Explosion explosion = Mission.Instance.ExplosionCache.Get(V.m_Explosion, inPosition, inRotation);
		if (explosion != null)
		{
			explosion.causer = Instigator;
			if (V.m_ExplosionDamage > 0f)
			{
				explosion.damage = V.m_ExplosionDamage;
			}
			else if (V.m_ExplosionDamage < 0f)
			{
				explosion.damage = (0f - V.m_ExplosionDamage) * (float)GameplayData.Instance.EnemyHealth();
			}
			if (V.m_ExplosionRadius > 0f)
			{
				explosion.damageRadius = V.m_ExplosionRadiusMax;
			}
			if (V.m_ExplosionRadiusMax > 0f)
			{
				explosion.damageMaxRadius = V.m_ExplosionRadius;
			}
		}
		m_IgnoreDamage = 2;
	}

	private void SetState(float Coef)
	{
		Coef = Mathf.Clamp(Coef, 0f, 1f);
		m_RemainingHP = Coef * m_TotalHP;
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		for (int num4 = m_Versions.Count - 1; num4 >= 0; num4--)
		{
			num2 = num3;
			num3 += m_Versions[num4].m_Health;
			if (num2 < m_RemainingHP && m_RemainingHP <= num3)
			{
				num = num4;
				break;
			}
		}
		m_CurrVersionHP = m_RemainingHP - num2;
		if (m_CurrVersion != num)
		{
			m_Versions[m_CurrVersion].DeActivate();
			m_CurrVersion = num;
			m_Versions[m_CurrVersion].Activate();
		}
		if (m_LifeBar != null)
		{
			m_LifeBar.Value = Coef;
		}
	}

	public AttackPoint FindAttackPoint(Agent agent)
	{
		foreach (AttackPoint attackPoint in AttackPoints)
		{
			if (attackPoint.RegisteredAgent == agent)
			{
				return attackPoint;
			}
		}
		return null;
	}

	public AttackPoint FindAttackPoint(Vector3 pos)
	{
		AttackPoint attackPoint = null;
		foreach (AttackPoint attackPoint2 in AttackPoints)
		{
			if (!(attackPoint2.RegisteredAgent == null))
			{
				continue;
			}
			if (attackPoint == null)
			{
				attackPoint = attackPoint2;
				continue;
			}
			float sqrMagnitude = (attackPoint.Transform.position - pos).sqrMagnitude;
			float sqrMagnitude2 = (attackPoint2.Transform.position - pos).sqrMagnitude;
			if (sqrMagnitude2 < sqrMagnitude)
			{
				attackPoint = attackPoint2;
			}
		}
		return attackPoint;
	}

	public Transform GetAttackPoint(Agent agent)
	{
		foreach (AttackPoint attackPoint in AttackPoints)
		{
			if (attackPoint.RegisteredAgent == agent)
			{
				return attackPoint.Transform;
			}
		}
		return null;
	}

	public void RegisterAgent(Agent agent)
	{
		if (!IsAgentRegistered(agent))
		{
			RegisteredAgents.Add(agent);
			AttackPoint attackPoint = FindAttackPoint(agent.Transform.position);
			if (attackPoint != null)
			{
				attackPoint.RegisteredAgent = agent;
			}
		}
	}

	public void UnregisterAgent(Agent agent)
	{
		RegisteredAgents.Remove(agent);
		AttackPoint attackPoint = FindAttackPoint(agent);
		if (attackPoint != null)
		{
			attackPoint.RegisteredAgent = null;
		}
	}

	public void UnregisterAllAgents()
	{
		RegisteredAgents.Clear();
		foreach (AttackPoint attackPoint in AttackPoints)
		{
			attackPoint.RegisteredAgent = null;
		}
	}

	public bool IsAgentRegistered(Agent agent)
	{
		return RegisteredAgents.Contains(agent);
	}

	public int GetRegisteredAgentsCount()
	{
		return RegisteredAgents.Count;
	}

	private void UpdateRegisteredAgents()
	{
		ArrayList arrayList = new ArrayList();
		foreach (Agent registeredAgent in RegisteredAgents)
		{
			if (registeredAgent == null || !registeredAgent.IsAlive)
			{
				arrayList.Add(registeredAgent);
			}
		}
		foreach (Agent item in arrayList)
		{
			UnregisterAgent(item);
		}
	}
}
