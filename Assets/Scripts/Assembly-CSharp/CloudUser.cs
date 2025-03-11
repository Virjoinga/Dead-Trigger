using System.Collections;
using UnityEngine;

public class CloudUser : MonoBehaviour
{
	public enum E_AuthenticationStatus
	{
		None = 0,
		InProgress = 1,
		RetrievingPPI = 2,
		Ok = 3,
		Failed = 4
	}

	public static int MIN_ACCOUNT_NAME_LENGTH = 6;

	public static int MAX_ACCOUNT_NAME_LENGTH = 18;

	public static int MIN_PASSWORD_LENGTH = 8;

	private static CloudUser ms_Instance;

	private string m_NickName;

	private string m_UserName;

	private string m_PasswordHash;

	private int m_PasswordLength = 8;

	private bool m_AutoLogin;

	private bool m_SkipAutoLogin;

	private bool m_AuthenticationDataLoaded;

	private UnigueUserID m_AuthenticatedUserID;

	private E_AuthenticationStatus m_AuthenticationStatus;

	private string m_LastAuthenticationFailReason;

	public static CloudUser instance
	{
		get
		{
			return GetInstance();
		}
	}

	public string nickName
	{
		get
		{
			return (!string.IsNullOrEmpty(m_NickName)) ? m_NickName : m_UserName;
		}
		set
		{
			m_NickName = value;
		}
	}

	public string userName
	{
		get
		{
			return m_UserName;
		}
	}

	public string passwordHash
	{
		get
		{
			return m_PasswordHash;
		}
	}

	public bool autoLogin
	{
		get
		{
			return m_AutoLogin;
		}
	}

	public bool isUserAuthenticated
	{
		get
		{
			return authenticationStatus == E_AuthenticationStatus.Ok;
		}
	}

	public bool authenticationDataPresent
	{
		get
		{
			return m_UserName != null && m_PasswordHash != null && m_PasswordLength > 0;
		}
	}

	public UnigueUserID authenticatedUserID
	{
		get
		{
			return m_AuthenticatedUserID;
		}
		set
		{
			m_AuthenticatedUserID = value;
		}
	}

	private static string productID
	{
		get
		{
			return GameCloudManager.productID;
		}
	}

	public E_AuthenticationStatus authenticationStatus
	{
		get
		{
			return m_AuthenticationStatus;
		}
	}

	public string lastAuthenticationFail
	{
		get
		{
			return m_LastAuthenticationFailReason;
		}
	}

	public bool authenticationInProggres
	{
		get
		{
			return authenticationStatus == E_AuthenticationStatus.InProgress || authenticationStatus == E_AuthenticationStatus.RetrievingPPI;
		}
	}

	private void OnDestroy()
	{
		ms_Instance = null;
	}

	private static CloudUser GetInstance()
	{
		if (ms_Instance == null)
		{
			GameObject gameObject = new GameObject("CloudUser");
			ms_Instance = gameObject.AddComponent<CloudUser>();
			Object.DontDestroyOnLoad(ms_Instance);
			ms_Instance.LoadAuthenticationData();
		}
		return ms_Instance;
	}

	public UserNameAlreadyExist CheckIfUserNameExist(string inUserName)
	{
		UserNameAlreadyExist userNameAlreadyExist = new UserNameAlreadyExist(inUserName);
		GameCloudManager.AddAction(userNameAlreadyExist);
		return userNameAlreadyExist;
	}

	public CreateNewMFAccount CreateNewUser(string inUserName, string inPasswordHash, string inNickName, string inEmail, bool inIWantNews)
	{
		UnigueUserID inUserID = new UnigueUserID(inUserName, inPasswordHash, productID);
		CreateNewMFAccount createNewMFAccount = new CreateNewMFAccount(inUserID, inNickName, inEmail, inIWantNews);
		GameCloudManager.AddAction(createNewMFAccount);
		m_AuthenticationStatus = E_AuthenticationStatus.None;
		return createNewMFAccount;
	}

	public bool CanAutoAuthenticate()
	{
		return !m_SkipAutoLogin && m_AutoLogin && !isUserAuthenticated && authenticationDataPresent && Application.internetReachability != NetworkReachability.NotReachable;
	}

	public void AuthenticateLocalUser()
	{
		if (authenticationStatus != E_AuthenticationStatus.InProgress && authenticationStatus != E_AuthenticationStatus.Ok)
		{
			if (!authenticationDataPresent)
			{
				m_AuthenticationStatus = E_AuthenticationStatus.Failed;
				m_LastAuthenticationFailReason = "Authentication data are missing";
			}
			else
			{
				StartCoroutine(AuthenticateUser_Corutine());
			}
		}
	}

