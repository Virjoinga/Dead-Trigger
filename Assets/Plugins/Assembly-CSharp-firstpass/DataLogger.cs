using System.Collections;
using System.Text;
using UnityEngine;

public static class DataLogger
{
	private static readonly string Indent = "    ";

	private static StringBuilder m_StringBuilder = new StringBuilder(2048);

	public static void Log(object obj)
	{
		Log(obj, string.Empty);
	}

	public static void Log(object obj, string prefix)
	{
		Debug.Log("\n");
		if (obj == null)
		{
			m_StringBuilder.AppendFormat("{0}null", prefix);
		}
		else if (obj is IList)
		{
			IList list = obj as IList;
			m_StringBuilder.AppendFormat("{0}i-list [{1}] \n", prefix, list.Count);
			ToString(prefix, list);
		}
		else if (obj is IDictionary)
		{
			IDictionary dictionary = obj as IDictionary;
			m_StringBuilder.AppendFormat("{0}i-dict [{1}] \n", prefix, dictionary.Count);
			ToString(prefix, dictionary);
		}
		else
		{
			m_StringBuilder.AppendFormat("{0}{1} \n", prefix, obj);
		}
		Flush(true);
		Debug.Log("\n");
	}

	private static void ToString(string prefix, IList obj)
	{
		int num = 0;
		string text = prefix + Indent;
		foreach (object item in obj)
		{
			if (item is IList)
			{
				IList list = item as IList;
				m_StringBuilder.AppendFormat("{0}- {1} ... i-list [{2}] \n", prefix, num, list.Count);
				ToString(text, list);
			}
			else if (item is IDictionary)
			{
				IDictionary dictionary = item as IDictionary;
				m_StringBuilder.AppendFormat("{0}- {1} ... i-dict [{2}] \n", prefix, num, dictionary.Count);
				ToString(text, dictionary);
			}
			else
			{
				m_StringBuilder.AppendFormat("{0}- {1} ... {2} \n", prefix, num, item);
			}
			Flush();
			num++;
		}
	}

	private static void ToString(string prefix, IDictionary obj)
	{
		string text = prefix + Indent;
		foreach (DictionaryEntry item in obj)
		{
			if (item.Value is IList)
			{
				IList list = item.Value as IList;
				m_StringBuilder.AppendFormat("{0}- {1} ... i-list [{2}] \n", prefix, item.Key, list.Count);
				ToString(text, list);
			}
			else if (item.Value is IDictionary)
			{
				IDictionary dictionary = item.Value as IDictionary;
				m_StringBuilder.AppendFormat("{0}- {1} ... i-dict [{2}] \n", prefix, item.Key, dictionary.Count);
				ToString(text, dictionary);
			}
			else
			{
				m_StringBuilder.AppendFormat("{0}- {1} : {2} \n", prefix, item.Key, item.Value);
			}
			Flush();
		}
	}

	private static void Flush(bool forced = false)
	{
		if (forced || m_StringBuilder.Length > 2 * m_StringBuilder.Capacity / 3)
		{
			Debug.Log(m_StringBuilder.ToString());
			m_StringBuilder.Length = 0;
		}
	}
}
