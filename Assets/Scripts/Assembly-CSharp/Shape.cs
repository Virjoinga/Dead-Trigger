using UnityEngine;

public class Shape
{
	public struct HitInfo
	{
		public Vector3 m_Point;

		public Vector3 m_Normal;

		public float m_Time;
	}

	protected float[] m_BBox = new float[6];

	public virtual int RayCast(Vector3 Origin, Vector3 Direction, HitInfo[] Hits)
	{
		return 0;
	}

	public virtual int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, HitInfo[] Hits)
	{
		return 0;
	}

	public virtual void UpdateBBox()
	{
	}
}
