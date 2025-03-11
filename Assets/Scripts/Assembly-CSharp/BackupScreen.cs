public class BackupScreen : BaseMenuScreen
{
	private BackupView m_BackupView;

	private GUIBase_Pivot m_ScreenPivot;

	private GUIBase_Layout m_ScreenLayout;

	private GUIBase_Button m_RestoreButton;

	private GUIBase_Button m_BackupButton;

	private BaseCloudAction m_RetrieveDataAction;

	private BaseCloudAction m_BackupDataAction;

	protected override void OnGUI_Init()
	{
		try
		{
			base.OnGUI_Init();
			m_ScreenPivot = GetPivot("Backup_Screen");
			m_ScreenLayout = GetLayout("Backup_Screen", "Backup_Layout");
			PrepareButton(m_ScreenLayout, "Back_Button", null, OnBack);
			m_RestoreButton = PrepareButton(m_ScreenLayout, "Download_Button", null, OnDownload);
			m_BackupButton = PrepareButton(m_ScreenLayout, "Upload_Button", null, OnUpload);
			m_RestoreButton.autoColorLabels = true;
			m_BackupButton.autoColorLabels = true;
			GUIBase_List component = GetWidget(m_ScreenLayout, "Table").GetComponent<GUIBase_List>();
			m_BackupView = base.gameObject.AddComponent<BackupView>();
			m_BackupView.GUIView_Init(m_ScreenLayout, component, null, null);
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
		m_BackupView.GUIView_Show();
		m_RetrieveDataAction = GameCloudManager.RetrieveProgressFromCloud();
		base.OnGUI_Show();
	}

	protected override void OnGUI_Hide()
	{
		m_BackupView.GUIView_Hide();
		MFGuiManager.Instance.ShowPivot(m_ScreenPivot, false);
		base.OnGUI_Hide();
	}

	protected override void OnGUI_Update()
	{
		if (m_RetrieveDataAction != null && m_RetrieveDataAction.isDone)
		{
			Invoke("RefreshView", 0.2f);
			m_RetrieveDataAction = null;
		}
		if (m_BackupDataAction != null && m_BackupDataAction.isDone)
		{
			Invoke("RefreshView", 0.2f);
			m_BackupDataAction = null;
		}
		bool flag = m_RetrieveDataAction != null || m_BackupDataAction != null;
		bool flag2 = GameCloudManager.CanRestoreProgressFromCloud();
		m_RestoreButton.SetDisabled(flag || !flag2);
		m_BackupButton.SetDisabled(flag);
		m_BackupView.retrivingInfoFromCloud = m_RetrieveDataAction != null;
		m_BackupView.GUIView_Update();
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

	private void OnBack(GUIBase_Widget inWidget)
	{
		m_OwnerMenu.Back();
	}

	private void OnDownload(GUIBase_Widget inWidget)
	{
		string inCaption = TextDatabase.instance[2040601];
		string inText = TextDatabase.instance[2040603];
		m_OwnerMenu.ShowPopup("Confirm", inCaption, inText, OnDownloadHandler);
	}

	private void OnUpload(GUIBase_Widget inWidget)
	{
		string inCaption = TextDatabase.instance[2040602];
		string inText = TextDatabase.instance[2040604];
		m_OwnerMenu.ShowPopup("Confirm", inCaption, inText, OnUploadHandler);
	}

	private void OnDownloadHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			GameCloudManager.RestoreProgressFromCloud();
		}
	}

	private void OnUploadHandler(BasePopupScreen inPopup, E_PopupResultCode inResult)
	{
		if (inResult == E_PopupResultCode.Ok)
		{
			m_BackupDataAction = GameCloudManager.BackupProgressToCloud();
		}
	}

	private void RefreshView()
	{
		m_BackupView.ForceUpdateView();
	}
}
