using UnityEngine;

public class AgentActionInjury : AgentAction
{
	public AgentHuman Attacker;

	public E_WeaponID FromWeapon;

	public E_WeaponType WeaponType;

	public Vector3 Impuls;

	public float Damage;

	public E_BodyPart BodyPart;

	public bool PlayAnim = true;

	public bool Destroy;

	public E_Direction Direction;

	public AgentActionInjury()
		: base(AgentActionFactory.E_Type.Injury)
	{
	}
}
