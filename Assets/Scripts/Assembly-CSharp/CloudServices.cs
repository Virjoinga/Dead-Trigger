#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using UnityEngine;

[AddComponentMenu("Cloud services/CloudServices")]
public class CloudServices : MonoBehaviour
{
	public class AsyncOpResult
	{
		public delegate void AsyncOpResDelegate(AsyncOpResult res);

		public bool m_Res;

		public bool m_Finished;

		public string m_ResultDesc;

		public string m_DbgId;

		public object m_UserData;

		internal string m_Password;

		public List<AsyncOpResDelegate> m_Listeners = new List<AsyncOpResDelegate>();

		internal AsyncOpResult(string password, AsyncOpResDelegate listener)
		{
			m_Password = password;
			if (listener != null)
			{
				m_Listeners.Add(listener);
			}
		}

		public void Finished()
		{
			m_Finished = true;
			foreach (AsyncOpResDelegate listener in m_Listeners)
			{
				listener(this);
			}
		}
	}

	public class AsyncOpResultChain
	{
		public delegate void AsyncOpResChainDelegate(AsyncOpResultChain res);

		public List<AsyncOpResult> m_PendingOps = new List<AsyncOpResult>();

		public bool m_Finished;

		private AsyncOpResChainDelegate m_Listener;

		internal AsyncOpResultChain(AsyncOpResChainDelegate listener)
		{
			m_Listener = listener;
		}

		public void Finished()
		{
			m_Finished = true;
			if (m_Listener != null)
			{
				m_Listener(this);
			}
		}
	}

	public struct S_ModItemInfo
	{
		public int m_GUID;

		public string m_Key;

		public string m_Val;

		public S_ModItemInfo(int guid, string key, string val)
		{
			m_GUID = guid;
			m_Key = key;
			m_Val = val;
		}
	}

	private class ParamsFormatter
	{
		private StringBuilder sb;

		private JsonWriter writer;

		public ParamsFormatter()
		{
			sb = new StringBuilder();
			writer = new JsonWriter(sb);
			writer.WriteObjectStart();
		}

		public void AddField(string id, string val)
		{
			writer.WritePropertyName(id);
			writer.Write(val);
		}

		public override string ToString()
		{
			writer.WriteObjectEnd();
			return sb.ToString();
		}
	}

	public const string SERVICE_URL_BASE = "madfingerdatastore.appspot.com";

	public const string PROP_ID_GENERIC_SETTINGS = "_GenericSettings";

	public const string PROP_ID_DEFAULT_PLAYER_DATA = "_DefaultPlayerData";

	public const string PROP_ID_PLAYER_DATA = "_PlayerData";

	public const string PROP_ID_SHOP_ITEMS = "_ShopItems";

	public const string PROP_ID_FRIENDS = "_Friends";

	public const string PROP_ID_PROGRESS = "_Progress";

	public const string PROP_ID_EMAIL = "Email";

	public const string PROP_ID_I_WANT_NEWS = "IWantNews";

	public const string PROP_ID_NICK_NAME = "NickName";

	public const string SECTION_ID_PARAMS = "Params";

	public const string RESP_OK = "ok";

	public const string RESP_ALREADY_PROCESSED = "alreadyproc";

	public const string RESP_NOT_FOUND = "notfound";

	public const string RESP_INVALID_USERID = "invalidusr";

	public const string RESP_DB_ERROR = "dberror";

	public const string RESP_INVALID_PARAMS = "invalidparams";

	public const string RESP_INVALID_REQUEST = "invalidreq";

	public const string RESP_GENERAL_ERROR = "error";

	public const string RESPONSE_PROP_ID_MESSAGES = "messages";

	public const string RESPONSE_PROP_ID_LAST_MSG_IDX = "lastMsgIdx";

	public const string RESPONSE_PROP_ID_SERVER_CMD = "_respCmd";

	private const string PARAM_ID_USERID = "userid";

	private const string PARAM_ID_CMD = "cmd";

	private const string PARAM_ID_DATA = "data";

	private const string PARAM_ID_SIGNATURE = "sig";

	private const string PARAM_ID_PRODUCT_ID = "prodId";

	private const string PARAM_ID_APP_VERSION = "appVer";

	private const string PARAM_ID_PARAM = "param";

	private const string PARAM_ID_PARAM2 = "param2";

	private const string PARAM_ID_PASSWORD = "pw";

