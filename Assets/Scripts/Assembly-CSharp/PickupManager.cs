using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	private class WeaponData
	{
		public int m_AmmoInLevel;

		public int m_AmmoInPockets;

		public int m_MaxAmmoInClip;

		public int m_MaxAmmoInWeapon;

		public float m_Evaluation;
	}

	private const float PosOffset = 0.7f;

	private const float ExpirationTime = 20f;

	private const float DistCollectTime = 1f;

	private const int AmmoInLevelLimit = 2;

	private const float AmmoTimeCheckPeriod = 0.4f;

	private static PickupManager m_Instance;

	private ResourceCache[] m_Cache;

	private List<Pickup> m_Pickups = new List<Pickup>();

	private List<PickupAutoCollectZone> m_AutoCollectZones = new List<PickupAutoCollectZone>();

	private Dictionary<E_WeaponID, WeaponData> m_WeaponsData;

	private int m_CollLayerMask;

	private int m_AmmoInLevel;

	private float m_Time2DropAmmo;

	private int m_MoneyDropRequests;

	private bool m_AutoCollecting;

	private bool m_AutoCollectZonesChanged;

	private float m_MoneyTimeLimit;

	private int m_MoneyRequestsMin;

	private int m_MoneyRequestsMax;

	private float m_AmmoDropRatioMin;

	private float m_AmmoDropRatioMax;

	public static PickupManager Instance
	{
		get
		{
			return m_Instance;
		}
	}

	public bool CollectAutomatically
	{
		get
		{
			return m_AutoCollecting;
		}
		set
		{
			m_AutoCollecting = value;
		}
	}

	private void Awake()
	{
		if (!(m_Instance != null))
		{
			m_Instance = this;
			m_AutoCollecting = false;
			m_AutoCollectZonesChanged = false;
			m_Cache = new ResourceCache[2];
			m_Cache[0] = new ResourceCache("Items/bonus_ammo", 4, 2);
			m_Cache[1] = new ResourceCache("Items/bonus_coin", 4, 2);
			m_Time2DropAmmo = 0f;
			m_CollLayerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
			m_WeaponsData = new Dictionary<E_WeaponID, WeaponData>();
			if (Game.Instance.GameplayType != GameplayType.Arena)
			{
				m_MoneyTimeLimit = 180f;
				m_MoneyRequestsMin = 8;
				m_MoneyRequestsMax = 16;
				m_AmmoDropRatioMin = 0.15f;
				m_AmmoDropRatioMax = 0.65f;
			}
			else
			{
				m_MoneyTimeLimit = -1f;
				m_MoneyRequestsMin = 8;
				m_MoneyRequestsMax = 16;
				m_AmmoDropRatioMin = 0.25f;
				m_AmmoDropRatioMax = 0.65f;
			}
		}
	}

	private void Destroy()
	{
		if (m_Instance == this)
		{
			m_Instance = null;
		}
	}

	public bool DropPickup(E_DropEvent DropEvent, E_PickupID Id, Vector3 Pos)
	{
		int num = 0;
		float expiration = -1f;
		Pickup pickup = null;
		switch (Id)
		{
		case E_PickupID.Ammo:
			pickup = SpawnAmmo(DropEvent);
			num = 1;
			break;
		case E_PickupID.Money:
			pickup = SpawnMoney(DropEvent);
			expiration = 20f;
			break;
		}
		if (pickup == null)
		{
			return false;
		}
		RaycastHit hitInfo;
		if (Physics.Raycast(Pos + Vector3.up * 0.1f, -Vector3.up, out hitInfo, 2f, m_CollLayerMask))
		{
			Pos = hitInfo.point;
			Pos.y += 0.7f;
		}
		if (IsInAutoCollectZone(Pos))
		{
			Player.Instance.PickUp(pickup);
			m_Cache[(int)pickup.GetID()].Return(pickup.GameObj);
			return false;
		}
		pickup.DroppedOut(Pos, expiration);
		m_Pickups.Add(pickup);
		m_AmmoInLevel += num;
		return true;
	}

	private Pickup SpawnAmmo(E_DropEvent DropEvent)
	{
		if (m_Time2DropAmmo >= 0f)
		{
			return null;
		}
		m_Time2DropAmmo = 0.4f;
		ComponentWeapons weaponComponent = Player.Instance.Owner.WeaponComponent;
		m_WeaponsData.Clear();
		WeaponData weaponData;
		foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in weaponComponent.Weapons)
		{
			WeaponBase value = weapon.Value;
			weaponData = new WeaponData();
			weaponData.m_AmmoInPockets = value.ClipAmmo + value.WeaponAmmo;
			weaponData.m_AmmoInLevel = 0;
			weaponData.m_MaxAmmoInClip = value.MaxAmmoInClip;
			weaponData.m_MaxAmmoInWeapon = value.MaxAmmoInWeapon;
			weaponData.m_Evaluation = Random.value;
			m_WeaponsData[value.WeaponID] = weaponData;
		}
		Pickup_Ammo pickup_Ammo = null;
		float num = float.MinValue;
		for (int i = 0; i < m_Pickups.Count; i++)
		{
			Pickup_Ammo pickup_Ammo2 = m_Pickups[i] as Pickup_Ammo;
			if (!(pickup_Ammo2 == null) && pickup_Ammo2.CanBePickedUp(false))
			{
				m_WeaponsData[pickup_Ammo2.m_WeaponID].m_AmmoInLevel += pickup_Ammo2.m_Amount;
				if (m_AmmoInLevel >= 2 && num < pickup_Ammo2.LifeTime)
				{
					pickup_Ammo = pickup_Ammo2;
					num = pickup_Ammo2.LifeTime;
				}
			}
		}
		float num2 = 0f;
		float num3 = float.MaxValue;
		E_WeaponID e_WeaponID = E_WeaponID.None;
		float num4;
		foreach (KeyValuePair<E_WeaponID, WeaponData> weaponsDatum in m_WeaponsData)
		{
			weaponData = weaponsDatum.Value;
			num4 = (float)(weaponData.m_AmmoInPockets + weaponData.m_AmmoInLevel) / (float)(weaponData.m_MaxAmmoInWeapon + weaponData.m_MaxAmmoInClip);
			num2 += num4;
			if (num4 < 1f && weaponData.m_Evaluation < num3)
			{
				e_WeaponID = weaponsDatum.Key;
				num3 = weaponData.m_Evaluation;
			}
		}
		if (e_WeaponID == E_WeaponID.None)
		{
			return null;
		}
		num4 = num2 / (float)m_WeaponsData.Count;
		if (DropEvent == E_DropEvent.OnDeath)
		{
			if (num4 > m_AmmoDropRatioMax)
			{
				return null;
			}
			if (num4 > m_AmmoDropRatioMin && Random.Range(m_AmmoDropRatioMin, m_AmmoDropRatioMax) < num4)
			{
				return null;
			}
		}
		else if (num4 > 0.15f)
		{
			return null;
		}
		if (pickup_Ammo != null)
		{
			pickup_Ammo.Expired();
		}
		weaponData = m_WeaponsData[e_WeaponID];
		GameObject gameObject = m_Cache[0].Get();
		Pickup_Ammo component = gameObject.GetComponent<Pickup_Ammo>();
		component.m_WeaponID = e_WeaponID;
		component.m_Amount = Mathf.CeilToInt((float)Mathf.Max(weaponData.m_MaxAmmoInClip, weaponData.m_MaxAmmoInWeapon) * 0.25f);
		return component;
	}

	private Pickup SpawnMoney(E_DropEvent DropEvent)
	{
		if (m_MoneyTimeLimit > 0f && Game.Instance.MissionResultData.MissionTime >= m_MoneyTimeLimit)
		{
			return null;
		}
		float value = Random.value;
		float num = (float)(++m_MoneyDropRequests - m_MoneyRequestsMin) / (float)(m_MoneyRequestsMax - m_MoneyRequestsMin);
		if (num < value)
		{
			return null;
		}
		m_MoneyDropRequests = 0;
		GameObject gameObject = m_Cache[1].Get();
		return gameObject.GetComponent<Pickup_Money>();
	}

	public void CollectPickups(ComponentPlayer Comp, float Distance)
	{
		float num = Distance * Distance;
		CharacterController characterController = Comp.Owner.CharacterController;
		for (int num2 = m_Pickups.Count - 1; num2 >= 0; num2--)
		{
			Pickup pickup = m_Pickups[num2];
			if (pickup.CanBePickedUp(false))
			{
				if (!pickup.IsMarked)
				{
					Vector3 vector = characterController.ClosestPointOnBounds(pickup.Position);
					if (Vector3.SqrMagnitude(vector - pickup.Position) <= num)
					{
						pickup.IsMarked |= true;
					}
				}
				if (!(pickup.LifeTime < 1f) && pickup.IsMarked)
				{
					Comp.PickUp(pickup);
					pickup.PickedUp();
				}
			}
		}
	}

	private void Update()
	{
		m_Time2DropAmmo -= Time.deltaTime;
		if (m_AutoCollectZonesChanged)
		{
			UpdatePickupsAutoCollectStatus();
			m_AutoCollectZonesChanged = false;
		}
		for (int num = m_Pickups.Count - 1; num >= 0; num--)
		{
			Pickup pickup = m_Pickups[num];
			if (pickup.IsInvalid)
			{
				if (pickup is Pickup_Ammo)
				{
					m_AmmoInLevel--;
				}
				m_Pickups.RemoveAt(num);
				m_Cache[(int)pickup.GetID()].Return(pickup.GameObj);
			}
		}
	}

	public void OnEndOfMission()
	{
		int num = 0;
		for (int i = 0; i < m_Pickups.Count; i++)
		{
			Pickup_Money pickup_Money = m_Pickups[i] as Pickup_Money;
			if (pickup_Money != null && pickup_Money.CanBePickedUp(true))
			{
				num += pickup_Money.m_Amount;
				pickup_Money.PickedUp();
			}
		}
		Player.Instance.MoneyPickedUp(num, -1);
	}

	public void RegisterAutoCollectZone(PickupAutoCollectZone Zone)
	{
		m_AutoCollectZonesChanged = true;
		m_AutoCollectZones.Add(Zone);
	}

	public void UnregisterAutoCollectZone(PickupAutoCollectZone Zone)
	{
		m_AutoCollectZonesChanged = true;
		m_AutoCollectZones.Remove(Zone);
	}

	private void UpdatePickupsAutoCollectStatus()
	{
	}

	private bool IsInAutoCollectZone(Vector3 Pos)
	{
		foreach (PickupAutoCollectZone autoCollectZone in m_AutoCollectZones)
		{
			if (autoCollectZone.IsInside(Pos))
			{
				return true;
			}
		}
		return false;
	}
}
