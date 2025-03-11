using System;
using UnityEngine;

[AddComponentMenu("Entities/Action Point")]
public class ActionPoint : MonoBehaviour
{
	[Serializable]
	public class AnimData
	{
		public AnimationClip m_Anim;

		public float m_MoveSpeed;
	}

	public bool AgentInvulnerable;

	public AnimData m_AnimMove = new AnimData();

	public AnimData m_AnimRun = new AnimData();

	private void Awake()
	{
	}
}
