using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimSetZombie : AnimSet
{
	public enum E_BaitAnim
	{
		LookForward = 0,
		LookLeft = 1,
		LookRight = 2
	}

	protected BlackBoard BlackBoard;

	protected WorldState WorldState;

	protected static Randomizer InjuryCrawl;

	private static Randomizer DeathBody;

	private static Randomizer DeathHead;

	private static Randomizer DeathLArm;

	private static Randomizer DeathRArm;

	private static Randomizer DeathLLeg;

	private static Randomizer DeathRLeg;

	private static Randomizer DeathCrawl;

	private static KeyedAnimList Walk;

	private static KeyedAnimList Run;

	static AnimSetZombie()
	{
		InjuryCrawl = new Randomizer();
		DeathBody = new Randomizer();
		DeathHead = new Randomizer();
		DeathLArm = new Randomizer();
		DeathRArm = new Randomizer();
		DeathLLeg = new Randomizer();
		DeathRLeg = new Randomizer();
		DeathCrawl = new Randomizer();
		Walk = new KeyedAnimList();
		Run = new KeyedAnimList();
		InjuryCrawl.Add("CrawlInjury1");
		InjuryCrawl.Add("CrawlInjury2");
		DeathBody.Add("DeathBody1");
		DeathBody.Add("DeathBody2");
		DeathBody.Add("DeathBody3");
		DeathHead.Add("DeathHead1");
		DeathHead.Add("DeathHead2");
		DeathHead.Add("DeathHead3");
		DeathHead.Add("DeathHead4");
		DeathLArm.Add("DeathLArm1");
		DeathLArm.Add("DeathLArm2");
		DeathRArm.Add("DeathRArm1");
		DeathRArm.Add("DeathRArm2");
		DeathLLeg.Add("DeathLLeg1");
		DeathRLeg.Add("DeathRLeg1");
		DeathRLeg.Add("DeathRLeg2");
		DeathCrawl.Add("CrawlDeath1");
		DeathCrawl.Add("CrawlDeath2");
		DeathCrawl.Add("CrawlDeath3");
		Walk.AddAnimation(new KeyValuePair<float, string>(0.7f, "WalkSlow1"));
		Walk.AddAnimation(new KeyValuePair<float, string>(0.7f, "WalkSlow2"));
		Walk.AddAnimation(new KeyValuePair<float, string>(0.7f, "WalkSlow3"));
		Walk.AddAnimation(new KeyValuePair<float, string>(0.7f, "WalkSlow4"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.1f, "Walk1"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.1f, "Walk2"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.1f, "Walk3"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.1f, "Walk4"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.6f, "WalkFast1"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.6f, "WalkFast2"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.6f, "WalkFast3"));
		Walk.AddAnimation(new KeyValuePair<float, string>(1.6f, "WalkFast4"));
		Run.AddAnimation(new KeyValuePair<float, string>(2.5f, "Run1"));
		Run.AddAnimation(new KeyValuePair<float, string>(2.5f, "Run2"));
		Run.AddAnimation(new KeyValuePair<float, string>(2.5f, "Run3"));
		Run.AddAnimation(new KeyValuePair<float, string>(2.5f, "Run4"));
		Run.AddAnimation(new KeyValuePair<float, string>(3.5f, "RunFast1"));
		Run.AddAnimation(new KeyValuePair<float, string>(3.5f, "RunFast2"));
		Run.AddAnimation(new KeyValuePair<float, string>(3.5f, "RunFast3"));
		Run.AddAnimation(new KeyValuePair<float, string>(3.5f, "RunFast4"));
		Run.AddAnimation(new KeyValuePair<float, string>(5f, "Sprint1"));
		Run.AddAnimation(new KeyValuePair<float, string>(5f, "Sprint2"));
	}

	private void Awake()
	{
		BlackBoard = GetComponent<AgentHuman>().BlackBoard;
		WorldState = BlackBoard.Owner.WorldState;
	}

	private void Start()
	{
	}

	public override string GetBlockAnim(E_BlockState state)
	{
		return null;
	}

	public override string GetIdleAnim()
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return "CrawlIdle";
		}
		if (WorldState.GetWSProperty(E_PropKey.CheckBait).GetBool() && WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Stand && BlackBoard.BaitPhase == BlackBoard.E_BaitPhase.Stare)
		{
			return "LookIdle2";
		}
		return "Idle1";
	}

	public override string GetIdleActionAnim()
	{
		throw new NotImplementedException();
	}

	protected virtual string GetWalkAnim()
	{
		List<string> animations = Walk.GetAnimations(BlackBoard.BaseSetup.MaxWalkSpeed, BlackBoard.BaseSetup.MaxWalkSpeed * 0.15f);
		if (animations.Count > 0)
		{
			return animations[UnityEngine.Random.Range(0, animations.Count)];
		}
		return "Walk1";
	}

	protected virtual string GetRunAnim()
	{
		List<string> animations = Run.GetAnimations(BlackBoard.BaseSetup.MaxRunSpeed, BlackBoard.BaseSetup.MaxRunSpeed * 0.15f);
		if (animations.Count > 0)
		{
			return animations[UnityEngine.Random.Range(0, animations.Count)];
		}
		return "Run1";
	}

	public override string GetMoveAnim(E_MotionSide motionSide)
	{
		switch (BlackBoard.MotionType)
		{
		case E_MotionType.Run:
			return GetRunAnim();
		case E_MotionType.Walk:
			return GetWalkAnim();
		case E_MotionType.Crawl:
			switch (motionSide)
			{
			case E_MotionSide.Left:
				return "CrawlLeftHand";
			case E_MotionSide.Right:
				return "CrawlRightHand";
			default:
				return "Crawl1";
			}
		default:
			return GetIdleAnim();
		}
	}

	public override string GetStrafeAnim(E_StrafeDirection dir)
	{
		throw new NotImplementedException();
	}

	public override string GetRotateAnim(E_RotationType rotationType, float angle)
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() != E_BodyPose.Crawl)
		{
			if (rotationType == E_RotationType.Left)
			{
				if (angle < 60f)
				{
					return "TurnL45";
				}
				if (angle < 120f)
				{
					return "TurnL90";
				}
				return "TurnL180";
			}
			if (angle < 60f)
			{
				return "TurnR45";
			}
			if (angle < 120f)
			{
				return "TurnR90";
			}
			return "TurnR180";
		}
		if (rotationType == E_RotationType.Left)
		{
			return "CrawlTurnL";
		}
		return "CrawlTurnR";
	}

	public virtual string GetBaitAnim(E_BaitAnim baitAnim)
	{
		switch (baitAnim)
		{
		case E_BaitAnim.LookForward:
			return "LookF";
		case E_BaitAnim.LookLeft:
			return "LookL";
		case E_BaitAnim.LookRight:
			return "LookR";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetDodgeAnim(E_StrafeDirection dir)
	{
		throw new NotImplementedException();
	}

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
				if (destructibleObject.m_AttackAnims.Length > 0)
				{
					int num = UnityEngine.Random.Range(0, destructibleObject.m_AttackAnims.Length);
					return destructibleObject.m_AttackAnims[num].name;
				}
				int num2 = UnityEngine.Random.Range(1, 5);
				return "AttackDestroy" + num2;
			}
			if (WorldState.GetWSProperty(E_PropKey.Berserk).GetBool() && BlackBoard.PrevMotionType == E_MotionType.Run)
			{
				return "BerserkAttack";
			}
			if (BlackBoard.Owner.EnemyComponent.GetNumDecapitatedLimbs() == 2)
			{
				return "AttackBite";
			}
			int num3 = UnityEngine.Random.Range(1, 4);
			switch (action)
			{
			case E_WeaponAction.MeleeLeft:
				return "AttackL" + num3;
			case E_WeaponAction.MeleeRight:
				return "AttackR" + num3;
			}
		}
		else
		{
			switch (action)
			{
			case E_WeaponAction.MeleeLeft:
				return "CrawlAttackL";
			case E_WeaponAction.MeleeRight:
				return "CrawlAttackR";
			}
		}
		throw new ArgumentOutOfRangeException();
	}

	public override string GetInjuryAnim(E_BodyPart bodyPart, bool bDestroy, E_Direction direction)
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return InjuryCrawl.Get();
		}
		switch (bodyPart)
		{
		case E_BodyPart.Body:
			return (!bDestroy) ? ("InjuryBodyHit" + UnityEngine.Random.Range(1, 5)) : "InjuryBodyDestroy";
		case E_BodyPart.Head:
			return "InjuryHeadHit" + UnityEngine.Random.Range(1, 3);
		case E_BodyPart.LeftArm:
			return (!bDestroy) ? ("InjuryLArmHit" + UnityEngine.Random.Range(1, 4)) : "InjuryLArmDestroy";
		case E_BodyPart.RightArm:
			return (!bDestroy) ? ("InjuryRArmHit" + UnityEngine.Random.Range(1, 4)) : "InjuryRArmDestroy";
		case E_BodyPart.LeftLeg:
			return (!bDestroy) ? ("InjuryLLegHit" + UnityEngine.Random.Range(1, 4)) : "InjuryLLegDestroy";
		case E_BodyPart.RightLeg:
			return (!bDestroy) ? ("InjuryRLegHit" + UnityEngine.Random.Range(1, 4)) : "InjuryRLegDestroy";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetStandToCrawlAnim(E_MotionSide side)
	{
		switch (side)
		{
		case E_MotionSide.Left:
			return "InjuryLLegDestroy";
		case E_MotionSide.Right:
			return "InjuryRLegDestroy";
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetDeathAnim(E_BodyPart bodyPart)
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() != 0)
		{
			return DeathCrawl.Get();
		}
		if (BlackBoard.Owner.IsInContest() && BlackBoard.Owner.ContestEnemy.IsInContest())
		{
			return GetContestAnim(E_ContestState.Lost);
		}
		switch (bodyPart)
		{
		case E_BodyPart.Body:
			return DeathBody.Get();
		case E_BodyPart.Head:
			return DeathHead.Get();
		case E_BodyPart.LeftArm:
			return DeathLArm.Get();
		case E_BodyPart.RightArm:
			return DeathRArm.Get();
		case E_BodyPart.LeftLeg:
			return DeathLLeg.Get();
		case E_BodyPart.RightLeg:
			return DeathRLeg.Get();
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetKnockdowAnim(E_KnockdownState knockdownState)
	{
		throw new NotImplementedException();
	}

	public override string GetContestAnim(E_ContestState state)
	{
		switch (state)
		{
		case E_ContestState.Start:
			return "ContestStart";
		case E_ContestState.LoopBase:
			return "ContestLoopNormal";
		case E_ContestState.LoopLoosing:
			return "ContestLoopBad";
		case E_ContestState.LoopWinning:
			return "ContestLoopGood";
		case E_ContestState.Won:
			return "ContestWin";
		case E_ContestState.Lost:
			return "ContestLoss";
		case E_ContestState.Injury:
			return (UnityEngine.Random.Range(0, 2) != 0) ? "ContestInjury2" : "ContestInjury1";
		default:
			throw new ArgumentOutOfRangeException("state", " = " + state);
		}
	}

	public override string GetTeleportAnim(E_TeleportAnim type)
	{
		throw new NotImplementedException();
	}

	public override string GetInjuryCritAnim()
	{
		throw new NotImplementedException();
	}

	public override string GetGadgetAnim(E_ItemID item)
	{
		throw new NotImplementedException();
	}
}
