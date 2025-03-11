using UnityEngine;

public static class ComponentExtension
{
	public static string GetFullName(this Component inComponent)
	{
		return GameObjectUtils.GetFullName((!inComponent) ? null : inComponent.gameObject) + ", " + ((!inComponent) ? "Invalid Component" : inComponent.GetType().Name);
	}
}
