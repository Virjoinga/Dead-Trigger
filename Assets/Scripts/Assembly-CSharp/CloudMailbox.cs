using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class CloudMailbox : MonoBehaviour
{
	public enum E_Mailbox
	{
		Global = 0,
		Product = 1
	}

	public class BaseMessage
	{
		public E_Mailbox m_Mailbox;

		public string m_Sender;

		public string m_Message;

		public string m_RAWMessage;

		public DateTime m_SendTime;

		public virtual string msgType
		{
			get
			{
				return "BaseMessage";
			}
		}

		public virtual bool isValid
		{
			get
			{
				return true;
			}
		}

		public virtual bool isSpecialMessage
		{
			get
			{
				return false;
			}
		}
	}

	public class SystemCommand : BaseMessage
	{
		public string m_TargetSystem;

		public override string msgType
		{
			get
			{
				return "SystemCommand";
			}
		}

		public override bool isSpecialMessage
		{
			get
			{
				return true;
			}
		}
	}

	public class FriendRequest : SystemCommand
	{
		public string m_ConfirmCommand;

		public override string msgType
		{
			get
			{
				return "FriendRequest";
			}
		}
	}

	public class FriendRequestReject : SystemCommand
	{
		public override string msgType
		{
			get
			{
				return "FriendRequestReject";
			}
		}
	}

	private const int SKIP_UPDATE_TIMEOUT = 5;

	private List<BaseMessage> m_Inbox = new List<BaseMessage>();

	private List<BaseMessage> m_Outbox = new List<BaseMessage>();

	private bool m_MessageFetchInProggres;

	private DateTime m_LastSyncTime;

	public void FetchMessages(bool inSkipTimeOutCheck = false)
	{
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("sender is not authenticated, this message can't be send");
		}
		else if (!m_MessageFetchInProggres && (inSkipTimeOutCheck || !(Mathf.Abs((float)(m_LastSyncTime - DateTime.UtcNow).TotalMinutes) < 5f)))
		{
			m_LastSyncTime = DateTime.UtcNow;
			m_MessageFetchInProggres = true;
			StartCoroutine(FetchMessages_Corutine());
		}
	}

	public void SendMessage(string inRecipient, string inMessageBody, E_Mailbox inMailbox = E_Mailbox.Global)
	{
		if (string.IsNullOrEmpty(inRecipient))
		{
			Debug.LogError("Invalid recipient " + inRecipient);
			return;
		}
		if (string.IsNullOrEmpty(inMessageBody))
		{
			Debug.LogError("Invalid message " + inMessageBody);
			return;
		}
		if (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.LogError("sender is not authenticated, this message can't be send");
			return;
		}
		BaseMessage baseMessage = new BaseMessage();
		baseMessage.m_Mailbox = inMailbox;
		baseMessage.m_Sender = CloudUser.instance.userName;
		baseMessage.m_Message = inMessageBody;
		SendMessage(inRecipient, baseMessage);
	}

	public void SendMessage(string inRecipient, BaseMessage inMessage)
	{
		if (string.IsNullOrEmpty(inRecipient))
		{
			Debug.LogError("Invalid recipient " + inRecipient);
			return;
		}
		if (inMessage == null || !inMessage.isValid)
		{
			Debug.LogError("Invalid message " + inMessage);
			return;
		}
		inMessage.m_SendTime = DateTime.Now;
		string inMessage2 = JsonMapper.ToJson(inMessage);
		if (inMessage is FriendRequest)
		{
			SendMessage inAction = new SendFriendRequestMessage(CloudUser.instance.authenticatedUserID, inRecipient, inMessage2);
			GameCloudManager.AddAction(inAction);
		}
		else
		{
			bool inGlobalInbox = inMessage.m_Mailbox == E_Mailbox.Global;
			SendMessage inAction2 = new SendMessage(CloudUser.instance.authenticatedUserID, inRecipient, inMessage2, inGlobalInbox);
			GameCloudManager.AddAction(inAction2);
		}
		m_Outbox.Add(inMessage);
		SaveOutbox();
	}

	public void OnUserAuthenticate()
	{
		LoadInbox();
		LoadOutbox();
	}

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		m_Inbox = null;
		m_Outbox = null;
	}

	private IEnumerator FetchMessages_Corutine()
	{
		yield return StartCoroutine(FetchMessages_Corutine(true));
		yield return StartCoroutine(FetchMessages_Corutine(false));
		m_MessageFetchInProggres = false;
	}

	private IEnumerator FetchMessages_Corutine(bool inGlobalInbox, bool inRemoveMessagesFromServer = true)
	{
		BaseCloudAction action2 = new GetMessagesFromInbox(CloudUser.instance.authenticatedUserID, inGlobalInbox);
		GameCloudManager.AddAction(action2);
		while (!action2.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		int lastMessageIndex = -1;
		if (action2.status == BaseCloudAction.E_Status.Failed)
		{
			Debug.LogError("Can't obtain messages " + action2.result);
		}
		else
		{
			lastMessageIndex = ProcessMessages(action2.result, inGlobalInbox);
		}
		SaveInbox();
		if (lastMessageIndex > 0 && inRemoveMessagesFromServer)
		{
			action2 = new RemoveMessagesFromInbox(CloudUser.instance.authenticatedUserID, inGlobalInbox, lastMessageIndex);
			GameCloudManager.AddAction(action2);
			while (!action2.isDone)
			{
				yield return new WaitForSeconds(0.2f);
			}
			if (action2.status == BaseCloudAction.E_Status.Failed)
			{
				Debug.LogError("messages wasn't removed correctly " + action2.result);
			}
		}
	}

	private int ProcessMessages(string inRawMessageFromCloud, bool inGlobalInbox)
	{
		JsonData jsonData = JsonMapper.ToObject(inRawMessageFromCloud);
		string[] array = JsonMapper.ToObject<string[]>((string)jsonData["messages"]);
		int result = (int)jsonData["lastMsgIdx"];
		string[] array2 = array;
		foreach (string inRawMessage in array2)
		{
			m_Inbox.Add(ProcessMessage(inRawMessage, inGlobalInbox));
		}
		return result;
	}

	private BaseMessage ProcessMessage(string inRawMessage, bool inGlobalInbox)
	{
		JsonData jsonData = null;
		try
		{
			jsonData = JsonMapper.ToObject(inRawMessage);
		}
		catch
		{
			Debug.Log("Message is not a JSON object");
		}
		BaseMessage baseMessage = null;
		if (jsonData == null || !jsonData.HasValue("msgType"))
		{
			baseMessage = new BaseMessage();
			baseMessage.m_RAWMessage = inRawMessage;
		}
		else
		{
			switch (jsonData["msgType"].ToString())
			{
			default:
			{
				int num = 0;
				baseMessage = ((num != 1) ? new BaseMessage() : new FriendRequestReject());
				break;
			}
			case "FriendRequest":
			{
				FriendRequest friendRequest = new FriendRequest();
				if (jsonData.HasValue("_respCmd"))
				{
					friendRequest.m_ConfirmCommand = (string)jsonData["_respCmd"];
				}
				else
				{
					Debug.LogWarning("invalid request friend message");
				}
				baseMessage = friendRequest;
				break;
			}
			}
			baseMessage.m_Sender = ((!jsonData.HasValue("m_Sender")) ? "<UNKNOWN>" : ((string)jsonData["m_Sender"]));
			baseMessage.m_Mailbox = ((!inGlobalInbox) ? E_Mailbox.Product : E_Mailbox.Global);
			baseMessage.m_RAWMessage = inRawMessage;
			if (baseMessage.isSpecialMessage)
			{
				if (jsonData.HasValue("m_TargetSystem"))
				{
					((SystemCommand)baseMessage).m_TargetSystem = (string)jsonData["m_TargetSystem"];
					switch (((SystemCommand)baseMessage).m_TargetSystem)
					{
					case "Game.FriendList":
						GameCloudManager.friendList.ProcessMessage(baseMessage);
						break;
					default:
						Debug.LogError("Unnown target system " + ((SystemCommand)baseMessage).m_TargetSystem);
						break;
					}
				}
				else
				{
					Debug.Log("Invalid message " + baseMessage.ToString());
				}
			}
		}
		return baseMessage;
	}

	private void SaveInbox()
	{
		string value = JsonMapper.ToJson(m_Inbox);
		string text = "Player[" + CloudUser.instance.userName + "].CloudMailbox";
		PlayerPrefs.SetString(text + ".Inbox", value);
		PlayerPrefs.Save();
	}

	private void SaveOutbox()
	{
		string value = JsonMapper.ToJson(m_Outbox);
		string text = "Player[" + CloudUser.instance.userName + "].CloudMailbox";
		PlayerPrefs.SetString(text + ".Outbox", value);
		PlayerPrefs.Save();
	}

	private void LoadInbox()
	{
		string empty = string.Empty;
		string text = "Player[" + CloudUser.instance.userName + "].CloudMailbox";
		empty = PlayerPrefs.GetString(text + ".Inbox", string.Empty);
		m_Inbox = JsonMapper.ToObject<List<BaseMessage>>(empty);
		m_Inbox = m_Inbox ?? new List<BaseMessage>();
	}

	private void LoadOutbox()
	{
		string empty = string.Empty;
		string text = "Player[" + CloudUser.instance.userName + "].CloudMailbox";
		empty = PlayerPrefs.GetString(text + ".Outbox", string.Empty);
		m_Outbox = JsonMapper.ToObject<List<BaseMessage>>(empty);
		m_Outbox = m_Outbox ?? new List<BaseMessage>();
	}
}
