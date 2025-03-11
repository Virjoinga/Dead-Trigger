using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interaction/Damage Event")]
public class DamageEvent : MonoBehaviour, IGameZoneControledObject
{
	public List<GameEvent> GameEvents = new List<GameEvent>();

	public List<OnGameEvent> OnGameEvents = new List<OnGameEvent>();

	public float MaxHealth;

	public GameObject Icon;

	private bool AllEventsAreOn;

	private float Health;

	private GameObject GameObject;

	private void Awake()
	{
		GameObject = base.gameObject;
	}

	private void Start()
	{
		GameZone firstComponentUpward = base.gameObject.GetFirstComponentUpward<GameZone>();
		firstComponentUpward.RegisterControllableObject(this);
		foreach (OnGameEvent onGameEvent in OnGameEvents)
		{
			GameBlackboard.Instance.GameEvents.AddEventChangeHandler(onGameEvent.Name, EventHandler);
		}
	}

	public void Enable()
	{
		Health = MaxHealth;
		AllEventsAreOn = TestEvents();
		GameObject._SetActiveRecursively(AllEventsAreOn);
		if ((bool)Icon)
		{
			Icon._SetActiveRecursively(AllEventsAreOn);
		}
	}

	public void Reset()
	{
		Health = MaxHealth;
	}

	public void Disable()
	{
		GameObject._SetActiveRecursively(false);
		if ((bool)Icon)
		{
			Icon._SetActiveRecursively(false);
		}
	}

	private bool TestEvents()
	{
		foreach (OnGameEvent onGameEvent in OnGameEvents)
		{
			if (GameBlackboard.Instance.GameEvents.GetState(onGameEvent.Name) != onGameEvent.State)
			{
				return false;
			}
		}
		return true;
	}

	public void OnProjectileHit(Projectile p)
	{
		if (!AllEventsAreOn)
		{
			return;
		}
		Health -= p.Damage();
		if (!(Health <= 0f))
		{
			return;
		}
		foreach (GameEvent gameEvent in GameEvents)
		{
			Mission.Instance.SendGameEvent(gameEvent.Name, gameEvent.State, gameEvent.Delay);
		}
		Disable();
	}

	public void EventHandler(string name, GameEvents.E_State state)
	{
		AllEventsAreOn = TestEvents();
		GameObject._SetActiveRecursively(AllEventsAreOn);
	}
}
