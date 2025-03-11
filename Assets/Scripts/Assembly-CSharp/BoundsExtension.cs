using UnityEngine;

public static class BoundsExtension
{
	private static Vector3 InvalidSize = new Vector3(-0.1f, -0.1f, -0.1f);

	public static bool IsValid(this Bounds B)
	{
		return B.size.x > 0f;
	}

	public static void Invalidate(this Bounds B)
	{
		B.size = InvalidSize;
	}

	public static void MergeTo(this Bounds B, ref Bounds Target)
	{
		if (Target.IsValid())
		{
			if (B.IsValid())
			{
				Vector3 vector = new Vector3(Mathf.Min(B.center.x - B.extents.x, Target.center.x - Target.extents.x), Mathf.Min(B.center.y - B.extents.y, Target.center.y - Target.extents.y), Mathf.Min(B.center.z - B.extents.z, Target.center.z - Target.extents.z));
				Vector3 vector2 = new Vector3(Mathf.Max(B.center.x + B.extents.x, Target.center.x + Target.extents.x), Mathf.Max(B.center.y + B.extents.y, Target.center.y + Target.extents.y), Mathf.Max(B.center.z + B.extents.z, Target.center.z + Target.extents.z));
				Target.center = (vector + vector2) * 0.5f;
				Target.extents = (vector2 - vector) * 0.5f;
			}
		}
		else
		{
			Target.center = B.center;
			Target.extents = B.extents;
		}
	}
}
