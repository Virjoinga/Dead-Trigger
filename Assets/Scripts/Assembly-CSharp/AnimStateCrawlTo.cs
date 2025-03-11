using UnityEngine;

public class AnimStateCrawlTo : AnimState
{
	private AgentActionCrawlTo Action;

	private string AnimName;

	private float TimeToFinishRotateAction;

	private AgentActionRotate RotateAction;

	public AnimStateCrawlTo(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		Owner.BlackBoard.Velocity = Vector3.zero;
		base.OnActivate(action);
		AnimName = null;
	}

	public override void OnDeactivate()
	{
		if (Owner.NavMeshAgent.enabled)
		{
			Owner.NavMeshAgent.Stop();
		}
		Owner.NavMeshAgent.updateRotation = false;
		if (RotateAction != null)
		{
			RotateAction.SetSuccess();
			RotateAction = null;
		}
		if (Owner.IsAlive)
		{
			Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
			Owner.BlackBoard.MotionType = E_MotionType.None;
		}
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
		Owner.BlackBoard.Velocity = Vector3.zero;
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
	}

	public override void Reset()
	{
		Action.SetSuccess();
		Action = null;
		if (RotateAction != null)
		{
			RotateAction.SetSuccess();
			RotateAction = null;
		}
		base.Reset();
	}

	private ActionPoint GetActionPoint()
	{
		UnityEngine.AI.NavMeshAgent component = Owner.GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (component == null || !component.isOnOffMeshLink)
		{
			return null;
		}
		return component.currentOffMeshLinkData.offMeshLink.gameObject.GetComponent<ActionPoint>();
	}

	private void UpdateActionPoint()
	{
		ActionPoint actionPoint = GetActionPoint();
		if (actionPoint != null && actionPoint.enabled)
		{
			KillMe();
		}
	}

	private void UpdateCrawlTime()
	{
		if (!(Owner.BlackBoard.DistanceToTarget < Owner.BlackBoard.CrawlTimePlayerRange))
		{
			Owner.BlackBoard.CrawlTime += Time.deltaTime;
			if (Owner.BlackBoard.CrawlTime > Owner.BlackBoard.BaseSetup.MaxCrawlTime)
			{
				KillMe();
			}
		}
	}

	private void KillMe()
	{
		Owner.TakeDamage(Owner, Owner.BlackBoard.Health, null, new Vector3(0f, 0f, 0f), E_WeaponID.None, E_WeaponType.Unknown);
	}

	public override void Update()
	{
		if (Owner.IsAlive && !(Time.deltaTime < float.Epsilon))
		{
			UpdateRotation();
			if (RotateAction != null && TimeToFinishRotateAction < Time.timeSinceLevelLoad)
			{
				RotateAction.SetSuccess();
				RotateAction = null;
			}
			if (!PlayingInjury())
			{
				Owner.BlackBoard.MotionType = GetMotionType();
				Owner.BlackBoard.MoveType = GetMoveType();
				Owner.BlackBoard.Speed = GetMoveSpeed(Owner.BlackBoard.MotionType);
				Owner.NavMeshAgent.speed = Owner.BlackBoard.Speed;
			}
			else
			{
				Owner.BlackBoard.Speed = Owner.NavMeshAgent.speed * 0.8f;
				Owner.BlackBoard.Velocity = Owner.NavMeshAgent.velocity * 0.8f;
				Owner.BlackBoard.MotionType = GetMotionType();
				Owner.BlackBoard.MoveType = GetMoveType();
				Owner.NavMeshAgent.speed = Owner.BlackBoard.Speed;
				Owner.NavMeshAgent.velocity = Owner.BlackBoard.Velocity;
			}
			PlayAnim();
			if (Owner.NavMeshAgent.remainingDistance < Action.MinDistance && !Owner.BlackBoard.ActionPointOn)
			{
				Release();
			}
			UpdateActionPoint();
			UpdateCrawlTime();
		}
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionCrawlTo)
		{
			if (Action != null)
			{
				Action.SetSuccess();
			}
			SetFinished(false);
			Initialize(action);
			return true;
		}
		if (action is AgentActionInjury)
		{
			PlayInjuryAnimation(action as AgentActionInjury);
			return true;
		}
		if (action is AgentActionAttack)
		{
			string weaponAnim = Owner.AnimSet.GetWeaponAnim(E_WeaponAction.Fire);
			Animation[weaponAnim].blendMode = AnimationBlendMode.Additive;
			if (Animation.IsPlaying(weaponAnim))
			{
				Animation.Stop(weaponAnim);
				Animation.Play(weaponAnim);
			}
			else
			{
				Animation.Blend(weaponAnim, 1f, 0.1f);
			}
			action.SetSuccess();
			return true;
		}
		if (action is AgentActionRotate)
		{
			RotateAction = action as AgentActionRotate;
			string rotateAnim = Owner.AnimSet.GetRotateAnim(RotateAction.Rotation, RotateAction.Angle);
			if (rotateAnim != null && !Animation.IsPlaying(rotateAnim))
			{
				Animation[rotateAnim].blendMode = AnimationBlendMode.Blend;
				Animation[rotateAnim].layer = 1;
				TimeToFinishRotateAction = Time.timeSinceLevelLoad + Animation[rotateAnim].length;
				Blend(rotateAnim, 0.15f);
				if (Owner.debugAnims)
				{
					Debug.Log("AnimStateCrawlTo: Rotate = " + rotateAnim + ", " + Animation[rotateAnim].length);
				}
			}
			return true;
		}
		return false;
	}

	private void PlayAnim()
	{
		AnimName = Owner.AnimSet.GetMoveAnim(Action.MotionSide);
		if (!Animation.IsPlaying(AnimName))
		{
			CrossFade(AnimName, 0.2f, PlayMode.StopSameLayer);
		}
	}

	private E_MotionType GetMotionType()
	{
		return Action.Motion;
	}

	private E_MoveType GetMoveType()
	{
		return Action.MoveType;
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Action = action as AgentActionCrawlTo;
		if (!SetTargetLocation(Action.FinalPosition))
		{
			Debug.LogWarning(ToString() + " SetTargetLocation Failed - " + Action.FinalPosition);
			Release();
			return;
		}
		Owner.NavMeshAgent.updateRotation = Action.UseNavMeshAgentRotation;
		Owner.BlackBoard.MotionType = Action.Motion;
		Owner.BlackBoard.MoveType = Action.MoveType;
		Owner.BlackBoard.Speed = GetMoveSpeed(Action.Motion);
		Owner.NavMeshAgent.speed = Owner.BlackBoard.Speed;
	}

	private float GetMoveSpeed(E_MotionType motion)
	{
		if (motion == E_MotionType.Crawl)
		{
			return Owner.BlackBoard.BaseSetup.MaxCrawlSpeed;
		}
		return Owner.MaxWalkSpeed;
	}

	private void UpdateRotation()
	{
		if (Action.UseNavMeshAgentRotation)
		{
			Owner.BlackBoard.Desires.Rotation = Owner.Transform.rotation;
		}
		else if ((bool)Action.LookTarget)
		{
			Owner.BlackBoard.Desires.Rotation.SetLookRotation((Action.LookTarget.position - Owner.Position).normalized);
		}
		else if (Owner.NavMeshAgent.velocity != Vector3.zero)
		{
			if (Owner.BlackBoard.MoveType == E_MoveType.Forward)
			{
				Owner.BlackBoard.Desires.Rotation.SetLookRotation(Owner.NavMeshAgent.velocity.normalized);
			}
			else if (Owner.BlackBoard.MoveType != E_MoveType.Backward)
			{
			}
		}
	}
}
