using UnityEngine;

public class AgentActionRoll : AgentAction
{
	public Vector3 Direction;

	public AgentHuman ToTarget;

	public AgentActionRoll()
		: base(AgentActionFactory.E_Type.Roll)
	{
	}

	public override void Reset()
	{
		ToTarget = null;
	}
}
