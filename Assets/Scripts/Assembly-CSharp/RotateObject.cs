using UnityEngine;

public class RotateObject : MonoBehaviour
{
	private Transform m_Transform;

	private Renderer m_Mesh;

	public float m_RotationSpeedX;

	public float m_RotationSpeedY;

	public float m_RotationSpeedZ;

	public float SpeedMultiplier { get; set; }

	private void Awake()
	{
		m_Transform = base.transform;
		m_Mesh = base.GetComponent<Renderer>();
		SpeedMultiplier = 1f;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Time.deltaTime != 0f && m_Mesh != null && m_Mesh.isVisible)
		{
			float num = TimeManager.Instance.GetRealDeltaTime() * SpeedMultiplier;
			if (num > 0.001f)
			{
				m_Transform.Rotate(m_RotationSpeedX * num, m_RotationSpeedY * num, m_RotationSpeedZ * num);
			}
		}
	}
}
