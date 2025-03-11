public class _CreateNewUser : DefaultCloudAction
{
	public _CreateNewUser(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().CreateUser(m_UserID.userName, m_UserID.productID, m_UserID.passwordHash);
	}
}
