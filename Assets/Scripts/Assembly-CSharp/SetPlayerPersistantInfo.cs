public class SetPlayerPersistantInfo : SetUserProductData
{
	public SetPlayerPersistantInfo(UnigueUserID inUserID, string inPPInfo, float inTimeOut = -1f)
		: base(inUserID, "_PlayerData", inPPInfo, inTimeOut)
	{
	}
}
