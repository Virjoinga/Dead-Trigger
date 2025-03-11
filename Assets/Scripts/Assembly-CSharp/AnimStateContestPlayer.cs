using System.Collections;
using UnityEngine;

public class AnimStateContestPlayer : AnimState
{
	private enum E_State
	{
		Start = 0,
		Loop = 1,
		Injury = 2,
		Won = 3,
		Lost = 4,
		Death = 5,
		Finish = 6,
		End = 7
	}

	private AgentActionContest Action;

	private E_State State;

	private float EndOfStateTime;

	private float ContestBalance;

	private string animBase;

	private string animGood;

	private string animBad;

	public AnimStateContestPlayer(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		base.OnActivate(action);
		Owner.BlackBoard.Invulnerable = true;
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = true;
		Owner.BlackBoard.Desires.WeaponTriggerOn = false;
		Owner.BlackBoard.MotionType = E_MotionType.Contest;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		Player.Instance.StopMove(true);
		Player.Instance.StopView(true);
		EnableSmoothRotation(true);
		animBase = Owner.AnimSet.GetContestAnim(E_ContestState.LoopBase);
		animGood = Owner.AnimSet.GetContestAnim(E_ContestState.LoopWinning);
		animBad = Owner.AnimSet.GetContestAnim(E_ContestState.LoopLoosing);
		ContestStart();
		TimeManager.Instance.SetTimeScale(1f, 0f, 0f, 0f);
	}

