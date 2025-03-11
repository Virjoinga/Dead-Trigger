using UnityEngine;

public class ItemSettings : Settings<E_ItemID>
{
	public int MoneyCost;

	public int GoldCost;

	public E_ItemType ItemType;

	public E_ItemUse ItemUse;

	public E_ItemBehaviour ItemBehaviour;

	public AudioClip UseSound;

	public GameObject SpawnObject;

	public int Count;

	public int MaxCountInMisson;

	public bool InfiniteUse;

	public float Timer;

	public float RechargeModificator;

	public float PowerUpModifier = 1f;

	public float HealHP;

	public override string GetSettingsClass()
	{
		return "item";
	}
}
