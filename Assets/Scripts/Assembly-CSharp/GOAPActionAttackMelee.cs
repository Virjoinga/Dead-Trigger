using System;
using UnityEngine;

internal class GOAPActionAttackMelee : GOAPAction
{
	private AgentActionAttackMelee Action;

	public GOAPActionAttackMelee(AgentHuman owner)
		: base(E_GOAPAction.AttackMelee, owner)
	{
	}

	public override void InitAction()
	{
		WorldPreconditions.SetWSProperty(E_PropKey.LookingAtTarget, true);
		WorldPreconditions.SetWSProperty(E_PropKey.InWeaponRange, true);
		WorldPreconditions.SetWSProperty(E_PropKey.Contest, false);
		WorldEffects.SetWSProperty(E_PropKey.KillTarget, true);
		Cost = 1f;
		Precedence = 10;
		Interruptible = false;
	}

	private E_WeaponAction ChooseWeaponAction()
	{
		ComponentEnemy component = Owner.GetComponent<ComponentEnemy>();
		if (component == null)
		{
			throw new MemberAccessException("ComponentEnemy not found!");
		}
		bool flag = !component.IsLimbDecapitated(E_BodyPart.LeftArm);
		bool flag2 = !component.IsLimbDecapitated(E_BodyPart.RightArm);
		if (flag && flag2)
		{
			return (UnityEngine.Random.Range(0, 2) != 0) ? E_WeaponAction.MeleeRight : E_WeaponAction.MeleeLeft;
		}
		if (flag2)
		{
			return E_WeaponAction.MeleeRight;
		}
		return E_WeaponAction.MeleeLeft;
	}

	public override void Activate()
	{
		base.Activate();
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.AttackMelee) as AgentActionAttackMelee;
		Action.AttackDir = Owner.BlackBoard.FireDir;
		Action.WeaponAction = ChooseWeaponAction();
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
		AnimState currentAnimState = Owner.AnimComponent.CurrentAnimState;
		if (currentAnimState != null && currentAnimState.PlayingInjury())
		{
			return false;
		}
		return base.ValidateContextPreconditions(current, planning);
	}
}
