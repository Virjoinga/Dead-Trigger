using UnityEngine;

[AddComponentMenu("Utils/SineMover")]
public class SineMover : MonoBehaviour
{
	public float m_SineAmplitudeX;

	public float m_SineFreqX = 1f;

	public float m_SineAmplitudeY;

	public float m_SineFreqY = 1f;

	public float m_SineAmplitudeZ;

	public float m_SineFreqZ = 1f;

	private Transform m_Transform;

	private Vector3 m_BasePos = Vector3.zero;

	private void Start()
	{
		m_Transform = base.transform;
		if (m_Transform != null)
		{
			m_BasePos = m_Transform.position;
		}
	}

	private void LateUpdate()
	{
		if ((bool)m_Transform)
		{
			Vector3 zero = Vector3.zero;
			zero.x = m_SineAmplitudeX * Mathf.Sin(Time.time * m_SineFreqX);
			zero.y = m_SineAmplitudeY * Mathf.Sin(Time.time * m_SineFreqY);
			zero.z = m_SineAmplitudeZ * Mathf.Sin(Time.time * m_SineFreqZ);
			m_Transform.position = m_BasePos + zero;
		}
	}
}
