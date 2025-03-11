using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Game))]
public class Notifier : MonoBehaviour
{
	private void Start()
	{
	}

	private IEnumerator RegisterForPushNotifications()
	{
		while (!CloudUser.instance.isUserAuthenticated)
		{
			Debug.Log("User NOT authenticated");
			yield return new WaitForSeconds(5f);
		}
		Debug.Log("User authenticated");
		MFNotificationService.RegisterPushNotifications();
	}

	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			ListReceivedNotifications();
			CancelScheduledNotifications();
		}
	}

	private void ListReceivedNotifications()
	{
		List<MFNotification> receivedNotifications = MFNotificationService.ReceivedNotifications;
		foreach (MFNotification item in receivedNotifications)
		{
			MFNotificationService.CancelNotification(item.Id);
		}
		MFNotificationService.ClearReceivedNotifications();
		MFNativeUtils.AppIconBadgeNumber = 0;
	}

	private void SheduleNotification(int id, string text, DateTime date)
	{
		MFNotificationService.Notify(id, new MFNotification("DEAD TRIGGER", text, "app_icon", string.Empty, 1), date);
	}

	private void CancelScheduledNotifications()
	{
		MFNotificationService.CancelAll();
	}
}
