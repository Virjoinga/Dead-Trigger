using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowInitializer : MonoBehaviour
{
	[Serializable]
	public class DynamicObstacleInfo
	{
		public DynamicDLObstacle Obstacle;

		public DynamicDLObstacle.E_State State;
	}

	[Serializable]
	public class SpawnZoneInfo
	{
		public SpawnZone Zone;

		public bool Enabled = true;
	}

	[Serializable]
	public class PickupAutoCollectZoneInfo
	{
		public PickupAutoCollectZone Zone;

		public bool Enabled = true;
	}

	[Serializable]
	public class ObjectInfo
	{
		public GameObject Obj;

		public bool Enabled = true;
	}

	public List<DynamicObstacleInfo> DynamicObstacles;

	public List<SpawnZoneInfo> SpawnZones;

	public List<PickupAutoCollectZoneInfo> PickupZones;

	public List<ObjectInfo> Objects;

	private bool FirstUpdate;

	private void Start()
	{
		FirstUpdate = true;
		foreach (DynamicObstacleInfo dynamicObstacle in DynamicObstacles)
		{
			switch (dynamicObstacle.State)
			{
			case DynamicDLObstacle.E_State.Closed:
				dynamicObstacle.Obstacle.Close();
				break;
			case DynamicDLObstacle.E_State.FullyOpen:
				dynamicObstacle.Obstacle.FullyOpen();
				break;
			case DynamicDLObstacle.E_State.OpenForAI:
				dynamicObstacle.Obstacle.OpenForAi();
				break;
			default:
				Debug.LogWarning("Unknown enum: " + dynamicObstacle.State);
				break;
			}
		}
		foreach (ObjectInfo @object in Objects)
		{
			bool flag = false;
			if (@object.Obj == null)
			{
				continue;
			}
			DisableBasedOnPerformance firstComponentUpward = GameObjectUtils.GetFirstComponentUpward<DisableBasedOnPerformance>(@object.Obj);
			if (@object.Enabled && firstComponentUpward != null)
			{
				if (firstComponentUpward.UseForDevicePerformance == DeviceInfo.Performance.Medium)
				{
					if (DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Low)
					{
						flag = true;
					}
				}
				else if (firstComponentUpward.UseForDevicePerformance == DeviceInfo.Performance.High && (DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Low || DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Medium))
				{
					flag = true;
				}
			}
			if (flag)
			{
				@object.Obj._SetActiveRecursively(false);
			}
			else
			{
				@object.Obj._SetActiveRecursively(@object.Enabled);
			}
			if (firstComponentUpward != null)
			{
				firstComponentUpward.gameObject.SetActive(true);
			}
		}
	}

	private void Update()
	{
		if (!FirstUpdate)
		{
			return;
		}
		FirstUpdate = false;
		foreach (SpawnZoneInfo spawnZone in SpawnZones)
		{
			if (spawnZone.Enabled)
			{
				spawnZone.Zone.Enable();
			}
			else
			{
				spawnZone.Zone.Disable();
			}
		}
		foreach (PickupAutoCollectZoneInfo pickupZone in PickupZones)
		{
			if (pickupZone.Enabled)
			{
				pickupZone.Zone.Enable();
			}
			else
			{
				pickupZone.Zone.Disable();
			}
		}
	}
}
