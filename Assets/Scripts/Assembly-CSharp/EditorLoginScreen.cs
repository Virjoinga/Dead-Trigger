using UnityEngine;

public class EditorLoginScreen : BaseMenuScreen
{
	private UserNameAlreadyExist m_CloudActionUserName;

	private CreateNewMFAccount m_CloudActionCreateUser;

	private bool userAuthenticationinProgress;

	private bool userNameExist;

	private GUIStyle infoStyle;

	private string infoString = string.Empty;

	private string nickName = string.Empty;

	private string userName = string.Empty;

	private string testedUserName = string.Empty;

	private string password = string.Empty;
}
