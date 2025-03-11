using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class DataFileJSON : IDataFile
{
	private Dictionary<string, object> m_Data = new Dictionary<string, object>();

	public DataFileJSON(string data = null)
	{
		if (data != null && data.Length > 0)
		{
			m_Data = JsonMapper.ToObject<Dictionary<string, object>>(data);
		}
	}

	public override void SetInt(string key, int value)
	{
		m_Data[key] = value;
	}

	public override int GetInt(string key, int defaultValue = 0)
	{
		object value;
		if (m_Data.TryGetValue(key, out value) && value is int)
		{
			return (int)value;
		}
		return defaultValue;
	}

	public override void SetFloat(string key, float value)
	{
		m_Data[key] = value;
	}

	public override float GetFloat(string key, float defaultValue = 0f)
	{
		object value;
		if (m_Data.TryGetValue(key, out value))
		{
			if (value is float)
			{
				return (float)value;
			}
			if (value is double)
			{
				double num = (double)value;
				return (float)num;
			}
		}
		return defaultValue;
	}

	public override void SetString(string key, string value)
	{
		m_Data[key] = value;
	}

	public override string GetString(string key, string defaultValue = "")
	{
		object value;
		if (m_Data.TryGetValue(key, out value) && value is string)
		{
			return (string)value;
		}
		return defaultValue;
	}

	public override bool KeyExists(string key)
	{
		return m_Data.ContainsKey(key);
	}

	public override string ToString()
	{
		return JsonMapper.ToJson(m_Data);
	}

	public bool InitFromString(string inJSON)
	{
		try
		{
			m_Data = JsonMapper.ToObject<Dictionary<string, object>>(inJSON);
		}
		catch (JsonException ex)
		{
			Debug.LogError("JSON exception caught: " + ex.Message);
			m_Data = new Dictionary<string, object>();
			return false;
		}
		return true;
	}
}
