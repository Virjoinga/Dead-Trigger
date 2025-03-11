using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AndroidRewardSystem
{
	private const string salt = "H2CF893476IQD56IQBT8C5CQIB5C9A5W";

	private static string Modify(string input)
	{
		char[] array = new char[input.Length];
		int num = 0;
		int num2 = array.Length - 1;
		while (num <= num2)
		{
			array[num] = input[num2];
			array[num2] = input[num];
			num++;
			num2--;
		}
		return new string(array);
	}

	public static bool CanReceiveReward()
	{
		List<byte[]> list = new List<byte[]>();
		//AndroidAccountManager.AccountsList accountsByType = AndroidAccountManager.GetAccountsByType("com.google");
		/*foreach (AndroidAccountManager.Account item in accountsByType)
		{
			list.Add(CryptoUtils.CalcMD5HashAsBytes(("H2CF893476IQD56IQBT8C5CQIB5C9A5W" + item.Name + Modify("H2CF893476IQD56IQBT8C5CQIB5C9A5W")).ToLower()));
		}*/
		TextAsset textAsset = Resources.Load("Other/RwSplashBin") as TextAsset;
		MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
		int num = 16;
		byte[] array = new byte[num];
		while (memoryStream.Read(array, 0, num) == num)
		{
			foreach (byte[] item2 in list)
			{
				if (ByteArrayCompare(item2, array))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool ByteArrayCompare(byte[] a1, byte[] a2)
	{
		if (a1.Length != a2.Length)
		{
			return false;
		}
		for (int i = 0; i < a1.Length; i++)
		{
			if (a1[i] != a2[i])
			{
				return false;
			}
		}
		return true;
	}
}
