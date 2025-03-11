using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Triggers/Set Game Event")]
public class TriggerGameEvent : MonoBehaviour
{
	public List<GameEvent> GameEvents = new List<GameEvent>();

	public E_AgentType CheckEnemyType = E_AgentType.None;

	public bool DisableAfterUse = true;

	private bool ResetEventsOnExit;

	private bool Fired;

	private GameObject GameObject;

	private void Awake()
	{
		GameObject = base.gameObject;
	}

	public void Enable(bool enable)
	{
		GameObject._SetActiveRecursively(enable);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (CheckEnemyType != E_AgentType.None)
		{
			if (other == null || other.GetComponent<Rigidbody>() == null || other.GetComponent<Rigidbody>().gameObject == null)
			{
				return;
			}
			Agent firstComponentUpward = other.GetComponent<Rigidbody>().gameObject.GetFirstComponentUpward<Agent>();
			if (firstComponentUpward == null || firstComponentUpward.AgentType != CheckEnemyType)
			{
				return;
			}
		}
		else if (other != Player.Instance.Owner.CharacterController)
		{
			return;
		}
		foreach (GameEvent gameEvent in GameEvents)
		{
			Mission.Instance.SendGameEvent(gameEvent.Name, gameEvent.State, gameEvent.Delay);
		}
		if (DisableAfterUse)
		{
			GameObject._SetActiveRecursively(false);
		}
		Fired = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (!ResetEventsOnExit)
		{
			return;
		}
		if (CheckEnemyType != E_AgentType.None)
		{
			if (other == null || other.GetComponent<Rigidbody>() == null || other.GetComponent<Rigidbody>().gameObject == null)
			{
				return;
			}
			Agent firstComponentUpward = other.GetComponent<Rigidbody>().gameObject.GetFirstComponentUpward<Agent>();
			if (firstComponentUpward == null || firstComponentUpward.AgentType != CheckEnemyType)
			{
				return;
			}
		}
		else if (other != Player.Instance.Owner.CharacterController)
		{
			return;
		}
		foreach (GameEvent gameEvent in GameEvents)
		{
			Mission.Instance.SendGameEvent(gameEvent.Name, gameEvent.invertedState, gameEvent.Delay);
		}
		if (DisableAfterUse)
		{
			GameObject._SetActiveRecursively(false);
		}
		Fired = true;
	}

	public void Enable()
	{
		if (!Fired || !DisableAfterUse)
		{
			GameObject._SetActiveRecursively(true);
		}
	}

	public void Disable()
	{
		GameObject._SetActiveRecursively(false);
	}

	public void Reset()
	{
		Fired = false;
		GameObject._SetActiveRecursively(true);
	}

	private void OnDrawGizmos()
	{
		BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
		if (boxCollider != null)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawCube(boxCollider.center, boxCollider.size);
		}
	}
}
