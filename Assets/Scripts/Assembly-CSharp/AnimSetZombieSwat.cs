using System;
using UnityEngine;

[Serializable]
public class AnimSetZombieSwat : AnimSetZombie
{
	private static Randomizer InjuryBody;

	private static Randomizer InjuryLArm;

	private static Randomizer InjuryRArm;

	private static Randomizer InjuryLegs;

	static AnimSetZombieSwat()
	{
		InjuryBody = new Randomizer();
		InjuryLArm = new Randomizer();
		InjuryRArm = new Randomizer();
		InjuryLegs = new Randomizer();
		InjuryBody.Add("InjuryBodySmall");
		InjuryBody.Add("InjuryLArmSmall");
		InjuryBody.Add("InjuryRArmSmall");
		InjuryLArm.Add("InjuryLArmSmall");
		InjuryLArm.Add("InjuryBodySmall");
		InjuryRArm.Add("InjuryRArmSmall");
		InjuryRArm.Add("InjuryBodySmall");
		InjuryLegs.Add("InjuryLLegSmall");
		InjuryLegs.Add("InjuryRLegSmall");
	}

	public override string GetIdleAnim()
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return "CrawlIdle";
		}
		if (WorldState.GetWSProperty(E_PropKey.CheckBait).GetBool() && WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Stand && BlackBoard.BaitPhase == BlackBoard.E_BaitPhase.Stare)
		{
			return "LookIdle3";
		}
		return "Idle1";
	}

	public override string GetInjuryAnim(E_BodyPart bodyPart, bool bDestroy, E_Direction direction)
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return AnimSetZombie.InjuryCrawl.Get();
		}
		switch (bodyPart)
		{
		case E_BodyPart.Body:
			return (!bDestroy) ? InjuryBody.Get() : "InjuryBodyDestroy";
		case E_BodyPart.Head:
			return "InjuryHeadHit" + UnityEngine.Random.Range(1, 3);
		case E_BodyPart.LeftArm:
			return (!bDestroy) ? InjuryLArm.Get() : "InjuryLArmDestroy";
		case E_BodyPart.RightArm:
			return (!bDestroy) ? InjuryRArm.Get() : "InjuryRArmDestroy";
		case E_BodyPart.LeftLeg:
			return (!bDestroy) ? InjuryLegs.Get() : "InjuryLLegDestroy";
		case E_BodyPart.RightLeg:
			return (!bDestroy) ? InjuryLegs.Get() : "InjuryRLegDestroy";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}
}
