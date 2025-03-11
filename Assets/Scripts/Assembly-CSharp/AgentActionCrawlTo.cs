public class AgentActionCrawlTo : AgentActionGoToBase
{
	public E_MotionSide MotionSide;

	public AgentActionCrawlTo()
		: base(AgentActionFactory.E_Type.CrawlTo)
	{
		MoveType = E_MoveType.Forward;
		Motion = E_MotionType.Crawl;
		MotionSide = E_MotionSide.Center;
		LookTarget = null;
		MinDistance = 0.3f;
		DontChangeParameters = false;
		UseNavMeshAgentRotation = true;
	}

	public override void Reset()
	{
		MoveType = E_MoveType.Forward;
		Motion = E_MotionType.Crawl;
		MotionSide = E_MotionSide.Center;
		LookTarget = null;
		MinDistance = 0.3f;
		DontChangeParameters = false;
		UseNavMeshAgentRotation = true;
	}
}
