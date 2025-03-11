using System;
using UnityEngine;

[Serializable]
public class ProjectileInitSettings
{
	public float Damage;

	public float BodyPartDamageModif;

	public float Impuls;

	public float Speed = 10f;

	public float EffectiveRange;

	public float MaxRange;

	public float MaxRangeDamage;

	public Vector3 Destination = new Vector3(0f, 0f, 0f);

	public AgentHuman Agent;

	public E_WeaponID Weapon;

	public E_WeaponType WeaponType;

	public Transform IgnoreTransform;

	public ProjectileInitSettings()
	{
	}

	public ProjectileInitSettings(ProjectileInitSettings inSettings)
	{
		Damage = inSettings.Damage;
		BodyPartDamageModif = inSettings.BodyPartDamageModif;
		Impuls = inSettings.Impuls;
		Speed = inSettings.Speed;
		Agent = inSettings.Agent;
		Weapon = inSettings.Weapon;
		WeaponType = inSettings.WeaponType;
		EffectiveRange = inSettings.EffectiveRange;
		MaxRange = inSettings.MaxRange;
		MaxRangeDamage = inSettings.MaxRangeDamage;
		IgnoreTransform = inSettings.IgnoreTransform;
		Destination = inSettings.Destination;
	}

	public ProjectileInitSettings(ProjectileInitSettingsInspector inSettings)
	{
		Damage = inSettings.Damage;
		Impuls = inSettings.Impuls;
		Speed = inSettings.Speed;
	}
}
