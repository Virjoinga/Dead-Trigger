using UnityEngine;

public class AnimFSMZombieNormal : AnimFSM
{
	public AnimFSMZombieNormal(Animation anims, AgentHuman owner)
		: base(anims, owner)
	{
	}

	public override void Initialize()
	{
		AnimStates.Clear();
		AnimStates.Add(AgentActionFactory.E_Type.Idle, new AnimStateIdle(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.Goto, new AnimStateGoTo(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.CrawlTo, new AnimStateCrawlTo(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.AttackMelee, new AnimStateAttackMelee(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.PlayAnim, new AnimStatePlayAnim(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.Contest, new AnimStateContestZombieButtons(AnimEngine, Owner));
		AnimStates.Add(AgentActionFactory.E_Type.Death, new AnimStateDeath(AnimEngine, Owner));
		DefaultAnimState = AnimStates[AgentActionFactory.E_Type.Idle];
		base.Initialize();
	}
}
