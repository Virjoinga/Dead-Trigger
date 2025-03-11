public class GetMessagesFromInbox : DefaultCloudAction
{
	public bool globalInbox { get; private set; }

	public GetMessagesFromInbox(UnigueUserID inUserID, bool inGlobalInbox, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		globalInbox = inGlobalInbox;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		string productId = ((!globalInbox) ? m_UserID.productID : null);
		return CloudServices.GetInstance().FetchInboxMessages(m_UserID.userName, productId, m_UserID.passwordHash);
	}
}
