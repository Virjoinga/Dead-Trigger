public class CreateNewMFAccount : CloudActionSerial
{
	public string email { get; private set; }

	public string iWantNews { get; private set; }

	public CreateNewMFAccount(UnigueUserID inUserID, string inNickName, string inEmail, bool iniWantNews, float inTimeOut = -1f)
		: base(inUserID, inTimeOut, new _CreateNewUser(inUserID), new SetUserData(inUserID, "NickName", inNickName), new SetUserData(inUserID, "Email", inEmail), new SetUserData(inUserID, "IWantNews", iniWantNews.ToString()))
	{
		email = inEmail;
		iWantNews = iniWantNews.ToString();
	}
}
