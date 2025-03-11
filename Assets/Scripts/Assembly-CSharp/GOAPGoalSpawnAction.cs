using UnityEngine;

internal class GOAPGoalSpawnAction : GOAPGoal
{
	public GOAPGoalSpawnAction(AgentHuman owner)
		: base(E_GOAPGoals.SpawnAction, owner)
	{
	}

	public override void InitGoal()
	{
		base.FailChance = 0;
	}

	public override float GetMaxRelevancy()
	{
		return 0.1f;
	}

	public override void CalculateGoalRelevancy()
	{
		base.GoalRelevancy = 0f;
		if (base.Owner.WorldState.GetWSProperty(E_PropKey.DoSpawnAction).GetBool())
		{
			base.GoalRelevancy = GetMaxRelevancy();
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = 1000f + Time.timeSinceLevelLoad;
	}

	public override void ChangeCurrentWSForPlanning(WorldState worldState)
	{
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		return false;
	}

	public override bool IsSatisfied()
	{
		return IsPlanFinished();
	}

	public override bool Activate(GOAPPlan plan)
	{
		base.Owner.BlackBoard.Desires.LookAtTarget = false;
		base.Owner.BlackBoard.SetFear(0f);
		base.Owner.BlackBoard.SetRage(0f);
		return base.Activate(plan);
	}

	public override void Deactivate()
	{
		base.Owner.BlackBoard.Desires.LookAtTarget = true;
		base.Owner.WorldState.SetWSProperty(E_PropKey.DoSpawnAction, false);
		base.Owner.NavMeshAgent.walkableMask &= ~(1 << LayerMask.NameToLayer("WalkOnlyWhenSpawn"));
		base.Owner.WeaponComponent.GetCurrentWeapon().SetBusy(0.4f);
		base.Deactivate();
	}
}
