using UnityEngine;

public class DummyColliderCapsule : DummyCollider
{
	public enum E_Axis
	{
		X = 0,
		Y = 1,
		Z = 2
	}

	public Vector3 Center = Vector3.zero;

	public float Length = 1f;

	public float Radius = 0.5f;

	public E_Axis Axis = E_Axis.Y;

	private ShapeCapsule Capsule;

	public new void Awake()
	{
		base.Awake();
		Capsule = new ShapeCapsule(GetLength(), Radius);
	}

	public override void UpdateTransform()
	{
		Matrix4x4 Mat = base.TForm.localToWorldMatrix;
		Vector3 origin = base.TForm.TransformPoint(Center);
		Vector3 vector = Matrix.RemoveScale(ref Mat);
		Matrix.SetOrigin(ref Mat, origin);
		Capsule.SetFrame(Mat, (int)Axis);
		Capsule.SetScale(vector.x);
		base.enabled = vector.x > 0.1f;
	}

	public override void Scale(float Multiplier)
	{
		Length *= Multiplier;
		Radius *= Multiplier;
		Capsule.SetLength(GetLength());
		Capsule.SetRadius(Radius);
	}

	public override int RayCast(Vector3 Origin, Vector3 Direction, Shape.HitInfo[] Hits)
	{
		return Capsule.RayCast(Origin, Direction, Hits);
	}

	public override int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, Shape.HitInfo[] Hits)
	{
		return Capsule.SphereCast(Origin, Direction, Radius, Hits);
	}

	private float GetLength()
	{
		return Mathf.Max(0f, Length - 2f * Radius);
	}

	private void OnDrawGizmos()
	{
		if (Capsule != null)
		{
			DebugDraw.DisplayTime = 0f;
			DebugDraw.DepthTest = true;
			Capsule.Draw((!base.enabled) ? DummyCollider.ColOff : DummyCollider.ColOn);
		}
	}
}
