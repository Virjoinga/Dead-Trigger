using UnityEngine;

public class GuiEquipMenu : BaseMenuScreen
{
	public delegate void HideDelegate();

	private const int maxWeaponSlot = 4;

	private const int maxItemsSlot = 4;

	public static GuiEquipMenu Instance;

	private GUIBase_Pivot m_Pivot;

	private GUIBase_Layout m_Layout;

	private GUIBase_Sprite m_HighlightSprite;

	private GUIBase_Button m_AmmoUpg_Button;

	private GUIBase_Button m_HealthUpg_Button;

	private GUIBase_Button m_RadarUpg_Button;

	private GUIBase_Button m_AutohealUpg_Button;

	private GUIBase_Widget m_TutorialEquip;

	private GUIBase_Button m_BackButton;

	private float m_PulseBackButton;

	public HideDelegate m_OnHideDelegate;

	private GuiEquipSlots m_WeaponSlots = new GuiEquipSlots(GuiShop.E_ItemType.Weapon);

	private GuiEquipSlots m_ItemSlots = new GuiEquipSlots(GuiShop.E_ItemType.Item);

	private GuiShop.E_ItemType m_CurrentSlotType = GuiShop.E_ItemType.Weapon;

	private int m_CurrentSlotIndex;

	private GUIBase_Button[] m_BuySlotGun = new GUIBase_Button[4];

	private GUIBase_Button[] m_BuySlotItem = new GUIBase_Button[4];

	private GUIBase_Button.ReleaseDelegate[] m_OnUpgrade;

	private GUIBase_Button.ReleaseDelegate[] m_OnBuyMore;

	public bool IsShown { get; private set; }

	public bool TutorialMode { get; set; }

