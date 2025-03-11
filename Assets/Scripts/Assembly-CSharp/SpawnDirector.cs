using System.Collections.Generic;
using UnityEngine;

public class SpawnDirector : SpawnManager.Director
{
	public enum GameplayIntensity
	{
		Normal = 0,
		Intensive = 1
	}

	private enum E_SpawnIntensity
	{
		Pause = 0,
		Low = 1,
		Normal = 2,
		High = 3,
		VeryHigh = 4
	}

	public interface SpawnObject
	{
		void Update(float deltaTime);

		bool CanSpawnEnemy(int CurrentlyLiveEnemies);

		bool IsFinished();

		void EnemySpawned();
	}

	private class Pause : SpawnObject
	{
		private float m_Timer;

		public void Init(float time)
		{
			m_Timer = time;
		}

		public void Update(float deltaTime)
		{
			m_Timer -= deltaTime;
		}

		public bool CanSpawnEnemy(int CurrentlyLiveEnemies)
		{
			return false;
		}

		public bool IsFinished()
		{
			return m_Timer <= 0f;
		}

		public void EnemySpawned()
		{
		}
	}

	private class Wave : SpawnObject
	{
		private int m_WaveSize;

		private float m_MinDelayBetweenSpawns;

		private float m_MaxDelayBetweenSpawns;

		private int m_LiveEnemiesLimit;

		private int m_SpawnedEnemies;

		private float m_BetweenSpawnsTimer;

		public void Init(int waveSize, int liveEnemiesLimit, float minDelayBetweenSpawns, float maxDelayBetweenSpawns)
		{
			m_WaveSize = waveSize;
			m_LiveEnemiesLimit = liveEnemiesLimit;
			m_MinDelayBetweenSpawns = minDelayBetweenSpawns;
			m_MaxDelayBetweenSpawns = maxDelayBetweenSpawns;
			m_SpawnedEnemies = 0;
			m_BetweenSpawnsTimer = 0f;
		}

		public void Update(float deltaTime)
		{
			if (m_BetweenSpawnsTimer > 0f)
			{
				m_BetweenSpawnsTimer -= deltaTime;
			}
		}

		public bool CanSpawnEnemy(int CurrentlyLiveEnemies)
		{
			if (m_SpawnedEnemies >= m_WaveSize)
			{
				return false;
			}
			if (CurrentlyLiveEnemies >= m_LiveEnemiesLimit)
			{
				return false;
			}
			if (m_BetweenSpawnsTimer <= 0f)
			{
				m_BetweenSpawnsTimer = Random.Range(m_MinDelayBetweenSpawns, m_MaxDelayBetweenSpawns);
				return true;
			}
			return false;
		}

		public void EnemySpawned()
		{
			m_SpawnedEnemies++;
		}

		public bool IsFinished()
		{
			return m_SpawnedEnemies >= m_WaveSize;
		}
	}

	private const int WAVE_SIZE = 6;

	private const float SPAWN_PAUSE = 1f;

	private const float CI_ENEMY_INJURY = 0.01f;

	private const float CI_PLAYER_INJURY = 0.3f;

	private const float CI_ENEMY_INRANGE = 0.05f;

	private const float CI_ENEMY_MAX_RANGE = 15f;

	private const float CI_DESTROY_OBJECT_IN_RANGE = 0.1f;

	private const float CI_ENEMY_DEATH = 0.1f;

	private const float CI_DECREASE_IN_TIME = 0.11f;

	private int m_NumberOfWaves;

	private int m_EnemiesAliveCounter;

	private float m_TimeWithoutLowIntensityOrPause;

	private float m_CombatIntesity;

	private float m_OptimalCombatIntensity;

	private SpawnManager.E_SpawnPointSelection m_SelectionMethod;

	private SpawnObject m_SpawnObject;

	private List<float> m_CombatHistory = new List<float>();

	private int m_CombatHistoryMaxValue = 100;

	private static float UpdateTimer;

	private static float lastInjury;

	public SpawnDirector()
	{
		m_EnemiesAliveCounter = 0;
		m_NumberOfWaves = 0;
		m_SelectionMethod = SpawnManager.E_SpawnPointSelection.Randomized;
		m_SpawnObject = null;
		m_CombatIntesity = 0f;
	}

