using System;
using UnityEngine;

public class BuildInfo : MonoBehaviour
{
	[Serializable]
	public class VersionInfo
	{
		public int Major;

		public int Minor;

		public int Build;

		public int Revision;

		public int Code;

		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Build, Revision);
		}

		public string Version()
		{
			return string.Format("{0}.{1}.{2}", Major, Minor, Build);
		}
	}

	[Serializable]
	public class DateInfo
	{
		public int Year;

		public int Month;

		public int Day;

		public int Hour;

		public int Minute;

		public int Second;

		public override string ToString()
		{
			return new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc).ToString();
		}
	}

	[SerializeField]
	private VersionInfo versionInfo;

	[SerializeField]
	private DateInfo dateInfo;

	private static bool infoPrinted;

	public static BuildInfo Instance
	{
		get
		{
			GameObject gameObject = GameObject.Find("/BuildInfo");
			return (!(gameObject != null)) ? null : gameObject.GetComponent<BuildInfo>();
		}
	}

	public VersionInfo Version
	{
		get
		{
			return versionInfo;
		}
	}

	public DateInfo Date
	{
		get
		{
			return dateInfo;
		}
	}

	public void Awake()
	{
		if (!infoPrinted)
		{
			Print();
			infoPrinted = true;
		}
	}

	public string FormatBuildInfo()
	{
		string[] value = new string[4]
		{
			"BuildInfo :",
			"   BuildInfo.Version : " + Version,
			"   BuildInfo.Code    : " + Version.Code,
			"   BuildInfo.Date    : " + Date
		};
		return string.Join(Environment.NewLine, value);
	}

	public string FormatSystemInfo()
	{
		string[] value = new string[19]
		{
			"SystemInfo :",
			"   SystemInfo.operatingSystem        : " + SystemInfo.operatingSystem,
			"   SystemInfo.processorType          : " + SystemInfo.processorType,
			"   SystemInfo.processorCount         : " + SystemInfo.processorCount,
			"   SystemInfo.systemMemorySize       : " + SystemInfo.systemMemorySize,
			"   SystemInfo.graphicsMemorySize     : " + SystemInfo.graphicsMemorySize,
			"   SystemInfo.graphicsDeviceName     : " + SystemInfo.graphicsDeviceName,
			"   SystemInfo.graphicsDeviceVendor   : " + SystemInfo.graphicsDeviceVendor,
			"   SystemInfo.graphicsDeviceID       : " + SystemInfo.graphicsDeviceID,
			"   SystemInfo.graphicsDeviceVendorID : " + SystemInfo.graphicsDeviceVendorID,
			"   SystemInfo.graphicsDeviceVersion  : " + SystemInfo.graphicsDeviceVersion,
			"   SystemInfo.graphicsShaderLevel    : " + SystemInfo.graphicsShaderLevel,
			"   SystemInfo.graphicsPixelFillrate  : " + SystemInfo.graphicsPixelFillrate,
			"   SystemInfo.supportsShadows        : " + SystemInfo.supportsShadows,
			"   SystemInfo.supportsRenderTextures : " + SystemInfo.supportsRenderTextures,
			"   SystemInfo.supportsImageEffects   : " + SystemInfo.supportsImageEffects,
			"   SystemInfo.deviceUniqueIdentifier : " + SystemInfo.deviceUniqueIdentifier,
			"   SystemInfo.deviceName             : " + SystemInfo.deviceName,
			"   SystemInfo.deviceModel            : " + SystemInfo.deviceModel
		};
		return string.Join(Environment.NewLine, value);
	}

	public void Print()
	{
		string[] value = new string[2]
		{
			"----------------------------------------------------------------------",
			FormatBuildInfo()
		};
		MonoBehaviour.print(string.Join(Environment.NewLine, value));
		string[] value2 = new string[2]
		{
			FormatSystemInfo(),
			"----------------------------------------------------------------------"
		};
		MonoBehaviour.print(string.Join(Environment.NewLine, value2));
	}
}
