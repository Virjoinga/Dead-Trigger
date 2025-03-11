using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEngine;

public class FriendList : MonoBehaviour
{
	public class PendingFriendInfo
	{
		public string m_Name;

		public string m_CloudCommand;

		public bool isItRequest
		{
			get
			{
				return !string.IsNullOrEmpty(m_CloudCommand);
			}
		}
	}

	public class FriendInfo
	{
		public string m_Name;

		public string m_LastOnline;

		public PlayerPersistentInfoData m_PPIData = new PlayerPersistentInfoData();

		public int Level
		{
			get
			{
				return (m_PPIData == null) ? (-1) : PlayerPersistantInfo.GetPlayerRankFromExperience(m_PPIData.Params.Experience);
			}
		}

		public int Missions
		{
			get
			{
				return (m_PPIData == null) ? (-1) : m_PPIData.Params.MissionCount;
			}
		}
	}

	private const int SKIP_UPDATE_TIMEOUT = 1;

	private List<FriendInfo> m_Friends = new List<FriendInfo>();

	private List<PendingFriendInfo> m_PendingFriends = new List<PendingFriendInfo>();

	private BaseCloudAction m_GetFriendListAction;

	private DateTime m_LastSyncTime;

	public List<FriendInfo> friends
	{
		get
		{
			return m_Friends;
		}
	}

	public List<PendingFriendInfo> pendingFriends
	{
		get
		{
			return m_PendingFriends;
		}
	}

	[method: MethodImpl(32)]
	public event EventHandler FriendListChanged;

	[method: MethodImpl(32)]
	public event EventHandler PendingFriendListChanged;

