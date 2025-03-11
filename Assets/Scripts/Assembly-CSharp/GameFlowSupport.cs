using System.Collections.Generic;
using UnityEngine;

[NESEvent(new string[] { "TutorialBandageUsed" })]
public class GameFlowSupport : MonoBehaviour
{
	public enum GameplayType
	{
		Static = 0,
		Run = 1
	}

	public E_MissionType MissionType;

	private NESController m_NESController;

	private bool m_BandagesUsed;

	private bool m_CheckBandages;

	private int m_BandagesCount = -1;

	private GameObject m_ActualGameplay;

	private SpawnSettings m_SpawnSettings;

	private SpawnManager.Director m_SpawnDirector;

	private int m_EnemySpawnCount;

	private static string[] s_GameflowTypes = new string[2] { "static", "run" };

	private static string[] s_SpawnTypes = new string[4] { "weak", "normal", "intensive", "static defense" };

	private static string[] s_TutorialTypes = new string[5] { "tutorial1", "tutorial2", "tutorial3", "tutorial4", "tutorial5" };

	private void Awake()
	{
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (m_NESController == null)
		{
		}
		if (Game.Instance.MissionType == MissionType)
		{
			m_ActualGameplay = null;
			if (Game.Instance.MissionSubtype != string.Empty)
			{
				foreach (Transform item in base.transform)
				{
					if (item.gameObject.name == Game.Instance.MissionSubtype)
					{
						item.gameObject._SetActiveRecursively(true);
						m_ActualGameplay = item.gameObject;
					}
					else
					{
						item.gameObject._SetActiveRecursively(false);
					}
				}
			}
			if (m_ActualGameplay == null)
			{
				List<string> list = new List<string>();
				foreach (Transform item2 in base.transform)
				{
					if (!item2.gameObject.name.Contains("Story") && !item2.gameObject.name.Contains("Heli"))
					{
						list.Add(item2.gameObject.name);
					}
				}
				int num = Random.Range(0, list.Count);
				if (num >= 0 && list.Count > 0)
				{
					foreach (Transform item3 in base.transform)
					{
						if (item3.gameObject.name == list[num])
						{
							item3.gameObject._SetActiveRecursively(true);
							m_ActualGameplay = item3.gameObject;
						}
						else
						{
							item3.gameObject._SetActiveRecursively(false);
						}
					}
				}
			}
			if ((bool)m_ActualGameplay)
			{
				Debug.Log("Enabled GamePlay: " + m_ActualGameplay.name);
			}
			else
			{
				Debug.LogWarning("Can't enable any GamePlay hierarchy!!!");
			}
		}
		else
		{
			base.gameObject._SetActiveRecursively(false);
		}
		Game.Instance.EnemyAutoSpawn = false;
	}

	public void SetEnemySpawnCount(int count)
	{
		m_EnemySpawnCount = count;
	}

	private void Start()
	{
		if (Game.Instance.PlayerPersistentInfo.storyId == 1)
		{
			Game.Instance.CanHeal = false;
		}
		m_SpawnDirector = new SpawnDirector();
		m_SpawnSettings = m_ActualGameplay.GetComponent<SpawnSettings>();
		if (!m_SpawnSettings)
		{
			m_SpawnSettings = base.gameObject.GetFirstComponentUpward<SpawnSettings>();
		}
		if (!m_SpawnSettings)
		{
			SpawnSettings.CreateSpawnManagerWithDefaultData();
			return;
		}
		m_SpawnSettings.CreateSpawnManager();
		m_SpawnSettings.SendSpawnDataToSpawnManager(0);
	}

	[NESAction(Argument1 = "GetAvailableNames")]
	public void SetGameplayType(string type)
	{
		if (type == s_GameflowTypes[0])
		{
			PickupManager.Instance.CollectAutomatically = true;
		}
		else if (type == s_GameflowTypes[1])
		{
			PickupManager.Instance.CollectAutomatically = false;
		}
	}

	[NESAction]
	public void RegisterRadarObject(GameObject obj)
	{
		GuiHUD.Instance.RegisterRadarObject(obj, GuiHUD.E_RadarObjectType.Target);
	}

	[NESAction]
	public void UnregisterRadarObject(GameObject obj)
	{
		GuiHUD.Instance.UnregisterRadarObject(obj, GuiHUD.E_RadarObjectType.Target);
	}

