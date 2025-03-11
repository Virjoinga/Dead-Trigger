using System;

[Serializable]
public class ComponentWeaponsAI : ComponentWeapons
{
	private void Activate(SpawnPoint spawn)
	{
	}

	protected override void Initialize()
	{
		base.Initialize();
		base.CurrentWeapon = E_WeaponID.None;
	}

	private void Activate()
	{
		throw new NotImplementedException();
	}

	private void Deactivate()
	{
		throw new NotImplementedException();
	}
}
