using UnityEngine;

public class GuiLoading : MonoBehaviour
{
	public int FirstHintTextIndex = 3070001;

	private int lastHintTextIndex;

	private int HACK_FrameCount = 3;

	private bool isInitialized;

	private MFGuiManager guiManager;

	private GUIBase_Widget widget;

	private float alpha;

	public void Start()
	{
		guiManager = MFGuiManager.Instance;
		TextDatabase instance = TextDatabase.instance;
		int i;
		for (i = FirstHintTextIndex; instance.Exists(i); i++)
		{
		}
		lastHintTextIndex = i - 1;
	}

	private void LateUpdate()
	{
		if (!isInitialized && --HACK_FrameCount < 0)
		{
			int newText = Random.Range(FirstHintTextIndex, lastHintTextIndex);
			GUIBase_Layout layout = guiManager.GetLayout("Hints");
			GUIBase_Platform component = layout.transform.parent.GetComponent<GUIBase_Platform>();
			GUIBase_Label gUIBase_Label = GuiBaseUtils.PrepareLabel(layout, "GUIBase_Label");
			if (SystemInfo.operatingSystem.Contains("iPhone") || (Application.platform == RuntimePlatform.Android && (float)Screen.width / (float)Screen.height <= 1.4f))
			{
				Vector2 vector = new Vector2(Screen.width, (float)component.m_Height * ((float)Screen.width / (float)component.m_Width));
				Vector3 position = layout.transform.position;
				position.y = (float)Screen.height * 0.5f - vector.y * 0.5f;
				layout.transform.position = position;
			}
			gUIBase_Label.SetNewText(newText);
			layout.ShowImmediate(true, false);
			widget = layout.GetWidget("GUIBase_Label");
			widget.m_FadeAlpha = 0f;
			isInitialized = true;
		}
		else if (isInitialized && alpha < 1f)
		{
			float num = 0.09f;
			alpha = Mathf.Min(alpha + num, 1f);
			widget.m_FadeAlpha = alpha;
		}
	}
}
