using UnityEngine;

public class AnimStateIdle : AnimState
{
	private string AnimNameBase;

	private float TimeToFinishWeaponAction;

	private AgentAction WeaponAction;

	private AgentActionRotate RotateAction;

	private float TimeToFinishRotateAction;

	private AgentActionReload ReloadAction;

	private float TimeToFinishReloadAction;

	public AnimStateIdle(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnDeactivate()
	{
		if (WeaponAction != null)
		{
			WeaponAction.SetSuccess();
			WeaponAction = null;
		}
		if (RotateAction != null)
		{
			RotateAction.SetSuccess();
			RotateAction = null;
		}
		if (ReloadAction != null)
		{
			ReloadAction.SetSuccess();
			ReloadAction = null;
		}
		base.OnDeactivate();
	}

	public override void Reset()
	{
		if (WeaponAction != null)
		{
			WeaponAction.SetSuccess();
			WeaponAction = null;
		}
		if (RotateAction != null)
		{
			RotateAction.SetSuccess();
			RotateAction = null;
		}
		if (ReloadAction != null)
		{
			ReloadAction.SetSuccess();
			ReloadAction = null;
		}
		base.Reset();
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionAttack)
		{
			string weaponAnim = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Fire);
			TimeToFinishWeaponAction = Time.timeSinceLevelLoad + Animation[weaponAnim].length * 0.5f;
			Animation[weaponAnim].layer = 2;
			Animation.Blend(weaponAnim, 1f, 0.2f);
			if (Animation.IsPlaying(weaponAnim))
			{
				Animation.Stop(weaponAnim);
				Animation.Play(weaponAnim);
				Animation.Blend(weaponAnim, 1f, 0.15f);
			}
			else
			{
				Animation.Blend(weaponAnim, 1f, 0f);
			}
			if (WeaponAction != null)
			{
				WeaponAction.SetSuccess();
			}
			WeaponAction = action;
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
			Blend(weaponAnim2, 0.1f);
			if (Owner.IsPlayer)
			{
				TimeToFinishReloadAction = Time.timeSinceLevelLoad + 0.1f;
			}
			else
			{
				TimeToFinishReloadAction = Time.timeSinceLevelLoad + Animation[weaponAnim2].length * 0.9f;
			}
			ReloadAction = action as AgentActionReload;
			float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			Owner.WeaponComponent.GetCurrentWeapon().SetBusy(Animation[weaponAnim2].length / num);
			return true;
		}
		if (action is AgentActionRotate)
		{
			RotateAction = action as AgentActionRotate;
			string rotateAnim = Owner.AnimSet.GetRotateAnim(RotateAction.Rotation, RotateAction.Angle);
			if (rotateAnim != null && !Animation.IsPlaying(rotateAnim))
			{
				Animation[rotateAnim].blendMode = AnimationBlendMode.Additive;
				Animation[rotateAnim].layer = 1;
				TimeToFinishRotateAction = Time.timeSinceLevelLoad + Animation[rotateAnim].length;
				Blend(rotateAnim, 0.1f);
			}
			return true;
		}
		return false;
	}

	public override void Update()
	{
		if (WeaponAction != null && TimeToFinishWeaponAction < Time.timeSinceLevelLoad)
		{
			WeaponAction.SetSuccess();
			WeaponAction = null;
			PlayIdleAnim();
		}
		if (RotateAction != null && TimeToFinishRotateAction < Time.timeSinceLevelLoad)
		{
			RotateAction.SetSuccess();
			RotateAction = null;
		}
		if (ReloadAction != null && TimeToFinishReloadAction < Time.timeSinceLevelLoad)
		{
			ReloadAction.SetSuccess();
			ReloadAction = null;
		}
		if (Owner.NavMeshAgent == null)
		{
			Move(Vector3.zero, false);
		}
		PlayIdleAnim();
		if (Owner.IsPlayer && !Owner.BlackBoard.Desires.WeaponIronSight)
		{
			PlayStrafeAnim();
		}
	}

	private void PlayStrafeAnim()
	{
		float num = Owner.Transform.rotation.eulerAngles.y - Owner.BlackBoard.Desires.Rotation.eulerAngles.y;
		if (num < 0.001f && num > -0.001f)
		{
			num = 0f;
		}
		else if (num > 180f)
		{
			num -= 360f;
		}
		else if (num < -180f)
		{
			num += 360f;
		}
		E_StrafeDirection dir = ((!(num > 0f)) ? E_StrafeDirection.Right : E_StrafeDirection.Left);
		num = Mathf.Abs(num);
		float num2 = Mathf.Min(1f, num / 10f);
		if (num2 > 0.1f)
		{
			string strafeAnim = Owner.AnimSet.GetStrafeAnim(dir);
			float num3 = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			Animation[strafeAnim].blendMode = AnimationBlendMode.Blend;
			Animation[strafeAnim].layer = 0;
			Animation.Blend(strafeAnim, num2, 0.15f / num3);
		}
		else
		{
			StopStrafeAnim();
		}
	}

	private void StopStrafeAnim()
	{
		string strafeAnim = Owner.AnimSet.GetStrafeAnim(E_StrafeDirection.Left);
		if (Animation.IsPlaying(strafeAnim))
		{
			if (Animation[strafeAnim].weight > 0.05f)
			{
				float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
				Animation.Blend(strafeAnim, 0f, 0.2f / num);
			}
			else
			{
				Animation.Stop(strafeAnim);
			}
		}
		strafeAnim = Owner.AnimSet.GetStrafeAnim(E_StrafeDirection.Right);
		if (Animation.IsPlaying(strafeAnim))
		{
			if (Animation[strafeAnim].weight > 0.05f)
			{
				float num2 = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
				Animation.Blend(strafeAnim, 0f, 0.2f / num2);
			}
			else
			{
				Animation.Stop(strafeAnim);
			}
		}
	}

	private void PlayIdleAnim()
	{
		AnimNameBase = Owner.AnimSet.GetIdleAnim();
		if (!Animation.IsPlaying(AnimNameBase))
		{
			float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			if (Owner.IsActionPointOn)
			{
				CrossFade(AnimNameBase, 0.5f / num, PlayMode.StopSameLayer);
			}
			else
			{
				CrossFade(AnimNameBase, 0.2f / num, PlayMode.StopSameLayer);
			}
		}
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Owner.BlackBoard.MotionType = E_MotionType.None;
		if (WeaponAction == null)
		{
			PlayIdleAnim();
		}
		if (action != null)
		{
			action.SetSuccess();
		}
	}
}
