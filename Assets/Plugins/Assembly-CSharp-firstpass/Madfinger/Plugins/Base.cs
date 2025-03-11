using UnityEngine;

namespace Madfinger.Plugins
{
	public abstract class Base
	{
		public abstract class Settings
		{
			private string m_Name;

			private int m_Version;

			public string Name
			{
				get
				{
					return m_Name;
				}
			}

			public int Version
			{
				get
				{
					return m_Version;
				}
			}

			public Settings(string name, int version)
			{
				m_Name = name;
				m_Version = version;
			}

			public abstract bool IsValid();
		}

		private const string PREFIX_PLUGINS = "PLUGINS.";

		private const string POSTFIX_INIT = ".INIT";

		private const string POSTFIX_VERSION = ".VERSION";

		private const int INIT_OK = 0;

		private const int INIT_RETRY = 3;

		private const int INIT_PERIOD = 10;

		private bool m_Initialized;

		private Settings m_Settings;

		public Settings PluginSettings
		{
			get
			{
				return m_Settings;
			}
		}

		private string StateID
		{
			get
			{
				return "PLUGINS." + m_Settings.Name + ".INIT";
			}
		}

		private string VersionID
		{
			get
			{
				return "PLUGINS." + m_Settings.Name + ".VERSION";
			}
		}

		private int InitState
		{
			get
			{
				return PlayerPrefs.GetInt(StateID, 0);
			}
			set
			{
				PlayerPrefs.SetInt(StateID, value);
				PlayerPrefs.Save();
			}
		}

		private int InitVersion
		{
			get
			{
				return PlayerPrefs.GetInt(VersionID, m_Settings.Version);
			}
			set
			{
				PlayerPrefs.SetInt(VersionID, value);
				PlayerPrefs.Save();
			}
		}

		protected Base(Settings settings)
		{
			m_Settings = settings;
		}

		protected abstract void Initialize();

		protected abstract void Deinitialize();

		public void Init()
		{
			if (m_Initialized)
			{
				Log("Re-Initialization, skipping...");
			}
			else if (m_Settings == null || !m_Settings.IsValid())
			{
				Log("Settings not valid! Skipping initialization...");
			}
			else if (PreInitialize())
			{
				Log("Initializing version " + m_Settings.Version);
				Initialize();
				PostInitialize();
				m_Initialized = true;
				Log("Initialization successful!");
			}
			else
			{
				Log("Last run crashed! Skipping initialization...");
			}
		}

		public void Done()
		{
			Log("Deinitializing...");
			Deinitialize();
			m_Initialized = false;
			m_Settings = null;
		}

		public bool IsInitialized()
		{
			return m_Initialized;
		}

		private bool PreInitialize()
		{
			int initState = InitState;
			if (!PlayerPrefs.HasKey(VersionID))
			{
				InitVersion = m_Settings.Version;
				Log("Setting up new version " + m_Settings.Version);
			}
			if (InitVersion != m_Settings.Version)
			{
				Log("Updating version from " + InitVersion + " to " + m_Settings.Version);
				InitVersion = m_Settings.Version;
				InitState = 0;
				return initState == 0;
			}
			InitState = initState + 1;
			return initState % 10 == 0 || initState == 3;
		}

		private void PostInitialize()
		{
			InitState = 0;
		}

		protected void Log(string message)
		{
		}
	}
}
