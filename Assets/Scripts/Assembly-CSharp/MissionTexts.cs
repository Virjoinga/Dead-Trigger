using System;
using UnityEngine;

[Serializable]
public class MissionTexts
{
	[SerializeField]
	private int m_Caption;

	[SerializeField]
	private int m_Objective;

	[SerializeField]
	private int m_Description;

	[SerializeField]
	private int m_Success;

	[SerializeField]
	private int m_Fail;

	public int caption
	{
		get
		{
			return m_Caption;
		}
		set
		{
			m_Caption = value;
		}
	}

	public int objective
	{
		get
		{
			return m_Objective;
		}
		set
		{
			m_Objective = value;
		}
	}

	public int description
	{
		get
		{
			return m_Description;
		}
		set
		{
			m_Description = value;
		}
	}

	public int success
	{
		get
		{
			return m_Success;
		}
		set
		{
			m_Success = value;
		}
	}

	public int fail
	{
		get
		{
			return m_Fail;
		}
		set
		{
			m_Fail = value;
		}
	}

	protected MissionTexts()
	{
	}

	public static MissionTexts CreateNew()
	{
		return new MissionTexts();
	}

	public static void Destroy(MissionTexts texts)
	{
	}

	public void Save(IDataFile file, string keyPrefix)
	{
		file.SetInt(keyPrefix + "Caption", caption);
		file.SetInt(keyPrefix + "Objective", objective);
		file.SetInt(keyPrefix + "Description", description);
		file.SetInt(keyPrefix + "Success", success);
		file.SetInt(keyPrefix + "Fail", fail);
	}

	public void Load(IDataFile file, string keyPrefix)
	{
		caption = file.GetInt(keyPrefix + "Caption");
		objective = file.GetInt(keyPrefix + "Objective");
		description = file.GetInt(keyPrefix + "Description");
		success = file.GetInt(keyPrefix + "Success");
		fail = file.GetInt(keyPrefix + "Fail");
	}
}
