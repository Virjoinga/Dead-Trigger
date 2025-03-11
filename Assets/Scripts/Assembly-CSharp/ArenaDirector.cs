using System.Collections.Generic;
using UnityEngine;

public class ArenaDirector : MonoBehaviour
{
	private enum State
	{
		Init = 0,
		WarmupStart = 1,
		WarmupInProgress = 2,
		WaveStart = 3,
		WaveInProgress = 4,
		WaveFinished = 5,
		WaveFinishedDelay = 6,
		WaveFinishedDelay2 = 7,
		PauseBetweenWaves = 8,
		PauseBetweenWavesFinished = 9
	}

	private class States
	{
		private State m_State;

		public State actualState
		{
			get
			{
				return m_State;
			}
		}

		public void NextState()
		{
			switch (m_State)
			{
			case State.Init:
				m_State = State.WarmupStart;
				break;
			case State.WarmupStart:
				m_State = State.WarmupInProgress;
				break;
			case State.WarmupInProgress:
				m_State = State.WaveStart;
				break;
			case State.WaveStart:
				m_State = State.WaveInProgress;
				break;
			case State.WaveInProgress:
				m_State = State.WaveFinished;
				break;
			case State.WaveFinished:
				m_State = State.WaveFinishedDelay;
				break;
			case State.WaveFinishedDelay:
				m_State = State.WaveFinishedDelay2;
				break;
			case State.WaveFinishedDelay2:
				m_State = State.PauseBetweenWaves;
				break;
			case State.PauseBetweenWaves:
				m_State = State.PauseBetweenWavesFinished;
				break;
			case State.PauseBetweenWavesFinished:
				m_State = State.WaveStart;
				break;
			default:
				Debug.LogWarning("Unknown state: " + m_State);
				break;
			}
		}
	}

	private const int WARMUP_TIME = 4;

	private const int WAVE_FINISHED_DELAY = 1;

	private const int WAVE_FINISHED_DELAY2 = 4;

	private const int PAUSE_BETWEEN_WAVES = 15;

	private const float PER_RANK_SCORE_MODIFIER = 0.01f;

	private const float WAVE_COMPLETE_SCORE_BONUS = 100f;

	private const float ENEMY_KILLED_SCORE_BONUS = 5f;

	private const float ENEMY_BODY_DESTROYED_BONUS = 2f;

	private const float ENEMY_HEAD_DESTROYED_BONUS = 2f;

	private const float ENEMY_ARM_DESTROYED_BONUS = 1f;

	private const float ENEMY_LEG_DESTROYED_BONUS = 1f;

	private const float DIFFICULTY_SCORE_MODIFIER = 0.5f;

	private const int BASE_MONEY_BAGS_WAVE_REWARD = 5;

	private const int DIFFICULTY_MONEY_BAGS_BONUS = 1;

	public static ArenaDirector Instance;

	public int ENEMIES_IN_WAVE = 25;

	private SpawnSettings m_SpawnSettings;

	private SpawnManager.Director m_SpawnDirector;

	private int m_Wave;

	private int m_WaveSize;

	private int m_WaveKilledEnemies;

	private bool m_KilledCounterUpdated = true;

	private float m_Timer;

	private float m_TimerLimit = -1f;

	private int m_TimerTextId = -1;

	private float m_Score;

	private float m_ScoreRankModifier;

	private float m_ScoreDifficultyModifier = 0.5f;

	private bool m_WaveInProgressTimerComplete;

	private IngameBuy[] m_IngameBuyObjects;

	private int m_WaveMoneyBagsReward = 5;

	private States m_States = new States();

	public int GetWave()
	{
		return m_Wave;
	}

	public int GetScore()
	{
		return Mathf.CeilToInt(m_Score);
	}

	public void OnAgentHit(AgentHuman agent, E_BodyPart bodyPart, bool bodyPartRemoved)
	{
		if (bodyPartRemoved)
		{
			switch (bodyPart)
			{
			case E_BodyPart.Body:
				AddScore(ScoreModifier() * 2f);
				break;
			case E_BodyPart.Head:
				AddScore(ScoreModifier() * 2f);
				break;
			case E_BodyPart.LeftArm:
			case E_BodyPart.RightArm:
				AddScore(ScoreModifier() * 1f);
				break;
			case E_BodyPart.LeftLeg:
			case E_BodyPart.RightLeg:
				AddScore(ScoreModifier() * 1f);
				break;
			}
		}
	}

	public void OnEnemyDeath(AgentHuman Enemy)
	{
		m_WaveKilledEnemies++;
		AddScore(ScoreModifier() * 5f);
		m_KilledCounterUpdated = true;
	}

	private void OnDestroy()
	{
		if (m_SpawnDirector != null)
		{
			SpawnManager.DestroyInstance();
		}
	}

