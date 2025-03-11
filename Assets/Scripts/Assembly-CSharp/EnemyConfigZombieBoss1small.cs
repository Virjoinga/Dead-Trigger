public class EnemyConfigZombieBoss1small : EnemyConfig
{
	private new void Awake()
	{
		base.Awake();
		AnimSet = base.Owner.GameObject.AddComponent<AnimSetZombieBoss1>();
		base.AnimComponent.SetFSM(E_AnimFSMTypes.ZombieBoss1);
	}

	protected override void SetProps()
	{
		base.Owner.BlackBoard.MovementSkill = F_MovementSkill.WalkAndRun;
		base.MaxWalkSpeed = 1.6f;
		base.MaxRunSpeed = 3.5f;
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
		base.Owner.AddGOAPGoal(E_GOAPGoals.LookAtTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.CheckEvent);
		base.Owner.AddGOAPGoal(E_GOAPGoals.KillTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.PlayAnim);
		base.Owner.InitializeGOAP();
	}
}
