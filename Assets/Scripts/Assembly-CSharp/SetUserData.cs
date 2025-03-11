public class SetUserData : DefaultCloudAction
{
	public string dataID { get; private set; }

	public string dataValue { get; private set; }

	public SetUserData(UnigueUserID inUserID, string inDataID, string inDataValue, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		dataID = inDataID;
		dataValue = inDataValue;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().UserSetData(m_UserID.userName, dataID, dataValue, m_UserID.passwordHash);
	}
}
