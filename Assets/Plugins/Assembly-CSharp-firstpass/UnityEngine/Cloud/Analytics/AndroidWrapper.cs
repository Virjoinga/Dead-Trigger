using System;
using System.Security.Cryptography;
using System.Text;

namespace UnityEngine.Cloud.Analytics
{
	internal class AndroidWrapper : BasePlatformWrapper
	{
		//public override string appVersion
		//{
		//	get
		//	{
		//		string text = null;
		//		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
		//		{
		//			return androidJavaClass.CallStatic<string>("getAppVersion", new object[0]);
		//		}
		//	}
		//}
		//
		//public override string appBundleIdentifier
		//{
		//	get
		//	{
		//		string text = null;
		//		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
		//		{
		//			return androidJavaClass.CallStatic<string>("getAppPackageName", new object[0]);
		//		}
		//	}
		//}
		//
		//public override string appInstallMode
		//{
		//	get
		//	{
		//		string text = null;
		//		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
		//		{
		//			return androidJavaClass.CallStatic<string>("getAppInstallMode", new object[0]);
		//		}
		//	}
		//}
		//
		//public override bool isRootedOrJailbroken
		//{
		//	get
		//	{
		//		bool flag = false;
		//		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unityengine.cloud.AppUtil"))
		//		{
		//			return androidJavaClass.CallStatic<bool>("isDeviceRooted", new object[0]);
		//		}
		//	}
		//}

		/*public override string deviceUniqueIdentifier
		{
			get
			{
				try
				{
					//AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					//AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					//AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
					string static2 = androidJavaClass2.GetStatic<string>("ANDROID_ID");
					//string input = androidJavaClass2.CallStatic<string>("getString", new object[2] { androidJavaObject, static2 });
					//return Md5Hex(input);
				}
				catch (AndroidJavaException)
				{
					return string.Empty;
				}
				catch (SystemException)
				{
					return string.Empty;
				}
			}
		}*/

		private string Md5Hex(string input)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(input);
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, '0');
			}
			return text.PadLeft(32, '0');
		}
	}
}
