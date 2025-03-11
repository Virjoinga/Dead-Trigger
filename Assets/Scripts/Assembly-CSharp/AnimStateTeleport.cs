using UnityEngine;

public class AnimStateTeleport : AnimState
{
	private AgentActionTeleport Action;

	public AnimStateTeleport(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void Release()
	{
		SetFinished(true);
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
	}

	public override void OnDeactivate()
	{
		Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		Action.SetSuccess();
		Action = null;
		base.Reset();
	}

	public override bool HandleNewAction(AgentAction action)
	{
		return false;
	}

	public override void Update()
	{
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionTeleport;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		Debug.LogError("beny: we do not have Teleport (yet) in DeadLand! Support removed!!!");
	}
}
