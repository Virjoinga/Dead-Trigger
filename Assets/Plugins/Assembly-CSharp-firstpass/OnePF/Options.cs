using System.Collections.Generic;

namespace OnePF
{
	public class Options
	{
		public const int DISCOVER_TIMEOUT_MS = 5000;

		public const int INVENTORY_CHECK_TIMEOUT_MS = 10000;

		public int discoveryTimeoutMs = 5000;

		public bool checkInventory = true;

		public int checkInventoryTimeoutMs = 10000;

		public OptionsVerifyMode verifyMode;

		public SearchStrategy storeSearchStrategy;

		public Dictionary<string, string> storeKeys = new Dictionary<string, string>();

		public string[] prefferedStoreNames = new string[0];

		public string[] availableStoreNames = new string[0];

		public int samsungCertificationRequestCode;
	}
}
