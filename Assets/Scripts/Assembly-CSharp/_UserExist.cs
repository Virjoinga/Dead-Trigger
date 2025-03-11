public class _UserExist : DefaultCloudAction
{
	public _UserExist(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserExists(m_UserID.userName, string.Empty, m_UserID.passwordHash);
	}

	protected override void OnSuccess()
	{
		if (m_AsyncOp.m_ResultDesc != "ok")
		{
			SetStatus(E_Status.Failed);
			base.failInfo = m_AsyncOp.m_ResultDesc;
		}
	}
}
