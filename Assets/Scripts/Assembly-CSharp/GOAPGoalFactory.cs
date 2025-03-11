using UnityEngine;

internal class GOAPGoalFactory
{
	public static GOAPGoal Create(E_GOAPGoals type, AgentHuman owner)
	{
		GOAPGoal gOAPGoal;
		switch (type)
		{
		case E_GOAPGoals.SpawnAction:
			gOAPGoal = new GOAPGoalSpawnAction(owner);
			break;
		case E_GOAPGoals.Advance:
			gOAPGoal = new GOAPGoalAdvance(owner);
			break;
		case E_GOAPGoals.FallBack:
			gOAPGoal = new GOAPGoalFallBack(owner);
			break;
		case E_GOAPGoals.RunAway:
			gOAPGoal = new GOAPGoalRunAway(owner);
			break;
		case E_GOAPGoals.KeepCombatRange:
			gOAPGoal = new GOAPGoalKeepCombatRange(owner);
			break;
		case E_GOAPGoals.Move:
			gOAPGoal = new GOAPGoalMove(owner);
			break;
		case E_GOAPGoals.LookAtTarget:
			gOAPGoal = new GOAPGoalLookAtTarget(owner);
			break;
		case E_GOAPGoals.CheckEvent:
			gOAPGoal = new GOAPGoalCheckEvent(owner);
			break;
		case E_GOAPGoals.KillTarget:
			gOAPGoal = new GOAPGoalKillTarget(owner);
			break;
		case E_GOAPGoals.WeaponReload:
			gOAPGoal = new GOAPGoalWeaponReload(owner);
			break;
		case E_GOAPGoals.WeaponChange:
			gOAPGoal = new GOAPGoalWeaponChange(owner);
			break;
		case E_GOAPGoals.Dodge:
			gOAPGoal = new GOAPGoalDodge(owner);
			break;
		case E_GOAPGoals.UseWorldObject:
			gOAPGoal = new GOAPGoalUseWorldObject(owner);
			break;
		case E_GOAPGoals.PlayAnim:
			gOAPGoal = new GOAPGoalPlayAnim(owner);
			break;
		case E_GOAPGoals.CriticalInjury:
			gOAPGoal = new GOAPGoalCriticalInjury(owner);
			break;
		case E_GOAPGoals.Suppressed:
			gOAPGoal = new GOAPGoalSuppressed(owner);
			break;
		case E_GOAPGoals.Teleport:
			gOAPGoal = new GOAPGoalTeleport(owner);
			break;
		case E_GOAPGoals.CheckBait:
			gOAPGoal = new GOAPGoalCheckBait(owner);
			break;
		case E_GOAPGoals.DestroyObject:
			gOAPGoal = new GOAPGoalDestroyObject(owner);
			break;
		case E_GOAPGoals.Contest:
			gOAPGoal = new GOAPGoalContest(owner);
			break;
		case E_GOAPGoals.UseGadget:
			gOAPGoal = new GOAPGoalUseGadget(owner);
			break;
		default:
			Debug.LogError(string.Concat("GOAPGoalFactory Unknow goal ", type, " for ", owner.name));
			return null;
		}
		gOAPGoal.InitGoal();
		return gOAPGoal;
	}
}
