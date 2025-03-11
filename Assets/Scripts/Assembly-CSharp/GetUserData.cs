public class GetUserData : DefaultCloudAction
{
	public string dataID { get; private set; }

	public GetUserData(UnigueUserID inUserID, string inDataID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		dataID = inDataID;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserGetData(m_UserID.userName, dataID, m_UserID.passwordHash);
	}

	protected override void OnSuccess()
	{
		if (m_AsyncOp.m_ResultDesc == "notfound")
		{
			SetStatus(E_Status.Failed);
			base.failInfo = m_AsyncOp.m_ResultDesc;
		}
	}
}
