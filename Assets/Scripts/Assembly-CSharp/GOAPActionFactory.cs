using UnityEngine;

internal class GOAPActionFactory
{
	public static GOAPAction Create(E_GOAPAction type, AgentHuman owner)
	{
		GOAPAction gOAPAction;
		switch (type)
		{
		case E_GOAPAction.Move:
			gOAPAction = new GOAPActionMove(owner);
			break;
		case E_GOAPAction.Goto:
			gOAPAction = new GOAPActionGoTo(owner);
			break;
		case E_GOAPAction.GotoWeaponRange:
			gOAPAction = new GOAPActionGoToWeaponRange(owner);
			break;
		case E_GOAPAction.Boss1Goto:
			gOAPAction = new GOAPActionBoss1GoTo(owner);
			break;
		case E_GOAPAction.BossSantaGoto:
			gOAPAction = new GOAPActionBossSantaGoTo(owner);
			break;
		case E_GOAPAction.CrawlToWeaponRange:
			gOAPAction = new GOAPActionCrawlToWeaponRange(owner);
			break;
		case E_GOAPAction.CheckBait:
			gOAPAction = new GOAPActionCheckBait(owner);
			break;
		case E_GOAPAction.DestroyObject:
			gOAPAction = new GOAPActionDestroyObject(owner);
			break;
		case E_GOAPAction.Contest:
			gOAPAction = new GOAPActionContest(owner);
			break;
		case E_GOAPAction.Fallback:
			gOAPAction = new GOAPActionFallback(owner);
			break;
		case E_GOAPAction.LookAtTarget:
			gOAPAction = new GOAPActionLookAtTarget(owner);
			break;
		case E_GOAPAction.CheckEvent:
			gOAPAction = new GOAPActionCheckEvent(owner);
			break;
		case E_GOAPAction.CheckLostEnemy:
			gOAPAction = new GOAPActionCheckLostEnemy(owner);
			break;
		case E_GOAPAction.WeaponChange:
			gOAPAction = new GOAPActionWeaponChange(owner);
			break;
		case E_GOAPAction.WeaponReload:
			gOAPAction = new GOAPActionWeaponReload(owner);
			break;
		case E_GOAPAction.Use:
			gOAPAction = new GOAPActionUse(owner);
			break;
		case E_GOAPAction.PlayAnim:
			gOAPAction = new GOAPActionPlayAnim(owner);
			break;
		case E_GOAPAction.AttackMelee:
			gOAPAction = new GOAPActionAttackMelee(owner);
			break;
		case E_GOAPAction.AttackVomit:
			gOAPAction = new GOAPActionAttackVomit(owner);
			break;
		case E_GOAPAction.Knockdown:
			gOAPAction = new GOAPActionKnockdown(owner);
			break;
		case E_GOAPAction.Teleport:
			gOAPAction = new GOAPActionTeleport(owner);
			break;
		case E_GOAPAction.DodgeStrafe:
			gOAPAction = new GOAPActionDodgeStrafe(owner);
			break;
		case E_GOAPAction.DodgeStrafeWalk:
			gOAPAction = new GOAPActionDodgeStrafeWalk(owner);
			break;
		case E_GOAPAction.Patrol:
			gOAPAction = new GOAPActionPatrol(owner);
			break;
		case E_GOAPAction.Suppressed:
			gOAPAction = new GOAPActionSuppressed(owner);
			break;
		case E_GOAPAction.UseGadget:
			gOAPAction = new GOAPActionUseGadget(owner);
			break;
		default:
			Debug.LogError("GOAPActionFactory -  unknow state " + type);
			return null;
		}
		gOAPAction.InitAction();
		return gOAPAction;
	}
}
