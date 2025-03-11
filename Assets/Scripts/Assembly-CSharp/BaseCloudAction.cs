using UnityEngine;

public abstract class BaseCloudAction
{
	public enum E_Status
	{
		Pending = 0,
		InProggres = 1,
		Success = 2,
		Failed = 3
	}

	public const float DefaultTimeOut = 30f;

	public const float NoTimeOut = -1f;

	protected UnigueUserID m_UserID;

	protected CloudServices.AsyncOpResult m_AsyncOp;

	public E_Status status { get; private set; }

	public bool isDone
	{
		get
		{
			return status == E_Status.Failed || status == E_Status.Success;
		}
	}

	public bool isFailed
	{
		get
		{
			return status == E_Status.Failed;
		}
	}

	public bool isSucceeded
	{
		get
		{
			return status == E_Status.Success;
		}
	}

	public string failInfo { get; protected set; }

	public string result { get; protected set; }

	public float timeOut { get; private set; }

	public float createTime { get; private set; }

	public float activationTime { get; private set; }

	public float lifeTime
	{
		get
		{
			return Time.realtimeSinceStartup - createTime;
		}
	}

	public float activeTime
	{
		get
		{
			return (status != 0) ? (Time.realtimeSinceStartup - activationTime) : 0f;
		}
	}

	public BaseCloudAction(UnigueUserID inUserID, float inTimeOut = -1f)
	{
		m_UserID = inUserID;
		timeOut = inTimeOut;
		status = E_Status.Pending;
		failInfo = string.Empty;
		createTime = Time.realtimeSinceStartup;
	}

	public abstract E_Status PPIManager_Update();

	protected void SetStatus(E_Status inStatus)
	{
		if (inStatus != status)
		{
			if (inStatus == E_Status.InProggres)
			{
				activationTime = Time.realtimeSinceStartup;
			}
			status = inStatus;
		}
	}
}
