using BlowFishCSMangled;

internal class SaveGameCryptUtl
{
	private static string PASSWORD = "B84EEBAF5D2F50A865802398095EBD2F";

	private static BlowFish ms_Encryptor = new BlowFish(PASSWORD);

	public static string Encrypt(string input)
	{
		return ms_Encryptor.Encrypt_CBC(input);
	}

	public static string Decrypt(string input)
	{
		return ms_Encryptor.Decrypt_CBC(input);
	}
}