	private const string PARAM_ID_GUID = "id";

	private const string PARAM_ID_KEY = "key";

	private const string PARAM_ID_VAL = "val";

	private const string PARAM_ID_EMAIL = "email";

	private const string CMD_ID_CREATE_PRODUCT = "createProduct";

	private const string CMD_ID_SET_PRODUCT_DATA = "setProductData";

	private const string CMD_ID_GET_PRODUCT_DATA = "getProductData";

	private const string CMD_ID_ADD_USR_PRODUCT_DATA = "addUsrProduct";

	private const string CMD_ID_VALIDATE_IOS_RECEIPT = "validateIOSReceipt";

	private const string CMD_ID_USER_SET_PER_PRODUCT_DATA = "setUsrPerProductData";

	private const string CMD_ID_USER_GET_PER_PRODUCT_DATA = "getUsrPerProductData";

	private const string CMD_ID_SET_USER_DATA = "setUsrData";

	private const string CMD_ID_GET_USER_DATA = "getUsrData";

	private const string CMD_ID_BUY_BUILTIN_ITEM = "buyBuiltInItem";

	private const string CMD_ID_EQUIP_ITEM = "equipItem";

	private const string CMD_ID_UNEQUIP_ITEM = "unEquipItem";

	private const string CMD_ID_MODIFY_ITEM = "modifyItem";

	private const string CMD_ID_SET_DATA_SECTION = "setDataSection";

	private const string CMD_ID_UPDATE_DATA_SECTION = "updateDataSection";

	private const string CMD_ID_CREATE_USER = "createUser";

	private const string CMD_ID_USER_EXISTS = "userExists";

	private const string CMD_ID_BUY_ITEM = "buyItem";

	private const string CMD_ID_TRANSACTION_PROCESSED = "transProcessed";

	private const string CMD_ID_VALIDATE_USER_ACCOUNT = "validateAccount";

	private const string CMD_ID_QUERY_FRIENDS_INFO = "getFriendsInfo";

	private const string CMD_ID_REQUEST_ADD_FRIEND = "reqAddFriend";

	private const string CMD_ID_PICKUP_INBOX_MESSAGES = "fetchInboxMsgs";

	private const string CMD_ID_INBOX_ADD_MSG = "inboxAddMsg";

	private const string CMD_ID_INBOX_REMOVE_MESSAGES = "inboxRemoveMsgs";

	private const string CMD_ID_REQUEST_RESET_PASSWORD = "reqResetPw";

	private const string CMD_ID_REQUEST_RESET_PASSWORD_WITH_EMAIL = "reqResetPwEmail";

	private const string CMD_ID_REGISTER_FOR_PUSH_NOTIFICATIONS = "registerForPushNotifications";

	private const string CMD_ID_GET_EIGC_DATA = "getEigcData";

	private const string CMD_ID_GET_EIGC_PASSWORD = "getEigcPassword";

	private const int NUM_DB_UPDATE_RETRIES = 5;

	private const float DB_UPDATE_RETRY_WAIT_MS = 500f;

	private const float PASSWORD_UPDATE_PERIOD_SEC = 30f;

	private const string RSA_PUBLIC_KEY_AS_XML = "<RSAKeyValue><Modulus>q+ABOYhyYYA5yrDDgpcdET9uvUOxrL/UelleOs+6kFjqu/7OzsYoeD8F+EcLL9PT/SGIteGxS87G8xvRAiWH4Q==</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>";

	private const bool IncommingTrafficEncrypted = false;

	private const string m_ServletURLCurrentGame = "http://madfingerdatastore.appspot.com/dt";

	private const int SYMMETRIC_ENC_PW_SIZE = 24;

	private const int SYMMETRIC_ENC_IV_SIZE = 8;

	public bool m_EnableEncryption = true;

	public bool m_EnableStoreKitSandbox = true;

	private RSACryptoServiceProvider m_RSA;

	private TripleDESCryptoServiceProvider m_SymmetricEnc;

	private ICryptoTransform m_SymmetricEncEncryptor;

	private RNGCryptoServiceProvider m_RndGen = new RNGCryptoServiceProvider();

	private byte[] m_SymmetricEncPassword = new byte[24];

	private byte[] m_SymmetricEncIV = new byte[8];

	private string m_SymmetricEncPasswordBase64;

	private string m_SymmetricEncIVBase64;

	private float m_LastPasswordChangeTime = -1f;

