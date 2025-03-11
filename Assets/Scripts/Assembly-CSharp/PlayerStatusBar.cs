using System;
using UnityEngine;

public class PlayerStatusBar
{
	private GUIBase_Layout m_Dialog;

	private AnimatedNumber m_Gold;

	private AnimatedNumber m_Money;

	private AnimatedNumber m_Level;

	private AnimatedNumber m_LevelProgress;

	private GUIBase_Sprite m_FullBar;

	private GUIBase_Sprite m_EmptyBar;

	private int m_MoneyVal;

	private int m_GoldVal;

	private int m_RankVal;

	private int m_RankProgress;

	private GUIBase_Button m_NewsButton;

	public void Init(GUIBase_Pivot pivot)
	{
		m_Dialog = pivot.GetLayout("StatusBar");
		m_Gold = new AnimatedNumber(m_Dialog.GetWidget("Gold").GetComponentInChildren<GUIBase_Label>());
		m_Money = new AnimatedNumber(m_Dialog.GetWidget("$").GetComponentInChildren<GUIBase_Label>());
		m_Level = new AnimatedNumber(m_Dialog.GetWidget("LevelText").GetComponentInChildren<GUIBase_Label>());
		m_LevelProgress = new AnimatedNumber(m_Dialog.GetWidget("ProgressText").GetComponentInChildren<GUIBase_Label>());
		m_FullBar = m_Dialog.GetWidget("FullBar").GetComponent<GUIBase_Sprite>();
		m_EmptyBar = m_Dialog.GetWidget("EmptyBar").GetComponent<GUIBase_Sprite>();
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo.OnGoldChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo.OnGoldChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnGoldChanged));
		PlayerPersistantInfo playerPersistentInfo2 = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo2.OnMoneyChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo2.OnMoneyChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnMoneyChanged));
		PlayerPersistantInfo playerPersistentInfo3 = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo3.OnExperienceChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo3.OnExperienceChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnExperienceChanged));
	}

	public void Show(GUIBase_Button.TouchDelegate back, GUIBase_Button.TouchDelegate gold, GUIBase_Button.TouchDelegate money, GUIBase_Button.TouchDelegate xp, GUIBase_Button.TouchDelegate options)
	{
		MFGuiManager.Instance.ShowLayout(m_Dialog, true);
		GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonOptions", options, null);
		GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonBuyGold", gold, null);
		GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonBuyGold", gold, null);
		InitData();
	}

	public void InitData()
	{
		SetGold(Game.Instance.PlayerPersistentInfo.gold);
		SetMoney(Game.Instance.PlayerPersistentInfo.money);
		SetXp();
	}

	public void Destroy()
	{
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo.OnGoldChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Remove(playerPersistentInfo.OnGoldChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnGoldChanged));
		PlayerPersistantInfo playerPersistentInfo2 = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo2.OnMoneyChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Remove(playerPersistentInfo2.OnMoneyChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnMoneyChanged));
		PlayerPersistantInfo playerPersistentInfo3 = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo3.OnExperienceChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Remove(playerPersistentInfo3.OnExperienceChanged, new PlayerPersistantInfo.PersistenInfoChanged(OnExperienceChanged));
	}

	public void Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_Dialog, false);
	}

	public void EnableControls(bool enable)
	{
		m_Dialog.EnableControls(enable);
		EnableButon("ButtonBuyGold", enable);
		EnableButon("ButtonOptions", enable);
		EnableWidget("Gold", enable);
		EnableWidget("$", enable);
		EnableWidget("XP", enable);
	}

	private void EnableButon(string buttonName, bool enable)
	{
		GuiBaseUtils.GetButton(m_Dialog, buttonName).SetDisabled(!enable);
		EnableWidget(buttonName, enable);
	}

	private void EnableWidget(string widgetName, bool enable)
	{
		GUIBase_Widget[] componentsInChildren = m_Dialog.GetWidget(widgetName).GetComponentsInChildren<GUIBase_Widget>();
		GUIBase_Widget[] array = componentsInChildren;
		foreach (GUIBase_Widget gUIBase_Widget in array)
		{
			gUIBase_Widget.m_FadeAlpha = ((!enable) ? 0.25f : 1f);
		}
	}

	private void OnGoldChanged()
	{
		SetGoldAnimate(Game.Instance.PlayerPersistentInfo.gold);
	}

	private void OnMoneyChanged()
	{
		SetMoneyAnimate(Game.Instance.PlayerPersistentInfo.money);
	}

	private void OnExperienceChanged()
	{
		SetXpAnimate();
	}

	private void SetGold(int Gold)
	{
		m_Gold.Label.StopAllCoroutines();
		m_GoldVal = Gold;
		m_Gold.Label.SetNewText(Gold.ToString());
	}

	private void SetMoney(int Money)
	{
		m_Money.Label.StopAllCoroutines();
		m_MoneyVal = Money;
		m_Money.Label.SetNewText(Money.ToString());
	}

	private void SetXp()
	{
		m_Level.Label.StopAllCoroutines();
		m_LevelProgress.Label.StopAllCoroutines();
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		SetPlayerLevel(rank, Game.Instance.PlayerPersistentInfo.experience, PlayerPersistantInfo.GetPlayerMinExperienceForRank(rank), PlayerPersistantInfo.GetPlayerMaxExperienceForRank(rank));
	}

	private void SetGoldAnimate(int Gold)
	{
		m_Gold.Label.StopAllCoroutines();
		m_Gold.Label.StartCoroutine(CityGUIResources.AnimateNumber(m_GoldVal, Gold, m_Gold, 1f, string.Empty));
		m_GoldVal = Gold;
	}

	private void SetMoneyAnimate(int Money)
	{
		m_Money.Label.StopAllCoroutines();
		m_Money.Label.StartCoroutine(CityGUIResources.AnimateNumber(m_MoneyVal, Money, m_Money, 1f, string.Empty));
		m_MoneyVal = Money;
	}

	private void SetXpAnimate()
	{
		m_Level.Label.StopAllCoroutines();
		m_LevelProgress.Label.StopAllCoroutines();
		int rankVal = m_RankVal;
		int num = m_RankProgress;
		int rank = Game.Instance.PlayerPersistentInfo.rank;
		SetPlayerLevel(rank, Game.Instance.PlayerPersistentInfo.experience, PlayerPersistantInfo.GetPlayerMinExperienceForRank(rank), PlayerPersistantInfo.GetPlayerMaxExperienceForRank(rank), false);
		if (num > m_RankProgress)
		{
			num = 0;
		}
		m_Level.Label.StartCoroutine(CityGUIResources.AnimateNumber(rankVal, m_RankVal, m_Level, 1f, string.Empty));
		m_LevelProgress.Label.StartCoroutine(CityGUIResources.AnimateNumber(num, m_RankProgress, m_LevelProgress, 1f, "%"));
	}

	private void SetPlayerLevel(int Level, int Xp, int MinXp, int MaxXp, bool updateGui = true)
	{
		Vector3 localPosition = m_EmptyBar.transform.localPosition;
		float width = m_EmptyBar.Widget.GetWidth();
		float num = ((MaxXp != MinXp) ? Mathf.Clamp((float)(Xp - MinXp) / (float)(MaxXp - MinXp), 0f, 1f) : 0f);
		int num2 = Mathf.RoundToInt(width * num);
		if (num2 == 0)
		{
			num2 = 1;
		}
		int num3 = Mathf.RoundToInt(width * num / 2f);
		Vector3 localScale = new Vector3(num2, 1f, 1f);
		m_RankVal = Level;
		m_RankProgress = Mathf.CeilToInt(num * 100f);
		if (m_RankProgress == 100)
		{
			m_RankProgress = 99;
		}
		if (updateGui)
		{
			m_Level.Label.SetNewText(Level.ToString());
			m_LevelProgress.Label.SetNewText(m_RankProgress + "%");
		}
		localPosition.x += (float)num3 - width / 2f;
		m_FullBar.transform.localPosition = localPosition;
		m_FullBar.transform.localScale = localScale;
		m_FullBar.Widget.SetModify();
	}
}
