using UnityEngine;

public static class Player
{
	public static ComponentPlayer Instance;

	public static float HealthSegment = 20f;

	public static Vector3 Pos
	{
		get
		{
			return Instance.Owner.Transform.position;
		}
	}

	public static Vector3 Dir
	{
		get
		{
			return Instance.Owner.Transform.forward;
		}
	}
}
