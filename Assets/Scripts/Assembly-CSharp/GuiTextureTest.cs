using UnityEngine;

public class GuiTextureTest : MonoBehaviour
{
	private GameObject ScreenObject;

	private GameObject Obj;

	private GUIBase_Widget Widget;

	public void SetGameObject(GameObject obj)
	{
		Obj = obj;
	}

	private void Start()
	{
		Widget = GetComponent<GUIBase_Widget>();
		ScreenObject = new GameObject("ScreenTexture");
		ScreenObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		GUITexture gUITexture = ScreenObject.AddComponent<GUITexture>();
		gUITexture.texture = Resources.Load("GameObjectiveKillAllZombies") as Texture;
	}

	private void LateUpdate()
	{
		if ((bool)Obj && (bool)Widget)
		{
		}
	}
}
