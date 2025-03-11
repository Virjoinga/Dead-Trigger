using UnityEngine;

public static class ColliderExtension
{
	public static bool IsPointInside(this BoxCollider C, Vector3 P)
	{
		return ColliderUtils.PointInCollider(P, C);
	}

	public static bool IsPointInside(this CapsuleCollider C, Vector3 P)
	{
		return ColliderUtils.PointInCollider(P, C);
	}

	public static bool IsPointInside(this SphereCollider C, Vector3 P)
	{
		return ColliderUtils.PointInCollider(P, C);
	}

	public static void Scale(this BoxCollider C, float Scale)
	{
		ColliderUtils.ScaleCollider(C, Scale);
	}

	public static void Scale(this CapsuleCollider C, float Scale)
	{
		ColliderUtils.ScaleCollider(C, Scale);
	}

	public static void Scale(this SphereCollider C, float Scale)
	{
		ColliderUtils.ScaleCollider(C, Scale);
	}

	public static void Scale(this Collider C, float Scale)
	{
		if (!ColliderUtils.ScaleCollider(C, Scale))
		{
			Debug.LogWarning("Not implemented for this collider type!");
		}
	}
}
