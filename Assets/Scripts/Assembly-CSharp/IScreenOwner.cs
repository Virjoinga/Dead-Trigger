public interface IScreenOwner
{
	void DoCommand(string inCommand);

	void ShowScreen(string inScreenName, bool inClearStack = false);

	void ShowPopup(string inPopupName, string inCaption, string inText, PopupHandler inHandler = null);

	void Back();

	void Exit();

	bool IsThisScreenTop(BaseMenuScreen inScreen);
}
