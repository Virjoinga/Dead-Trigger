using UnityEngine;

internal class GOAPGoalKillTarget : GOAPGoal
{
	private static float _DisabledForEverybodyTimer;

	protected override float DisabledForEverybodyTimer
	{
		get
		{
			return _DisabledForEverybodyTimer;
		}
		set
		{
			_DisabledForEverybodyTimer = value;
		}
	}

	public GOAPGoalKillTarget(AgentHuman owner)
		: base(E_GOAPGoals.KillTarget, owner)
	{
	}

	public override void InitGoal()
	{
		base.FailChance = 10;
		_DisabledForEverybodyTimer = 0f;
		DisabledForEverybodyDelay = 1f;
	}

	public override float GetMaxRelevancy()
	{
		return base.Owner.BlackBoard.GoapSetup.KillTargetRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		base.GoalRelevancy = 0f;
		if (!(base.Owner.BlackBoard.VisibleTarget == null) && base.Owner.CanFire())
		{
			float num = 1f;
			num *= base.Owner.BlackBoard.Rage / 100f;
			if (!(num < 0.15f))
			{
				num = Mathf.Clamp(num, 0f, 1f);
				base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.KillTargetRelevancy * num;
			}
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = Random.Range(0.1f, base.Owner.BlackBoard.GoapSetup.KillTargetDelay) + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.KillTarget, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		return worldState.GetWSProperty(E_PropKey.KillTarget).GetBool();
	}

	public override bool IsSatisfied()
	{
		return IsPlanFinished();
	}

	public override bool Activate(GOAPPlan plan)
	{
		base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.KillTargetRelevancy;
		base.Owner.BlackBoard.SetFear(0f);
		base.Owner.BlackBoard.Desires.LookAtTarget = true;
		return base.Activate(plan);
	}

	public override void Deactivate()
	{
		base.Owner.BlackBoard.SetRage(0f);
		base.Deactivate();
	}

	public override void Reset()
	{
		base.Reset();
	}
}
