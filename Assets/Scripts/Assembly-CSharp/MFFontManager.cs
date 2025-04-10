#define DEBUG
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GUI/Hiearchy/Font Manager")]
public class MFFontManager : MonoBehaviour
{
	[Serializable]
	public class FontSetup
	{
		public string m_FontName;

		public GUIBase_FontBase m_Default;

		public GUIBase_FontBase m_German;

		public GUIBase_FontBase m_French;

		public GUIBase_FontBase m_Italian;

		public GUIBase_FontBase m_Japanese;

		public GUIBase_FontBase m_Chinese;

		public GUIBase_FontBase m_Korean;

		public GUIBase_FontBase GetFont(SystemLanguage inLanguage)
		{
			GUIBase_FontBase result = m_Default;
			switch (inLanguage)
			{
			case SystemLanguage.German:
				result = m_German;
				break;
			case SystemLanguage.French:
				result = m_French;
				break;
			case SystemLanguage.Italian:
				result = m_Italian;
				break;
			case SystemLanguage.Japanese:
				result = m_Japanese;
				break;
			case SystemLanguage.Chinese:
				result = m_Chinese;
				break;
			case SystemLanguage.Korean:
				result = m_Korean;
				break;
			}
			return result;
		}
	}

	private static MFFontManager Instance;

	private static string DefaultFontName_Static = "Default";

	[SerializeField]
	private string m_DefaultFontName = DefaultFontName_Static;

	[SerializeField]
	private GUIBase_FontBase m_DefaultFont;

	[SerializeField]
	private List<FontSetup> m_Fonts = new List<FontSetup>();

	public static string defaultFontName
	{
		get
		{
			return (!(Instance != null)) ? DefaultFontName_Static : Instance.m_DefaultFontName;
		}
	}

	private void Awake()
	{
		if (Application.isPlaying && (bool)Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public static GUIBase_FontBase GetFont(string inFontName)
	{
		return GetFont(inFontName, TextDatabase.instance.databaseLangugae);
	}

	public static GUIBase_FontBase GetFont(string inFontName, SystemLanguage inLanguage)
	{
		if (Instance == null)
		{
			return null;
		}
		return Instance._GetFont(inFontName, inLanguage);
	}

	private GUIBase_FontBase _GetFont(string inFontName, SystemLanguage inLanguage)
	{
		GUIBase_FontBase gUIBase_FontBase = null;
		foreach (FontSetup font in m_Fonts)
		{
			if (font.m_FontName == inFontName)
			{
				gUIBase_FontBase = font.GetFont(inLanguage);
				break;
			}
		}
		if (gUIBase_FontBase == null)
		{
			gUIBase_FontBase = m_DefaultFont;
		}
		return gUIBase_FontBase;
	}

	private static MFFontManager GetManager_InEditor()
	{
		MFFontManager[] array = UnityEngine.Object.FindObjectsOfType(typeof(MFFontManager)) as MFFontManager[];
		if (array == null || array.Length == 0)
		{
			Debug.LogError("can't find MFFontManager component in active scene");
			return null;
		}
		if (array.Length > 0)
		{
			Debug.LogWarning("There are more then one MFFontManager objects in scene, first one will be used !!!");
		}
		return array[0];
	}

	public virtual void CheckDataConsistency()
	{
		if (m_DefaultFont == null)
		{
			Debug.LogError("Default font is not assigned", this);
		}
		DebugUtils.Assert(m_DefaultFont != null);
		List<FontSetup> list = m_Fonts.FindAll((FontSetup font) => font.m_FontName == m_DefaultFontName);
		if (list != null && list.Count > 0)
		{
			Debug.LogError(string.Format("'{0}' is reserved name and can't be used for other fonts", m_DefaultFontName), this);
		}
		if (m_DefaultFontName != DefaultFontName_Static)
		{
			list = m_Fonts.FindAll((FontSetup font) => font.m_FontName == DefaultFontName_Static);
			if (list != null && list.Count > 0)
			{
				Debug.LogError(string.Format("'{0}' is reserved name and can't be used for other fonts", DefaultFontName_Static), this);
			}
		}
		FontSetup setup;
		foreach (FontSetup font in m_Fonts)
		{
			setup = font;
			if (setup.m_Default == null)
			{
				Debug.LogError(string.Format("Font setup [{0}] doesn't have assigned default font", setup.m_FontName), this);
			}
			list = m_Fonts.FindAll((FontSetup f) => f.m_FontName == setup.m_FontName);
			if (list.Count != 1)
			{
				Debug.LogError(string.Format("Font name {0} isn't unique [{1}]", setup.m_FontName, list.Count), this);
			}
		}
	}
}
