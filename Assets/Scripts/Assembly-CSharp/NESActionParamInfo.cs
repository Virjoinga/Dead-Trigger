using System;
using UnityEngine;

[Serializable]
public class NESActionParamInfo
{
	private enum E_Type
	{
		Unsupported = -1,
		None = 0,
		Bool = 1,
		Int = 2,
		Float = 3,
		String = 4,
		UnityObject = 5
	}

	[SerializeField]
	private E_Type m_Type;

	[SerializeField]
	private string m_Value_String;

	[SerializeField]
	private float m_Value_Float;

	[SerializeField]
	private int m_Value_Int;

	[SerializeField]
	private bool m_Value_Bool;

	[SerializeField]
	private UnityEngine.Object m_Value_UnityObject;

	public NESActionParamInfo()
	{
	}

	public NESActionParamInfo(Type inType)
	{
		Clear();
		m_Type = Convert(inType);
	}

	public void SetValue(object inValue)
	{
		Clear();
		E_Type valueType = GetValueType(inValue);
		switch (valueType)
		{
		case E_Type.Bool:
			m_Value_Bool = (bool)inValue;
			break;
		case E_Type.Int:
			m_Value_Int = (int)inValue;
			break;
		case E_Type.Float:
			m_Value_Float = (float)inValue;
			break;
		case E_Type.String:
			m_Value_String = (string)inValue;
			break;
		case E_Type.UnityObject:
			m_Value_UnityObject = (UnityEngine.Object)inValue;
			break;
		default:
			Debug.LogWarning("NESActionParamInfo - Unsupported parameter type !!!");
			break;
		case E_Type.None:
			break;
		}
		if (valueType != E_Type.Unsupported)
		{
			m_Type = valueType;
		}
	}

	public object GetValue()
	{
		switch (m_Type)
		{
		case E_Type.None:
			return null;
		case E_Type.Bool:
			return m_Value_Bool;
		case E_Type.Int:
			return m_Value_Int;
		case E_Type.Float:
			return m_Value_Float;
		case E_Type.String:
			return m_Value_String;
		case E_Type.UnityObject:
			return m_Value_UnityObject;
		default:
			Debug.LogError("Unknown param type !!!");
			return null;
		}
	}

	public Type GetValueType()
	{
		return Convert(m_Type);
	}

	public static bool IsAccaplableType(Type inType)
	{
		E_Type e_Type = Convert(inType);
		return e_Type != E_Type.Unsupported && e_Type != E_Type.None;
	}

	private void Clear()
	{
		m_Type = E_Type.None;
		m_Value_String = null;
		m_Value_Float = 0f;
		m_Value_Int = 0;
		m_Value_Bool = false;
		m_Value_UnityObject = null;
	}

	private static E_Type GetValueType(object inValue)
	{
		return (inValue != null) ? Convert(inValue.GetType()) : E_Type.Unsupported;
	}

	private static Type Convert(E_Type inType)
	{
		switch (inType)
		{
		case E_Type.None:
			return null;
		case E_Type.Bool:
			return typeof(bool);
		case E_Type.Int:
			return typeof(int);
		case E_Type.Float:
			return typeof(float);
		case E_Type.String:
			return typeof(string);
		case E_Type.UnityObject:
			return typeof(UnityEngine.Object);
		default:
			Debug.LogError("Unknown param type !!!");
			return null;
		}
	}

	private static E_Type Convert(Type inType)
	{
		if (inType == null)
		{
			return E_Type.None;
		}
		if (inType == typeof(bool))
		{
			return E_Type.Bool;
		}
		if (inType == typeof(int))
		{
			return E_Type.Int;
		}
		if (inType == typeof(float))
		{
			return E_Type.Float;
		}
		if (inType == typeof(string))
		{
			return E_Type.String;
		}
		if (inType == typeof(UnityEngine.Object) || inType.IsSubclassOf(typeof(UnityEngine.Object)))
		{
			return E_Type.UnityObject;
		}
		return E_Type.Unsupported;
	}
}
