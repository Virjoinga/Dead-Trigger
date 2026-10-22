using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("Triggers/Spawn water drops")]
public class TriggerWaterDrops : MonoBehaviour
{
    public float m_NumDropsPerSecond = 10f;
    private float m_NumDropsToGenerate;
    private float m_LastGenerateTime = -1f;
    private float m_OrigVelocityScale = 1f;
    private float m_OrigEvaporation = 0.35f;
    private WaterDroplets m_WaterDroplets;

    private void Start() { ResolveEffect(); }

    private MFRefractionEffects ResolveEffect()
    {
        MFRefractionEffects effect = MFRefractionEffects.Instance;
        if (effect == null && Camera.main != null)
        {
            WaterDropletsMgr manager = Camera.main.GetComponent<WaterDropletsMgr>();
            if (manager == null) manager = Camera.main.gameObject.AddComponent<WaterDropletsMgr>();
            manager.EnsureInitialized();
            effect = Camera.main.GetComponent<MFRefractionEffects>();
        }
        if (effect != null && m_WaterDroplets != effect.Droplets)
        {
            m_WaterDroplets = effect.Droplets;
            m_OrigVelocityScale = m_WaterDroplets.GetVelocityScale();
            m_OrigEvaporation = m_WaterDroplets.GetEvaporationRate();
        }
        return effect;
    }

    private static bool IsPlayer(Collider other)
    {
        return Player.Instance != null && Player.Instance.Owner != null &&
            other == Player.Instance.Owner.CharacterController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;
        MFRefractionEffects effect = ResolveEffect();
        if (effect != null)
        {
            m_WaterDroplets.SetVelocityScale(m_OrigVelocityScale);
            m_WaterDroplets.SetEvaporationRate(m_OrigEvaporation);
            effect.SetDropletsColor(Color.clear);
        }
        m_LastGenerateTime = Time.time;
        m_NumDropsToGenerate = 0f;
    }

    private void OnTriggerStay(Collider other) { if (IsPlayer(other)) GenerateDrops(); }
    private void OnTriggerExit(Collider other) { if (IsPlayer(other)) ResetEmission(); }
    private void OnDisable() { ResetEmission(); }
    private void ResetEmission() { m_LastGenerateTime = -1f; m_NumDropsToGenerate = 0f; }

    private void GenerateDrops()
    {
        if (m_LastGenerateTime < 0f) m_LastGenerateTime = Time.time;
        float elapsed = Mathf.Max(0f, Time.time - m_LastGenerateTime);
        m_LastGenerateTime = Time.time;
        m_NumDropsToGenerate += elapsed * Mathf.Max(0f, m_NumDropsPerSecond);
        int number = Mathf.FloorToInt(m_NumDropsToGenerate);
        m_NumDropsToGenerate -= number;
        MFRefractionEffects effect = ResolveEffect();
        if (effect != null && number > 0) effect.AddWaterDropsToScreen(number);
    }
}
