using UnityEngine;

internal class GOAPGoalUseGadget : GOAPGoal
{
	public GOAPGoalUseGadget(AgentHuman owner)
		: base(E_GOAPGoals.UseGadget, owner)
	{
	}

	public override void InitGoal()
	{
	}

	public override float GetMaxRelevancy()
	{
		return base.Owner.BlackBoard.GoapSetup.UseWorlObjectRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		WorldStateProp wSProperty = base.Owner.WorldState.GetWSProperty(E_PropKey.UseGadget);
		if (wSProperty != null && wSProperty.GetBool())
		{
			base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.UseWorlObjectRelevancy;
		}
		else
		{
			base.GoalRelevancy = 0f;
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = base.Owner.BlackBoard.GoapSetup.UseWorlObjectDelay + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.UseGadget, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp wSProperty = worldState.GetWSProperty(E_PropKey.UseGadget);
		if (!wSProperty.GetBool())
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
