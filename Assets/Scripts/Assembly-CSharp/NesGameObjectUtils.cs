using UnityEngine;

[AddComponentMenu("Game/NES/GameObjectUtils")]
public class NesGameObjectUtils : MonoBehaviour
{
	[NESAction]
	public void Enable(bool enable)
	{
		base.gameObject.SetActive(enable);
	}

	[NESAction]
	public void PlayAnim()
	{
		if ((bool)base.GetComponent<Animation>())
		{
			base.GetComponent<Animation>().Play();
		}
	}
}