	private void Awake()
	{
		Instance = this;
		m_SpawnSettings = GetComponent<SpawnSettings>();
		if (!m_SpawnSettings)
		{
			m_SpawnSettings = GetComponentInChildren<SpawnSettings>();
		}
		if ((bool)m_SpawnSettings)
		{
			Debug.Log("Arena - Enabled GamePlay: " + m_SpawnSettings.name);
			m_SpawnSettings.CreateSpawnManager();
		}
		else
		{
			Debug.LogWarning("Arena - Can't enable any GamePlay hierarchy!!!");
		}
		m_IngameBuyObjects = GetComponentsInChildren<IngameBuy>();
		m_SpawnDirector = new SpawnDirector();
		m_TimerLimit = -1f;
	}

	private void Start()
	{
		m_Wave = 0;
	}

	private void Update()
	{
		SpawnManager.Instance.Update(Time.deltaTime);
		switch (m_States.actualState)
		{
		case State.Init:
			if ((bool)Player.Instance)
			{
				m_States.NextState();
				GameplayData.Instance.ResetStrongerWave();
			}
			break;
		case State.WarmupStart:
		{
			int num = 4;
			ShowText(3000300, Game.Instance.PlayerPersistentInfo.rank, false);
			ShowTextSmall(3000305, 0, false);
			StartTimer(num, -1);
			m_States.NextState();
			break;
		}
		case State.WarmupInProgress:
			if (IsTimerComplete())
			{
				m_States.NextState();
				ResetScore();
			}
			break;
		case State.WaveStart:
		{
			StartNewWave();
			StartTimer(4f, -1);
			m_WaveInProgressTimerComplete = false;
			m_States.NextState();
			IngameBuy[] ingameBuyObjects2 = m_IngameBuyObjects;
			foreach (IngameBuy ingameBuy2 in ingameBuyObjects2)
			{
				if (ingameBuy2.gameObject.activeInHierarchy)
				{
					ingameBuy2.WaveStart(m_Wave);
				}
			}
			break;
		}
		case State.WaveInProgress:
			if (IsTimerComplete() && !m_WaveInProgressTimerComplete)
			{
				m_WaveInProgressTimerComplete = true;
				GuiHUD.Instance.ShowCounter(true);
				GuiHUD.Instance.ShowArenaScore(true);
			}
			UpdateWave(Time.deltaTime);
			break;
		case State.WaveFinished:
		{
			StartTimer(1f, -1);
			m_States.NextState();
			IngameBuy[] ingameBuyObjects = m_IngameBuyObjects;
			foreach (IngameBuy ingameBuy in ingameBuyObjects)
			{
				if (ingameBuy.gameObject.activeInHierarchy)
				{
					ingameBuy.WaveFinished(m_Wave);
				}
			}
			break;
		}
		case State.WaveFinishedDelay:
			if (IsTimerComplete())
			{
				Game.Instance.PlayerPersistentInfo.AddMoneyBags(m_WaveMoneyBagsReward);
				AudioClip moneyPicked = Player.Instance.SoundSetup.MoneyPicked;
				Player.Instance.Owner.SoundUniquePlay(moneyPicked, moneyPicked.length * 0.1f, "Money");
				AddWaveReward();
				int num = 4;
				StartTimer(num, -1);
				m_States.NextState();
			}
			break;
		case State.WaveFinishedDelay2:
			if (IsTimerComplete())
			{
				int num = 15;
				StartTimer(num, 3000330);
				m_States.NextState();
			}
			break;
		case State.PauseBetweenWaves:
			if (IsTimerComplete())
			{
				StartTimer(1f, -1);
				m_States.NextState();
			}
			break;
		case State.PauseBetweenWavesFinished:
			if (IsTimerComplete())
			{
				m_States.NextState();
			}
			break;
		default:
			Debug.LogWarning("Unknown state: " + m_States.actualState);
			break;
		}
	}

	private void AddWaveReward()
	{
		int num = m_Wave % m_SpawnSettings.SpawnSettingsCount();
		int num2 = m_Wave / m_SpawnSettings.SpawnSettingsCount();
		if (num == 0 && m_Wave > 1)
		{
			if (num2 == 2)
			{
				int num3 = 3;
				ShowTextSmall(3000327, num3, false);
				Game.Instance.PlayerPersistentInfo.AddTicket(num3);
			}
			else
			{
				int argument = m_WaveMoneyBagsReward * GameplayData.Instance.MoneyPerZombie() * 4;
				ShowTextSmall(3000326, argument, false);
			}
		}
		else
		{
			int argument2 = m_WaveMoneyBagsReward * GameplayData.Instance.MoneyPerZombie();
			ShowTextSmall(3000325, argument2, false);
		}
	}

