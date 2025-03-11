using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class NESUtils
{
	internal static BindingFlags _commonBindingFlags
	{
		get
		{
			return BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		}
	}

	internal static bool IsWorkAroundComponent(Component inComponent)
	{
		return inComponent is Animation || inComponent is AudioSource;
	}

	internal static bool IsNESSupportedParamType(Type inType)
	{
		return NESActionParamInfo.IsAccaplableType(inType);
	}

	public static bool HasEvents(GameObject inObject)
	{
		MonoBehaviour[] components = inObject.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour inComponent in components)
		{
			if (HasEvents(inComponent))
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasEvents(Component inComponent)
	{
		return inComponent.GetType().IsDefined(typeof(NESEventAttribute), true);
	}

	public static string[] GetEvents(Component inComponent)
	{
		List<string> list = new List<string>();
		object[] customAttributes = inComponent.GetType().GetCustomAttributes(typeof(NESEventAttribute), true);
		foreach (object obj in customAttributes)
		{
			NESEventAttribute nESEventAttribute = (NESEventAttribute)obj;
			if (nESEventAttribute != null && nESEventAttribute.events.Length > 0)
			{
				list.AddRange(nESEventAttribute.events);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public static bool HasEvent(Component inComponent, string inEvent)
	{
		string[] events = GetEvents(inComponent);
		if (events != null && events.Length > 0)
		{
			return Array.IndexOf(events, inEvent) >= 0;
		}
		return false;
	}

	public static List<Component> GetGameLogicEventComponents(GameObject inGameObject)
	{
		List<Component> list = new List<Component>();
		Component[] components = inGameObject.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (HasEvents(component))
			{
				list.Add(component);
			}
		}
		return list;
	}

	public static bool HasActions(GameObject inObject)
	{
		return HasActions(inObject, false);
	}

	public static bool HasActions(GameObject inObject, bool inAcceptNESWorkAround)
	{
		if (!inAcceptNESWorkAround)
		{
			MonoBehaviour[] components = inObject.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour inComponent in components)
			{
				if (HasActions(inComponent))
				{
					return true;
				}
			}
		}
		else
		{
			Component[] components2 = inObject.GetComponents<Component>();
			foreach (Component inComponent2 in components2)
			{
				if (IsWorkAroundComponent(inComponent2) || HasActions(inComponent2))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool HasActions(Component inComponent)
	{
		MethodInfo[] methods = inComponent.GetType().GetMethods(_commonBindingFlags);
		foreach (MethodInfo methodInfo in methods)
		{
			if (methodInfo.IsDefined(typeof(NESActionAttribute), false))
			{
				return true;
			}
		}
		return false;
	}

	public static string[] GetActions(Component inComponent)
	{
		List<string> list = new List<string>();
		MethodInfo[] methods = inComponent.GetType().GetMethods(_commonBindingFlags);
		foreach (MethodInfo methodInfo in methods)
		{
			if (Attribute.IsDefined(methodInfo, typeof(NESActionAttribute)))
			{
				list.Add(methodInfo.Name);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public static bool HasAction(Component inComponent, string inAction)
	{
		string[] actions = GetActions(inComponent);
		if (actions != null && actions.Length > 0)
		{
			return Array.IndexOf(actions, inAction) >= 0;
		}
		return false;
	}

	public static string[] GetActionNames(Component inComponent)
	{
		List<string> list = new List<string>();
		MethodInfo[] methods = inComponent.GetType().GetMethods(_commonBindingFlags);
		foreach (MethodInfo methodInfo in methods)
		{
			NESActionAttribute nESActionAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(NESActionAttribute), false) as NESActionAttribute;
			if (nESActionAttribute != null)
			{
				if (nESActionAttribute.DisplayName != null && nESActionAttribute.DisplayName.Length > 0)
				{
					list.Add(nESActionAttribute.DisplayName);
				}
				else
				{
					list.Add(methodInfo.Name);
				}
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public static List<Component> GetGameLogicActionComponents(GameObject inGameObject)
	{
		return GetGameLogicActionComponents(inGameObject, false);
	}

	public static List<Component> GetGameLogicActionComponents(GameObject inGameObject, bool inAcceptNESWorkAround)
	{
		List<Component> list = new List<Component>();
		Component[] components = inGameObject.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (HasActions(component) || (inAcceptNESWorkAround && IsWorkAroundComponent(component)))
			{
				list.Add(component);
			}
		}
		return list;
	}

	public static bool GetAction1stParameterInfo(Component inComponent, string inActionName, out Type out1stParamType, out string[] outAvailibleNames)
	{
		out1stParamType = null;
		outAvailibleNames = null;
		if (inComponent == null || string.IsNullOrEmpty(inActionName))
		{
			return false;
		}
		MethodInfo method = inComponent.GetType().GetMethod(inActionName, _commonBindingFlags);
		if (method == null)
		{
			return false;
		}
		ParameterInfo[] parameters = method.GetParameters();
		if (parameters.Length > 1)
		{
			return false;
		}
		if (parameters.Length == 0)
		{
			return true;
		}
		out1stParamType = parameters[0].ParameterType;
		if (!IsNESSupportedParamType(out1stParamType))
		{
			out1stParamType = null;
			return false;
		}
		object[] customAttributes = method.GetCustomAttributes(typeof(NESActionAttribute), false);
		if (customAttributes == null && customAttributes.Length != 1)
		{
			return false;
		}
		NESActionAttribute nESActionAttribute = customAttributes[0] as NESActionAttribute;
		if (nESActionAttribute != null && !string.IsNullOrEmpty(nESActionAttribute.Argument1))
		{
			MethodInfo method2 = inComponent.GetType().GetMethod(nESActionAttribute.Argument1, _commonBindingFlags);
			if (method2 != null)
			{
				outAvailibleNames = method2.Invoke(inComponent, null) as string[];
			}
		}
		return true;
	}

	public static Type[] GetDerivedClassTypes(Type inBaseType)
	{
		return GetDerivedClassTypes(inBaseType, false);
	}

	public static Type[] GetDerivedClassTypes(Type inBaseType, Assembly[] inAssemblies)
	{
		return GetDerivedClassTypes(inBaseType, false, inAssemblies);
	}

	public static Type[] GetDerivedClassTypes(Type inBaseType, bool inAcceptAbstract)
	{
		return GetDerivedClassTypes(inBaseType, inAcceptAbstract, new Assembly[1] { inBaseType.Assembly });
	}

	public static Type[] GetDerivedClassTypes(Type inBaseType, bool inAcceptAbstract, Assembly[] inAssemblies)
	{
		List<Type> list = new List<Type>();
		foreach (Assembly assembly in inAssemblies)
		{
			Type[] exportedTypes = assembly.GetExportedTypes();
			foreach (Type type in exportedTypes)
			{
				if (type.IsClass && (!type.IsAbstract || inAcceptAbstract) && type.IsSubclassOf(inBaseType))
				{
					list.Add(type);
				}
			}
		}
		list.Sort((Type p1, Type p2) => p1.Name.CompareTo(p2.Name));
		return list.ToArray();
	}
}
