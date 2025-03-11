using UnityEngine;

[AddComponentMenu("Effects/Screen space light FX mgr")]
public class ScreenSpaceLightFXMgr : MonoBehaviour
{
	private void LateUpdate()
	{
		if ((bool)MFScreenSpaceVertexGridFX.Instance)
		{
			MFScreenSpaceVertexGridFX.Instance.InternalUpdate();
			if (MFScreenSpaceVertexGridFX.Instance.AnyEffectActive())
			{
				MFScreenSpaceVertexGridFX.Instance.enabled = true;
			}
			else
			{
				MFScreenSpaceVertexGridFX.Instance.enabled = false;
			}
		}
	}
}
