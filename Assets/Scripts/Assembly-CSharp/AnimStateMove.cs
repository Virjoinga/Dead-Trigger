using UnityEngine;

public class AnimStateMove : AnimState
{
	private AgentActionMove Action;

	private float MaxSpeed;

	public AnimStateMove(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
	}

	public override void OnDeactivate()
	{
		Owner.BlackBoard.AngleForward = 0f;
		Owner.BlackBoard.AngleRight = 0f;
		StopStrafeAnim(false, E_StrafeDirection.Left);
		if (!Owner.BlackBoard.KeepMotion)
		{
			Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
			Owner.BlackBoard.MotionType = E_MotionType.None;
			Owner.BlackBoard.MoveDir = Vector3.zero;
			Owner.BlackBoard.Speed = 0f;
		}
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		Action.SetSuccess();
		Action = null;
		base.Reset();
	}

	public override void Update()
	{
		if (Time.deltaTime < float.Epsilon || Time.timeScale < float.Epsilon)
		{
			return;
		}
		if (!Action.IsActive())
		{
			Release();
		}
		else if (!(Owner.BlackBoard.Desires.MoveDirection == Vector3.zero))
		{
			if (Owner.IsPlayer && Owner.BlackBoard.Desires.WeaponIronSight)
			{
				MaxSpeed = Mathf.Max(Owner.MaxWalkSpeed * 0.7f, Owner.MaxRunSpeed * Owner.BlackBoard.Desires.MoveSpeedModifier * 0.7f);
			}
			else
			{
				MaxSpeed = Mathf.Max(Owner.MaxWalkSpeed, Owner.MaxRunSpeed * Owner.BlackBoard.Desires.MoveSpeedModifier);
			}
			float t = Owner.BlackBoard.BaseSetup.SpeedSmooth * (1f / Time.timeScale) * TimeManager.Instance.GetRealDeltaTime();
			Owner.BlackBoard.Speed = Mathf.Lerp(Owner.BlackBoard.Speed, MaxSpeed, t);
			Owner.BlackBoard.MoveDir = Owner.BlackBoard.Desires.MoveDirection;
			if (!Move(Owner.BlackBoard.Desires.MoveDirection * Owner.BlackBoard.Speed * TimeManager.Instance.GetRealDeltaTime()))
			{
				Release();
			}
			PlayMoveAnim(false);
			if (Owner.IsPlayer && !Owner.BlackBoard.Desires.WeaponIronSight)
			{
				PlayStrafeAnim();
			}
		}
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionMove)
		{
			if (Action != null)
			{
				Action.SetSuccess();
			}
			SetFinished(false);
			Initialize(action);
			return true;
		}
		if (action is AgentActionIdle)
		{
			action.SetSuccess();
			SetFinished(true);
			return true;
		}
		if (action is AgentActionAttack)
		{
			string weaponAnim = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Fire);
			Animation[weaponAnim].layer = 2;
			Animation.Blend(weaponAnim, 1f, 0f);
			if (Animation.IsPlaying(weaponAnim))
			{
				Animation.Stop(weaponAnim);
				Animation.Play(weaponAnim);
				Animation.Blend(weaponAnim, 1f, 0.25f);
			}
			else
			{
				Animation.Blend(weaponAnim, 1f, 0f);
			}
			action.SetSuccess();
			return true;
		}
		if (action is AgentActionInjury)
		{
			PlayInjuryAnimation(action as AgentActionInjury);
			return true;
		}
		if (action is AgentActionReload)
		{
			string weaponAnim2 = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Reload);
			Animation[weaponAnim2].layer = 3;
			Blend(weaponAnim2, 0.2f);
			action.SetSuccess();
			return true;
		}
		return false;
	}

	private void PlayMoveAnim(bool force)
	{
		Owner.BlackBoard.MotionType = GetMotionType();
		Owner.BlackBoard.MoveType = GetMoveType();
		string moveAnim = Owner.AnimSet.GetMoveAnim();
		bool flag = Animation.IsPlaying(moveAnim);
		if (force || !flag)
		{
			if (flag)
			{
				Animation.Stop(moveAnim);
			}
			float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			if (Owner.BlackBoard.MotionType == E_MotionType.Walk)
			{
				CrossFade(moveAnim, 0.2f / num, PlayMode.StopSameLayer);
			}
			else
			{
				CrossFade(moveAnim, 0.2f / num, PlayMode.StopSameLayer);
			}
		}
	}

	private void PlayStrafeAnim()
	{
		E_StrafeDirection e_StrafeDirection;
		switch (Owner.BlackBoard.MoveType)
		{
		case E_MoveType.StrafeRight:
			e_StrafeDirection = E_StrafeDirection.Right;
			break;
		case E_MoveType.StrafeLeft:
			e_StrafeDirection = E_StrafeDirection.Left;
			break;
		default:
			e_StrafeDirection = ((!(Owner.BlackBoard.AngleRight > 90f)) ? E_StrafeDirection.Right : E_StrafeDirection.Left);
			break;
		}
		float num = ((e_StrafeDirection != E_StrafeDirection.Right) ? Mathf.Abs(Owner.BlackBoard.AngleRight - 180f) : Owner.BlackBoard.AngleRight);
		float num2 = Owner.BlackBoard.Speed / MaxSpeed * (1f - num / 90f);
		if (num2 > 0.1f)
		{
			string strafeAnim = Owner.AnimSet.GetStrafeAnim(e_StrafeDirection);
			float num3 = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			Animation[strafeAnim].blendMode = AnimationBlendMode.Blend;
			Animation[strafeAnim].layer = 0;
			Animation.Blend(strafeAnim, num2, 0.15f / num3);
			StopStrafeAnim(true, (e_StrafeDirection != E_StrafeDirection.Right) ? E_StrafeDirection.Right : E_StrafeDirection.Left);
		}
		else
		{
			StopStrafeAnim(false, E_StrafeDirection.Left);
		}
	}

	private void StopStrafeAnim(bool justOne, E_StrafeDirection whichOne)
	{
		if (!justOne || whichOne == E_StrafeDirection.Left)
		{
			string strafeAnim = Owner.AnimSet.GetStrafeAnim(E_StrafeDirection.Left);
			if (Animation.IsPlaying(strafeAnim))
			{
				float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
				if (Animation[strafeAnim].weight > 0.05f)
				{
					Animation.Blend(strafeAnim, 0f, 0.2f / num);
				}
				else
				{
					Animation.Stop(strafeAnim);
				}
			}
		}
		if (justOne && whichOne != E_StrafeDirection.Right)
		{
			return;
		}
		string strafeAnim2 = Owner.AnimSet.GetStrafeAnim(E_StrafeDirection.Right);
		if (Animation.IsPlaying(strafeAnim2))
		{
			if (Animation[strafeAnim2].weight > 0.05f)
			{
				Animation.Blend(strafeAnim2, 0f, 0.2f);
			}
			else
			{
				Animation.Stop(strafeAnim2);
			}
		}
	}

	private E_MoveType GetMoveType()
	{
		Vector2 from = new Vector2(Transform.forward.x, Transform.forward.z);
		Vector2 from2 = new Vector2(Transform.right.x, Transform.right.z);
		Vector2 to = new Vector2(Owner.BlackBoard.Desires.MoveDirection.x, Owner.BlackBoard.Desires.MoveDirection.z);
		Owner.BlackBoard.AngleForward = Vector2.Angle(from, to);
		Owner.BlackBoard.AngleRight = Vector2.Angle(from2, to);
		if (Owner.BlackBoard.AngleForward <= 45f)
		{
			return E_MoveType.Forward;
		}
		if (Owner.BlackBoard.AngleForward > 135f)
		{
			return E_MoveType.Backward;
		}
		if (Owner.BlackBoard.AngleRight < 90f)
		{
			return E_MoveType.StrafeRight;
		}
		return E_MoveType.StrafeLeft;
	}

	private E_MotionType GetMotionType()
	{
		if (Owner.IsActionPointOn)
		{
			return E_MotionType.ActionPoint;
		}
		if (Owner.BlackBoard.Speed > (Owner.MaxRunSpeed - Owner.MaxWalkSpeed) * 0.5f)
		{
			return E_MotionType.Run;
		}
		return E_MotionType.Walk;
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionMove;
		PlayMoveAnim(true);
	}
}