	public void RetriveFriendListFromCloud(bool inSkipTimeOutCheck = false)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("user is not authenticated, can't fetch friends list");
		}
		else if (m_GetFriendListAction == null && (inSkipTimeOutCheck || !(Mathf.Abs((float)(m_LastSyncTime - DateTime.UtcNow).TotalMinutes) < 1f)))
		{
			m_LastSyncTime = DateTime.UtcNow;
			StartCoroutine(GetFriendListFromCloud_Corutine());
		}
	}

	public void AddNewFriend(string inFriendName)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("user is not authenticated, can't fetch friends list");
			return;
		}
		FriendInfo friendInfo = m_Friends.Find((FriendInfo f) => f.m_Name == inFriendName);
		if (friendInfo == null)
		{
			PendingFriendInfo pendingFriendInfo = m_PendingFriends.Find((PendingFriendInfo f) => f.m_Name == inFriendName);
			if (pendingFriendInfo == null)
			{
				CloudMailbox.FriendRequest friendRequest = new CloudMailbox.FriendRequest();
				friendRequest.m_TargetSystem = "Game.FriendList";
				friendRequest.m_Mailbox = CloudMailbox.E_Mailbox.Global;
				friendRequest.m_Sender = CloudUser.instance.userName;
				friendRequest.m_Message = "Be my friend request";
				GameCloudManager.mailbox.SendMessage(inFriendName, friendRequest);
				PendingFriendInfo pendingFriendInfo2 = new PendingFriendInfo();
				pendingFriendInfo2.m_Name = inFriendName;
				m_PendingFriends.Add(pendingFriendInfo2);
				OnPendingFriendListChanged();
				Save();
			}
		}
	}

	public void AcceptFriendRequest(string inFriendName)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("user is not authenticated, can't Accept Friend Request");
			return;
		}
		PendingFriendInfo pendingFriendInfo = m_PendingFriends.Find((PendingFriendInfo f) => f.m_Name == inFriendName && f.isItRequest);
		if (pendingFriendInfo == null)
		{
			Debug.LogError("Can't accept friend which is not in pending list");
			return;
		}
		GameCloudManager.AddAction(new SendCloudCommand(CloudUser.instance.authenticatedUserID, pendingFriendInfo.m_CloudCommand));
		m_PendingFriends.Remove(pendingFriendInfo);
		OnPendingFriendListChanged();
		Save();
		RetriveFriendListFromCloud(true);
	}

	public void RejectFriendRequest(string inFriendName)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("user is not authenticated, can't Accept Friend Request");
			return;
		}
		PendingFriendInfo pendingFriendInfo = m_PendingFriends.Find((PendingFriendInfo f) => f.m_Name == inFriendName);
		if (pendingFriendInfo == null || !pendingFriendInfo.isItRequest)
		{
			Debug.LogError("Can't reject friend which is not in pending list");
			return;
		}
		CloudMailbox.FriendRequestReject friendRequestReject = new CloudMailbox.FriendRequestReject();
		friendRequestReject.m_TargetSystem = "Game.FriendList";
		friendRequestReject.m_Mailbox = CloudMailbox.E_Mailbox.Global;
		friendRequestReject.m_Sender = CloudUser.instance.userName;
		friendRequestReject.m_Message = "FriendShip rejected";
		GameCloudManager.mailbox.SendMessage(inFriendName, friendRequestReject);
		m_PendingFriends.Remove(pendingFriendInfo);
		OnPendingFriendListChanged();
		Save();
	}

	public void ProcessMessage(CloudMailbox.BaseMessage inMessage)
	{
		CloudMailbox.FriendRequest friendRequest = inMessage as CloudMailbox.FriendRequest;
		if (friendRequest != null)
		{
			FriendInfo friendInfo = m_Friends.Find((FriendInfo f) => f.m_Name == inMessage.m_Sender);
			if (friendInfo != null)
			{
				return;
			}
			List<PendingFriendInfo> list = m_PendingFriends.FindAll((PendingFriendInfo f) => f.m_Name == inMessage.m_Sender);
			if (list != null && list.Count > 0)
			{
				foreach (PendingFriendInfo item in list)
				{
					if (item != null && item.isItRequest)
					{
						return;
					}
				}
			}
			PendingFriendInfo pendingFriendInfo = new PendingFriendInfo();
			pendingFriendInfo.m_Name = friendRequest.m_Sender;
			pendingFriendInfo.m_CloudCommand = friendRequest.m_ConfirmCommand;
			m_PendingFriends.Add(pendingFriendInfo);
			OnPendingFriendListChanged();
			Save();
			return;
		}
		CloudMailbox.FriendRequestReject friendRequestReject = inMessage as CloudMailbox.FriendRequestReject;
		if (friendRequestReject != null)
		{
			List<PendingFriendInfo> list2 = m_PendingFriends.FindAll((PendingFriendInfo friend) => friend.m_Name == inMessage.m_Sender);
			if (list2 != null && list2.Count > 0)
			{
				foreach (PendingFriendInfo item2 in list2)
				{
					m_PendingFriends.Remove(item2);
				}
			}
			OnPendingFriendListChanged();
			Save();
		}
		else
		{
			Debug.LogError(string.Concat("Unknown message ", inMessage, " ", inMessage.msgType));
		}
	}

	public void OnUserAuthenticate()
	{
		Load();
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		m_Friends = null;
		m_PendingFriends = null;
	}

	private void OnPendingFriendListChanged()
	{
		if (this.PendingFriendListChanged != null)
		{
			this.PendingFriendListChanged(this, EventArgs.Empty);
		}
	}

	private void OnFriendListChanged()
	{
		if (this.FriendListChanged != null)
		{
			this.FriendListChanged(this, EventArgs.Empty);
		}
	}

	private IEnumerator GetFriendListFromCloud_Corutine()
	{
		BaseCloudAction action = new GetUserData(CloudUser.instance.authenticatedUserID, "_Friends");
		GameCloudManager.AddAction(action);
		m_GetFriendListAction = action;
		while (!action.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (action.status == BaseCloudAction.E_Status.Failed)
		{
			Debug.LogError("Can't obtain frinds list " + action.result);
		}
		else
		{
			RegenerateFriendList(action.result);
		}
		m_GetFriendListAction = null;
	}

	private void Save()
	{
		string text = "Player[" + CloudUser.instance.userName + "].FriendList";
		string value = JsonMapper.ToJson(m_Friends);
		PlayerPrefs.SetString(text + ".ActiveFriends", value);
		value = JsonMapper.ToJson(m_PendingFriends);
		PlayerPrefs.SetString(text + ".PendingFriends", value);
		PlayerPrefs.Save();
	}

	private void Load()
	{
		string text = "Player[" + CloudUser.instance.userName + "].FriendList";
		string empty = string.Empty;
		empty = PlayerPrefs.GetString(text + ".ActiveFriends", string.Empty);
		m_Friends = JsonMapper.ToObject<List<FriendInfo>>(empty);
		m_Friends = m_Friends ?? new List<FriendInfo>();
		foreach (FriendInfo friend in m_Friends)
		{
			friend.m_PPIData = friend.m_PPIData ?? new PlayerPersistentInfoData();
		}
		empty = PlayerPrefs.GetString(text + ".PendingFriends", string.Empty);
		m_PendingFriends = JsonMapper.ToObject<List<PendingFriendInfo>>(empty);
		m_PendingFriends = m_PendingFriends ?? new List<PendingFriendInfo>();
	}

	private void RegenerateFriendList(string inFriendListInJSON)
	{
		List<FriendInfo> list = new List<FriendInfo>();
		List<string> list2 = new List<string>();
		string[] friends = JsonMapper.ToObject<string[]>(inFriendListInJSON);
		for (int i = 0; i < friends.Length; i++)
		{
			FriendInfo friendInfo = m_Friends.Find((FriendInfo friend) => friend.m_Name == friends[i]);
			if (friendInfo == null)
			{
				FriendInfo friendInfo2 = new FriendInfo();
				friendInfo2.m_Name = friends[i];
				friendInfo2.m_LastOnline = null;
				list.Add(friendInfo2);
				list2.Add(friends[i]);
			}
			else
			{
				list.Add(friendInfo);
				list2.Add(friends[i]);
			}
			List<PendingFriendInfo> list3 = m_PendingFriends.FindAll((PendingFriendInfo friend) => friend.m_Name == friends[i]);
			if (list3 == null || list3.Count <= 0)
			{
				continue;
			}
			foreach (PendingFriendInfo item in list3)
			{
				m_PendingFriends.Remove(item);
			}
		}
		if (list2.Count > 0)
		{
			StartCoroutine(UpdateFriendsData_Corutine(list2));
		}
		m_Friends = list;
		OnFriendListChanged();
		OnPendingFriendListChanged();
		Save();
	}

	private IEnumerator UpdateFriendsData_Corutine(List<string> inFriends)
	{
		BaseCloudAction action = new QueryFriendsInfo(inFriends: JsonMapper.ToJson(inFriends), inUserID: CloudUser.instance.authenticatedUserID);
		GameCloudManager.AddAction(action);
		while (!action.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (action.status == BaseCloudAction.E_Status.Success)
		{
			ProcessFriendsDetails(action.result);
			Save();
		}
		else
		{
			Debug.LogError("Can't obtain frinds info " + action.result);
		}
	}

	private void ProcessFriendsDetails(string inFriendsDetails)
	{
		JsonData[] array = JsonMapper.ToObject<JsonData[]>(inFriendsDetails);
		JsonData[] array2 = array;
		foreach (JsonData jsonData in array2)
		{
			try
			{
				string name = (string)jsonData["name"];
				string jsonStr = (string)jsonData["data"];
				FriendInfo friendInfo = m_Friends.Find((FriendInfo friend) => friend.m_Name == name);
				if (friendInfo != null)
				{
					friendInfo.m_PPIData = InitPlayerDataFromStr(jsonStr);
					if (friendInfo.m_PPIData == null)
					{
						Debug.LogWarning("Can't read PlayerPersistentInfoData");
						friendInfo.m_PPIData = new PlayerPersistentInfoData();
					}
				}
				else
				{
					Debug.LogWarning("Code error this is imposible!!!");
				}
				OnFriendListChanged();
			}
			catch
			{
				Debug.Log("Mesage is not a valid JSON object");
			}
		}
	}

	private PlayerPersistentInfoData InitPlayerDataFromStr(string jsonStr)
	{
		try
		{
			return JsonMapper.ToObject<PlayerPersistentInfoData>(jsonStr);
		}
		catch (JsonException ex)
		{
			Debug.LogError("JSON exception caught: " + ex.Message);
		}
		return null;
	}
}
