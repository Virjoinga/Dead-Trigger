using System.Collections.Generic;

public class CloudActionSerial : BaseCloudAction
{
	private List<BaseCloudAction> m_Actions;

	public CloudActionSerial(UnigueUserID inUserID, float inTimeOut = -1f, params BaseCloudAction[] inActions)
		: base(inUserID, inTimeOut)
	{
		m_Actions = new List<BaseCloudAction>(inActions);
		if (m_Actions == null || m_Actions.Count <= 0)
		{
			SetStatus(E_Status.Success);
		}
	}

	public override E_Status PPIManager_Update()
	{
		if (base.status == E_Status.Pending)
		{
			SetStatus(E_Status.InProggres);
		}
		else if (base.status == E_Status.Failed || base.status == E_Status.Success)
		{
			return base.status;
		}
		BaseCloudAction baseCloudAction = m_Actions[0];
		switch (baseCloudAction.PPIManager_Update())
		{
		case E_Status.Failed:
			m_Actions = null;
			base.failInfo = baseCloudAction.failInfo;
			SetStatus(E_Status.Failed);
			OnFailed();
			break;
		case E_Status.Success:
			m_Actions.Remove(baseCloudAction);
			if (m_Actions == null || m_Actions.Count <= 0)
			{
				SetStatus(E_Status.Success);
				OnSuccess();
			}
			break;
		default:
			if (base.timeOut > 0f && base.activeTime > base.timeOut)
			{
				base.failInfo = "Action timeout expired!";
				SetStatus(E_Status.Failed);
				OnFailed();
			}
			break;
		}
		return base.status;
	}

	protected virtual void OnSuccess()
	{
	}

	protected virtual void OnFailed()
	{
	}
}
