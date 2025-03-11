public class GetUserProductData : DefaultCloudAction
{
	public string dataID { get; private set; }

	public GetUserProductData(UnigueUserID inUserID, string inDataID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		dataID = inDataID;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserGetPerProductData(m_UserID.userName, m_UserID.productID, dataID, m_UserID.passwordHash);
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
