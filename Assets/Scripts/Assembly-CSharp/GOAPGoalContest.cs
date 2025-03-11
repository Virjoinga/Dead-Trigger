using UnityEngine;

internal class GOAPGoalContest : GOAPGoal
{
	public GOAPGoalContest(AgentHuman owner)
		: base(E_GOAPGoals.Contest, owner)
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
		return base.Owner.BlackBoard.GoapSetup.ContestRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		WorldStateProp wSProperty = base.Owner.WorldState.GetWSProperty(E_PropKey.Contest);
		if (wSProperty != null && wSProperty.GetBool())
		{
			base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.ContestRelevancy;
		}
		else
		{
			base.GoalRelevancy = 0f;
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = 0.05f + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.Contest, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp wSProperty = worldState.GetWSProperty(E_PropKey.Contest);
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
