using System;
using LitJson;
using UnityEngine;

public class RegisterForPushNotifications : DefaultCloudAction
{
	public enum Provider
	{
		Apple = 0,
		Google = 1
	}

	public const string RESULT_REGISTERED = "registered";

	public const string RESULT_UNREGISTERED = "unregistered";

	public Provider provider { get; private set; }

	public string registrationId { get; private set; }

	public bool register { get; private set; }

	public RegisterForPushNotifications(UnigueUserID inUserID, Provider provider, string registrationId, bool register, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		this.provider = provider;
		this.registrationId = registrationId;
		this.register = register;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().RegisterForPushNotifications(m_UserID.userName, m_UserID.productID, provider.ToString(), registrationId, register, m_UserID.passwordHash);
	}

	protected override void OnSuccess()
	{
		E_HttpResultCode e_HttpResultCode = E_HttpResultCode.None;
		try
		{
			JsonData jsonData = JsonMapper.ToObject(base.result);
			JsonData jsonData2 = ((!jsonData.HasValue("httpResult")) ? null : jsonData["httpResult"]);
			if (jsonData2 != null)
			{
				e_HttpResultCode = (jsonData2.HasValue("code") ? ((E_HttpResultCode)(int)jsonData2["code"]) : E_HttpResultCode.None);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to parse result data when registering push id to cloud -> " + ex.ToString());
		}
		base.result = string.Empty;
		if (e_HttpResultCode >= E_HttpResultCode.Ok && e_HttpResultCode < E_HttpResultCode.BadRequest)
		{
			base.result = ((!register) ? "unregistered" : "registered");
		}
	}
}
