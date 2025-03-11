using System;

[Serializable]
public class AnimSetZombieSlow1 : AnimSetZombie
{
	public override string GetIdleAnim()
	{
		if (WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Crawl)
		{
			return "CrawlIdle";
		}
		if (WorldState.GetWSProperty(E_PropKey.CheckBait).GetBool() && WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Stand && BlackBoard.BaitPhase == BlackBoard.E_BaitPhase.Stare)
		{
			return "LookIdle1";
		}
		return "Idle1";
	}
}
