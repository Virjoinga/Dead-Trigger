using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
	public AnimSet AnimSet;

	public AgentHuman Owner { get; private set; }

	public AnimComponent AnimComponent { get; private set; }

	public float MaxWalkSpeed { get; protected set; }

	public float MaxRunSpeed { get; protected set; }

	protected void Awake()
	{
		Owner = GetComponent<AgentHuman>();
		AnimComponent = GetComponent<AnimComponent>();
		SetProps();
		Owner.BlackBoard.BaseSetup.MaxWalkSpeed = MaxWalkSpeed;
		Owner.BlackBoard.BaseSetup.MaxRunSpeed = MaxRunSpeed;
	}

	protected virtual void SetProps()
	{
		Owner.BlackBoard.MovementSkill = F_MovementSkill.WalkAndRun;
		MaxWalkSpeed = 1.1f;
		MaxRunSpeed = 2.5f;
	}
}
