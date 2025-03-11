using UnityEngine;

public class DummyColliderBox : DummyCollider
{
	public Vector3 Center;

	public Vector3 Size = Vector3.one;

	private ShapeBox Box;

	public new void Awake()
	{
		base.Awake();
		Box = new ShapeBox(Size * 0.5f);
	}

	public override void UpdateTransform()
	{
		Matrix4x4 Mat = base.TForm.localToWorldMatrix;
		Vector3 origin = base.TForm.TransformPoint(Center);
		Vector3 vector = Matrix.RemoveScale(ref Mat);
		Matrix.SetOrigin(ref Mat, origin);
		Box.SetFrame(Mat);
		Box.SetScale(vector.x);
		base.enabled = vector.x > 0.1f;
	}

	public override void Scale(float Multiplier)
	{
		Size *= Multiplier;
		Box.SetExtents(Size * 0.5f);
	}

	public override int RayCast(Vector3 Origin, Vector3 Direction, Shape.HitInfo[] Hits)
	{
		return Box.RayCast(Origin, Direction, Hits);
	}

	public override int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, Shape.HitInfo[] Hits)
	{
		return Box.SphereCast(Origin, Direction, Radius, Hits);
	}

	private void OnDrawGizmos()
	{
		if (Box != null)
		{
			DebugDraw.DisplayTime = 0f;
			DebugDraw.DepthTest = true;
			Box.Draw((!base.enabled) ? DummyCollider.ColOff : DummyCollider.ColOn);
		}
	}
}
