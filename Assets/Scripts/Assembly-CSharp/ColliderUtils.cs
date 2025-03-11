using UnityEngine;

public class ColliderUtils
{
	public static bool PointInCollider(Vector3 Point, BoxCollider Box)
	{
		return PointInCollider(Point, Box, Box.transform);
	}

	public static bool PointInCollider(Vector3 Point, BoxCollider Box, Transform BoxTransform)
	{
		Vector3 vector = BoxTransform.worldToLocalMatrix.MultiplyPoint(Point);
		Vector3 vector2 = vector - Box.center;
		return Mathf.Abs(vector2.x) <= Box.size.x && Mathf.Abs(vector2.y) <= Box.size.y && Mathf.Abs(vector2.z) <= Box.size.z;
	}

	public static bool PointInCollider(Vector3 Point, CapsuleCollider Capsule)
	{
		return PointInCollider(Point, Capsule, Capsule.transform);
	}

	public static bool PointInCollider(Vector3 Point, CapsuleCollider Capsule, Transform CapsuleTransform)
	{
		Vector3 vector = CapsuleTransform.worldToLocalMatrix.MultiplyPoint(Point);
		Vector3 zero = Vector3.zero;
		zero[Capsule.direction] = Capsule.height / 2f;
		Vector3 vector2 = ClosestPoint.PointSegment(vector, Capsule.center - zero, Capsule.center + zero);
		Vector3 a = vector - vector2;
		return Vector3.SqrMagnitude(a) <= Capsule.radius * Capsule.radius;
	}

	public static bool PointInCollider(Vector3 Point, SphereCollider Sphere)
	{
		return PointInCollider(Point, Sphere, Sphere.transform);
	}

	public static bool PointInCollider(Vector3 Point, SphereCollider Sphere, Transform SphereTransform)
	{
		Vector3 vector = SphereTransform.worldToLocalMatrix.MultiplyPoint(Point);
		Vector3 a = vector - Sphere.center;
		return Vector3.SqrMagnitude(a) <= Sphere.radius * Sphere.radius;
	}

	public static bool ScaleCollider(Collider Coll, float Scale)
	{
		BoxCollider boxCollider = Coll as BoxCollider;
		if (boxCollider != null)
		{
			ScaleCollider(boxCollider, Scale);
			return true;
		}
		CapsuleCollider capsuleCollider = Coll as CapsuleCollider;
		if (capsuleCollider != null)
		{
			ScaleCollider(capsuleCollider, Scale);
			return true;
		}
		SphereCollider sphereCollider = Coll as SphereCollider;
		if (sphereCollider != null)
		{
			ScaleCollider(sphereCollider, Scale);
			return true;
		}
		return false;
	}

	public static void ScaleCollider(BoxCollider Box, float Scale)
	{
		Box.size *= Scale;
	}

	public static void ScaleCollider(CapsuleCollider Capsule, float Scale)
	{
		Capsule.radius *= Scale;
		Capsule.height *= Scale;
	}

	public static void ScaleCollider(SphereCollider Sphere, float Scale)
	{
		Sphere.radius *= Scale;
	}
}
