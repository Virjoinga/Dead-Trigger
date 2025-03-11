using System;
using UnityEngine;

[Serializable]
public class AnimSetPlayer : AnimSet
{
	private BlackBoard BlackBoard;

	private ComponentWeapons Weapons;

	private void Awake()
	{
		BlackBoard = GetComponent<AgentHuman>().BlackBoard;
		Weapons = GetComponent<ComponentWeapons>();
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
		if (BlackBoard.Desires.WeaponIronSight)
		{
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.AimIdle.name;
		}
		return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Idle.name;
	}

	public override string GetIdleActionAnim()
	{
		return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Idle.name;
	}

	public override string GetMoveAnim(E_MotionSide motionSide = E_MotionSide.Center)
	{
		if (BlackBoard.Desires.WeaponIronSight)
		{
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.AimWalk.name;
		}
		switch (BlackBoard.MotionType)
		{
		case E_MotionType.Run:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Run.name;
		case E_MotionType.Walk:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Walk.name;
		default:
			return GetIdleAnim();
		}
	}

	public override string GetStrafeAnim(E_StrafeDirection dir)
	{
		switch (dir)
		{
		case E_StrafeDirection.Left:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.StrafeL.name;
		case E_StrafeDirection.Right:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.StrafeR.name;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public override string GetRotateAnim(E_RotationType rotationType, float angle)
	{
		return null;
	}

	public override string GetDodgeAnim(E_StrafeDirection dir)
	{
		throw new NotImplementedException();
	}

	public override string GetWeaponAnim(E_WeaponAction action)
	{
		switch (action)
		{
		case E_WeaponAction.Fire:
			if (BlackBoard.Desires.WeaponIronSight)
			{
				return Weapons.GetCurrentWeapon().PlayerAnimsSetup.AimFire.name;
			}
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Fire.name;
		case E_WeaponAction.Reload:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Reload.name;
		case E_WeaponAction.Arm:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Arm.name;
		case E_WeaponAction.Disarm:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Disarm.name;
		default:
			return GetIdleAnim();
		}
	}

	public override string GetInjuryAnim(E_BodyPart bodyPart, bool bDestroy, E_Direction direction)
	{
		switch (direction)
		{
		case E_Direction.Left:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryL.name;
		case E_Direction.Right:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryR.name;
		case E_Direction.ForwardLeft:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryFrontL.name;
		case E_Direction.ForwardRight:
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryFrontR.name;
		default:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryL.name;
			}
			return Weapons.GetCurrentWeapon().PlayerAnimsSetup.InjuryR.name;
		}
	}

	public override string GetGadgetAnim(E_ItemID item)
	{
		return "throwGrenade";
	}

	public override string GetStandToCrawlAnim(E_MotionSide side)
	{
		throw new NotImplementedException();
	}

	public override string GetDeathAnim(E_BodyPart bodyPart)
	{
		return Weapons.GetCurrentWeapon().PlayerAnimsSetup.Death.name;
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
}
