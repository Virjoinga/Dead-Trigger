using System.Linq;
using UnityEngine;

public class IAPExtensionEditorConfig : IAP.Extension
{
	[SerializeField]
	private float m_InitDelay;

	[SerializeField]
	private float m_BuyDelay;

	[SerializeField]
	private float m_RestoreDelay;

	[SerializeField]
	private float m_PriceRangeMin;

	[SerializeField]
	private float m_PriceRangeMax;

	[SerializeField]
	private string[] m_InvalidInitProductIds = new string[0];

	[SerializeField]
	private string[] m_RestoredProductIds = new string[0];

	[SerializeField]
	private bool m_RestoreResult = true;

	[SerializeField]
	private IAP.E_Init m_InitResult;

	[SerializeField]
	private IAP.E_Buy m_DefaultBuyResult;

	[SerializeField]
	private string[] m_BuySuccessProductIds = new string[0];

	[SerializeField]
	private string[] m_BuyFailureProductIds = new string[0];

	[SerializeField]
	private string[] m_BuyUserCancelledProductIds = new string[0];

	[SerializeField]
	private string[] m_BuyFatalProductIds = new string[0];

	public virtual float InitDelay
	{
		get
		{
			return m_InitDelay;
		}
	}

	public virtual float BuyDelay
	{
		get
		{
			return m_BuyDelay;
		}
	}

	public virtual float RestoreDelay
	{
		get
		{
			return m_RestoreDelay;
		}
	}

	public virtual float PriceRangeMin
	{
		get
		{
			return m_PriceRangeMin;
		}
	}

	public virtual float PriceRangeMax
	{
		get
		{
			return m_PriceRangeMax;
		}
	}

	public virtual string[] InvalidInitProductIds
	{
		get
		{
			return m_InvalidInitProductIds;
		}
	}

	public virtual string[] RestoredProductIds
	{
		get
		{
			return m_RestoredProductIds;
		}
	}

	public virtual bool RestoreResult
	{
		get
		{
			return m_RestoreResult;
		}
	}

	public virtual IAP.E_Init InitResult
	{
		get
		{
			return m_InitResult;
		}
	}

	public virtual IAP.E_Buy BuyResult(string productId)
	{
		if (m_BuySuccessProductIds.Contains(productId))
		{
			return IAP.E_Buy.Success;
		}
		if (m_BuyFailureProductIds.Contains(productId))
		{
			return IAP.E_Buy.Failure;
		}
		if (m_BuyUserCancelledProductIds.Contains(productId))
		{
			return IAP.E_Buy.UserCancelled;
		}
		if (m_BuyFatalProductIds.Contains(productId))
		{
			return IAP.E_Buy.Fatal;
		}
		return m_DefaultBuyResult;
	}
}
