public class FNVHash
{
	public static int CalcFNVHash(string str)
	{
		uint num = 2166136261u;
		char[] array = str.ToCharArray();
		char[] array2 = array;
		foreach (char c in array2)
		{
			num += (num << 1) + (num << 4) + (num << 7) + (num << 8) + (num << 24);
			num ^= c;
		}
		return (int)num;
	}

	public static int CalcModFNVHash(string str)
	{
		char[] array = str.ToCharArray();
		uint num = 2166136261u;
		char[] array2 = array;
		foreach (char c in array2)
		{
			num = (num ^ c) * 16777619;
		}
		num += num << 13;
		num ^= num >> 7;
		num += num << 3;
		num ^= num >> 17;
		return (int)(num + (num << 5));
	}
}
