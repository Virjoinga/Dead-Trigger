using System.Collections;
using UnityEngine;

public class GuiShopFreeGold : BaseMenuScreen
{
	private GUIBase_Layout m_Layout;

	private GUIBase_Button m_RewardedVideoButton;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("CityScreens");
			m_Layout = pivot.GetLayout("FreeGold");
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "ButtonClose", null, OnButtonBack);
			GuiBaseUtils.RegisterButtonDelegate(m_Layout, "ButtonGold2", null, OnButtonGold);
			m_RewardedVideoButton = GuiBaseUtils.RegisterButtonDelegate(m_Layout, "ButtonGoldVideo", null, OnButtonVideo);
			PrepareButton(m_Layout, "ButtonMoreApps", null, OnButtonMoreApps);
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		MFGuiManager.Instance.ShowLayout(m_Layout, true);
		m_RewardedVideoButton.SetDisabled(true);
		//TapjoyWrapper.GetFullScreenAd();
		//StartCoroutine(WaitForVideoLoaded());
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		//StopCoroutine(WaitForVideoLoaded());
		MFGuiManager.Instance.ShowLayout(m_Layout, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		Debug.Log("Enable Shop menu");
		m_Layout.EnableControls(true);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_Layout.EnableControls(false);
		base.OnGUI_Disable();
	}

	private void OnButtonBack(bool inside)
	{
		if (inside)
		{
			m_OwnerMenu.Back();
			if (CityManager.Instance != null && !CityManager.Instance.hasSpentRealMoney)
			{
				//Advertisement.Instance.ShowInterstitialAd("FreeGold");
			}
		}
	}

	private void OnButtonGold(bool inside)
	{
		if (inside)
		{
			ShopItemId goldId = new ShopItemId(13, GuiShop.E_ItemType.Fund);
			GuiShopMenu.ShowFreeGoldOffer(goldId);
		}
	}

	private void OnButtonVideo(bool inside)
	{
		if (inside)
		{
			//Advertisement.RemoveOtherCallbacksAndAddGoldReward();
			/*if (TapjoyWrapper.IsFullScreenAdLoaded())
			{
				TapjoyWrapper.ShowFullScreenAd();
			}*/
			/*else if (Advertisement.IsUnityAdsRewordVideoAvailable())
			{
				Advertisement.ShowUnityRewordVideo();
			}*/
		}
	}

	/*private IEnumerator WaitForVideoLoaded()
	{
		while (!TapjoyWrapper.IsFullScreenAdLoaded() && !Advertisement.IsUnityAdsRewordVideoAvailable())
		{
			yield return new WaitForSeconds(0.1f);
		}
		m_RewardedVideoButton.SetDisabled(false);
	}*/

	private void OnButtonMoreApps(GUIBase_Widget inWidget)
	{
		ChartBoost.ShowMoreApps();
	}
}
