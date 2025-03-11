public class AgentActionPlayAnim : AgentAction
{
	public string AnimName;

	public E_TriState Invulnerable;

	public AgentActionPlayAnim()
		: base(AgentActionFactory.E_Type.PlayAnim)
	{
	}
}
