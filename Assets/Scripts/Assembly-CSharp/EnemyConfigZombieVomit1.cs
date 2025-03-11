public class EnemyConfigZombieVomit1 : EnemyConfig
{
	private new void Awake()
	{
		base.Awake();
		AnimSet = base.Owner.GameObject.AddComponent<AnimSetZombieSlow1>();
		base.AnimComponent.SetFSM(E_AnimFSMTypes.ZombieVomit);
	}

	protected override void SetProps()
	{
		base.Owner.BlackBoard.MovementSkill = F_MovementSkill.WalkAndRun;
		base.MaxWalkSpeed = 0.7f;
		base.MaxRunSpeed = 2.5f;
	}

	private void Start()
	{
		base.Owner.SensorsComponent.AddSensor(E_SensorType.EyesAi, true);
		base.Owner.AddGOAPAction(E_GOAPAction.LookAtTarget);
		base.Owner.AddGOAPAction(E_GOAPAction.CheckEvent);
		base.Owner.AddGOAPAction(E_GOAPAction.AttackMelee);
		base.Owner.AddGOAPAction(E_GOAPAction.AttackVomit);
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
	}
}
