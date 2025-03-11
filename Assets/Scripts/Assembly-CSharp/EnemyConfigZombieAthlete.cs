public class EnemyConfigZombieAthlete : EnemyConfig
{
	private new void Awake()
	{
		base.Awake();
		AnimSet = base.Owner.GameObject.AddComponent<AnimSetZombieAthlete>();
		base.AnimComponent.SetFSM(E_AnimFSMTypes.ZombieNormal);
	}

	protected override void SetProps()
	{
		base.Owner.BlackBoard.MovementSkill = F_MovementSkill.Run;
		base.MaxWalkSpeed = 1.6f;
		base.MaxRunSpeed = 5f;
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
		base.Owner.AddGOAPGoal(E_GOAPGoals.LookAtTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.CheckEvent);
		base.Owner.AddGOAPGoal(E_GOAPGoals.KillTarget);
		base.Owner.AddGOAPGoal(E_GOAPGoals.PlayAnim);
		base.Owner.InitializeGOAP();
	}
}
