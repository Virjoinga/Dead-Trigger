public class UserNameAlreadyExist : DefaultCloudAction
{
	public string userName { get; private set; }

	public bool userExist { get; private set; }

	public UserNameAlreadyExist(string inUserName, float inTimeOut = -1f)
		: base(null, inTimeOut)
	{
		userName = inUserName.ToLower();
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserNameExists(userName);
	}

	protected override void OnSuccess()
	{
		userExist = m_AsyncOp.m_ResultDesc == "ok";
	}
}
