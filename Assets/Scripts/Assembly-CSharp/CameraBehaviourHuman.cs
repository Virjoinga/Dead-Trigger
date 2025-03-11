using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourHuman : CameraBehaviour
{
	private enum E_State
	{
		ThirdPerson = 0,
		FPV = 1
	}

	private AgentHuman Owner;

	private CameraState State;

	private Dictionary<E_State, CameraState> States = new Dictionary<E_State, CameraState>();

	private void Awake()
	{
		Owner = GetComponent<AgentHuman>();
		States.Add(E_State.ThirdPerson, new CameraState3RD(Owner));
		States.Add(E_State.FPV, new CameraStateFPV(Owner));
		State = States[E_State.FPV];
		BlackBoard blackBoard = Owner.BlackBoard;
		blackBoard.ActionHandler = (BlackBoard.AgentActionHandler)Delegate.Combine(blackBoard.ActionHandler, new BlackBoard.AgentActionHandler(HandleAction));
	}

	public override Transform GetCameraWorldTransform()
	{
		if (State != null)
		{
			return State.GetCameraWorldTransform();
		}
		return null;
	}

	public override Transform GetCameraFPVTransform()
	{
		if (State != null)
		{
			return State.GetCameraFPVTransform();
		}
		return null;
	}

	public override void Activate(SpawnPoint spawn)
	{
		State = States[E_State.FPV];
		State.Activate(spawn.Transform);
	}

	public void HandleAction(AgentAction a)
	{
	}

	private void Update()
	{
	}
}
