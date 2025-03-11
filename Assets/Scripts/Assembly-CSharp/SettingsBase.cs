using UnityEngine;

public class SettingsBase : MonoBehaviour
{
	public bool DISABLED = true;

	[OnlineShopItemSystemProperty]
	public int GUID;

	public virtual string GetSettingsClass()
	{
		return string.Empty;
	}

	public virtual string GetIdAsStr()
	{
		return string.Empty;
	}

	public int CalcGUIDFromID()
	{
		return Mathf.Abs(FNVHash.CalcModFNVHash(GetSettingsClass() + GetIdAsStr() + "ver2"));
	}
}
