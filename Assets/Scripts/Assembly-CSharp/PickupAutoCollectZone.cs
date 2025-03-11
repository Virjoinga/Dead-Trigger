using UnityEngine;

[AddComponentMenu("Entities/PickupAutoCollectZone")]
[RequireComponent(typeof(BoxCollider))]
public class PickupAutoCollectZone : MonoBehaviour, IGameZoneChild, IGameZoneChild_AutoRegister
{
	private static readonly Color Col = new Color(0.2f, 0.2f, 0.8f, 0.5f);

	private GameObject m_Object;

	private Transform m_XForm;

	private BoxCollider m_Box;

	public bool m_ActivateWithGZ = true;

	private void Awake()
	{
		m_Object = base.gameObject;
		m_Object.layer = ObjectLayer.IgnoreRaycast;
		m_XForm = m_Object.transform;
		m_Box = m_Object.GetComponent<Collider>() as BoxCollider;
		if (m_Box != null)
		{
			m_Box.isTrigger = true;
		}
		m_Object._SetActiveRecursively(false);
	}

	private void Update()
	{
	}

	[NESAction]
	public void Enable()
	{
		m_Object._SetActiveRecursively(true);
		PickupManager.Instance.RegisterAutoCollectZone(this);
	}

	[NESAction]
	public void Disable()
	{
		m_Object._SetActiveRecursively(false);
		PickupManager.Instance.UnregisterAutoCollectZone(this);
	}

	public void Reset()
	{
	}

	public bool IsActivatedWithGameZone()
	{
		return m_ActivateWithGZ;
	}

	public bool IsInside(Vector3 Point)
	{
		return ColliderUtils.PointInCollider(Point, m_Box, m_XForm);
	}

	private void OnDrawGizmos()
	{
		m_Box = base.GetComponent<Collider>() as BoxCollider;
		m_XForm = base.gameObject.transform;
		if (m_Box != null)
		{
			Gizmos.matrix = m_XForm.localToWorldMatrix;
			Gizmos.color = Col;
			Gizmos.DrawCube(m_Box.center, m_Box.size);
			Gizmos.matrix = Matrix4x4.identity;
		}
	}
}
