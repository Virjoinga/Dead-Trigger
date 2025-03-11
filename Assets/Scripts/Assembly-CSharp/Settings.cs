using UnityEngine;

public class Settings<Key> : SettingsBase
{
	public Key ID;

	public int Name;

	public int Description;

	public int Rank;

	[HideInInspector]
	public Transform ShopWidgetPrefab;

	public string ShopWidgetPrefabName;

	public Transform HudWidgetPrefab;

	public int SaleInPercent;

	public bool NewInShop;

	public GUIBase_Widget ShopWidget
	{
		get
		{
			if ((bool)ShopWidgetPrefab)
			{
				return ShopWidgetPrefab.GetComponent<GUIBase_Widget>();
			}
			return null;
		}
	}

	public GUIBase_Widget HudWidget
	{
		get
		{
			if ((bool)HudWidgetPrefab)
			{
				return HudWidgetPrefab.GetComponent<GUIBase_Widget>();
			}
			return null;
		}
	}

	public override string GetIdAsStr()
	{
		return ID.ToString();
	}
}
