using UnityEngine;

[AddComponentMenu("Entities/Entry Point")]
public class EntryPoint : MonoBehaviour
{
	private static readonly Color Col = new Color(1f, 0.5f, 0f, 1f);

	public Transform Transform { get; private set; }

	private void Awake()
	{
		Transform = base.gameObject.transform;
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

	private void OnDrawGizmos()
	{
		Transform transform = base.gameObject.transform;
		Vector3 from = transform.position + transform.forward * 0.2f;
		Vector3 to = transform.position + transform.forward * 0.6f;
		DrawOrientedLine(Color.white, from, to);
		Gizmos.color = Col;
		Gizmos.DrawLine(transform.position, transform.parent.position);
		Gizmos.DrawIcon(transform.position, "EntryPoint.png", true);
	}
}
