using UnityEngine;

[AddComponentMenu("Game/Spawn Point - Player")]
public class SpawnPointPlayer : SpawnPoint
{
	private void Start()
	{
		if (base.gameObject.transform.rotation.eulerAngles.z == 0f)
		{
		}
	}

	private void OnDrawGizmos()
	{
		DrawSpawnPoint(Color.blue);
	}

	[ContextMenu("Use As Start-Point")]
	public void UseAsStart()
	{
		Object[] array = Object.FindObjectsOfType(typeof(SpawnPointPlayer));
		if (array != null)
		{
			Object[] array2 = array;
			foreach (Object @object in array2)
			{
				(@object as SpawnPointPlayer).enabled = false;
			}
		}
		base.enabled = true;
	}
}
