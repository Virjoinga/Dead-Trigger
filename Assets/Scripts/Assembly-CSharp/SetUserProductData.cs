public class SetUserProductData : DefaultCloudAction
{
	public string dataID { get; private set; }

	public string dataValue { get; private set; }

	public SetUserProductData(UnigueUserID inUserID, string inDataID, string inDataValue, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		dataID = inDataID;
		dataValue = inDataValue;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserSetPerProductData(m_UserID.userName, m_UserID.productID, dataID, dataValue, m_UserID.passwordHash);
	}
}
