using UnityEngine;

public class AgentActionDeath : AgentAction
{
	public AgentHuman Attacker;

	public E_CriticalHitType DecapType;

	public E_WeaponID FromWeapon;

	public E_WeaponType WeaponType;

	public Vector3 Impuls;

	public E_BodyPart BodyPart;

	public bool BodyDisintegrated = true;

	public float Damage;

	public AgentActionDeath()
		: base(AgentActionFactory.E_Type.Death)
	{
	}
}
