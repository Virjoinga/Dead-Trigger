using UnityEngine;

public class MFNativeUtils
{
	private static AndroidJavaClass ms_AndroidMFNativeUtils;

	public static int AppIconBadgeNumber { get; set; }

	static MFNativeUtils()
	{
		//ms_AndroidMFNativeUtils = new AndroidJavaClass("com.madfingergames.android.utils.MFNativeUtils");
	}

	public static void OpenURLExternal(string url)
	{
		ms_AndroidMFNativeUtils.CallStatic("openURLExternal", url);
	}

	public static string GetPhoneNumber()
	{
		return ms_AndroidMFNativeUtils.CallStatic<string>("getPhoneNumber", new object[0]);
	}

	public static string GetDeviceId()
	{
		return ms_AndroidMFNativeUtils.CallStatic<string>("getDeviceId", new object[0]);
	}
}
