using UnityEngine;

[AddComponentMenu("Effects/Water droplets mgr")]
public class WaterDropletsMgr : MonoBehaviour
{
	//private WaterDroplets m_WaterDroplets;

	private void Start()
	{
		//m_WaterDroplets = base.gameObject.GetComponent<WaterDroplets>();
	}

	private void LateUpdate()
	{
		/*if (!m_WaterDroplets || !MFRefractionEffects.Instance)
		{
			return;
		}
		if (m_WaterDroplets.NumActiveDroplets() > 0)
		{
			if (!MFRefractionEffects.Instance.enabled)
			{
				MFRefractionEffects.Instance.enabled = true;
				PostFXTracking.RefractionFXActive = true;
			}
		}
		else if (MFRefractionEffects.Instance.enabled)
		{
			MFRefractionEffects.Instance.enabled = false;
			PostFXTracking.RefractionFXActive = false;
		}*/
	}
}
