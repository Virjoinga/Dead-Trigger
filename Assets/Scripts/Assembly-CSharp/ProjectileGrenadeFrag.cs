using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Items/ProjectileGrenade Frag")]
public class ProjectileGrenadeFrag : MonoBehaviour, IImportantObject
{
	protected AgentHuman m_Owner;

	private ProjectileInitSettings m_GrenadeSettings;

	protected GameObject m_GameObject;

	protected Transform m_Transform;

	private Animation m_Anim;

	private AudioSource m_Audio;

	private MeshRenderer m_Mesh;

	protected Rigidbody m_RBody;

	protected Collider m_Collider;

	private float m_SleepTimer;

	protected int m_HitSoundNum;

	public int m_HitSoundLimit = 3;

	protected Collider m_WaterVolume;

	public float Speed;

	public float DamageCoef;

	public E_ImportantObjectType m_ImportantObjectType;

	public Explosion m_Explosion;

	public Vector3 m_ExplosionOffset = Vector3.zero;

	public float m_ExplosionDelay = 5f;

	public bool m_ExplodeOnHit = true;

	public bool m_IsSticky;

	public bool Stuck { get; private set; }

	public bool Finished { get; private set; }

	public bool Initialized { get; private set; }

	public void Awake()
	{
		m_GameObject = base.gameObject;
		m_Transform = base.transform;
		m_RBody = base.GetComponent<Rigidbody>();
		m_RBody.sleepVelocity = 0.3f;
		m_RBody.sleepAngularVelocity = 5f;
		m_RBody.solverIterations = 10;
		Initialized = false;
		m_Anim = base.GetComponent<Animation>();
		m_Audio = base.GetComponent<AudioSource>();
		m_Collider = base.GetComponent<Collider>();
		m_Mesh = GetComponentInChildren<MeshRenderer>();
	}

	private void OnDestroy()
	{
		CancelInvoke();
		m_Explosion = null;
		m_WaterVolume = null;
		m_RBody.Sleep();
		if ((bool)m_Anim)
		{
			m_Anim.Stop();
		}
		if ((bool)m_Audio)
		{
			m_Audio.Stop();
		}
	}

	public void Init(Item.InitData Data)
	{
		Init(Data, null);
	}

	public void Init(Item.InitData Data, ProjectileInitSettings Settings)
	{
		m_Owner = Data.Owner;
		m_Transform.position = Data.Pos;
		m_GrenadeSettings = Settings;
		m_SleepTimer = 0f;
		m_HitSoundNum = m_HitSoundLimit;
		Stuck = false;
		Finished = false;
		Initialized = true;
		m_RBody.velocity = Data.Dir * ((m_GrenadeSettings == null) ? Speed : m_GrenadeSettings.Speed);
		m_RBody.angularVelocity = Random.insideUnitSphere * (Speed / 2f);
		m_RBody.WakeUp();
		m_Mesh.enabled = true;
		m_Collider.enabled = true;
		if ((bool)m_Anim)
		{
			m_Anim.Play();
		}
		if ((bool)m_Audio)
		{
			m_Audio.Play();
		}
		ComputeTrajectory();
		Mission.Instance.CurrentGameZone.RegisterImportantObject(this);
		Invoke("Explode", m_ExplosionDelay);
	}

	public void Update()
	{
		if (!Finished && Initialized)
		{
			if (m_IsSticky || m_ExplodeOnHit)
			{
				DetectCollisions();
			}
			DebugDraw.Sphere(Color.white, 1f, m_Transform.position);
			m_SleepTimer += Time.deltaTime;
			if (m_RBody.velocity.magnitude > m_RBody.sleepVelocity || m_RBody.angularVelocity.magnitude > m_RBody.sleepAngularVelocity)
			{
				m_SleepTimer = 0f;
			}
			if (m_SleepTimer > 0.5f)
			{
				m_RBody.Sleep();
			}
		}
	}

