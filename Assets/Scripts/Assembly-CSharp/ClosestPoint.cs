using UnityEngine;

public class ClosestPoint
{
	public static Vector3 PointRay(Vector3 P, Vector3 RayO, Vector3 RayD)
	{
		float num = Vector3.Dot(RayD, P - RayO);
		float num2 = Vector3.SqrMagnitude(RayD);
		return RayO + RayD * (num / num2);
	}

	public static Vector3 PointSegment(Vector3 P, Vector3 S0, Vector3 S1)
	{
		Vector3 vector = S1 - S0;
		Vector3 lhs = P - S0;
		float num = Vector3.Dot(lhs, vector);
		if (num <= 0f)
		{
			return S0;
		}
		float num2 = Vector3.SqrMagnitude(vector);
		return (!(num >= num2)) ? (S0 + vector * (num / num2)) : S1;
	}

	public static Vector3 PointBounds(Vector3 P, Bounds B)
	{
		Vector3 result = P;
		Vector3 vector = P - B.center;
		result.x -= Mathf.Sign(vector.x) * Mathf.Max(0f, Mathf.Abs(vector.x) - B.extents.x);
		result.y -= Mathf.Sign(vector.y) * Mathf.Max(0f, Mathf.Abs(vector.y) - B.extents.y);
		result.z -= Mathf.Sign(vector.z) * Mathf.Max(0f, Mathf.Abs(vector.z) - B.extents.z);
		return result;
	}

	public static Vector3 PointBoundsCenter(Vector3 P, Bounds B)
	{
		Vector3 vector = P - B.center;
		float num = B.extents.x / Mathf.Abs(vector.x);
		float num2 = B.extents.y / Mathf.Abs(vector.y);
		float num3 = B.extents.z / Mathf.Abs(vector.z);
		float num4 = Mathf.Min(num, num2, num3);
		num4 = Mathf.Max(0f, 1f - num4);
		return P - num4 * vector;
	}

	public static void LineLine(Vector3 a, Vector3 aD, Vector3 b, Vector3 bD, ref Vector3 aClosest, ref Vector3 bClosest)
	{
		float num = Vector3.Dot(aD, aD);
		float num2 = 0f - Vector3.Dot(aD, bD);
		float num3 = Vector3.Dot(bD, bD);
		float num4 = Mathf.Abs(num * num3 - num2 * num2);
		if (num4 > float.Epsilon)
		{
			Vector3 lhs = a - b;
			float num5 = Vector3.Dot(lhs, aD);
			float num6 = 0f - Vector3.Dot(lhs, bD);
			num4 = 1f / num4;
			float num7 = (num2 * num6 - num3 * num5) * num4;
			float num8 = (num2 * num5 - num * num6) * num4;
			aClosest = a + aD * num7;
			bClosest = b + bD * num8;
		}
		else
		{
			aClosest = a;
			bClosest = b;
		}
	}
}
