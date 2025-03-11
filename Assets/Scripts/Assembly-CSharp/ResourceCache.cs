#define DEBUG
using System.Collections.Generic;
using UnityEngine;

internal class ResourceCache
{
	private GameObject m_Resource;

	private string m_ResourceName;

	private int m_CacheSize;

	private int m_CacheSizeIncrement;

	private List<GameObject> m_FreeObjects = new List<GameObject>();

	public ResourceCache(GameObject Resource, int InitSize)
		: this(Resource, InitSize, InitSize)
	{
	}

	public ResourceCache(GameObject Resource, int InitSize, int SizeIncrement)
	{
		DebugUtils.Assert(InitSize > 0 || SizeIncrement > 0);
		m_Resource = Resource;
		m_ResourceName = Resource.GetFullName();
		m_CacheSize = 0;
		m_CacheSizeIncrement = Mathf.Max(0, SizeIncrement);
		if (InitSize > 0)
		{
			AllocateObjects(InitSize);
		}
	}

	public ResourceCache(string ResourceName, int InitSize)
		: this(ResourceName, InitSize, InitSize)
	{
	}

	public ResourceCache(string ResourceName, int InitSize, int SizeIncrement)
	{
		DebugUtils.Assert(InitSize > 0 || SizeIncrement > 0);
		m_Resource = null;
		m_ResourceName = ResourceName;
		m_CacheSize = 0;
		m_CacheSizeIncrement = Mathf.Max(0, SizeIncrement);
		if (InitSize > 0)
		{
			AllocateObjects(InitSize);
		}
	}

	~ResourceCache()
	{
		m_Resource = null;
		m_FreeObjects.Clear();
	}

	public GameObject Get()
	{
		if (m_FreeObjects.Count == 0)
		{
			if (m_CacheSizeIncrement == 0)
			{
				Debug.LogWarning("ResourceCache::Get() ... Cache for '" + m_ResourceName + "' is empty and set as non-resize-able!");
				return null;
			}
			AllocateObjects(m_CacheSizeIncrement);
		}
		GameObject result = null;
		if (m_FreeObjects.Count > 0)
		{
			result = m_FreeObjects[0];
			m_FreeObjects.RemoveAt(0);
		}
		return result;
	}

	public void Return(GameObject Obj)
	{
		if (Obj == null)
		{
			Debug.LogWarning("ResourceCache::Return() ... Given object is 'null'!");
		}
		else if (m_FreeObjects.Contains(Obj))
		{
			Debug.LogWarning(string.Concat("ResourceCache::Return() ... Given object '", Obj, "' is already in 'free' list!"));
		}
		else
		{
			m_FreeObjects.Add(Obj);
		}
	}

	public int GetFreeNum()
	{
		return m_FreeObjects.Count;
	}

	public int GetUsedNum()
	{
		return m_CacheSize - m_FreeObjects.Count;
	}

	public bool IsResizeable()
	{
		return m_CacheSizeIncrement > 0;
	}

	public GameObject GetResource()
	{
		return m_Resource;
	}

	public string GetResourceName()
	{
		return m_ResourceName;
	}

	internal void AllocateObjects(int Num)
	{
		if (m_Resource == null)
		{
			m_Resource = Resources.Load(m_ResourceName) as GameObject;
			if (m_Resource == null)
			{
				Debug.LogWarning("ResourceCache::AllocateObjects() ... Resource '" + m_ResourceName + "' not loaded!");
				return;
			}
		}
		while (Num-- > 0)
		{
			GameObject gameObject = Object.Instantiate(m_Resource) as GameObject;
			gameObject._SetActiveRecursively(false);
			gameObject.name += m_CacheSize;
			m_CacheSize++;
			m_FreeObjects.Add(gameObject);
		}
	}
}