	public override void OnDeactivate()
	{
		Owner.BlackBoard.Invulnerable = false;
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = false;
		Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Action.SetSuccess();
		Action = null;
		Player.Instance.StopMove(false);
		Player.Instance.StopView(false);
		Owner.WeaponComponent.GetCurrentWeapon().WeaponShow(null, false);
		EnableSmoothRotation(false);
		Owner.BlackBoard.ContestAllowNextTime = Time.timeSinceLevelLoad + Owner.BlackBoard.BaseSetup.ContestDelay;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		Owner.BlackBoard.Invulnerable = false;
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = false;
		Action.SetSuccess();
		Action = null;
		base.Reset();
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionContest)
		{
			Debug.LogError("Another AgentActionContest arrived");
			action.SetFailed();
			return true;
		}
		if (action is AgentActionInjury)
		{
			action.SetSuccess();
			return true;
		}
		return false;
	}

	public override void Release()
	{
		SetFinished(true);
	}

	public override void Update()
	{
		switch (State)
		{
		case E_State.Start:
			if (EndOfStateTime <= Time.timeSinceLevelLoad)
			{
				InitializeLoop();
			}
			break;
		case E_State.Loop:
			UpdateLoop();
			break;
		case E_State.Lost:
			ContestLost();
			break;
		case E_State.Won:
			ContestWon();
			break;
		case E_State.Finish:
			if (EndOfStateTime <= Time.timeSinceLevelLoad)
			{
				State = E_State.End;
				StopAnims();
				Owner.StopContest(Action.Enemy);
				Owner.ToggleCollisions(true, true);
				Release();
			}
			break;
		case E_State.End:
			break;
		case E_State.Injury:
		case E_State.Death:
			break;
		}
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionContest;
		State = E_State.Start;
		Owner.ToggleCollisions(false, false);
		Owner.BlackBoard.Desires.Rotation.SetLookRotation(Action.Enemy.Transform.position - Transform.position);
		TeleportEnemy(Action.Enemy);
		Owner.BlackBoard.ContestBalance = (ContestBalance = 0f - Action.Enemy.BlackBoard.ContestBalance);
	}

	private void TeleportEnemy(AgentHuman enemy)
	{
		Transform transform = Owner.Transform.FindChildByName("ContestRoot");
		enemy.Transform.position = transform.position;
		enemy.NavMeshAgent.transform.position = transform.position;
		enemy.Transform.rotation = transform.rotation;
		enemy.NavMeshAgent.transform.rotation = transform.rotation;
		enemy.BlackBoard.Desires.Rotation = transform.rotation;
	}

	private void ContestStart()
	{
		Animation[animBase].layer = 7;
		Animation[animGood].layer = 7;
		Animation[animBad].layer = 7;
		Animation.SyncLayer(7);
		string contestAnim = Owner.AnimSet.GetContestAnim(E_ContestState.Start);
		CrossFade(contestAnim, 0.1f, PlayMode.StopAll);
		Owner.StartCoroutine(_HideWeapon(0.1f));
		EndOfStateTime = Time.timeSinceLevelLoad + Animation[contestAnim].length * 0.9f;
	}

	private IEnumerator _HideWeapon(float delay)
	{
		yield return new WaitForSeconds(delay);
		Owner.WeaponComponent.GetCurrentWeapon().WeaponHide(false);
	}

	private void InitializeLoop()
	{
		State = E_State.Loop;
		CrossFade(animBase, 0.1f, PlayMode.StopSameLayer);
		EndOfStateTime = Time.timeSinceLevelLoad + Action.Time;
	}

	private void UpdateLoop()
	{
		string text = null;
		string text2 = null;
		float num = 0f;
		Owner.BlackBoard.Desires.Rotation.SetLookRotation(Action.Enemy.Transform.position - Transform.position);
		Owner.BlackBoard.ContestBalance = (ContestBalance = 0f - Action.Enemy.BlackBoard.ContestBalance);
		if (ContestBalance < 0f)
		{
			num = 0f - ContestBalance;
			text = animBad;
			text2 = animGood;
		}
		else
		{
			num = ContestBalance;
			text = animGood;
			text2 = animBad;
		}
		if (text != null && num > 0.01f)
		{
			Animation[animBase].weight = 1f - num;
			Blend(text, num, 0f);
			if (Animation.IsPlaying(text2))
			{
				Animation.Stop(text2);
			}
		}
		else
		{
			if (Animation.IsPlaying(animGood))
			{
				Animation.Stop(animGood);
			}
			if (Animation.IsPlaying(animBad))
			{
				Animation.Stop(animBad);
			}
		}
		if (ContestBalance <= -1f)
		{
			State = E_State.Lost;
			StopAnims();
		}
		else if (ContestBalance >= 1f)
		{
			State = E_State.Won;
			StopAnims();
		}
	}

	private void ContestLost()
	{
		Owner.BlackBoard.Invulnerable = false;
		State = E_State.Finish;
		string contestAnim = Owner.AnimSet.GetContestAnim(E_ContestState.Lost);
		CrossFade(contestAnim, 0.1f, PlayMode.StopSameLayer);
		EndOfStateTime = Time.timeSinceLevelLoad + Animation[contestAnim].length * 0.9f;
		Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
		Owner.BlackBoard.MotionType = E_MotionType.None;
	}

	private void ContestWon()
	{
		Owner.BlackBoard.Invulnerable = false;
		State = E_State.Finish;
		string contestAnim = Owner.AnimSet.GetContestAnim(E_ContestState.Won);
		CrossFade(contestAnim, 0.1f, PlayMode.StopSameLayer);
		EndOfStateTime = Time.timeSinceLevelLoad + Animation[contestAnim].length * 0.9f;
	}

	private void EnableSmoothRotation(bool enable)
	{
		ComponentBody component = Owner.GetComponent<ComponentBody>();
		if ((bool)component)
		{
			component.EnableSmoothRotation(enable);
		}
	}

	private void StopAnims()
	{
		if (Animation.IsPlaying(animBase))
		{
			Animation.Stop(animBase);
		}
		if (Animation.IsPlaying(animGood))
		{
			Animation.Stop(animGood);
		}
		if (Animation.IsPlaying(animBad))
		{
			Animation.Stop(animBad);
		}
	}
}
