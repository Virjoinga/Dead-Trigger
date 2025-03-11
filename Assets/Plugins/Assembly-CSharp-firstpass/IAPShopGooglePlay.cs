using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IAPShopGooglePlay : MonoBehaviour, IAP.Shop
{
	public class GooglePlayTransaction : IAP.Transaction
	{
		public MFGooglePlayBillingTransactionV3 Native { get; set; }
	}

	[method: MethodImpl(32)]
	public event Action<IAP.Transaction, Action<IAP.E_ProcessingState>> AsyncTransactionEvent;

	private void Awake()
	{
		base.gameObject.AddComponent<MFGooglePlayBillingManagerV3>();
	}

	public Coroutine Init(IAP.Product[] products, Action<IAP.E_Init, IAP.Product[]> callback)
	{
		return StartCoroutine(InitCoroutine(products, callback));
	}

	public Coroutine Buy(IAP.Product product, Action<IAP.E_Buy, IAP.Transaction> callback)
	{
		return StartCoroutine(BuyCoroutine(product, callback));
	}

	public Coroutine Restore(Action<bool> callback)
	{
		callback(true);
		return null;
	}

	public void Confirm(IAP.Transaction transaction)
	{
		StartCoroutine(ConfirmCoroutine(transaction));
	}

	private IEnumerator InitCoroutine(IAP.Product[] products, Action<IAP.E_Init, IAP.Product[]> callback)
	{
		bool setupFinished = false;
		bool queryProductsDetailsFinished = false;
		bool queryPurchasesFinished = false;
		bool internalError = false;
		bool isBillingSupported = false;
		List<IAP.Product> availableProducts = new List<IAP.Product>();
		IAP.Transaction[] pendingTransactions = null;
		IAP.E_Init initResult = IAP.E_Init.Retry;
		string[] productIds = products.Select((IAP.Product p) => p.Id).ToArray();
		Action<int, bool> onSetupFinished = delegate(int result, bool billingSupported)
		{
			isBillingSupported = billingSupported;
			internalError = result != 0;
			setupFinished = true;
		};
		IAP.Product[] products2 = default(IAP.Product[]);
		Action<int, List<MFGooglePlayBillingProductV3>> onQueryProductsDetailsFinished = delegate(int result, List<MFGooglePlayBillingProductV3> details)
		{
			if (result == 0)
			{
				for (int k = 0; k < details.Count; k++)
				{
					IAP.Product product = products2.FirstOrDefault((IAP.Product p) => p.Id.Equals(details[k].productId));
					if (product != null)
					{
						availableProducts.Add(SetupProductDetails(product, details[k]));
					}
				}
			}
			internalError = result != 0;
			queryProductsDetailsFinished = true;
		};
		Action<int, List<MFGooglePlayBillingTransactionV3>> onQueryPurchasesFinished = delegate(int result, List<MFGooglePlayBillingTransactionV3> transactions)
		{
			if (result == 0 && transactions != null)
			{
				pendingTransactions = new IAP.Transaction[transactions.Count];
				for (int j = 0; j < pendingTransactions.Length; j++)
				{
					pendingTransactions[j] = ConvertTransaction(transactions[j]);
				}
			}
			internalError = result != 0;
			queryPurchasesFinished = true;
		};
		MFGooglePlayBillingManagerV3.SetupFinishedEvent += onSetupFinished;
		MFGooglePlayBillingManagerV3.QueryProductsDetailsFinishedEvent += onQueryProductsDetailsFinished;
		MFGooglePlayBillingManagerV3.QueryPurchasesFinishedEvent += onQueryPurchasesFinished;
		MFGooglePlayBillingV3.Start();
		while (!setupFinished)
		{
			yield return new WaitForEndOfFrame();
		}
		if (!isBillingSupported)
		{
			initResult = IAP.E_Init.Failure;
		}
		else
		{
			MFGooglePlayBillingV3.QueryProductsDetails(productIds);
			while (!queryProductsDetailsFinished)
			{
				yield return new WaitForEndOfFrame();
			}
			if (!internalError)
			{
				MFGooglePlayBillingV3.QueryPurchases();
				while (!queryPurchasesFinished)
				{
					yield return new WaitForEndOfFrame();
				}
				if (!internalError)
				{
					initResult = IAP.E_Init.Success;
				}
			}
		}
		MFGooglePlayBillingManagerV3.SetupFinishedEvent -= onSetupFinished;
		MFGooglePlayBillingManagerV3.QueryProductsDetailsFinishedEvent -= onQueryProductsDetailsFinished;
		MFGooglePlayBillingManagerV3.QueryPurchasesFinishedEvent -= onQueryPurchasesFinished;
		int transactionCount = 0;
		if (pendingTransactions != null)
		{
			for (int i = 0; i < pendingTransactions.Length; i++)
			{
				if (this.AsyncTransactionEvent == null)
				{
					break;
				}
				transactionCount++;
				this.AsyncTransactionEvent(pendingTransactions[i], delegate
				{
					transactionCount--;
				});
			}
		}
		while (transactionCount != 0)
		{
			yield return new WaitForEndOfFrame();
		}
		callback(initResult, availableProducts.ToArray());
	}

	private IEnumerator BuyCoroutine(IAP.Product product, Action<IAP.E_Buy, IAP.Transaction> callback)
	{
		GooglePlayTransaction transaction = null;
		bool purchaseFinished = false;
		int purchaseResult = -1;
		string developerPayload = Guid.NewGuid().ToString();
		Action<int, MFGooglePlayBillingTransactionV3> onPurchaseFinished = delegate(int result, MFGooglePlayBillingTransactionV3 nativeTransaction)
		{
			transaction = ((nativeTransaction == null) ? null : ConvertTransaction(nativeTransaction));
			if (transaction == null || developerPayload.Equals(transaction.Native.DeveloperPayload))
			{
				purchaseResult = result;
				purchaseFinished = true;
			}
		};
		MFGooglePlayBillingManagerV3.PurchaseFinishedEvent += onPurchaseFinished;
		MFGooglePlayBillingV3.RequestPurchase(product.Id, developerPayload);
		while (!purchaseFinished)
		{
			yield return new WaitForEndOfFrame();
		}
		IAP.E_Buy status = IAP.E_Buy.Failure;
		switch (purchaseResult)
		{
		case 5:
		case 6:
			status = IAP.E_Buy.Fatal;
			break;
		case 1:
			status = IAP.E_Buy.UserCancelled;
			break;
		case 0:
			status = IAP.E_Buy.Success;
			break;
		}
		callback(status, transaction);
		MFGooglePlayBillingManagerV3.PurchaseFinishedEvent -= onPurchaseFinished;
	}

	private IEnumerator ConfirmCoroutine(IAP.Transaction transaction)
	{
		if (transaction.Product == null || transaction.Product.Type != 0)
		{
			yield break;
		}
		bool consumeFinished = false;
		int numRetries = 3;
		Action<int, MFGooglePlayBillingTransactionV3> onConsumeFinished = delegate(int result, MFGooglePlayBillingTransactionV3 nativeTransaction)
		{
			if (result == 6 && numRetries > 0)
			{
				numRetries--;
				MFGooglePlayBillingV3.ConsumePurchase(nativeTransaction);
			}
			else
			{
				consumeFinished = true;
			}
		};
		MFGooglePlayBillingManagerV3.ConsumeFinishedEvent += onConsumeFinished;
		MFGooglePlayBillingV3.ConsumePurchase((transaction as GooglePlayTransaction).Native);
		while (!consumeFinished)
		{
			yield return new WaitForEndOfFrame();
		}
		MFGooglePlayBillingManagerV3.ConsumeFinishedEvent -= onConsumeFinished;
	}

	private IAP.Product SetupProductDetails(IAP.Product product, MFGooglePlayBillingProductV3 details)
	{
		product.Price = (float)details.price_amount_micros / 1000000f;
		product.Title = details.title;
		product.Description = details.description;
		product.FormattedPrice = details.price;
		product.CurrencyCode = details.price_currency_code;
		return product;
	}

	private GooglePlayTransaction ConvertTransaction(MFGooglePlayBillingTransactionV3 nativeTransaction)
	{
		GooglePlayTransaction googlePlayTransaction = new GooglePlayTransaction();
		googlePlayTransaction.Id = nativeTransaction.OrderId;
		googlePlayTransaction.ProductId = nativeTransaction.ProductId;
		googlePlayTransaction.Native = nativeTransaction;
		return googlePlayTransaction;
	}
}
