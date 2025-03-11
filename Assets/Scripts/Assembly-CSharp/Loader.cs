using System;
using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour
{
	private enum E_State
	{
		ConfirmDialog = 0,
		DataAvailable = 1,
		Downloading = 2,
		InvalidDataError = 3,
		DataDownloadError = 4,
		DeviceError = 5,
		PlatformError = 6,
		StorageError = 7,
		NetworkError = 8,
		ExitDialog = 9
	}

	private enum E_DataFileStatus
	{
		NoStorage = 0,
		Missing = 1,
		NotBind = 2,
		Bind = 3
	}

	public GUIStyle m_LabelStyle;

	public GUIStyle m_ButtonFontStyle;

	public GUIStyle m_BackGroundStyle;

	private int m_SleepTimeout;

	private E_State m_State;

	private Rect m_ActiveScreen;

	private Rect mDefaultButtonPositionRect;

	private bool mDowloadInterrupted;

	private string m_FirstLevel = "City";

	private bool m_isRunningOnAndroid;

	private bool m_DownloadInProggres;

	private float NextDataCheckTime;

	private static string m_LineTextOffsetBegin = "\n\n";

	private static string m_LineTextOffsetEnd = "\n";

	private static string m_IntroText = "Application is going to download data for DEAD TRIGGER.\nYou may want to wait with download until you are on wi-fi network.\nExpect aproximatly 100 MB of data.";

	private static string m_DataDownloadErrorText = "Failed to download DEAD TRIGGER data. \nYou may have interrupted the download manually, if not please\ncheck your internet connectivity and try again latter";

	private static string m_InvalidDataErrorText = "Data file is broken or in invalid version. \nPlease try reinstall application";

	private static string m_UnsupportedDeviceText = "Unable to start DEAD TRIGGER. \nYOUR DEVICE IS NOT SUPPORTED.";

	private static string m_NoAndroidDeviceText = "OBB files downloader works only on Android devices! \nApplication is not able to download data!";

	private static string m_NoStorageDeviceText = "External storage is not available! \nApplication is not able to download data!";

	private static string m_NoNetworkText = "Network is not reachable! \nApplication is not able to download data!";

	private static string m_ExitText = "Do you really want to exit application?";

	private E_State State
	{
		get
		{
			return m_State;
		}
		set
		{
			m_State = value;
			Debug.Log("Loader.SetState " + value);
		}
	}

	private static string introText
	{
		get
		{
			return m_LineTextOffsetBegin + m_IntroText + m_LineTextOffsetEnd;
		}
	}

	private static string dataDownloadErrorText
	{
		get
		{
			return m_LineTextOffsetBegin + m_DataDownloadErrorText + m_LineTextOffsetEnd;
		}
	}

	private static string dataErrorText
	{
		get
		{
			return m_LineTextOffsetBegin + m_InvalidDataErrorText + m_LineTextOffsetEnd;
		}
	}

	private static string unsupportedDeviceText
	{
		get
		{
			return m_LineTextOffsetBegin + m_UnsupportedDeviceText + m_LineTextOffsetEnd;
		}
	}

	private static string noAndroidDeviceText
	{
		get
		{
			return m_LineTextOffsetBegin + m_NoAndroidDeviceText + m_LineTextOffsetEnd;
		}
	}

	private static string noStorageDeviceText
	{
		get
		{
			return m_LineTextOffsetBegin + m_NoStorageDeviceText + m_LineTextOffsetEnd;
		}
	}

	private static string noNetworkText
	{
		get
		{
			return m_LineTextOffsetBegin + m_NoNetworkText + m_LineTextOffsetEnd;
		}
	}

	private static string exitText
	{
		get
		{
			return m_LineTextOffsetBegin + m_ExitText + m_LineTextOffsetEnd;
		}
	}

	private void Start()
	{
		m_SleepTimeout = Screen.sleepTimeout;
		Screen.sleepTimeout = -1;
		UnityEngine.Object.DontDestroyOnLoad(this);
		m_ActiveScreen = new Rect(0f, 0f, Screen.width, Screen.height);
		GameObject gameObject = GameObject.Find("BackGroundImage");
		if (gameObject != null && gameObject.GetComponent<GUITexture>() != null && gameObject.GetComponent<GUITexture>().texture != null)
		{
			Vector2 vector = new Vector2(gameObject.GetComponent<GUITexture>().texture.width, gameObject.GetComponent<GUITexture>().texture.height);
			Vector2 vector2 = new Vector2(Screen.width, Screen.height);
			Vector2 vector3 = new Vector2(vector.x / vector2.x, vector.y / vector2.y);
			if (vector3.x > vector3.y)
			{
				float num = vector2.x / vector.x * vector.y;
				float num2 = vector2.y - num;
				gameObject.GetComponent<GUITexture>().pixelInset = new Rect(0f, num2 * 0.5f, 0f, 0f - num2);
			}
			else
			{
				float num3 = vector2.y / vector.y * vector.x;
				float num4 = vector2.x - num3;
				gameObject.GetComponent<GUITexture>().pixelInset = new Rect(num4 * 0.5f, 0f, 0f - num4, 0f);
			}
			m_ActiveScreen = new Rect(gameObject.GetComponent<GUITexture>().pixelInset.x, gameObject.GetComponent<GUITexture>().pixelInset.y, (float)Screen.width + gameObject.GetComponent<GUITexture>().pixelInset.width, (float)Screen.height + gameObject.GetComponent<GUITexture>().pixelInset.height);
		}
		//m_isRunningOnAndroid = GooglePlayDownloader.RunningOnAndroid();
		DataDownloaded();
		if (!m_isRunningOnAndroid)
		{
			State = E_State.PlatformError;
			return;
		}
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath == null)
		{
			State = E_State.StorageError;
			return;
		}
		string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
		bool flag = mainOBBPath != null;
		bool flag2 = false;
		if (mainOBBPath != null)
		{
			string value = mainOBBPath.Substring(expansionFilePath.Length);
			flag2 = Application.dataPath != null && Application.dataPath.EndsWith(value);
		}
		if (!flag)
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				State = E_State.NetworkError;
			}
			else
			{
				State = E_State.ConfirmDialog;
			}
			return;
		}
		if (!flag2)
		{
			State = E_State.InvalidDataError;
			return;
		}
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		State = E_State.DataAvailable;
		DataDownloaded();
	}

	private void Update()
	{
		if (State == E_State.DataAvailable)
		{
			return;
		}
		if (m_DownloadInProggres && NextDataCheckTime < Time.realtimeSinceStartup)
		{
			switch (GetDataFileStatus())
			{
			case E_DataFileStatus.NoStorage:
				m_DownloadInProggres = false;
				State = E_State.StorageError;
				break;
			case E_DataFileStatus.Missing:
				m_DownloadInProggres = false;
				State = E_State.DataDownloadError;
				break;
			case E_DataFileStatus.NotBind:
				m_DownloadInProggres = false;
				State = E_State.InvalidDataError;
				break;
			case E_DataFileStatus.Bind:
				m_DownloadInProggres = false;
				State = E_State.DataAvailable;
				DataDownloaded();
				break;
			}
			NextDataCheckTime = Time.realtimeSinceStartup + 5.5f;
		}
		if (PressedDeclineButton())
		{
			if (State == E_State.ExitDialog)
			{
				State = E_State.ConfirmDialog;
			}
			else if (State == E_State.ConfirmDialog)
			{
				State = E_State.ExitDialog;
			}
		}
	}

	private void OnGUI()
	{
		m_ButtonFontStyle = new GUIStyle(GUI.skin.button);
		if (State != E_State.DataAvailable)
		{
			int num = (int)(m_ActiveScreen.width * 0.48f);
			int num2 = (int)(m_ActiveScreen.height * 0.35f);
			Rect position = new Rect(m_ActiveScreen.xMin + (m_ActiveScreen.width - (float)num) * 0.06f, m_ActiveScreen.yMin + (m_ActiveScreen.height - (float)num2 - m_ActiveScreen.height * 0.15f), num, num2);
			SetFontSize(m_ButtonFontStyle, num2, num);
			SetFontSize(m_LabelStyle, num2, num);
			GUI.BeginGroup(position);
			GUI.Box(new Rect(0f, 0f, num, num2), string.Empty, m_BackGroundStyle);
			switch (State)
			{
			case E_State.ConfirmDialog:
				Gui_ConfirmDownloadDialog(num, num2);
				break;
			case E_State.InvalidDataError:
				Gui_ShowErrorDialog(num, num2, dataErrorText);
				break;
			case E_State.DataDownloadError:
				Gui_ShowErrorDialog(num, num2, dataDownloadErrorText);
				break;
			case E_State.DeviceError:
				Gui_ShowErrorDialog(num, num2, unsupportedDeviceText);
				break;
			case E_State.PlatformError:
				Gui_ShowErrorDialog(num, num2, noAndroidDeviceText);
				break;
			case E_State.StorageError:
				Gui_ShowErrorDialog(num, num2, noStorageDeviceText);
				break;
			case E_State.NetworkError:
				Gui_ShowErrorDialog(num, num2, noNetworkText);
				break;
			case E_State.ExitDialog:
				Gui_ExitDialog(num, num2);
				break;
			}
			GUI.EndGroup();
		}
	}

	private void Gui_ConfirmDownloadDialog(int inWidth, int inHeight)
	{
		GUILayout.Label(introText, m_LabelStyle, GUILayout.Width(inWidth));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUI.SetNextControlName("DownloadButton");
		string text = "Begin download";
		if (mDowloadInterrupted)
		{
			text = "Resume download";
		}
		if (GUILayout.Button(text, m_ButtonFontStyle, GUILayout.MaxWidth((float)inWidth * 0.5f), GUILayout.MinHeight((float)inHeight * 0.2f)) || PressedConfirmButton())
		{
			DownloadData();
		}
		GUI.FocusControl("DownloadButton");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	private void Gui_ShowErrorDialog(int inWidth, int inHeight, string inErrorlabel)
	{
		GUILayout.BeginVertical();
		GUILayout.Label(inErrorlabel, m_LabelStyle, GUILayout.Width(inWidth), GUILayout.Height((int)((float)inHeight * 0.5f)));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUI.SetNextControlName("ExitButton");
		string text = "Quit";
		if (State == E_State.DataDownloadError || State == E_State.NetworkError)
		{
			mDowloadInterrupted = true;
			text = "Retry";
		}
		if (GUILayout.Button(text, m_ButtonFontStyle, GUILayout.MaxWidth((float)inWidth * 0.35f), GUILayout.MinHeight((float)inHeight * 0.25f)) || PressedConfirmButton())
		{
			if (State == E_State.DataDownloadError || State == E_State.NetworkError)
			{
				State = E_State.ConfirmDialog;
			}
			else
			{
				Application.Quit();
			}
		}
		GUI.FocusControl("ExitButton");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void Gui_ExitDialog(int inWidth, int inHeight)
	{
		GUI.Button(new Rect(-10000f, -10000f, 0f, 0f), GUIContent.none);
		GUILayout.BeginVertical();
		GUILayout.Label(exitText, m_LabelStyle, GUILayout.Width(inWidth));
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUI.SetNextControlName("YesButton");
		bool flag = GUILayout.Button("Yes", m_ButtonFontStyle, GUILayout.MaxWidth((float)inWidth * 0.25f), GUILayout.MinHeight((float)inHeight * 0.15f));
		GUILayout.FlexibleSpace();
		GUI.SetNextControlName("NoButton");
		bool flag2 = GUILayout.Button("No", m_ButtonFontStyle, GUILayout.MaxWidth((float)inWidth * 0.25f), GUILayout.MinHeight((float)inHeight * 0.15f));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		float axis = Input.GetAxis("HorizontalMove");
		if (axis < 0f)
		{
			GUI.FocusControl("YesButton");
		}
		else if (axis > 0f)
		{
			GUI.FocusControl("NoButton");
		}
		if (flag || (GUI.GetNameOfFocusedControl() == "YesButton" && PressedConfirmButton()))
		{
			Application.Quit();
		}
		else if (flag2 || (GUI.GetNameOfFocusedControl() == "NoButton" && PressedConfirmButton()))
		{
			State = E_State.ConfirmDialog;
		}
	}

	private bool PressedConfirmButton()
	{
		return Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0);
	}

	private bool PressedDeclineButton()
	{
		return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown("escape");
	}

	private E_DataFileStatus GetDataFileStatus()
	{
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath == null)
		{
			return E_DataFileStatus.NoStorage;
		}
		string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
		if (mainOBBPath == null)
		{
			return E_DataFileStatus.Missing;
		}
		string value = mainOBBPath.Substring(expansionFilePath.Length);
		if (Application.dataPath == null || !Application.dataPath.EndsWith(value))
		{
			return E_DataFileStatus.NotBind;
		}
		return E_DataFileStatus.Bind;
	}

	private void DownloadData()
	{
		GooglePlayDownloader.FetchOBB();
		State = E_State.Downloading;
		NextDataCheckTime = Time.realtimeSinceStartup + 1f;
		m_DownloadInProggres = true;
	}

	private void DataDownloaded(bool inWithEmpty = true)
	{
		Screen.sleepTimeout = m_SleepTimeout;
		StartCoroutine(LoadScene(m_FirstLevel, inWithEmpty));
		base.enabled = false;
	}

	private IEnumerator LoadScene(string scene, bool inWithEmpty)
	{
		if (inWithEmpty)
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			Application.LoadLevel("empty");
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Application.LoadLevel(scene);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
	}

	private void SetFontSize(GUIStyle labelStyle, int surroundingQuadH, int surroundingQuadW)
	{
		labelStyle.fontSize = (int)(Math.Sqrt(surroundingQuadH * surroundingQuadW) * 0.05000000074505806);
		labelStyle.wordWrap = true;
	}
}
