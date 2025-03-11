using System.Collections.Generic;
using LitJson;
using UnityEngine;

[AddComponentMenu("Cloud services/CloudServicesTester")]
public class CloudServiceTester : MonoBehaviour
{
	public string m_UserID = "culibrk";

	public string m_FriendID = "hovko";

	public string m_ProductID = "ShadowgunMP";

	public string m_ProductDataID = "testDataID";

	public string m_ProductData = "someValue";

	public string m_UserDataID = "testDataID";

	public string m_UserData = "someValue";

	public string m_ItemDataID = "NumKills";

	public string m_ItemData = "2";

	public string m_InitialPlayerData = "initialPlayerData";

	public string m_Password = "testPassword17673a";

	public int m_ItemID = -1;

	public int m_SlotIdx;

	public string m_Message = "{}";

	public string m_TransactionId;

	public string m_ReceiptData;

	private void CreateUsers(int numUsers)
	{
		CloudServices.AsyncOpResult[] array = new CloudServices.AsyncOpResult[numUsers];
		string passwordHash = CloudServices.CalcPasswordHash(m_Password);
		for (int i = 0; i < numUsers; i++)
		{
			array[i] = CloudServices.GetInstance().CreateUser(m_UserID + i, m_ProductID, passwordHash);
		}
	}

	private void OnGUI()
	{
		float width = 200f;
		float height = 30f;
		float num = 20f;
		float num2 = 40f;
		if (GUI.Button(new Rect(20f, num, width, height), "Create user"))
		{
			CloudServices.GetInstance().CreateUser(m_UserID, m_ProductID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "User exists"))
		{
			CloudServices.GetInstance().UserExists(m_UserID, string.Empty, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Create product"))
		{
			CloudServices.GetInstance().CreateProduct(m_ProductID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Set product data"))
		{
			CloudServices.GetInstance().ProductSetParam(m_ProductID, m_ProductDataID, m_ProductData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Get product data"))
		{
			CloudServices.GetInstance().ProductGetParam(m_ProductID, m_ProductDataID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Set user per-product data"))
		{
			CloudServices.GetInstance().UserSetPerProductData(m_UserID, m_ProductID, m_UserDataID, m_UserData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Get user per-product data"))
		{
			CloudServices.GetInstance().UserGetPerProductData(m_UserID, m_ProductID, m_UserDataID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Set player data section"))
		{
			CloudServices.GetInstance().UserSetPerProductDataSection(m_UserID, m_ProductID, "_PlayerData", m_UserDataID, m_UserData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Update player data section"))
		{
			CloudServices.GetInstance().UserUpdatePerProductDataSection(m_UserID, m_ProductID, "_PlayerData", m_UserDataID, m_UserData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Set user data"))
		{
			CloudServices.GetInstance().UserSetData(m_UserID, m_UserDataID, m_UserData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Get user data"))
		{
			CloudServices.GetInstance().UserGetData(m_UserID, m_UserDataID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Buy item"))
		{
			CloudServices.GetInstance().BuyItem(m_UserID, m_ProductID, m_ItemID, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Equip item"))
		{
			CloudServices.GetInstance().EquipItem(m_UserID, m_ProductID, m_ItemID, -1, m_SlotIdx, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "UnEquip item"))
		{
			CloudServices.GetInstance().UnEquipItem(m_UserID, m_ProductID, m_ItemID, -1, m_SlotIdx, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Modify item"))
		{
			CloudServices.GetInstance().ModifyItem(m_UserID, m_ProductID, m_ItemID, m_ItemDataID, m_ItemData, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Hash password"))
		{
			Debug.Log(CloudServices.CalcPasswordHash(m_Password));
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Default PPI"))
		{
			PlayerPersistantInfo defaultPPI = PlayerPersistantInfo.GetDefaultPPI();
			Debug.Log(defaultPPI.GetPlayerDataAsJsonStr());
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "Set friends"))
		{
			List<string> list = new List<string>();
			list.Add("Alexščřá");
			list.Add("MrPicus4");
			list.Add("alex");
			list.Add("jv");
			list.Add("Roarke");
			string val = JsonMapper.ToJson(list);
			CloudServices.GetInstance().UserSetData(m_UserID, "_Friends", val, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
		}
		num += num2;
		if (GUI.Button(new Rect(20f, num, width, height), "VerifyReceipt"))
		{
			CloudServices.GetInstance().VerifyStoreKitReceipt(m_TransactionId, m_ReceiptData, AsyncOpFinished);
		}
		num += num2;
	}

	private void AsyncOpFinished(CloudServices.AsyncOpResult res)
	{
		Debug.Log(res.m_ResultDesc);
	}

	private void FetchMessagesAsyncOpFinished(CloudServices.AsyncOpResult res)
	{
		if (res.m_Res)
		{
			Debug.Log(res.m_ResultDesc);
			JsonData jsonData = JsonMapper.ToObject(res.m_ResultDesc);
			string[] array = JsonMapper.ToObject<string[]>((string)jsonData["messages"]);
			int lastMessageIdx = (int)jsonData["lastMsgIdx"];
			string[] array2 = array;
			foreach (string text in array2)
			{
				Debug.Log(text);
				JsonData jsonData2 = JsonMapper.ToObject(text);
				if (jsonData2.HasValue("_respCmd"))
				{
					JsonData jsonData3 = JsonMapper.ToObject((string)jsonData2["_respCmd"]);
					Debug.Log(jsonData3["param"]);
					CloudServices.GetInstance().ProcessResponseCmd((string)jsonData3["param"], AsyncOpFinished);
				}
			}
			if (array.Length > 0)
			{
				CloudServices.GetInstance().InboxRemoveMessages(m_UserID, null, lastMessageIdx, CloudServices.CalcPasswordHash(m_Password), AsyncOpFinished);
			}
		}
		else
		{
			Debug.LogError("Error fetching messages");
		}
	}
}
