public class AgentActionUseItem : AgentAction
{
	public bool Throw;

	public AgentActionUseItem()
		: base(AgentActionFactory.E_Type.UseItem)
	{
	}

	public override void Reset()
	{
		Throw = false;
	}
}
