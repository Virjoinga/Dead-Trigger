using UnityEngine;

public class AgentActionAttackMelee : AgentAction
{
	public Vector3 AttackDir;

	public E_WeaponAction WeaponAction = E_WeaponAction.MeleeLeft;

	public AgentActionAttackMelee()
		: base(AgentActionFactory.E_Type.AttackMelee)
	{
	}
}
