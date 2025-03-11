using UnityEngine;

internal class GameSaveLoadUtl
{
	private const string PLAYER_DATA_KEY_ID = "DeadTriggerPlayerData";

	private const string GAME_DATA_KEY_ID = "DeadTriggerGameData";

	public static IDataFile OpenReadPlayerData()
	{
		string @string = PlayerPrefs.GetString("DeadTriggerPlayerData", string.Empty);
		if (@string.Length > 0)
		{
			return new DataFileJSON(SaveGameCryptUtl.Decrypt(@string));
		}
		return new DataFileJSON("{}");
	}

	public static IDataFile OpenReadGameData()
	{
		string @string = PlayerPrefs.GetString("DeadTriggerGameData", string.Empty);
		if (@string.Length > 0)
		{
			return new DataFileJSON(SaveGameCryptUtl.Decrypt(@string));
		}
		return new DataFileJSON("{}");
	}

	public static bool SavePlayerData(IDataFile dataFile)
	{
		string text = dataFile.ToString();
		if (text.Length > 0)
		{
			PlayerPrefs.SetString("DeadTriggerPlayerData", SaveGameCryptUtl.Encrypt(text));
		}
		else
		{
			PlayerPrefs.DeleteKey("DeadTriggerPlayerData");
		}
		PlayerPrefs.Save();
		return true;
	}

	public static bool SaveGameData(IDataFile dataFile)
	{
		string text = dataFile.ToString();
		if (text.Length > 0)
		{
			PlayerPrefs.SetString("DeadTriggerGameData", SaveGameCryptUtl.Encrypt(text));
		}
		else
		{
			PlayerPrefs.DeleteKey("DeadTriggerGameData");
		}
		PlayerPrefs.Save();
		return true;
	}

	public static void DeletePlayerData()
	{
		PlayerPrefs.DeleteKey("DeadTriggerPlayerData");
		PlayerPrefs.Save();
	}

	public static void DeleteGameData()
	{
		PlayerPrefs.DeleteKey("DeadTriggerGameData");
		PlayerPrefs.Save();
	}
}
