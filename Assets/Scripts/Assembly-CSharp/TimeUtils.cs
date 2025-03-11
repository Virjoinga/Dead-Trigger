using System;
using UnityEngine;

public class TimeUtils
{
	public static double GetCurrentTimeInSeconds()
	{
		return DateTime.Now.Ticks / 10000000;
	}

	public static string SecondsToString_HHMMSS(float timeSeconds)
	{
		int num = Mathf.FloorToInt(timeSeconds / 3600f);
		timeSeconds -= (float)(num * 3600);
		int num2 = Mathf.FloorToInt(timeSeconds / 60f);
		timeSeconds -= (float)(num2 * 60);
		int num3 = Mathf.RoundToInt(timeSeconds);
		string text = string.Empty;
		if (num < 10)
		{
			text += "0";
		}
		text += num;
		text += ":";
		if (num2 < 10)
		{
			text += "0";
		}
		text += num2;
		text += ":";
		if (num3 < 10)
		{
			text += "0";
		}
		return text + num3;
	}

	public static string SecondsToString_MMSS(float timeSeconds)
	{
		int num = Mathf.FloorToInt(timeSeconds / 60f);
		int num2 = Mathf.RoundToInt(timeSeconds) % num;
		if (num < 10)
		{
			if (num2 < 10)
			{
				return "0" + num + ":0" + num2;
			}
			return "0" + num + ":" + num2;
		}
		if (num2 < 10)
		{
			return num + ":0" + num2;
		}
		return num + ":" + num2;
	}
}
