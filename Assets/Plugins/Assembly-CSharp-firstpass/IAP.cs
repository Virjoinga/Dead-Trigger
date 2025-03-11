using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IAP
{
	public class Product : IComparable<Product>
	{
		public enum E_Type
		{
			Consumable = 0,
			Entitlement = 1
		}

		public string Id { get; set; }

		public float Price { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string FormattedPrice { get; set; }

		public string CurrencyCode { get; set; }

		public E_Type Type { get; set; }

		public override int GetHashCode()
		{
			return (Id != null) ? Id.GetHashCode() : 0;
		}

		public override bool Equals(object other)
		{
			return other != null && Id != null && other is Product && Id.Equals(((Product)other).Id);
		}

		public bool Equals(Product other)
		{
			return other != null && Id != null && Id.Equals(other.Id);
		}

		public int CompareTo(Product other)
		{
			return (other == null) ? 1 : string.Compare(Id, other.Id);
		}
	}

	public class Transaction
	{
		public string Id { get; set; }

		public string ProductId { get; set; }

		public Product Product
		{
			get
			{
				return m_AllProducts.FirstOrDefault((Product p) => p.Id.Equals(ProductId));
			}
		}
	}

	public interface Verifier
	{
		void Process(Transaction tr, Action<E_ProcessingState> callback);
	}

	public abstract class Extension : MonoBehaviour
	{
	}

	public enum E_Init
	{
		Success = 0,
		Failure = 1,
		Retry = 2
	}

	public enum E_Buy
	{
		Success = 0,
		Failure = 1,
		UserCancelled = 2,
		Fatal = 3
	}

	public enum E_ProcessingState
	{
		Accepted = 0,
		Rejected = 1,
		CantVerify = 2
	}

	public interface Shop
	{
		event Action<Transaction, Action<E_ProcessingState>> AsyncTransactionEvent;

		Coroutine Init(Product[] products, Action<E_Init, Product[]> callback);

		Coroutine Buy(Product product, Action<E_Buy, Transaction> callback);

		Coroutine Restore(Action<bool> callback);

		void Confirm(Transaction transaction);
	}

	public const string VERSION = "0.1.0";

	private static Product[] m_AvailableProducts = new Product[0];

	private static Product[] m_AllProducts = new Product[0];

	private static Verifier m_Verifier;

	private static Shop m_Shop;

	public static IEnumerable<Product> AvailableProducts
	{
		get
		{
			return m_AvailableProducts;
		}
	}

	public static bool IsReady { get; private set; }

	private static Shop CurrentShop
	{
		get
		{
			if (m_Shop == null)
			{
				GameObject gameObject = GameObject.Find("IAP");
				if (null == gameObject)
				{
					gameObject = new GameObject("IAP");
				}
				m_Shop = gameObject.AddComponent<IAPShopGooglePlay>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return m_Shop;
		}
	}

	public static Coroutine Init(Product[] products, Verifier verifier, Action<E_Init> callback)
	{
		return InitImpl(products, verifier, callback);
	}

	public static Coroutine Buy(Product product, Action<E_Buy> callback)
	{
		return BuyImpl(product, callback);
	}

	public static Coroutine Restore(Action<bool> callback)
	{
		return RestoreImpl(callback);
	}

	public static Extension RegisterExtension<T>() where T : Extension
	{
		return RegisterExtensionImpl<T>();
	}

	private static Coroutine InitImpl(Product[] products, Verifier verifier, Action<E_Init> callback)
	{
		bool flag = null != products.FirstOrDefault((Product p) => !m_AllProducts.Contains(p));
		if (IsReady && !flag)
		{
			callback(E_Init.Success);
			return null;
		}
		for (int i = 0; i < products.Length; i++)
		{
			if (string.IsNullOrEmpty(products[i].Id) || !Enum.IsDefined(typeof(Product.E_Type), products[i].Type))
			{
				callback(E_Init.Failure);
				return null;
			}
		}
		if (flag)
		{
			m_AllProducts = products.Except(m_AllProducts).Concat(m_AllProducts).ToArray();
		}
		Product[] requestedProducts = m_AllProducts.Intersect(products).ToArray();
		CurrentShop.AsyncTransactionEvent -= ProcessAsyncTransaction;
		CurrentShop.AsyncTransactionEvent += ProcessAsyncTransaction;
		m_Verifier = verifier;
		return CurrentShop.Init(requestedProducts, delegate(E_Init result, Product[] availableProducts)
		{
			if (result == E_Init.Success)
			{
				m_AvailableProducts = ((availableProducts == null) ? new Product[0] : availableProducts);
				Array.Sort(m_AvailableProducts);
				Array.Sort(requestedProducts);
				m_AllProducts = ((!requestedProducts.SequenceEqual(m_AvailableProducts)) ? requestedProducts : m_AvailableProducts);
				IsReady = true;
			}
			callback(result);
		});
	}

	private static Coroutine BuyImpl(Product product, Action<E_Buy> callback)
	{
		if (IsReady && AvailableProducts.Contains(product))
		{
			return CurrentShop.Buy(product, delegate(E_Buy result, Transaction transaction)
			{
				switch (result)
				{
				case E_Buy.Success:
					m_Verifier.Process(transaction, delegate(E_ProcessingState state)
					{
						callback((state != 0) ? E_Buy.Failure : E_Buy.Success);
						if (state != E_ProcessingState.CantVerify)
						{
							CurrentShop.Confirm(transaction);
						}
					});
					return;
				case E_Buy.Fatal:
					IsReady = false;
					break;
				}
				callback(result);
			});
		}
		callback(E_Buy.Failure);
		return null;
	}

	private static Coroutine RestoreImpl(Action<bool> callback)
	{
		if (IsReady)
		{
			return CurrentShop.Restore(delegate(bool result)
			{
				callback(result);
			});
		}
		callback(false);
		return null;
	}

	private static void ProcessAsyncTransaction(Transaction transaction, Action<E_ProcessingState> callback)
	{
		if (m_Verifier != null)
		{
			m_Verifier.Process(transaction, delegate(E_ProcessingState state)
			{
				if (state != E_ProcessingState.CantVerify)
				{
					CurrentShop.Confirm(transaction);
				}
				if (callback != null)
				{
					callback(state);
				}
			});
		}
		else if (callback != null)
		{
			callback(E_ProcessingState.CantVerify);
		}
	}

	private static Extension RegisterExtensionImpl<T>() where T : Extension
	{
		if (CurrentShop is Component)
		{
			GameObject gameObject = ((Component)CurrentShop).gameObject;
			T component = gameObject.GetComponent<T>();
			return (!((UnityEngine.Object)null != (UnityEngine.Object)component)) ? gameObject.AddComponent<T>() : component;
		}
		return null;
	}
}
