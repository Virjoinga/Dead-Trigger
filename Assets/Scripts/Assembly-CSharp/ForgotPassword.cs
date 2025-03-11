public class ForgotPassword : DefaultCloudAction
{
	public string userName { get; private set; }

	public ForgotPassword(string inUserName, float inTimeOut = -1f)
		: base(null, inTimeOut)
	{
		userName = inUserName.ToLower();
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().RequestResetPassword(userName, "Please follow this link to change the password:", "Reset Password");
	}
}
