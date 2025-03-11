public class SendMessage : DefaultCloudAction
{
	public string recipient { get; private set; }

	public string message { get; private set; }

	public bool globalInbox { get; private set; }

	public SendMessage(UnigueUserID inUserID, string inRecipient, string inMessage, bool inGlobalInbox, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		recipient = inRecipient;
		message = inMessage;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		string productId = ((!globalInbox) ? m_UserID.productID : null);
		return CloudServices.GetInstance().InboxAddMsg(m_UserID.userName, recipient, productId, message, m_UserID.passwordHash);
	}
}
