using System.Net.NetworkInformation;
using UnityEngine;

public class SysUtils
{
	public static string GetUniqueDeviceID()
	{
		return CryptoUtils.CalcSHA1Hash(SystemInfo.deviceUniqueIdentifier);
	}

	public static string GetMacAddress()
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		int num = ((allNetworkInterfaces != null) ? allNetworkInterfaces.Length : 0);
		for (int i = 0; i < num; i++)
		{
			NetworkInterface networkInterface = allNetworkInterfaces[i];
			PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
			if (physicalAddress.ToString() != string.Empty)
			{
				return physicalAddress.ToString();
			}
		}
		return string.Empty;
	}
}
