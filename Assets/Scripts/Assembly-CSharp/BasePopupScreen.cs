public abstract class BasePopupScreen : BaseMenuScreen
{
	private PopupHandler m_ResultHandler;

	public abstract void SetCaption(string inCaption);

	public abstract void SetText(string inText);

	public void SetHandler(PopupHandler inHandler)
	{
		m_ResultHandler = inHandler;
	}

	protected void SendResult(E_PopupResultCode inResult)
	{
		if (m_ResultHandler != null)
		{
			m_ResultHandler(this, inResult);
		}
	}
}
