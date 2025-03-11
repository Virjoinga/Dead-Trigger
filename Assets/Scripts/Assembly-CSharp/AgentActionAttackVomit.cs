using UnityEngine;

public class AgentActionAttackVomit : AgentAction
{
	public Vector3 AttackDir;

	public E_WeaponAction WeaponAction = E_WeaponAction.Vomit;

	public AgentActionAttackVomit()
		: base(AgentActionFactory.E_Type.AttackVomit)
	{
	}
}
