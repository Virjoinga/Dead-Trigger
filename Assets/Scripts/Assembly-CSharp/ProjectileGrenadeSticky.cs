using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Items/ProjectileGrenade Sticky")]
public class ProjectileGrenadeSticky : ProjectileGrenadeFrag, IImportantObject
{
	private int m_StickToLayers = ObjectLayerMask.Default;

	public new void Update()
	{
		TryToStick();
		base.Update();
	}

	private void TryToStick()
	{
		Vector3 position = m_RBody.position;
		Vector3 vector = m_RBody.position + m_RBody.velocity * Time.deltaTime;
		Vector3 vector2 = vector - position;
		float magnitude = vector2.magnitude;
		vector2 /= magnitude;
		position -= 0.1f * vector2;
		magnitude += 0.1f;
		List<HitInfo> list = HitDetection.RayCast(position, vector2, magnitude, m_StickToLayers);
		foreach (HitInfo item in list)
		{
			if (item.data.collider == m_Collider || item.data.collider.isTrigger)
			{
				continue;
			}
			m_Collider.enabled = false;
			m_Transform.position = item.data.point;
			m_Transform.forward = item.data.normal;
			m_Transform.parent = item.data.transform;
			m_RBody.Sleep();
			SemanticMaterialManager.Instance.SpawnPlacementEffect(item.data);
			break;
		}
	}

	internal new void OnCollisionEnter(Collision Coll)
	{
		if (m_Owner != null && Coll.transform.IsChildOf(m_Owner.Transform))
		{
			return;
		}
		Agent component = Coll.gameObject.GetComponent<Agent>();
		AgentHuman agentHuman = component as AgentHuman;
		if (agentHuman != null && !agentHuman.BlackBoard.GrenadesExplodeOnHit)
		{
			if (m_HitSoundNum > 0)
			{
				PlayHitSound(Coll);
				m_HitSoundNum--;
			}
		}
		else if (m_WaterVolume == null && m_HitSoundNum > 0)
		{
			PlayHitSound(Coll);
			m_HitSoundNum--;
		}
		ComputeTrajectory();
	}
}
