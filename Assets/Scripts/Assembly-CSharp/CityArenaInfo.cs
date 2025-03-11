using System.Collections.Generic;
using UnityEngine;

public class CityArenaInfo : CitySiteInfo
{
	public class Arena
	{
		public ArenaId arenaId { get; set; }

		public string level { get; set; }

		public string levelPreview { get; set; }

		public int name { get; set; }

		public bool enabled
		{
			get
			{
				return anyAvailableGame && !locked;
			}
		}

		public bool anyAvailableGame
		{
			get
			{
				return availableGames > 0;
			}
		}

		public bool locked { get; set; }

		public int availableGames { get; set; }
	}

	private List<Arena> m_Arenas = new List<Arena>();

	public IList<Arena> arenas
	{
		get
		{
			return m_Arenas;
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

	public int arenaRank
	{
		get
		{
			return Game.Instance.PlayerPersistentInfo.rank;
		}
	}

	public int arenaScore
	{
		get
		{
			return missionResult.ArenaScore;
		}
	}

	public int arenaWaves
	{
		get
		{
			return missionResult.ArenaWaves;
		}
	}

	public int arenaActiveIndex { get; set; }

	public int arenaMoney
	{
		get
		{
			return totalMoney;
		}
	}

	public int arenaXp
	{
		get
		{
			return totalExperience;
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
			return Mathf.CeilToInt((float)zombieXp * 1.2f);
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
			return foundMoney - spentMoney + headShotMoney + desintegrationMoney + limbMoney + specialEnemiesMoney;
		}
	}

	public CityArenaInfo()
		: base(InfoType.Arena)
	{
		missionResult = null;
		Arena item = new Arena
		{
			arenaId = ArenaId.Arena1,
			name = 1010600,
			level = "subway",
			levelPreview = "Arena1",
			availableGames = 5,
			locked = false
		};
		m_Arenas.Add(item);
		item = new Arena
		{
			arenaId = ArenaId.Arena2,
			name = 1010602,
			level = "graveyard",
			levelPreview = "Arena2",
			availableGames = 5,
			locked = false
		};
		m_Arenas.Add(item);
		item = new Arena
		{
			arenaId = ArenaId.Arena3,
			name = 1010604,
			level = "arena_3",
			levelPreview = "Arena3",
			availableGames = 5,
			locked = false
		};
		m_Arenas.Add(item);
		item = new Arena
		{
			arenaId = ArenaId.Arena4,
			name = 1010606,
			level = "northpole",
			levelPreview = "Arena4",
			availableGames = 5,
			locked = false
		};
		m_Arenas.Add(item);
	}

	public int HiMoney(ArenaId arenaId)
	{
		return Game.Instance.PlayerPersistentInfo.ArenaHiMoney(arenaId);
	}

	public int Played(ArenaId arenaId)
	{
		return Game.Instance.PlayerPersistentInfo.ArenaPlayed(arenaId);
	}

	public int HiScore(ArenaId arenaId)
	{
		return Game.Instance.PlayerPersistentInfo.ArenaHiScore(arenaId);
	}

	public void DailyReward()
	{
		foreach (Arena arena in m_Arenas)
		{
			arena.availableGames = 100000;
		}
	}

	public int MissionRating()
	{
		float num = 0f;
		float num2 = missionResult.KilledZombies;
		if (num2 > 0f)
		{
			num += Mathf.Clamp(400f * ((float)missionResult.HeadShots / num2), 0f, 150f);
			num += Mathf.Clamp(200f * ((float)missionResult.RemovedLimbs / num2), 0f, 150f);
			num += Mathf.Clamp(400f * ((float)missionResult.Disintegrations / num2), 0f, 150f);
			num += Mathf.Clamp(2f * num2 - missionResult.HealthLost * 100f, -400f, 100f);
		}
		num += (float)missionResult.ArenaWaves * 25f;
		num += 0.5f * (float)missionResult.FireAccuracy;
		num -= Mathf.Clamp(missionResult.WastedMoneyBags * 2, 0f, 100f);
		return Mathf.Clamp(Mathf.RoundToInt(num / 100f), 0, 5);
	}

	public override string ToString()
	{
		return string.Empty;
	}

	public void Save(IDataFile file, string keyPrefix)
	{
		file.SetInt(keyPrefix + "_ArenaId", arenaActiveIndex);
		for (int i = 0; i < m_Arenas.Count; i++)
		{
			file.SetInt(keyPrefix + "_index" + i, m_Arenas[i].availableGames);
		}
	}

	public void Load(IDataFile file, string keyPrefix, int version)
	{
		arenaActiveIndex = file.GetInt(keyPrefix + "_ArenaId");
		int num = 0;
		while (true)
		{
			string key = keyPrefix + "_index" + num;
			if (num >= m_Arenas.Count || !file.KeyExists(key))
			{
				break;
			}
			m_Arenas[num].availableGames = file.GetInt(key);
			if (version <= 6)
			{
				m_Arenas[num].availableGames = 5;
			}
			num++;
		}
	}
}