	public void SetIntensity(GameplayIntensity gameplayIntensity)
	{
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		int rankCount = GameplayData.Instance.playerLevelData.GetRankCount();
		m_OptimalCombatIntensity = 0.4f;
		m_OptimalCombatIntensity += (float)(rank / rankCount) * 0.3f;
		if (gameplayIntensity == GameplayIntensity.Intensive)
		{
			m_OptimalCombatIntensity += 0.1f;
		}
	}

	public void Init()
	{
		UpdateTimer = 0f;
	}

	public void UpdateCombatIntensity()
	{
		m_CombatIntesity -= 0.11f;
		foreach (Agent enemy in Mission.Instance.CurrentGameZone.Enemies)
		{
			AgentHuman agentHuman = enemy as AgentHuman;
			if (!(agentHuman == null))
			{
				if (agentHuman.WorldState.GetWSProperty(E_PropKey.DestroyObject).GetBool())
				{
					m_CombatIntesity += (1f - Mathf.Clamp(agentHuman.BlackBoard.DistanceToTarget, 0f, 15f) / 15f) * 0.1f;
				}
				else
				{
					m_CombatIntesity += (1f - Mathf.Clamp(agentHuman.BlackBoard.DistanceToTarget, 0f, 15f) / 15f) * 0.05f;
				}
			}
		}
		m_CombatIntesity = Mathf.Clamp(m_CombatIntesity, 0f, 1f);
		if (m_CombatHistory.Count == m_CombatHistoryMaxValue)
		{
			m_CombatHistory.RemoveAt(0);
		}
		m_CombatHistory.Add(m_CombatIntesity);
	}

	public void Update(float deltaTime)
	{
		if (m_SpawnObject != null)
		{
			m_SpawnObject.Update(deltaTime);
		}
		UpdateTimer -= TimeManager.Instance.GetRealDeltaTime();
		if (!(UpdateTimer > 0f))
		{
			UpdateCombatIntensity();
			if (m_SpawnObject == null || m_SpawnObject.IsFinished())
			{
				ConstructNextWave();
			}
			UpdateTimer = 1f;
		}
	}

