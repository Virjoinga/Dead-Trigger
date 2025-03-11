public class AgentActionContest : AgentAction
{
	public AgentHuman Enemy;

	public float Time;

	public AgentActionContest()
		: base(AgentActionFactory.E_Type.Contest)
	{
	}
}
