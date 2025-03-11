using System;
using System.Collections;
using UnityEngine;

public class GameCloudManager : MonoBehaviour
{
	private const int SKIP_UPDATE_TIMEOUT = 1;

	private static GameCloudManager m_sInstance;

	private Queue<BaseCloudAction> PendingActions = new Queue<BaseCloudAction>();

	private BaseCloudAction ActiveAction;

	private FriendList m_FriendList;

	private CloudMailbox m_CloudMailbox;

	private PlayerPersistantInfo m_CloudPPI;

	private DataFileJSON m_CloudProgress;

	private DateTime m_LastDownloadTime;

	public static string productID
	{
		get
		{
			return "DeadTrigger";
		}
	}

	public static FriendList friendList
	{
		get
		{
			return instance.m_FriendList;
		}
	}

	public static CloudMailbox mailbox
	{
		get
		{
			return instance.m_CloudMailbox;
		}
	}

	public static PlayerPersistantInfo cloudPPI
	{
		get
		{
			return instance.m_CloudPPI;
		}
	}

	private static GameCloudManager instance
	{
		get
		{
			return GetInstance();
		}
	}

	public static void AddAction(BaseCloudAction inAction)
	{
		instance.PendingActions.Enqueue(inAction);
	}

	public static BaseCloudAction SendPPIToCloud(string inPPIInJSON)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogWarning("Can't send PPI to cloud. User is not authenticated.");
			return null;
		}
		BaseCloudAction baseCloudAction = new SetPlayerPersistantInfo(CloudUser.instance.authenticatedUserID, inPPIInJSON);
		AddAction(baseCloudAction);
		return baseCloudAction;
	}

	public static BaseCloudAction BackupProgressToCloud()
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogWarning("Can't send progress to cloud. User is not authenticated.");
			return null;
		}
		if (Game.Instance == null || Game.Instance.PlayerPersistentInfo == null)
		{
			Debug.LogWarning("Can't send progress to cloud. There isn't valid local PPI");
			return null;
		}
		if (CityManager.Instance == null)
		{
			Debug.LogWarning("Can't send progress to cloud. Can't access CityManager");
			return null;
		}
		Game.Instance.PlayerPersistentInfo.SetAccountNameifNotExist(CloudUser.instance.userName);
		string playerDataAsJsonStr = Game.Instance.PlayerPersistentInfo.GetPlayerDataAsJsonStr();
		string cityProgressAsJSON = CityManager.Instance.GetCityProgressAsJSON();
		DataFileJSON dataFileJSON = new DataFileJSON(string.Empty);
		dataFileJSON.SetString("PlayerData", playerDataAsJsonStr);
		dataFileJSON.SetString("GameProgress", cityProgressAsJSON);
		BaseCloudAction baseCloudAction = new SetUserProductData(CloudUser.instance.authenticatedUserID, "_Progress", dataFileJSON.ToString());
		instance.StartCoroutine(instance.BackupProgressToCloud_Corutine(baseCloudAction, playerDataAsJsonStr, cityProgressAsJSON));
		return baseCloudAction;
	}

	public static BaseCloudAction RetrieveProgressFromCloud(bool inSkipTimeOutCheck = false)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogWarning("Can't send PPI to cloud. User is not authenticated.");
			return null;
		}
		if (!inSkipTimeOutCheck && Mathf.Abs((float)(instance.m_LastDownloadTime - DateTime.UtcNow).TotalMinutes) < 1f)
		{
			return null;
		}
		BaseCloudAction baseCloudAction = new GetUserProductData(CloudUser.instance.authenticatedUserID, "_Progress");
		instance.StartCoroutine(instance.RetrieveProgressFromCloud_Corutine(baseCloudAction));
		return baseCloudAction;
	}

	public static bool CanRestoreProgressFromCloud()
	{
		if (instance.m_CloudPPI == null || instance.m_CloudProgress == null)
		{
			return false;
		}
		return true;
	}

	public static bool RestoreProgressFromCloud()
	{
		if (!CanRestoreProgressFromCloud())
		{
			Debug.LogError("RestoreProgressFromCloud, internal error !!!");
			return false;
		}
		Game.Instance.PlayerPersistentInfo.CopyPlayerData(instance.m_CloudPPI);
		Game.Instance.PlayerPersistentInfo.Save();
		instance.m_CloudPPI = null;
		instance.m_LastDownloadTime = default(DateTime);
		GameSaveLoadUtl.SaveGameData(instance.m_CloudProgress);
		Game.Instance.MissionResultData.Result = MissionResult.Type.NONE;
		Game.Instance.LoadMainMenu(true);
		return true;
	}

	private void Awake()
	{
		m_FriendList = base.gameObject.AddComponent<FriendList>();
		m_CloudMailbox = base.gameObject.AddComponent<CloudMailbox>();
	}

	private void Update()
	{
		UpdateCloudActions();
	}

	private void OnDestroy()
	{
		PendingActions.Clear();
		ActiveAction = null;
	}

	private static GameCloudManager GetInstance()
	{
		if (m_sInstance == null)
		{
			GameObject gameObject = new GameObject("GameCloudManager");
			m_sInstance = gameObject.AddComponent<GameCloudManager>();
			UnityEngine.Object.DontDestroyOnLoad(m_sInstance);
		}
		return m_sInstance;
	}

	private void UpdateCloudActions()
	{
		if (ActiveAction != null)
		{
			ActiveAction.PPIManager_Update();
			if (!ActiveAction.isDone)
			{
				return;
			}
			ActiveAction = null;
		}
		if (PendingActions.Count != 0)
		{
			ActiveAction = PendingActions.Dequeue();
		}
	}

	private IEnumerator BackupProgressToCloud_Corutine(BaseCloudAction inAction, string inPPI, string inProgress)
	{
		AddAction(inAction);
		while (!inAction.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (inAction.status != BaseCloudAction.E_Status.Success)
		{
			Debug.LogError("Cloud backup failed: " + inAction.failInfo);
		}
		else if (!SetCloudProgressData(inPPI, inProgress))
		{
			m_CloudPPI = null;
			m_CloudProgress = null;
			m_LastDownloadTime = default(DateTime);
		}
	}

	private IEnumerator RetrieveProgressFromCloud_Corutine(BaseCloudAction inAction)
	{
		AddAction(inAction);
		while (!inAction.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (inAction.status != BaseCloudAction.E_Status.Success)
		{
			Debug.LogError("Retrieve Progress From Cloud failed: " + inAction.failInfo);
			yield break;
		}
		try
		{
			DataFileJSON jsonData = new DataFileJSON(inAction.result);
			string ppi = jsonData.GetString("PlayerData", string.Empty);
			string progress = jsonData.GetString("GameProgress", string.Empty);
			if (!SetCloudProgressData(ppi, progress))
			{
				m_CloudPPI = null;
				m_CloudProgress = null;
				m_LastDownloadTime = default(DateTime);
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Exception during processing progress data from Cloud. \nDetails: \n" + e.Message);
		}
	}

	private bool SetCloudProgressData(string inPPIJSON, string inProgressJSON)
	{
		PlayerPersistantInfo playerPersistantInfo = new PlayerPersistantInfo();
		DataFileJSON dataFileJSON = new DataFileJSON();
		if (string.IsNullOrEmpty(inPPIJSON) || !playerPersistantInfo.InitPlayerDataFromStr(inPPIJSON))
		{
			Debug.LogError("JSON data consistency error. Can't recreate PPI from JSON string");
			return false;
		}
		if (string.IsNullOrEmpty(inProgressJSON) || !dataFileJSON.InitFromString(inProgressJSON))
		{
			Debug.LogError("JSON data consistency error. City progress string is not valid");
			return false;
		}
		dataFileJSON.SetString("DEVICE_SIG", SysUtils.GetUniqueDeviceID());
		m_CloudPPI = playerPersistantInfo;
		m_CloudProgress = dataFileJSON;
		m_LastDownloadTime = DateTime.UtcNow;
		return true;
	}
}
