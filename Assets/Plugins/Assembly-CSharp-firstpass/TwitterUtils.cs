using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class TwitterUtils
{
	private class Job_Login
	{
		private Action<bool> m_Callback;

		public Job_Login(Action<bool> Callback)
		{
			m_Callback = Callback;
		}

		public void Process()
		{
			AddJob(this);
			TwitterWrapper.OnLoginSuccess += OnSuccess;
			TwitterWrapper.OnLoginFailure += OnFailure;
			TwitterWrapper.LogIn();
		}

		private void OnSuccess()
		{
			Finish(true);
		}

		private void OnFailure(string Error)
		{
			Finish(false);
		}

		private void Finish(bool Result)
		{
			m_Callback(Result);
			TwitterWrapper.OnLoginSuccess -= OnSuccess;
			TwitterWrapper.OnLoginFailure -= OnFailure;
			RemoveJob(this);
		}
	}

	private class Job_UserID
	{
		private Action<string> m_Callback;

		public Job_UserID(Action<string> Callback)
		{
			m_Callback = Callback;
		}

		public void Process()
		{
			AddJob(this);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["skip_status"] = "true";
			dictionary["include_entities"] = "false";
			TwitterWrapper.SendRequest("/1.1/account/verify_credentials.json", RequestType.Get, dictionary, OnRequestResult);
		}

		private void OnRequestResult(string Error, object Result)
		{
			string obj = string.Empty;
			object obj2;
			if (Error == null)
			{
				IDictionary dictionary = Result as IDictionary;
				obj2 = dictionary;
			}
			else
			{
				obj2 = null;
			}
			IDictionary dictionary2 = (IDictionary)obj2;
			if (dictionary2 != null && dictionary2.Contains("id_str"))
			{
				obj = dictionary2["id_str"].ToString();
			}
			if (Error == null)
			{
			}
			m_Callback(obj);
			RemoveJob(this);
		}
	}

	private class Job_FollowingIDs
	{
		private const int BatchSize = 1000;

		private Action<List<string>> m_Callback;

		private Dictionary<string, string> m_Params;

		private bool m_AskAgain;

		private bool m_Finished;

		private bool m_Success;

		private List<string> m_List;

		private int m_ResSize;

		public Job_FollowingIDs(Action<List<string>> Callback)
		{
			m_Callback = Callback;
		}

		public void Process()
		{
			AddJob(this);
			TwitterWrapper.Instance.StartCoroutine(Do());
		}

		private IEnumerator Do()
		{
			m_ResSize = 0;
			m_List = new List<string>(128);
			m_Params = new Dictionary<string, string>();
			m_Params["stringify_ids"] = "true";
			m_Params["cursor"] = "-1";
			m_Params["count"] = 1000.ToString();
			do
			{
				m_Success = false;
				m_Finished = false;
				m_AskAgain = false;
				TwitterWrapper.SendRequest("/1.1/friends/ids.json", RequestType.Get, m_Params, OnRequestCompleted);
				float start = Time.realtimeSinceStartup;
				do
				{
					yield return new WaitForSeconds(0.1f);
				}
				while (!m_Finished && Time.realtimeSinceStartup - start < 20f);
			}
			while (m_AskAgain);
			if (m_Success)
			{
				m_Callback(m_List);
			}
			else
			{
				if (!m_Finished)
				{
				}
				m_Callback(null);
			}
			RemoveJob(this);
		}

		private void OnRequestCompleted(string Error, object Result)
		{
			m_Finished = true;
			if (Error == null)
			{
				IDictionary dictionary = Result as IDictionary;
				if (dictionary != null)
				{
					m_Success = ExtractIDs(dictionary);
					m_AskAgain = ExtractPaging(dictionary);
				}
			}
		}

		private bool ExtractIDs(IDictionary Data)
		{
			if (!Data.Contains("ids"))
			{
				return false;
			}
			IList list = Data["ids"] as IList;
			if (list == null)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				m_List.Add(list[i].ToString());
				m_ResSize++;
			}
			if (m_List.Count > 0)
			{
				m_Callback(m_List);
				m_List.Clear();
			}
			return true;
		}

		private bool ExtractPaging(IDictionary Data)
		{
			if (Data.Contains("next_cursor_str"))
			{
				string text = Data["next_cursor_str"].ToString();
				if (!string.Equals(text, "0"))
				{
					m_Params["cursor"] = text;
					return true;
				}
			}
			return false;
		}
	}

	private class Job_DoesUserFollow
	{
		private Action<bool> m_Callback;

		private string m_IDs;

		public Job_DoesUserFollow(Action<bool> Callback, string TwitterIDs)
		{
			m_Callback = Callback;
			m_IDs = TwitterIDs;
		}

		public void Process()
		{
			AddJob(this);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("user_id", m_IDs);
			Dictionary<string, string> @params = dictionary;
			TwitterWrapper.SendRequest("/1.1/friendships/lookup.json", RequestType.Get, @params, OnRequestResult);
		}

		private void OnRequestResult(string Error, object Result)
		{
			bool obj = false;
			if (Error == null)
			{
				int num = 0;
				int num2 = 0;
				IList list = Result as IList;
				if (list != null && list.Count > 0)
				{
					num = list.Count;
					num2 = ProcessData(list);
					obj = num2 == num && num > 0;
				}
			}
			m_Callback(obj);
			RemoveJob(this);
		}

		private int ProcessData(IList Data)
		{
			int num = 0;
			for (int i = 0; i < Data.Count; i++)
			{
				IDictionary dictionary = Data[i] as IDictionary;
				if (dictionary != null && dictionary.Contains("connections"))
				{
					IList list = dictionary["connections"] as IList;
					if (list != null && list.Contains("following"))
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	private class Job_PostMessage
	{
		private Action<bool> m_Callback;

		private string m_Message;

		private static bool m_LoginSucceeded;

		private static bool m_LoginInProgress;

		public Job_PostMessage(Action<bool> Callback, string Message)
		{
			m_Callback = Callback;
			m_Message = Message;
		}

		public void Process()
		{
			AddJob(this);
			TwitterWrapper.Instance.StartCoroutine(Do());
		}

		private IEnumerator Do()
		{
			if (!TwitterWrapper.IsLoggedIn())
			{
				if (!m_LoginInProgress)
				{
					m_LoginInProgress = true;
					LogIn(OnLoginResult);
				}
				while (m_LoginInProgress)
				{
					yield return new WaitForEndOfFrame();
				}
				if (!m_LoginSucceeded)
				{
					Finish(false);
					yield break;
				}
			}
			TwitterWrapper.PostMessage(m_Message, OnPostResult);
		}

		private void OnLoginResult(bool Success)
		{
			if (!Success)
			{
			}
			m_LoginInProgress = false;
			m_LoginSucceeded = Success;
		}

		private void OnPostResult(string Error, object Result)
		{
			bool flag = Error == null;
			if (flag)
			{
			}
			Finish(flag);
		}

		private void Finish(bool Success)
		{
			if (m_Callback != null)
			{
				m_Callback(Success);
			}
			RemoveJob(this);
		}
	}

	private class Job_PostScreenshot
	{
		private Action<bool> m_Callback;

		private string m_Message;

		private string m_PicName;

		private static int m_PicCounter;

		private static bool m_LoginSucceeded;

		private static bool m_LoginInProgress;

		public Job_PostScreenshot(Action<bool> Callback, string Message)
		{
			m_Message = Message;
			m_PicName = "_tw_pic" + m_PicCounter + "_.png";
			m_PicCounter = (m_PicCounter + 1) % 6;
			m_Callback = Callback;
		}

		public void Process()
		{
			AddJob(this);
			TwitterWrapper.Instance.StartCoroutine(Do());
		}

		private IEnumerator Do()
		{
			string filePath = Application.persistentDataPath + "/" + m_PicName;
			File.Delete(filePath);
			ScreenCapture.CaptureScreenshot(m_PicName);
			float wait = 12f;
			float step = 0.25f;
			bool fileExists2 = false;
			float num;
			do
			{
				yield return new WaitForSeconds(step);
				fileExists2 = File.Exists(filePath);
				if (fileExists2)
				{
					break;
				}
				wait = (num = wait - step);
			}
			while (num > 0f);
			if (!fileExists2)
			{
				Finish(false);
				yield break;
			}
			if (!TwitterWrapper.IsLoggedIn())
			{
				if (!m_LoginInProgress)
				{
					m_LoginInProgress = true;
					LogIn(OnLoginResult);
				}
				while (m_LoginInProgress)
				{
					yield return new WaitForEndOfFrame();
				}
				if (!m_LoginSucceeded)
				{
					Finish(false);
					yield break;
				}
			}
			TwitterWrapper.PostImage(m_Message, filePath);
			Finish(true);
		}

		private void OnLoginResult(bool Success)
		{
			m_LoginInProgress = false;
			m_LoginSucceeded = Success;
		}

		private void Finish(bool Success)
		{
			if (m_Callback != null)
			{
				m_Callback(Success);
			}
			RemoveJob(this);
		}
	}

	private const float CompletionCheckPeriod = 0.1f;

	private const float CompletionTimeOut = 20f;

	private static List<object> m_Jobs = new List<object>();

	public static void LogIn(Action<bool> CompletionCallback)
	{
		if (CompletionCallback != null)
		{
			Job_Login job_Login = new Job_Login(CompletionCallback);
			job_Login.Process();
		}
	}

	public static void GetUserID(Action<string> CompletionCallback)
	{
		if (CompletionCallback != null)
		{
			Job_UserID job_UserID = new Job_UserID(CompletionCallback);
			job_UserID.Process();
		}
	}

	public static void GetFollowingIDs(Action<List<string>> CompletionCallback)
	{
		if (CompletionCallback != null)
		{
			Job_FollowingIDs job_FollowingIDs = new Job_FollowingIDs(CompletionCallback);
			job_FollowingIDs.Process();
		}
	}

	public static void DoesUserFollow(string TwitterIDs, Action<bool> CompletionCallback)
	{
		if (CompletionCallback != null && !string.IsNullOrEmpty(TwitterIDs))
		{
			Job_DoesUserFollow job_DoesUserFollow = new Job_DoesUserFollow(CompletionCallback, TwitterIDs);
			job_DoesUserFollow.Process();
		}
	}

	public static void PostMessage(string Message, Action<bool> CompletionCallback)
	{
		Job_PostMessage job_PostMessage = new Job_PostMessage(CompletionCallback, Message);
		job_PostMessage.Process();
	}

	public static void PostScreenshot(string Message, Action<bool> CompletionCallback)
	{
		Job_PostScreenshot job_PostScreenshot = new Job_PostScreenshot(CompletionCallback, Message);
		job_PostScreenshot.Process();
	}

	private static void AddJob(object Job)
	{
		m_Jobs.Add(Job);
	}

	private static void RemoveJob(object Job)
	{
		m_Jobs.Remove(Job);
	}

	[Conditional("DEBUG_LOG")]
	public static void Log(object Msg)
	{
	}
}
