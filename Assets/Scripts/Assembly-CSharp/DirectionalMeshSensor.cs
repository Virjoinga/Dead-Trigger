using System.Collections.Generic;
using UnityEngine;

public class DirectionalMeshSensor : MonoBehaviour
{
	public float m_MaxLength = 2f;

	public bool m_IgnoreTriggers = true;

	public int m_LayerMask = 1;

	private Transform m_XForm;

	private MeshRenderer m_Renderer;

	private HitInfo m_HitInfo = new HitInfo();

	public bool HitFound
	{
		get
		{
			return m_HitInfo.data.distance >= 0f;
		}
	}

	public HitInfo HitInfo
	{
		get
		{
			return m_HitInfo;
		}
	}

	private void Awake()
	{
		GameObject gameObject = base.gameObject;
		m_XForm = gameObject.transform;
		m_Renderer = gameObject.GetComponent<MeshRenderer>();
		m_HitInfo.data.distance = -1f;
	}

	private void Update()
	{
		Vector3 position = m_XForm.position;
		Vector3 forward = m_XForm.forward;
		Vector3 vector = m_XForm.worldToLocalMatrix.MultiplyVector(forward);
		m_HitInfo.data.distance = -1f;
		if (m_MaxLength > 0f)
		{
			List<HitInfo> list = HitDetection.RayCast(position, forward, m_MaxLength);
			foreach (HitInfo item in list)
			{
				if (!m_IgnoreTriggers || !item.data.collider.isTrigger)
				{
					item.CopyTo(m_HitInfo);
					break;
				}
			}
		}
		float w = ((!(m_HitInfo.data.distance > 0f)) ? m_MaxLength : m_HitInfo.data.distance);
		m_Renderer.material.SetVector("_Scaling", new Vector4(vector.x, vector.y, vector.z, w));
	}
}
