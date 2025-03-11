using UnityEngine;

public class AnimStatePlayAnim : AnimState
{
	private AgentAction Action;

	private float EndOfStateTime;

	public bool LookAtTarget;

	private string AnimName;

	private bool PrevInvulnerable;

	public AnimStatePlayAnim(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
		Owner.BlackBoard.Desires.WeaponTriggerOn = false;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		if (Action is AgentActionPlayAnim)
		{
			E_TriState invulnerable = (Action as AgentActionPlayAnim).Invulnerable;
			PrevInvulnerable = Owner.IsInvulnerable;
			if (invulnerable != E_TriState.Default)
			{
				Owner.BlackBoard.Invulnerable = invulnerable == E_TriState.True;
			}
		}
	}

	public override void OnDeactivate()
	{
		if (Action is AgentActionPlayAnim)
		{
			Owner.BlackBoard.Invulnerable = PrevInvulnerable;
		}
		Animation[AnimName].layer = 0;
		LookAtTarget = false;
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		Animation.Stop(AnimName);
		LookAtTarget = false;
		Action.SetSuccess();
		Action = null;
		base.Reset();
	}

	public override void Update()
	{
		if (EndOfStateTime <= Time.timeSinceLevelLoad)
		{
			Release();
		}
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
		Action = action;
		if (Action is AgentActionPlayAnim)
		{
			AnimName = (Action as AgentActionPlayAnim).AnimName;
			LookAtTarget = false;
			Animation[AnimName].layer = 5;
		}
		else if (Action is AgentActionPlayIdleAnim)
		{
			AnimName = Owner.AnimSet.GetIdleActionAnim();
			LookAtTarget = true;
		}
		if (AnimName == null)
		{
			Action.SetFailed();
			Action = null;
			Release();
			return;
		}
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		float fadeInTime = Mathf.Min(Animation[AnimName].length * 0.25f, 0.6f) / num;
		CrossFade(AnimName, fadeInTime, PlayMode.StopAll);
		if (Animation[AnimName].wrapMode == WrapMode.Loop)
		{
			EndOfStateTime = 100000f + Time.timeSinceLevelLoad;
		}
		else
		{
			EndOfStateTime = Animation[AnimName].length + Time.timeSinceLevelLoad - 0.2f / num;
		}
	}

	public override void HandleAnimationEvent(E_AnimEvent animEvent)
	{
	}
}
