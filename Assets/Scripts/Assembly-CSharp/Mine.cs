using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Items/Mine")]
[RequireComponent(typeof(Animation))]
public class Mine : MonoBehaviour
{
	private const float MaxPitchMod = 0.1f;

	private AgentHuman m_Owner;

	private AudioSource m_Audio;

	private Animation m_Animation;

	private Transform m_Transform;

	public float m_ActivationDelay = 1f;

	private bool m_CountingDown;

	public float m_DetectionDistance = 4f;

	public float m_DetectionDuration = 1.5f;

	public float m_DetectionPause = 1f;

	public AudioClip m_DetectionSound;

	private float m_DetectionStartTime;

	private bool m_DetectionInProgress;

	public GameObject m_Detector;

	public float m_DetectorMaxScale = 2f;

	public Explosion m_Explosion;

	public Transform m_ExplosionOrigin;

	public float m_ExplosionDamageCoef = -1f;

	public float m_ExplosionEffRadius = -1f;

	public float m_ExplosionMaxDmgRadius = -1f;

	public GameObject m_BlastMark;

	public void Init(Item.InitData Data)
	{
		m_Owner = Data.Owner;
		base.transform.position = Data.Pos;
	}

	private void Start()
	{
		GameObject gameObject = base.gameObject;
		m_Transform = gameObject.transform;
		m_Audio = gameObject.GetComponent<AudioSource>();
		m_Audio.pitch += Random.Range(-0.1f, 0.1f);
		m_Animation = gameObject.GetComponent<Animation>();
		m_CountingDown = false;
		m_DetectionInProgress = false;
		m_DetectionStartTime = -1f;
		if (m_BlastMark != null)
		{
			m_BlastMark.SetActive(false);
		}
		if (m_Detector != null)
		{
			m_Detector.transform.localScale = new Vector3(2f * m_DetectionDistance, 2f * m_DetectionDistance, 1f);
			MeshRenderer component = m_Detector.GetComponent<MeshRenderer>();
			Vector4 vector = new Vector4(m_DetectionDuration, m_DetectorMaxScale, 0f - Time.time - m_ActivationDelay, m_DetectionPause);
			component.material.SetVector("_Params", vector);
		}
		InvokeRepeating("StartDetection", m_ActivationDelay, m_DetectionDuration + m_DetectionPause);
	}

	private void Update()
	{
		if (m_CountingDown)
		{
			if (!m_Animation.isPlaying)
			{
				Explode();
			}
		}
		else if (m_DetectionInProgress && UpdateDetection())
		{
			CancelInvoke();
			m_Audio.Play();
			m_Animation.Play();
			m_CountingDown = true;
		}
	}

	private void StartDetection()
	{
		m_DetectionInProgress = true;
		m_DetectionStartTime = Time.time;
		m_Audio.PlayOneShot(m_DetectionSound);
	}

	private bool UpdateDetection()
	{
		float num = (Time.time - m_DetectionStartTime) / m_DetectionDuration;
		if (num >= 1f)
		{
			num = 1f;
			m_DetectionInProgress = false;
		}
		float num2 = 2f;
		float num3 = m_DetectionDistance * num;
		num3 *= num3;
		foreach (Agent enemy in Mission.Instance.CurrentGameZone.Enemies)
		{
			Vector3 a = enemy.Position - m_Transform.position;
			float num4 = Vector3.SqrMagnitude(a);
			if (num4 > num3 || a.y < -0.3f - num2 || a.y > 0.3f)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private void Explode()
	{
		m_Audio.Stop();
		m_CountingDown = false;
		Vector3 inPosition = ((!(m_ExplosionOrigin != null)) ? m_Transform.position : m_ExplosionOrigin.position);
		Explosion explosion = Mission.Instance.ExplosionCache.Get(m_Explosion, inPosition, Quaternion.identity);
		if (explosion != null)
		{
			explosion.causer = m_Owner;
			if (m_ExplosionDamageCoef > 0f)
			{
				explosion.damage = m_ExplosionDamageCoef * (float)GameplayData.Instance.EnemyHealth();
			}
			if (m_ExplosionEffRadius > 0f)
			{
				explosion.damageRadius = m_ExplosionEffRadius;
			}
			if (m_ExplosionMaxDmgRadius > 0f)
			{
				explosion.damageMaxRadius = m_ExplosionMaxDmgRadius;
			}
		}
		base.gameObject._SetActiveRecursively(false);
		if (m_BlastMark != null)
		{
			m_BlastMark.SetActive(true);
		}
	}
}
