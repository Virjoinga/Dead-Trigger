using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Items/Projectile Bait")]
public class ProjectileBait : MonoBehaviour, IImportantObject
{
	private AgentHuman m_Owner;

	public float m_Speed;

	public float m_Timer;

	private GameObject m_GameObject;

	private Transform m_Transform;

	private Rigidbody m_RBody;

	private Collider m_Collider;

	private float m_SleepTimer;

	private int m_HitSoundNum;

	public int m_HitSoundLimit = 3;

	private Collider m_WaterVolume;

	public E_ImportantObjectType m_ImportantObjectType;

	private Animation m_Anim;

	private AudioSource m_Audio;

	private List<Vector3> m_Trajectory = new List<Vector3>();

	public void Awake()
	{
		m_GameObject = base.gameObject;
		m_Transform = base.transform;
		m_RBody = base.GetComponent<Rigidbody>();
		m_RBody.sleepVelocity = 0.3f;
		m_RBody.sleepAngularVelocity = 5f;
		m_RBody.solverIterations = 10;
		m_Collider = base.GetComponent<Collider>();
		m_Anim = base.GetComponent<Animation>();
		m_Audio = base.GetComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		CancelInvoke();
		m_RBody.Sleep();
		m_WaterVolume = null;
		if (m_Anim != null)
		{
			m_Anim.Stop();
		}
		if (m_Audio != null)
		{
			m_Audio.Stop();
		}
		if (Mission.Instance != null)
		{
			Mission.Instance.CurrentGameZone.UnregisterImportantObject(this);
		}
	}

	public void Init(Item.InitData Data)
	{
		m_Owner = Data.Owner;
		m_Transform.position = Data.Pos;
		m_SleepTimer = 0f;
		m_HitSoundNum = m_HitSoundLimit;
		m_Collider.enabled = true;
		m_RBody.velocity = Data.Dir * m_Speed;
		m_RBody.angularVelocity = Random.insideUnitSphere * (m_Speed / 2f);
		m_RBody.WakeUp();
		ComputeTrajectory();
		if ((bool)m_Anim)
		{
			m_Anim.Play();
		}
		if (m_Audio != null)
		{
			m_Audio.Play();
		}
		Mission.Instance.CurrentGameZone.RegisterImportantObject(this);
		Invoke("DestroyBait", m_Timer);
	}

	private void DestroyBait()
	{
		Object.Destroy(base.gameObject);
	}

	public void Update()
	{
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

	internal void OnCollisionEnter(Collision Coll)
	{
		if (!(m_Owner != null) || !Coll.transform.IsChildOf(m_Owner.Transform))
		{
			if (m_WaterVolume == null && m_HitSoundNum > 0)
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
		DisplayTrajectory(0f);
		CapsuleCollider capsuleCollider = base.GetComponent<Collider>() as CapsuleCollider;
		if (capsuleCollider != null)
		{
			DebugDraw.Capsule((!m_RBody.IsSleeping()) ? Color.white : Color.gray, capsuleCollider);
		}
	}

	public void ComputeTrajectory()
	{
		m_Trajectory.Clear();
		if (!(Mathf.Abs(m_RBody.velocity.y) < 0.2f))
		{
			Throw.ComputeTrajectory(m_Transform.position, m_RBody.velocity, 4f, 0.1f, m_Trajectory);
			Throw.ClipTrajectoryToFirstHit(m_Trajectory);
		}
	}

	public void DisplayTrajectory(float DisplayTime)
	{
		DebugDraw.DepthTest = true;
		DebugDraw.DisplayTime = DisplayTime;
		for (int i = 0; i < m_Trajectory.Count - 1; i++)
		{
			DebugDraw.Line(Color.red, m_Trajectory[i], m_Trajectory[i + 1]);
		}
		DebugDraw.DisplayTime = 0f;
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
