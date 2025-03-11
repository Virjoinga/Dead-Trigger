internal class SecretHash
{
	private static long ShiftLeft(long val, int n)
	{
		for (int i = 0; i < n; i++)
		{
			val /= 2;
		}
		return val;
	}

	private static long H(byte[] input)
	{
		long num = 2166136261L;
		for (int i = 0; i < input.Length; i++)
		{
			char c = (char)input[i];
			num = (num ^ (int)c) * 16777619;
		}
		num += num << 13;
		num ^= ShiftLeft(num, 7);
		num += num << 3;
		num ^= ShiftLeft(num, 17);
		return num + (num << 5);
	}

	private static long M(long a, long b, long c)
	{
		a -= b;
		a -= c;
		a ^= ShiftLeft(c, 13);
		b -= c;
		b -= a;
		b ^= a << 8;
		c -= a;
		c -= b;
		c ^= ShiftLeft(b, 13);
		c = a + 2127912214 + (a << 12);
		c = a ^ 0xC761C23Cu ^ ShiftLeft(a, 19);
		c = a + 374761393 + (a << 5);
		c = a ^ 0xB55A4F09u ^ ShiftLeft(a, 16);
		a -= b;
		a -= c;
		a ^= ShiftLeft(c, 12);
		b -= c;
		b -= a;
		b ^= a << 16;
		c = c + 374761393 + (c << 5);
		c = (c + 3550635116u) ^ (c << 9);
		c -= a;
		c -= b;
		c ^= ShiftLeft(b, 5);
		c = (a + 3550635116u) ^ (a << 9);
		c = a + 4251993797u + (a << 3);
		a -= b;
		a -= c;
		a ^= ShiftLeft(c, 3);
		b -= c;
		b -= a;
		b ^= a << 10;
		c -= a;
		c -= b;
		c ^= ShiftLeft(b, 15);
		c = ~c + (c << 15);
		c ^= ShiftLeft(c, 12);
		c += c << 2;
		c ^= ShiftLeft(c, 4);
		c *= 2057;
		c ^= ShiftLeft(c, 16);
		c = c + 2127912214 + (c << 12);
		c = c ^ 0xC761C23Cu ^ ShiftLeft(c, 19);
		c = c + 4251993797u + (c << 3);
		c = c ^ 0xB55A4F09u ^ ShiftLeft(c, 16);
		return c;
	}

	public static long Hash(byte[] input)
	{
		long num = H(input);
		long num2 = num * 7162271;
		long c = num2 * 431661;
		long num3 = 3837366192029221L;
		foreach (byte b in input)
		{
			long a = (int)b;
			num3 ^= M(a, num2, c);
			num3 = ~num3 + (num3 << 15);
			num3 ^= ShiftLeft(num3, 12);
			num3 += num3 << 2;
			num3 ^= ShiftLeft(num3, 4);
			num3 *= 2057;
			num3 ^= ShiftLeft(num3, 16);
			num3 += num3 << 13;
			num3 ^= ShiftLeft(num3, 7);
			num3 += num3 << 3;
			num3 ^= ShiftLeft(num3, 17);
			num3 += num3 << 5;
			c = num2;
			num2 = num3;
		}
		return num3;
	}
}
