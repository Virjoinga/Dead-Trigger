using System;
using UnityEngine;

[Serializable]
public class AnimSetZombieBoss1 : AnimSetZombie
{
	public override string GetWeaponAnim(E_WeaponAction action)
	{
		if (action == E_WeaponAction.Vomit)
		{
			return "Spit";
		}
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() != E_BodyPose.Crawl)
		{
			DestructibleObject destructibleObject = BlackBoard.ImportantObject as DestructibleObject;
			if ((bool)destructibleObject)
			{
				int num = UnityEngine.Random.Range(1, 5);
				return "AttackDestroy" + num;
			}
			if (!WorldState.GetWSProperty(E_PropKey.Berserk).GetBool() || BlackBoard.PrevMotionType != E_MotionType.Run)
			{
				if (BlackBoard.Owner.EnemyComponent.GetNumDecapitatedLimbs() == 2)
				{
					return "AttackBite";
				}
				int num2 = UnityEngine.Random.Range(1, 3);
				return "BossAttack" + num2;
			}
			return "BerserkAttack";
		}
		switch (action)
		{
		case E_WeaponAction.MeleeLeft:
			return "CrawlAttackL";
		case E_WeaponAction.MeleeRight:
			return "CrawlAttackR";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetIdleAnim()
	{
		return "BossIdleLoop";
	}

	public override string GetMoveAnim(E_MotionSide motionSide)
	{
		switch (BlackBoard.MotionType)
		{
		case E_MotionType.Run:
			return "BossRun";
		case E_MotionType.Walk:
			return "BossWalk";
		default:
			return base.GetMoveAnim(motionSide);
		}
	}
}
