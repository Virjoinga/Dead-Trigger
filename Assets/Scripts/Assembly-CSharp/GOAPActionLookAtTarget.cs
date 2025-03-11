using UnityEngine;

internal class GOAPActionLookAtTarget : GOAPAction
{
	private Vector3 TargetDir;

	private Vector3 TargetPos;

	private float MaxActionTime;

	public GOAPActionLookAtTarget(AgentHuman owner)
		: base(E_GOAPAction.LookAtTarget, owner)
	{
	}

	public override void InitAction()
	{
		WorldEffects.SetWSProperty(E_PropKey.LookingAtTarget, true);
		Cost = 1f;
		Precedence = 95;
	}

	public override bool ValidateContextPreconditions(WorldState current, bool planning)
	{
		if (Owner.WorldState.GetWSProperty(E_PropKey.DestroyObject).GetBool())
		{
			DestructibleObject destructibleObject = Owner.BlackBoard.ImportantObject as DestructibleObject;
			if (destructibleObject == null)
			{
				if (Owner.debugGOAP)
				{
					Debug.Log("look at target object FAILED - no destroy object");
				}
				return false;
			}
			Transform attackPoint = destructibleObject.GetAttackPoint(Owner);
			if (attackPoint != null)
			{
				TargetPos = attackPoint.position - attackPoint.forward;
			}
			else
			{
				TargetPos = destructibleObject.GetGameObject().transform.position;
				TargetPos -= (TargetPos - Owner.Transform.position).normalized;
			}
			if (Owner.debugGOAP)
			{
				Debug.Log("look at target object");
			}
		}
		else
		{
			AgentHuman dangerousEnemy = Owner.BlackBoard.DangerousEnemy;
			if (dangerousEnemy == null)
			{
				Fact validFact = Owner.Memory.GetValidFact(E_EventTypes.EnemyLost);
				if (validFact == null)
				{
					validFact = Owner.Memory.GetValidFact(E_EventTypes.EnemyHideInCover);
					if (validFact == null)
					{
						if (Owner.debugGOAP)
						{
							Debug.Log("look at target object FAILED - no info about enemy");
						}
						return false;
					}
				}
				TargetPos = validFact.Agent.Position;
				if (Owner.debugGOAP)
				{
					Debug.Log("look at enemy");
				}
			}
			else
			{
				TargetPos = dangerousEnemy.Position;
			}
		}
		return true;
	}

	public override void Update()
	{
	}

	public override void Activate()
	{
		base.Activate();
		Vector3 position = Owner.Position;
		TargetPos.y = 0f;
		position.y = 0f;
		TargetDir = (TargetPos - position).normalized;
		Owner.BlackBoard.Desires.Rotation.SetLookRotation(TargetDir);
		MaxActionTime = Time.timeSinceLevelLoad + 1f;
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}

	public override bool IsActionComplete()
	{
		if ((double)Vector3.Dot(TargetDir, Owner.Forward) > 0.95)
		{
			return true;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.LookingAtTarget).GetBool())
		{
			return true;
		}
		if (Time.timeSinceLevelLoad > MaxActionTime)
		{
			return true;
		}
		return false;
	}

	public override bool ValidateAction()
	{
		if (!Owner.WorldState.GetWSProperty(E_PropKey.SeeEnemy).GetBool())
		{
			return false;
		}
		return true;
	}
}
