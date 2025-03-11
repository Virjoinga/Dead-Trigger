using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerCameraShowSomething : MonoBehaviour
{
	[Serializable]
	public class Waypoint
	{
		public Transform Transform;

		public float Time = 1f;

		public GameObject TrackObject;
	}

	public List<Waypoint> WayPoints;

	public bool DisableAfterUse;

	private void OnDrawGizmos()
	{
		if (WayPoints == null)
		{
			return;
		}
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube((base.GetComponent<Collider>() as BoxCollider).center + base.transform.position, (base.GetComponent<Collider>() as BoxCollider).size);
		for (int i = 0; i < WayPoints.Count; i++)
		{
			Gizmos.DrawSphere(WayPoints[i].Transform.position, 0.5f);
			if (WayPoints[i].TrackObject != null)
			{
				Gizmos.DrawWireSphere(WayPoints[i].TrackObject.transform.position, 0.2f);
				Gizmos.DrawLine(WayPoints[i].Transform.position, WayPoints[i].TrackObject.transform.position);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (WayPoints == null)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube((base.GetComponent<Collider>() as BoxCollider).center + base.transform.position, (base.GetComponent<Collider>() as BoxCollider).size);
		for (int i = 0; i < WayPoints.Count; i++)
		{
			Gizmos.DrawSphere(WayPoints[i].Transform.position, 0.5f);
			if (WayPoints[i].TrackObject != null)
			{
				Gizmos.DrawSphere(WayPoints[i].TrackObject.transform.position, 0.2f);
				Gizmos.DrawLine(WayPoints[i].Transform.position, WayPoints[i].TrackObject.transform.position);
			}
		}
	}

	public void Disable()
	{
		base.gameObject._SetActiveRecursively(false);
	}
}
