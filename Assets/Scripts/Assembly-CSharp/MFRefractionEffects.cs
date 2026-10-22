using UnityEngine;

[RequireComponent(typeof(WaterDroplets))]
[AddComponentMenu("Image Effects/MADFINGER Refraction Effects")]
public class MFRefractionEffects : ImageEffectBase
{
    public Material m_WaterScreenRefractionMat;
    public float m_WaterDropSizeMin = 1f;
    public float m_WaterDropSizeMax = 2f;
    public static MFRefractionEffects Instance;

    private WaterDroplets m_WaterDroplets;
    private Material m_RuntimeMaterial;
    private Color m_DropletsColor = Color.clear;
    public WaterDroplets Droplets { get { EnsureInitialized(); return m_WaterDroplets; } }

    private void Awake() { Instance = this; EnsureInitialized(); }

    // ImageEffectBase.Start disables effects without a shader. Resolve the
    // existing material/resource first, including scenes where this starts off.
    protected new void Start()
    {
        EnsureInitialized();
        if (!SystemInfo.supportsImageEffects) enabled = false;
    }

    public bool EnsureInitialized()
    {
        if (m_WaterDroplets == null)
        {
            m_WaterDroplets = GetComponent<WaterDroplets>();
            if (m_WaterDroplets == null) m_WaterDroplets = gameObject.AddComponent<WaterDroplets>();
        }
        if (m_RuntimeMaterial != null) return true;
        Shader effectShader = shader;
        if (effectShader == null && m_WaterScreenRefractionMat != null) effectShader = m_WaterScreenRefractionMat.shader;
        if (effectShader == null) effectShader = Resources.Load<Shader>("shaders/MADFINGER-water-distort");
        if (effectShader == null || !effectShader.isSupported) return false;
        shader = effectShader;
        m_RuntimeMaterial = m_WaterScreenRefractionMat != null ? new Material(m_WaterScreenRefractionMat) : new Material(effectShader);
        m_RuntimeMaterial.shader = effectShader;
        m_RuntimeMaterial.hideFlags = HideFlags.HideAndDontSave;
        m_RuntimeMaterial.SetColor("_Color", m_DropletsColor);
        return true;
    }

    public void SetDropletsColor(Color color)
    {
        m_DropletsColor = color;
        if (m_RuntimeMaterial != null) m_RuntimeMaterial.SetColor("_Color", color);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!EnsureInitialized() || m_WaterDroplets.NumActiveDroplets() == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }
        m_RuntimeMaterial.SetTexture("_DropletMap", m_WaterDroplets.GetDropletTexture());
        Graphics.Blit(source, destination, m_RuntimeMaterial, 0);
    }

    public void AddWaterDropsToScreen(int number)
    {
        if (!EnsureInitialized()) return;
        float minimum = Mathf.Max(0.25f, m_WaterDropSizeMin);
        float maximum = Mathf.Max(minimum, m_WaterDropSizeMax);
        for (int i = 0; i < Mathf.Clamp(number, 0, 96); i++)
            m_WaterDroplets.AddDroplet(new Vector2(Random.value, Random.value), Random.Range(minimum, maximum));
        if (m_WaterDroplets.NumActiveDroplets() > 0)
        {
            enabled = true;
            PostFXTracking.RefractionFXActive = true;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) { Instance = null; PostFXTracking.RefractionFXActive = false; }
        if (m_RuntimeMaterial != null)
        {
            if (Application.isPlaying) Destroy(m_RuntimeMaterial); else DestroyImmediate(m_RuntimeMaterial);
        }
    }
}
