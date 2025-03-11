using System;
using System.Collections.Generic;

namespace UnityEngine.Cloud.Analytics
{
	internal class BasePlatformWrapper : IPlatformWrapper, IApplication, ISystem, ISystemInfo, IWWWFactory
	{
		private System.Random m_Random;

		public virtual string appVersion
		{
			get
			{
				return null;
			}
		}

		public virtual string appBundleIdentifier
		{
			get
			{
				return null;
			}
		}

		public virtual string appInstallMode
		{
			get
			{
				return null;
			}
		}

		public virtual bool isRootedOrJailbroken
		{
			get
			{
				return false;
			}
		}

		public virtual string deviceMake
		{
			get
			{
				return Application.platform.ToString();
			}
		}

		public virtual bool isNetworkReachable
		{
			get
			{
				return Application.internetReachability != NetworkReachability.NotReachable;
			}
		}

		public virtual bool isWebPlayer
		{
			get
			{
				//return Application.isWebPlayer;
				return false;
			}
		}

		public virtual bool isEditor
		{
			get
			{
				return Application.isEditor;
			}
		}

		public virtual int levelCount
		{
			get
			{
				return Application.levelCount;
			}
		}

		public virtual int loadedLevel
		{
			get
			{
				return Application.loadedLevel;
			}
		}

		public virtual string loadedLevelName
		{
			get
			{
				return Application.loadedLevelName;
			}
		}

		public virtual string persistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		public virtual string platformName
		{
			get
			{
				switch (Application.platform)
				{
				case RuntimePlatform.OSXEditor:
					return "editor-mac";
				case RuntimePlatform.OSXPlayer:
					return "mac";
				case RuntimePlatform.WindowsPlayer:
					return "win";
				/*case RuntimePlatform.OSXWebPlayer:
					return "web-mac";
				case RuntimePlatform.OSXDashboardPlayer:
					return "dash-mac";
				case RuntimePlatform.WindowsWebPlayer:
					return "web-win";*/
				case RuntimePlatform.WindowsEditor:
					return "editor-win";
				case RuntimePlatform.IPhonePlayer:
					return "ios";
				case RuntimePlatform.XBOX360:
					return "xbox360";
				case RuntimePlatform.PS3:
					return "ps3";
				case RuntimePlatform.Android:
					return "android";
				case RuntimePlatform.LinuxPlayer:
					return "linux";
				default:
					return "unity";
				}
			}
		}

		public virtual string unityVersion
		{
			get
			{
				return Application.unityVersion;
			}
		}

		public virtual string deviceModel
		{
			get
			{
				return SystemInfo.deviceModel;
			}
		}

		public virtual string deviceUniqueIdentifier
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual string operatingSystem
		{
			get
			{
				return SystemInfo.operatingSystem;
			}
		}

		public virtual string processorType
		{
			get
			{
				return SystemInfo.processorType;
			}
		}

		public virtual int systemMemorySize
		{
			get
			{
				return SystemInfo.systemMemorySize;
			}
		}

		internal BasePlatformWrapper()
		{
			m_Random = new System.Random();
		}

		public long GetLongRandom()
		{
			byte[] array = new byte[8];
			m_Random.NextBytes(array);
			return (long)(BitConverter.ToUInt64(array, 0) & 0x7FFFFFFFFFFFFFFFL);
		}

		public IWWW newWWW(string url, byte[] body, Dictionary<string, string> headers)
		{
			WWW www = new WWW(url, body, headers);
			return new UnityWWW(www);
		}
	}
}