	private float m_DbgIntroducedFailureRate = -1f;

	private static CloudServices ms_Instance;

	private string AppVersion
	{
		get
		{
			if (BuildInfo.Instance != null && BuildInfo.Instance.Version != null)
			{
				return BuildInfo.Instance.Version.Version();
			}
			return string.Empty;
		}
	}

	private void OnDestroy()
	{
		ms_Instance = null;
	}

	public static CloudServices GetInstance()
	{
		if (ms_Instance == null)
		{
			GameObject gameObject = new GameObject("CloudServices");
			ms_Instance = gameObject.AddComponent<CloudServices>();
			ms_Instance.InitEncryption();
			UnityEngine.Object.DontDestroyOnLoad(ms_Instance);
		}
		return ms_Instance;
	}

	private void InitEncryption()
	{
		if ("<RSAKeyValue><Modulus>q+ABOYhyYYA5yrDDgpcdET9uvUOxrL/UelleOs+6kFjqu/7OzsYoeD8F+EcLL9PT/SGIteGxS87G8xvRAiWH4Q==</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>".Length > 0)
		{
			m_RSA = new RSACryptoServiceProvider();
			m_RSA.FromXmlString("<RSAKeyValue><Modulus>q+ABOYhyYYA5yrDDgpcdET9uvUOxrL/UelleOs+6kFjqu/7OzsYoeD8F+EcLL9PT/SGIteGxS87G8xvRAiWH4Q==</Modulus><Exponent>EQ==</Exponent></RSAKeyValue>");
		}
		RefreshSymmetricEncParams();
	}

	private void RefreshSymmetricEncParams()
	{
		DebugUtils.Assert(m_RndGen != null);
		m_RndGen.GetBytes(m_SymmetricEncPassword);
		m_RndGen.GetBytes(m_SymmetricEncIV);
		m_SymmetricEnc = new TripleDESCryptoServiceProvider();
		m_SymmetricEnc.Key = m_SymmetricEncPassword;
		m_SymmetricEnc.IV = m_SymmetricEncIV;
		m_SymmetricEnc.Mode = CipherMode.CBC;
		m_SymmetricEncEncryptor = m_SymmetricEnc.CreateEncryptor();
		DebugUtils.Assert(m_SymmetricEncEncryptor != null);
		m_SymmetricEncPasswordBase64 = Convert.ToBase64String(AsymmetricEncrypt(m_SymmetricEncPassword));
		m_SymmetricEncIVBase64 = Convert.ToBase64String(m_SymmetricEncIV);
	}

	public static string CalcPasswordHash(string password)
	{
		byte[] array = CalcSHA1Hash(password + "BC2DACFF812D6B71BAC1485FD0E0CCB5");
		return BitConverter.ToString(array).Replace("-", string.Empty);
	}

	private static byte[] CalcSHA1Hash(string str)
	{
		SHA1 sHA = new SHA1CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		return sHA.ComputeHash(bytes);
	}

