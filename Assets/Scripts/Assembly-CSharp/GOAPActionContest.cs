using UnityEngine;

internal class GOAPActionContest : GOAPAction
{
	private AgentActionContest Action;

	public GOAPActionContest(AgentHuman owner)
		: base(E_GOAPAction.Contest, owner)
	{
	}

	public override void InitAction()
	{
		if (!Owner.IsPlayer)
		{
			WorldPreconditions.SetWSProperty(E_PropKey.InContestRange, true);
			WorldPreconditions.SetWSProperty(E_PropKey.Contest, true);
			WorldEffects.SetWSProperty(E_PropKey.KillTarget, true);
		}
		WorldEffects.SetWSProperty(E_PropKey.Contest, false);
		Cost = 1f;
		Precedence = 100;
		Interruptible = false;
	}

	public override void Activate()
	{
		base.Activate();
		if (Owner.ContestEnemy == null)
		{
			Debug.Log(string.Concat("GOAPActionContest: Zombie=", Owner.name, ", DangerousEnemy=", Owner.BlackBoard.DangerousEnemy, ", VisibleTarget=", Owner.BlackBoard.VisibleTarget, ", Contest=", Owner.WorldState.GetWSProperty(E_PropKey.Contest).GetBool()));
		}
		CreateAgentActionContest(Owner.ContestEnemy);
	}

	public override void Update()
	{
		if (!Owner.IsAlive && (bool)Action.Enemy)
		{
			Action.Enemy.StopContest(Owner);
		}
	}

	public override void Deactivate()
	{
		Owner.WorldState.SetWSProperty(E_PropKey.AtTargetPos, true);
		Owner.WorldState.SetWSProperty(E_PropKey.Contest, false);
		AgentActionIdle action = AgentActionFactory.Create(AgentActionFactory.E_Type.Idle) as AgentActionIdle;
		Owner.BlackBoard.ActionAdd(action);
		base.Deactivate();
	}

	private void CreateAgentActionContest(AgentHuman enemy)
	{
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.Contest) as AgentActionContest;
		Action.Enemy = enemy;
		Action.Time = 5f;
		Owner.BlackBoard.ActionAdd(Action);
	}

	public override bool IsActionComplete()
	{
		if (Owner.BlackBoard.ActionPointOn)
		{
			return false;
		}
		if (!Action.IsActive())
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
		return true;
	}

	public override bool ValidateContextPreconditions(WorldState current, bool planning)
	{
		if ((bool)Owner.BlackBoard.VisibleTarget && Owner.BlackBoard.VisibleTarget.IsInContest())
		{
			return false;
		}
		return base.ValidateContextPreconditions(current, planning);
	}
}
