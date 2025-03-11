using UnityEngine;

internal class GOAPActionAttackVomit : GOAPAction
{
	private AgentActionAttackVomit Action;

	private static float TimeAllowed;

	public GOAPActionAttackVomit(AgentHuman owner)
		: base(E_GOAPAction.AttackVomit, owner)
	{
	}

	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.InVomitRange, true);
		WorldPreconditions.SetWSProperty(E_PropKey.Contest, false);
		WorldEffects.SetWSProperty(E_PropKey.KillTarget, true);
		Cost = 1f;
		Precedence = 20;
		Interruptible = false;
	}

	public override void Activate()
	{
		base.Activate();
		TimeAllowed = Time.timeSinceLevelLoad + 5f;
		Owner.BlackBoard.NextVomitTime = Time.timeSinceLevelLoad + Random.Range(5f, 10f);
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.AttackVomit) as AgentActionAttackVomit;
		Action.AttackDir = Owner.BlackBoard.FireDir;
		Action.WeaponAction = E_WeaponAction.Vomit;
		Owner.BlackBoard.ActionAdd(Action);
	}

	public override void Deactivate()
	{
		AgentActionIdle action = AgentActionFactory.Create(AgentActionFactory.E_Type.Idle) as AgentActionIdle;
		Owner.BlackBoard.ActionAdd(action);
		base.Deactivate();
	}

	public override bool IsActionComplete()
	{
		if (!Action.IsActive())
		{
			return true;
		}
		return false;
	}

	public override bool ValidateAction()
	{
		AnimState currentAnimState = Owner.AnimComponent.CurrentAnimState;
		if (currentAnimState != null && currentAnimState.PlayingInjury())
		{
			return false;
		}
		return base.ValidateAction();
	}

	public override bool ValidateContextPreconditions(WorldState current, bool planning)
	{
		if ((bool)Owner.BlackBoard.VisibleTarget && Owner.BlackBoard.VisibleTarget.IsInContest())
		{
			return false;
		}
		if (Time.timeSinceLevelLoad < TimeAllowed || Owner.WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() != 0)
		{
			return false;
		}
		AnimState currentAnimState = Owner.AnimComponent.CurrentAnimState;
		if (currentAnimState != null && currentAnimState.PlayingInjury())
		{
			return false;
		}
		return base.ValidateContextPreconditions(current, planning);
	}
}
