using UnityEngine;

[AddComponentMenu("Game/Spawn Zone")]
public class SpawnZone : SpawnZoneBase, IGameZoneChild, IGameZoneChild_AutoRegister
{
	private void Start()
	{
		SpawnManager.Instance.RegisterSpawnZone(this);
		base.gameObject._SetActiveRecursively(false);
	}

	[NESAction]
	public override void Enable()
	{
		base.Enable();
		PickupAutoCollectZone[] componentsInChildren = GetComponentsInChildren<PickupAutoCollectZone>();
		PickupAutoCollectZone[] array = componentsInChildren;
		foreach (PickupAutoCollectZone pickupAutoCollectZone in array)
		{
			pickupAutoCollectZone.Enable();
		}
		SpawnManager.Instance.OnSpawnZoneStateChange(this);
	}

	[NESAction]
	public override void Disable()
	{
		base.Disable();
		PickupAutoCollectZone[] componentsInChildren = GetComponentsInChildren<PickupAutoCollectZone>();
		PickupAutoCollectZone[] array = componentsInChildren;
		foreach (PickupAutoCollectZone pickupAutoCollectZone in array)
		{
			pickupAutoCollectZone.Disable();
		}
		SpawnManager.Instance.OnSpawnZoneStateChange(this);
	}

	public bool IsActivatedWithGameZone()
	{
		return false;
	}

	private void OnDrawGizmos()
	{
	}

	void IGameZoneChild.Reset()
	{
		Reset();
	}
}
