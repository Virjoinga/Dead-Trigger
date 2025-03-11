using UnityEngine;

internal class GOAPGoalTeleport : GOAPGoal
{
	private static float _DisabledForEverybodyTimer;

	private AgentHuman Enemy;

	private static WayPoint LastWaypoint;

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

	public GOAPGoalTeleport(AgentHuman owner)
		: base(E_GOAPGoals.Teleport, owner)
	{
	}

	public override void InitGoal()
	{
		DisabledForEverybodyDelay = 2f;
	}

	public override float GetMaxRelevancy()
	{
		return base.Owner.BlackBoard.GoapSetup.TeleportRelevancy;
	}

	public override void CalculateGoalRelevancy()
	{
		base.GoalRelevancy = 0f;
		if (base.Owner.BlackBoard.VisibleTarget == null && base.Owner.BlackBoard.DangerousEnemy == null)
		{
			return;
		}
		float num = base.Owner.BlackBoard.Fear / 100f;
		float num2 = base.Owner.BlackBoard.Rage / 100f;
		if (num < 0.2f && num2 < 0.2f)
		{
			return;
		}
		if ((bool)base.Owner.BlackBoard.VisibleTarget && num > num2)
		{
			LastWaypoint = GetAnyTeleportPositionAgainstEnemy(base.Owner.BlackBoard.VisibleTarget, base.Owner.BlackBoard.CombatRange, base.Owner.BlackBoard.WeaponRange);
			if (!(LastWaypoint == null))
			{
				Enemy = base.Owner.BlackBoard.VisibleTarget;
				base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.TeleportRelevancy * num;
			}
		}
		else
		{
			LastWaypoint = GetTeleportPositionBehindEnemy(base.Owner.BlackBoard.DangerousEnemy, base.Owner.BlackBoard.CombatRange, base.Owner.BlackBoard.WeaponRange);
			if (!(LastWaypoint == null))
			{
				Enemy = base.Owner.BlackBoard.DangerousEnemy;
				base.GoalRelevancy = base.Owner.BlackBoard.GoapSetup.TeleportRelevancy * num2;
			}
		}
	}

	public override void SetDisableTime()
	{
		base.NextEvaluationTime = base.Owner.BlackBoard.GoapSetup.TeleportDelay + Time.timeSinceLevelLoad;
	}

	public override void SetWSSatisfactionForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.Teleport, false);
	}

	public override void ChangeCurrentWSForPlanning(WorldState worldState)
	{
		worldState.SetWSProperty(E_PropKey.Teleport, true);
	}

	public override bool IsWSSatisfiedForPlanning(WorldState worldState)
	{
		return !worldState.GetWSProperty(E_PropKey.Teleport).GetBool();
	}

	public override bool IsSatisfied()
	{
		return IsPlanFinished();
	}

	public override bool Activate(GOAPPlan plan)
	{
		base.Owner.WorldState.SetWSProperty(E_PropKey.Teleport, true);
		base.Owner.BlackBoard.Desires.TeleportDestination = LastWaypoint.Position;
		base.Owner.BlackBoard.Desires.TeleportRotation.SetLookRotation((Enemy.Position - LastWaypoint.Position).normalized);
		base.Owner.BlackBoard.SetFear(0f);
		base.Owner.BlackBoard.SetRage(0f);
		return base.Activate(plan);
	}

	public override void Reset()
	{
		LastWaypoint = null;
		base.Reset();
	}

	private WayPoint GetAnyTeleportPositionAgainstEnemy(AgentHuman enemy, float minDistance, float maxDistance)
	{
		return null;
	}

	private WayPoint GetTeleportPositionBehindEnemy(AgentHuman enemy, float minDistance, float maxDistance)
	{
		return null;
	}
}
