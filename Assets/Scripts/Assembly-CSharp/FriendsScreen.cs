public class FriendsScreen : BaseMenuScreen
{
	private PendingFriendListView m_PendingFriendsView;

	private FriendListView m_FriendsView;

	private BaseListView m_ActiveView;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_ActiveButton;

	private GUIBase_Button m_PendingButton;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("Friends_Screen");
			m_ScreenLayout = GetLayout("Friends_Screen", "Friends_Layout");
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			m_ActiveButton = PrepareButton(m_ScreenLayout, "Active_Button", null, OnActive);
			m_PendingButton = PrepareButton(m_ScreenLayout, "Pending_Button", null, OnPending);
			m_ActiveButton.stayDown = true;
			m_PendingButton.stayDown = true;
			GUIBase_Button gUIBase_Button = PrepareButton(m_ScreenLayout, "Prev_Button", null, null);
			GUIBase_Button gUIBase_Button2 = PrepareButton(m_ScreenLayout, "Next_Button", null, null);
			gUIBase_Button.autoColorLabels = true;
			gUIBase_Button2.autoColorLabels = true;
			m_ActiveButton = PrepareButton(m_ScreenLayout, "Active_Button", null, OnActive);
			m_PendingButton = PrepareButton(m_ScreenLayout, "Pending_Button", null, OnPending);
			GUIBase_Layout layout = GetLayout("Friends_Screen", "ActiveFriends_Layout");
			GUIBase_List component = GetWidget(layout, "Table").GetComponent<GUIBase_List>();
			m_FriendsView = base.gameObject.AddComponent<FriendListView>();
			m_FriendsView.GUIView_Init(layout, component, gUIBase_Button, gUIBase_Button2);
			GUIBase_Layout layout2 = GetLayout("Friends_Screen", "PendingFriends_Layout");
			GUIBase_List component2 = GetWidget(layout2, "Table").GetComponent<GUIBase_List>();
			m_PendingFriendsView = base.gameObject.AddComponent<PendingFriendListView>();
			m_PendingFriendsView.GUIView_Init(layout2, component2, gUIBase_Button, gUIBase_Button2);
			PrepareButton(m_ScreenLayout, "AddFriend_Button", null, OnAddNewFriend);
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
		if (m_ActiveView == null || m_ActiveView == m_FriendsView)
		{
			ShowActive();
		}
		else
		{
			ShowPending();
		}
		GameCloudManager.friendList.RetriveFriendListFromCloud();
		GameCloudManager.mailbox.FetchMessages();
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		if (m_ActiveView != null)
		{
			m_ActiveView.GUIView_Update();
		}
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	protected override void OnGUI_Enable()
	{
		m_ScreenPivot.EnableControls(true);
	}

	protected override void OnGUI_Disable()
	{
		m_ScreenPivot.EnableControls(false);
	}

	private void OnActive(GUIBase_Widget inWidget)
	{
		ShowActive();
	}

	private void OnPending(GUIBase_Widget inWidget)
	{
		ShowPending();
	}

	private void OnPrev(GUIBase_Widget inWidget)
	{
	}

	private void OnNext(GUIBase_Widget inWidget)
	{
	}

	private void OnAddNewFriend(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.ShowPopup("NewFriend", string.Empty, string.Empty, OnAddFrend);
	}

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void ShowActive()
	{
		m_FriendsView.GUIView_Show();
		m_PendingFriendsView.GUIView_Hide();
		m_ActiveView = m_FriendsView;
		m_ActiveButton.ForceDownStatus(true);
		m_PendingButton.ForceDownStatus(false);
	}

	private void ShowPending()
	{
		m_FriendsView.GUIView_Hide();
		m_PendingFriendsView.GUIView_Show();
		m_ActiveView = m_PendingFriendsView;
		m_ActiveButton.ForceDownStatus(false);
		m_PendingButton.ForceDownStatus(true);
		GameCloudManager.friendList.RetriveFriendListFromCloud();
	}

	public void OnAddFrend(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			ShowPending();
		}
	}
}
