using System.Collections.Generic;
using UnityEngine;

[NESEvent(new string[] { "Success", "CloseToSuccess" })]
public class GameFlowObjective : MonoBehaviour, IGameZoneControledObject
{
	public enum ObjectiveType
	{
		KillAllZombies = 0,
		Timer = 1,
		CarryObject = 2,
		ProtectObjects = 3,
		Script = 4,
		KillBoss = 5
	}

	public enum CounterState
	{
		Show = 0,
		Hide = 1,
		Ignore = 2
	}

	private class Destructible
	{
		public DestructibleObject obj;

		public float health;
	}

	public ObjectiveType m_Objective;

	public float m_TimerSeconds;

	public int m_KilledZombies;

	public int m_KilledZombiesRandomAdd;

	public bool m_SpawnExactNumberOfZombies;

	public int m_KilledBosses;

	public CounterState m_CounterState;

	public int m_ObjectiveText;

	public int m_ObjectiveText2;

	public int m_ObjectiveText3;

	public bool m_DontDisappearText;

	public AudioClip m_WarningSound;

	private int KilledZombiesStart;

	private int KilledZombiesCounter;

	private int KilledZombiesLimit;

	private int KilledBossesCounter;

	private int KilledBossesLimit;

	private bool KilledBossesCounterUpdated;

	private float Timer;

	private float TimerLimit;

	private bool CloseToSuccessTriggered;

	private int CarryDLObjectsCounter;

	private int CarryDLObjectsLimit;

	private CarryDLObject PreviousDLObject;

	private int CurrentDLObjectIndex;

	private List<CarryDLObject> m_CarryDLObjects = new List<CarryDLObject>();

	private List<Destructible> m_DestructibleObjects = new List<Destructible>();

	private NESController m_NESController;

	[NESAction]
	public void Activate()
	{
		Reset();
		switch (m_Objective)
		{
		case ObjectiveType.Timer:
			m_DontDisappearText = true;
			StartTimer();
			ShowObjective(m_ObjectiveText);
			if (m_CounterState == CounterState.Show)
			{
				GuiHUD.Instance.SetMissionType(E_MissionType.TimeDefense);
			}
			break;
		case ObjectiveType.KillAllZombies:
			m_DontDisappearText = true;
			StartKillZombies();
			ShowObjective(m_ObjectiveText);
			if (m_CounterState == CounterState.Show)
			{
				GuiHUD.Instance.SetMissionType(E_MissionType.KillZombies);
			}
			break;
		case ObjectiveType.KillBoss:
			m_DontDisappearText = true;
			StartKillBoss();
			ShowObjective(m_ObjectiveText);
			if (m_CounterState == CounterState.Show)
			{
				GuiHUD.Instance.SetMissionType(E_MissionType.KillZombies);
			}
			break;
		case ObjectiveType.CarryObject:
			m_DontDisappearText = true;
			StartCarryObjects();
			if (m_CounterState == CounterState.Show)
			{
				GuiHUD.Instance.SetMissionType(E_MissionType.CarryResources);
			}
			break;
		case ObjectiveType.ProtectObjects:
			m_DontDisappearText = true;
			StartProtectObjects();
			if (m_CounterState == CounterState.Show)
			{
				GuiHUD.Instance.SetMissionType(E_MissionType.ProtectObjects);
			}
			break;
		case ObjectiveType.Script:
			ShowObjective(m_ObjectiveText);
			break;
		}
		GameFlowSupport firstComponentUpward = GameObjectUtils.GetFirstComponentUpward<GameFlowSupport>(base.gameObject);
		if ((bool)firstComponentUpward)
		{
			firstComponentUpward.SetEnemySpawnCount(m_SpawnExactNumberOfZombies ? KilledZombiesLimit : 0);
		}
		else
		{
			Debug.LogWarning("Can't find GameFlowSupport parent object");
		}
		if (m_CounterState == CounterState.Show)
		{
			ShowCounter(true);
		}
		else if (m_CounterState == CounterState.Hide)
		{
			ShowCounter(false);
		}
	}

	[NESAction]
	public void ForceSuccess()
	{
		if (!CloseToSuccessTriggered && (bool)m_NESController)
		{
			m_NESController.SendGameEvent(this, "CloseToSuccess");
		}
		if ((bool)m_NESController)
		{
			m_NESController.SendGameEvent(this, "Success");
		}
		if (m_Objective == ObjectiveType.ProtectObjects)
		{
			foreach (Destructible destructibleObject in m_DestructibleObjects)
			{
				GuiHUD.Instance.SetObjectHP(destructibleObject.obj.gameObject, destructibleObject.obj.HealthCoef, false);
				GuiHUD.Instance.HighlightRadarObject(destructibleObject.obj.gameObject, false);
				GuiHUD.Instance.UnregisterRadarObject(destructibleObject.obj.gameObject, GuiHUD.E_RadarObjectType.ProtectObject);
				GuiHUD.Instance.UnregisterObjectHP(destructibleObject.obj.gameObject);
			}
		}
		Reset();
	}

