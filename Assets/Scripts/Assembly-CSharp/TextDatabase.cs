using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class TextDatabase : ScriptableObject
{
	public struct GameText
	{
		public int m_Index;

		public string m_Text;
	}

	private const SystemLanguage _DefaultLangugae = SystemLanguage.English;

	private Dictionary<int, GameText> _DataBase = new Dictionary<int, GameText>();

	private SystemLanguage _DatabaseLangugae = SystemLanguage.Unknown;

	private int _ReloadCount;

	private static TextDatabase s_Instance;

	public static TextDatabase instance
	{
		get
		{
			return GetInstance();
		}
	}

	public int reloadCount
	{
		get
		{
			return _ReloadCount;
		}
	}

	public SystemLanguage databaseLangugae
	{
		get
		{
			return _DatabaseLangugae;
		}
	}

	public string this[int i]
	{
		get
		{
			if (_DataBase.ContainsKey(i))
			{
				return _DataBase[i].m_Text;
			}
			return "<UNKNOWN TEXT>";
		}
	}

	public bool Reload()
	{
		SystemLanguage inLanguage = SystemLanguage.English;
		/*if (!Reload(inLanguage))
		{
			return Reload(SystemLanguage.Unknown);
		}*/
		return true;
	}

	/*public bool Reload(SystemLanguage inLanguage)
	{
		string outlanguagePostfix;
		if (!GetLanguageFilePostfix(SystemLanguage.English, out outlanguagePostfix))
		{
			Debug.LogError("Can't obtain file extension for default language... : " + SystemLanguage.English);
			return false;
		}
		string outlanguagePostfix2;
		if (!GetLanguageFilePostfix(inLanguage, out outlanguagePostfix2))
		{
			inLanguage = SystemLanguage.English;
			outlanguagePostfix2 = outlanguagePostfix;
		}
		Dictionary<int, GameText> dictionary = new Dictionary<int, GameText>();
		string[] array = new string[2] { "Texts/Texts.", "Texts/Texts_CityMap." };
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (inLanguage != SystemLanguage.English)
			{
				string text2 = text + outlanguagePostfix2;
				if (LoadTextFile(text2, dictionary))
				{
					continue;
				}
				Debug.LogError("Can't process text file : " + text2 + ".\n   So now we try to process default language text file.");
			}
			string text3 = text + outlanguagePostfix;
			if (!LoadTextFile(text3, dictionary))
			{
				Debug.LogError("Can't process text file : " + text3);
				return false;
			}
		}
		_DataBase = dictionary;
		_DatabaseLangugae = inLanguage;
		_ReloadCount++;
		return true;
	}*/

	public bool Exists(int i)
	{
		return _DataBase.ContainsKey(i);
	}

	public static Dictionary<int, GameText> GetDatabase_ForInspector()
	{
		return instance._DataBase;
	}

	private static TextDatabase GetInstance()
	{
		if (s_Instance == null)
		{
			s_Instance = ScriptableObject.CreateInstance<TextDatabase>();
			if (s_Instance == null)
			{
				Debug.LogError("Can't create TextDatabase");
				return null;
			}
			Object.DontDestroyOnLoad(s_Instance);
			s_Instance.Reload();
		}
		return s_Instance;
	}

	private static bool GetLanguageFilePostfix(SystemLanguage inLanguage, out string outlanguagePostfix)
	{
		switch (inLanguage)
		{
		case SystemLanguage.English:
			outlanguagePostfix = "eng";
			return true;
		case SystemLanguage.German:
			outlanguagePostfix = "ger";
			return true;
		case SystemLanguage.French:
			outlanguagePostfix = "fre";
			return true;
		case SystemLanguage.Italian:
			outlanguagePostfix = "ita";
			return true;
		case SystemLanguage.Japanese:
			outlanguagePostfix = "jpn";
			return true;
		case SystemLanguage.Chinese:
			outlanguagePostfix = "chi";
			return true;
		case SystemLanguage.Korean:
			outlanguagePostfix = "kor";
			return true;
		default:
			outlanguagePostfix = "eng";
			return false;
		}
	}

	private bool LoadTextFile(string inFileName, Dictionary<int, GameText> inoutNewDictionary)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(inFileName, typeof(TextAsset));
		if (textAsset == null)
		{
			Debug.Log(inFileName + " -- was not found");
			return false;
		}
		StringReader stringReader = new StringReader(textAsset.text);
		if (stringReader == null)
		{
			Debug.Log("file not found or not readable");
			return false;
		}
		int num = 0;
		string text;
		while ((text = stringReader.ReadLine()) != null)
		{
			num++;
			int outTextID;
			string outText;
			if (!ProcessLine(text, out outTextID, out outText))
			{
				Debug.LogError("Parse error in file " + inFileName + " on line [" + num + "] line is: " + text);
			}
			else if (outTextID >= 0)
			{
				if (inoutNewDictionary.ContainsKey(outTextID))
				{
					Debug.LogWarning(string.Concat("Text with ID [", outTextID, "] already exist in text database. Content \"", inoutNewDictionary[outTextID], "\" will be replaced by \"", outText, "\""));
				}
				inoutNewDictionary[outTextID] = new GameText
				{
					m_Index = outTextID,
					m_Text = outText
				};
			}
		}
		return true;
	}

	private bool ProcessLine(string inLine, out int outTextID, out string outText)
	{
		outTextID = -1;
		outText = string.Empty;
		int num = inLine.IndexOf('#');
		if (num == 0)
		{
			return true;
		}
		if (num > 0)
		{
			inLine = inLine.Remove(num);
		}
		inLine = inLine.Trim();
		if (inLine.Length == 0)
		{
			return true;
		}
		char[] anyOf = new char[4] { ' ', '\t', '\n', '\r' };
		int num2 = inLine.IndexOfAny(anyOf);
		if (num2 <= 0)
		{
			if (!int.TryParse(inLine, out outTextID))
			{
				return false;
			}
		}
		else
		{
			string s = inLine.Substring(0, num2).Trim();
			if (!int.TryParse(s, out outTextID))
			{
				return false;
			}
			outText = inLine.Substring(num2).Trim();
			outText = outText.Replace("\\n", "\n");
		}
		outText = RemoveSpacesAroundNewLine(outText);
		return true;
	}

	private string RemoveSpacesAroundNewLine(string inText)
	{
		string text = inText;
		int length;
		int length2;
		do
		{
			length = text.Length;
			text = text.Replace(" \n", "\n");
			text = text.Replace("\n ", "\n");
			length2 = text.Length;
		}
		while (length2 < length);
		return text;
	}
}
