public class AuthenticateUser : CloudActionSerial
{
	public AuthenticateUser(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut, new _UserExist(inUserID), new _VaidateUserData(inUserID))
	{
	}
}
