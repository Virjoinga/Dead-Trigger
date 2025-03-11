public class SendFriendRequestMessage : SendMessage
{
	public SendFriendRequestMessage(UnigueUserID inUserID, string inRecipient, string inMessage, float inTimeOut = -1f)
		: base(inUserID, inRecipient, inMessage, true, inTimeOut)
	{
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().RequestAddFriend(m_UserID.userName, base.recipient, base.message, m_UserID.passwordHash);
	}
}
