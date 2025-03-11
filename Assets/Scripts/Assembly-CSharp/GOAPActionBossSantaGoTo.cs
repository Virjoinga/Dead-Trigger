using System.Collections;
using UnityEngine;

internal class GOAPActionBossSantaGoTo : GOAPAction
{
	private AgentActionGoTo Action;

	private Vector3 Position;

	private float NextPathRecalcTime;

	private static float NextLookAroundTime;

	private static bool RunLikeHell;

	private E_MotionType MotionType;

	public GOAPActionBossSantaGoTo(AgentHuman owner)
		: base(E_GOAPAction.BossSantaGoto, owner)
	{
	}

	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.BodyPose, E_BodyPose.Stand);
		WorldEffects.SetWSProperty(E_PropKey.InWeaponRange, true);
		WorldEffects.SetWSProperty(E_PropKey.AtTargetPos, true);
		Owner.WorldState.SetWSProperty(E_PropKey.AtTargetPos, false);
		NextLookAroundTime = 0f;
		RunLikeHell = false;
		SetMotionType();
		Cost = 1f;
		Precedence = 20;
	}

	public override void Activate()
	{
		base.Activate();
		NextPathRecalcTime = 0f;
		SetMotionType();
		Owner.StartCoroutine(_DoSetInvulnerable(1f));
		Vector3 targetPos = ((!Owner.BlackBoard.DangerousEnemy) ? Owner.Transform.position : Owner.BlackBoard.DangerousEnemy.Transform.position);
		CreateAgentActionGoTo(targetPos);
	}

	public override void Update()
	{
		if (!Owner.BlackBoard.ActionPointOn && (bool)Owner.BlackBoard.DangerousEnemy)
		{
			if (Owner.IsInvulnerable)
			{
				Owner.BlackBoard.SetFear(0f);
			}
			if (Time.timeSinceLevelLoad > NextLookAroundTime && Owner.BlackBoard.Fear > 50f)
			{
				NextLookAroundTime = Time.timeSinceLevelLoad + 100f;
				NextPathRecalcTime = 0f;
				RunLikeHell = true;
			}
			else
			{
				CreateAgentActionGoTo(Owner.BlackBoard.DangerousEnemy.Transform.position);
			}
		}
	}

	private IEnumerator _DoPlayAnim(string animName)
	{
		yield return new WaitForSeconds(Owner.PlayAnim(animName, E_TriState.True) - 0.2f);
		Owner.BlackBoard.Invulnerable = false;
	}

	private IEnumerator _DoSetInvulnerable(float time)
	{
		Owner.BlackBoard.Invulnerable = true;
		Owner.BlackBoard.ReactOnHits = false;
		yield return new WaitForSeconds(time);
		Owner.BlackBoard.Invulnerable = false;
		Owner.BlackBoard.ReactOnHits = true;
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
		if (num < 0.25f && NextPathRecalcTime > Time.timeSinceLevelLoad)
		{
			return;
		}
		NextPathRecalcTime = Time.timeSinceLevelLoad + 30f;
		Position = targetPos;
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Goto) as AgentActionGoTo;
		Action.FinalPosition = targetPos;
		Action.MoveType = E_MoveType.Forward;
		Action.Motion = MotionType;
		Action.DontChangeParameters = false;
		Action.ReselectMoveAnim = false;
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
		if (RunLikeHell)
		{
			MotionType = E_MotionType.Run;
		}
		else
		{
			MotionType = ((Random.Range(0, 3) < 1) ? E_MotionType.Walk : E_MotionType.Run);
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
