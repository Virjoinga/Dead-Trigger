using UnityEngine;

public class AnimStateAttackMelee : AnimState
{
	private enum E_State
	{
		E_PREPARING_FOR_ATTACK = 0,
		E_ATTACKING = 1,
		E_FINISH = 2
	}

	private const float MoveTime = 0.2f;

	private AgentActionAttackMelee Action;

	private DestructibleObject DestrObj;

	private GameObject Target;

	private Vector3 StartPosition;

	private Vector3 FinalPosition;

	private Vector3 DestrObjDir;

	private float CurrentMoveTime;

	private bool PositionOK;

	private E_State State;

	private float AnimTime;

	private string AnimName;

	public AnimStateAttackMelee(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void OnActivate(AgentAction action)
	{
		DestrObj = ((Owner.BlackBoard.ImportantObject == null) ? null : (Owner.BlackBoard.ImportantObject as DestructibleObject));
		Target = ((DestrObj != null) ? DestrObj.GetGameObject() : ((!(Owner.BlackBoard.DangerousEnemy != null)) ? null : Owner.BlackBoard.DangerousEnemy.GameObject));
		base.OnActivate(action);
		if (Target == null)
		{
			Release();
			return;
		}
		Owner.BlackBoard.ReactOnHits = false;
		Owner.BlackBoard.BusyAction = true;
		Owner.BlackBoard.Desires.WeaponTriggerOn = false;
		Owner.BlackBoard.MotionType = E_MotionType.Attack;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0f;
	}

	public override void OnDeactivate()
	{
		Owner.BlackBoard.ReactOnHits = true;
		Owner.BlackBoard.BusyAction = false;
		Owner.BlackBoard.PrevMotionType = Owner.BlackBoard.MotionType;
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Action.SetSuccess();
		Action = null;
		base.OnDeactivate();
		Target = null;
		DestrObj = null;
	}

	public override void Reset()
	{
		Action.SetSuccess();
		Action = null;
		CurrentMoveTime = 0f;
		DestrObj = null;
		Target = null;
		base.Reset();
	}

	public override void Update()
	{
		if (!Owner || !Owner.BlackBoard.DangerousEnemy)
		{
			Release();
			return;
		}
		Vector3 lookRotation = ((!(DestrObj != null)) ? (Target.transform.position - Owner.Transform.position) : DestrObjDir);
		Owner.BlackBoard.Desires.Rotation.SetLookRotation(lookRotation);
		AnimTime += Time.deltaTime;
		if (State == E_State.E_PREPARING_FOR_ATTACK)
		{
			if (!PositionOK)
			{
				if ((FinalPosition - StartPosition).sqrMagnitude < 0.010000001f)
				{
					CurrentMoveTime = 0.2f;
				}
				CurrentMoveTime += Time.deltaTime;
				if (CurrentMoveTime >= 0.2f)
				{
					CurrentMoveTime = 0.2f;
					PositionOK = true;
				}
				float num = Mathf.Min(1f, CurrentMoveTime / 0.2f);
				Vector3 destination = Mathfx.Sinerp(StartPosition, FinalPosition, num);
				Owner.NavMeshAgent.speed = 1.5f;
				Owner.NavMeshAgent.SetDestination(destination);
				if (num > 0.9f)
				{
					PositionOK = true;
				}
			}
			else
			{
				State = E_State.E_ATTACKING;
				PlayAnim();
			}
		}
		if (State == E_State.E_ATTACKING && AnimTime > Animation[AnimName].length * 0.25f)
		{
			if (DestrObj == null)
			{
				if (Owner.BlackBoard.DistanceToTarget <= Owner.BlackBoard.WeaponRange && Owner.WorldState.GetWSProperty(E_PropKey.EnemyAheadOfMe).GetBool())
				{
					Owner.BlackBoard.DangerousEnemy.TakeDamage(Owner, Owner.BlackBoard.AttackSetup.MeleeAttackDamage, null, Action.AttackDir * 1f, E_WeaponID.None, E_WeaponType.Melee);
				}
			}
			else
			{
				DestrObj.TakeDamage(Owner, Owner.BlackBoard.AttackSetup.ObjectAttackDamage);
			}
			State = E_State.E_FINISH;
		}
		if (State == E_State.E_FINISH && AnimTime > Animation[AnimName].length * 0.75f)
		{
			Release();
		}
	}

	public override void Release()
	{
		Transform.parent = null;
		base.Release();
	}

	public override bool HandleNewAction(AgentAction action)
	{
		if (action is AgentActionUse)
		{
			if (Action != null)
			{
				action.SetFailed();
			}
		}
		else if (action is AgentActionInjury)
		{
			if (!PlayingInjury())
			{
				Animation.Stop();
			}
			PlayInjuryAnimation(action as AgentActionInjury);
			return true;
		}
		return false;
	}

	private void PlayAnim()
	{
		if (Owner.debugAnims)
		{
			Debug.Log("PlayAnim() " + Time.timeSinceLevelLoad);
		}
		AnimName = Owner.AnimSet.GetWeaponAnim(Action.WeaponAction);
		float num = TimeManager.Instance.GetRealDeltaTime() / Time.deltaTime;
		CrossFade(AnimName, 0.25f / num, PlayMode.StopSameLayer);
		AnimTime = 0f;
	}

	protected override void Initialize(AgentAction action)
	{
		base.Initialize(action);
		Owner.BlackBoard.MotionType = E_MotionType.Attack;
		Action = action as AgentActionAttackMelee;
		StartPosition = Transform.position;
		if (DestrObj == null)
		{
			FinalPosition = Transform.position;
		}
		else
		{
			Transform attackPoint = DestrObj.GetAttackPoint(Owner);
			if (attackPoint != null)
			{
				FinalPosition = attackPoint.position;
				DestrObjDir = -attackPoint.forward;
			}
			else
			{
				FinalPosition = DestrObj.GetGameObject().transform.position;
				DestrObjDir = FinalPosition - Transform.position;
				FinalPosition -= DestrObjDir.normalized;
			}
		}
		CurrentMoveTime = 0f;
		PositionOK = false;
		State = E_State.E_PREPARING_FOR_ATTACK;
	}
}
