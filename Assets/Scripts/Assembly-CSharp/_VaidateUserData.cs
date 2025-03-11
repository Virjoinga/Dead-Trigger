public class _VaidateUserData : DefaultCloudAction
{
	public _VaidateUserData(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().ValidateUserAccount(m_UserID.userName, m_UserID.productID, m_UserID.passwordHash);
	}
}
