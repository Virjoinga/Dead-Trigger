public class QueryFriendsInfo : DefaultCloudAction
{
	public string friends { get; private set; }

	public QueryFriendsInfo(UnigueUserID inUserID, string inFriends, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		friends = inFriends;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().QueryFriendsInfo(m_UserID.userName, m_UserID.productID, friends, m_UserID.passwordHash);
	}
}
