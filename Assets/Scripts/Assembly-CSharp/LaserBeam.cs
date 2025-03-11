using UnityEngine;

[AddComponentMenu("__CAPA__/Laser Beam")]
[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
	private Transform m_Transform;

	private MeshRenderer m_DotRenderer;

	private LineRenderer m_BeamRenderer;

	public float m_BeamMinWidth = 0.1f;

	public float m_BeamMaxWidth = 0.3f;

	public float m_BeamMaxLength = 8f;

	private float m_BeamPulseDuration = 0.5f;

	private RaycastHit m_HitInfo = default(RaycastHit);

	public RaycastHit HitInfo
	{
		get
		{
			return m_HitInfo;
		}
	}

	private void Awake()
	{
		m_Transform = base.gameObject.transform;
		m_BeamRenderer = base.gameObject.GetComponent<LineRenderer>();
		if (m_BeamRenderer != null)
		{
			m_BeamRenderer.castShadows = false;
			m_BeamRenderer.receiveShadows = false;
			m_BeamRenderer.useWorldSpace = false;
		}
		m_DotRenderer = base.gameObject.GetComponentInChildren<MeshRenderer>();
		if (m_DotRenderer != null)
		{
			m_DotRenderer.castShadows = false;
			m_DotRenderer.receiveShadows = false;
		}
	}

	private void Update()
	{
		Vector3 position = m_Transform.position;
		Vector3 end = m_Transform.position + m_Transform.forward * m_BeamMaxLength;
		bool flag = Physics.Linecast(position, end, out m_HitInfo);
		float num = ((!flag) ? m_BeamMaxLength : m_HitInfo.distance);
		m_BeamRenderer.SetPosition(1, num * Vector3.forward);
		m_BeamRenderer.material.SetTextureScale("_MainTex", new Vector2(0.1f * num, 1f));
		m_BeamRenderer.material.SetTextureOffset("_NoiseTex", new Vector2(-0.1f * Time.time, 0f));
		float num2 = m_BeamMaxWidth - m_BeamMinWidth;
		float num3 = num2 * 0.2f;
		float num4 = Mathf.PingPong(Time.time * m_BeamPulseDuration, 1f);
		float num5 = Random.Range(0f - num3, num3);
		float num6 = m_BeamMinWidth + Mathf.Clamp(num2 * num4 + num5, 0f, num2);
		m_BeamRenderer.SetWidth(num6, num6);
		if (m_DotRenderer != null)
		{
			m_DotRenderer.enabled = flag;
			if (flag)
			{
				m_DotRenderer.transform.position = m_HitInfo.point - m_Transform.forward * 0.01f;
				m_DotRenderer.transform.rotation = Quaternion.LookRotation(m_HitInfo.normal, m_Transform.up);
			}
		}
	}
}
