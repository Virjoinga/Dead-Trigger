public class AgentActionGoTo : AgentActionGoToBase
{
	public bool ReselectMoveAnim;

	public AgentActionGoTo()
		: base(AgentActionFactory.E_Type.Goto)
	{
	}

	public override void Reset()
	{
		MoveType = E_MoveType.Forward;
		Motion = E_MotionType.Run;
		LookTarget = null;
		MinDistance = 0.5f;
		DontChangeParameters = false;
		UseNavMeshAgentRotation = true;
		ReselectMoveAnim = false;
	}
}
