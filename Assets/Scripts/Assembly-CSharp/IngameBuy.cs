using System.Collections.Generic;
using UnityEngine;

[NESEvent(new string[] { "AreaUnlocked" })]
[RequireComponent(typeof(Trigger))]
public class IngameBuy : MonoBehaviour
{
	public IngameBuyType m_Type;

	public int m_BaseCostInMoneyBags;

	public int m_WavesToEnableFirstTime;

	public float m_ColldownWaves;

	private MappingChange m_MappingChange;

	private bool m_Enabled;

	private bool m_TriggerActivated;

	private bool m_AngleOK;

	private bool m_ImmediateAngleOK;

	private bool m_Visible;

	private float m_TimeAngleOK;

	private int m_CooldownKills;

	private int m_StartCooldownKills;

	private List<GUIBase_Widget> m_Icons = new List<GUIBase_Widget>();

	private List<E_ItemID> m_Powerups = new List<E_ItemID>();

	private int m_ActualPowerup;

	private NESController m_NESController;

	private int m_Description;

	private int[] TextIds = new int[4] { 3000400, 3000410, 3000415, 3000420 };

	public void WaveStart(int wave)
	{
	}

	public void WaveFinished(int wave)
	{
	}

	public int Description()
	{
		return m_Description;
	}

	public int GetAdditionalDescription()
	{
		if (m_Type != IngameBuyType.PowerUp)
		{
			return 0;
		}
		ItemSettings itemSettings = ItemSettingsManager.Instance.Get(m_Powerups[m_ActualPowerup]);
		return itemSettings.Name;
	}

	public GUIBase_Widget GetIcon()
	{
		if (m_Type != IngameBuyType.PowerUp)
		{
			return m_Icons[0];
		}
		return m_Icons[m_ActualPowerup];
	}

	public CanBuy IsBuyPossible()
	{
		int cost = GetCost();
		if (IsBusy())
		{
			return CanBuy.CoolDown;
		}
		if (cost == 0 && m_Type == IngameBuyType.RestoreHealth)
		{
			return CanBuy.HealthFull;
		}
		if (cost == 0 && m_Type == IngameBuyType.RefillAmmo)
		{
			return CanBuy.AmmoFull;
		}
		if (cost <= GetPPIMoney())
		{
			return CanBuy.Yes;
		}
		return CanBuy.NotEnoughMoney;
	}

