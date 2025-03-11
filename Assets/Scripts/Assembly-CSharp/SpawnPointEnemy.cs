using UnityEngine;

[AddComponentMenu("Game/Spawn Point - Enemy")]
public class SpawnPointEnemy : SpawnPoint
{
	public E_AgentType[] m_AllowedTypes;

	public bool m_CheckVisibility;

	private void Start()
	{
		base.enabled = false;
	}

	private void OnDestroy()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(base.transform.position, base.transform.parent.position);
		DrawSpawnPoint(Gizmos.color);
	}
}