	private void Awake()
	{
		Instance = this;
		IsShown = false;
		if (ShopDataBridge.Instance == null)
		{
			ShopDataBridge.CreateInstance();
		}
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			GuiEquipSelection.Instance.InitGui();
			m_Pivot = MFGuiManager.Instance.GetPivot("EquipMenu");
			m_Layout = m_Pivot.GetLayout("Main_Layout");
			m_BackButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "Back_Button", null, OnButtonBack);
			m_RadarUpg_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "RadarUpg_Button", null, OnButtonUpgRadar);
			m_HealthUpg_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "HealthUpg_Button", null, OnButtonUpgHealth);
			m_AmmoUpg_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "AmmoUpg_Button", null, OnButtonUpgAmmo);
			m_AutohealUpg_Button = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "AutohealUpg_Button", null, OnButtonUpgAutoheal);
			m_HighlightSprite = GuiBaseUtils.PrepareSprite(m_Layout, "Highlight_Sprite");
			m_BuySlotGun[2] = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "BuySlotGun2", null, OnBuySlotGun2);
			m_BuySlotGun[3] = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "BuySlotGun3", null, OnBuySlotGun3);
			m_BuySlotItem[2] = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "BuySlotItem2", null, OnBuySlotItem2);
			m_BuySlotItem[3] = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "BuySlotItem3", null, OnBuySlotItem3);
			m_OnUpgrade = new GUIBase_Button.ReleaseDelegate[4] { OnBuyUpgrade0, OnBuyUpgrade1, OnBuyUpgrade2, OnBuyUpgrade3 };
			m_OnBuyMore = new GUIBase_Button.ReleaseDelegate[4] { OnBuyMore0, OnBuyMore1, OnBuyMore2, OnBuyMore3 };
			m_TutorialEquip = m_Layout.GetWidget("TutorialEquip");
			for (int i = 0; i < 4; i++)
			{
				GUIBase_Button button = GuiBaseUtils.GetButton(m_Layout, "Gun_Button" + i);
				SlotViewWeapon slotViewWeapon = new SlotViewWeapon();
				slotViewWeapon.InitGui(m_Layout, button, i);
				m_WeaponSlots.AddSlot(slotViewWeapon, button);
				GUIBase_Button childButton = GuiBaseUtils.GetChildButton(button.Widget, "Upgrade_Button");
				childButton.RegisterReleaseDelegate(m_OnUpgrade[i]);
				m_WeaponSlots.m_OnSlotSelectionDone = OnWeaponSelected;
			}
			for (int j = 0; j < 4; j++)
			{
				GUIBase_Button button2 = GuiBaseUtils.GetButton(m_Layout, "Item_Button" + j);
				SlotViewItem slotViewItem = new SlotViewItem();
				slotViewItem.InitGui(m_Layout, button2, j);
				m_ItemSlots.AddSlot(slotViewItem, button2);
				GUIBase_Button childButton2 = GuiBaseUtils.GetChildButton(button2.Widget, "BuyMore_Button");
				childButton2.RegisterReleaseDelegate(m_OnBuyMore[j]);
				m_ItemSlots.m_OnSlotSelectionDone = OnItemSelected;
			}
			UpdateLockedSlots();
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	public void OnUpdatePPIInfo()
	{
		SyncSlotsWithPPI();
		m_WeaponSlots.Show();
		m_ItemSlots.Show();
	}

	private void SyncSlotsWithPPI()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!ShopDataBridge.Instance.IsWeaponSlotLocked(i))
			{
				m_WeaponSlots.InitItemInSlot(ShopDataBridge.Instance.GetWeaponInSlot(i), i);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (!ShopDataBridge.Instance.IsItemSlotLocked(j))
			{
				m_ItemSlots.InitItemInSlot(ShopDataBridge.Instance.GetItemInSlot(j), j);
			}
		}
		UpdateLockedSlots();
	}

	protected override void OnGUI_Show()
	{
		SyncSlotsWithPPI();
		MFGuiManager.Instance.ShowPivot(m_Pivot, true);
		m_WeaponSlots.Show();
		m_ItemSlots.Show();
		IsShown = true;
		base.OnGUI_Show();
		ShopItemId lastSelectedItem = GuiEquipSelection.Instance.LastSelectedItem;
		SelectSlot(m_CurrentSlotType, m_CurrentSlotIndex);
		if (lastSelectedItem != ShopItemId.EmptyId)
		{
			GuiEquipSelection.Instance.SelectItem(lastSelectedItem);
		}
		UpdateUpgradesButtons();
		UpdateLockedSlots();
		if (TutorialMode)
		{
			m_TutorialEquip.Show(true, true);
			DisableByTutorial(true);
			StartHighlightBackButton();
		}
		else
		{
			DisableByTutorial(false);
		}
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_Pivot, false);
		GuiEquipSelection.Instance.Hide();
		m_WeaponSlots.Hide();
		m_ItemSlots.Hide();
		IsShown = false;
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		base.OnGUI_Update();
		if (TutorialMode && m_PulseBackButton > 0f)
		{
			m_BackButton.Widget.m_FadeAlpha = GuiShopMenu.PulseAlpha(m_PulseBackButton);
		}
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		m_Pivot.EnableControls(true);
		SelectSlot(m_CurrentSlotType, m_CurrentSlotIndex);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_Pivot.EnableControls(false);
		GuiEquipSelection.Instance.Hide();
		base.OnGUI_Disable();
	}

	public void UpdateLockedSlots()
	{
		for (int i = 0; i < 4; i++)
		{
			bool flag = ShopDataBridge.Instance.IsWeaponSlotLocked(i);
			m_WeaponSlots.SetSlotLocked(i, flag);
			if (m_BuySlotGun[i] != null && IsShown)
			{
				m_BuySlotGun[i].Widget.Show(flag, true);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			bool flag2 = ShopDataBridge.Instance.IsItemSlotLocked(j);
			m_ItemSlots.SetSlotLocked(j, flag2);
			if (m_BuySlotItem[j] != null && IsShown)
			{
				m_BuySlotItem[j].Widget.Show(flag2, true);
			}
		}
	}

	private void OnButtonBack(bool inside)
	{
		if (inside)
		{
			if (TutorialMode)
			{
				StopHighlightBackButton();
				TutorialMode = false;
			}
			m_OwnerMenu.Back();
			Game.Instance.PlayerPersistentInfo.Save();
		}
	}

	private void SelectSlot(GuiShop.E_ItemType slotType, int slotIndex)
	{
		switch (slotType)
		{
		case GuiShop.E_ItemType.Weapon:
			m_WeaponSlots.SelectSlotHACK(slotIndex);
			break;
		case GuiShop.E_ItemType.Item:
			m_ItemSlots.SelectSlotHACK(slotIndex);
			break;
		}
	}

	private void OnButtonUpgRadar(bool inside)
	{
		if (inside)
		{
			BuyDirectly(ShopDataBridge.Instance.NextRadarUpg());
		}
	}

	private void OnButtonUpgAmmo(bool inside)
	{
		if (inside)
		{
			BuyDirectly(ShopDataBridge.Instance.NextAmmoUpg());
		}
	}

	private void OnButtonUpgAutoheal(bool inside)
	{
		if (inside)
		{
			BuyDirectly(ShopDataBridge.Instance.NextAutohealUpg());
		}
	}

	private void OnButtonUpgHealth(bool inside)
	{
		if (inside)
		{
			BuyDirectly(ShopDataBridge.Instance.NextHealthUpg());
		}
	}

	private void OnBuySlotGun2(bool inside)
	{
		if (inside)
		{
			BuyDirectly(new ShopItemId(2, GuiShop.E_ItemType.Upgrade));
		}
	}

	private void OnBuySlotGun3(bool inside)
	{
		if (inside)
		{
			int id = ((!ShopDataBridge.Instance.HasOwnedUpgrade(E_UpgradeID.WeaponSlot2)) ? 1 : 2);
			BuyDirectly(new ShopItemId(id, GuiShop.E_ItemType.Upgrade));
		}
	}

	private void OnBuySlotItem2(bool inside)
	{
		if (inside)
		{
			BuyDirectly(new ShopItemId(5, GuiShop.E_ItemType.Upgrade));
		}
	}

	private void OnBuySlotItem3(bool inside)
	{
		if (inside)
		{
			int id = ((!ShopDataBridge.Instance.HasOwnedUpgrade(E_UpgradeID.ItemSlot2)) ? 4 : 5);
			BuyDirectly(new ShopItemId(id, GuiShop.E_ItemType.Upgrade));
		}
	}

	private void BuyDirectly(ShopItemId selId)
	{
		if (!selId.IsEmpty())
		{
			GuiShopBuyPopup.Instance.SetCaptionID(GuiShopBuyPopup.E_BuyType.Buy);
			GuiShopBuyPopup.Instance.SetBuyItem(selId);
			m_OwnerMenu.ShowPopup("ShopBuyPopup", string.Empty, string.Empty, BuyResultHandler);
			Debug.Log("Buy directly: " + selId);
		}
	}

	private void OnWeaponSelected(int slotIndex)
	{
	}

	private void OnItemSelected(int slotIndex)
	{
	}

	public void HighlightSlot(GuiShop.E_ItemType slotType, int slotIndex)
	{
		m_HighlightSprite.Widget.Show(true, true);
		switch (slotType)
		{
		case GuiShop.E_ItemType.Weapon:
			m_HighlightSprite.Widget.transform.position = m_WeaponSlots.GetHighlightPos(slotIndex);
			break;
		case GuiShop.E_ItemType.Item:
			m_HighlightSprite.Widget.transform.position = m_ItemSlots.GetHighlightPos(slotIndex);
			break;
		}
		m_CurrentSlotType = slotType;
		m_CurrentSlotIndex = slotIndex;
	}

	private bool ShopAvailable()
	{
		return Game.Instance.PlayerPersistentInfo.storyId >= 2;
	}

	private void UpdateUpgradesButtons()
	{
		UpdateAmmoUpg();
		UpdateHealthUpg();
		UpdateRadarUpg();
		UpdateAutohealUpg();
	}

	private void UpdateAmmoUpg()
	{
		ShopDataBridge.UpgradeInfo upgradeInfo = ShopDataBridge.Instance.AmmoUpgInfo();
		GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(m_AmmoUpg_Button.Widget, "Ammo_Label");
		childLabel.SetNewText("+" + upgradeInfo.Value);
		childLabel = GuiBaseUtils.GetChildLabel(m_AmmoUpg_Button.Widget, "AmmoUpg_Label");
		childLabel.SetNewText(TextDatabase.instance[2050025] + " " + (upgradeInfo.Level + 1));
		m_AmmoUpg_Button.SetDisabled(upgradeInfo.Level >= upgradeInfo.MaxLevel || !ShopAvailable());
	}

	private void UpdateHealthUpg()
	{
		ShopDataBridge.UpgradeInfo upgradeInfo = ShopDataBridge.Instance.HealthUpgInfo();
		GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(m_HealthUpg_Button.Widget, "Health_Label");
		childLabel.SetNewText(upgradeInfo.Value.ToString());
		childLabel = GuiBaseUtils.GetChildLabel(m_HealthUpg_Button.Widget, "HealthUpg_Label");
		childLabel.SetNewText(TextDatabase.instance[2050024] + " " + (upgradeInfo.Level + 1));
		m_HealthUpg_Button.SetDisabled(upgradeInfo.Level >= upgradeInfo.MaxLevel || !ShopAvailable());
	}

	private void UpdateRadarUpg()
	{
		ShopDataBridge.UpgradeInfo upgradeInfo = ShopDataBridge.Instance.RadarUpgInfo();
		GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(m_RadarUpg_Button.Widget, "Radar_Label");
		childLabel.SetNewText(upgradeInfo.Value.ToString());
		childLabel = GuiBaseUtils.GetChildLabel(m_RadarUpg_Button.Widget, "RadarUpg_Label");
		childLabel.SetNewText(TextDatabase.instance[2050026] + " " + (upgradeInfo.Level + 1));
		m_RadarUpg_Button.SetDisabled(upgradeInfo.Level >= upgradeInfo.MaxLevel || !ShopAvailable());
	}

	private void UpdateAutohealUpg()
	{
		ShopDataBridge.UpgradeInfo upgradeInfo = ShopDataBridge.Instance.AutohealUpgInfo();
		GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(m_AutohealUpg_Button.Widget, "Autoheal_Label");
		childLabel.SetNewText((upgradeInfo.Level <= 0) ? TextDatabase.instance[2050033] : TextDatabase.instance[2050032]);
		m_AutohealUpg_Button.SetDisabled(upgradeInfo.Level >= upgradeInfo.MaxLevel || !ShopAvailable());
	}

	public void TryEquipBoughtItem(ShopItemId item)
	{
		if (item.ItemType == GuiShop.E_ItemType.Weapon)
		{
			int freeSlotIndex = m_WeaponSlots.GetFreeSlotIndex();
			if (freeSlotIndex >= 0)
			{
				ShopDataBridge.Instance.Action_Equip(item, freeSlotIndex);
			}
		}
		else if (item.ItemType == GuiShop.E_ItemType.Item)
		{
			int freeSlotIndex2 = m_ItemSlots.GetFreeSlotIndex();
			if (freeSlotIndex2 >= 0)
			{
				ShopDataBridge.Instance.Action_Equip(item, freeSlotIndex2);
			}
		}
	}

	private void OnBuyUpgrade0(bool inside)
	{
		if (inside)
		{
			BuyUpgrade(0);
		}
	}

	private void OnBuyUpgrade1(bool inside)
	{
		if (inside)
		{
			BuyUpgrade(1);
		}
	}

	private void OnBuyUpgrade2(bool inside)
	{
		if (inside)
		{
			BuyUpgrade(2);
		}
	}

	private void OnBuyUpgrade3(bool inside)
	{
		if (inside)
		{
			BuyUpgrade(3);
		}
	}

	private void BuyUpgrade(int slot)
	{
		ShopItemId slotItem = m_WeaponSlots.GetSlotItem(slot);
		if (!slotItem.IsEmpty())
		{
			GuiShopUpgradePopup.Instance.SetBuyItem(slotItem);
			m_OwnerMenu.ShowPopup("ShopUpgradePopup", string.Empty, string.Empty, UpgradeResultHandler);
			Debug.Log(string.Concat("Buy Upgrade: ", slotItem, ", slot ", slot));
		}
	}

	private void OnBuyMore0(bool inside)
	{
		if (inside)
		{
			BuyMore(0);
		}
	}

	private void OnBuyMore1(bool inside)
	{
		if (inside)
		{
			BuyMore(1);
		}
	}

	private void OnBuyMore2(bool inside)
	{
		if (inside)
		{
			BuyMore(2);
		}
	}

	private void OnBuyMore3(bool inside)
	{
		if (inside)
		{
			BuyMore(3);
		}
	}

	private void BuyMore(int slot)
	{
		ShopItemId slotItem = m_ItemSlots.GetSlotItem(slot);
		if (!slotItem.IsEmpty())
		{
			GuiShopBuyPopup.Instance.SetCaptionID(GuiShopBuyPopup.E_BuyType.Buy);
			GuiShopBuyPopup.Instance.SetBuyItem(slotItem);
			m_OwnerMenu.ShowPopup("ShopBuyPopup", string.Empty, string.Empty, BuyResultHandler);
		}
	}

	private void UpgradeResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			ShopItemId buyItem = GuiShopUpgradePopup.Instance.GetBuyItem();
			if (!buyItem.IsEmpty())
			{
				ShopDataBridge.Instance.UpgradeWeapon(buyItem);
				Game.Instance.PlayerPersistentInfo.Save();
			}
			m_WeaponSlots.Show();
			GuiEquipSelection.Instance.UpdateScrollerView();
			GuiEquipSelection.Instance.UpdateItemButtons();
		}
	}

	private void BuyResultHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Success)
		{
			ShopItemId buyItem = GuiShopBuyPopup.Instance.GetBuyItem();
			if (buyItem.ItemType != GuiShop.E_ItemType.Fund)
			{
				ShopDataBridge.Instance.SynchroniseBoughtItem(buyItem);
				Instance.TryEquipBoughtItem(buyItem);
				Game.Instance.PlayerPersistentInfo.Save();
			}
			m_ItemSlots.Show();
			GuiEquipSelection.Instance.UpdateScrollerView();
			GuiEquipSelection.Instance.UpdateItemButtons();
		}
	}

	private void DisableByTutorial(bool dis)
	{
		m_RadarUpg_Button.SetDisabled(dis);
		m_HealthUpg_Button.SetDisabled(dis);
		m_AmmoUpg_Button.SetDisabled(dis);
		m_AutohealUpg_Button.SetDisabled(dis);
		for (int i = 0; i < 4; i++)
		{
			GUIBase_Button gUIBase_Button = m_BuySlotGun[i];
			if (gUIBase_Button != null)
			{
				gUIBase_Button.SetDisabled(dis);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			GUIBase_Button gUIBase_Button2 = m_BuySlotItem[j];
			if (gUIBase_Button2 != null)
			{
				gUIBase_Button2.SetDisabled(dis);
			}
		}
	}

	private void StartHighlightBackButton()
	{
		m_PulseBackButton = Time.time;
		m_BackButton.Widget.m_FadeAlpha = 1f;
	}

	private void StopHighlightBackButton()
	{
		m_PulseBackButton = 0f;
		m_BackButton.Widget.m_FadeAlpha = 1f;
	}
}
