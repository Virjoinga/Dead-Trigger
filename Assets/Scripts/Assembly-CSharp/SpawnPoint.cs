using UnityEngine;

[NESEvent(new string[] { "On Spawn" })]
public class SpawnPoint : MonoBehaviour
{
	private const float Size = 0.3f;

	private NESController m_NESController;

	public Transform Transform { get; private set; }

	public float RespawnTime { get; protected set; }

	private void Awake()
	{
		Transform = base.gameObject.transform;
		RespawnTime = 2f;
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (!(m_NESController == null))
		{
		}
	}

	public void OnSpawn()
	{
		if (m_NESController != null)
		{
			m_NESController.SendGameEvent(this, "On Spawn");
		}
	}

	private static void DrawOrientedLine(Color Col, Vector3 From, Vector3 To)
	{
		Vector3 tangent = default(Vector3);
		Vector3 binormal = default(Vector3);
		Vector3 normal = To - From;
		Vector3.OrthoNormalize(ref normal, ref tangent, ref binormal);
		Vector3 vector = To - normal * 0.12f;
		tangent *= 0.08f;
		binormal *= 0.08f;
		Gizmos.color = Col;
		Gizmos.DrawLine(To, From);
		Gizmos.DrawLine(To, vector + tangent);
		Gizmos.DrawLine(To, vector - tangent);
		Gizmos.DrawLine(To, vector + binormal);
		Gizmos.DrawLine(To, vector - binormal);
	}

	protected void DrawSpawnPoint(Color Col)
	{
		Transform transform = base.gameObject.transform;
		Vector3 from = transform.position + transform.forward * 0.3f;
		Vector3 to = transform.position + transform.forward * 0.6f;
		DrawOrientedLine(Color.white, from, to);
		Gizmos.color = Col;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
	}

	[ContextMenu("Snap To Ground")]
	public void SnapToGround()
	{
		Transform transform = base.gameObject.transform;
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position, -Vector3.up, out hitInfo, 5f))
		{
			transform.position = hitInfo.point + Vector3.up * 0.6f;
		}
	}
}
