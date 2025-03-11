using UnityEngine;

public class AgentActionGoToBase : AgentAction
{
	public Vector3 FinalPosition;

	public E_MoveType MoveType;

	public E_MotionType Motion = E_MotionType.Run;

	public Transform LookTarget;

	public float MinDistance = 0.5f;

	public bool DontChangeParameters;

	public bool UseNavMeshAgentRotation = true;

	public AgentActionGoToBase(AgentActionFactory.E_Type type)
		: base(type)
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
	}
}
