internal class GOAPActionUseGadget : GOAPAction
{
	private AgentAction Action;

	public GOAPActionUseGadget(AgentHuman owner)
		: base(E_GOAPAction.UseGadget, owner)
	{
	}

	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.UseGadget, false);
		Interruptible = false;
		Cost = 2f;
	}

	public override void Activate()
	{
		base.Activate();
		Action = AgentActionFactory.Create(AgentActionFactory.E_Type.UseItem);
		Owner.BlackBoard.ActionAdd(Action);
		Owner.BlackBoard.BusyAction = true;
	}

	public override void Deactivate()
	{
		Owner.BlackBoard.BusyAction = false;
		Owner.WorldState.SetWSProperty(E_PropKey.UseGadget, false);
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
		if (Action != null && Action.IsFailed())
		{
			return false;
		}
		return Owner.IsAlive;
	}
}
