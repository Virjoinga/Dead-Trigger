using UnityEngine;

public class AnimStateDeathRagdoll : AnimState
{
	private Vector3 StartPosition;

	private Vector3 FinalPosition;

	private Quaternion FinalRotation;

	private Quaternion StartRotation;

	private float RotationProgress;

	private float MoveTime;

	private float CurrentMoveTime;

	private AgentActionDeath Action;

	public AnimStateDeathRagdoll(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
		Debug.Log("AnimStateDeathRagdoll IS OBSOLETE! Use AnimStateDeath instead. Agent=" + Owner.name);
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		Animation.animatePhysics = true;
	}

	public override void Update()
	{
	}

	public override void Reset()
	{
		Action.SetSuccess();
	}

	public override void Release()
	{
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionDeath)
		{
			action.SetFailed();
			return true;
		}
		return false;
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionDeath;
		Animation.Stop();
		if (Action.WeaponType == E_WeaponType.ReviveKit)
		{
			Owner.Dissolve(0f);
		}
		else
		{
			Owner.Dissolve(2f);
		}
		Owner.NavMeshAgent.Stop();
		Owner.NavMeshAgent.enabled = false;
		Owner.EnableRagdoll(true);
		Vector3 force = ((Action.WeaponType == E_WeaponType.Explosion) ? (Action.Impuls + Vector3.up * 2f) : ((Action.WeaponType != E_WeaponType.Explosion) ? Action.Impuls : ((Action.Impuls + Vector3.up) * 2000f)));
		Owner.RigidBodyForce.AddForce(force);
		Owner.BlackBoard.MotionType = E_MotionType.Death;
	}
}
