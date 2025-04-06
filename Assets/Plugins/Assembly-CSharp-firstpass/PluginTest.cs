using System.Collections.Generic;
using UnityEngine;

public class PluginTest
{
	public static void OnGUI()
	{
		float num = 120f;
		float num2 = 30f;
		float num3 = 50f;
		float num4 = 100f;
		float num5 = num4;
		/*if (GUI.Button(new Rect(num3, num4, num, num2), "TW Login"))
		{
			TwitterUtils.LogIn(WaitForBool);
		}
		if (GUI.Button(new Rect(num3 + num + 10f, num4, num, num2), "TW Logout"))
		{
			TwitterWrapper.LogOut();
		}
		num4 += num2 + 10f;
		if (!GUI.Button(new Rect(num3, num4, num, num2), "TW IsLogged") || TwitterWrapper.IsLoggedIn())
		{
		}
		num4 += num2 + 10f;
		if (GUI.Button(new Rect(num3, num4, num, num2), "TW Post ScrShot"))
		{
			TwitterUtils.PostScreenshot("Some screenshot from SG:DZ.", null);
		}
		if (GUI.Button(new Rect(num3 + num + 10f, num4, num, num2), "TW Post Message"))
		{
			TwitterUtils.PostMessage("Pokusna zpravicka z SG:DZ.", null);
		}
		num4 += num2 + 10f;
		if (GUI.Button(new Rect(num3, num4, num, num2), "TW UserID"))
		{
			TwitterUtils.GetUserID(WaitForString);
		}
		num4 += num2 + 10f;
		if (GUI.Button(new Rect(num3, num4, num, num2), "TW Following"))
		{
			TwitterUtils.GetFollowingIDs(WaitForStringList);
		}
		num4 += num2 + 10f;
		if (GUI.Button(new Rect(num3, num4, num, num2), "TW DoesFollow"))
		{
			TwitterUtils.DoesUserFollow("85328174,428333,428332", WaitForBool);
		}*/
		num3 += 2f * (num + 10f) + 30f;
		num4 = num5;
	}

	private static void WaitForBool(bool Result)
	{
	}

	private static void WaitForInt(int Result)
	{
	}

	private static void WaitForString(string Result)
	{
	}

	private static void WaitForStringList(List<string> Result)
	{
	}

	private static void WaitForTexture(Texture2D Result)
	{
	}
}
