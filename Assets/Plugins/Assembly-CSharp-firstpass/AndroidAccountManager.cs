using System.Collections.Generic;
using UnityEngine;

public class AndroidAccountManager : MonoBehaviour
{
	public struct Account
	{
		public string Name;

		public string Type;
	}

	public class AccountsList : List<Account>
	{
	}

	/*public static AccountsList GetAccountsByType(string DesiredType = null)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.accounts.AccountManager"))
		{
			if (androidJavaClass != null)
			{
				using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					return _GetAccountsFromManager(_GetAccountManagerObject(androidJavaClass, unityPlayerClass), DesiredType);
				}
			}
		}
		return null;
	}*/

	private static AndroidJavaObject _GetAccountManagerObject(AndroidJavaClass AccountManagerClass, AndroidJavaClass UnityPlayerClass)
	{
		if (UnityPlayerClass != null && AccountManagerClass != null)
		{
			AndroidJavaObject @static = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
			return AccountManagerClass.CallStatic<AndroidJavaObject>("get", new object[1] { androidJavaObject });
		}
		return null;
	}

	private static AccountsList _GetAccountsFromManager(AndroidJavaObject AccountManagerObject, string DesiredType)
	{
		if (AccountManagerObject != null)
		{
			AndroidJavaObject[] array = null;
			array = ((DesiredType != null) ? AccountManagerObject.Call<AndroidJavaObject[]>("getAccountsByType", new object[1] { DesiredType }) : AccountManagerObject.Call<AndroidJavaObject[]>("getAccounts", new object[0]));
			return _GetAccountsFromJavaArray(array);
		}
		return null;
	}

	private static AccountsList _GetAccountsFromJavaArray(AndroidJavaObject[] Accounts)
	{
		if (Accounts != null)
		{
			AccountsList accountsList = new AccountsList();
			foreach (AndroidJavaObject androidJavaObject in Accounts)
			{
				Account item = default(Account);
				item.Name = androidJavaObject.Get<string>("name");
				item.Type = androidJavaObject.Get<string>("type");
				accountsList.Add(item);
			}
			return accountsList;
		}
		return null;
	}
}
