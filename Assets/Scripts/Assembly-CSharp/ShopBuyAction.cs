internal class ShopBuyAction : DefaultCloudAction
{
	private int m_ItemID;

	public ShopBuyAction(UnigueUserID inUserID, int itemID)
		: base(inUserID)
	{
		m_ItemID = itemID;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().BuyItem(m_UserID.userName, m_UserID.productID, m_ItemID, m_UserID.passwordHash);
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
