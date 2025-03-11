public class AgentActionRotate : AgentAction
{
	public E_RotationType Rotation;

	public float Angle;

	public AgentActionRotate()
		: base(AgentActionFactory.E_Type.Rotate)
	{
	}
}
