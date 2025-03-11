using UnityEngine;

public class DummyColliderSphere : DummyCollider
{
	public Vector3 Center;

	public float Radius = 0.5f;

	private ShapeSphere Sphere;

	public new void Awake()
	{
		base.Awake();
		Sphere = new ShapeSphere(Radius);
	}

	public override void UpdateTransform()
	{
		Vector3 pos = base.TForm.TransformPoint(Center);
		float x = base.TForm.lossyScale.x;
		Sphere.SetPos(pos);
		Sphere.SetScale(x);
		base.enabled = x > 0.1f;
	}

	public override void Scale(float Multiplier)
	{
		Radius *= Multiplier;
		Sphere.SetRadius(Radius);
	}

	public override int RayCast(Vector3 Origin, Vector3 Direction, Shape.HitInfo[] Hits)
	{
		return Sphere.RayCast(Origin, Direction, Hits);
	}

	public override int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, Shape.HitInfo[] Hits)
	{
		return Sphere.SphereCast(Origin, Direction, Radius, Hits);
	}

	private void OnDrawGizmos()
	{
		if (Sphere != null)
		{
			DebugDraw.DisplayTime = 0f;
			DebugDraw.DepthTest = true;
			Sphere.Draw((!base.enabled) ? DummyCollider.ColOff : DummyCollider.ColOn);
		}
	}
}
