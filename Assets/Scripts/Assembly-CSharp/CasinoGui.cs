using System;
using System.Collections;
using UnityEngine;

public class CasinoGui
{
	private class MainHud : CityBaseScreen
	{
		private bool m_BuyChipsShouldBeVisible;

		private bool m_OutOfChips;

		private bool m_Busy;

		private int m_ChipsCount;

		private GUIBase_Layout m_Dialog;

		private GUIBase_Widget m_BuyChips;

		private GUIBase_Button m_Spin;

		private GUIBase_Button.TouchDelegate CloseDelegate;

		private AnimatedNumber m_Chips;

		private Vector3 m_OrigPos;

		private GUIBase_Button m_RewardedVideoButton;

		public MainHud()
			: base(CityScreen.CasinoMain)
		{
		}

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("CasinoScreen");
			m_Spin = m_Dialog.GetWidget("ButtonAccept").GetComponent<GUIBase_Button>();
			m_BuyChips = m_Dialog.GetWidget("_BuyChips").GetComponent<GUIBase_Widget>();
			m_Chips = new AnimatedNumber(m_Dialog.GetWidget("ChipsStatus").GetComponentInChildren<GUIBase_Label>());
			m_OrigPos = m_Dialog.transform.localPosition;
			m_RewardedVideoButton = m_Dialog.GetWidget("ButtonWatchVideo").GetComponent<GUIBase_Button>();
		}

		private void OnChipsChanged()
		{
			SetChipsAnimate(Game.Instance.PlayerPersistentInfo.numTickets);
		}

		private void SetChipsAnimate(int Chips)
		{
			m_Chips.Label.StopAllCoroutines();
			m_Chips.Label.StartCoroutine(CityGUIResources.AnimateNumber(m_ChipsCount, Chips, m_Chips, 2f, string.Empty));
			m_ChipsCount = Chips;
		}

