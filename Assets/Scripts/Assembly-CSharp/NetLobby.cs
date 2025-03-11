using UnityEngine;

public class NetLobby : MonoBehaviour
{
	private bool m_IsConnected;

	private bool m_ServerListReady;

	private bool m_RefreshingServersList;

	private void Awake()
	{
		RefreshServersList();
	}

	private void RefreshServersList()
	{
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("ShadowGunTest");
		MasterServer.PollHostList();
		m_ServerListReady = false;
		m_RefreshingServersList = true;
	}

	private void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			m_RefreshingServersList = false;
			m_ServerListReady = true;
		}
	}

	private void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			GUILayout.Label("Conntect to:");
			HostData[] array = MasterServer.PollHostList();
			for (int i = 0; i < array.Length; i++)
			{
				if (GUILayout.Button(array[i].gameName))
				{
					if (NetworkMgr.GetInstance().ClientConnect(array[i].ip[0], array[i].port))
					{
						m_IsConnected = true;
					}
					break;
				}
			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			if (GUILayout.Button("Disconnect"))
			{
				NetworkMgr.GetInstance().ClientDisconnect();
				RefreshServersList();
			}
		}
		else if (Network.peerType == NetworkPeerType.Connecting)
		{
			GUILayout.Label("Connecting ...");
		}
	}
}
