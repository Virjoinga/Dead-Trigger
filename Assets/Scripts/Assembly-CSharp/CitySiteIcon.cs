using UnityEngine;

public class CitySiteIcon : MonoBehaviour
{
	public int indicatorIndex = -1;

	private CitySiteInfo m_Info;

	private static int m_MaxDifficulty;

	public CitySiteInfo siteInfo
	{
		get
		{
			return m_Info;
		}
	}

	public CityMissionInfo missionInfo
	{
		get
		{
			return m_Info as CityMissionInfo;
		}
	}

	public CityArenaInfo arenaInfo
	{
		get
		{
			return m_Info as CityArenaInfo;
		}
	}

	public CityMoneyInfo moneyInfo
	{
		get
		{
			return m_Info as CityMoneyInfo;
		}
	}

	public void Init(CitySiteInfo info)
	{
		m_Info = info;
		if (info is CityMissionInfo)
		{
			CityMissionInfo cityMissionInfo = info as CityMissionInfo;
			int num = (int)(cityMissionInfo.difficulty + 1);
			string text = "star_" + num;
			string text2 = "icon";
			string text3 = "icon";
			if (cityMissionInfo.specialIcon != string.Empty)
			{
				text3 += cityMissionInfo.specialIcon;
			}
			text2 += (int)(cityMissionInfo.missionType + 1);
			{
				foreach (Transform item in base.transform)
				{
					if (item.gameObject.name == text)
					{
						item.gameObject._SetActiveRecursively(true);
					}
					else if (item.gameObject.name[0] == 's')
					{
						Object.Destroy(item.gameObject);
					}
					else if (item.gameObject.name == text2 || item.gameObject.name == text3)
					{
						item.gameObject._SetActiveRecursively(true);
					}
					else if (item.gameObject.name[0] == 'i')
					{
						Object.Destroy(item.gameObject);
					}
					else if (item.gameObject.name[0] == 'b')
					{
						if (cityMissionInfo.bonus != 0)
						{
							item.gameObject._SetActiveRecursively(true);
						}
						else
						{
							Object.Destroy(item.gameObject);
						}
					}
				}
				return;
			}
		}
		foreach (Transform item2 in base.transform)
		{
			if (item2.gameObject.name[0] == 'i')
			{
				if (item2.gameObject.name == "iconShop" && info.infoType != CitySiteInfo.InfoType.Shop)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconHideout" && info.infoType != CitySiteInfo.InfoType.SafeHaven)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconArena" && info.infoType != CitySiteInfo.InfoType.Arena)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconEquip" && info.infoType != CitySiteInfo.InfoType.Equip)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconBank" && info.infoType != CitySiteInfo.InfoType.Bank)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconDollar" && info.infoType != CitySiteInfo.InfoType.Money)
				{
					Object.Destroy(item2.gameObject);
				}
				if (item2.gameObject.name == "iconCasino" && info.infoType != CitySiteInfo.InfoType.Casino)
				{
					Object.Destroy(item2.gameObject);
				}
			}
		}
	}

	public void PrepareForDestroy()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
