using System.Runtime.CompilerServices;
using UnityEngine;

public static class Etcetera
{
	public delegate void DialogFinishedHandler(string TextOnPressedButton);

	public delegate void DialogCanceledHandler();

	public delegate void PromptFinishedHandler(string FieldText, string Field2Text);

	public delegate void PromptCanceledHandler();

	private static GameObject m_ExtManagerObject;

	[method: MethodImpl(32)]
	public static event DialogFinishedHandler OnDialogFinished;

	[method: MethodImpl(32)]
	public static event DialogCanceledHandler OnDialogCanceled;

	[method: MethodImpl(32)]
	public static event PromptFinishedHandler OnPromptFinished;

	[method: MethodImpl(32)]
	public static event PromptCanceledHandler OnPromptCanceled;

	public static void Init()
	{
		m_ExtManagerObject = new GameObject("Etcetera");
		m_ExtManagerObject.AddComponent<EtceteraAndroidManager>();
		EtceteraAndroidManager.alertButtonClickedEvent += DialogFinished;
		EtceteraAndroidManager.alertCancelledEvent += DialogCanceled;
		EtceteraAndroidManager.promptFinishedWithTextEvent += PromptFinished;
		EtceteraAndroidManager.promptCancelledEvent += PromptCanceled;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += Prompt2Finished;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent += PromptCanceled;
	}

	public static void Done()
	{
		EtceteraAndroidManager.alertButtonClickedEvent -= DialogFinished;
		EtceteraAndroidManager.alertCancelledEvent -= DialogCanceled;
		EtceteraAndroidManager.promptFinishedWithTextEvent -= PromptFinished;
		EtceteraAndroidManager.promptCancelledEvent -= PromptCanceled;
		EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= Prompt2Finished;
		EtceteraAndroidManager.twoFieldPromptCancelledEvent -= PromptCanceled;
		Object.Destroy(m_ExtManagerObject);
		m_ExtManagerObject = null;
	}

	public static void ShowDialog(string Title, string Message, string TextOnButton)
	{
		EtceteraAndroid.showAlert(Title, Message, TextOnButton);
	}

	public static void ShowDialog(string Title, string Message, string TextOnButtonA, string TextOnButtonB)
	{
		EtceteraAndroid.showAlert(Title, Message, TextOnButtonA, TextOnButtonB);
	}

	public static void ShowPrompt(string Title, string Message, string FieldLabel, string FieldText)
	{
		string positiveButton = "Confirm";
		string negativeButton = "Cancel";
		EtceteraAndroid.showAlertPrompt(Title, Message, FieldLabel, FieldText, positiveButton, negativeButton);
	}

	public static void ShowPrompt(string Title, string Message, string FieldLabel, string FieldText, string Field2Label, string Field2Text)
	{
		string positiveButton = "Confirm";
		string negativeButton = "Cancel";
		EtceteraAndroid.showAlertPromptWithTwoFields(Title, Message, FieldLabel, FieldText, Field2Label, Field2Text, positiveButton, negativeButton);
	}

	public static void ShowActivityNotification(string Message)
	{
		EtceteraAndroid.showProgressDialog(string.Empty, Message);
	}

	public static void HideActivityNotification()
	{
		EtceteraAndroid.hideProgressDialog();
	}

	public static void ShowWeb(string URL)
	{
		EtceteraAndroid.showWebView(URL);
	}

	public static bool ShowEmailComposer(string Address, string Subject, string Message)
	{
		bool isHTML = false;
		EtceteraAndroid.showEmailComposer(Address, Subject, Message, isHTML);
		return true;
	}

	public static void AskForReview(string Title, string Message)
	{
		EtceteraAndroid.askForReviewNow(Title, Message);
	}

	public static void AskForReview(string Title, string Message, int LauchCount, int HoursBetween)
	{
		int hoursUntilFirstPrompt = 0;
		EtceteraAndroid.askForReview(LauchCount, hoursUntilFirstPrompt, HoursBetween, Title, Message);
	}

	private static void DialogFinished(string TextOnPressedButton)
	{
		if (Etcetera.OnDialogFinished != null)
		{
			Etcetera.OnDialogFinished(TextOnPressedButton);
		}
	}

	private static void DialogCanceled()
	{
		if (Etcetera.OnDialogCanceled != null)
		{
			Etcetera.OnDialogCanceled();
		}
	}

	private static void PromptFinished(string FieldText)
	{
		if (Etcetera.OnPromptFinished != null)
		{
			Etcetera.OnPromptFinished(FieldText, string.Empty);
		}
	}

	private static void Prompt2Finished(string FieldText, string Field2Text)
	{
		if (Etcetera.OnPromptFinished != null)
		{
			Etcetera.OnPromptFinished(FieldText, Field2Text);
		}
	}

	private static void PromptCanceled()
	{
		if (Etcetera.OnPromptCanceled != null)
		{
			Etcetera.OnPromptCanceled();
		}
	}
}
