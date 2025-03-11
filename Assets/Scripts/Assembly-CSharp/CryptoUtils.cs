using System;
using System.Security.Cryptography;
using System.Text;

public class CryptoUtils
{
	private static SHA1CryptoServiceProvider ms_SHA1Provider = new SHA1CryptoServiceProvider();

	private static MD5CryptoServiceProvider ms_MD5Provider = new MD5CryptoServiceProvider();

	public static string CalcSHA1Hash(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = ms_SHA1Provider.ComputeHash(bytes);
		return BitConverter.ToString(array).Replace("-", string.Empty);
	}

	public static string CalcMD5Hash(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = ms_MD5Provider.ComputeHash(bytes);
		return BitConverter.ToString(array).Replace("-", string.Empty);
	}

	public static byte[] CalcMD5HashAsBytes(string input)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		return ms_MD5Provider.ComputeHash(bytes);
	}
}
