using System.Collections.Generic;
using UnityEngine;

public class SensorTest : SensorBase
{
	private AgentHuman MyEnemy;

	public SensorTest(AgentHuman owner)
		: base(owner)
	{
		base.Owner.BlackBoard.VisibleTarget = null;
		MyEnemy = ((!Player.Instance) ? null : Player.Instance.Owner);
	}

	public override void Update()
	{
		if (!base.Owner.BlackBoard.Stop)
		{
			base.Owner.WorldState.SetWSProperty(E_PropKey.SeeEnemy, false);
			base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, false);
			base.Owner.BlackBoard.VisibleTarget = null;
			if ((bool)CheckForBait())
			{
				base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, true);
			}
			if (!(MyEnemy == null) && MyEnemy.IsAlive)
			{
				SendSeeEvent(MyEnemy);
				base.Owner.BlackBoard.VisibleTarget = MyEnemy;
			}
		}
	}

	public override void Reset()
	{
		MyEnemy = ((!Player.Instance) ? null : Player.Instance.Owner);
		base.Owner.WorldState.SetWSProperty(E_PropKey.SeeEnemy, false);
		base.Owner.WorldState.SetWSProperty(E_PropKey.CheckBait, false);
		base.Owner.BlackBoard.VisibleTarget = null;
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
		fact.Delay = Random.Range(0.3f, 0.7f);
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

	private GameObject CheckForBait()
	{
		List<IImportantObject> importantObjects = Mission.Instance.CurrentGameZone.ImportantObjects;
		foreach (IImportantObject item in importantObjects)
		{
			if ((item.GetImportantObjectType() == E_ImportantObjectType.Bait || item.GetImportantObjectType() == E_ImportantObjectType.GrenadeBait) && GetSqrSpeed(item.GetGameObject()) < 2f)
			{
				return item.GetGameObject();
			}
		}
		return null;
	}
}
