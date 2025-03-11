using System;
using UnityEngine;

public class EnemyConfigZombieWalker1 : EnemyConfig
{
	private new void Awake()
	{
		base.Awake();
		AnimSet = base.Owner.GameObject.AddComponent<AnimSetZombie>();
		base.AnimComponent.SetFSM(E_AnimFSMTypes.ZombieNormal);
	}

	protected override void SetProps()
	{
		base.Owner.BlackBoard.MovementSkill = F_MovementSkill.Walk;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			base.MaxWalkSpeed = 1.1f;
			base.MaxRunSpeed = 2.5f;
		}
		else
		{
			base.MaxWalkSpeed = 1.6f;
			base.MaxRunSpeed = 3.5f;
		}
	}

	private void Start()
	{
		base.Owner.SensorsComponent.AddSensor(E_SensorType.EyesAi, true);
		base.Owner.AddGOAPAction(E_GOAPAction.LookAtTarget);
		base.Owner.AddGOAPAction(E_GOAPAction.CheckEvent);
		base.Owner.AddGOAPAction(E_GOAPAction.AttackMelee);
		base.Owner.AddGOAPAction(E_GOAPAction.GotoWeaponRange);
		base.Owner.AddGOAPAction(E_GOAPAction.CrawlToWeaponRange);
		base.Owner.AddGOAPAction(E_GOAPAction.PlayAnim);
		base.Owner.AddGOAPAction(E_GOAPAction.CheckBait);
		base.Owner.AddGOAPAction(E_GOAPAction.DestroyObject);
		base.Owner.AddGOAPAction(E_GOAPAction.Contest);
		base.Owner.AddGOAPGoal(E_GOAPGoals.LookAtTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.CheckEvent);
		base.Owner.AddGOAPGoal(E_GOAPGoals.KillTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.PlayAnim);
		base.Owner.AddGOAPGoal(E_GOAPGoals.CheckBait);
		base.Owner.AddGOAPGoal(E_GOAPGoals.DestroyObject);
		base.Owner.InitializeGOAP();
		BlackBoard blackBoard = base.Owner.BlackBoard;
		blackBoard.ActionHandler = (BlackBoard.AgentActionHandler)Delegate.Combine(blackBoard.ActionHandler, new BlackBoard.AgentActionHandler(HandleAction));
	}

	private void Activate(SpawnPoint spawn)
	{
	}

	private void Deactivate()
	{
	}

	private void Update()
	{
	}

	public void HandleAction(AgentAction a)
	{
	}
}
