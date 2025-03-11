using System.Collections.Generic;
using UnityEngine;

public class OneTimeSaleOfferManager
{
	public struct Offer
	{
		public ShopItemId m_Offer;

		public float m_Discount;
	}

	private bool m_Initialized;

	private List<Offer> m_Offers = new List<Offer>();

	private Offer m_ActiveOffer;

	private List<E_WeaponID> m_UsedOffers = new List<E_WeaponID>();

	private float m_NextOfferGameTime;

	private int m_NextOfferGameDeaths;

	private void Initialize()
	{
		m_Initialized = true;
		m_Offers.Clear();
		if (Game.Instance.PlayerPersistentInfo.totalMissionsPlayed >= 5 && (m_NextOfferGameTime <= GetActualGameTime() || m_NextOfferGameDeaths <= GetActualDeaths()))
		{
			List<ShopItemId> weapons = ShopDataBridge.Instance.GetWeapons();
			Offer item = default(Offer);
			foreach (ShopItemId item2 in weapons)
			{
				ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(item2);
				if (!itemInfo.GoldCurrency || Game.Instance.PlayerPersistentInfo.InventoryList.ContainsWeapon((E_WeaponID)item2.Id))
				{
					continue;
				}
				float num = 0f;
				int num2 = 0;
				foreach (E_WeaponID usedOffer in m_UsedOffers)
				{
					if (usedOffer == (E_WeaponID)item2.Id)
					{
						num2++;
					}
				}
				if (num2 < 2 && itemInfo.RequiredLevel + 2 <= Game.Instance.PlayerPersistentInfo.rank)
				{
					float num3 = Game.Instance.PlayerPersistentInfo.rank - itemInfo.RequiredLevel;
					float num4 = ((!(m_NextOfferGameTime > GetActualGameTime())) ? ((!(Random.value < 0.51f)) ? (-0.05f) : ((float)Random.Range(0, 4) * 0.05f)) : ((float)Random.Range(1, 4) * 0.05f));
					num3 *= 0.05f;
					if (num3 > 0.35f)
					{
						if (num3 > 0.5f)
						{
							num4 += 0.1f;
						}
						num3 = 0.35f;
					}
					num = Mathf.Clamp(0.3f + num3 + num4, 0.35f, 0.75f);
				}
				if (num > 0f)
				{
					item.m_Offer = item2;
					item.m_Discount = num;
					m_Offers.Add(item);
				}
			}
		}
		if (m_Offers.Count == 0)
		{
			m_ActiveOffer.m_Offer = null;
			return;
		}
		int index = Random.Range(0, m_Offers.Count);
		if (m_UsedOffers.Count > 0 && m_Offers.Count > 0 && m_Offers[index].m_Offer.Id == (int)m_UsedOffers[m_UsedOffers.Count - 1])
		{
			index = Random.Range(0, m_Offers.Count);
		}
		m_ActiveOffer = m_Offers[index];
	}

	public bool OneTimeSaleOfferAvailable()
	{
		if (!m_Initialized)
		{
			Initialize();
		}
		return m_ActiveOffer.m_Offer != null;
	}

	public ShopItemId ActualOneTimeSaleOffer()
	{
		return m_ActiveOffer.m_Offer;
	}

	public float ActualOfferDiscount()
	{
		return m_ActiveOffer.m_Discount;
	}

	public int ActualOfferOrigPrice()
	{
		ShopItemInfo itemInfo = ShopDataBridge.Instance.GetItemInfo(m_ActiveOffer.m_Offer);
		return itemInfo.Cost;
	}

	public void Save(IDataFile file)
	{
		if (m_ActiveOffer.m_Offer != null)
		{
			m_UsedOffers.Add((E_WeaponID)m_ActiveOffer.m_Offer.Id);
			int num = Mathf.Clamp(Game.Instance.PlayerPersistentInfo.rank, 0, 20);
			if (m_NextOfferGameTime > GetActualGameTime())
			{
				m_NextOfferGameDeaths = GetActualDeaths() + Random.Range(10 + num / 4, 15 + num / 4);
			}
			else
			{
				m_NextOfferGameDeaths = GetActualDeaths() + Random.Range(2 + num / 4, 4 + num / 4);
			}
			m_NextOfferGameTime = GetActualGameTime() + 60f * Random.Range(20 + num, (float)(20 + num) * 1.5f);
		}
		file.SetFloat("NextOfferGameTime", m_NextOfferGameTime);
		file.SetInt("NextOfferGameDeaths", m_NextOfferGameDeaths);
		int num2 = 1;
		foreach (E_WeaponID usedOffer in m_UsedOffers)
		{
			file.SetInt("OfferUsed" + num2, (int)usedOffer);
			num2++;
		}
		m_ActiveOffer.m_Offer = null;
		m_Initialized = false;
	}

	public void Load(IDataFile file)
	{
		m_UsedOffers.Clear();
		m_NextOfferGameTime = file.GetFloat("NextOfferGameTime", 0f);
		m_NextOfferGameDeaths = file.GetInt("NextOfferGameDeaths");
		int num = 1;
		while (true)
		{
			string key = "OfferUsed" + num;
			if (!file.KeyExists(key))
			{
				break;
			}
			E_WeaponID @int = (E_WeaponID)file.GetInt(key);
			m_UsedOffers.Add(@int);
			num++;
		}
	}

	public float GetActualGameTime()
	{
		float gameTime = Game.Instance.PlayerPersistentInfo.GameTime;
		gameTime += Game.Instance.PlayerPersistentInfo.ArenaTotalTime(ArenaId.Arena1);
		gameTime += Game.Instance.PlayerPersistentInfo.ArenaTotalTime(ArenaId.Arena2);
		gameTime += Game.Instance.PlayerPersistentInfo.ArenaTotalTime(ArenaId.Arena3);
		return gameTime + Game.Instance.PlayerPersistentInfo.ArenaTotalTime(ArenaId.Arena4);
	}

	public int GetActualDeaths()
	{
		return Game.Instance.PlayerPersistentInfo.deaths;
	}
}