	public AsyncOpResult CreateUser(string uniqueUserID, string productID, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(CreateUser(uniqueUserID, productID, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserNameExists(string userId, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(string.Empty, listener);
		StartCoroutine(UserExists(userId, string.Empty, string.Empty, result));
		return result;
	}

	public AsyncOpResult UserExists(string userId, string productId, string password, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(password, listener);
		StartCoroutine(UserExists(userId, productId, password, result));
		return result;
	}

	public AsyncOpResult ValidateUserAccount(string userId, string productId, string password, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(password, listener);
		StartCoroutine(ValidateUserAccount(userId, productId, password, result));
		return result;
	}

	public AsyncOpResult UserSetPerProductData(string userId, string productId, string key, string val, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(UserSetProductSpecificData(userId, productId, key, val, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserGetPerProductData(string userId, string productId, string key, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(UserGetProductSpecificData(userId, productId, key, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserSetPerProductDataSection(string userId, string productId, string key, string sectionId, string val, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(UserSetPerProductDataSection(userId, productId, key, sectionId, val, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserUpdatePerProductDataSection(string userId, string productId, string key, string sectionId, string val, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(UserUpdatePerProductDataSection(userId, productId, key, sectionId, val, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserSetData(string userId, string key, string val, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(SetUserData(userId, key, val, passwordHash, result));
		return result;
	}

	public AsyncOpResult UserGetData(string userId, string key, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(GetUserData(userId, key, passwordHash, result));
		return result;
	}

	public AsyncOpResult BuyItem(string userId, string productId, int itemID, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(BuyItem(userId, productId, itemID, passwordHash, result));
		return result;
	}

	public AsyncOpResult EquipItem(string userId, string productId, int itemID, int teamIdx, int slotIdx, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(EquipItem(userId, productId, itemID, teamIdx, slotIdx, passwordHash, result));
		return result;
	}

	public AsyncOpResult UnEquipItem(string userId, string productId, int itemID, int teamIdx, int slotIdx, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(UnEquipItem(userId, productId, itemID, teamIdx, slotIdx, passwordHash, result));
		return result;
	}

	public AsyncOpResult ModifyItem(string userId, string productId, int itemID, string key, string val, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		S_ModItemInfo[] items = new S_ModItemInfo[1]
		{
			new S_ModItemInfo(itemID, key, val)
		};
		StartCoroutine(ModifyItems(userId, productId, items, passwordHash, result));
		return result;
	}

	public AsyncOpResult ModifyItems(string userId, string productId, S_ModItemInfo[] items, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(ModifyItems(userId, productId, items, passwordHash, result));
		return result;
	}

	public AsyncOpResult CreateProduct(string productID, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(CreateProduct(productID, passwordHash, result));
		return result;
	}

	public AsyncOpResult ProductSetParam(string productID, string paramId, string paramVal, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(ProductSetParam(productID, paramId, paramVal, passwordHash, result));
		return result;
	}

	public AsyncOpResult ProductGetParam(string productID, string paramId, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(ProductGetParam(productID, paramId, passwordHash, result));
		return result;
	}

	public AsyncOpResult QueryFriendsInfo(string userId, string productId, string friendsListJSON, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(QueryFriendsInfo(userId, productId, friendsListJSON, passwordHash, result));
		return result;
	}

	public AsyncOpResult RequestAddFriend(string userId, string friendUserId, string infoMessage, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(RequestAddFriend(userId, friendUserId, infoMessage, passwordHash, result));
		return result;
	}

	public AsyncOpResult FetchInboxMessages(string userId, string productId, string passwordHash, int startMsgIdx = 0, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(FetchInboxMessages(startMsgIdx, userId, productId, passwordHash, result));
		return result;
	}

	public AsyncOpResult FetchProductInboxMessages(string productId, string passwordHash, int startMsgIdx = 0, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(FetchInboxMessages(startMsgIdx, string.Empty, productId, passwordHash, result));
		return result;
	}

	public AsyncOpResult InboxRemoveMessages(string userId, string productId, int lastMessageIdx, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(InboxRemoveMessages(userId, productId, lastMessageIdx, passwordHash, result));
		return result;
	}

	public AsyncOpResult ProductInboxRemoveMessages(string productId, int lastMessageIdx, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(InboxRemoveMessages(string.Empty, productId, lastMessageIdx, passwordHash, result));
		return result;
	}

	public AsyncOpResult InboxAddMsg(string userId, string targetUserId, string productId, string msg, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(InboxAddMsg(userId, targetUserId, productId, msg, passwordHash, result));
		return result;
	}

	public AsyncOpResult ProductInboxAddMsg(string productId, string msg, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(InboxAddMsg(string.Empty, string.Empty, productId, msg, passwordHash, result));
		return result;
	}

	public AsyncOpResult ProcessResponseCmd(string cmdData, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(string.Empty, listener);
		StartCoroutine(ProcessResponseCmd(cmdData, result));
		return result;
	}

	public AsyncOpResult RequestResetPassword(string userId, string msgBody, string msgSubject, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(string.Empty, listener);
		StartCoroutine(RequestResetPassword(userId, msgBody, msgSubject, result));
		return result;
	}

	public AsyncOpResult VerifyStoreKitReceipt(string transactionId, string receiptData, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(string.Empty, listener);
		StartCoroutine(VerifyStoreKitReceipt(transactionId, receiptData, result));
		return result;
	}

	public AsyncOpResult GetEigcData(string userid, string productID, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(GetEigcData(userid, productID, passwordHash, result));
		return result;
	}

	public AsyncOpResult GetEigcPassword(string userid, string productID, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult result = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(GetEigcPassword(userid, productID, passwordHash, result));
		return result;
	}

	public AsyncOpResult RegisterForPushNotifications(string userID, string productID, string provider, string registrationID, bool register, string passwordHash, AsyncOpResult.AsyncOpResDelegate listener = null)
	{
		AsyncOpResult asyncOpResult = new AsyncOpResult(passwordHash, listener);
		StartCoroutine(RegisterForPushNotifications(userID, productID, provider, registrationID, register, passwordHash, asyncOpResult));
		return asyncOpResult;
	}

	private IEnumerator CreateProduct(string productID, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "createProduct");
		httpData.AddField("prodId", productID);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "CreateProduct"));
	}

	private IEnumerator ProductSetParam(string productID, string paramId, string paramVal, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "setProductData");
		httpData.AddField("prodId", productID);
		httpData.AddField("param", paramId);
		httpData.AddField("data", paramVal);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "ProductSetParam"));
	}

	private IEnumerator ProductGetParam(string productID, string paramId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getProductData");
		httpData.AddField("prodId", productID);
		httpData.AddField("appVer", AppVersion);
		httpData.AddField("param", paramId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "ProductGetParam"), result));
	}

	private IEnumerator SetUserData(string userId, string fieldId, string data, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "setUsrData");
		httpData.AddField("data", data);
		httpData.AddField("param", fieldId);
		httpData.AddField("userid", userId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "SetUserData"));
	}

	private IEnumerator GetUserData(string userId, string fieldId, string password, AsyncOpResult result)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getUsrData");
		httpData.AddField("param", fieldId);
		httpData.AddField("userid", userId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "GetUserData"), result));
	}

	private IEnumerator UserExists(string userId, string productId, string password, AsyncOpResult result)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "userExists");
		httpData.AddField("userid", userId);
		if (password.Length > 0)
		{
			httpData.AddField("pw", password);
		}
		if (productId.Length > 0)
		{
			httpData.AddField("prodId", productId);
		}
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "UserExists"), result));
	}

	private IEnumerator CreateUser(string userId, string productId, string password, AsyncOpResult result, int numRetries = 5)
	{
		AsyncOpResult tmpResult = new AsyncOpResult(password, null)
		{
			m_DbgId = "UserExists"
		};
		yield return StartCoroutine(UserExists(userId, productId, password, tmpResult));
		if (tmpResult.m_Res && tmpResult.m_ResultDesc == "notfound")
		{
			tmpResult.m_DbgId = "CreateUser";
			yield return StartCoroutine(CreateUser(userId, password, tmpResult));
			if (tmpResult.m_Res && (tmpResult.m_ResultDesc == "ok" || tmpResult.m_ResultDesc == "alreadyproc"))
			{
				tmpResult.m_DbgId = "AddUserProductData";
				yield return StartCoroutine(UserAddProductData(userId, productId, password, tmpResult));
			}
		}
		else if (tmpResult.m_Res && tmpResult.m_ResultDesc == "ok")
		{
			tmpResult.m_Res = false;
		}
		result.m_Res = tmpResult.m_Res;
		result.m_ResultDesc = tmpResult.m_ResultDesc;
		result.m_DbgId = tmpResult.m_DbgId;
		result.m_Finished = tmpResult.m_Finished;
		result.Finished();
	}

	private IEnumerator CreateUser(string userId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		ASCIIEncoding enc = new ASCIIEncoding();
		long secretCode = SecretHash.Hash(enc.GetBytes(password));
		httpData.AddField("cmd", "createUser");
		httpData.AddField("userid", userId);
		httpData.AddField("pw", password);
		httpData.AddField("param", secretCode.ToString());
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "CreateUser"));
	}

	private IEnumerator ValidateUserAccount(string userId, string productId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "validateAccount");
		httpData.AddField("userid", userId);
		httpData.AddField("pw", password);
		httpData.AddField("prodId", productId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "ValidateUserAccount"));
	}

	private IEnumerator UserAddProductData(string userId, string productId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "addUsrProduct");
		httpData.AddField("userid", userId);
		httpData.AddField("prodId", productId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "UserAddProductData"));
	}

	private IEnumerator UserSetProductSpecificData(string userId, string productId, string key, string val, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "setUsrPerProductData");
		httpData.AddField("userid", userId);
		httpData.AddField("prodId", productId);
		httpData.AddField("param", key);
		httpData.AddField("data", val);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "UserSetProductSpecificData"));
	}

	private IEnumerator UserGetProductSpecificData(string userId, string productId, string paramId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getUsrPerProductData");
		httpData.AddField("userid", userId);
		httpData.AddField("prodId", productId);
		httpData.AddField("param", paramId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "UserGetProductSpecificData"), result));
	}

	private IEnumerator UserSetPerProductDataSection(string userId, string productId, string key, string sectionId, string data, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "setDataSection");
		httpData.AddField("prodId", productId);
		httpData.AddField("param", key);
		httpData.AddField("param2", sectionId);
		httpData.AddField("data", data);
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "UserSetPerProductDataSection"));
	}

	private IEnumerator UserUpdatePerProductDataSection(string userId, string productId, string key, string sectionId, string data, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "updateDataSection");
		httpData.AddField("prodId", productId);
		httpData.AddField("param", key);
		httpData.AddField("param2", sectionId);
		httpData.AddField("data", data);
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "UserUpdatePerProductDataSection"));
	}

	private IEnumerator BuyItem(string userId, string productId, int itemID, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "buyBuiltInItem");
		httpData.AddField("prodId", productId);
		httpData.AddField("data", itemID.ToString());
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "BuyItem"));
	}

	private IEnumerator EquipItem(string userId, string productId, int itemID, int teamIdx, int slotIdx, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "equipItem");
		httpData.AddField("prodId", productId);
		httpData.AddField("data", itemID.ToString());
		httpData.AddField("param", teamIdx.ToString());
		httpData.AddField("param2", slotIdx.ToString());
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "EquipItem"));
	}

	private IEnumerator UnEquipItem(string userId, string productId, int itemID, int teamIdx, int slotIdx, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "unEquipItem");
		httpData.AddField("prodId", productId);
		httpData.AddField("data", itemID.ToString());
		httpData.AddField("param", teamIdx.ToString());
		httpData.AddField("param2", slotIdx.ToString());
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "UnEquipItem"));
	}

	private IEnumerator ModifyItem(string userId, string productId, int itemID, string key, string val, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "modifyItem");
		httpData.AddField("prodId", productId);
		httpData.AddField("param", key);
		httpData.AddField("param2", itemID.ToString());
		httpData.AddField("data", val);
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "ModifyItem"));
	}

	private IEnumerator QueryFriendsInfo(string userId, string productId, string friendsListJSON, string passwordHash, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getFriendsInfo");
		httpData.AddField("userid", userId);
		httpData.AddField("prodId", productId);
		httpData.AddField("param", friendsListJSON);
		httpData.AddField("pw", passwordHash);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "QueryFriendsInfo"), result));
	}

	private IEnumerator RequestAddFriend(string userId, string friendUserId, string infoMessage, string passwordHash, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "reqAddFriend");
		httpData.AddField("param", friendUserId);
		httpData.AddField("param2", infoMessage);
		httpData.AddField("pw", passwordHash);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "RequestAddFriend"));
	}

	private IEnumerator ModifyItems(string userId, string productId, S_ModItemInfo[] items, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		StringBuilder sb = new StringBuilder();
		JsonWriter writer = new JsonWriter(sb);
		writer.WriteArrayStart();
		for (int i = 0; i < items.Length; i++)
		{
			S_ModItemInfo curr = items[i];
			writer.WriteObjectStart();
			writer.WritePropertyName("id");
			writer.Write(curr.m_GUID.ToString());
			writer.WritePropertyName("key");
			writer.Write(curr.m_Key);
			writer.WritePropertyName("val");
			writer.Write(curr.m_Val);
			writer.WriteObjectEnd();
		}
		writer.WriteArrayEnd();
		httpData.AddField("cmd", "modifyItem");
		httpData.AddField("prodId", productId);
		httpData.AddField("data", sb.ToString());
		httpData.AddField("pw", password);
		httpData.AddField("userid", userId);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "ModifyItems"));
	}

	private IEnumerator GetEigcData(string UserId, string productId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getEigcData");
		httpData.AddField("userid", UserId);
		httpData.AddField("prodId", productId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "GetEigcData"), result));
	}

	private IEnumerator GetEigcPassword(string UserId, string productId, string password, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "getEigcPassword");
		httpData.AddField("userid", UserId);
		httpData.AddField("prodId", productId);
		httpData.AddField("pw", password);
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "GetEigcPassword"), result));
	}

	private IEnumerator InboxAddMsg(string userId, string targetUserId, string productId, string msg, string passwordHash, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "inboxAddMsg");
		httpData.AddField("pw", passwordHash);
		httpData.AddField("param2", msg);
		if (userId != null && userId.Length > 0)
		{
			httpData.AddField("userid", userId);
		}
		if (targetUserId != null && targetUserId.Length > 0)
		{
			httpData.AddField("param", targetUserId);
		}
		if (productId != null && productId.Length > 0)
		{
			httpData.AddField("prodId", productId);
		}
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "InboxAddMsg"));
	}

	private IEnumerator FetchInboxMessages(int startMsgIdx, string userId, string productId, string passwordHash, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "fetchInboxMsgs");
		httpData.AddField("pw", passwordHash);
		httpData.AddField("param", startMsgIdx.ToString());
		if (userId != null && userId.Length > 0)
		{
			httpData.AddField("userid", userId);
		}
		if (productId != null && productId.Length > 0)
		{
			httpData.AddField("prodId", productId);
		}
		yield return StartCoroutine(ProcessGetWebRequest(BuildURLFromParams(httpData, "FetchInboxMessages"), result));
	}

	private IEnumerator InboxRemoveMessages(string userId, string productId, int lastMessageIdx, string passwordHash, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "inboxRemoveMsgs");
		httpData.AddField("pw", passwordHash);
		httpData.AddField("param", lastMessageIdx.ToString());
		if (userId != null && userId.Length > 0)
		{
			httpData.AddField("userid", userId);
		}
		if (productId != null && productId.Length > 0)
		{
			httpData.AddField("prodId", productId);
		}
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "InboxRemoveMessages"));
	}

	private IEnumerator RequestResetPassword(string userId, string message, string subjectMsg, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		string usrIdSHA = Convert.ToBase64String(CalcSHA1Hash(userId));
		long secretCode = SecretHash.Hash(Encoding.UTF8.GetBytes(usrIdSHA));
		httpData.AddField("cmd", "reqResetPw");
		httpData.AddField("userid", userId);
		httpData.AddField("param", secretCode.ToString());
		httpData.AddField("param2", message);
		httpData.AddField("data", subjectMsg);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "RequestResetPassword"));
	}

	private IEnumerator RequestResetPasswordWithEmail(string userId, string userEmail, string message, string subjectMsg, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		string usrIdSHA = Convert.ToBase64String(CalcSHA1Hash(userId));
		long secretCode = SecretHash.Hash(Encoding.UTF8.GetBytes(usrIdSHA));
		httpData.AddField("cmd", "reqResetPwEmail");
		httpData.AddField("userid", userId);
		httpData.AddField("param", secretCode.ToString());
		httpData.AddField("param2", message);
		httpData.AddField("data", subjectMsg);
		httpData.AddField("email", userEmail);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "RequestResetPasswordWithEmail"));
	}

	private IEnumerator VerifyStoreKitReceipt(string transactionId, string receiptData, AsyncOpResult result, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		string dataHash = Convert.ToBase64String(CalcSHA1Hash(receiptData));
		long secretCode = SecretHash.Hash(Encoding.UTF8.GetBytes(dataHash));
		httpData.AddField("cmd", "validateIOSReceipt");
		httpData.AddField("param", receiptData);
		httpData.AddField("param2", secretCode.ToString());
		httpData.AddField("txn", transactionId);
		if (m_EnableStoreKitSandbox)
		{
			httpData.AddField("data", "useSandbox");
		}
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), result, numRetries, "VerifyStoreKitReceipt"));
	}

	private IEnumerator RegisterForPushNotifications(string userID, string productID, string provider, string registrationID, bool register, string passwordHash, AsyncOpResult res, int numRetries = 5)
	{
		ParamsFormatter httpData = new ParamsFormatter();
		httpData.AddField("cmd", "registerForPushNotifications");
		httpData.AddField("userid", userID);
		httpData.AddField("prodId", productID);
		httpData.AddField("param", provider);
		httpData.AddField("param2", registrationID);
		httpData.AddField("data", (!register) ? "unregister" : "register");
		httpData.AddField("pw", passwordHash);
		yield return StartCoroutine(ProcessWebRequest(CreateWWWForm(httpData.ToString()), res, numRetries, "RegisterForPushNotifications"));
	}

	private IEnumerator ProcessResponseCmd(string cmdData, AsyncOpResult result, int numRetries = 5)
	{
		WWWForm form = CreateWWWForm(cmdData, true);
		form.AddField("ipw", string.Empty);
		yield return StartCoroutine(ProcessWebRequest(form, result, numRetries));
	}

	private string BuildURLFromParams(ParamsFormatter httpData, string cmdURL = null)
	{
		string text = "http://madfingerdatastore.appspot.com/dt";
		if (cmdURL != null)
		{
			text = text + "/" + cmdURL;
		}
		return text + "?param=" + EncodeURLParam(httpData.ToString());
	}

	private IEnumerator ProcessGetWebRequest(string url, AsyncOpResult result)
	{
		if (UnityEngine.Random.Range(0f, 1f) < m_DbgIntroducedFailureRate)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 1f));
			result.m_ResultDesc = "dbgfailure";
			result.m_Res = false;
			result.Finished();
			yield return 0;
		}
		else
		{
			WWW httpReq = new WWW(url);
			yield return httpReq;
			result.m_Res = httpReq.error == null;
			if (result.m_Res)
			{
				result.m_ResultDesc = httpReq.text;
			}
			else
			{
				result.m_ResultDesc = httpReq.error;
				Debug.LogWarning(httpReq.error);
			}
			result.Finished();
		}
		yield return 0;
	}

	private WWWForm CreateWWWForm(string data, bool forceDisableEncryption = false)
	{
		WWWForm wWWForm = new WWWForm();
		if (m_EnableEncryption && !forceDisableEncryption)
		{
			if (!m_SymmetricEncEncryptor.CanReuseTransform)
			{
				m_SymmetricEncEncryptor = m_SymmetricEnc.CreateEncryptor();
			}
			wWWForm.AddField("param", EncryptStr(data));
			wWWForm.AddField("_pw", m_SymmetricEncPasswordBase64);
			wWWForm.AddField("_iv", m_SymmetricEncIVBase64);
		}
		else
		{
			wWWForm.AddField("param", data);
		}
		return wWWForm;
	}

	private IEnumerator ProcessWebRequest(WWWForm httpData, AsyncOpResult result, int numRetries, string cmdURL = null)
	{
		float currRetryTimeout = 500f;
		if (UnityEngine.Random.Range(0f, 1f) < m_DbgIntroducedFailureRate)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.25f, 1f));
			result.m_ResultDesc = "dbgfailure";
			result.m_Res = false;
			result.Finished();
			yield return 0;
		}
		else
		{
			string servletURL = "http://madfingerdatastore.appspot.com/dt";
			if (cmdURL != null)
			{
				servletURL = servletURL + "/" + cmdURL;
			}
			for (int i = 0; i < numRetries; i++)
			{
				WWW httpPutReq = new WWW(servletURL, httpData);
				yield return httpPutReq;
				result.m_Res = httpPutReq.error == null;
				if (result.m_Res)
				{
					result.m_ResultDesc = httpPutReq.text;
				}
				else
				{
					Debug.LogWarning(httpPutReq.error);
					result.m_ResultDesc = httpPutReq.error;
				}
				if (result.m_ResultDesc != "dberror" && result.m_ResultDesc != "error")
				{
					break;
				}
				yield return new WaitForSeconds(currRetryTimeout / 1000f);
				currRetryTimeout *= 2f;
			}
			result.Finished();
		}
		yield return 0;
	}

	private byte[] AsymmetricEncrypt(byte[] input)
	{
		DebugUtils.Assert(m_RSA != null);
		return m_RSA.Encrypt(input, false);
	}

	private string EncodeURLParam(string str)
	{
		if (m_EnableEncryption)
		{
			if (!m_SymmetricEncEncryptor.CanReuseTransform)
			{
				m_SymmetricEncEncryptor = m_SymmetricEnc.CreateEncryptor();
			}
			return WWW.EscapeURL(EncryptStr(str)) + "&_pw=" + WWW.EscapeURL(m_SymmetricEncPasswordBase64) + "&_iv=" + WWW.EscapeURL(m_SymmetricEncIVBase64);
		}
		return WWW.EscapeURL(str);
	}

	private string EncryptStr(string str)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(str);
		return Convert.ToBase64String(m_SymmetricEncEncryptor.TransformFinalBlock(bytes, 0, bytes.Length));
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (m_LastPasswordChangeTime < 0f)
		{
			m_LastPasswordChangeTime = realtimeSinceStartup;
		}
		if (realtimeSinceStartup - m_LastPasswordChangeTime >= 30f)
		{
			RefreshSymmetricEncParams();
			m_LastPasswordChangeTime = realtimeSinceStartup;
		}
	}
}
