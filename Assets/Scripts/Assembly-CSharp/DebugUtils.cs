using System;
using System.Diagnostics;
using UnityEngine;

public class DebugUtils
{
	public const string alpha_big_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public const string alpha_small_chars = "abcdefghijklmnopqrstuvwxyz";

	public const string numbers_chars = "0123456789";

	public const string alpha_num_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	[Conditional("DEBUG")]
	public static void Assert(bool Condition)
	{
		if (!Condition)
		{
			//throw new Exception();
		}
	}

	public static string GetFullName(GameObject Obj)
	{
		if (Obj != null)
		{
			if (Obj.transform.parent != null)
			{
				return GetFullName(Obj.transform.parent.gameObject) + "/" + Obj.name;
			}
			return Obj.name;
		}
		return string.Empty;
	}

	public static void GetMethodCallerInfo(out string File, out string Method, out int Line, int StackOffset = 2)
	{
		StackFrame stackFrame = new StackFrame(StackOffset, true);
		Method = stackFrame.GetMethod().Name;
		File = stackFrame.GetFileName();
		File = File.Substring(File.LastIndexOf("\\") + 1);
		Line = stackFrame.GetFileLineNumber();
	}

	public static void DumpCallstack()
	{
		StackTrace stackTrace = new StackTrace(true);
		StackFrame[] frames = stackTrace.GetFrames();
		foreach (StackFrame stackFrame in frames)
		{
            UnityEngine.Debug.Log(string.Concat("File :", stackFrame.GetFileName(), ", ", stackFrame.GetMethod(), " ", stackFrame.GetFileLineNumber()));
		}
	}

	public static string GetRandomString(int Size, string AvailibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
	{
		char[] array = new char[Size];
		for (int i = 0; i < Size; i++)
		{
			array[i] = AvailibleChars[UnityEngine.Random.Range(0, AvailibleChars.Length)];
		}
		return new string(array);
	}
}