	private void DetectCollisions()
	{
		Vector3 position = m_RBody.position;
		Vector3 vector = m_RBody.position + m_RBody.velocity * Time.deltaTime;
		Vector3 vector2 = vector - position;
		float magnitude = vector2.magnitude;
		vector2 /= magnitude;
		position -= 1.5f * vector2;
		magnitude += 1.5f;
		int layersMask = ((!m_IsSticky || m_ExplodeOnHit) ? (~(ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.IgnoreBullets)) : ObjectLayerMask.Default);
		List<HitInfo> list = HitDetection.SphereCast(position, (m_IsSticky || !m_ExplodeOnHit) ? 0.05f : 0.2f, vector2, magnitude, layersMask);
		foreach (HitInfo item in list)
		{
			if (!(item.data.distance < 1.125f) && !(item.data.collider == m_Collider) && !item.data.collider.isTrigger && (!(m_Owner != null) || !item.data.collider.transform.IsChildOf(m_Owner.Transform)))
			{
				HitZone component = item.data.collider.gameObject.GetComponent<HitZone>();
				Agent firstComponentUpward = GameObjectUtils.GetFirstComponentUpward<Agent>(item.data.collider.gameObject);
				AgentHuman agentHuman = firstComponentUpward as AgentHuman;
				if ((((bool)agentHuman && agentHuman.IsAlive) || (bool)component) && m_ExplodeOnHit)
				{
					Explode(item.data.point);
					break;
				}
				if (m_IsSticky)
				{
					m_Collider.enabled = false;
					m_Transform.position = item.data.point;
					m_Transform.forward = item.data.normal;
					m_Transform.parent = item.data.transform;
					m_RBody.Sleep();
					Stuck = true;
					SemanticMaterialManager.Instance.SpawnPlacementEffect(item.data);
					break;
				}
			}
		}
	}

	internal void Explode()
	{
		Explode(m_Transform.position);
	}

	internal void Explode(Vector3 pos)
	{
		CancelInvoke("Explode");
		m_RBody.Sleep();
		m_Transform.position = pos;
		Explosion explosion = null;
		pos += m_ExplosionOffset;
		if (m_Explosion != null)
		{
			explosion = ((!(m_Owner != null)) ? Mission.Instance.ExplosionCache.Get(m_Explosion, pos, Quaternion.identity, m_Transform) : Mission.Instance.ExplosionCache.Get(m_Explosion, pos, Quaternion.identity, new Transform[2] { m_Transform, m_Owner.Transform }));
		}
		if (explosion != null)
		{
			explosion.causer = m_Owner;
			if (m_GrenadeSettings == null)
			{
				explosion.damage = DamageCoef * (float)GameplayData.Instance.EnemyHealth();
			}
			else
			{
				explosion.damage = m_GrenadeSettings.Damage;
			}
		}
		Finished = true;
		if (m_GrenadeSettings == null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			m_Mesh.enabled = false;
		}
		Mission.Instance.CurrentGameZone.UnregisterImportantObject(this);
		Initialized = false;
	}

	internal void OnCollisionEnter(Collision Coll)
	{
		if (!Finished && (!(m_Owner != null) || !Coll.transform.IsChildOf(m_Owner.Transform)))
		{
			HitZone component = Coll.gameObject.GetComponent<HitZone>();
			Agent firstComponentUpward = Coll.gameObject.GetFirstComponentUpward<Agent>();
			AgentHuman agentHuman = firstComponentUpward as AgentHuman;
			bool flag = false;
			if (m_ExplodeOnHit)
			{
				flag |= agentHuman != null && agentHuman.BlackBoard.GrenadesExplodeOnHit;
				flag |= agentHuman == null && component != null;
			}
			if (flag)
			{
				Explode(m_Transform.position);
			}
			else if (m_HitSoundNum > 0)
			{
				PlayHitSound(Coll);
				m_HitSoundNum--;
			}
			ComputeTrajectory();
		}
	}

	protected void PlayHitSound(Collision Coll)
	{
		SemanticMaterialManager.Instance.SpawnImpactEffect(Coll);
	}

	internal void OnCollisionStay(Collision Coll)
	{
		ComputeTrajectory();
	}

	internal void OnCollisionExit(Collision Coll)
	{
		ComputeTrajectory();
	}

	private void OnRenderObject()
	{
	}

	public void ComputeTrajectory()
	{
	}

	public void DisplayTrajectory(float DisplayTime)
	{
	}

	public E_ImportantObjectType GetImportantObjectType()
	{
		return m_ImportantObjectType;
	}

	public GameObject GetGameObject()
	{
		return m_GameObject;
	}
}
