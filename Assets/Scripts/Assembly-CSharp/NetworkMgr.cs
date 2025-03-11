#define NETMGR_DBG
#define DEBUG
using System.Diagnostics;
using System.Net;
using UnityEngine;

public class NetworkMgr : MonoBehaviour
{
	public const string m_MasterServerGameTypeId = "ShadowGunTest";

	private static NetworkMgr ms_Instance;

	private static GameObject ms_FakeGameObj;

	private bool m_IsServerRunning;

	private bool m_ServerRegisteredOnMaster;

	private bool m_IsClientConnected;

	private int m_MaxConnections = 32;

	private int m_Port = 27873;

	private string m_MasterServerGameId = "ShadowGun";

	private NetworkView m_NetView;

	public static NetworkMgr GetInstance()
	{
		if (ms_Instance == null)
		{
			ms_FakeGameObj = new GameObject();
			ms_Instance = ms_FakeGameObj.AddComponent<NetworkMgr>() as NetworkMgr;
			ms_Instance.m_NetView = ms_FakeGameObj.AddComponent<NetworkView>() as NetworkView;
			ms_FakeGameObj.name = "NetworkMgrGO";
			DebugUtils.Assert(ms_Instance);
			DebugUtils.Assert(ms_Instance.m_NetView);
		}
		return ms_Instance;
	}

	public bool IsServerRunning()
	{
		return m_IsServerRunning;
	}

	public bool InitServer()
	{
		DebugUtils.Assert(!IsServerRunning());
		DbgMsg("Server init (port " + m_Port + ")");
		m_ServerRegisteredOnMaster = false;
		NetworkConnectionError networkConnectionError;
		if ((networkConnectionError = Network.InitializeServer(m_MaxConnections, m_Port, !Network.HavePublicAddress())) != 0)
		{
			UnityEngine.Debug.LogError("Error initializing server " + networkConnectionError);
			return false;
		}
		m_MasterServerGameId = "ShadowGun on " + Dns.GetHostName();
		MasterServer.RegisterHost("ShadowGunTest", m_MasterServerGameId, string.Empty);
		m_NetView.RPC("LoadLevel", RPCMode.OthersBuffered, Application.loadedLevelName);
		BroadcastCameraParams();
		BroadcastCameraPos();
		m_IsServerRunning = true;
		return true;
	}

	public void DoneServer()
	{
		DbgMsg("Server shutdown");
		if (m_ServerRegisteredOnMaster)
		{
			MasterServer.UnregisterHost();
			m_ServerRegisteredOnMaster = false;
		}
		if (IsServerRunning())
		{
			Network.Disconnect();
		}
	}

	public bool IsClientConnected()
	{
		return m_IsClientConnected;
	}

	public bool ClientConnect(string ipAddr, int port)
	{
		if (m_IsClientConnected)
		{
			ClientDisconnect();
		}
		DbgMsg("Connecting client to " + ipAddr + ":" + port);
		NetworkConnectionError networkConnectionError;
		if ((networkConnectionError = Network.Connect(ipAddr, port)) != 0)
		{
			UnityEngine.Debug.LogError("Error connecting client : " + networkConnectionError);
			return false;
		}
		return true;
	}

	public void ClientDisconnect()
	{
		Network.Disconnect();
		m_IsClientConnected = false;
	}

	private void BroadcastCameraParams()
	{
		if ((bool)Camera.main)
		{
			m_NetView.RPC("SetCameraParams", RPCMode.OthersBuffered, Camera.main.nearClipPlane, Camera.main.farClipPlane, Camera.main.fieldOfView, Camera.main.aspect);
		}
	}

	private void BroadcastCameraPos()
	{
		if ((bool)Camera.main)
		{
			m_NetView.RPC("SetCameraPos", RPCMode.Others, Camera.main.transform.position, Camera.main.transform.rotation);
		}
	}

	private void LateUpdate()
	{
	}

	private void DumpMasterServerGames()
	{
		if (m_ServerRegisteredOnMaster)
		{
			HostData[] array = MasterServer.PollHostList();
			UnityEngine.Debug.Log("Games found on master server:");
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Debug.Log("Game name: " + array[i].gameName);
			}
		}
	}

	private void OnConnectedToServer()
	{
		DbgMsg("Client connected to server");
		m_IsClientConnected = true;
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		Application.LoadLevel(0);
	}

	private void OnServerInitialized()
	{
		UnityEngine.Debug.Log("Server initialized");
	}

	private void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		UnityEngine.Debug.Log("Could not connect to master server: " + info);
	}

	private void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		switch (msEvent)
		{
		case MasterServerEvent.RegistrationFailedGameName:
		case MasterServerEvent.RegistrationFailedGameType:
		case MasterServerEvent.RegistrationFailedNoServer:
			UnityEngine.Debug.LogError("Server registration failed!");
			break;
		case MasterServerEvent.RegistrationSucceeded:
			UnityEngine.Debug.Log("Server registered on MasterServer as " + m_MasterServerGameId);
			m_ServerRegisteredOnMaster = true;
			break;
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		UnityEngine.Debug.Log("Loaded level " + level);
	}

	[RPC]
	private void LoadLevel(string levelName)
	{
		Application.LoadLevelAdditive(levelName);
	}

	[RPC]
	private void SetCameraPos(Vector3 pos, Quaternion rot)
	{
		if ((bool)Camera.main)
		{
			Camera.main.transform.position = pos;
			Camera.main.transform.rotation = rot;
		}
	}

	[RPC]
	private void SetCameraParams(float near, float far, float fov, float aspect)
	{
		if ((bool)Camera.main)
		{
			Camera.main.nearClipPlane = near;
			Camera.main.farClipPlane = far;
			Camera.main.fieldOfView = fov;
			Camera.main.aspect = aspect;
		}
	}

	[Conditional("NETMGR_DBG")]
	private void DbgMsg(string str)
	{
		UnityEngine.Debug.Log(str);
	}
}