	[NESAction]
	public void Cancel()
	{
		Reset();
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public void Reset()
	{
		if ((bool)base.GetComponent<AudioSource>() && base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Stop();
		}
		CancelInvoke();
		KilledZombiesLimit = -1;
		TimerLimit = -1f;
		CarryDLObjectsCounter = -1;
		CurrentDLObjectIndex = 0;
		CloseToSuccessTriggered = false;
	}

	private void Awake()
	{
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (m_NESController == null)
		{
		}
		if (m_Objective == ObjectiveType.ProtectObjects)
		{
			if (base.GetComponent<AudioSource>() == null)
			{
				base.gameObject.AddComponent<AudioSource>();
			}
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().playOnAwake = false;
			base.GetComponent<AudioSource>().clip = m_WarningSound;
			base.GetComponent<AudioSource>().volume = 0.33f;
		}
		Reset();
	}

	private void Start()
	{
		if (m_Objective == ObjectiveType.CarryObject)
		{
			CarryDLObject[] componentsInChildren = base.gameObject.transform.parent.GetComponentsInChildren<CarryDLObject>();
			Debug.Log("Objects to carry: " + componentsInChildren.Length);
			m_CarryDLObjects.Capacity = componentsInChildren.Length;
			CarryDLObjectsLimit = 0;
			CarryDLObject[] array = componentsInChildren;
			foreach (CarryDLObject carryDLObject in array)
			{
				m_CarryDLObjects.Add(carryDLObject);
				CarryDLObjectsLimit += carryDLObject.UsableInstances();
				carryDLObject.Deactivate();
			}
			m_CarryDLObjects.Shuffle();
		}
		else
		{
			if (m_Objective != ObjectiveType.ProtectObjects)
			{
				return;
			}
			DestructibleObject[] componentsInChildren2 = base.gameObject.transform.parent.GetComponentsInChildren<DestructibleObject>();
			m_DestructibleObjects.Capacity = componentsInChildren2.Length;
			DestructibleObject[] array2 = componentsInChildren2;
			foreach (DestructibleObject destructibleObject in array2)
			{
				if (destructibleObject.m_IsImportantObject)
				{
					Destructible destructible = new Destructible();
					destructible.health = -1f;
					destructible.obj = destructibleObject;
					m_DestructibleObjects.Add(destructible);
					destructibleObject.Disable();
					destructibleObject.Enable();
				}
			}
			Debug.Log("Objects to protect: " + m_DestructibleObjects.Count);
		}
	}

	private void UpdateTimer()
	{
		Timer += 1f;
		float num = Mathf.Clamp(TimerLimit - Timer, 0f, 10000000f);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounterTime(num);
		}
		if (!CloseToSuccessTriggered && num <= (float)(SpawnManager.Instance.EnemiesNum * 2 + 3))
		{
			CloseToSuccessTriggered = true;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "CloseToSuccess");
			}
		}
		if (Timer >= TimerLimit)
		{
			TimerLimit = -1f;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "Success");
			}
			CancelInvoke("UpdateTimer");
		}
	}

	private void StartTimer()
	{
		CancelInvoke("UpdateTimer");
		Timer = 0f;
		TimerLimit = m_TimerSeconds;
		InvokeRepeating("UpdateTimer", 3f, 1f);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounterTime(Mathf.Clamp(TimerLimit - Timer, 0f, 10000000f));
		}
	}

	private int NumOfLiveEnemies()
	{
		int num = 0;
		foreach (Agent enemy in Mission.Instance.CurrentGameZone.Enemies)
		{
			if (enemy.IsAlive)
			{
				num++;
			}
		}
		return num;
	}

	private void StartKillZombies()
	{
		CancelInvoke("UpdateKilledZombies");
		KilledZombiesCounter = 0;
		int killedZombies = m_KilledZombies;
		int max = m_KilledZombies + m_KilledZombiesRandomAdd;
		KilledZombiesStart = Game.Instance.MissionResultData.KilledZombies;
		KilledZombiesLimit = Random.Range(killedZombies, max);
		InvokeRepeating("UpdateKilledZombies", 0.2f, 0.2f);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounter(KilledZombiesLimit);
		}
	}

	private void UpdateKilledZombies()
	{
		if (m_Objective != 0 || Game.Instance.MissionResultData.KilledZombies == KilledZombiesCounter + KilledZombiesStart)
		{
			return;
		}
		KilledZombiesCounter = Game.Instance.MissionResultData.KilledZombies - KilledZombiesStart;
		int num = Mathf.Clamp(KilledZombiesLimit - KilledZombiesCounter, 0, 10000000);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounter(num);
		}
		if (!CloseToSuccessTriggered && num <= 6)
		{
			CloseToSuccessTriggered = true;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "CloseToSuccess");
			}
		}
		if (num <= 0)
		{
			KilledZombiesLimit = -1;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "Success");
			}
			CancelInvoke("UpdateKilledZombies");
		}
	}

	private void StartKillBoss()
	{
		CancelInvoke("UpdateKilledBosses");
		SpawnManager.Instance.OnEnemyDead += OnEnemyDeath;
		KilledBossesCounter = 0;
		KilledBossesCounterUpdated = true;
		KilledBossesLimit = m_KilledBosses;
		InvokeRepeating("UpdateKilledBosses", 0.2f, 0.2f);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounter(KilledBossesLimit);
		}
	}

	public void OnEnemyDeath(AgentHuman Enemy)
	{
		if (Enemy.AgentType == E_AgentType.Boss1 && m_Objective == ObjectiveType.KillBoss)
		{
			KilledBossesCounter++;
			KilledBossesCounterUpdated = true;
		}
	}

	private void UpdateKilledBosses()
	{
		if (!KilledBossesCounterUpdated)
		{
			return;
		}
		KilledBossesCounterUpdated = false;
		int num = Mathf.Clamp(KilledBossesLimit - KilledBossesCounter, 0, 10000000);
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounter(num);
		}
		if (!CloseToSuccessTriggered && num == 1)
		{
			CloseToSuccessTriggered = true;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "CloseToSuccess");
			}
		}
		if (num <= 0)
		{
			KilledBossesLimit = -1;
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "Success");
			}
			CancelInvoke("UpdateKilledBosses");
			SpawnManager.Instance.OnEnemyDead -= OnEnemyDeath;
		}
	}

	private void UpdateProtectedObjects()
	{
		bool flag = false;
		float num = 0f;
		for (int i = 0; i < m_DestructibleObjects.Count; i++)
		{
			Destructible destructible = m_DestructibleObjects[i];
			num += destructible.obj.HealthCoef;
			if (Mathf.Approximately(destructible.health, destructible.obj.HealthCoef))
			{
				continue;
			}
			bool flag2 = destructible.obj.HealthCoef <= 0.51f;
			destructible.health = destructible.obj.HealthCoef;
			GuiHUD.Instance.SetObjectHP(destructible.obj.gameObject, destructible.obj.HealthCoef, flag2);
			GuiHUD.Instance.HighlightRadarObject(destructible.obj.gameObject, flag2);
			if (flag2)
			{
				flag = true;
			}
			if (destructible.obj.HealthCoef <= 0.001f)
			{
				Mission.Instance.EndOfMission(Mission.E_MissionResult.DoorDestroyed);
				if ((bool)base.GetComponent<AudioSource>())
				{
					base.GetComponent<AudioSource>().Stop();
				}
				CancelInvoke("UpdateProtectedObjects");
			}
		}
		if (m_DestructibleObjects.Count > 0)
		{
			num /= (float)m_DestructibleObjects.Count;
		}
		Game.Instance.MissionResultData.AverageProtectObjectsHP = num;
		if (flag && !base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Play();
		}
		else if (!flag && base.GetComponent<AudioSource>().isPlaying)
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	private void HideTutorial()
	{
		GuiHUD.Instance.ShowTutorial(5, false);
		Invoke("ShowTutorial2", 10f);
	}

	private void HideTutorial2()
	{
		Game.Instance.PlayerPersistentInfo.ProtectObjectsTutorial = true;
		GuiHUD.Instance.ShowTutorial(6, false);
		Invoke("HideTutorial2", 10f);
	}

	private void ShowTutorial2()
	{
		GuiHUD.Instance.ShowTutorial(6, true);
		Invoke("HideTutorial2", 10f);
	}

	private void StartProtectObjects()
	{
		if (!Game.Instance.PlayerPersistentInfo.ProtectObjectsTutorial)
		{
			GuiHUD.Instance.ShowTutorial(5, true);
			Invoke("HideTutorial", 10f);
		}
		CancelInvoke("UpdateProtectedObjects");
		foreach (Destructible destructibleObject in m_DestructibleObjects)
		{
			GuiHUD.Instance.RegisterRadarObject(destructibleObject.obj.gameObject, GuiHUD.E_RadarObjectType.ProtectObject);
			GuiHUD.Instance.RegisterObjectHP(destructibleObject.obj.gameObject);
		}
		InvokeRepeating("UpdateProtectedObjects", 0.2f, 0.2f);
	}

	private void StartCarryObjects()
	{
		CarryDLObjectsCounter = -1;
		CurrentDLObjectIndex = 0;
		PreviousDLObject = null;
		NextObjectToCarry();
	}

	private CarryDLObject PreviousCarryObject()
	{
		return PreviousDLObject;
	}

	private CarryDLObject CurrentCarryObject()
	{
		int num = CurrentDLObjectIndex;
		if (m_CarryDLObjects.Count <= num)
		{
			num %= m_CarryDLObjects.Count;
		}
		return m_CarryDLObjects[num];
	}

	private void NextObjectToCarry()
	{
		PreviousDLObject = CurrentCarryObject();
		if (CarryDLObjectsCounter >= 0)
		{
			GuiHUD.Instance.UnregisterRadarObject(CurrentCarryObject().Target(), GuiHUD.E_RadarObjectType.CarryObjectTarget);
		}
		CarryDLObjectsCounter++;
		if (CurrentCarryObject().UsableInstances() == 0)
		{
			CurrentDLObjectIndex++;
		}
		if (m_CounterState != CounterState.Ignore)
		{
			GuiHUD.Instance.SetCounter(CarryDLObjectsLimit - CarryDLObjectsCounter);
		}
		if (CarryDLObjectsCounter >= CarryDLObjectsLimit)
		{
			CarryDLObjectsCounter = -1;
			if (!CloseToSuccessTriggered)
			{
				CloseToSuccessTriggered = true;
				if ((bool)m_NESController)
				{
					m_NESController.SendGameEvent(this, "CloseToSuccess");
				}
			}
			if ((bool)m_NESController)
			{
				m_NESController.SendGameEvent(this, "Success");
			}
		}
		else
		{
			CarryDLObject carryDLObject = CurrentCarryObject();
			carryDLObject.Activate(PickUpObject, PutDownObject);
			GuiHUD.Instance.RegisterRadarObject(carryDLObject.Source(), GuiHUD.E_RadarObjectType.CarryObjectSource);
			if (CarryDLObjectsCounter == 0)
			{
				ShowObjective(m_ObjectiveText);
			}
			else if (CarryDLObjectsCounter == 1)
			{
				ShowObjective(m_ObjectiveText3);
			}
		}
	}

	private void PickUpObject(CarryDLObject obj)
	{
		CarryDLObject carryDLObject = PreviousCarryObject();
		CarryDLObject carryDLObject2 = CurrentCarryObject();
		if (carryDLObject != null && carryDLObject != carryDLObject2)
		{
			carryDLObject.Deactivate();
		}
		GuiHUD.Instance.ShowCarryObjectIcon(true);
		GuiHUD.Instance.UnregisterRadarObject(carryDLObject2.Source(), GuiHUD.E_RadarObjectType.CarryObjectSource);
		GuiHUD.Instance.RegisterRadarObject(carryDLObject2.Target(), GuiHUD.E_RadarObjectType.CarryObjectTarget);
		if (CarryDLObjectsCounter == 0)
		{
			ShowObjective(m_ObjectiveText2);
		}
	}

	private void PutDownObject(CarryDLObject obj)
	{
		GuiHUD.Instance.ShowCarryObjectIcon(false);
		NextObjectToCarry();
	}

	private void ShowObjective(int messageId)
	{
		if (m_ObjectiveText > 0)
		{
			string text = TextDatabase.instance[messageId];
			if (m_Objective == ObjectiveType.KillAllZombies)
			{
				text = text.Replace("%d", KilledZombiesLimit.ToString());
			}
			else if (m_Objective == ObjectiveType.Timer)
			{
				text = text.Replace("%f", Mathf.Round(TimerLimit).ToString());
			}
			else if (m_Objective == ObjectiveType.CarryObject)
			{
				text = text.Replace("%d", (CarryDLObjectsLimit - CarryDLObjectsCounter).ToString());
			}
			GuiHUD.Instance.ShowMessage(GuiHUD.E_MessageType.Objective, text, m_DontDisappearText, 0f);
		}
	}

	private void ShowCounter(bool show)
	{
		GuiHUD.Instance.ShowCounter(show);
	}

	private void OnDrawGizmos()
	{
		switch (m_Objective)
		{
		case ObjectiveType.Timer:
			Gizmos.DrawIcon(base.transform.position, "GameObjectiveTimer");
			break;
		case ObjectiveType.KillAllZombies:
			Gizmos.DrawIcon(base.transform.position, "GameObjectiveKillAllZombies");
			break;
		case ObjectiveType.Script:
			Gizmos.DrawIcon(base.transform.position, "GameObjectiveScript");
			break;
		case ObjectiveType.CarryObject:
			Gizmos.DrawIcon(base.transform.position, "GameObjectiveCarryObject");
			break;
		case ObjectiveType.ProtectObjects:
			Gizmos.DrawIcon(base.transform.position, "GameObjectiveProtectObject");
			break;
		}
	}
}
