using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LitJson;
using UnityEngine;

public class MFGooglePlayBillingManagerV3 : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action<int, bool> SetupFinishedEvent;

	[method: MethodImpl(32)]
	public static event Action<int, MFGooglePlayBillingTransactionV3> PurchaseFinishedEvent;

	[method: MethodImpl(32)]
	public static event Action<int, List<MFGooglePlayBillingProductV3>> QueryProductsDetailsFinishedEvent;

	[method: MethodImpl(32)]
	public static event Action<int, MFGooglePlayBillingTransactionV3> ConsumeFinishedEvent;

	[method: MethodImpl(32)]
	public static event Action<int, List<MFGooglePlayBillingTransactionV3>> QueryPurchasesFinishedEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void OnSetupFinished(string msgJSON)
	{
		JsonData jsonData = JsonMapper.ToObject(msgJSON);
		int arg = (int)jsonData["result"];
		try
		{
			bool arg2 = (bool)jsonData["billingSupported"];
			MFGooglePlayBillingManagerV3.SetupFinishedEvent(arg, arg2);
		}
		catch
		{
			if (MFGooglePlayBillingManagerV3.SetupFinishedEvent != null)
			{
				MFGooglePlayBillingManagerV3.SetupFinishedEvent(arg, false);
			}
		}
	}

	public void OnPurchaseFinished(string msgJSON)
	{
		JsonData jsonData = JsonMapper.ToObject(msgJSON);
		int arg = (int)jsonData["result"];
		try
		{
			string receiptJSON = (string)jsonData["purchaseData"];
			string signature = (string)jsonData["purchaseSignature"];
			MFGooglePlayBillingManagerV3.PurchaseFinishedEvent(arg, new MFGooglePlayBillingTransactionV3(receiptJSON, signature));
		}
		catch
		{
			if (MFGooglePlayBillingManagerV3.PurchaseFinishedEvent != null)
			{
				MFGooglePlayBillingManagerV3.PurchaseFinishedEvent(arg, null);
			}
		}
	}

	public void OnQueryProductsDetailsFinished(string msgJSON)
	{
		JsonData jsonData = JsonMapper.ToObject(msgJSON);
		int num = (int)jsonData["result"];
		try
		{
			JsonData jsonData2 = jsonData["productsDetails"];
			List<MFGooglePlayBillingProductV3> list = new List<MFGooglePlayBillingProductV3>(jsonData2.Count);
			for (int i = 0; i < jsonData2.Count; i++)
			{
				MFGooglePlayBillingProductV3 item = new MFGooglePlayBillingProductV3(jsonData2[i]);
				list.Add(item);
			}
			MFGooglePlayBillingManagerV3.QueryProductsDetailsFinishedEvent(num, list);
		}
		catch
		{
			if (MFGooglePlayBillingManagerV3.QueryProductsDetailsFinishedEvent != null)
			{
				MFGooglePlayBillingManagerV3.QueryProductsDetailsFinishedEvent((num != 0) ? num : 6, null);
			}
		}
	}

	public void OnConsumeFinished(string msgJSON)
	{
		JsonData jsonData = JsonMapper.ToObject(msgJSON);
		int arg = (int)jsonData["result"];
		try
		{
			string receiptJSON = JsonMapper.ToObject((string)jsonData["purchaseData"]).ToJson();
			MFGooglePlayBillingManagerV3.ConsumeFinishedEvent(arg, new MFGooglePlayBillingTransactionV3(receiptJSON, string.Empty));
		}
		catch
		{
			if (MFGooglePlayBillingManagerV3.ConsumeFinishedEvent != null)
			{
				MFGooglePlayBillingManagerV3.ConsumeFinishedEvent(arg, null);
			}
		}
	}

	public void OnQueryPurchasesFinished(string msgJSON)
	{
		JsonData jsonData = JsonMapper.ToObject(msgJSON);
		int arg = (int)jsonData["result"];
		try
		{
			JsonData jsonData2 = jsonData["data"];
			JsonData jsonData3 = jsonData["signatures"];
			List<MFGooglePlayBillingTransactionV3> list = new List<MFGooglePlayBillingTransactionV3>(jsonData2.Count);
			for (int i = 0; i < jsonData2.Count; i++)
			{
				list.Add(new MFGooglePlayBillingTransactionV3((string)jsonData2[i], (string)jsonData3[i]));
			}
			MFGooglePlayBillingManagerV3.QueryPurchasesFinishedEvent(arg, list);
		}
		catch
		{
			if (MFGooglePlayBillingManagerV3.QueryPurchasesFinishedEvent != null)
			{
				MFGooglePlayBillingManagerV3.QueryPurchasesFinishedEvent(6, null);
			}
		}
	}
}