	public void LogoutLocalUser()
	{
		m_AuthenticationStatus = E_AuthenticationStatus.None;
		m_SkipAutoLogin = true;
	}

	private IEnumerator AuthenticateUser_Corutine()
	{
		UnigueUserID userID = new UnigueUserID(userName, passwordHash, productID);
		m_AuthenticationStatus = E_AuthenticationStatus.InProgress;
		BaseCloudAction action = new AuthenticateUser(userID);
		GameCloudManager.AddAction(action);
		while (!action.isDone)
		{
			yield return new WaitForSeconds(0.2f);
		}
		if (action.status == BaseCloudAction.E_Status.Success)
		{
			authenticatedUserID = userID;
			m_AuthenticationStatus = E_AuthenticationStatus.Ok;
			GameCloudManager.friendList.OnUserAuthenticate();
			GameCloudManager.mailbox.OnUserAuthenticate();
		}
		else
		{
			m_AuthenticationStatus = E_AuthenticationStatus.Failed;
			m_LastAuthenticationFailReason = action.failInfo;
			Debug.LogError("Authentication failed");
		}
	}

	public bool GetLoginData(ref string outNickName, ref string outUserName, ref string outPaswordHash, ref int outPasswordLength, ref bool outRemeberLoginData, ref bool outAutoLogin)
	{
		if (!authenticationDataPresent)
		{
			return false;
		}
		outNickName = m_NickName;
		outUserName = m_UserName;
		outPaswordHash = m_PasswordHash;
		outPasswordLength = m_PasswordLength;
		outRemeberLoginData = m_AuthenticationDataLoaded;
		outAutoLogin = m_AutoLogin;
		return true;
	}

	public void SetLoginData(string inNickName, string inUserName, string inPaswordHash, int inPasswordLength, bool inRemeberThem, bool inAutoLogin)
	{
		m_NickName = ((!string.IsNullOrEmpty(inNickName)) ? inNickName : null);
		m_UserName = ((!string.IsNullOrEmpty(inUserName)) ? inUserName.ToLower() : null);
		m_PasswordHash = ((!string.IsNullOrEmpty(inPaswordHash)) ? inPaswordHash : null);
		m_PasswordLength = inPasswordLength;
		m_AutoLogin = inAutoLogin;
		m_AuthenticatedUserID = new UnigueUserID(userName, passwordHash, productID);
		m_AuthenticationDataLoaded = inRemeberThem;
		if (inRemeberThem)
		{
			PlayerPrefs.SetString("CloudUser.NickName", m_NickName);
			PlayerPrefs.SetString("CloudUser.UserName", m_UserName);
			PlayerPrefs.SetString("CloudUser.PasswordHash", m_PasswordHash);
			PlayerPrefs.SetInt("CloudUser.PasswordLength", m_PasswordLength);
			PlayerPrefs.SetInt("CloudUser.AutoLogin", m_AutoLogin ? 1 : 0);
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.DeleteKey("CloudUser.NickName");
			PlayerPrefs.DeleteKey("CloudUser.UserName");
			PlayerPrefs.DeleteKey("CloudUser.PasswordHash");
			PlayerPrefs.DeleteKey("CloudUser.PasswordLength");
			PlayerPrefs.DeleteKey("CloudUser.AutoLogin");
			PlayerPrefs.Save();
		}
	}

	public void LoadAuthenticationData()
	{
		m_NickName = PlayerPrefs.GetString("CloudUser.NickName", null);
		m_UserName = PlayerPrefs.GetString("CloudUser.UserName", null);
		m_PasswordHash = PlayerPrefs.GetString("CloudUser.PasswordHash", null);
		m_PasswordLength = PlayerPrefs.GetInt("CloudUser.PasswordLength", 0);
		m_AutoLogin = ((PlayerPrefs.GetInt("CloudUser.AutoLogin", 0) != 0) ? true : false);
		m_AutoLogin = true;
		if (!string.IsNullOrEmpty(m_UserName))
		{
			m_UserName = m_UserName.ToLower();
		}
		m_AuthenticationDataLoaded = m_UserName != null && m_PasswordHash != null && m_PasswordLength > 0;
		if (m_AuthenticationDataLoaded)
		{
			m_AuthenticatedUserID = new UnigueUserID(userName, passwordHash, productID);
		}
	}
}
