using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Items/ProjectileGrenade Cluster")]
public class ProjectileGrenadeCluster : MonoBehaviour
{
	protected AgentHuman m_Owner;

	protected GameObject m_GameObject;

	protected Transform m_Transform;

	protected Rigidbody m_RBody;

	protected Collider m_Collider;

	private float m_SleepTimer;

	public GameObject m_ChildPrefab;

	public ParticleSystem[] m_ActivationEffect;

	public float m_ActivationDelay = 5f;

	protected int m_HitSoundNum;

	public int m_HitSoundLimit = 3;

	protected Collider m_WaterVolume;

	public float Speed;

	private Animation m_Anim;

	private AudioSource m_Audio;

	private List<Vector3> m_Trajectory = new List<Vector3>();

	public void Awake()
	{
		m_RBody = base.GetComponent<Rigidbody>();
		m_RBody.sleepVelocity = 0.3f;
		m_RBody.sleepAngularVelocity = 5f;
		m_RBody.solverIterations = 10;
		m_GameObject = base.gameObject;
		m_Anim = base.GetComponent<Animation>();
		m_Audio = base.GetComponent<AudioSource>();
		m_Collider = base.GetComponent<Collider>();
		m_Transform = base.transform;
	}

	private void OnDestroy()
	{
		CancelInvoke();
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
		m_Owner = Data.Owner;
		m_Transform.position = Data.Pos;
		m_SleepTimer = 0f;
		m_HitSoundNum = m_HitSoundLimit;
		m_RBody.velocity = Data.Dir * Speed;
		m_RBody.angularVelocity = Random.insideUnitSphere * (Speed / 2f);
		m_RBody.WakeUp();
		m_Collider.enabled = true;
		ComputeTrajectory();
		if ((bool)m_Anim)
		{
			m_Anim.Play();
		}
		if ((bool)m_Audio)
		{
			m_Audio.Play();
		}
		Invoke("Explode", m_ActivationDelay);
	}

	public void Update()
	{
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

	private void Explode()
	{
		CancelInvoke("Explode");
		if (m_ChildPrefab != null)
		{
			Item.InitData initData = new Item.InitData();
			foreach (Transform item in base.gameObject.transform)
			{
				GameObject gameObject = Object.Instantiate(m_ChildPrefab) as GameObject;
				if (gameObject != null)
				{
					initData.Owner = m_Owner;
					initData.Pos = item.position;
					initData.Dir = item.forward;
					gameObject._SetActiveRecursively(true);
					gameObject.BroadcastMessage("Init", initData, SendMessageOptions.RequireReceiver);
				}
			}
		}
		ParticleSystem[] activationEffect = m_ActivationEffect;
		foreach (ParticleSystem particleSystem in activationEffect)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
		}
		StartCoroutine(DestroyItself());
	}

	private IEnumerator DestroyItself()
	{
		m_RBody.Sleep();
		m_Collider.enabled = false;
		base.GetComponent<Renderer>().enabled = false;
		bool active;
		do
		{
			active = false;
			ParticleSystem[] activationEffect = m_ActivationEffect;
			foreach (ParticleSystem ps in activationEffect)
			{
				if (ps != null)
				{
					active |= ps.isPlaying || ps.IsAlive();
				}
			}
			yield return new WaitForEndOfFrame();
		}
		while (active);
		Object.Destroy(base.gameObject);
	}

	private void OnCollisionEnter(Collision Coll)
	{
		if (m_Owner != null && Coll.transform.IsChildOf(m_Owner.Transform))
		{
			return;
		}
		HitZone component = Coll.gameObject.GetComponent<HitZone>();
		Agent component2 = Coll.gameObject.GetComponent<Agent>();
		AgentHuman agentHuman = component2 as AgentHuman;
		if (agentHuman != null && !agentHuman.BlackBoard.GrenadesExplodeOnHit)
		{
			if (m_HitSoundNum > 0)
			{
				PlayHitSound(Coll);
				m_HitSoundNum--;
			}
		}
		else if (component2 != null)
		{
			Explode();
		}
		else if (component != null)
		{
			Explode();
		}
		else if (m_WaterVolume == null && m_HitSoundNum > 0)
		{
			PlayHitSound(Coll);
			m_HitSoundNum--;
		}
		ComputeTrajectory();
	}

	private void PlayHitSound(Collision Coll)
	{
		SemanticMaterialManager.Instance.SpawnImpactEffect(Coll);
	}

	private void OnCollisionStay(Collision Coll)
	{
		ComputeTrajectory();
	}

	private void OnCollisionExit(Collision Coll)
	{
		ComputeTrajectory();
	}

	private void OnRenderObject()
	{
		DisplayTrajectory(0f);
		CapsuleCollider capsuleCollider = base.GetComponent<Collider>() as CapsuleCollider;
		if (capsuleCollider != null)
		{
			DebugDraw.Capsule((!m_RBody.IsSleeping()) ? Color.white : Color.gray, capsuleCollider);
		}
	}

	private void ComputeTrajectory()
	{
		m_Trajectory.Clear();
		if (!(Mathf.Abs(m_RBody.velocity.y) < 0.2f))
		{
			Throw.ComputeTrajectory(m_Transform.position, m_RBody.velocity, 4f, 0.1f, m_Trajectory);
			Throw.ClipTrajectoryToFirstHit(m_Trajectory);
		}
	}

	private void DisplayTrajectory(float DisplayTime)
	{
		DebugDraw.DepthTest = true;
		DebugDraw.DisplayTime = DisplayTime;
		for (int i = 0; i < m_Trajectory.Count - 1; i++)
		{
			DebugDraw.Line(Color.red, m_Trajectory[i], m_Trajectory[i + 1]);
		}
		DebugDraw.DisplayTime = 0f;
	}
}
