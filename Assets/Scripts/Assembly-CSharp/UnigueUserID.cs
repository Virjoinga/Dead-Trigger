public class UnigueUserID
{
	public string userName { get; private set; }

	public string passwordHash { get; private set; }

	public string productID { get; private set; }

	public UnigueUserID(string inUserName, string inPasswordHash, string inProductID)
	{
		userName = inUserName.ToLower();
		passwordHash = inPasswordHash;
		productID = inProductID;
	}
}
