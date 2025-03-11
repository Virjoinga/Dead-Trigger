using System.Collections.Generic;
using UnityEngine;

public class StatisticsScreen : BaseMenuScreen
{
	private const int TEXT_ID_PLAYER_VALUE = 2040303;

	private const int TEXT_ID_FRIEND_VALUE = 2040304;

	private const int TEXT_ID_FRIEND_NAME = 2040305;

	private StatisticsView m_StatisticsView;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_SelectFriends;

	private GUIBase_Button m_BestButton;

	private GUIBase_Label m_FriendValueLabel;

	private GUIBase_Label m_FriendNameLabel;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("Statistics_Screen");
			m_ScreenLayout = GetLayout("Statistics_Screen", "Statistics_Layout");
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			m_SelectFriends = PrepareButton(m_ScreenLayout, "SelectFriend_Button", null, OnSelectFriend);
			m_SelectFriends.autoColorLabels = true;
			m_BestButton = PrepareButton(m_ScreenLayout, "Best_Button", null, Delegate_SelectBest);
			m_BestButton.autoColorLabels = true;
			GUIBase_Button gUIBase_Button = PrepareButton(m_ScreenLayout, "Prev_Button", null, null);
			GUIBase_Button gUIBase_Button2 = PrepareButton(m_ScreenLayout, "Next_Button", null, null);
			gUIBase_Button.autoColorLabels = true;
			gUIBase_Button2.autoColorLabels = true;
			GUIBase_List component = GetWidget(m_ScreenLayout, "Table").GetComponent<GUIBase_List>();
			m_StatisticsView = base.gameObject.AddComponent<StatisticsView>();
			m_StatisticsView.GUIView_Init(m_ScreenLayout, component, gUIBase_Button, gUIBase_Button2);
			GUIBase_Widget widget = GetWidget(m_ScreenLayout, "Header");
			m_FriendValueLabel = widget.transform.GetChildComponent<GUIBase_Label>("03_friend_value");
			m_FriendNameLabel = widget.transform.GetChildComponent<GUIBase_Label>("04_friend_name");
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, true);
		List<FriendList.FriendInfo> friends = GameCloudManager.friendList.friends;
		bool disabled = friends == null || friends.Count <= 0;
		m_SelectFriends.SetDisabled(disabled);
		m_BestButton.SetDisabled(disabled);
		m_StatisticsView.GUIView_Show();
		m_StatisticsView.SetStatisticsMode(Statistics.E_Mode.Player, string.Empty);
		UpdateHeader(Statistics.E_Mode.Player, string.Empty);
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		m_StatisticsView.GUIView_Hide();
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		m_StatisticsView.GUIView_Update();
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		m_ScreenPivot.EnableControls(true);
		base.OnGUI_Enable();
	}

	protected override void OnGUI_Disable()
	{
		m_ScreenPivot.EnableControls(false);
		base.OnGUI_Disable();
	}

	private void UpdateHeader(Statistics.E_Mode inMode, string inFriendName)
	{
		if (inFriendName != string.Empty)
		{
			m_FriendValueLabel.SetNewText(inFriendName);
		}
		else
		{
			m_FriendValueLabel.SetNewText(2040304);
		}
		m_FriendNameLabel.SetNewText(2040305);
		bool flag = inMode == Statistics.E_Mode.CompareWithBest;
		m_FriendNameLabel.Widget.m_FadeAlpha = ((!flag) ? 0.5f : 1f);
	}

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void OnSelectFriend(GUIBase_Widget inInstigator)
	{
		m_OwnerMenu.ShowPopup("SelectFriend", string.Empty, string.Empty, SelectFriendPopupHandler);
	}

	private void SelectFriendPopupHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		SelectFriendDialog selectFriendDialog = inPopup as SelectFriendDialog;
		if (selectFriendDialog == null)
		{
			Debug.LogError("This handler can be used only with SelectFriendDialog !!!");
		}
		else if (selectFriendDialog.selectedFriend == "{[__BEST__]}")
		{
			m_StatisticsView.SetStatisticsMode(Statistics.E_Mode.CompareWithBest, string.Empty);
			UpdateHeader(Statistics.E_Mode.CompareWithBest, string.Empty);
		}
		else
		{
			m_StatisticsView.SetStatisticsMode(Statistics.E_Mode.CompareWithFriend, selectFriendDialog.selectedFriend);
			UpdateHeader(Statistics.E_Mode.CompareWithFriend, selectFriendDialog.selectedFriend);
		}
	}

	private void Delegate_SelectBest(GUIBase_Widget inInstigator)
	{
		m_StatisticsView.SetStatisticsMode(Statistics.E_Mode.CompareWithBest, string.Empty);
		UpdateHeader(Statistics.E_Mode.CompareWithBest, string.Empty);
	}
}
