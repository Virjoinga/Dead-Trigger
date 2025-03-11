using System.Collections.Generic;
using UnityEngine;

public class MissionResult
{
	public enum Type
	{
		SUCCESS = 0,
		FAIL = 1,
		NONE = 2
	}

	public Type Result = Type.NONE;

	public float MissionTime;

	public int KilledZombies;

	public int CollectedMoney;

	public int SpentMoney;

	public int WastedMoneyBags;

	public float HealthLost;

	public float AverageProtectObjectsHP;

	public int SpecialEnemies;

	public int HeadShots;

	public int RemovedLimbs;

	public int Disintegrations;

	public int ArenaScore;

	public int ArenaWaves;

	public Dictionary<E_WeaponID, float> FavouriteWeapon = new Dictionary<E_WeaponID, float>();

	public Dictionary<E_ItemID, int> FavouriteItem = new Dictionary<E_ItemID, int>();

	private int ShotsFired;

	private int ShotHits;

	public int FireAccuracy
	{
		get
		{
			if (ShotHits > 0)
			{
				return Mathf.RoundToInt(100f * (float)ShotHits / (float)ShotsFired);
			}
			return 0;
		}
	}

	public void Clear()
	{
		Result = Type.FAIL;
		MissionTime = 0f;
		KilledZombies = 0;
		CollectedMoney = 0;
		SpentMoney = 0;
		WastedMoneyBags = 0;
		HealthLost = 0f;
		AverageProtectObjectsHP = 1f;
		SpecialEnemies = 0;
		HeadShots = 0;
		RemovedLimbs = 0;
		Disintegrations = 0;
		ShotsFired = 0;
		ShotHits = 0;
		ArenaScore = 0;
		ArenaWaves = 0;
		FavouriteWeapon.Clear();
		FavouriteItem.Clear();
	}

	public void WeaponUsed(E_WeaponID id, float fireTime)
	{
		if (FavouriteWeapon.ContainsKey(id))
		{
			Dictionary<E_WeaponID, float> favouriteWeapon;
			Dictionary<E_WeaponID, float> dictionary = (favouriteWeapon = FavouriteWeapon);
			E_WeaponID key;
			E_WeaponID key2 = (key = id);
			float num = favouriteWeapon[key];
			dictionary[key2] = num + fireTime;
		}
		else
		{
			FavouriteWeapon.Add(id, fireTime);
		}
	}

	public void ItemUsed(E_ItemID id)
	{
		if (FavouriteItem.ContainsKey(id))
		{
			Dictionary<E_ItemID, int> favouriteItem;
			Dictionary<E_ItemID, int> dictionary = (favouriteItem = FavouriteItem);
			E_ItemID key;
			E_ItemID key2 = (key = id);
			int num = favouriteItem[key];
			dictionary[key2] = num + 1;
		}
		else
		{
			FavouriteItem.Add(id, 1);
		}
	}

	public E_ItemID GetFavouriteItem()
	{
		E_ItemID result = E_ItemID.None;
		int num = 0;
		foreach (KeyValuePair<E_ItemID, int> item in FavouriteItem)
		{
			if (item.Value != num)
			{
				result = item.Key;
				num = item.Value;
			}
		}
		return result;
	}

	public E_WeaponID GetFavouriteWeapon()
	{
		E_WeaponID result = E_WeaponID.None;
		float num = 0f;
		foreach (KeyValuePair<E_WeaponID, float> item in FavouriteWeapon)
		{
			if (item.Value != num)
			{
				result = item.Key;
				num = item.Value;
			}
		}
		return result;
	}

	public void RegisterShot(bool hit)
	{
		ShotsFired++;
		if (hit)
		{
			ShotHits++;
		}
	}

	public string GetMissionTime()
	{
		int num = Mathf.FloorToInt(MissionTime / 60f);
		int num2 = ((num != 0) ? (Mathf.RoundToInt(MissionTime) % (num * 60)) : Mathf.RoundToInt(MissionTime));
		if (num < 10)
		{
			if (num2 < 10)
			{
				return "0" + num + ":0" + num2;
			}
			return "0" + num + ":" + num2;
		}
		if (num2 < 10)
		{
			return num + ":0" + num2;
		}
		return num + ":" + num2;
	}
}
