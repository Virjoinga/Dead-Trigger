using UnityEngine;

public abstract class DummyCollider : MonoBehaviour
{
	protected static readonly Color ColOn = new Color(1f, 0.5f, 0.2f);

	protected static readonly Color ColOff = new Color(0.4f, 0.4f, 0.4f);

	protected static readonly Color ColBounds = new Color(0.3f, 0.3f, 0.3f);

	public PhysicMaterial m_PhysMaterial;

	public Transform TForm { get; protected set; }

	public void Awake()
	{
		TForm = base.gameObject.transform;
		if (!Mathf.Approximately(TForm.localScale.x, 1f) || !Mathf.Approximately(TForm.localScale.y, 1f) || !Mathf.Approximately(TForm.localScale.z, 1f))
		{
			TForm.localScale = Vector3.one;
			Debug.LogWarning("DummyCollider::Awake() ... Local scale on '" + TForm.gameObject.GetFullName() + "' set back to [1,1,1] !!!");
		}
	}

	public virtual int RayCast(Vector3 Origin, Vector3 Direction, Shape.HitInfo[] Hits)
	{
		return 0;
	}

	public virtual int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, Shape.HitInfo[] Hits)
	{
		return 0;
	}

	public virtual void UpdateTransform()
	{
	}

	public virtual void Scale(float Multiplier)
	{
	}
}
