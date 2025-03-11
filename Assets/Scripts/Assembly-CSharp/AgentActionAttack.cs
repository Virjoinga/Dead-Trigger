using UnityEngine;

public class AgentActionAttack : AgentAction
{
	public Vector3 AttackDir;

	public AgentActionAttack()
		: base(AgentActionFactory.E_Type.Attack)
	{
	}
}
