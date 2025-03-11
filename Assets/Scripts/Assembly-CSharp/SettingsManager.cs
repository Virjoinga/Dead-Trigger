using System.Collections.Generic;
using UnityEngine;

public abstract class SettingsManager<T, Key> : ScriptableObject where T : Settings<Key>
{
	protected Dictionary<Key, T> Objects = new Dictionary<Key, T>();

	public T Get(Key key)
	{
		T value;
		if (!Objects.TryGetValue(key, out value))
		{
			Debug.LogWarning("SettingsManager: Can't find settings for " + key);
			return (T)null;
		}
		return value;
	}

	public T[] GetAll()
	{
		T[] array = new T[Objects.Count];
		Objects.Values.CopyTo(array, 0);
		return array;
	}

	public void LoadShopWidgets()
	{
		InternalLoadShopWidgets();
	}

	protected bool Init(string resourcePath)
	{
		GameObject gameObject = Resources.Load(resourcePath) as GameObject;
		if (gameObject == null)
		{
			Debug.LogWarning("Cant find prefab: " + resourcePath);
			return false;
		}
		T[] componentsInChildren = gameObject.GetComponentsInChildren<T>(true);
		T[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			T val = array[i];
			if (Objects.ContainsKey(val.ID))
			{
				Debug.LogWarning("Duplicite obj id: " + val.ToString());
			}
			else
			{
				Objects[val.ID] = val;
			}
		}
		return true;
	}

	protected void InternalLoadShopWidgets()
	{
		foreach (KeyValuePair<Key, T> @object in Objects)
		{
			if (!(@object.Value.ShopWidgetPrefab != null))
			{
				string shopWidgetPrefabName = @object.Value.ShopWidgetPrefabName;
				GameObject gameObject = Resources.Load(shopWidgetPrefabName) as GameObject;
				@object.Value.ShopWidgetPrefab = ((!gameObject) ? null : gameObject.transform);
				if (!(gameObject == null))
				{
				}
			}
		}
	}
}
