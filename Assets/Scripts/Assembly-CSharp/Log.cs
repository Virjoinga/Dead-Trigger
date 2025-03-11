using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public static class Log
{
	public const int NamelessGroup = 0;

	private static HashSet<int> m_Enabled;

	private static StringBuilder m_Message;

	private static int m_StackOffset;

	static Log()
	{
		m_Enabled = new HashSet<int>();
		m_Message = new StringBuilder(512);
		EnableGroup(0);
	}

	public static void EnableGroup(int Group)
	{
		m_Enabled.Add(Group);
	}

	public static void DisableGroup(int Group)
	{
		m_Enabled.Remove(Group);
	}

	[Conditional("LOG_INFOS")]
	public static void Info(object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		m_StackOffset++;
	}

	[Conditional("LOG_INFOS")]
	public static void Info(int Group, object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		if (IsEnabled(Group))
		{
			m_StackOffset++;
			ConstructMessage(Msg, WithTimeStamp);
			UnityEngine.Debug.Log(m_Message.ToString(), Owner);
		}
		m_StackOffset = 0;
	}

	[Conditional("LOG_WARNINGS")]
	public static void Warning(object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		m_StackOffset++;
	}

	[Conditional("LOG_WARNINGS")]
	public static void Warning(int Group, object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		if (IsEnabled(Group))
		{
			m_StackOffset++;
			ConstructMessage(Msg, WithTimeStamp);
			UnityEngine.Debug.LogWarning(m_Message.ToString(), Owner);
		}
		m_StackOffset = 0;
	}

	[Conditional("LOG_ERRORS")]
	public static void Error(object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		m_StackOffset++;
	}

	[Conditional("LOG_ERRORS")]
	public static void Error(int Group, object Msg, Object Owner = null, bool WithTimeStamp = true)
	{
		if (IsEnabled(Group))
		{
			m_StackOffset++;
			ConstructMessage(Msg, WithTimeStamp);
			UnityEngine.Debug.LogError(m_Message.ToString(), Owner);
		}
		m_StackOffset = 0;
	}

	private static bool IsEnabled(int Group)
	{
		return m_Enabled.Contains(Group);
	}

	private static void ConstructMessage(object Msg, bool AddTime)
	{
		bool flag = false;
		m_Message.Length = 0;
		if (AddTime)
		{
			flag = true;
			float timeSinceLevelLoad = Time.timeSinceLevelLoad;
			int num = Mathf.FloorToInt(timeSinceLevelLoad / 3600f);
			timeSinceLevelLoad -= (float)num * 3600f;
			int num2 = Mathf.FloorToInt(timeSinceLevelLoad / 60f);
			timeSinceLevelLoad -= (float)num2 * 60f;
			m_Message.AppendFormat("[ {0:D2}:{1:D2}:{2:00.000} ] ", num, num2, timeSinceLevelLoad);
		}
		if (flag)
		{
			m_Message.Append("  ");
		}
		m_Message.Append(Msg);
	}
}
