using UnityEngine;

public static class GameObjectExtension
{
	public static T GetFirstComponentUpwardWithInterface<T>(this GameObject inGameObject) where T : class
	{
		return GameObjectUtils.GetFirstComponentUpwardWithInterface<T>(inGameObject);
	}

	public static T GetFirstComponentUpward<T>(this GameObject inGameObject) where T : Component
	{
		return GameObjectUtils.GetFirstComponentUpward<T>(inGameObject);
	}

	public static T GetComponentWithInterface<T>(this GameObject inGameObject) where T : class
	{
		return GameObjectUtils.GetComponentWithInterface<T>(inGameObject);
	}

	public static string GetFullName(this GameObject inGameObject)
	{
		return GameObjectUtils.GetFullName(inGameObject);
	}

	public static T[] GetComponentsInChildrenWithInterface<T>(this GameObject inGameObject) where T : class
	{
		return inGameObject.GetComponentsInChildrenWithInterface<T>(false);
	}

	public static T[] GetComponentsInChildrenWithInterface<T>(this GameObject inGameObject, bool inIncludeInactive) where T : class
	{
		return GameObjectUtils.GetComponentsInChildrenWithInterface<T>(inGameObject, inIncludeInactive);
	}

	public static void _SetActiveRecursively(this GameObject inGameObject, bool inActive)
	{
		if (!(inGameObject == null))
		{
			_SetActiveRecursively(inGameObject.transform, inActive);
		}
	}

	private static void _SetActiveRecursively(Transform inTransform, bool inActive)
	{
		if (inTransform == null)
		{
			return;
		}
		inTransform.gameObject.SetActive(inActive);
		foreach (Transform item in inTransform)
		{
			_SetActiveRecursively(item, inActive);
		}
	}

	public static void SetActive(this GameObject inGameObject, bool inActive)
	{
		if (!(inGameObject == null))
		{
			inGameObject.SetActive(inActive);
		}
	}
}
