public class GuiMapDummyScreen : BaseMenuScreen
{
	protected override void OnGUI_Init()
	{
		base.isInitialized = true;
	}

	protected override void OnGUI_Show()
	{
	}

	protected override void OnGUI_Hide()
	{
	}

	protected override void OnGUI_Update()
	{
	}

	protected override void OnGUI_Enable()
	{
	}

	protected override void OnGUI_Disable()
	{
	}

	protected override void OnGUI_Destroy()
	{
		base.isInitialized = false;
	}
}
