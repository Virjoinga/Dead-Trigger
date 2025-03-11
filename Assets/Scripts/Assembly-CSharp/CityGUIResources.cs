using System;
using System.Collections;
using UnityEngine;

public class CityGUIResources
{
	private struct ScreenFade
	{
		private GUIBase_Layout m_Dialog;

		private GameObject m_BlackPlane;

		private Vector3 m_OrigPos;

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("ScreenFade");
			m_OrigPos = m_Dialog.transform.localPosition;
			foreach (Transform item in Camera.main.transform)
			{
				if (item.gameObject.name == "camerablackplane")
				{
					m_BlackPlane = item.gameObject;
					break;
				}
			}
		}

		public void Show(bool instant)
		{
			if (instant)
			{
				MFGuiManager.Instance.ShowLayout(m_Dialog, true);
			}
			else
			{
				m_Dialog.StartCoroutine(ShowDialog(m_Dialog, m_OrigPos));
			}
		}

		public void Hide()
		{
			if ((bool)m_BlackPlane)
			{
				m_BlackPlane.SetActive(false);
			}
			m_Dialog.StartCoroutine(InternalShowHideDialog(m_Dialog, m_OrigPos, 0.8f, false));
		}
	}

	private struct CityHud
	{
		public AnimatedNumber m_ChipsStatus;

		private GUIBase_Layout m_Dialog;

		private GUIBase_Button[] m_Indicator;

		private GUIBase_Button m_SafeHaven;

		private GUIBase_Widget m_SafeHavenTutorial;

		private GUIBase_Button m_Shop;

		private GUIBase_Widget m_ShopTutorial;

		private GUIBase_Button m_Equip;

		private GUIBase_Widget m_EquipTutorial;

		private GUIBase_Button m_Bank;

		private GUIBase_Button m_Casino;

		private GUIBase_Widget m_ObjectiveParent;

		private GUIBase_Widget m_ObjectiveBck;

		private GUIBase_Label m_ObjectiveText;

		private Vector3 m_ObjectiveOrigPos;

		private GUIBase_Widget m_NotificationParent;

		private GUIBase_Widget m_NotificationBck;

		private GUIBase_Label m_NotificationText;

		private Vector3 m_NotificationOrigPos;

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("CityHud");
			m_Indicator = new GUIBase_Button[10];
			m_Indicator[0] = m_Dialog.GetWidget("Indicator1").GetComponent<GUIBase_Button>();
			m_Indicator[1] = m_Dialog.GetWidget("Indicator2").GetComponent<GUIBase_Button>();
			m_Indicator[2] = m_Dialog.GetWidget("Indicator3").GetComponent<GUIBase_Button>();
			m_Indicator[3] = m_Dialog.GetWidget("Indicator4").GetComponent<GUIBase_Button>();
			m_Indicator[4] = m_Dialog.GetWidget("Indicator5").GetComponent<GUIBase_Button>();
			m_Indicator[5] = m_Dialog.GetWidget("Indicator6").GetComponent<GUIBase_Button>();
			m_Indicator[6] = m_Dialog.GetWidget("Indicator7").GetComponent<GUIBase_Button>();
			m_Indicator[7] = m_Dialog.GetWidget("Indicator8").GetComponent<GUIBase_Button>();
			m_Indicator[8] = m_Dialog.GetWidget("Indicator9").GetComponent<GUIBase_Button>();
			m_Indicator[9] = m_Dialog.GetWidget("Indicator10").GetComponent<GUIBase_Button>();
			m_Shop = m_Dialog.GetWidget("Shop").GetComponent<GUIBase_Button>();
			m_ShopTutorial = m_Dialog.GetWidget("TutorialShop").GetComponent<GUIBase_Widget>();
			m_Equip = m_Dialog.GetWidget("Equip").GetComponent<GUIBase_Button>();
			m_EquipTutorial = m_Dialog.GetWidget("TutorialEquip").GetComponent<GUIBase_Widget>();
			m_SafeHaven = m_Dialog.GetWidget("SafeHavenButton").GetComponent<GUIBase_Button>();
			m_SafeHavenTutorial = m_Dialog.GetWidget("TutorialSafeHaven").GetComponent<GUIBase_Widget>();
			m_Bank = m_Dialog.GetWidget("FreeGold").GetComponent<GUIBase_Button>();
			m_Casino = m_Dialog.GetWidget("CasinoButton").GetComponent<GUIBase_Button>();
			m_ChipsStatus = new AnimatedNumber(m_Casino.GetComponentInChildren<GUIBase_Label>());
			m_ObjectiveParent = m_Dialog.GetWidget("Objective").GetComponent<GUIBase_Widget>();
			m_ObjectiveBck = m_Dialog.GetWidget("ObjectiveBck").GetComponent<GUIBase_Widget>();
			m_ObjectiveText = m_Dialog.GetWidget("ObjectiveText").GetComponent<GUIBase_Label>();
			m_ObjectiveOrigPos = m_ObjectiveBck.transform.localPosition;
			m_NotificationParent = m_Dialog.GetWidget("Notification").GetComponent<GUIBase_Widget>();
			m_NotificationBck = m_Dialog.GetWidget("NotificationBck").GetComponent<GUIBase_Widget>();
			m_NotificationText = m_Dialog.GetWidget("NotificationText").GetComponent<GUIBase_Label>();
			m_NotificationOrigPos = m_NotificationBck.transform.localPosition;
		}

		public void Show(GUIBase_Button.TouchDelegate[] indicators, GUIBase_Button.TouchDelegate hideout, GUIBase_Button.TouchDelegate shop, GUIBase_Button.TouchDelegate equip, GUIBase_Button.TouchDelegate freeGold, GUIBase_Button.TouchDelegate casino)
		{
			for (int i = 0; i < indicators.Length; i++)
			{
				GuiBaseUtils.RegisterButtonDelegate(m_Dialog, m_Indicator[i].name, indicators[i], null);
			}
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "SafeHavenButton", hideout, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "Shop", shop, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "Equip", equip, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "FreeGold", freeGold, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "CasinoButton", casino, null);
			MFGuiManager.Instance.ShowLayout(m_Dialog, true);
			CityHudEnableInput(CitySiteInfo.InfoType.Normal);
		}

		public void Show()
		{
			MFGuiManager.Instance.ShowLayout(m_Dialog, true);
		}

		public void Hide()
		{
			MFGuiManager.Instance.ShowLayout(m_Dialog, false);
		}

		public void CityHudEnableInput(CitySiteInfo.InfoType type)
		{
			GUIBase_Button[] indicator = m_Indicator;
			foreach (GUIBase_Button gUIBase_Button in indicator)
			{
				gUIBase_Button.SetDisabled(type != CitySiteInfo.InfoType.Normal);
			}
			m_Shop.SetDisabled(type != 0 && type != CitySiteInfo.InfoType.Shop);
			m_Equip.SetDisabled(type != 0 && type != CitySiteInfo.InfoType.Equip);
			m_Bank.SetDisabled(type != 0 && type != CitySiteInfo.InfoType.Bank);
			m_Casino.SetDisabled(type != 0 && type != CitySiteInfo.InfoType.Casino);
			m_SafeHaven.SetDisabled(type != 0 && type != CitySiteInfo.InfoType.SafeHaven);
		}

		public void CityHudDisableInput()
		{
			GUIBase_Button[] indicator = m_Indicator;
			foreach (GUIBase_Button gUIBase_Button in indicator)
			{
				gUIBase_Button.SetDisabled(true);
			}
			m_Shop.SetDisabled(true);
			m_Equip.SetDisabled(true);
			m_Bank.SetDisabled(true);
			m_Casino.SetDisabled(true);
			m_SafeHaven.SetDisabled(true);
		}

		private void ShowText(string text, GUIBase_Label label, GUIBase_Widget background, Vector3 origPos)
		{
			label.SetNewText(text);
			int length = text.Length;
			float num = label.textSize.x - 34f - 8f * (float)length;
			float x = 1f + num / 100f;
			Vector3 localPosition = origPos;
			localPosition.x += num / 2f - (float)length * 2.3f;
			background.transform.localPosition = localPosition;
			background.transform.localScale = new Vector3(x, 1f, 1f);
			background.Show(true, true);
			label.Widget.Show(true, true);
		}

		public void ShowObjective(string text)
		{
			if (text == string.Empty)
			{
				HideObjective();
			}
			else
			{
				ShowText(text, m_ObjectiveText, m_ObjectiveBck, m_ObjectiveOrigPos);
			}
		}

		public void HideObjective()
		{
			m_ObjectiveParent.Show(false, true);
		}

		public void ShowNotification(string text)
		{
			ShowText(text, m_NotificationText, m_NotificationBck, m_NotificationOrigPos);
		}

		public void HideNotification()
		{
			m_NotificationParent.Show(false, true);
		}

		public void ShowIndicator(CitySiteInfo.InfoType type, int index)
		{
			GUIBase_Widget gUIBase_Widget = null;
			switch (type)
			{
			case CitySiteInfo.InfoType.Normal:
				gUIBase_Widget = m_Indicator[index].Widget;
				break;
			case CitySiteInfo.InfoType.Shop:
				gUIBase_Widget = m_Shop.Widget;
				break;
			case CitySiteInfo.InfoType.SafeHaven:
				gUIBase_Widget = m_SafeHaven.Widget;
				break;
			case CitySiteInfo.InfoType.Equip:
				gUIBase_Widget = m_Equip.Widget;
				break;
			case CitySiteInfo.InfoType.Bank:
				gUIBase_Widget = m_Bank.Widget;
				break;
			case CitySiteInfo.InfoType.Casino:
				gUIBase_Widget = m_Casino.Widget;
				break;
			}
			if ((bool)gUIBase_Widget)
			{
				gUIBase_Widget.Show(true, true);
				gUIBase_Widget.StopAllCoroutines();
				SetColor(1f, gUIBase_Widget);
			}
		}

		public void ShowIndicatorTutorial(CitySiteInfo.InfoType type)
		{
			GUIBase_Widget gUIBase_Widget = null;
			GUIBase_Widget gUIBase_Widget2 = null;
			switch (type)
			{
			case CitySiteInfo.InfoType.Shop:
				gUIBase_Widget = m_ShopTutorial;
				gUIBase_Widget2 = m_Shop.Widget;
				break;
			case CitySiteInfo.InfoType.SafeHaven:
				gUIBase_Widget = m_SafeHavenTutorial;
				gUIBase_Widget2 = m_SafeHaven.Widget;
				break;
			case CitySiteInfo.InfoType.Equip:
				gUIBase_Widget = m_EquipTutorial;
				gUIBase_Widget2 = m_Equip.Widget;
				break;
			}
			if ((bool)gUIBase_Widget)
			{
				gUIBase_Widget.Show(true, true);
				gUIBase_Widget2.StopAllCoroutines();
				gUIBase_Widget2.StartCoroutine(Blink(gUIBase_Widget2));
			}
		}

		public void HideAllIndicators()
		{
			GUIBase_Button[] indicator = m_Indicator;
			foreach (GUIBase_Button gUIBase_Button in indicator)
			{
				gUIBase_Button.Widget.Show(false, true);
			}
			m_Shop.Widget.Show(false, true);
			m_Shop.StopAllCoroutines();
			m_SafeHaven.Widget.Show(false, true);
			m_SafeHaven.StopAllCoroutines();
			m_Equip.Widget.Show(false, true);
			m_Equip.StopAllCoroutines();
			m_ShopTutorial.Show(false, true);
			m_SafeHavenTutorial.Show(false, true);
			m_EquipTutorial.Show(false, true);
			m_Bank.Widget.Show(false, true);
			m_Casino.Widget.Show(false, true);
		}

		public Vector3 GetIndicatorPos(CitySiteInfo.InfoType type, int index)
		{
			GUIBase_Button gUIBase_Button = null;
			switch (type)
			{
			case CitySiteInfo.InfoType.Normal:
				gUIBase_Button = m_Indicator[index];
				break;
			case CitySiteInfo.InfoType.Shop:
				gUIBase_Button = m_Shop;
				break;
			case CitySiteInfo.InfoType.SafeHaven:
				gUIBase_Button = m_SafeHaven;
				break;
			case CitySiteInfo.InfoType.Equip:
				gUIBase_Button = m_Equip;
				break;
			case CitySiteInfo.InfoType.Casino:
				gUIBase_Button = m_Casino;
				break;
			case CitySiteInfo.InfoType.Bank:
				gUIBase_Button = m_Bank;
				break;
			}
			if ((bool)gUIBase_Button)
			{
				return gUIBase_Button.transform.position;
			}
			return Vector3.zero;
		}

		public void MoveIndicator(CitySiteInfo.InfoType type, int index, Vector3 pos)
		{
			GUIBase_Button gUIBase_Button = null;
			switch (type)
			{
			case CitySiteInfo.InfoType.Normal:
				gUIBase_Button = m_Indicator[index];
				break;
			case CitySiteInfo.InfoType.Shop:
				gUIBase_Button = m_Shop;
				break;
			case CitySiteInfo.InfoType.SafeHaven:
				gUIBase_Button = m_SafeHaven;
				break;
			case CitySiteInfo.InfoType.Equip:
				gUIBase_Button = m_Equip;
				break;
			case CitySiteInfo.InfoType.Casino:
				gUIBase_Button = m_Casino;
				break;
			case CitySiteInfo.InfoType.Bank:
				gUIBase_Button = m_Bank;
				break;
			}
			if ((bool)gUIBase_Button)
			{
				gUIBase_Button.transform.position = pos;
			}
		}

		public void MoveIndicatorTutorial(CitySiteInfo.InfoType type, Vector3 pos)
		{
			GUIBase_Widget gUIBase_Widget = null;
			switch (type)
			{
			case CitySiteInfo.InfoType.Shop:
				gUIBase_Widget = m_ShopTutorial;
				break;
			case CitySiteInfo.InfoType.SafeHaven:
				gUIBase_Widget = m_SafeHavenTutorial;
				break;
			case CitySiteInfo.InfoType.Equip:
				gUIBase_Widget = m_EquipTutorial;
				break;
			}
			if ((bool)gUIBase_Widget)
			{
				gUIBase_Widget.transform.position = pos;
			}
		}

		public void SetColor(float modif, GUIBase_Widget widget)
		{
			widget.m_Color = Color.white * modif;
		}

		public IEnumerator Blink(GUIBase_Widget widget)
		{
			float color = 1f;
			while (true)
			{
				if (!(color > 0.7f))
				{
					while (color < 1f)
					{
						SetColor(color, widget);
						color += 0.018f;
						yield return new WaitForSeconds(0.033f);
					}
				}
				else
				{
					SetColor(color, widget);
					color -= 0.018f;
					yield return new WaitForSeconds(0.033f);
				}
			}
		}
	}

	public PlayerStatusBar m_StatusBar = new PlayerStatusBar();

	private ScreenFade m_ScreenFade;

	private CityHud m_CityHud;

	public PlayerStatusBar statusBar
	{
		get
		{
			return m_StatusBar;
		}
	}

	public CityGUIResources()
	{
		Init();
	}

	public void Destroy()
	{
		DestroyInternal();
	}

	public void ShowScreenFade(bool instant)
	{
		m_ScreenFade.Show(instant);
	}

	public void HideScreenFade()
	{
		m_ScreenFade.Hide();
	}

	public void ShowCityHud(GUIBase_Button.TouchDelegate[] indicators, GUIBase_Button.TouchDelegate hideout, GUIBase_Button.TouchDelegate shop, GUIBase_Button.TouchDelegate equip, GUIBase_Button.TouchDelegate freeGold, GUIBase_Button.TouchDelegate casino)
	{
		m_CityHud.Show(indicators, hideout, shop, equip, freeGold, casino);
	}

	public void ShowCityHud()
	{
		m_CityHud.Show();
	}

	public void HideCityHud()
	{
		m_CityHud.Hide();
	}

	public void CityHudEnableInput(CitySiteInfo.InfoType type)
	{
		m_CityHud.CityHudEnableInput(type);
	}

	public void CityHudDisableInput()
	{
		m_CityHud.CityHudDisableInput();
	}

	public void ShowObjective(string text)
	{
		m_CityHud.ShowObjective(text);
	}

	public void HideObjective()
	{
		m_CityHud.HideObjective();
	}

	public void ShowNotification(string text)
	{
		m_CityHud.ShowNotification(text);
	}

	public void HideNotification()
	{
		m_CityHud.HideNotification();
	}

	public void ShowIndicator(CitySiteInfo.InfoType type, int index)
	{
		m_CityHud.ShowIndicator(type, index);
	}

	public void ShowIndicatorTutorial(CitySiteInfo.InfoType type)
	{
		m_CityHud.ShowIndicatorTutorial(type);
	}

	public void HideAllIndicators()
	{
		m_CityHud.HideAllIndicators();
	}

	public Vector3 GetIndicatorPos(CitySiteInfo.InfoType type, int index)
	{
		return m_CityHud.GetIndicatorPos(type, index);
	}

	public void MoveIndicator(CitySiteInfo.InfoType type, int index, Vector3 pos)
	{
		m_CityHud.MoveIndicator(type, index, pos);
	}

	public void MoveIndicatorTutorial(CitySiteInfo.InfoType type, Vector3 pos)
	{
		m_CityHud.MoveIndicatorTutorial(type, pos);
	}

	private void Init()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot("CityScreens");
		m_ScreenFade.Init(pivot);
		m_CityHud.Init(pivot);
		m_StatusBar.Init(pivot);
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo.OnTicketChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo.OnTicketChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnChipsChanged));
		OnChipsChanged();
	}

	private void DestroyInternal()
	{
		m_StatusBar.Destroy();
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo.OnTicketChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Remove(playerPersistentInfo.OnTicketChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnChipsChanged));
	}

	public void EnableStatusBarButtons(bool hidden)
	{
		m_StatusBar.EnableControls(hidden);
	}

	private void OnChipsChanged()
	{
		SetChipsAnimate(Game.Instance.PlayerPersistentInfo.numTickets);
	}

	private void SetChipsAnimate(int Chips)
	{
		m_CityHud.m_ChipsStatus.Label.StopAllCoroutines();
		m_CityHud.m_ChipsStatus.Label.StartCoroutine(AnimateNumber(Chips, Chips, m_CityHud.m_ChipsStatus, 1f, string.Empty));
	}

	public static IEnumerator HighlightObject(GUIBase_Widget sprite)
	{
		while (true)
		{
			sprite.m_FadeAlpha = 0.4f;
			yield return new WaitForSeconds(0.3f);
			sprite.m_FadeAlpha = 1f;
			yield return new WaitForSeconds(0.3f);
		}
	}

	public static IEnumerator AnimateNumber(int initValue, int finalValue, AnimatedNumber number, float speedModif = 1f, string suffix = "")
	{
		if ((bool)CityManager.Instance)
		{
			CityManager.Instance.PlaySound(CityManager.Sounds.GUI_NumberLoop, true);
		}
		int step = Mathf.Abs(finalValue - initValue);
		step = ((step <= 15) ? 1 : (step / 15 + ((step % 15 > 0) ? 1 : 0)));
		number.RestoreOrigScale();
		int val = initValue;
		float scaleXOrig = number.OrigScale.x;
		float scaleYOrig = number.OrigScale.y;
		float scaleX = scaleXOrig * 1.6f;
		float scaleY = scaleYOrig * 1.6f;
		float scaleXStep = (scaleXOrig - scaleX) / 15f;
		float scaleYStep = (scaleYOrig - scaleY) / 15f;
		number.Label.transform.localScale = new Vector3(scaleXOrig, scaleYOrig, scaleXOrig);
		if (initValue < finalValue)
		{
			while (val < finalValue)
			{
				val += step;
				if (val > finalValue)
				{
					val = finalValue;
				}
				number.Label.SetNewText(val + suffix);
				scaleX += scaleXStep;
				scaleY += scaleYStep;
				number.Label.transform.localScale = new Vector3(scaleX, scaleY, scaleX);
				yield return new WaitForSeconds(0.033f * speedModif);
			}
		}
		else if (initValue > finalValue)
		{
			while (val > finalValue)
			{
				val -= step;
				if (val < finalValue)
				{
					val = finalValue;
				}
				number.Label.SetNewText(val + suffix);
				scaleX += scaleXStep;
				scaleY += scaleYStep;
				number.Label.transform.localScale = new Vector3(scaleX, scaleY, scaleX);
				yield return new WaitForSeconds(0.033f * speedModif);
			}
		}
		else
		{
			number.Label.SetNewText(initValue + suffix);
		}
		number.RestoreOrigScale();
		if ((bool)CityManager.Instance)
		{
			CityManager.Instance.StopSound(CityManager.Sounds.GUI_NumberLoop);
		}
		yield return new WaitForSeconds(0.15f);
	}

	public static IEnumerator RewardTimer(GUIBase_Label timer)
	{
		while (true)
		{
			yield return new WaitForSeconds(0.01f);
		}
	}

	public static IEnumerator ShowDialog(GUIBase_Layout layout, Vector3 origPos)
	{
		if ((bool)CityManager.Instance)
		{
			CityManager.Instance.PlaySound(CityManager.Sounds.GUI_ZoomIn, false);
		}
		yield return layout.StartCoroutine(InternalShowHideDialog(layout, origPos, 0.2f, true));
	}

	public static IEnumerator Reset(GUIBase_Layout layout, Vector3 origPos)
	{
		layout.transform.localPosition = origPos;
		layout.transform.localScale = new Vector3(1f, 1f, 1f);
		layout.m_FadeAlpha = 1f;
		yield return new WaitForEndOfFrame();
	}

	public static IEnumerator HideDialog(GUIBase_Layout layout, Vector3 origPos)
	{
		if ((bool)CityManager.Instance)
		{
			CityManager.Instance.PlaySound(CityManager.Sounds.GUI_ZoomOut, false);
		}
		yield return layout.StartCoroutine(InternalShowHideDialog(layout, origPos, 0.1f, false));
	}

	public static IEnumerator InternalShowHideDialog(GUIBase_Layout layout, Vector3 origPos, float totalTime, bool show, float initDelay = 0f)
	{
		float scaleStart = ((!show) ? 1f : 0.85f);
		Vector3 scale = ((!show) ? new Vector3(1f, 1f, 1f) : new Vector3(scaleStart, scaleStart, scaleStart));
		GUIBase_Platform platform = MFGuiManager.Instance.GetPlatform(layout);
		Vector3 origSize = new Vector3(platform.m_Width, platform.m_Height, 0f) / 2f;
		Vector3 pos = origPos;
		float steps = totalTime / 0.02f;
		float scaleAdd = (1f - scaleStart) / steps;
		float alphaAdd = 1f / steps * ((!show) ? 1f : 1.5f);
		if (show)
		{
			MFGuiManager.Instance.ShowLayout(layout, true);
			layout.m_FadeAlpha = 0f;
		}
		else
		{
			layout.m_FadeAlpha = 1f;
		}
		yield return new WaitForEndOfFrame();
		if (!Mathf.Approximately(initDelay, 0f))
		{
			yield return new WaitForSeconds(initDelay);
		}
		for (int i = 0; (float)i < steps; i++)
		{
			layout.transform.localScale = scale;
			pos.x = origPos.x + origSize.x * (1f - scale.x);
			pos.y = origPos.y + origSize.y * (1f - scale.y);
			layout.transform.localPosition = pos;
			if (show)
			{
				layout.m_FadeAlpha += alphaAdd;
				scale.x += scaleAdd;
				scale.y += scaleAdd;
			}
			else
			{
				layout.m_FadeAlpha -= alphaAdd;
				scale.x -= scaleAdd;
				scale.y -= scaleAdd;
			}
			Mathf.Clamp(layout.m_FadeAlpha, 0f, 1f);
			Mathf.Clamp(scale.x, 0f, 1f);
			Mathf.Clamp(scale.y, 0f, 1f);
			yield return new WaitForSeconds(0.02f);
		}
		if (!show)
		{
			MFGuiManager.Instance.ShowLayout(layout, false);
		}
		layout.transform.localPosition = origPos;
		layout.transform.localScale = new Vector3(1f, 1f, 1f);
		layout.m_FadeAlpha = 1f;
		yield return new WaitForSeconds(0.01f);
	}
}
