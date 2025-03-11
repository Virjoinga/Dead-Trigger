using UnityEngine;

[AddComponentMenu("Cloud services/CloudServiceUtils")]
public class CloudServiceUtils : MonoBehaviour
{
	public string m_ProductId = "ShadowgunMP";

	public string m_ProductPassword = "jn83v983naks";

	public string m_AdminPasswordHash = string.Empty;

	private string m_StrKey = string.Empty;

	private string m_StrVal = string.Empty;

	private string m_StrMsg = string.Empty;

	private void OnGUI()
	{
		float width = 200f;
		float width2 = 200f;
		float num = 30f;
		float num2 = 20f;
		float num3 = 40f;
		GUI.Label(new Rect(20f, num2, width, num), "ID");
		num2 += num3;
		m_StrKey = GUI.TextField(new Rect(20f, num2, 500f, num), m_StrKey);
		num2 += num3;
		GUI.Label(new Rect(20f, num2, width, num), "Value");
		num2 += num3;
		m_StrVal = GUI.TextArea(new Rect(20f, num2, 500f, num * 3f), m_StrVal);
		num2 += num3;
		num2 += num3;
		num2 += num3;
		GUI.Label(new Rect(20f, num2, width, num), "Message");
		num2 += num3;
		m_StrMsg = GUI.TextArea(new Rect(20f, num2, 500f, num * 3f), m_StrMsg);
		num2 += num3;
		num2 += num3;
		num2 += num3;
		if (GUI.Button(new Rect(20f, num2, width2, num), "Set product param"))
		{
			CloudServices.GetInstance().ProductSetParam(m_ProductId, m_StrKey, m_StrVal, CloudServices.CalcPasswordHash(m_ProductPassword), AsyncOpFinished);
		}
		num2 += num3;
		if (GUI.Button(new Rect(20f, num2, width2, num), "Add product inbox message"))
		{
			CloudServices.GetInstance().ProductInboxAddMsg(m_ProductId, m_StrMsg, m_AdminPasswordHash, AsyncOpFinished);
		}
		num2 += num3;
	}

	private void AsyncOpFinished(CloudServices.AsyncOpResult res)
	{
	}
}
