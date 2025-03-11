using System.Collections.Generic;

public static class ContainersUtils
{
	public static void Shuffle<T>(this T[] Container)
	{
		int num = Container.Length;
		while (num > 1)
		{
			int num2 = MiscUtils.SysRandom.Next(0, num);
			num--;
			T val = Container[num2];
			Container[num2] = Container[num];
			Container[num] = val;
		}
	}

	public static void Shuffle<T>(this List<T> Container)
	{
		int num = Container.Count;
		while (num > 1)
		{
			int index = MiscUtils.SysRandom.Next(0, num);
			num--;
			T value = Container[index];
			Container[index] = Container[num];
			Container[num] = value;
		}
	}

	public static void AddUnique<T>(this List<T> Container, T Item)
	{
		if (Container.IndexOf(Item) == -1)
		{
			Container.Add(Item);
		}
	}

	public static T PopLast<T>(this List<T> Container)
	{
		int index = Container.Count - 1;
		T result = Container[index];
		Container.RemoveAt(index);
		return result;
	}

	public static void Swap<T>(this T[] Container, int IndexA, int IndexB)
	{
		T val = Container[IndexA];
		Container[IndexA] = Container[IndexB];
		Container[IndexB] = val;
	}

	public static void Swap<T>(this List<T> Container, int IndexA, int IndexB)
	{
		T value = Container[IndexA];
		Container[IndexA] = Container[IndexB];
		Container[IndexB] = value;
	}

	public static T GetRandomItem<T>(this T[] Container)
	{
		if (Container != null && Container.Length > 0)
		{
			return Container[MiscUtils.SysRandom.Next(0, Container.Length)];
		}
		return default(T);
	}

	public static T GetRandomItem<T>(this List<T> Container)
	{
		if (Container != null && Container.Count > 0)
		{
			return Container[MiscUtils.SysRandom.Next(0, Container.Count)];
		}
		return default(T);
	}
}
