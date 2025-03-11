using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Items/MiniMortar")]
public class MiniMortar : MonoBehaviour
{
	[Serializable]
	public class ActivationData
	{
		public float m_Delay;

		public string m_Anim;

		public AudioClip m_Sound;
	}

	[Serializable]
	public class DeactivationData
	{
		public float m_Delay;

		public string m_Anim;

		public AudioClip m_Sound;

		public ParticleSystem[] m_Particles;

		public MeshRenderer[] m_ObjectsToHide;
	}

	private class ProjRecord
	{
		public Transform m_XForm;

		public Vector3 m_Pos;

		public float m_Timer;

		public ProjRecord(Transform XForm)
		{
			m_XForm = XForm;
			m_Pos = XForm.position;
			m_Timer = 0f;
		}
	}

	private const float MaxPitchMod = 0.1f;

	private AgentHuman m_Owner;

	private Animation m_Anim;

	private AudioSource m_Audio;

	private Transform m_Transform;

	public ActivationData m_Activation = new ActivationData();

	public DeactivationData m_Deactivation = new DeactivationData();

	public float m_PauseDuration = 4f;

	private float m_PauseTimer;

	public float m_DetectionDistance = 4f;

	public float m_DetectionDuration = 1.5f;

	public float m_DetectionPause = 1f;

	public AudioClip m_DetectionSound;

	private float m_DetectionStartTime;

	private bool m_DetectionInProgress;

	public GameObject m_Detector;

	public float m_DetectorMaxScale = 2f;

	public GameObject m_ProjPrefab;

	public Transform[] m_ProjList;

	private int m_ProjIndex;

	public AudioClip m_EjectionSound;

	public float m_EjectionSpeedMult;

	private static bool m_FireLock = false;

	private float m_FireLockTimer = -1f;

	private static List<ProjRecord> m_Projectiles = new List<ProjRecord>();

	private static int m_FrameCounter = -1;

	private void Awake()
	{
		GameObject gameObject = base.gameObject;
		m_Transform = gameObject.transform;
		m_Anim = gameObject.GetComponent<Animation>();
		m_Audio = gameObject.GetComponent<AudioSource>();
		m_Audio.pitch += UnityEngine.Random.Range(-0.1f, 0.1f);
		m_DetectionInProgress = false;
		m_DetectionStartTime = -1f;
	}

	public void Init(Item.InitData Data)
	{
		m_Owner = Data.Owner;
		m_Transform.position = Data.Pos;
		StartCoroutine(Activation());
	}

