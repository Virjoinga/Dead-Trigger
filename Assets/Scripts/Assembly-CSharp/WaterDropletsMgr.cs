using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Water droplets mgr")]
public class WaterDropletsMgr : MonoBehaviour
{
    private WaterDroplets m_WaterDroplets;
    private MFRefractionEffects m_Effect;

    private void Awake() { EnsureInitialized(); }

    public void EnsureInitialized()
    {
        if (m_Effect == null)
        {
            m_Effect = GetComponent<MFRefractionEffects>();
            if (m_Effect == null) m_Effect = gameObject.AddComponent<MFRefractionEffects>();
        }
        m_Effect.EnsureInitialized();
        m_WaterDroplets = m_Effect.Droplets;
    }

    private void LateUpdate()
    {
        EnsureInitialized();
        bool active = SystemInfo.supportsImageEffects && m_WaterDroplets.NumActiveDroplets() > 0;
        m_Effect.enabled = active;
        if (MFRefractionEffects.Instance == m_Effect) PostFXTracking.RefractionFXActive = active;
    }
}
