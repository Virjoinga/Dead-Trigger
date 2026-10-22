using System.Collections.Generic;
using UnityEngine;

// Compatibility implementation for the original game's native WaterDroplets API.
// The exported UnityEngine.dll contains the signatures, but not the native solver.
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Water droplets")]
public sealed class WaterDroplets : MonoBehaviour
{
    private struct Drop { public Vector2 position; public float radius, mass; }
    private readonly List<Drop> m_Drops = new List<Drop>();
    private const int Width = 64, Height = 40, MaxDrops = 96;
    private readonly float[] m_Heights = new float[Width * Height];
    private readonly Color32[] m_Pixels = new Color32[Width * Height];
    private Texture2D m_Texture;
    private float m_VelocityScale = 1f, m_EvaporationRate = 0.35f;
    private float m_Aspect;
    private bool m_Dirty = true;

    public void AddDroplet(Vector2 pos, float size)
    {
        if (!Finite(pos.x) || !Finite(pos.y) || !Finite(size) || size <= 0f) return;
        Add(pos, 0.035f * Mathf.Clamp(size, 0.25f, 4f), 1f);
    }

    public void AddMass(Vector2 pos, float radius, float mass)
    {
        if (!Finite(pos.x) || !Finite(pos.y) || !Finite(radius) || !Finite(mass) || radius <= 0f) return;
        if (mass > 0f) Add(pos, Mathf.Clamp(radius, 0.005f, 0.25f), Mathf.Min(mass, 2f));
        else if (mass < 0f)
        {
            for (int i = m_Drops.Count - 1; i >= 0; i--)
            {
                Drop drop = m_Drops[i];
                if ((drop.position - pos).sqrMagnitude > radius * radius) continue;
                drop.mass += mass;
                if (drop.mass <= 0f) m_Drops.RemoveAt(i); else m_Drops[i] = drop;
            }
            m_Dirty = true;
        }
    }

    private void Add(Vector2 position, float radius, float mass)
    {
        if (m_Drops.Count == MaxDrops) m_Drops.RemoveAt(0);
        m_Drops.Add(new Drop { position = new Vector2(Mathf.Clamp01(position.x), Mathf.Clamp01(position.y)), radius = radius, mass = mass });
        m_Dirty = true;
    }

    public void SetVelocityScale(float value) { if (Finite(value)) m_VelocityScale = Mathf.Max(0f, value); }
    public float GetVelocityScale() { return m_VelocityScale; }
    public void SetEvaporationRate(float value) { if (Finite(value)) m_EvaporationRate = Mathf.Max(0f, value); }
    public float GetEvaporationRate() { return m_EvaporationRate; }
    public int NumActiveDroplets() { return m_Drops.Count; }
    public void ClearDrops() { m_Drops.Clear(); m_Dirty = true; }

    private void Update() { AdvanceSimulation(Time.deltaTime); }

    public void AdvanceSimulation(float deltaTime)
    {
        if (!Finite(deltaTime) || deltaTime <= 0f || m_Drops.Count == 0) return;
        for (int i = m_Drops.Count - 1; i >= 0; i--)
        {
            Drop drop = m_Drops[i];
            drop.mass -= deltaTime * m_EvaporationRate;
            drop.position.y -= deltaTime * m_VelocityScale * 0.035f * Mathf.Max(0f, drop.mass);
            if (drop.mass <= 0.001f || drop.position.y < -drop.radius) m_Drops.RemoveAt(i); else m_Drops[i] = drop;
        }
        m_Dirty = true;
    }

    public Texture2D GetDropletTexture()
    {
        if (m_Texture == null)
        {
            m_Texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false, true);
            m_Texture.name = "Water droplets simulation";
            m_Texture.hideFlags = HideFlags.HideAndDontSave;
            m_Texture.wrapMode = TextureWrapMode.Clamp;
            m_Texture.filterMode = FilterMode.Bilinear;
            m_Dirty = true;
        }
        float aspect = Mathf.Max(0.1f, GetComponent<Camera>().aspect);
        if (!Mathf.Approximately(aspect, m_Aspect)) { m_Aspect = aspect; m_Dirty = true; }
        if (m_Dirty) RebuildTexture();
        return m_Texture;
    }

    private void RebuildTexture()
    {
        System.Array.Clear(m_Heights, 0, m_Heights.Length);
        foreach (Drop drop in m_Drops)
        {
            float radius = drop.radius * Mathf.Sqrt(Mathf.Clamp01(drop.mass));
            float xRadius = radius / m_Aspect;
            int minX = Mathf.Clamp(Mathf.FloorToInt((drop.position.x - xRadius) * (Width - 1)), 0, Width - 1);
            int maxX = Mathf.Clamp(Mathf.CeilToInt((drop.position.x + xRadius) * (Width - 1)), 0, Width - 1);
            int minY = Mathf.Clamp(Mathf.FloorToInt((drop.position.y - radius) * (Height - 1)), 0, Height - 1);
            int maxY = Mathf.Clamp(Mathf.CeilToInt((drop.position.y + radius) * (Height - 1)), 0, Height - 1);
            for (int y = minY; y <= maxY; y++) for (int x = minX; x <= maxX; x++)
            {
                float dx = (x / (float)(Width - 1) - drop.position.x) / xRadius;
                float dy = (y / (float)(Height - 1) - drop.position.y) / radius;
                float shape = Mathf.Max(0f, 1f - dx * dx - dy * dy);
                m_Heights[y * Width + x] = Mathf.Min(1f, m_Heights[y * Width + x] + shape * shape * drop.mass);
            }
        }
        for (int y = 0; y < Height; y++) for (int x = 0; x < Width; x++)
        {
            int index = y * Width + x;
            float dx = (m_Heights[y * Width + Mathf.Min(x + 1, Width - 1)] - m_Heights[y * Width + Mathf.Max(x - 1, 0)]) * 0.125f;
            float dy = (m_Heights[Mathf.Min(y + 1, Height - 1) * Width + x] - m_Heights[Mathf.Max(y - 1, 0) * Width + x]) * 0.125f;
            m_Pixels[index] = new Color(dx * 0.5f + 0.5f, dy * 0.5f + 0.5f, m_Heights[index], m_Heights[index]);
        }
        m_Texture.SetPixels32(m_Pixels);
        m_Texture.Apply(false, false);
        m_Dirty = false;
    }

    private static bool Finite(float value) { return !float.IsNaN(value) && !float.IsInfinity(value); }
    private void OnDestroy()
    {
        if (m_Texture != null)
        {
            if (Application.isPlaying) Destroy(m_Texture); else DestroyImmediate(m_Texture);
        }
    }
}
