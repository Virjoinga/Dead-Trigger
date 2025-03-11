using UnityEngine;

internal class GOAPGoalCheckBait : GOAPGoal
{
	public GOAPGoalCheckBait(AgentHuman owner)
		: base(E_GOAPGoals.CheckBait, owner)
	{
	}

	public override void InitGoal()
	{
	}

	public override bool Activate(GOAPPlan plan)
	{
		base.Owner.BlackBoard.Desires.LookAtTarget = true;
		return base.Activate(plan);
	}

	public override float GetMaxRelevancy()
	{
		return base.Owner.BlackBoard.GoapSetup.CheckBaitRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		WorldStateProp wSProperty = base.Owner.WorldState.GetWSProperty(E_PropKey.CheckBait);
		if (wSProperty != null && wSProperty.GetBool())
		{
			base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.CheckBaitRelevancy;
		}
		else
		{
			base.GoalRelevancy = 0f;
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = 0.1f + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.CheckBait, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp wSProperty = worldState.GetWSProperty(E_PropKey.CheckBait);
		if (wSProperty != null && !wSProperty.GetBool())
		{
			return true;
		}
		return false;
	}

	public override bool IsSatisfied()
	{
		return IsPlanFinished();
	}
}
