using UnityEngine;

public static class AgentActionFactory
{
	public enum E_Type
	{
		Idle = 0,
		Move = 1,
		Goto = 2,
		GotoByAnim = 3,
		CrawlTo = 4,
		CombatMove = 5,
		Attack = 6,
		AttackMelee = 7,
		AttackVomit = 8,
		Contest = 9,
		Injury = 10,
		InjuryCrit = 11,
		DamageBlocked = 12,
		Block = 13,
		Roll = 14,
		IncommingAttack = 15,
		WeaponShow = 16,
		Rotate = 17,
		Use = 18,
		PlayAnim = 19,
		PlayIdleAnim = 20,
		Death = 21,
		Knockdown = 22,
		Teleport = 23,
		Reload = 24,
		DodgeStrafe = 25,
		WeaponChange = 26,
		UseItem = 27,
		Count = 28
	}

	private static Queue<AgentAction>[] m_UnusedActions;

	static AgentActionFactory()
	{
		m_UnusedActions = new Queue<AgentAction>[28];
		for (E_Type e_Type = E_Type.Idle; e_Type < E_Type.Count; e_Type++)
		{
			m_UnusedActions[(int)e_Type] = new Queue<AgentAction>();
		}
	}

	public static AgentAction Create(E_Type type)
	{
		AgentAction agentAction;
		if (m_UnusedActions[(int)type].Count != 0)
		{
			agentAction = m_UnusedActions[(int)type].Dequeue();
		}
		else
		{
			switch (type)
			{
			case E_Type.Idle:
				agentAction = new AgentActionIdle();
				break;
			case E_Type.Move:
				agentAction = new AgentActionMove();
				break;
			case E_Type.Goto:
				agentAction = new AgentActionGoTo();
				break;
			case E_Type.CrawlTo:
				agentAction = new AgentActionCrawlTo();
				break;
			case E_Type.GotoByAnim:
				agentAction = new AgentActionGoToByAnim();
				break;
			case E_Type.CombatMove:
				agentAction = new AgentActioCombatMove();
				break;
			case E_Type.Attack:
				agentAction = new AgentActionAttack();
				break;
			case E_Type.AttackMelee:
				agentAction = new AgentActionAttackMelee();
				break;
			case E_Type.AttackVomit:
				agentAction = new AgentActionAttackVomit();
				break;
			case E_Type.Contest:
				agentAction = new AgentActionContest();
				break;
			case E_Type.Injury:
				agentAction = new AgentActionInjury();
				break;
			case E_Type.InjuryCrit:
				agentAction = new AgentActionInjuryCrit();
				break;
			case E_Type.DamageBlocked:
				agentAction = new AgentActionDamageBlocked();
				break;
			case E_Type.Roll:
				agentAction = new AgentActionRoll();
				break;
			case E_Type.IncommingAttack:
				agentAction = new AgentActionIncommingAttack();
				break;
			case E_Type.WeaponChange:
				agentAction = new AgentActionWeaponChange();
				break;
			case E_Type.Rotate:
				agentAction = new AgentActionRotate();
				break;
			case E_Type.Use:
				agentAction = new AgentActionUse();
				break;
			case E_Type.PlayAnim:
				agentAction = new AgentActionPlayAnim();
				break;
			case E_Type.PlayIdleAnim:
				agentAction = new AgentActionPlayIdleAnim();
				break;
			case E_Type.Death:
				agentAction = new AgentActionDeath();
				break;
			case E_Type.Knockdown:
				agentAction = new AgentActionKnockdown();
				break;
			case E_Type.Teleport:
				agentAction = new AgentActionTeleport();
				break;
			case E_Type.Reload:
				agentAction = new AgentActionReload();
				break;
			case E_Type.DodgeStrafe:
				agentAction = new AgentActionDodgeStrafe();
				break;
			case E_Type.UseItem:
				agentAction = new AgentActionUseItem();
				break;
			default:
				Debug.LogError("no AgentAction to create");
				return null;
			}
		}
		agentAction.Reset();
		agentAction.SetActive();
		return agentAction;
	}

	public static void Return(AgentAction action)
	{
		action.SetUnused();
		m_UnusedActions[(int)action.Type].Enqueue(action);
	}

	public static void Clear()
	{
		for (E_Type e_Type = E_Type.Idle; e_Type < E_Type.Count; e_Type++)
		{
			m_UnusedActions[(int)e_Type].Clear();
		}
	}
}