	private IEnumerator Activation()
	{
		m_PauseTimer = float.MaxValue;
		if (m_Detector != null)
		{
			m_Detector.transform.localScale = new Vector3(2f * m_DetectionDistance, 2f * m_DetectionDistance, 1f);
			MeshRenderer r = m_Detector.GetComponent<MeshRenderer>();
			Vector4 p = new Vector4(m_DetectionDuration, m_DetectorMaxScale, 0f - Time.time - m_Activation.m_Delay, m_DetectionPause);
			r.material.SetVector("_Params", p);
		}
		if (m_Activation.m_Delay > 0f)
		{
			yield return new WaitForSeconds(m_Activation.m_Delay);
		}
		if (m_Audio != null && m_Activation.m_Sound != null)
		{
			m_Audio.PlayOneShot(m_Activation.m_Sound);
		}
		if (m_Anim != null && !string.IsNullOrEmpty(m_Activation.m_Anim))
		{
			m_Anim.Play(m_Activation.m_Anim);
			while (m_Anim.isPlaying)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		m_PauseTimer = 0f;
	}

	private IEnumerator Deactivation()
	{
		m_PauseTimer = float.MaxValue;
		if (m_Detector != null)
		{
			m_Detector._SetActiveRecursively(false);
		}
		if (m_Deactivation.m_Delay > 0f)
		{
			yield return new WaitForSeconds(m_Deactivation.m_Delay);
		}
		bool sfx = false;
		if (m_Audio != null && m_Deactivation.m_Sound != null)
		{
			sfx = true;
			m_Audio.PlayOneShot(m_Deactivation.m_Sound);
		}
		if (m_Anim != null && !string.IsNullOrEmpty(m_Deactivation.m_Anim))
		{
			m_Anim.Play(m_Deactivation.m_Anim);
			while (m_Anim.isPlaying)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		EffectUtils.HideRenderers(m_Deactivation.m_ObjectsToHide);
		if (EffectUtils.ActivateParticles(m_Deactivation.m_Particles))
		{
			while (!EffectUtils.AreParticlesFinished(m_Deactivation.m_Particles))
			{
				yield return new WaitForEndOfFrame();
			}
		}
		while (sfx && m_Audio.isPlaying)
		{
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void StartDetection()
	{
		m_DetectionInProgress = true;
		m_DetectionStartTime = Time.time;
		m_Audio.PlayOneShot(m_DetectionSound);
	}

	private void Update()
	{
		UpdateProjRecords();
		if (m_FireLockTimer > 0f)
		{
			m_FireLock = (m_FireLockTimer -= Time.deltaTime) > 0f;
		}
		if (m_PauseTimer >= 0f)
		{
			if ((m_PauseTimer -= Time.deltaTime) <= 0f)
			{
				InvokeRepeating("StartDetection", 0f, m_DetectionDuration + m_DetectionPause);
			}
		}
		else if (m_DetectionInProgress)
		{
			Vector3 Velocity = Vector3.zero;
			if (!m_FireLock && FindTarget(ref Velocity))
			{
				FireAtTarget(Velocity);
			}
		}
	}

	private static void UpdateProjRecords()
	{
		if (m_FrameCounter == Time.frameCount)
		{
			return;
		}
		m_FrameCounter = Time.frameCount;
		for (int num = m_Projectiles.Count - 1; num >= 0; num--)
		{
			ProjRecord projRecord = m_Projectiles[num];
			if (projRecord.m_XForm != null)
			{
				projRecord.m_Pos = projRecord.m_XForm.position;
			}
			else if ((projRecord.m_Timer += Time.deltaTime) > 0.25f)
			{
				m_Projectiles.RemoveAt(num);
			}
		}
	}

	private bool FindTarget(ref Vector3 Velocity)
	{
		float num = (Time.time - m_DetectionStartTime) / m_DetectionDuration;
		if (num >= 1f)
		{
			num = 1f;
			m_DetectionInProgress = false;
		}
		bool result = false;
		Vector3 vector = Vector3.zero;
		float num2 = float.MaxValue;
		float num3 = 2f;
		float num4 = m_DetectionDistance * num;
		num4 *= num4;
		foreach (Agent enemy in Mission.Instance.CurrentGameZone.Enemies)
		{
			if (enemy.IsAlive)
			{
				Vector3 a = enemy.Position - m_Transform.position;
				float num5 = Vector3.SqrMagnitude(a);
				if (!(num5 > num4) && !(a.y < -0.5f - num3) && !(a.y > 0.5f) && IsTargetValid(enemy, ref Velocity) && num5 < num2)
				{
					result = true;
					vector = Velocity;
					num2 = num5;
				}
			}
		}
		Velocity = vector;
		return result;
	}

	private bool IsTargetValid(Agent Target, ref Vector3 Velocity)
	{
		Vector3 vector = new Vector3(0f, 0.3f, 0f);
		Vector3 vector2 = m_Transform.position + vector;
		Vector3 position = Target.Position;
		Vector3 vector3 = position - vector2;
		int layermask = ~(ObjectLayerMask.Enemy | ObjectLayerMask.EnemyBox | ObjectLayerMask.IgnoreRaycast);
		RaycastHit[] array = Physics.RaycastAll(vector2, vector3.normalized, vector3.magnitude, layermask);
		RaycastHit[] array2 = array;
		foreach (RaycastHit raycastHit in array2)
		{
			if (!raycastHit.collider.isTrigger)
			{
				return false;
			}
		}
		position += 1.5f * Target.Forward;
		float num = 16f;
		foreach (ProjRecord projectile in m_Projectiles)
		{
			if (Vector3.SqrMagnitude(position - projectile.m_Pos) < num)
			{
				return false;
			}
		}
		float speed = Throw.ComputeMinSpeed(vector2, position);
		if (!Throw.ComputeVelocity(vector2, position, speed, ref Velocity))
		{
			return false;
		}
		return true;
	}

	private void FireAtTarget(Vector3 Velocity)
	{
		Transform transform = m_ProjList[m_ProjIndex];
		if (!(transform != null))
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(m_ProjPrefab) as GameObject;
		if (gameObject != null)
		{
			transform.gameObject._SetActiveRecursively(false);
			Item.InitData initData = new Item.InitData();
			initData.Owner = m_Owner;
			initData.Pos = transform.position;
			initData.Dir = Velocity * 0.1f;
			gameObject._SetActiveRecursively(true);
			gameObject.BroadcastMessage("Init", initData, SendMessageOptions.RequireReceiver);
			m_Audio.PlayOneShot(m_EjectionSound);
			CancelInvoke("StartDetection");
			m_PauseTimer = m_PauseDuration;
			m_DetectionInProgress = false;
			if (++m_ProjIndex >= m_ProjList.Length)
			{
				StartCoroutine(Deactivation());
			}
			m_FireLock = true;
			m_FireLockTimer = 1f;
			m_Projectiles.Add(new ProjRecord(gameObject.transform));
		}
	}
}
