public class RemoveMessagesFromInbox : DefaultCloudAction
{
	public bool globalInbox { get; private set; }

	public int lastMsgIndex { get; private set; }

	public RemoveMessagesFromInbox(UnigueUserID inUserID, bool inGlobalInbox, int inLastMsgIndex, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		globalInbox = inGlobalInbox;
		lastMsgIndex = inLastMsgIndex;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		string productId = ((!globalInbox) ? m_UserID.productID : null);
		return CloudServices.GetInstance().InboxRemoveMessages(m_UserID.userName, productId, lastMsgIndex, m_UserID.passwordHash);
	}
}