	public int GetCost()
	{
		float num = m_BaseCostInMoneyBags * GameplayData.Instance.MoneyPerZombie();
		if (Player.Instance == null)
		{
			return 0;
		}
		if (m_Type == IngameBuyType.RestoreHealth)
		{
			float num2 = Player.Instance.Owner.BlackBoard.RealMaxHealth - Player.Instance.Owner.BlackBoard.Health;
			num = num2 / 100f * num;
		}
		else if (m_Type == IngameBuyType.RefillAmmo)
		{
			float num3 = 0f;
			float num4 = 0f;
			foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in Player.Instance.Owner.WeaponComponent.Weapons)
			{
				WeaponBase value = weapon.Value;
				num3 += (float)(value.MaxAmmoInClip + value.MaxAmmoInWeapon);
				num4 += (float)(value.WeaponAmmo + value.ClipAmmo);
			}
			num = (1f - num4 / num3) * num;
		}
		return Mathf.FloorToInt(num);
	}

	public int GetPPIMoney()
	{
		int money = Game.Instance.PlayerPersistentInfo.money;
		int roundMoney = Game.Instance.PlayerPersistentInfo.roundMoney;
		return money + roundMoney;
	}

	[NESAction]
	public void EnableIngameBuy(bool enable)
	{
		m_Enabled = enable;
		m_TriggerActivated = false;
		m_AngleOK = false;
		base.gameObject._SetActiveRecursively(enable);
	}

	public bool NothingToBuy()
	{
		int cost = GetCost();
		if (cost == 0 && m_Type == IngameBuyType.RestoreHealth)
		{
			return true;
		}
		if (cost == 0 && m_Type == IngameBuyType.RefillAmmo)
		{
			return true;
		}
		return false;
	}

	private void LoadNewIcon(string name)
	{
		GameObject gameObject = Resources.Load("IngameBuy/" + name) as GameObject;
		if ((bool)gameObject)
		{
			m_Icons.Add(gameObject.GetComponent<GUIBase_Widget>());
		}
		else
		{
			Debug.Log("Missing inagame buy icon: " + name);
		}
	}

	private void Initialize()
	{
		m_Description = TextIds[(int)m_Type];
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		switch (m_Type)
		{
		case IngameBuyType.RestoreHealth:
			LoadNewIcon("RestoreHealthIcon");
			break;
		case IngameBuyType.RefillAmmo:
			LoadNewIcon("RefillAmmoIcon");
			break;
		case IngameBuyType.PowerUp:
		{
			m_Powerups.Add(E_ItemID.BoosterDamage);
			m_Powerups.Add(E_ItemID.BoosterSpeed);
			m_Powerups.Add(E_ItemID.BigHeads);
			for (int i = 0; i < m_Powerups.Count; i++)
			{
				if (Game.Instance.PlayerPersistentInfo.EquipList.ContainsItem(m_Powerups[i]))
				{
					m_Powerups.RemoveAt(i);
					i--;
				}
			}
			m_Powerups.Shuffle();
			foreach (E_ItemID powerup in m_Powerups)
			{
				string text = string.Empty;
				switch (powerup)
				{
				case E_ItemID.BoosterDamage:
					text = "DamageBoosterIcon";
					break;
				case E_ItemID.BoosterSpeed:
					text = "SpeedBoosterIcon";
					break;
				case E_ItemID.BigHeads:
					text = "BigHeadsIcon";
					break;
				default:
					Debug.LogWarning("Unknown powerup ID!");
					break;
				}
				LoadNewIcon(text);
			}
			break;
		}
		case IngameBuyType.UnlockArea:
			LoadNewIcon("UnlockAreaIcon");
			break;
		default:
			Debug.LogWarning("Unknown ingame buy type");
			break;
		}
		if (m_WavesToEnableFirstTime > 0)
		{
			SetCoolDown(m_WavesToEnableFirstTime * ArenaDirector.Instance.ENEMIES_IN_WAVE);
		}
	}

	private void Awake()
	{
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		m_MappingChange = GetComponentInChildren<MappingChange>();
	}

	private void Start()
	{
		Initialize();
	}

	private void SetCoolDown(float enemiesToKill)
	{
		if (enemiesToKill <= 0f)
		{
			m_CooldownKills = Mathf.FloorToInt(enemiesToKill);
			m_StartCooldownKills = 0;
		}
		else
		{
			m_CooldownKills = Mathf.FloorToInt((float)Game.Instance.MissionResultData.KilledZombies + enemiesToKill);
			m_StartCooldownKills = Game.Instance.MissionResultData.KilledZombies;
		}
	}

	private void ApplyBuyCost()
	{
		int cost = GetCost();
		Game.Instance.PlayerPersistentInfo.AddRoundMoney(-cost);
		Game.Instance.MissionResultData.SpentMoney += cost;
	}

	private void IngameBuyFinished(bool buyPressed)
	{
		GuiHUD.Instance.HideIngameBuy();
		m_Visible = false;
		if (buyPressed && IsBuyPossible() == CanBuy.Yes)
		{
			ApplyBuyCost();
			switch (m_Type)
			{
			case IngameBuyType.RestoreHealth:
				Player.Instance.Owner.BlackBoard.Health = Player.Instance.Owner.BlackBoard.RealMaxHealth;
				break;
			case IngameBuyType.RefillAmmo:
				foreach (KeyValuePair<E_WeaponID, WeaponBase> weapon in Player.Instance.Owner.WeaponComponent.Weapons)
				{
					WeaponBase value = weapon.Value;
					value.SetAmmo(value.MaxAmmoInClip, value.MaxAmmoInWeapon);
				}
				break;
			case IngameBuyType.PowerUp:
				Player.Instance.AddPowerup(m_Powerups[m_ActualPowerup], ArenaDirector.Instance.ENEMIES_IN_WAVE);
				m_ActualPowerup++;
				if (m_ActualPowerup >= m_Powerups.Count)
				{
					m_ActualPowerup = 0;
				}
				break;
			case IngameBuyType.UnlockArea:
				if ((bool)m_NESController)
				{
					m_NESController.SendGameEvent(this, "AreaUnlocked");
				}
				break;
			default:
				Debug.LogWarning("Unknown ingame buy type");
				break;
			}
			Enable(false);
			if (m_ColldownWaves < 0f)
			{
				SetCoolDown(m_ColldownWaves);
			}
			else
			{
				SetCoolDown(m_ColldownWaves * (float)ArenaDirector.Instance.ENEMIES_IN_WAVE);
			}
		}
		else
		{
			Debug.Log("IngameBuy exit");
		}
	}

	private void DialogStateChanged()
	{
		if (m_TriggerActivated && m_Enabled && m_AngleOK)
		{
			if (!m_Visible)
			{
				GuiHUD.Instance.ShowIngameBuy(this, IngameBuyFinished);
				m_Visible = true;
			}
		}
		else if (m_Visible)
		{
			GuiHUD.Instance.HideIngameBuy();
			m_Visible = false;
		}
	}

	private void Enable(bool on)
	{
		m_Enabled = on;
		DialogStateChanged();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(Player.Instance == null) && !(other != Player.Instance.Owner.CharacterController))
		{
			m_TriggerActivated = true;
			DialogStateChanged();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!(Player.Instance == null) && !(other != Player.Instance.Owner.CharacterController))
		{
			m_TriggerActivated = false;
			DialogStateChanged();
		}
	}

	private bool IsBusy()
	{
		if (m_CooldownKills <= Game.Instance.MissionResultData.KilledZombies && m_CooldownKills >= 0 && (m_Type != IngameBuyType.PowerUp || m_Powerups.Count > 0))
		{
			return false;
		}
		return true;
	}

	private float CoolDownProgress()
	{
		if (m_CooldownKills > 0)
		{
			return (float)(Game.Instance.MissionResultData.KilledZombies - m_StartCooldownKills) / (float)(m_CooldownKills - m_StartCooldownKills);
		}
		return 1f;
	}

	private void Update()
	{
		if (m_TriggerActivated)
		{
			bool angleOK = m_AngleOK;
			Vector3 forward = Camera.main.transform.forward;
			Vector3 to = base.transform.position - Camera.main.transform.position;
			forward.y = 0f;
			to.y = 0f;
			float num = Vector3.Angle(forward, to);
			bool flag = num < 35f;
			if (flag)
			{
				if (!m_AngleOK)
				{
					m_TimeAngleOK += TimeManager.Instance.GetRealDeltaTime();
					if (m_TimeAngleOK > 0.8f)
					{
						m_AngleOK = true;
					}
				}
			}
			else
			{
				if (m_AngleOK || m_ImmediateAngleOK)
				{
					m_TimeAngleOK = 0f;
				}
				else
				{
					m_TimeAngleOK += TimeManager.Instance.GetRealDeltaTime();
				}
				m_AngleOK = false;
			}
			m_ImmediateAngleOK = flag;
			if (m_AngleOK != angleOK)
			{
				DialogStateChanged();
			}
		}
		else
		{
			m_TimeAngleOK += TimeManager.Instance.GetRealDeltaTime();
		}
		bool flag2 = m_Enabled;
		bool flag3 = IsBusy();
		bool flag4 = NothingToBuy();
		if ((bool)m_MappingChange)
		{
			if (!flag2)
			{
				m_MappingChange.TextureColor = Color.red;
			}
			else if (flag3 || flag4)
			{
				m_MappingChange.TextureColor = Color.gray;
			}
			else
			{
				m_MappingChange.TextureColor = Color.green;
			}
			m_MappingChange.Value = 1f - CoolDownProgress();
		}
		if (!flag2 && !flag3 && !flag4)
		{
			Enable(true);
		}
	}
}
