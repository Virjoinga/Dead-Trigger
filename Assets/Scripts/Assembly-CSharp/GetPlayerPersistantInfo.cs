public class GetPlayerPersistantInfo : GetUserProductData
{
	public GetPlayerPersistantInfo(UnigueUserID inUserID, float inTimeOut = -1f)
		: base(inUserID, "_PlayerData", inTimeOut)
	{
	}
}
