using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MiscUtils
{
	public static System.Random SysRandom = new System.Random();

	public static void Swap<T>(ref T A, ref T B)
	{
		T val = A;
		A = B;
		B = val;
	}

	public static void Sort<T>(ref T A, ref T B) where T : IComparable
	{
		if (A.CompareTo(B) > 0)
		{
			Swap(ref A, ref B);
		}
	}

	public static void Sort<T>(ref T A, ref T B, Comparison<T> Cmp)
	{
		if (Cmp(A, B) > 0)
		{
			Swap(ref A, ref B);
		}
	}

	public static void Sort<T>(ref T A, ref T B, ref T C) where T : IComparable
	{
		if (A.CompareTo(B) > 0)
		{
			Swap(ref A, ref B);
		}
		if (A.CompareTo(C) > 0)
		{
			Swap(ref A, ref C);
		}
		if (B.CompareTo(C) > 0)
		{
			Swap(ref B, ref C);
		}
	}

	public static void Sort<T>(ref T A, ref T B, ref T C, Comparison<T> Cmp)
	{
		if (Cmp(A, B) > 0)
		{
			Swap(ref A, ref B);
		}
		if (Cmp(A, C) > 0)
		{
			Swap(ref A, ref C);
		}
		if (Cmp(B, C) > 0)
		{
			Swap(ref B, ref C);
		}
	}

	public static T RandomEnum<T>()
	{
		T[] array = (T[])Enum.GetValues(typeof(T));
		int num = UnityEngine.Random.Range(0, array.Length);
		return array[num];
	}

	public static T RandomValue<T>(T[] Values)
	{
		if (Values != null)
		{
			return Values[UnityEngine.Random.Range(0, Values.Length)];
		}
		return default(T);
	}

	public static T Create<T>()
	{
		Type typeFromHandle = typeof(T);
		if (typeFromHandle.IsValueType || typeFromHandle == typeof(string))
		{
			return default(T);
		}
		if (typeFromHandle.IsSubclassOf(typeof(UnityEngine.Object)))
		{
			return default(T);
		}
		return (T)Activator.CreateInstance(typeFromHandle, true);
	}

	public static T DeepCopy<T>(T Obj)
	{
		return (T)CreateDeepCopy(Obj);
	}

	private static object CreateDeepCopy(object Obj)
	{
		if (Obj == null)
		{
			return null;
		}
		Type type = Obj.GetType();
		if (type.IsValueType || type == typeof(string))
		{
			return Obj;
		}
		if (type.IsSubclassOf(typeof(UnityEngine.Object)))
		{
			return Obj;
		}
		if (type.IsArray)
		{
			Array array = (Array)Obj;
			Type elementType = type.GetElementType();
			Array array2 = Array.CreateInstance(elementType, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array2.SetValue(CreateDeepCopy(array.GetValue(i)), i);
			}
			return array2;
		}
		object obj = Activator.CreateInstance(type, true);
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		FieldInfo[] array3 = fields;
		foreach (FieldInfo fieldInfo in array3)
		{
			if (!fieldInfo.FieldType.IsPrimitive && fieldInfo.FieldType != typeof(string))
			{
				object value = CreateDeepCopy(fieldInfo.GetValue(Obj));
				fieldInfo.SetValue(obj, value);
			}
			else
			{
				fieldInfo.SetValue(obj, fieldInfo.GetValue(Obj));
			}
		}
		return obj;
	}

	public static List<Type> GetSubClasses(Assembly[] Assemblies, Type Base, bool IncludeBase, bool IncludeAbstract)
	{
		if (Base == null)
		{
			return null;
		}
		List<Type> list = new List<Type>();
		if (IncludeBase)
		{
			list.Add(Base);
		}
		if (Assemblies == null)
		{
			GetSubClasses(Assembly.GetExecutingAssembly(), Base, IncludeAbstract, list);
		}
		else
		{
			foreach (Assembly asm in Assemblies)
			{
				GetSubClasses(asm, Base, IncludeAbstract, list);
			}
		}
		return list;
	}

	private static void GetSubClasses(Assembly Asm, Type Base, bool IncludeAbstract, List<Type> SubClasses)
	{
		Type[] types = Asm.GetTypes();
		foreach (Type type in types)
		{
			if (type != Base && Base.IsAssignableFrom(type))
			{
				AddSubClass(type, IncludeAbstract, SubClasses);
			}
		}
	}

	private static void AddSubClass(Type T, bool IncludeAbstract, List<Type> SubClasses)
	{
		if (!T.IsAbstract || IncludeAbstract)
		{
			SubClasses.Add(T);
		}
	}
}
