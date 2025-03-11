using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IAPShopEditor : MonoBehaviour, IAP.Shop
{
	public class EditorTransaction : IAP.Transaction
	{
		public string Request { get; set; }

		public string Signature { get; set; }
	}

	private IAPExtensionEditorConfig Config { get; set; }

	private IAPExtensionRequestPurchase RequestPurchase { get; set; }

	[method: MethodImpl(32)]
	public event Action<IAP.Transaction, Action<IAP.E_ProcessingState>> AsyncTransactionEvent;

	public Coroutine Init(IAP.Product[] products, Action<IAP.E_Init, IAP.Product[]> callback)
	{
		InitExtensions();
		return StartCoroutine(InitCoroutine(products, callback));
	}

	public Coroutine Buy(IAP.Product product, Action<IAP.E_Buy, IAP.Transaction> callback)
	{
		return StartCoroutine(BuyCoroutine(product, callback));
	}

	public Coroutine Restore(Action<bool> callback)
	{
		return StartCoroutine(RestoreCoroutine(callback));
	}

	public void Confirm(IAP.Transaction transaction)
	{
	}

	private void InitExtensions()
	{
		if (null == Config)
		{
			Config = base.gameObject.GetComponent<IAPExtensionEditorConfig>();
			if (null == Config)
			{
				Config = base.gameObject.AddComponent<IAPExtensionEditorConfig>();
			}
			RequestPurchase = base.gameObject.GetComponent<IAPExtensionRequestPurchase>();
		}
	}

	private IEnumerator InitCoroutine(IAP.Product[] products, Action<IAP.E_Init, IAP.Product[]> callback)
	{
		if (Config.InitDelay > 0f)
		{
			yield return new WaitForSeconds(Config.InitDelay);
		}
		if (Config.InitResult == IAP.E_Init.Success)
		{
			foreach (IAP.Product prod in products)
			{
				prod.Price = UnityEngine.Random.Range(Config.PriceRangeMin, Config.PriceRangeMax);
				prod.FormattedPrice = string.Format("{0:C2}", prod.Price);
			}
			IAP.Product[] filteredProducts = products.Where((IAP.Product p) => !Config.InvalidInitProductIds.Contains(p.Id)).ToArray();
			callback(IAP.E_Init.Success, filteredProducts);
		}
		else
		{
			callback(Config.InitResult, null);
		}
	}

	private IEnumerator BuyCoroutine(IAP.Product product, Action<IAP.E_Buy, IAP.Transaction> callback)
	{
		if (Config.BuyDelay > 0f)
		{
			yield return new WaitForSeconds(Config.BuyDelay);
		}
		IAP.E_Buy expectedBuyResult = Config.BuyResult(product.Id);
		if (expectedBuyResult == IAP.E_Buy.Success)
		{
			if (null == RequestPurchase)
			{
				callback(IAP.E_Buy.Success, new EditorTransaction
				{
					Id = Guid.NewGuid().ToString(),
					ProductId = product.Id
				});
				yield break;
			}
			IAP.Product product2 = default(IAP.Product);
			Action<IAP.E_Buy, IAP.Transaction> callback2 = default(Action<IAP.E_Buy, IAP.Transaction>);
			RequestPurchase.MakeRequest(product.Id, delegate(string requestId, string request, string signature)
			{
				IAP.Transaction transaction = null;
				if (requestId != null && request != null && signature != null)
				{
					transaction = new EditorTransaction
					{
						Id = requestId,
						ProductId = product2.Id,
						Request = request,
						Signature = signature
					};
				}
				callback2((transaction == null) ? IAP.E_Buy.Failure : IAP.E_Buy.Success, transaction);
			});
		}
		else
		{
			callback(expectedBuyResult, null);
		}
	}

	private IEnumerator RestoreCoroutine(Action<bool> callback)
	{
		if (Config.RestoreDelay > 0f)
		{
			yield return new WaitForSeconds(Config.RestoreDelay);
		}
		if (null != RequestPurchase)
		{
			if (Config.RestoreResult && Config.RestoredProductIds.Length > 0)
			{
				Debug.Log("IAP: IAPExtensionRequestPurchase doens't support restore purchase -> skipping!");
			}
		}
		else if (Config.RestoreResult && this.AsyncTransactionEvent != null)
		{
			for (int i = 0; i < Config.RestoredProductIds.Length; i++)
			{
				this.AsyncTransactionEvent(new IAP.Transaction
				{
					Id = Guid.NewGuid().ToString(),
					ProductId = Config.RestoredProductIds[i]
				}, null);
				yield return new WaitForEndOfFrame();
			}
		}
		callback(Config.RestoreResult);
	}
}
