using System;
using UnityEngine;

internal class GameObjectUtils
{
	public static T GetFirstComponentUpward<T>(GameObject inGameObject) where T : Component
	{
		if (inGameObject == null)
		{
			return (T)null;
		}
		T component = inGameObject.GetComponent<T>();
		if ((UnityEngine.Object)component != (UnityEngine.Object)null)
		{
			return component;
		}
		if (inGameObject.transform.parent == null || inGameObject.transform.parent.gameObject == null)
		{
			return (T)null;
		}
		return GetFirstComponentUpward<T>(inGameObject.transform.parent.gameObject);
	}

	public static T GetComponentWithInterface<T>(GameObject inGameObject) where T : class
	{
		Component[] components = inGameObject.GetComponents<Component>();
		foreach (Component component in components)
		{
			T val = component as T;
			if (val != null)
			{
				return val;
			}
		}
		return (T)null;
	}

	public static T GetFirstComponentUpwardWithInterface<T>(GameObject inGameObject) where T : class
	{
		if (inGameObject != null)
		{
			Component[] components = inGameObject.GetComponents<Component>();
			foreach (Component component in components)
			{
				T val = component as T;
				if (val != null)
				{
					return val;
				}
			}
			if (inGameObject.transform.parent != null && inGameObject.transform.parent.gameObject != null)
			{
				return GetFirstComponentUpwardWithInterface<T>(inGameObject.transform.parent.gameObject);
			}
		}
		return (T)null;
	}

	public static T[] GetComponentsInChildrenWithInterface<T>(GameObject inGameObject, bool inIncludeInactive) where T : class
	{
		if (inGameObject != null)
		{
			MonoBehaviour[] componentsInChildren = inGameObject.GetComponentsInChildren<MonoBehaviour>(inIncludeInactive);
			T[] array = new T[componentsInChildren.Length];
			int num = 0;
			MonoBehaviour[] array2 = componentsInChildren;
			foreach (MonoBehaviour monoBehaviour in array2)
			{
				T val = monoBehaviour as T;
				if (val != null)
				{
					array[num++] = val;
				}
			}
			if (num > 0)
			{
				Array.Resize(ref array, num);
				return array;
			}
		}
		return null;
	}

	public static string GetFullName(GameObject inObject)
	{
		if ((bool)inObject)
		{
			if ((bool)inObject.transform.parent)
			{
				return GetFullName(inObject.transform.parent.gameObject) + "/" + inObject.name;
			}
			return inObject.name;
		}
		return string.Empty;
	}

	public static Transform FindChildByName(Transform inTransform, string inName)
	{
		foreach (Transform item in inTransform)
		{
			if (item.name == inName)
			{
				return item;
			}
			Transform transform2 = FindChildByName(item, inName);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}
}