	private void StartNewWave()
	{
		int num = m_Wave % m_SpawnSettings.SpawnSettingsCount();
		m_Wave++;
		Game.Instance.MissionResultData.ArenaWaves = m_Wave;
		m_WaveSize = ENEMIES_IN_WAVE;
		m_WaveKilledEnemies = 0;
		SpawnManager.SpawnData spawnData = new SpawnManager.SpawnData();
		spawnData.m_EnemiesLimit = m_WaveSize;
		spawnData.m_Director = m_SpawnDirector;
		spawnData.m_Mods = new List<AgentHuman.ModData>();
		ShowText(3000310, m_Wave, true);
		if (num == 0)
		{
			if (m_Wave > 1)
			{
				ShowTextSmall(3000315, 0, true);
			}
			IncreaseWaveDifficulty();
		}
		m_SpawnSettings.FillAgentData(num, spawnData.m_Mods);
		m_SpawnSettings.SendSpawnDataToSpawnManager(num);
		(m_SpawnDirector as SpawnDirector).SetIntensity(SpawnDirector.GameplayIntensity.Normal);
		SpawnManager.Instance.OnEnemyDead += OnEnemyDeath;
		SpawnManager.Instance.StopSpawning();
		SpawnManager.Instance.StartSpawning(spawnData);
		GuiHUD.Instance.SetMissionType(E_MissionType.KillZombies);
		m_KilledCounterUpdated = true;
		UpdateWave(0f);
	}

	private void UpdateWave(float deltaTime)
	{
		if (m_KilledCounterUpdated)
		{
			int num = m_WaveSize - m_WaveKilledEnemies;
			GuiHUD.Instance.SetCounter(num);
			if (num <= 0)
			{
				SpawnManager.Instance.OnEnemyDead -= OnEnemyDeath;
				WaveFinished();
			}
		}
	}

	private void WaveFinished()
	{
		GuiHUD.Instance.ShowCounter(false);
		AddScore(ScoreModifier() * 100f);
		ShowText(3000320, m_Wave, true);
		m_States.NextState();
	}

	private void IncreaseWaveDifficulty()
	{
		GameplayData.Instance.SetStrongerWave();
		m_WaveMoneyBagsReward++;
		m_ScoreDifficultyModifier += 0.5f;
		m_ScoreRankModifier = (float)Game.Instance.PlayerPersistentInfo.rank * 0.5f * 0.01f;
	}

	private bool IsWaveComplete()
	{
		return m_WaveSize <= m_WaveKilledEnemies;
	}

	private void StartTimer(float seconds, int textId)
	{
		CancelInvoke("UpdateTimer");
		m_Timer = 0f;
		m_TimerLimit = seconds;
		m_TimerTextId = textId;
		InvokeRepeating("UpdateTimer", 1f, 1f);
		if (m_TimerTextId > 0)
		{
			ShowText(m_TimerTextId, Mathf.RoundToInt(m_TimerLimit), false);
		}
	}

	private void UpdateTimer()
	{
		m_Timer += 1f;
		float f = Mathf.Clamp(m_TimerLimit - m_Timer, 0f, 10000000f);
		if (m_TimerTextId > 0)
		{
			ShowText(m_TimerTextId, Mathf.RoundToInt(f), false);
		}
		if (m_Timer >= m_TimerLimit)
		{
			CancelInvoke("UpdateTimer");
			GuiHUD.Instance.ShowCounter(false);
		}
	}

	private bool IsTimerComplete()
	{
		return m_Timer >= m_TimerLimit;
	}

	private void ShowText(int messageId, int argument, bool dontDisappearText)
	{
		string text = TextDatabase.instance[messageId];
		text = text.Replace("%d", argument.ToString());
		GuiHUD.Instance.ShowMessage(GuiHUD.E_MessageType.Objective, text, dontDisappearText, (!dontDisappearText) ? 2f : 3f);
	}

	private void ShowTextSmall(int messageId, int argument, bool dontDisappearText)
	{
		string text = TextDatabase.instance[messageId];
		text = text.Replace("%d", argument.ToString());
		GuiHUD.Instance.ShowMessage(GuiHUD.E_MessageType.SecondObjective, text, dontDisappearText, (!dontDisappearText) ? 2f : 3f);
	}

	private float ScoreModifier()
	{
		return m_ScoreRankModifier + m_ScoreDifficultyModifier;
	}

	private void AddScore(float addValue)
	{
		m_Score += addValue;
		GuiHUD.Instance.SetArenaScore(GetScore());
		Game.Instance.MissionResultData.ArenaScore = Mathf.CeilToInt(m_Score);
	}

	private void ResetScore()
	{
		m_Score = 0f;
		GuiHUD.Instance.SetArenaScore(GetScore());
	}
}
