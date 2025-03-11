using System;
using System.Runtime.CompilerServices;
using OnePF;
using UnityEngine;

public class OpenIABEventManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action billingSupportedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> billingNotSupportedEvent;

	[method: MethodImpl(32)]
	public static event Action<Inventory> queryInventorySucceededEvent;

	[method: MethodImpl(32)]
	public static event Action<string> queryInventoryFailedEvent;

	[method: MethodImpl(32)]
	public static event Action<Purchase> purchaseSucceededEvent;

	[method: MethodImpl(32)]
	public static event Action<int, string> purchaseFailedEvent;

	[method: MethodImpl(32)]
	public static event Action<Purchase> consumePurchaseSucceededEvent;

	[method: MethodImpl(32)]
	public static event Action<string> consumePurchaseFailedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> transactionRestoredEvent;

	[method: MethodImpl(32)]
	public static event Action<string> restoreFailedEvent;

	[method: MethodImpl(32)]
	public static event Action restoreSucceededEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnMapSkuFailed(string exception)
	{
		Debug.LogError("SKU mapping failed: " + exception);
	}

	private void OnBillingSupported(string empty)
	{
		if (OpenIABEventManager.billingSupportedEvent != null)
		{
			OpenIABEventManager.billingSupportedEvent();
		}
	}

	private void OnBillingNotSupported(string error)
	{
		if (OpenIABEventManager.billingNotSupportedEvent != null)
		{
			OpenIABEventManager.billingNotSupportedEvent(error);
		}
	}

	private void OnQueryInventorySucceeded(string json)
	{
		if (OpenIABEventManager.queryInventorySucceededEvent != null)
		{
			Inventory obj = new Inventory(json);
			OpenIABEventManager.queryInventorySucceededEvent(obj);
		}
	}

	private void OnQueryInventoryFailed(string error)
	{
		if (OpenIABEventManager.queryInventoryFailedEvent != null)
		{
			OpenIABEventManager.queryInventoryFailedEvent(error);
		}
	}

	private void OnPurchaseSucceeded(string json)
	{
		if (OpenIABEventManager.purchaseSucceededEvent != null)
		{
			OpenIABEventManager.purchaseSucceededEvent(new Purchase(json));
		}
	}

	private void OnPurchaseFailed(string message)
	{
		int result = -1;
		string arg = "Unknown error";
		if (!string.IsNullOrEmpty(message))
		{
			string[] array = message.Split('|');
			if (array.Length >= 2)
			{
				int.TryParse(array[0], out result);
				arg = array[1];
			}
			else
			{
				arg = message;
			}
		}
		if (OpenIABEventManager.purchaseFailedEvent != null)
		{
			OpenIABEventManager.purchaseFailedEvent(result, arg);
		}
	}

	private void OnConsumePurchaseSucceeded(string json)
	{
		if (OpenIABEventManager.consumePurchaseSucceededEvent != null)
		{
			OpenIABEventManager.consumePurchaseSucceededEvent(new Purchase(json));
		}
	}

	private void OnConsumePurchaseFailed(string error)
	{
		if (OpenIABEventManager.consumePurchaseFailedEvent != null)
		{
			OpenIABEventManager.consumePurchaseFailedEvent(error);
		}
	}

	public void OnTransactionRestored(string sku)
	{
		if (OpenIABEventManager.transactionRestoredEvent != null)
		{
			OpenIABEventManager.transactionRestoredEvent(sku);
		}
	}

	public void OnRestoreTransactionFailed(string error)
	{
		if (OpenIABEventManager.restoreFailedEvent != null)
		{
			OpenIABEventManager.restoreFailedEvent(error);
		}
	}

	public void OnRestoreTransactionSucceeded(string message)
	{
		if (OpenIABEventManager.restoreSucceededEvent != null)
		{
			OpenIABEventManager.restoreSucceededEvent();
		}
	}
}
