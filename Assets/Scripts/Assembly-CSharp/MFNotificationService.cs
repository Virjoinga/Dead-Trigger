using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MFNotificationService : MonoBehaviour
{
	private static MFNotificationService ms_Instance;

	public static List<MFNotification> ReceivedNotifications
	{
		get
		{
			return Instance.ReceivedNotificationsInternal();
		}
	}

	protected static MFNotificationService Instance
	{
		get
		{
			if (ms_Instance == null)
			{
				GameObject gameObject = new GameObject(typeof(MFNotificationService).Name);
				ms_Instance = CreatePlatformInstance(gameObject);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return ms_Instance;
		}
	}

	public static void CancelNotification(int id)
	{
		Instance.CancelNotificationInternal(id);
	}

	public static void CancelAll()
	{
		Instance.CancelAllInternal();
	}

	public static void Notify(int id, MFNotification notification)
	{
		Instance.NotifyInternal(id, notification, DateTime.Now, TimeSpan.Zero);
	}

	public static void Notify(int id, MFNotification notification, DateTime when)
	{
		Instance.NotifyInternal(id, notification, when, TimeSpan.Zero);
	}

	public static void Notify(int id, MFNotification notification, DateTime when, TimeSpan period)
	{
		Instance.NotifyInternal(id, notification, when, period);
	}

	public static void ClearReceivedNotifications()
	{
		Instance.ClearReceivedNotificationsInternal();
	}

	public static void RegisterPushNotifications()
	{
		Instance.RegisterPushNotificationsInternal();
	}

	public static void UnregisterPushNotifications()
	{
		Instance.UnregisterPushNotificationsInternal();
	}

	protected abstract void NotifyInternal(int id, MFNotification notification, DateTime when, TimeSpan period);

	protected abstract void CancelNotificationInternal(int id);

	protected abstract void CancelAllInternal();

	protected abstract List<MFNotification> ReceivedNotificationsInternal();

	protected abstract void ClearReceivedNotificationsInternal();

	protected abstract void RegisterPushNotificationsInternal();

	protected abstract void UnregisterPushNotificationsInternal();

	private static MFNotificationService CreatePlatformInstance(GameObject go)
	{
		return go.AddComponent<MFNotificationServiceAndroid>();
	}
}
