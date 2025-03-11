public class SendCloudCommand : DefaultCloudAction
{
	public string command { get; private set; }

	public SendCloudCommand(UnigueUserID inUserID, string inCommand, float inTimeOut = -1f)
		: base(inUserID, inTimeOut)
	{
		command = inCommand;
	}

	protected override CloudServices.AsyncOpResult GetCloudAsyncOp()
	{
		return CloudServices.GetInstance().ProcessResponseCmd(command);
	}
}
