using UnityEngine;

public class CityMissionInfo : CitySiteInfo
{
	private MissionManager.MissionData m_MissionData;

	private PlayerLevelData.PlayerLevelInfo m_PlayerLevelInfo;

	public int storyID
	{
		get
		{
			return m_MissionData.mission.storyId;
		}
	}

	public MissionFlowData.Difficulty difficulty
	{
		get
		{
			return m_MissionData.difficulty;
		}
	}

	public E_MissionType missionType
	{
		get
		{
			return m_MissionData.mission.missionType;
		}
	}

	public string missionSubtype
	{
		get
		{
			return m_MissionData.mission.missionSubtype;
		}
	}

	public MissionFlowData.StoryBound story
	{
		get
		{
			return m_MissionData.mission.storyBound;
		}
	}

	public SpecialReward.Type bonus
	{
		get
		{
			return m_MissionData.mission.specialBonus;
		}
	}

	public string level
	{
		get
		{
			return m_MissionData.level;
		}
	}

	public string levelPreview
	{
		get
		{
			return m_MissionData.levelPreview;
		}
	}

	public E_WeaponID recommendedWeapon
	{
		get
		{
			return m_PlayerLevelInfo.recommendedWeapon;
		}
	}

	public int caption
	{
		get
		{
			return m_MissionData.texts.caption;
		}
	}

	public int objective
	{
		get
		{
			return m_MissionData.texts.objective;
		}
	}

	public int description
	{
		get
		{
			return m_MissionData.texts.description;
		}
	}

	public int successText
	{
		get
		{
			return m_MissionData.texts.success;
		}
	}

	public int failText
	{
		get
		{
			return m_MissionData.texts.fail;
		}
	}

	public MissionResult missionResult { get; set; }

	public int specialEnemiesMoney
	{
		get
		{
			return GameplayData.Instance.MoneyPerSpecialEnemy() * missionResult.SpecialEnemies;
		}
	}

	public int headShotMoney
	{
		get
		{
			return GameplayData.Instance.MoneyPerHead() * missionResult.HeadShots;
		}
	}

	public int limbMoney
	{
		get
		{
			return GameplayData.Instance.MoneyPerLimb() * missionResult.RemovedLimbs;
		}
	}

	public int desintegrationMoney
	{
		get
		{
			return GameplayData.Instance.MoneyPerCarnage() * missionResult.Disintegrations;
		}
	}

	public int foundMoney
	{
		get
		{
			return missionResult.CollectedMoney;
		}
	}

	public int spentMoney
	{
		get
		{
			return missionResult.SpentMoney;
		}
	}

	public int wastedMoney
	{
		get
		{
			return GameplayData.Instance.MoneyPerZombie() * missionResult.WastedMoneyBags;
		}
	}

	public int bonusMoney
	{
		get
		{
			if (missionResult.Result != 0)
			{
				return 0;
			}
			float num = 1f;
			if (missionResult.HeadShots + 1 >= missionResult.KilledZombies)
			{
				num += 0.5f;
			}
			else if (missionResult.HeadShots + missionResult.Disintegrations >= missionResult.KilledZombies)
			{
				num += 0.2f;
			}
			if (missionResult.HealthLost < 0.0001f)
			{
				num += 0.5f;
			}
			if (missionType == E_MissionType.ProtectObjects)
			{
				num += Mathf.Clamp(missionResult.AverageProtectObjectsHP - 0.5f, 0f, 0.5f);
			}
			return Mathf.CeilToInt(num * (float)GameplayData.Instance.MoneyCompletionBonus() + (float)specialEnemiesMoney);
		}
	}

	public int zombieXp
	{
		get
		{
			return GameplayData.Instance.XpPerZombie() * missionResult.KilledZombies;
		}
	}

	public int bonusXp
	{
		get
		{
			return (missionResult.Result == MissionResult.Type.SUCCESS) ? Mathf.RoundToInt(GameplayData.Instance.XpCompletionBonus()) : 0;
		}
	}

	public int totalExperience
	{
		get
		{
			return bonusXp + zombieXp;
		}
	}

	public int totalMoney
	{
		get
		{
			return foundMoney + bonusMoney + headShotMoney + desintegrationMoney + limbMoney;
		}
	}

	public string slotSuggestion
	{
		get
		{
			return m_MissionData.mission.specialSlot;
		}
	}

	public string specialIcon
	{
		get
		{
			return m_MissionData.mission.specialIcon;
		}
	}

	public CityMissionInfo()
	{
		missionResult = null;
	}

	public CityMissionInfo(MissionManager.MissionData missionData)
	{
		m_MissionData = missionData;
		m_InfoType = InfoType.Normal;
		missionResult = null;
	}

	public void AssignPlayerLevelInfo(PlayerLevelData.PlayerLevelInfo levelInfo)
	{
		m_PlayerLevelInfo = levelInfo;
	}

	public int MissionRating()
	{
		float num = 0f;
		if (missionResult.Result == MissionResult.Type.SUCCESS)
		{
			switch (missionType)
			{
			case E_MissionType.KillZombies:
				num = 0f;
				break;
			case E_MissionType.TimeDefense:
				num = 0f;
				break;
			case E_MissionType.CarryResources:
				num = 0f;
				break;
			case E_MissionType.ProtectObjects:
				num = 0f;
				break;
			}
		}
		else
		{
			num = -250f;
		}
		float num2 = missionResult.KilledZombies;
		if (num2 > 0f)
		{
			num += Mathf.Clamp(800f * ((float)missionResult.HeadShots / num2), 0f, 250f);
			num += Mathf.Clamp(300f * ((float)missionResult.RemovedLimbs / num2), 0f, 250f);
			num += Mathf.Clamp(800f * ((float)missionResult.Disintegrations / num2), 0f, 250f);
			num += Mathf.Clamp(3f * num2 - missionResult.HealthLost * 100f, -250f, 250f);
			num -= Mathf.Clamp(missionResult.WastedMoneyBags * 2, 0f, 100f);
		}
		num += (float)missionResult.FireAccuracy;
		if (missionResult.Result != 0 && num > 300f)
		{
			num = 300f;
		}
		return Mathf.Clamp(Mathf.RoundToInt(num / 100f), 0, 5);
	}

	public void MarkAsUsed()
	{
		m_MissionData.mission.used++;
	}

	public override string ToString()
	{
		return string.Concat("CityMissionInfo - Type: ", MissionTypeNames.name[(int)missionType], ", unique: ", story, ", caption:", caption, ", level:", level, ",  zombies:");
	}

	public void Save(IDataFile file, string keyPrefix, MissionManager missionManager)
	{
		missionManager.SaveMissionData(m_MissionData, file, keyPrefix);
	}

	public void Load(IDataFile file, string keyPrefix, MissionManager missionManager)
	{
		m_MissionData = new MissionManager.MissionData();
		missionManager.LoadMissionData(m_MissionData, file, keyPrefix);
	}
}
