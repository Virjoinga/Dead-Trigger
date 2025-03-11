using System.Collections.Generic;
using UnityEngine;

public class DummyColliderCollection : MonoBehaviour
{
	private class Record
	{
		public DummyCollider m_Collider;

		public HitZone m_HitZone;
	}

	protected static readonly Color Col = new Color(1f, 0.5f, 0.2f, 0.3f);

	public Transform m_Root;

	private List<Record> m_Records = new List<Record>();

	private Shape.HitInfo[] m_Hits = new Shape.HitInfo[2];

	private bool m_Dirty = true;

	private void Awake()
	{
		if (m_Root != null)
		{
			CreateRecords(m_Root);
		}
		if (!(base.gameObject.GetComponent<Collider>() == null))
		{
		}
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		m_Dirty = true;
	}

	public bool VerifyRayCast(HitInfo Hit, Vector3 Origin, Vector3 Direction, float Distance)
	{
		if (m_Dirty)
		{
			UpdateTransforms();
			m_Dirty = false;
		}
		Vector3 vector = Origin + Direction * (Hit.data.distance - 0.5f);
		Vector3 to = vector + Direction * 2.5f;
		Record record = null;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Vector3.zero;
		float num = float.MaxValue;
		foreach (Record record2 in m_Records)
		{
			if (!record2.m_Collider.enabled)
			{
				continue;
			}
			int num2 = record2.m_Collider.RayCast(Origin, Direction, m_Hits);
			if (num2 > 0 && m_Hits[0].m_Time < num)
			{
				num = m_Hits[0].m_Time;
				if (num <= Distance)
				{
					record = record2;
					vector2 = m_Hits[0].m_Point;
					vector3 = m_Hits[0].m_Normal;
				}
			}
		}
		if (record != null)
		{
			Hit.data.point = vector2;
			Hit.data.normal = vector3;
			Hit.data.distance = num;
			Hit.dummyCollider = record.m_Collider;
			Hit.hitZone = record.m_HitZone;
			DebugDraw.DisplayTime = 0.08f;
			DebugDraw.Line(Color.gray, vector, to);
			DebugDraw.Diamond(Color.red, 0.02f, vector2);
			DebugDraw.Line(Color.red, vector2, vector2 + vector3 * 0.3f);
			return true;
		}
		return false;
	}

	public bool VerifySphereCast(HitInfo Hit, Vector3 Origin, float Radius, Vector3 Direction, float Distance)
	{
		if (m_Dirty)
		{
			UpdateTransforms();
			m_Dirty = false;
		}
		Vector3 vector = Origin + Direction * (Hit.data.distance - 0.5f);
		Vector3 to = vector + Direction * 2.5f;
		Record record = null;
		Vector3 point = Vector3.zero;
		Vector3 normal = Vector3.zero;
		float num = float.MaxValue;
		foreach (Record record2 in m_Records)
		{
			if (!record2.m_Collider.enabled)
			{
				continue;
			}
			int num2 = record2.m_Collider.SphereCast(Origin, Direction, Radius, m_Hits);
			if (num2 > 0 && m_Hits[0].m_Time < num)
			{
				num = m_Hits[0].m_Time;
				if (num <= Distance)
				{
					record = record2;
					point = m_Hits[0].m_Point;
					normal = m_Hits[0].m_Normal;
				}
			}
		}
		if (record != null)
		{
			Hit.data.point = point;
			Hit.data.normal = normal;
			Hit.data.distance = num;
			Hit.dummyCollider = record.m_Collider;
			Hit.hitZone = record.m_HitZone;
			DebugDraw.DisplayTime = 0.08f;
			DebugDraw.Line(Color.gray, vector, to);
			DebugDraw.Sphere(Color.blue, Radius, Origin + Direction * num);
			DebugDraw.Diamond(Color.red, 0.02f, Hit.data.point);
			DebugDraw.Line(Color.red, Hit.data.point, Hit.data.point + Hit.data.normal * 0.3f);
			return true;
		}
		return false;
	}

	private void CreateRecords(Transform Parent)
	{
		foreach (Transform item in Parent)
		{
			CreateRecords(item);
			DummyCollider component = item.GetComponent<DummyCollider>();
			if (!(component == null))
			{
				HitZone component2 = item.GetComponent<HitZone>();
				m_Records.Add(new Record
				{
					m_HitZone = component2,
					m_Collider = component
				});
			}
		}
	}

	private void UpdateTransforms()
	{
		foreach (Record record in m_Records)
		{
			record.m_Collider.UpdateTransform();
		}
	}

	private void OnDrawGizmos()
	{
		GameObject gameObject = base.gameObject;
		Collider collider = gameObject.GetComponent<Collider>();
		if (!(collider == null))
		{
			Gizmos.color = Col;
			Gizmos.matrix = gameObject.transform.localToWorldMatrix;
			if (collider is BoxCollider)
			{
				BoxCollider boxCollider = collider as BoxCollider;
				Gizmos.DrawCube(boxCollider.center, boxCollider.size);
			}
			else if (collider is SphereCollider)
			{
				SphereCollider sphereCollider = collider as SphereCollider;
				Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);
			}
			else if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = collider as CapsuleCollider;
				Vector3 size = new Vector3(2f * capsuleCollider.radius, 2f * capsuleCollider.radius, 2f * capsuleCollider.radius);
				int direction;
				int index = (direction = capsuleCollider.direction);
				float num = size[direction];
				size[index] = num + Mathf.Max(0f, capsuleCollider.height - size.x);
				Gizmos.DrawCube(capsuleCollider.center, size);
			}
			Gizmos.matrix = Matrix4x4.identity;
		}
	}
}
