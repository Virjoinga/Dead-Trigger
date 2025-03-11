using UnityEngine;

public class AnimStateWeaponChange : AnimState
{
	private enum E_State
	{
		Prepare = 0,
		Hide = 1,
		Show = 2
	}

	private AgentActionWeaponChange Action;

	private E_State State = E_State.Show;

	private float TimeToFinishState;

	public AnimStateWeaponChange(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnDeactivate()
	{
		if (Action != null)
		{
			Action.SetSuccess();
			Action = null;
		}
	}

	public override void Reset()
	{
		Action.SetSuccess();
		Action = null;
		base.Reset();
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionInjury)
		{
			PlayInjuryAnimation(action as AgentActionInjury);
			return true;
		}
		return false;
	}

	private void DoMove()
	{
		if (!(Time.deltaTime < float.Epsilon) && !(Time.timeScale < float.Epsilon) && !(Owner.BlackBoard.Desires.MoveDirection == Vector3.zero))
		{
			float to = Mathf.Max(Owner.MaxWalkSpeed, Owner.MaxRunSpeed * Owner.BlackBoard.Desires.MoveSpeedModifier);
			float t = Owner.BlackBoard.BaseSetup.SpeedSmooth * (1f / Time.timeScale) * TimeManager.Instance.GetRealDeltaTime();
			Owner.BlackBoard.Speed = Mathf.Lerp(Owner.BlackBoard.Speed, to, t);
			Owner.BlackBoard.MoveDir = Owner.BlackBoard.Desires.MoveDirection;
			if (Move(Owner.BlackBoard.Desires.MoveDirection * Owner.BlackBoard.Speed * TimeManager.Instance.GetRealDeltaTime()))
			{
			}
		}
	}

	public override void Update()
	{
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		switch (State)
		{
		case E_State.Prepare:
			if (TimeToFinishState < Time.timeSinceLevelLoad)
			{
				string weaponAnim = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Disarm);
				Animation[weaponAnim].speed = num;
				CrossFade(weaponAnim, 0.2f / num, PlayMode.StopAll);
				TimeToFinishState = Animation[weaponAnim].length / num + Time.timeSinceLevelLoad;
				State = E_State.Hide;
				Owner.WeaponComponent.GetCurrentWeapon().SetBusy((Animation[weaponAnim].length + 0.1f) / num);
				Owner.WeaponComponent.GetCurrentWeapon().WeaponDisArm();
			}
			break;
		case E_State.Hide:
			if (TimeToFinishState < Time.timeSinceLevelLoad)
			{
				Owner.WeaponComponent.SwitchWeapons(Action.NewWeapon);
				string weaponAnim2 = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Arm);
				Animation[weaponAnim2].speed = num;
				CrossFade(weaponAnim2, 0.1f / num, PlayMode.StopAll);
				TimeToFinishState = Animation[weaponAnim2].length / num + Time.timeSinceLevelLoad - 0.1f / num;
				State = E_State.Show;
				Owner.WeaponComponent.GetCurrentWeapon().WeaponArm();
			}
			break;
		case E_State.Show:
			if (TimeToFinishState < Time.timeSinceLevelLoad)
			{
				string idleAnim = Owner.AnimSet.GetIdleAnim();
				Animation[idleAnim].speed = num;
				CrossFade(idleAnim, 0.1f / num, PlayMode.StopAll);
				Release();
			}
			break;
		}
		if (Owner.NavMeshAgent == null)
		{
			Move(Vector3.zero, false);
		}
		DoMove();
	}

	private void PlayIdleAnim()
	{
		string idleAnim = Owner.AnimSet.GetIdleAnim();
		if (!Animation.IsPlaying(idleAnim))
		{
			float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
			CrossFade(idleAnim, 0.2f / num, PlayMode.StopAll);
		}
	}

	protected override void Initialize(AgentAction action)
	{
		State = E_State.Prepare;
		base.Initialize(action);
		if (!Owner.BlackBoard.KeepMotion)
		{
			Owner.BlackBoard.MotionType = E_MotionType.None;
			Owner.BlackBoard.MoveDir = Vector3.zero;
			Owner.BlackBoard.Speed = 0f;
		}
		Action = action as AgentActionWeaponChange;
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		TimeToFinishState = 0.2f / num + Time.timeSinceLevelLoad;
		Owner.WeaponComponent.GetCurrentWeapon().SetBusy(0.2f / num);
		PlayIdleAnim();
	}
}
