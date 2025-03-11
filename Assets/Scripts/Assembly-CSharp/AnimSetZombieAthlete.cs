using System;

[Serializable]
public class AnimSetZombieAthlete : AnimSetZombie
{
	protected override string GetRunAnim()
	{
		return "Sprint3";
	}
}
