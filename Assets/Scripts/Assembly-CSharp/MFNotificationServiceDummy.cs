using System;
using System.Collections.Generic;

public class MFNotificationServiceDummy : MFNotificationService
{
	protected override void NotifyInternal(int id, MFNotification notification, DateTime when, TimeSpan period)
	{
	}

	protected override void CancelNotificationInternal(int id)
	{
	}

	protected override void CancelAllInternal()
	{
	}

	protected override List<MFNotification> ReceivedNotificationsInternal()
	{
		return new List<MFNotification>();
	}

	protected override void ClearReceivedNotificationsInternal()
	{
	}

	protected override void RegisterPushNotificationsInternal()
	{
	}

	protected override void UnregisterPushNotificationsInternal()
	{
	}
}
