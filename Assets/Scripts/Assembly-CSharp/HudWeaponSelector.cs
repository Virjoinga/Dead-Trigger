using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudWeaponSelector : HudComponent
{
	private class WeaponWidgets
	{
		public GUIBase_Widget m_Weapon;

		public GUIBase_Label m_WeaponName;

		public GUIBase_Label m_WeaponAmmo;

		public GUIBase_Widget m_ActiveWeaponBck;

		public bool m_WeaponBlink;
	}

	private GUIBase_Button.TouchDelegate[] s_SelectWeaponDlgt;

	private GUIBase_Layout m_WeaponLayout;

	private GUIBase_Button m_WeaponButton;

	private GUIBase_Button[] m_SelectWeapon;

	private Vector2[] m_OrigSelectWeaponPos;

	private int m_GuiSelectedWeaponIndex;

	private WeaponWidgets m_WeaponWidget;

	private WeaponWidgets[] m_SelectionWidgets;

	private AgentAction m_WaitForEndOfWeaponChange;

	private E_WeaponID[] m_WeaponInventory = new E_WeaponID[4];

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "HUD_Layout";

	private string s_WeaponAmmo = "Weapon_Ammo";

	private string s_WeaponImage = "Weapon_Image";

	private string s_WeaponText = "Weapon_Text";

	private string s_WeaponButtonName = "Weapon";

	private string s_WeaponSelectionImageName = "Weapon_Image";

	private string s_ActiveWeaponBck = "Background";

	private string[] s_WeaponSelectionButtonName = new string[4] { "Weapon1", "Weapon2", "Weapon3", "Weapon4" };

	public override bool VisibleOnStart()
	{
		return true;
	}

	public override void Init()
	{
		s_SelectWeaponDlgt = new GUIBase_Button.TouchDelegate[4] { TouchDelegateWeapon1, TouchDelegateWeapon2, TouchDelegateWeapon3, TouchDelegateWeapon4 };
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(s_PivotMainName);
		if (!pivot)
		{
			Debug.LogError("'" + s_PivotMainName + "' not found!!! Assert should come now");
			return;
		}
		m_WeaponLayout = pivot.GetLayout(s_LayoutMainName);
		if (!m_WeaponLayout)
		{
			Debug.LogError("'" + s_LayoutMainName + "' not found!!! Assert should come now");
			return;
		}
		if (s_WeaponButtonName.Length > 0)
		{
			m_WeaponWidget = new WeaponWidgets();
			m_WeaponButton = GuiBaseUtils.RegisterButtonDelegate(m_WeaponLayout, s_WeaponButtonName, WeaponButtonDelegate, null);
			m_WeaponWidget.m_Weapon = GetChildByName<GUIBase_Widget>(m_WeaponButton, s_WeaponImage);
			m_WeaponWidget.m_WeaponAmmo = GetChildByName<GUIBase_Label>(m_WeaponButton, s_WeaponAmmo);
			m_WeaponWidget.m_WeaponName = GetChildByName<GUIBase_Label>(m_WeaponButton, s_WeaponText);
		}
		m_SelectWeapon = new GUIBase_Button[s_WeaponSelectionButtonName.Length];
		m_SelectionWidgets = new WeaponWidgets[s_WeaponSelectionButtonName.Length];
		m_OrigSelectWeaponPos = new Vector2[s_WeaponSelectionButtonName.Length];
		if (s_WeaponButtonName.Length > 0)
		{
			for (int i = 0; i < s_WeaponSelectionButtonName.Length; i++)
			{
				m_SelectWeapon[i] = GuiBaseUtils.RegisterButtonDelegate(m_WeaponLayout, s_WeaponSelectionButtonName[i], s_SelectWeaponDlgt[i], null);
				m_OrigSelectWeaponPos[i] = m_SelectWeapon[i].transform.position;
				m_SelectionWidgets[i] = new WeaponWidgets();
				m_SelectionWidgets[i].m_WeaponAmmo = null;
				m_SelectionWidgets[i].m_Weapon = GetChildByName<GUIBase_Widget>(m_SelectWeapon[i], s_WeaponSelectionImageName);
				m_SelectionWidgets[i].m_ActiveWeaponBck = GetChildByName<GUIBase_Widget>(m_SelectWeapon[i], s_ActiveWeaponBck);
				m_SelectionWidgets[i].m_WeaponName = null;
			}
		}
		m_GuiSelectedWeaponIndex = 0;
	}

	public override void Reset()
	{
		m_WeaponButton.StopAllCoroutines();
		m_GuiSelectedWeaponIndex = 0;
		ClearWaitingOnWeaponChange();
		if (Player.Instance != null && Player.Instance.Owner != null && Player.Instance.Owner.WeaponComponent != null && Player.Instance.Owner.WeaponComponent.CurrentWeapon != 0)
		{
			OnCurrentWeaponChanged(Player.Instance.Owner.WeaponComponent.CurrentWeapon);
		}
	}

	public override void LateUpdate(float deltaTime)
	{
		base.LateUpdate(deltaTime);
		UpdateAllAmmo();
		UpdateAmmoDisplay(m_WeaponInventory[m_GuiSelectedWeaponIndex], m_WeaponWidget);
		if (m_WaitForEndOfWeaponChange != null && !m_WaitForEndOfWeaponChange.IsActive())
		{
			ClearWaitingOnWeaponChange();
		}
	}

	protected override void ShowWidgets(bool on)
	{
		HideWeaponsSprites();
		if (on)
		{
			ShowWeaponsSprites();
		}
	}

	public override bool Enable(EnableLayer layer, bool enable)
	{
		bool flag = base.Enable(layer, enable);
		if (flag)
		{
			if (IsEnabled())
			{
				HideWeaponsSprites();
				ShowWeaponsSprites();
			}
			else
			{
				ClearWaitingOnWeaponChange();
			}
		}
		return flag;
	}

	public E_WeaponID WeaponID(int index)
	{
		return m_WeaponInventory[index];
	}

	public void OnCurrentWeaponChanged(E_WeaponID Weapon)
	{
		HideWeaponsSprites();
		int guiSelectedWeaponIndex = WeaponIndex(Weapon);
		m_GuiSelectedWeaponIndex = guiSelectedWeaponIndex;
		SetWeaponHudInfo(Weapon, m_WeaponWidget);
		ShowWeaponsSprites();
	}

	public void SetWeapons(List<E_WeaponID> weapons)
	{
		for (int i = 0; i < weapons.Count; i++)
		{
			if (i >= m_WeaponInventory.Length)
			{
				Debug.LogError("Attempt to add more weapons than current hud supoports! Weapons:" + weapons.Count + ",  Hud: " + m_WeaponInventory.Length);
				break;
			}
			m_WeaponInventory[i] = weapons[i];
			SetWeaponHudInfo(weapons[i], m_SelectionWidgets[i]);
		}
	}

	public bool FingerIdInUse(int fingerId)
	{
		if (m_WeaponLayout.FingerIdInUse(fingerId))
		{
			return true;
		}
		return false;
	}

	public override void StoreControlsOrigPositions()
	{
		base.StoreControlsOrigPositions();
		GuiOptions.WeaponButton.OrigPos = new Vector2(m_WeaponButton.transform.position.x, m_WeaponButton.transform.position.y);
	}

	public override void UpdateControlsPosition()
	{
		base.UpdateControlsPosition();
		m_WeaponButton.transform.position = GuiOptions.WeaponButton.Positon;
		for (int i = 0; i < m_SelectWeapon.Length; i++)
		{
			m_SelectWeapon[i].transform.position = m_OrigSelectWeaponPos[i] + GuiOptions.WeaponButton.Offset;
		}
	}

	public override void HandleAction(AgentAction a)
	{
		if (a is AgentActionWeaponChange)
		{
			DisableWeaponSelection(true);
			m_WaitForEndOfWeaponChange = a;
		}
		else if (a is AgentActionDeath)
		{
			ClearWaitingOnWeaponChange();
		}
	}

	private int WeaponIndex(E_WeaponID w)
	{
		for (int i = 0; i < m_WeaponInventory.Length; i++)
		{
			if (m_WeaponInventory[i] == w)
			{
				return i;
			}
		}
		return -1;
	}

	private void ClearWaitingOnWeaponChange()
	{
		DisableWeaponSelection(false);
		m_WaitForEndOfWeaponChange = null;
	}

	private void DisableWeaponSelection(bool disable)
	{
		GUIBase_Button[] selectWeapon = m_SelectWeapon;
		foreach (GUIBase_Button gUIBase_Button in selectWeapon)
		{
			gUIBase_Button.SetDisabled(disable);
		}
	}

	private void WeaponButtonDelegate()
	{
	}

	private void OnWeaponSelectTouchOutside()
	{
		ShowWeaponsSprites();
	}

	private void SelectWeapon(int newWeaponIdx)
	{
		if (Player.Instance.CanChangeWeapon())
		{
			if (newWeaponIdx != m_GuiSelectedWeaponIndex)
			{
				m_GuiSelectedWeaponIndex = newWeaponIdx;
				SetWeaponHudInfo(m_WeaponInventory[m_GuiSelectedWeaponIndex], m_WeaponWidget);
				Player.Instance.Controls.ChangeWeaponDelegate(m_WeaponInventory[m_GuiSelectedWeaponIndex]);
			}
			ShowWeaponsSprites();
		}
	}

	private void SetWeaponHudInfo(E_WeaponID Weapon, WeaponWidgets Widget)
	{
		WeaponSettings weaponSettings = WeaponSettingsManager.Instance.Get(Weapon);
		Widget.m_Weapon.CopyMaterialSettings(weaponSettings.HudWidget);
		if ((bool)Widget.m_WeaponName)
		{
			Widget.m_WeaponName.SetNewText(weaponSettings.Name);
		}
	}

	private void ShowWeaponsSprites()
	{
		if (m_WeaponButton != null)
		{
			m_WeaponButton.Widget.Show(true, true);
			UpdateAmmoDisplay(m_WeaponInventory[m_GuiSelectedWeaponIndex], m_WeaponWidget);
		}
		for (int i = 0; i < m_SelectWeapon.Length; i++)
		{
			if (m_WeaponInventory[i] != 0)
			{
				m_SelectWeapon[i].Widget.Show(true, true);
				m_SelectionWidgets[i].m_ActiveWeaponBck.Show(i == m_GuiSelectedWeaponIndex, true);
			}
			else
			{
				m_SelectWeapon[i].Widget.Show(false, true);
				m_SelectionWidgets[i].m_ActiveWeaponBck.Show(false, true);
			}
		}
	}

	private void HideWeaponsSprites()
	{
		if (m_WeaponButton != null)
		{
			m_WeaponButton.Widget.Show(false, true);
		}
		GUIBase_Button[] selectWeapon = m_SelectWeapon;
		foreach (GUIBase_Button gUIBase_Button in selectWeapon)
		{
			gUIBase_Button.Widget.Show(false, true);
		}
	}

	private void TouchDelegateWeapon1()
	{
		SelectWeapon(0);
	}

	private void TouchDelegateWeapon2()
	{
		SelectWeapon(1);
	}

	private void TouchDelegateWeapon3()
	{
		SelectWeapon(2);
	}

	private void TouchDelegateWeapon4()
	{
		SelectWeapon(3);
	}

	private T GetChildByName<T>(GUIBase_Button btn, string name) where T : Component
	{
		Transform transform = btn.transform.Find(name);
		return (!(transform != null)) ? ((T)null) : transform.GetComponent<T>();
	}

	private void UpdateAmmoDisplay(E_WeaponID Weapon, WeaponWidgets Widget)
	{
		if (Player.Instance == null || Player.Instance.Owner == null || Player.Instance.Owner.WeaponComponent == null || Player.Instance.Owner.WeaponComponent.CurrentWeapon == E_WeaponID.None)
		{
			return;
		}
		WeaponBase weapon = Player.Instance.Owner.WeaponComponent.GetWeapon(Weapon);
		if (weapon == null || Widget == null)
		{
			return;
		}
		if (Widget.m_WeaponAmmo != null)
		{
			if (weapon.WeaponID == E_WeaponID.Propeller || weapon.WeaponID == E_WeaponID.Chainsaw)
			{
				string newText = (((float)weapon.WeaponAmmo + (float)weapon.ClipAmmo) / 100f).ToString("F2");
				Widget.m_WeaponAmmo.SetNewText(newText);
			}
			else
			{
				Widget.m_WeaponAmmo.SetNewText((weapon.WeaponAmmo + weapon.ClipAmmo).ToString());
			}
			if (weapon.IsCriticalAmmo)
			{
				if (!Widget.m_WeaponBlink)
				{
					Widget.m_Weapon.StartCoroutine(HighlightObject(Widget));
					Widget.m_WeaponBlink = true;
				}
				return;
			}
			Widget.m_WeaponAmmo.Widget.m_Color = Color.white;
			if (Widget.m_WeaponBlink)
			{
				Widget.m_WeaponBlink = false;
				Widget.m_Weapon.StopAllCoroutines();
				ResetHighlight(Widget);
			}
		}
		else if ((bool)Widget.m_Weapon && !weapon.HasAnyAmmo)
		{
			if (Widget.m_Weapon.m_Color != Color.red)
			{
				Widget.m_Weapon.m_Color = Color.red;
			}
		}
		else if (Widget.m_Weapon.m_Color != Color.white)
		{
			Widget.m_Weapon.m_Color = Color.white;
		}
	}

	private IEnumerator HighlightObject(WeaponWidgets widgets)
	{
		while (true)
		{
			widgets.m_Weapon.m_Color = Color.red;
			widgets.m_WeaponAmmo.Widget.m_Color = Color.red;
			yield return new WaitForSeconds(0.25f);
			widgets.m_Weapon.m_Color = Color.white;
			widgets.m_WeaponAmmo.Widget.m_Color = Color.white;
			yield return new WaitForSeconds(0.25f);
		}
	}

	private void ResetHighlight(WeaponWidgets widgets)
	{
		widgets.m_Weapon.m_Color = Color.white;
		widgets.m_WeaponAmmo.Widget.m_Color = Color.white;
	}

	private void UpdateAllAmmo()
	{
		for (int i = 0; i < m_WeaponInventory.Length; i++)
		{
			UpdateAmmoDisplay(m_WeaponInventory[i], m_SelectionWidgets[i]);
		}
	}
}
