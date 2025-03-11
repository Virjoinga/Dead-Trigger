public class AgentActionWeaponChange : AgentAction
{
	public E_WeaponID NewWeapon;

	public AgentActionWeaponChange()
		: base(AgentActionFactory.E_Type.WeaponChange)
	{
	}
}
