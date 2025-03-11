using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ExplodableObjectPart : MonoBehaviour
{
	private const float FadeOutDuration = 1f;

	private Collider m_Collider;

	private Rigidbody m_RBody;

	private MeshRenderer m_Renderer;

	private float m_SleepTimer;

	private float m_FadeOutTimer = -1f;

	public float m_ImpulseMin;

	public float m_ImpulseMax;

	private int m_HitSoundNum;

	public int m_HitSoundLimit = 3;

	public AudioClip[] m_HitSounds;

	private float m_Time2ImpactSound;

	private ParticleSystem[] m_Particles;

	public void Awake()
	{
		GameObject gameObject = base.gameObject;
		m_Collider = gameObject.GetComponent<Collider>();
		m_Renderer = gameObject.GetComponent<MeshRenderer>();
		m_RBody = gameObject.GetComponent<Rigidbody>();
		m_RBody.sleepVelocity = 0.2f;
		m_RBody.sleepAngularVelocity = 4f;
		m_Particles = gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	private void Start()
	{
		Vector3 vector = MathUtils.RandomVectorInsideCone(base.transform.forward, 15f);
		float num = Random.Range(m_ImpulseMin, m_ImpulseMax);
		m_RBody.velocity = vector * num;
		m_RBody.angularVelocity = Random.insideUnitSphere * 50f;
		m_RBody.WakeUp();
		ParticleSystem[] particles = m_Particles;
		foreach (ParticleSystem particleSystem in particles)
		{
			particleSystem.Play();
		}
		m_Time2ImpactSound = 0.2f;
	}

	public void Update()
	{
		if (m_FadeOutTimer < 0f)
		{
			m_SleepTimer += Time.deltaTime;
			m_Time2ImpactSound -= Time.deltaTime;
			if (Mathf.Abs(m_RBody.velocity.z) > 0.1f || m_RBody.velocity.magnitude > m_RBody.sleepVelocity || m_RBody.angularVelocity.magnitude > m_RBody.sleepAngularVelocity)
			{
				m_SleepTimer = 0f;
			}
			else if (m_SleepTimer > 0.5f)
			{
				FadeOutStart();
			}
		}
		else
		{
			FadeOutUpdate();
		}
	}

	private void FadeOutStart()
	{
		m_FadeOutTimer = 0f;
		m_RBody.Sleep();
		m_Collider.enabled = false;
		ParticleSystem[] particles = m_Particles;
		foreach (ParticleSystem particleSystem in particles)
		{
			particleSystem.Stop();
		}
	}

	private void FadeOutUpdate()
	{
		m_FadeOutTimer += Time.deltaTime;
		float num = Mathf.Min(1f, m_FadeOutTimer / 1f);
		m_Renderer.material.SetFloat("_Alpha", 1f - num);
		if (num == 1f)
		{
			bool flag = true;
			ParticleSystem[] particles = m_Particles;
			foreach (ParticleSystem particleSystem in particles)
			{
				flag &= !particleSystem.IsAlive(true);
			}
			if (flag)
			{
				base.gameObject._SetActiveRecursively(false);
			}
		}
	}

	private void OnCollisionEnter(Collision Coll)
	{
		PlayImpactSound(Coll, true);
	}

	private void OnCollisionStay(Collision Coll)
	{
		PlayImpactSound(Coll, false);
	}

	private void PlayImpactSound(Collision Coll, bool NewCollision)
	{
		if (m_HitSoundNum < m_HitSoundLimit && m_HitSounds.Length != 0 && !(m_Time2ImpactSound > 0f))
		{
			float num = ((!NewCollision) ? 0.5f : 0.3f);
			float magnitude = Coll.relativeVelocity.magnitude;
			if (!(magnitude <= num) && !(Vector3.Dot(Coll.contacts[0].normal, Coll.relativeVelocity / magnitude) >= 0f))
			{
				AudioClip clip = m_HitSounds[Random.Range(0, m_HitSounds.Length)];
				SemanticMaterialManager.Instance.Audio.PlayOneShot(clip);
				m_HitSoundNum++;
				m_Time2ImpactSound = 0.3f;
			}
		}
	}
}