	[NESAction(Argument1 = "GetTutorialTypes")]
	public void ShowTutorial(string type)
	{
		Player.Instance.StopMove(false);
		Player.Instance.StopView(false);
		int index = 0;
		if (type == s_TutorialTypes[1])
		{
			index = 1;
		}
		else if (type == s_TutorialTypes[2])
		{
			index = 2;
		}
		else if (type == s_TutorialTypes[3])
		{
			index = 3;
		}
		else if (type == s_TutorialTypes[4])
		{
			index = 4;
			Player.Instance.StopMove(true);
			Player.Instance.StopView(true);
		}
		GuiHUD.Instance.ShowTutorial(index, true);
	}

	[NESAction]
	public void DisableHudControlsTutorial(bool disable)
	{
		GuiHUD.Instance.EnableControlsTutorial(!disable);
	}

	[NESAction]
	public void HideTutorial()
	{
		Player.Instance.StopMove(false);
		Player.Instance.StopView(false);
		GuiHUD.Instance.HideTutorials();
	}

	[NESAction]
	public void StopSpawning()
	{
		SpawnManager.Instance.StopSpawning();
	}

	[NESAction(Argument1 = "GetSpawnTypes")]
	public void StartSpawning(string type)
	{
		SpawnManager.SpawnData spawnData = new SpawnManager.SpawnData();
		spawnData.m_EnemiesLimit = m_EnemySpawnCount;
		spawnData.m_Director = m_SpawnDirector;
		(m_SpawnDirector as SpawnDirector).SetIntensity((!(type == s_SpawnTypes[0]) && !(type == s_SpawnTypes[1])) ? SpawnDirector.GameplayIntensity.Intensive : SpawnDirector.GameplayIntensity.Normal);
		SpawnManager.Instance.StopSpawning();
		SpawnManager.Instance.StartSpawning(spawnData);
	}

	[NESAction]
	public void SpawnBoss(GameObject obj)
	{
		if (!(obj == null))
		{
			SpawnPointEnemy component = obj.GetComponent<SpawnPointEnemy>();
			EnemySettings.Info enemyInfo = GameplayData.Instance.enemySettings.GetEnemyInfo(E_AgentType.Boss1);
			if (enemyInfo.sincePlayerRank <= Game.Instance.PlayerPersistentInfo.rank)
			{
				SpawnManager.Instance.SpawnEnemy(E_AgentType.Boss1, component);
			}
		}
	}

	[NESAction]
	public void ChangeSpawnSettings(int index)
	{
		if ((bool)m_SpawnSettings)
		{
			m_SpawnSettings.SendSpawnDataToSpawnManager(index);
		}
	}

	[NESAction]
	public void EndMissionSuccess()
	{
		Mission.Instance.EndOfMission(Mission.E_MissionResult.Success);
	}

	[NESAction]
	public void EndMissionFail(Mission.E_MissionResult reason)
	{
		Mission.Instance.EndOfMission(reason);
	}

	[NESAction]
	public void StartCheckingBandages()
	{
		m_CheckBandages = true;
		Game.Instance.CanHeal = true;
		m_BandagesCount = -1;
		foreach (PPIItemData item in Game.Instance.PlayerPersistentInfo.EquipList.Items)
		{
			if (item.ID == E_ItemID.Bandage)
			{
				m_BandagesCount = item.Count;
				break;
			}
		}
	}

	private void Update()
	{
		SpawnManager.Instance.Update(Time.deltaTime);
		if (!m_NESController || Game.Instance.PlayerPersistentInfo.storyId != 1 || !m_CheckBandages || m_BandagesUsed)
		{
			return;
		}
		foreach (PPIItemData item in Game.Instance.PlayerPersistentInfo.EquipList.Items)
		{
			if (item.ID == E_ItemID.Bandage && item.Count < m_BandagesCount)
			{
				m_BandagesUsed = true;
				m_NESController.SendGameEvent(this, "TutorialBandageUsed");
			}
		}
	}

	private void OnDestroy()
	{
		if (m_SpawnDirector != null)
		{
			SpawnManager.DestroyInstance();
		}
	}

	private string[] GetAvailableNames()
	{
		return s_GameflowTypes;
	}

	private string[] GetSpawnTypes()
	{
		return s_SpawnTypes;
	}

	private string[] GetTutorialTypes()
	{
		return s_TutorialTypes;
	}
}
