using System.Collections;
using UnityEngine;

public class GuiBankScreen : BaseMenuScreen
{
	private class SiteData
	{
		public string m_URL;

		public string m_ID;

		public bool m_Rewarded;

		public bool m_FBSite;

		public GUIBase_Button m_Button;

		public string m_ButtonRewardedLabel;
	}

	private const int TextID_Reward_Caption = 1011110;

	private const int TextID_Reward_Message = 1011111;

	private const int TextID_Rewarded_TWMFG = 1011071;

	private const int TextID_Rewarded_FBMFG = 1011091;

	private const int TextID_Rewarded_FBSG = 1011081;

	private const int TextID_Rewarded_FBDT = 1011101;

	private const int TextID_CheckingStatus = 1011120;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_ButtonFBDT;

	private GUIBase_Button m_ButtonFBMFG;

	private GUIBase_Button m_ButtonFBSG;

	private GUIBase_Button m_ButtonTwitter;

	private SiteData m_TwitterMFG;

	private SiteData m_FacebookMFG;

	private SiteData m_FacebookDT;

	private SiteData m_FacebookSG;

	private SiteData m_VisitingSite;

	private bool m_LoggedIn;

	private float m_WaitForLogin;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenLayout = GetLayout("CityScreens", "Bank");
			PrepareButton(m_ScreenLayout, "ButtonClose", null, OnClose);
			PrepareButton(m_ScreenLayout, "ButtonBanner", null, OnButtonAdBanner);
			m_ButtonFBDT = PrepareButton(m_ScreenLayout, "ButtonFBDT", null, OnButtonFBDT);
			m_ButtonFBMFG = PrepareButton(m_ScreenLayout, "ButtonFBMFG", null, OnButtonFBMFG);
			m_ButtonFBSG = PrepareButton(m_ScreenLayout, "ButtonFBSG", null, OnButtonFBSG);
			m_ButtonTwitter = PrepareButton(m_ScreenLayout, "ButtonTwitter", null, OnButtonTwitter);
			PrepareButton(m_ScreenLayout, "ButtonMoreApps", null, OnButtonMoreApps);
			PPIBankData bankData = Game.Instance.PlayerPersistentInfo.BankData;
			m_TwitterMFG = new SiteData();
			m_TwitterMFG.m_URL = "https://twitter.com/madfingergames";
			m_TwitterMFG.m_ID = "85328174";
			m_TwitterMFG.m_Rewarded = bankData.TwitterSites.Contains(m_TwitterMFG.m_ID);
			m_TwitterMFG.m_FBSite = false;
			m_TwitterMFG.m_Button = m_ButtonTwitter;
			m_TwitterMFG.m_ButtonRewardedLabel = TextDatabase.instance[1011071];
			m_FacebookMFG = new SiteData();
			m_FacebookMFG.m_URL = "http://www.facebook.com/madfingergames";
			m_FacebookMFG.m_ID = "131826663523131";
			m_FacebookMFG.m_Rewarded = bankData.FacebookSites.Contains(m_FacebookMFG.m_ID);
			m_FacebookMFG.m_FBSite = true;
			m_FacebookMFG.m_Button = m_ButtonFBMFG;
			m_FacebookMFG.m_ButtonRewardedLabel = TextDatabase.instance[1011091];
			m_FacebookDT = new SiteData();
			m_FacebookDT.m_URL = "http://www.facebook.com/DEADTRIGGER";
			m_FacebookDT.m_ID = "202653433190538";
			m_FacebookDT.m_Rewarded = bankData.FacebookSites.Contains(m_FacebookDT.m_ID);
			m_FacebookDT.m_FBSite = true;
			m_FacebookDT.m_Button = m_ButtonFBDT;
			m_FacebookDT.m_ButtonRewardedLabel = TextDatabase.instance[1011101];
			m_FacebookSG = new SiteData();
			m_FacebookSG.m_URL = "http://www.facebook.com/Shadowgun";
			m_FacebookSG.m_ID = "302554313089127";
			m_FacebookSG.m_Rewarded = bankData.FacebookSites.Contains(m_FacebookSG.m_ID);
			m_FacebookSG.m_FBSite = true;
			m_FacebookSG.m_Button = m_ButtonFBSG;
			m_FacebookSG.m_ButtonRewardedLabel = TextDatabase.instance[1011081];
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, true);
		UpdateSiteButton(m_TwitterMFG);
		UpdateSiteButton(m_FacebookMFG);
		UpdateSiteButton(m_FacebookDT);
		UpdateSiteButton(m_FacebookSG);
		m_VisitingSite = null;
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Enable()
	{
		m_ScreenLayout.EnableControls(true);
	}

	protected override void OnGUI_Disable()
	{
		m_ScreenLayout.EnableControls(false);
	}

	private void OnClose(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void OnButtonAdBanner(GUIBase_Widget inWidget)
	{
		//TapjoyPlugin.ShowOffers();
	}

	private void OnButtonFBDT(GUIBase_Widget inWidget)
	{
		VisitSite(m_FacebookDT);
	}

	private void OnButtonFBMFG(GUIBase_Widget inWidget)
	{
		VisitSite(m_FacebookMFG);
	}

	private void OnButtonFBSG(GUIBase_Widget inWidget)
	{
		VisitSite(m_FacebookSG);
	}

	private void OnButtonTwitter(GUIBase_Widget inWidget)
	{
		VisitSite(m_TwitterMFG);
	}

	private void OnButtonMoreApps(GUIBase_Widget inWidget)
	{
		ChartBoost.ShowMoreApps();
	}

	private void UpdateSiteButton(SiteData Site)
	{
		if (Site.m_Rewarded && Site.m_Button != null && Site.m_ButtonRewardedLabel != string.Empty)
		{
			GUIBase_Label childLabel = GuiBaseUtils.GetChildLabel(Site.m_Button.Widget, "Button_Caption");
			if (childLabel != null)
			{
				childLabel.SetNewText(Site.m_ButtonRewardedLabel);
			}
			childLabel = GuiBaseUtils.GetChildLabel(Site.m_Button.Widget, "Button_CaptionDisabled");
			if (childLabel != null)
			{
				childLabel.SetNewText(Site.m_ButtonRewardedLabel);
			}
		}
	}

	private void VisitSite(SiteData Site)
	{
		m_LoggedIn = true;
		m_WaitForLogin = 0f;
		m_VisitingSite = Site;
		if (!Site.m_FBSite)
		{
			if (!Site.m_Rewarded && !TwitterWrapper.IsLoggedIn())
			{
				m_LoggedIn = false;
				m_WaitForLogin = float.MaxValue;
				TwitterUtils.LogIn(OnLoginResult);
			}
			StartCoroutine(WaitForLogin());
		}
	}

	private void OnLoginResult(bool Result)
	{
		m_LoggedIn = Result;
		m_WaitForLogin = 0f;
	}

	private IEnumerator WaitForLogin()
	{
		float timeStep = 0.25f;
		bool extraWait = m_WaitForLogin > 0f;
		while ((m_WaitForLogin -= timeStep) > 0f)
		{
			yield return new WaitForSeconds(timeStep);
		}
		if (extraWait)
		{
			yield return new WaitForSeconds(0.5f);
		}
		Etcetera.ShowWeb(m_VisitingSite.m_URL);
	}

	private void OnApplicationPause(bool Pause)
	{
		if (Pause || m_VisitingSite == null || m_VisitingSite.m_Rewarded)
		{
			return;
		}
		if (m_WaitForLogin > 0f)
		{
			m_WaitForLogin = Mathf.Min(15f, m_WaitForLogin);
		}
		else if (m_LoggedIn)
		{
			m_LoggedIn = false;
			string message = TextDatabase.instance[1011120];
			Etcetera.ShowActivityNotification(message);
			if (!m_VisitingSite.m_FBSite)
			{
				TwitterUtils.DoesUserFollow(m_VisitingSite.m_ID, OnStatusCheckResult);
			}
		}
	}

	private void OnStatusCheckResult(bool Result)
	{
		Etcetera.HideActivityNotification();
		if (Result)
		{
			int gold = 5;
			string inCaption = TextDatabase.instance[1011110];
			string text = TextDatabase.instance[1011111];
			text = text.Replace("%d", gold.ToString());
			GuiMainMenu.Instance.ShowPopup("OkDialog", inCaption, text);
			PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
			if (m_VisitingSite.m_FBSite)
			{
				playerPersistentInfo.BankData.FacebookSites.Add(m_VisitingSite.m_ID);
			}
			else
			{
				playerPersistentInfo.BankData.TwitterSites.Add(m_VisitingSite.m_ID);
			}
			playerPersistentInfo.AddGold(gold);
			playerPersistentInfo.Save();
			m_VisitingSite.m_Rewarded = true;
			UpdateSiteButton(m_VisitingSite);
			m_VisitingSite = null;
		}
	}
}
