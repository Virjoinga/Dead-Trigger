using System;
using System.Collections.Generic;
using UnityEngine;

public class GameZoneBase : GameZoneCommon
{
	[NonSerialized]
	public SpawnPointPlayer PlayerSpawn;

	protected List<Agent> _Enemies = new List<Agent>();

	protected List<IImportantObject> _ImportantObjects = new List<IImportantObject>();

	protected GameObject GameObject;

	public List<Agent> Enemies
	{
		get
		{
			return _Enemies;
		}
	}

	public List<IImportantObject> ImportantObjects
	{
		get
		{
			return _ImportantObjects;
		}
	}

	public virtual void Initialize()
	{
		SpawnPointPlayer[] componentsInChildren = GameObject.GetComponentsInChildren<SpawnPointPlayer>(false);
		if (componentsInChildren == null)
		{
			return;
		}
		SpawnPointPlayer[] array = componentsInChildren;
		foreach (SpawnPointPlayer spawnPointPlayer in array)
		{
			if (spawnPointPlayer.enabled)
			{
				PlayerSpawn = spawnPointPlayer;
				break;
			}
		}
	}

	public void AddEnemy(Agent enemy)
	{
		if (!_Enemies.Contains(enemy))
		{
			_Enemies.Add(enemy);
		}
	}

	public virtual void Enable()
	{
	}

	public virtual void Disable()
	{
	}

	public virtual void CleanAllEnemies()
	{
		for (int num = _Enemies.Count - 1; num >= 0; num--)
		{
			Mission.Instance.ReturnAgentToCache(_Enemies[num].GameObject);
		}
		_Enemies.Clear();
	}

	public virtual void Reset()
	{
		StopAllCoroutines();
		for (int num = _Enemies.Count - 1; num >= 0; num--)
		{
			Mission.Instance.ReturnAgentToCache(_Enemies[num].GameObject);
		}
		_Enemies.Clear();
	}

	public bool IsEnemyInRange(Vector3 center, float maxLen)
	{
		float num = maxLen * maxLen;
		for (int num2 = _Enemies.Count - 1; num2 >= 0; num2--)
		{
			Agent agent = _Enemies[num2];
			if (agent.IsAlive && (agent.Position - center).sqrMagnitude < num)
			{
				return true;
			}
		}
		return false;
	}

	public float GetDistanceToNearestEnemy(Vector3 center, Agent ignoreAgent)
	{
		float num = float.MaxValue;
		for (int i = 0; i < _Enemies.Count; i++)
		{
			Agent agent = _Enemies[i];
			if (!(agent == ignoreAgent) && agent.IsAlive)
			{
				float num2 = Vector3.Distance(center, agent.Position);
				if (num2 < num)
				{
					num = num2;
				}
			}
		}
		return num;
	}

	public Agent GetNearestAliveEnemy(Vector3 start, Vector3 end, float radius, AgentHuman ignoreAgent)
	{
		float num = radius;
		Agent result = null;
		for (int i = 0; i < _Enemies.Count; i++)
		{
			Agent agent = _Enemies[i];
			if (!(agent == ignoreAgent) && agent.IsAlive)
			{
				float num2 = Mathfx.DistanceFromPointToVector(start, end, agent.Position);
				if (num2 < num)
				{
					num = num2;
					result = agent;
				}
			}
		}
		return result;
	}

	public Agent GetNearestAliveEnemy(AgentHuman agent, E_Direction direction, float maxRadius)
	{
		Vector3 vector;
		switch (direction)
		{
		case E_Direction.Forward:
			vector = agent.Forward;
			break;
		case E_Direction.Backward:
			vector = -agent.Forward;
			break;
		case E_Direction.Right:
			vector = agent.Right;
			break;
		case E_Direction.Left:
			vector = -agent.Right;
			break;
		default:
			return null;
		}
		return GetNearestAliveEnemy(agent.Position + vector, agent.Position + vector * 3f, maxRadius, agent);
	}

	public void SendFactToEnemies(AgentHuman owner, AgentHuman attacker, E_EventTypes eventType, float radius, float lifetime, bool toFriends)
	{
		float num = radius * radius;
		foreach (Agent enemy in Enemies)
		{
			AgentHuman agentHuman = enemy as AgentHuman;
			if ((bool)agentHuman)
			{
				if (!(owner == agentHuman) && (owner.Position - agentHuman.Position).sqrMagnitude < num)
				{
					agentHuman.AddFactToMemory(eventType, (!attacker) ? owner : attacker, lifetime, UnityEngine.Random.Range(0.3f, 0.6f));
				}
			}
			else if (eventType == E_EventTypes.EnemyFire && owner.IsPlayer && (owner.Position - enemy.transform.position).sqrMagnitude < num)
			{
				enemy.OnShootSound(owner);
			}
		}
	}

	public void KillAllEnemies(AgentHuman attacker)
	{
		foreach (Agent enemy in Enemies)
		{
			AgentHuman agentHuman = enemy as AgentHuman;
			if ((bool)agentHuman && agentHuman.IsAlive)
			{
				agentHuman.TakeDamage(attacker, agentHuman.BlackBoard.Health * 2f, null, new Vector3(0f, 0f, 0f), E_WeaponID.None, E_WeaponType.Unknown);
			}
		}
	}

	public void RegisterImportantObject(IImportantObject Obj)
	{
		_ImportantObjects.Add(Obj);
	}

	public void UnregisterImportantObject(IImportantObject Obj)
	{
		_ImportantObjects.Remove(Obj);
	}
}