	private int GetMaxLiveEnemiesBasedOnRank(E_SpawnIntensity intensity)
	{
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			return 6;
		}
		int num;
		switch (intensity)
		{
		case E_SpawnIntensity.VeryHigh:
			num = 6;
			break;
		case E_SpawnIntensity.High:
			num = 6;
			break;
		case E_SpawnIntensity.Normal:
			num = 5;
			break;
		case E_SpawnIntensity.Low:
			num = 4;
			break;
		default:
			num = 3;
			break;
		}
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		if (rank < 3)
		{
			Mathf.Clamp(num, 3, 4);
		}
		else if (rank < 5)
		{
			Mathf.Clamp(num, 3, 5);
		}
		return num;
	}

	private float MinTimeBetweenSpawnsBasedOnRank()
	{
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			return 0f;
		}
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		int rankCount = GameplayData.Instance.playerLevelData.GetRankCount();
		return 1f * (float)rank / (float)rankCount;
	}

	private float MaxTimeBetweenSpawnsBasedOnRank()
	{
		if (Game.Instance.GameplayType == GameplayType.Arena)
		{
			return 0f;
		}
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		int rankCount = GameplayData.Instance.playerLevelData.GetRankCount();
		return 0.5f * (float)rank / (float)rankCount;
	}

	private E_SpawnIntensity ComputeNextSpawnIntesity(float intensityFast, float intensitySlow)
	{
		if (intensityFast == -1f)
		{
			return E_SpawnIntensity.Normal;
		}
		if (intensitySlow == -1f)
		{
			if (m_OptimalCombatIntensity > intensityFast)
			{
				return E_SpawnIntensity.High;
			}
			return E_SpawnIntensity.Normal;
		}
		if (intensityFast == 1f)
		{
			return E_SpawnIntensity.Pause;
		}
		if (m_OptimalCombatIntensity > intensityFast && m_OptimalCombatIntensity > intensitySlow)
		{
			if (intensityFast / m_OptimalCombatIntensity < 0.6f && intensitySlow / m_OptimalCombatIntensity < 0.6f)
			{
				return E_SpawnIntensity.VeryHigh;
			}
			return E_SpawnIntensity.High;
		}
		if (m_OptimalCombatIntensity < intensityFast && m_OptimalCombatIntensity > intensitySlow)
		{
			if (intensitySlow / m_OptimalCombatIntensity < 0.6f)
			{
				return E_SpawnIntensity.High;
			}
			return E_SpawnIntensity.Normal;
		}
		if (m_OptimalCombatIntensity > intensityFast && m_OptimalCombatIntensity < intensitySlow)
		{
			return E_SpawnIntensity.Low;
		}
		if (m_OptimalCombatIntensity < intensityFast && m_OptimalCombatIntensity < intensitySlow)
		{
			return E_SpawnIntensity.Pause;
		}
		return E_SpawnIntensity.Normal;
	}

	private void ConstructNextWave()
	{
		float num = MinTimeBetweenSpawnsBasedOnRank();
		float num2 = MaxTimeBetweenSpawnsBasedOnRank();
		Wave wave = null;
		Pause pause = null;
		float intensityFast = CalculateMAIntensity(5);
		float intensitySlow = CalculateMAIntensity(20);
		E_SpawnIntensity e_SpawnIntensity = ComputeNextSpawnIntesity(intensityFast, intensitySlow);
		switch (e_SpawnIntensity)
		{
		case E_SpawnIntensity.Pause:
			pause = new Pause();
			pause.Init(1f);
			break;
		case E_SpawnIntensity.Low:
			wave = new Wave();
			wave.Init(5, GetMaxLiveEnemiesBasedOnRank(e_SpawnIntensity), 2f + num, 2.5f + num2);
			break;
		case E_SpawnIntensity.Normal:
			wave = new Wave();
			wave.Init(6, GetMaxLiveEnemiesBasedOnRank(e_SpawnIntensity), 1.5f + num, 2f + num2);
			break;
		case E_SpawnIntensity.High:
			wave = new Wave();
			wave.Init(6, GetMaxLiveEnemiesBasedOnRank(e_SpawnIntensity), 1f + num, 1.5f + num2);
			break;
		case E_SpawnIntensity.VeryHigh:
			wave = new Wave();
			wave.Init(6, GetMaxLiveEnemiesBasedOnRank(e_SpawnIntensity), 0f + num, 0f + num2);
			break;
		}
		if (wave != null)
		{
			m_SpawnObject = wave;
		}
		else
		{
			m_SpawnObject = pause;
		}
		m_NumberOfWaves++;
	}

	public bool IsEnemySpawnAllowed()
	{
		if (m_SpawnObject != null)
		{
			return m_SpawnObject.CanSpawnEnemy(m_EnemiesAliveCounter);
		}
		return false;
	}

	public void OnEnemySpawn(AgentHuman Enemy)
	{
		m_EnemiesAliveCounter++;
		if (m_SpawnObject != null)
		{
			m_SpawnObject.EnemySpawned();
		}
	}

	public void OnEnemyInjured(AgentHuman Enemy, float damage)
	{
		if (lastInjury != Time.timeSinceLevelLoad)
		{
			lastInjury = Time.timeSinceLevelLoad;
			m_CombatIntesity += 0.01f;
		}
	}

	public void OnEnemyDeath(AgentHuman Enemy)
	{
		m_CombatIntesity += 0.1f;
		m_EnemiesAliveCounter--;
	}

	public void OnEnemyDespawn(AgentHuman Enemy)
	{
	}

	public void OnPlayerHealthChange(float Change)
	{
		m_CombatIntesity += Mathf.Abs(Change) / Player.Instance.Owner.BlackBoard.RealMaxHealth * 0.3f;
	}

	public SpawnManager.E_SpawnPointSelection GetSpawnPointSelection()
	{
		return m_SelectionMethod;
	}

	private float CalculateMAIntensity(int maxValues)
	{
		if (m_CombatHistory.Count < maxValues)
		{
			return -1f;
		}
		float num = 0f;
		for (int num2 = maxValues; num2 > 0; num2--)
		{
			num += m_CombatHistory[m_CombatHistory.Count - num2];
		}
		return num / (float)maxValues;
	}
}
