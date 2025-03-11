using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TwitterWrapper : MonoBehaviour
{
	private class Request
	{
		public string Name { get; private set; }

		public RequestType Type { get; private set; }

		public Dictionary<string, string> Params { get; private set; }

		public Action<string, object> Callback { get; private set; }

		public Request(string Name, RequestType Type, Dictionary<string, string> Params, Action<string, object> Callback)
		{
			this.Name = Name;
			this.Type = Type;
			this.Params = Params;
			this.Callback = Callback;
		}
	}

	private const int MsgLengthLimit = 140;

	private static TwitterWrapper m_Instance;

	private static GameObject m_InstanceOwner;

	private static List<Request> m_Requests = new List<Request>();

	private static bool m_ProcessingRequest;

	public static TwitterWrapper Instance
	{
		get
		{
			return m_Instance;
		}
	}

	public static int PendingRequests
	{
		get
		{
			return m_Requests.Count;
		}
	}

	[method: MethodImpl(32)]
	public static event Action OnLoginSuccess;

	[method: MethodImpl(32)]
	public static event Action<string> OnLoginFailure;

	public static void Init(string CustomerKey, string CustomerSecret)
	{
		if (m_Instance == null)
		{
			m_InstanceOwner = new GameObject("_Twitter_");
			m_Instance = m_InstanceOwner.AddComponent<TwitterWrapper>();
			UnityEngine.Object.DontDestroyOnLoad(m_Instance);
			TwitterAndroid.init(CustomerKey, CustomerSecret);
			TwitterManager.loginSucceededEvent += LoginSuccess;
			TwitterManager.loginFailedEvent += LoginFailure;
			TwitterManager.requestDidFinishEvent += RequestSuccess;
			TwitterManager.requestDidFailEvent += RequestFailure;
		}
	}

	public static void Done()
	{
		if (m_Instance != null)
		{
			TwitterAndroid.logout();
			TwitterManager.loginSucceededEvent -= LoginSuccess;
			TwitterManager.loginFailedEvent -= LoginFailure;
			TwitterManager.requestDidFinishEvent -= RequestSuccess;
			TwitterManager.requestDidFailEvent -= RequestFailure;
			UnityEngine.Object.Destroy(m_InstanceOwner);
			m_InstanceOwner = null;
			m_Instance = null;
		}
	}

	public static void LogIn()
	{
		if (m_Instance != null)
		{
			TwitterAndroid.showLoginDialog();
		}
	}

	public static void LogOut()
	{
		if (m_Instance != null)
		{
			TwitterAndroid.logout();
		}
	}

	public static bool IsLoggedIn()
	{
		if (m_Instance != null)
		{
			return TwitterAndroid.isLoggedIn();
		}
		return false;
	}

	public static void PostMessage(string Message, Action<string, object> Callback)
	{
		if (Message.Length > 140)
		{
			Message = Message.Substring(0, 140);
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("status", Message);
		Dictionary<string, string> @params = dictionary;
		SendRequest("/1.1/statuses/update.json", RequestType.Post, @params, Callback);
	}

	public static void PostImage(string Message, string PathToImage)
	{
		int num = 115;
		if (Message.Length > num)
		{
			Message = Message.Substring(0, num);
		}
		byte[] array = null;
		if (File.Exists(PathToImage))
		{
			array = File.ReadAllBytes(PathToImage);
		}
		if (array != null)
		{
			TwitterAndroid.postStatusUpdate(Message, array);
		}
		else
		{
			TwitterAndroid.postStatusUpdate(Message);
		}
	}

	public static void SendRequest(string Name, RequestType Type, Dictionary<string, string> Params, Action<string, object> Callback)
	{
		if (Callback != null)
		{
			m_Requests.Add(new Request(Name, Type, Params, Callback));
			RequestInit();
		}
	}

	public static void ShowMessageComposer(string Message)
	{
		if (m_Instance != null)
		{
			m_Instance.StartCoroutine(MessageComposer(Message));
		}
	}

	private static IEnumerator MessageComposer(string Message)
	{
		TouchScreenKeyboard kb = TouchScreenKeyboard.Open(Message, TouchScreenKeyboardType.NamePhonePad, true, true, false);
		if (kb == null)
		{
			yield break;
		}
		while (!kb.done)
		{
			if (kb.text.Length > 140)
			{
				kb.text = kb.text.Substring(0, 140);
			}
			yield return new WaitForEndOfFrame();
		}
		if (!kb.wasCanceled && kb.text.Length > 0)
		{
			PostMessage(kb.text, null);
		}
	}

	private static void RequestInit()
	{
		if (!m_ProcessingRequest && m_Requests.Count > 0)
		{
			Request request = m_Requests[0];
			TwitterAndroid.performRequest((request.Type != RequestType.Post) ? "get" : "post", request.Name, request.Params);
			m_ProcessingRequest = true;
		}
	}

	private static void RequestDone(string Error, object Result)
	{
		Request request = m_Requests[0];
		request.Callback(Error, Result);
		m_Requests.RemoveAt(0);
		m_ProcessingRequest = false;
	}

	private static void RequestSuccess(object Result)
	{
		RequestDone(null, Result);
		RequestInit();
	}

	private static void RequestFailure(string Error)
	{
		RequestDone((Error == null) ? "null-error-message" : Error, null);
		RequestInit();
	}

	private static void LoginSuccess(string Empty)
	{
		if (TwitterWrapper.OnLoginSuccess != null)
		{
			TwitterWrapper.OnLoginSuccess();
		}
	}

	private static void LoginFailure(string Error)
	{
		if (TwitterWrapper.OnLoginFailure != null)
		{
			TwitterWrapper.OnLoginFailure(Error);
		}
	}
}
