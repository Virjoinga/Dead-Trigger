using UnityEngine;

public class AnimStateUseItem : AnimState
{
	private enum E_State
	{
		Disarm = 0,
		Use = 1,
		Arm = 2
	}

	private AgentActionUseItem Action;

	private float EndOfStateTime;

	private float ThrowTime;

	private string AnimName;

	private Transform Hand;

	private E_State State;

	public AnimStateUseItem(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
		Owner.BlackBoard.Desires.WeaponTriggerOn = false;
		Owner.BlackBoard.ReactOnHits = false;
		Owner.BlackBoard.BusyAction = true;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
	}

	public override void OnDeactivate()
	{
		ThrowTime = 0f;
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = false;
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		ThrowTime = 0f;
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = false;
		Animation.Stop(AnimName);
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
		switch (State)
		{
		case E_State.Disarm:
			if (EndOfStateTime <= Time.timeSinceLevelLoad)
			{
				InitUse();
			}
			break;
		case E_State.Use:
			if (!Action.Throw && ThrowTime <= Time.timeSinceLevelLoad)
			{
				Action.Throw = true;
			}
			if (EndOfStateTime <= Time.timeSinceLevelLoad)
			{
				InitArm();
			}
			break;
		case E_State.Arm:
			if (EndOfStateTime <= Time.timeSinceLevelLoad)
			{
				Release();
			}
			break;
		}
		float to = Mathf.Max(Owner.MaxWalkSpeed, Owner.MaxRunSpeed * Owner.BlackBoard.Desires.MoveSpeedModifier);
		float t = Owner.BlackBoard.BaseSetup.SpeedSmooth * (1f / Time.timeScale) * TimeManager.Instance.GetRealDeltaTime();
		Owner.BlackBoard.Speed = Mathf.Lerp(Owner.BlackBoard.Speed, to, t);
		Owner.BlackBoard.MoveDir = Owner.BlackBoard.Desires.MoveDirection;
		Move(Owner.BlackBoard.Desires.MoveDirection * Owner.BlackBoard.Speed * TimeManager.Instance.GetRealDeltaTime());
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionPlayAnim && Action != null)
		{
			action.SetFailed();
		}
		return false;
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionUseItem;
		InitDisarm();
	}

	private void InitDisarm()
	{
		State = E_State.Disarm;
		AnimName = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Disarm);
		if (AnimName == null)
		{
			Action.SetFailed();
			Release();
			return;
		}
		Owner.WeaponComponent.GetCurrentWeapon().WeaponDisArm();
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		EndOfStateTime = Animation[AnimName].length * 0.9f / num + Time.timeSinceLevelLoad;
		CrossFade(AnimName, 0.1f, PlayMode.StopSameLayer);
	}

	private void InitUse()
	{
		State = E_State.Use;
		AnimName = Owner.AnimSet.GetGadgetAnim(Owner.BlackBoard.Desires.Gadget);
		if (AnimName == null)
		{
			Action.SetFailed();
			Release();
			return;
		}
		Owner.WeaponComponent.GetCurrentWeapon().WeaponHide(false);
		Item gadget = Owner.GadgetsComponent.GetGadget(Owner.BlackBoard.Desires.Gadget);
		gadget.AddToHand(Owner.WeaponComponent.Hand);
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		EndOfStateTime = Animation[AnimName].length / num + Time.timeSinceLevelLoad;
		ThrowTime = Animation[AnimName].length * 0.8f / num + Time.timeSinceLevelLoad;
		CrossFade(AnimName, 0.1f / num, PlayMode.StopSameLayer);
		Owner.SoundPlay(Owner.ThrowSound);
	}

	private void InitArm()
	{
		State = E_State.Arm;
		AnimName = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Arm);
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		EndOfStateTime = Animation[AnimName].length * 0.9f / num + Time.timeSinceLevelLoad;
		CrossFade(AnimName, 0f, PlayMode.StopSameLayer);
		Owner.WeaponComponent.GetCurrentWeapon().WeaponArm();
		Owner.WeaponComponent.GetCurrentWeapon().WeaponShow(null, false);
	}
}
