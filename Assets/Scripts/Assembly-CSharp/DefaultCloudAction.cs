public abstract class DefaultCloudAction : BaseCloudAction
{
	public DefaultCloudAction(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
	}

	public override E_Status PPIManager_Update()
	{
		if (base.status == E_Status.Pending)
		{
			m_AsyncOp = GetCloudAsyncOp();
			SetStatus((m_AsyncOp != null) ? E_Status.InProggres : E_Status.Failed);
		}
		if (base.status == E_Status.InProggres)
		{
			if (m_AsyncOp.m_Finished)
			{
				if (!m_AsyncOp.m_Res)
				{
					base.failInfo = m_AsyncOp.m_ResultDesc;
					SetStatus(E_Status.Failed);
					OnFailed();
				}
				else
				{
					base.result = m_AsyncOp.m_ResultDesc;
					SetStatus(E_Status.Success);
					OnSuccess();
				}
			}
			else if (base.timeOut > 0f && base.activeTime > base.timeOut)
			{
				base.failInfo = "Action timeout expired!";
				SetStatus(E_Status.Failed);
				OnFailed();
			}
		}
		return base.status;
	}

	protected abstract CloudServices.AsyncOpResult GetCloudAsyncOp();

	protected virtual void OnSuccess()
	{
	}

	protected virtual void OnFailed()
	{
	}
}
