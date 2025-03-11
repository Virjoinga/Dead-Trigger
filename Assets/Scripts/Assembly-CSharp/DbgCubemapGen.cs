using UnityEngine;

[AddComponentMenu("Effects/DbgCubemapGen")]
public class DbgCubemapGen : MonoBehaviour
{
	public Cubemap m_CubeMapTex;

	private void Awake()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (!base.gameObject.GetComponent<Camera>().RenderToCubemap(m_CubeMapTex))
			{
				Debug.LogError("Error generating cubemap");
			}
			else
			{
				Debug.Log("Cubemap updated");
			}
		}
	}
}
