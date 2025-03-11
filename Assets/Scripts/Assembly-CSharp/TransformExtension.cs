using UnityEngine;

public static class TransformExtension
{
	public static Transform FindChildByName(this Transform inTransform, string inName)
	{
		return GameObjectUtils.FindChildByName(inTransform, inName);
	}

	public static T GetChildComponent<T>(this Transform inTransform, string inName) where T : Component
	{
		Transform transform = GameObjectUtils.FindChildByName(inTransform, inName);
		return transform.GetComponent<T>();
	}
}
