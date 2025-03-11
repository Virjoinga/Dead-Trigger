using UnityEngine;

internal class GOAPGoalDestroyObject : GOAPGoal
{
	public GOAPGoalDestroyObject(AgentHuman owner)
		: base(E_GOAPGoals.DestroyObject, owner)
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
		return base.Owner.BlackBoard.GoapSetup.DestroyObjectRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		base.GoalRelevancy = 0f;
		WorldStateProp wSProperty = base.Owner.WorldState.GetWSProperty(E_PropKey.DestroyObject);
		if (!(wSProperty == null) && wSProperty.GetBool() && !base.Owner.WorldState.GetWSProperty(E_PropKey.InWeaponRange).GetBool())
		{
			base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.DestroyObjectRelevancy;
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = 0.1f + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.DestroyObject, false);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		WorldStateProp wSProperty = worldState.GetWSProperty(E_PropKey.DestroyObject);
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
