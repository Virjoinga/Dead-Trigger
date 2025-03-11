internal class SlotEquipAction : DefaultCloudAction
{
	private int m_ItemID;

	private int m_TeamIdx;

	private int m_SlotIdx;

	private bool m_Equip;

	public SlotEquipAction(UnigueUserID inUserID, int itemID, int teamIdx, int slotIdx, bool equip)
		: base(inUserID)
	{
		m_ItemID = itemID;
		m_TeamIdx = teamIdx;
		m_SlotIdx = slotIdx;
		m_Equip = equip;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		if (m_Equip)
		{
			return CloudServices.GetInstance().EquipItem(m_UserID.userName, m_UserID.productID, m_ItemID, m_TeamIdx, m_SlotIdx, m_UserID.passwordHash);
		}
		return CloudServices.GetInstance().UnEquipItem(m_UserID.userName, m_UserID.productID, m_ItemID, m_TeamIdx, m_SlotIdx, m_UserID.passwordHash);
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
