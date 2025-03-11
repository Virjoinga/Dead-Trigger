using UnityEngine;

internal class GOAPActionGoToWeaponRange : GOAPAction
{
	private AgentActionGoTo Action;

	private Vector3 Position;

	private float NextPathRecalcTime;

	private bool PrevBerserk;

	private E_MotionType MotionType;

	public GOAPActionGoToWeaponRange(AgentHuman owner)
		: base(E_GOAPAction.GotoWeaponRange, owner)
	{
	}

	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.BodyPose, E_BodyPose.Stand);
		WorldEffects.SetWSProperty(E_PropKey.InWeaponRange, true);
		WorldEffects.SetWSProperty(E_PropKey.AtTargetPos, true);
		Owner.WorldState.SetWSProperty(E_PropKey.AtTargetPos, false);
		SetMotionType();
		Cost = 5f;
		Precedence = 20;
	}

	public override void Activate()
	{
		base.Activate();
		NextPathRecalcTime = 0f;
		PrevBerserk = false;
		Owner.WorldState.SetWSProperty(E_PropKey.Berserk, false);
		SetMotionType();
		Vector3 targetPos = ((!Owner.BlackBoard.DangerousEnemy) ? Owner.Transform.position : Owner.BlackBoard.DangerousEnemy.Transform.position);
		CreateAgentActionGoTo(targetPos);
	}

	public override void Update()
	{
		if (!Owner.BlackBoard.ActionPointOn && (bool)Owner.BlackBoard.DangerousEnemy)
		{
			CreateAgentActionGoTo(Owner.BlackBoard.DangerousEnemy.Transform.position);
		}
	}

	public override void Deactivate()
	{
		Owner.WorldState.SetWSProperty(E_PropKey.AtTargetPos, true);
		if (Action != null && Action.IsActive())
		{
			AgentActionIdle action = AgentActionFactory.Create(AgentActionFactory.E_Type.Idle) as AgentActionIdle;
			Owner.BlackBoard.ActionAdd(action);
		}
		base.Deactivate();
	}

	private void CreateAgentActionGoTo(Vector3 targetPos)
	{
		float num = float.PositiveInfinity;
		if (Action != null && Action.IsActive())
		{
			num = (Action.FinalPosition - targetPos).sqrMagnitude;
		}
		bool @bool = Owner.WorldState.GetWSProperty(E_PropKey.Berserk).GetBool();
		bool flag = PrevBerserk != @bool;
		if (num < 0.25f && NextPathRecalcTime > Time.timeSinceLevelLoad && !flag)
		{
			return;
		}
		NextPathRecalcTime = Time.timeSinceLevelLoad + 30f;
		if (@bool)
		{
			MotionType = E_MotionType.Run;
		}
		else if (flag)
		{
			SetMotionType();
		}
		Position = targetPos;
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Goto) as AgentActionGoTo;
		Action.FinalPosition = targetPos;
		Action.MoveType = E_MoveType.Forward;
		Action.Motion = MotionType;
		Action.DontChangeParameters = false;
		Action.ReselectMoveAnim = flag;
		if (@bool)
		{
			Action.MinDistance = 2f;
		}
		PrevBerserk = @bool;
		Owner.NavMeshAgent.stoppingDistance = 0f;
		if (Owner.BlackBoard.Desires.LookAtTarget && Owner.WorldState.GetWSProperty(E_PropKey.SeeEnemy).GetBool())
		{
			if ((bool)Owner.BlackBoard.VisibleTarget)
			{
				Action.LookTarget = Owner.BlackBoard.VisibleTarget.Transform;
			}
			else if ((bool)Owner.BlackBoard.DangerousEnemy)
			{
				Action.LookTarget = Owner.BlackBoard.DangerousEnemy.Transform;
			}
		}
		if (Action.LookTarget == null)
		{
			Action.UseNavMeshAgentRotation = true;
		}
		Owner.BlackBoard.ActionAdd(Action);
	}

	private void SetMotionType()
	{
		if (Owner.BlackBoard.MovementSkill == F_MovementSkill.Run)
		{
			MotionType = E_MotionType.Run;
		}
		else if (Owner.BlackBoard.MovementSkill == F_MovementSkill.Walk || Owner.BlackBoard.MovementSkill == F_MovementSkill.Berserk)
		{
			MotionType = E_MotionType.Walk;
		}
		else
		{
			MotionType = ((Random.Range(0, 2) != 0) ? E_MotionType.Walk : E_MotionType.Run);
		}
	}

	public override bool IsActionComplete()
	{
		if (Owner.BlackBoard.ActionPointOn)
		{
			return false;
		}
		if ((Owner.Transform.position - Position).sqrMagnitude < Owner.BlackBoard.sqrWeaponRange)
		{
			return true;
		}
		if (Action != null && Action.IsSuccess())
		{
			return true;
		}
		return false;
	}

	public override bool ValidateAction()
	{
		if (Action != null && Action.IsFailed())
		{
			return false;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return false;
		}
		if (Owner.GetAction(E_GOAPAction.AttackVomit) != null && Time.timeSinceLevelLoad >= Owner.BlackBoard.NextVomitTime && Owner.WorldState.GetWSProperty(E_PropKey.InVomitRange).GetBool())
		{
			Owner.BlackBoard.NextVomitTime = Time.timeSinceLevelLoad + Random.Range(7f, 14f);
			return false;
		}
		return true;
	}

	public override void SolvePlanWSVariable(WorldState currentState, WorldState goalState)
	{
		base.SolvePlanWSVariable(currentState, goalState);
	}

	public override void SetPlanWSPreconditions(WorldState goalState)
	{
		base.SetPlanWSPreconditions(goalState);
	}

	public override bool ValidateContextPreconditions(WorldState current, bool planning)
	{
		return true;
	}
}
