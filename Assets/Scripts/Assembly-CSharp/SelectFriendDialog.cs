public class SelectFriendDialog : BasePopupScreen
{
	public const string SHOW_BEST_RESULTS = "{[__BEST__]}";

	private FriendListView m_FriendsView;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	public string selectedFriend { get; private set; }

	public override void SetCaption(string inCaption)
	{
	}

	public override void SetText(string inText)
	{
	}

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = MFGuiManager.Instance.GetPivot("SelectFriend_Dialog");
			m_ScreenLayout = m_ScreenPivot.GetLayout("SelectFriend_Layout");
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			GUIBase_Button gUIBase_Button = PrepareButton(m_ScreenLayout, "Prev_Button", null, null);
			GUIBase_Button gUIBase_Button2 = PrepareButton(m_ScreenLayout, "Next_Button", null, null);
			gUIBase_Button.autoColorLabels = true;
			gUIBase_Button2.autoColorLabels = true;
			GUIBase_List component = GetWidget(m_ScreenLayout, "Table").GetComponent<GUIBase_List>();
			m_FriendsView = base.gameObject.AddComponent<FriendListView>();
			m_FriendsView.m_OnFriendSelectDelegate = Delegate_OnSelect;
			m_FriendsView.GUIView_Init(m_ScreenLayout, component, gUIBase_Button, gUIBase_Button2);
			base.isInitialized = true;
		}
		catch
		{
			throw;
		}
	}

	protected override void OnGUI_Show()
	{
		base.OnGUI_Show();
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, true);
		m_FriendsView.GUIView_Show();
	}

	protected override void OnGUI_Hide()
	{
		MFGuiManager.Instance.ShowLayout(m_ScreenLayout, false);
		m_FriendsView.GUIView_Hide();
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		m_FriendsView.GUIView_Update();
		base.OnGUI_Update();
	}

	protected override void OnGUI_Destroy()
	{
		base.OnGUI_Destroy();
	}

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void Delegate_OnSelect(string inFriendName)
	{
		selectedFriend = inFriendName;
		SendResult(E_PopupResultCode.Ok);
		m_OwnerMenu.Back();
	}
}
