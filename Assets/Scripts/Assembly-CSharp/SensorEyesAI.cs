using System.Collections.Generic;
using UnityEngine;

public class SensorEyesAI : SensorBase
{
	private AgentHuman MyEnemy;

	private float NextImportantObjCheckTime;

	public SensorEyesAI(AgentHuman owner)
		: base(owner)
	{
		base.Owner.BlackBoard.VisibleTarget = null;
		base.Owner.BlackBoard.SetImportantObject(null);
		MyEnemy = ((!Player.Instance) ? null : Player.Instance.Owner);
	}

	public override void Update()
	{
		if (base.Owner.BlackBoard.Stop)
		{
			return;
		}
		base.Owner.WorldState.SetWSProperty(E_PropKey.SeeEnemy, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.LookingAtTarget, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.AheadOfEnemy, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyAheadOfMe, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyLookingAtMe, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InWeaponRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InContestRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InVomitRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.DestroyObject, false);
		base.Owner.BlackBoard.VisibleTarget = null;
		if (Time.timeSinceLevelLoad >= NextImportantObjCheckTime && !base.Owner.BlackBoard.ActionPointOn)
		{
			IImportantObject importantObject = null;
			if ((importantObject = CheckForBait()) != null)
			{
				if (importantObject != base.Owner.BlackBoard.ImportantObject)
				{
					base.Owner.BlackBoard.SetImportantObject(importantObject);
				}
				base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, true);
			}
			else if ((importantObject = CheckForDestructibleObject()) != null)
			{
				if (importantObject != base.Owner.BlackBoard.ImportantObject)
				{
					base.Owner.BlackBoard.SetImportantObject(importantObject);
				}
				base.Owner.WorldState.SetWSProperty(E_PropKey.DestroyObject, true);
			}
			else
			{
				base.Owner.BlackBoard.SetImportantObject(null);
			}
		}
		if (MyEnemy == null || !MyEnemy.IsAlive)
		{
			return;
		}
		Vector3 position = base.Owner.TransformEye.position;
		Vector3 vector = MyEnemy.TransformTarget.position - position;
		Vector3 forward = base.Owner.Forward;
		float num = (base.Owner.WorldState.GetWSProperty(E_PropKey.DestroyObject).GetBool() ? (base.Owner.BlackBoard.ImportantObject.GetGameObject().transform.position - base.Owner.Transform.position).magnitude : vector.magnitude);
		base.Owner.BlackBoard.DistanceToTarget = num;
		base.Owner.BlackBoard.DirToTarget = vector;
		if (num > base.Owner.BlackBoard.SightRange || Vector3.Angle(forward, vector) > base.Owner.BlackBoard.SightFov)
		{
			SendLostEvent(MyEnemy);
			return;
		}
		if (num < base.Owner.BlackBoard.WeaponRange && !base.Owner.BlackBoard.ActionPointOn)
		{
			base.Owner.WorldState.SetWSProperty(E_PropKey.InWeaponRange, true);
		}
		if (num < base.Owner.BlackBoard.ContestRange && !base.Owner.BlackBoard.ActionPointOn)
		{
			base.Owner.WorldState.SetWSProperty(E_PropKey.InContestRange, true);
		}
		vector.Normalize();
		SendSeeEvent(MyEnemy);
		int layerMask = ~(ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.Player | ObjectLayerMask.Enemy | ObjectLayerMask.EnemyBox);
		RaycastHit hitInfo;
		bool flag = ((!Physics.Raycast(base.Owner.EyePosition, vector, out hitInfo, num, layerMask)) ? true : false);
		if (base.Owner.WorldState.GetWSProperty(E_PropKey.SeeEnemy).GetBool())
		{
			base.Owner.BlackBoard.VisibleTarget = MyEnemy;
			float num2 = Vector3.Angle(forward, vector);
			if (num2 < Mathf.Lerp(10f, 60f, 1f - base.Owner.BlackBoard.DistanceToTarget / base.Owner.BlackBoard.SightRange))
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.LookingAtTarget, true);
			}
			float num3 = Vector3.Angle(MyEnemy.Forward, -vector);
			if (num3 < 10f)
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyLookingAtMe, true);
			}
			float num4 = Vector3.Angle(vector, MyEnemy.Forward);
			if (num4 > 135f && num4 < 225f)
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.AheadOfEnemy, true);
			}
			if (num2 < 90f)
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyAheadOfMe, true);
			}
			if (flag && num3 < 45f && num < base.Owner.BlackBoard.VomitRangeMax && num > base.Owner.BlackBoard.VomitRangeMin && !base.Owner.BlackBoard.ActionPointOn)
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.InVomitRange, true);
			}
			if ((base.Owner.BlackBoard.MovementSkill & F_MovementSkill.Berserk) != 0 && num > Random.Range(7f, 8f) && num3 < 20f && flag)
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.Berserk, true);
			}
			if (num2 < 30f && num3 > 100f)
			{
				if (base.Owner.CanDoContest(MyEnemy, true))
				{
					base.Owner.StartContest(MyEnemy);
				}
			}
			else if (base.Owner.IsInContest() && !MyEnemy.IsInContest())
			{
				base.Owner.StopContest(MyEnemy);
			}
		}
		CheckContestValid();
	}

	private void CheckContestValid()
	{
		if (base.Owner.WorldState.GetWSProperty(E_PropKey.Contest).GetBool() && !MyEnemy.WorldState.GetWSProperty(E_PropKey.Contest).GetBool() && !base.Owner.CanDoContest(MyEnemy, false))
		{
			base.Owner.StopContest(MyEnemy);
		}
	}

	public override void Reset()
	{
		base.Owner.BlackBoard.VisibleTarget = null;
		base.Owner.BlackBoard.SetImportantObject(null);
		MyEnemy = ((!Player.Instance) ? null : Player.Instance.Owner);
		base.Owner.WorldState.SetWSProperty(E_PropKey.SeeEnemy, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.LookingAtTarget, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.AheadOfEnemy, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyAheadOfMe, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.EnemyLookingAtMe, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InWeaponRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InContestRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.InVomitRange, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.DestroyObject, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.Contest, false);
	}

	private void SendLostEvent(AgentHuman target)
	{
		Fact fact = base.Owner.Memory.GetFact(E_EventTypes.EnemySee);
		if (fact != null)
		{
			base.Owner.Memory.RemoveFact(E_EventTypes.EnemySee);
			fact = FactsFactory.Create(E_EventTypes.EnemyLost);
			fact.Agent = target;
			fact.Position = target.Position;
			fact.LiveTime = 180f;
			fact.Delay = Random.Range(0.2f, 0.6f);
			base.Owner.AddFactToMemory(fact);
		}
	}

	private void SendSeeEvent(AgentHuman target)
	{
		base.Owner.Memory.RemoveFact(E_EventTypes.EnemyLost);
		base.Owner.Memory.RemoveFact(E_EventTypes.EnemyHideInCover);
		Fact fact = FactsFactory.Create(E_EventTypes.EnemySee);
		fact.Type = E_EventTypes.EnemySee;
		fact.Position = target.Position;
		fact.Delay = 0f;
		fact.LiveTime = 100f;
		fact.Agent = target;
		base.Owner.AddFactToMemory(fact);
		if (base.Owner.Memory.GetValidFact(E_EventTypes.EnemySee) != null)
		{
			base.Owner.WorldState.SetWSProperty(E_PropKey.SeeEnemy, true);
		}
	}

	private float GetSqrSpeed(GameObject obj)
	{
		return (!obj.GetComponent<Rigidbody>()) ? 0f : obj.GetComponent<Rigidbody>().velocity.sqrMagnitude;
	}

	private float GetSqrDistance(GameObject obj, Vector3 pos)
	{
		return (!(obj != null)) ? float.PositiveInfinity : (obj.transform.position - pos).sqrMagnitude;
	}

	private IImportantObject CompareDistance(IImportantObject first, IImportantObject second)
	{
		float sqrMagnitude = (first.GetGameObject().transform.position - base.Owner.transform.position).sqrMagnitude;
		float sqrMagnitude2 = (second.GetGameObject().transform.position - base.Owner.transform.position).sqrMagnitude;
		IImportantObject result;
		if (sqrMagnitude < sqrMagnitude2)
		{
			result = first;
		}
		else
		{
			result = second;
		}
		return result;
	}

	private IImportantObject CheckForBait()
	{
		List<IImportantObject> importantObjects = Mission.Instance.CurrentGameZone.ImportantObjects;
		foreach (IImportantObject item in importantObjects)
		{
			if (item.GetGameObject() != null && (item.GetImportantObjectType() == E_ImportantObjectType.Bait || item.GetImportantObjectType() == E_ImportantObjectType.GrenadeBait) && GetSqrSpeed(item.GetGameObject()) < 2f && IsPointReachable(item.GetGameObject().transform.position, base.Owner.BlackBoard.BaitRange))
			{
				return item;
			}
		}
		return null;
	}

	private IImportantObject CheckForDestructibleObject()
	{
		float sqrMagnitude = (MyEnemy.Transform.position - base.Owner.Transform.position).sqrMagnitude;
		if (sqrMagnitude < 25f)
		{
			Fact fact = base.Owner.Memory.GetFact(E_EventTypes.EnemyInjuredMe);
			if (fact != null && fact.Belief > 0.2f)
			{
				return null;
			}
		}
		DestructibleObject destructibleObject = base.Owner.BlackBoard.ImportantObject as DestructibleObject;
		IImportantObject importantObject = null;
		if (destructibleObject != null && destructibleObject.IsAlive && destructibleObject.GetGameObject() != null)
		{
			importantObject = destructibleObject;
		}
		else
		{
			List<IImportantObject> importantObjects = Mission.Instance.CurrentGameZone.ImportantObjects;
			foreach (IImportantObject item in importantObjects)
			{
				if (item.GetImportantObjectType() != E_ImportantObjectType.DestructibleObject || !(item.GetGameObject() != null))
				{
					continue;
				}
				destructibleObject = (DestructibleObject)item;
				if (destructibleObject.IsAlive && destructibleObject.GetRegisteredAgentsCount() < 1)
				{
					AttackPoint attackPoint = destructibleObject.FindAttackPoint(null);
					if (attackPoint != null && IsPointReachable(attackPoint.Transform.position, base.Owner.BlackBoard.DestructibleObjectRange))
					{
						importantObject = ((importantObject != null) ? CompareDistance(importantObject, item) : item);
					}
				}
			}
		}
		if (importantObject != null && MyEnemy != null)
		{
			float num = GetSqrDistance(importantObject.GetGameObject(), base.Owner.Transform.position) * 0.4f;
			if (sqrMagnitude > 4f && num < sqrMagnitude)
			{
				return importantObject;
			}
		}
		return null;
	}

	private bool IsPointReachable(Vector3 pos, float dist)
	{
		if (base.Owner.NavMeshAgent == null || !base.Owner.NavMeshAgent.enabled)
		{
			return false;
		}
		if ((base.Owner.Position - pos).sqrMagnitude <= dist * dist)
		{
			return true;
		}
		UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();
		bool flag = base.Owner.NavMeshAgent.CalculatePath(pos, navMeshPath);
		if (!flag)
		{
			Debug.Log(string.Concat("IsPointReachable: result=", flag, ", pos=", pos, ", status=", navMeshPath.status, ", corners=", navMeshPath.corners.Length));
		}
		return flag;
	}
}
