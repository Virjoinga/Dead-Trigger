using System;
using UnityEngine;

public class LogResources : MonoBehaviour
{
	private struct CompRecord
	{
		public GameObject m_Owner;

		public bool m_Active;
	}

	public float m_Delay = 1f;

	private void Start()
	{
		Invoke("Do", m_Delay);
	}

	private void Update()
	{
	}

	private void Do()
	{
		LogTextures();
	}

	public static void LogObjects()
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));
		for (int i = 0; i < array.Length; i++)
		{
		}
	}

	public static void LogComponents(Type ComponentType, bool ActiveOnly, bool IncludingPrefabs)
	{
	}

	public static void LogTextures()
	{
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Texture));
		for (int i = 0; i < array.Length; i++)
		{
			Texture texture = array[i] as Texture;
			Texture2D texture2D = array[i] as Texture2D;
		}
	}
}