		public void Show(GUIBase_Button.TouchDelegate buy, GUIBase_Button.TouchDelegate accept, GUIBase_Button.TouchDelegate close, Casino.Prize jackpot)
		{
			PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
			playerPersistentInfo.OnTicketChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo.OnTicketChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnChipsChanged));
			m_ChipsCount = 0;
			OnChipsChanged();
			base.Show(CloseAttempt);
			m_Dialog.StopAllCoroutines();
			m_Busy = false;
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonAccept", accept, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", CloseAttempt, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonBuy", buy, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "Back_Button", CloseAttempt, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonWatchVideo", OnButtonVideo, null);
			CloseDelegate = close;
			GUIBase_Widget component = m_Dialog.GetWidget("JackpotPreview").GetComponent<GUIBase_Widget>();
			GUIBase_Label component2 = m_Dialog.GetWidget("JackpotName").GetComponent<GUIBase_Label>();
			GUIBase_Label component3 = m_Dialog.GetWidget("JackpotCount").GetComponent<GUIBase_Label>();
			component.CopyMaterialSettings(jackpot.icon);
			component2.SetNewText(jackpot.name);
			component3.SetNewText("+" + jackpot.amount);
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			m_Dialog.StartCoroutine(CityGUIResources.ShowDialog(m_Dialog, m_OrigPos));
			m_RewardedVideoButton.SetDisabled(true);
			TapjoyWrapper.GetFullScreenAd();
			//m_Dialog.StartCoroutine(WaitForVideoLoaded());
		}

		public void OutOfChips(bool outOfChips)
		{
			m_OutOfChips = outOfChips;
			GUIBase_Label component = m_Dialog.GetWidget("OutOfChips1").GetComponent<GUIBase_Label>();
			GUIBase_Label component2 = m_Dialog.GetWidget("OutOfChips2").GetComponent<GUIBase_Label>();
			component.Widget.Show(outOfChips, true);
			component2.Widget.Show(outOfChips, true);
			if (outOfChips)
			{
				m_BuyChipsShouldBeVisible = true;
				ApplyBusyState();
			}
			else
			{
				m_BuyChipsShouldBeVisible = false;
				HideBuyChips();
			}
		}

		public void SetBusy(bool busy)
		{
			m_Busy = busy;
			ApplyBusyState();
		}

		public void CloseAttempt()
		{
			if (!m_Busy)
			{
				CloseDelegate();
			}
		}

		private void ApplyBusyState()
		{
			bool flag = m_OutOfChips || m_Busy;
			if (m_Spin.IsDisabled() != flag)
			{
				m_Spin.SetDisabled(flag);
			}
			if (!m_Busy && m_BuyChipsShouldBeVisible)
			{
				ShowBuyChips();
			}
		}

		public void DisableControls(bool disable)
		{
			m_Dialog.EnableControls(!disable);
		}

		public override void Hide()
		{
			PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
			playerPersistentInfo.OnTicketChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Remove(playerPersistentInfo.OnTicketChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnChipsChanged));
			base.Hide();
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", null, null);
			HideBuyChips();
			m_Dialog.StopAllCoroutines();
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			m_Dialog.StartCoroutine(CityGUIResources.HideDialog(m_Dialog, m_OrigPos));
			//m_Dialog.StopCoroutine(WaitForVideoLoaded());
		}

		public void RefreshChipsBuyState()
		{
		}

		private void ShowBuyChips()
		{
			if (!m_BuyChips.IsVisible())
			{
				m_BuyChips.Show(true, true);
				GUIBase_Label component = m_Dialog.GetWidget("ChipsCount").GetComponent<GUIBase_Label>();
				GUIBase_Label component2 = m_Dialog.GetWidget("Cost_Label").GetComponent<GUIBase_Label>();
				component.SetNewText("+" + 5);
				component2.SetNewText(5.ToString());
				RefreshChipsBuyState();
			}
		}

		private void HideBuyChips()
		{
			m_BuyChips.Show(false, true);
		}

		private void OnButtonVideo()
		{
			/*Advertisement.RemoveOtherCallbacksAndAddSlotMachineSpin();
			if (TapjoyWrapper.IsFullScreenAdLoaded())
			{
				TapjoyWrapper.ShowFullScreenAd();
			}
			else if (Advertisement.IsUnityAdsRewordVideoAvailable())
			{
				Advertisement.ShowUnityRewordVideo();
			}*/
		}

		/*private IEnumerator WaitForVideoLoaded()
		{
			while (!TapjoyWrapper.IsFullScreenAdLoaded() && !Advertisement.IsUnityAdsRewordVideoAvailable())
			{
				yield return new WaitForSeconds(0.1f);
			}
			m_RewardedVideoButton.SetDisabled(false);
		}*/
	}

	private class SpecialRewardDlg : CityBaseScreen
	{
		private GUIBase_Layout m_Dialog;

		private Vector3 m_OrigPos;

		public SpecialRewardDlg()
			: base(CityScreen.CasinoSpecialReward)
		{
		}

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("SpecialReward");
			m_OrigPos = m_Dialog.transform.localPosition;
		}

		public void Show(GUIBase_Button.TouchDelegate close, Casino.Prize reward)
		{
			base.Show();
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", close, null);
			GUIBase_Label component = m_Dialog.GetWidget("Reward_Count").GetComponent<GUIBase_Label>();
			GUIBase_Label component2 = m_Dialog.GetWidget("Reward_Label").GetComponent<GUIBase_Label>();
			GUIBase_Widget component3 = m_Dialog.GetWidget("Reward1").GetComponent<GUIBase_Widget>();
			component.SetNewText("+" + reward.amount);
			component2.SetNewText(reward.name);
			component3.CopyMaterialSettings(reward.icon);
			m_Dialog.StartCoroutine(CityGUIResources.ShowDialog(m_Dialog, m_OrigPos));
		}

		public override void Hide()
		{
			base.Hide();
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", null, null);
			m_Dialog.StartCoroutine(CityGUIResources.HideDialog(m_Dialog, m_OrigPos));
		}
	}

	private MainHud m_MainHud = new MainHud();

	private SpecialRewardDlg m_SpecialReward = new SpecialRewardDlg();

	private GUIBase_Pivot m_Pivot;

	public CasinoGui()
	{
		Init();
	}

	public void Destroy()
	{
		DestroyInternal();
	}

	public void ShowMainHud(GUIBase_Button.TouchDelegate buy, GUIBase_Button.TouchDelegate accept, GUIBase_Button.TouchDelegate close, Casino.Prize jackpot)
	{
		m_MainHud.Show(buy, accept, close, jackpot);
	}

	public void OutOfChips(bool outOfChips)
	{
		m_MainHud.OutOfChips(outOfChips);
	}

	public void RefreshChipsBuyState()
	{
		m_MainHud.RefreshChipsBuyState();
	}

	public void DisableHudControls(bool disable)
	{
		m_MainHud.DisableControls(disable);
	}

	public void SetBusy(bool busy)
	{
		m_MainHud.SetBusy(busy);
	}

	public void HideMainHud()
	{
		m_MainHud.Hide();
	}

	public void ShowSpecialReward(GUIBase_Button.TouchDelegate close, Casino.Prize reward)
	{
		m_SpecialReward.Show(close, reward);
	}

	public void HideSpecialReward()
	{
		m_SpecialReward.Hide();
	}

	private void Init()
	{
		m_Pivot = MFGuiManager.Instance.GetPivot("Casino");
		m_MainHud.Init(m_Pivot);
		m_SpecialReward.Init(m_Pivot);
	}

	private void DestroyInternal()
	{
	}
}
