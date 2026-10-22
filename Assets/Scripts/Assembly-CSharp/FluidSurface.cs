using System.Collections.Generic;
using UnityEngine;

// The original UnityEngine.FluidSurface exposed this API through native calls.
// This replacement simulates waves on the exported rectangular water grid.
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public sealed class FluidSurface : MonoBehaviour
{
    public float waveSpeed = 2f;
    public float damping = 2.5f;
    private MeshFilter m_Filter;
    private Mesh m_Source, m_Mesh;
    private Vector3[] m_BaseVertices, m_Vertices;
    private float[] m_Heights, m_Velocities, m_NextVelocities;
    private int[] m_VertexAt;
    private int m_Width, m_Depth;
    private float m_StepX, m_StepZ;
    private Bounds m_Bounds;
    private bool m_Moving;

    private void Awake() { EnsureInitialized(); }

    public bool EnsureInitialized()
    {
        if (m_Mesh != null) return true;
        m_Filter = GetComponent<MeshFilter>();
        if (m_Filter.sharedMesh == null) return false;
        m_Source = m_Filter.sharedMesh;
        m_BaseVertices = m_Source.vertices;
        if (m_BaseVertices.Length < 4) return false;
        List<float> xs = new List<float>(), zs = new List<float>();
        foreach (Vector3 vertex in m_BaseVertices)
        {
            if (!xs.Contains(vertex.x)) xs.Add(vertex.x);
            if (!zs.Contains(vertex.z)) zs.Add(vertex.z);
        }
        xs.Sort(); zs.Sort();
        m_Width = xs.Count; m_Depth = zs.Count;
        if (m_Width < 2 || m_Depth < 2 || m_Width * m_Depth != m_BaseVertices.Length) return false;
        m_StepX = (xs[m_Width - 1] - xs[0]) / (m_Width - 1);
        m_StepZ = (zs[m_Depth - 1] - zs[0]) / (m_Depth - 1);
        if (m_StepX <= 0 || m_StepZ <= 0) return false;
        m_VertexAt = new int[m_BaseVertices.Length];
        for (int i = 0; i < m_VertexAt.Length; i++) m_VertexAt[i] = -1;
        for (int i = 0; i < m_BaseVertices.Length; i++)
        {
            Vector3 vertex = m_BaseVertices[i];
            int x = Mathf.RoundToInt((vertex.x - xs[0]) / m_StepX);
            int z = Mathf.RoundToInt((vertex.z - zs[0]) / m_StepZ);
            if (x < 0 || x >= m_Width || z < 0 || z >= m_Depth ||
                Mathf.Abs(vertex.x - xs[0] - x * m_StepX) > 0.001f ||
                Mathf.Abs(vertex.z - zs[0] - z * m_StepZ) > 0.001f ||
                Mathf.Abs(vertex.y - m_BaseVertices[0].y) > 0.001f || m_VertexAt[z * m_Width + x] >= 0) return false;
            m_VertexAt[z * m_Width + x] = i;
        }
        m_Heights = new float[m_BaseVertices.Length];
        m_Velocities = new float[m_Heights.Length];
        m_NextVelocities = new float[m_Heights.Length];
        m_Vertices = (Vector3[])m_BaseVertices.Clone();
        m_Bounds = m_Source.bounds;
        m_Mesh = Instantiate(m_Source);
        m_Mesh.name = m_Source.name + " (fluid simulation)";
        m_Mesh.hideFlags = HideFlags.HideAndDontSave;
        m_Mesh.MarkDynamic();
        m_Filter.sharedMesh = m_Mesh;
        return true;
    }

    public void AddDropletAtWorldPos(Vector3 pos, float radius, float intensity)
    {
        if (!isActiveAndEnabled || !Finite(pos.x) || !Finite(pos.y) || !Finite(pos.z) ||
            !Finite(radius) || !Finite(intensity) || radius <= 0 || intensity == 0 || !EnsureInitialized()) return;
        Vector3 local = transform.InverseTransformPoint(pos);
        if (local.x < m_Bounds.min.x || local.x > m_Bounds.max.x ||
            local.z < m_Bounds.min.z || local.z > m_Bounds.max.z) return;
        float verticalScale = Mathf.Max(0.001f, transform.TransformVector(Vector3.up).magnitude);
        float worldStep = Mathf.Max(transform.TransformVector(Vector3.right * m_StepX).magnitude,
                                   transform.TransformVector(Vector3.forward * m_StepZ).magnitude);
        radius = Mathf.Max(radius, worldStep);
        float impulse = Mathf.Clamp(intensity, -0.5f, 0.5f) * 4f / verticalScale;
        for (int z = 1; z < m_Depth - 1; z++) for (int x = 1; x < m_Width - 1; x++)
        {
            int grid = z * m_Width + x;
            Vector3 offset = m_BaseVertices[m_VertexAt[grid]] - local; offset.y = 0;
            float distanceSq = transform.TransformVector(offset).sqrMagnitude;
            if (distanceSq >= radius * radius) continue;
            float shape = 1f - distanceSq / (radius * radius);
            m_Velocities[grid] = Mathf.Clamp(m_Velocities[grid] + impulse * shape * shape, -2f / verticalScale, 2f / verticalScale);
            m_Moving = true;
        }
    }

    private void Update() { AdvanceSimulation(Time.deltaTime); }

    public void AdvanceSimulation(float deltaTime)
    {
        if (!m_Moving || !Finite(deltaTime) || deltaTime <= 0 || !EnsureInitialized()) return;
        float dx = Mathf.Max(0.001f, transform.TransformVector(Vector3.right * m_StepX).magnitude);
        float dz = Mathf.Max(0.001f, transform.TransformVector(Vector3.forward * m_StepZ).magnitude);
        float speed = Finite(waveSpeed) ? Mathf.Clamp(waveSpeed, 0.1f, 4f) : 2f;
        float decay = Finite(damping) ? Mathf.Max(0.1f, damping) : 2.5f;
        float time = Mathf.Min(deltaTime, 0.1f);
        // Subdivide to satisfy the explicit wave equation's stability condition.
        float stableStep = 0.45f / (speed * Mathf.Sqrt(1f / (dx * dx) + 1f / (dz * dz)));
        int steps = Mathf.Clamp(Mathf.CeilToInt(time / stableStep), 1, 64);
        float dt = Mathf.Min(time / steps, stableStep);
        for (int step = 0; step < steps; step++)
        {
            for (int z = 1; z < m_Depth - 1; z++) for (int x = 1; x < m_Width - 1; x++)
            {
                int i = z * m_Width + x;
                float laplacian = (m_Heights[i - 1] + m_Heights[i + 1] - 2f * m_Heights[i]) / (dx * dx) +
                                  (m_Heights[i - m_Width] + m_Heights[i + m_Width] - 2f * m_Heights[i]) / (dz * dz);
                m_NextVelocities[i] = (m_Velocities[i] + speed * speed * laplacian * dt) * Mathf.Exp(-decay * dt);
            }
            for (int i = 0; i < m_Heights.Length; i++)
            {
                m_Velocities[i] = m_NextVelocities[i];
                // Relax the long-wavelength height offset as well as velocity, so
                // an isolated impact returns to the original flat surface.
                m_Heights[i] = (m_Heights[i] + m_Velocities[i] * dt) * Mathf.Exp(-decay * 0.25f * dt);
            }
        }
        float peak = 0;
        for (int i = 0; i < m_Heights.Length; i++) peak = Mathf.Max(peak, Mathf.Max(Mathf.Abs(m_Heights[i]), Mathf.Abs(m_Velocities[i])));
        if (peak < 0.00001f)
        {
            System.Array.Clear(m_Heights, 0, m_Heights.Length);
            System.Array.Clear(m_Velocities, 0, m_Velocities.Length);
            System.Array.Clear(m_NextVelocities, 0, m_NextVelocities.Length);
            m_Moving = false;
        }
        for (int i = 0; i < m_VertexAt.Length; i++)
        {
            int vertex = m_VertexAt[i];
            m_Vertices[vertex] = m_BaseVertices[vertex] + Vector3.up * m_Heights[i];
        }
        m_Mesh.vertices = m_Vertices;
        m_Mesh.RecalculateNormals();
        m_Mesh.RecalculateBounds();
    }

    private static bool Finite(float value) { return !float.IsNaN(value) && !float.IsInfinity(value); }
    private void OnDestroy()
    {
        if (m_Mesh == null) return;
        if (m_Filter != null && m_Filter.sharedMesh == m_Mesh) m_Filter.sharedMesh = m_Source;
        if (Application.isPlaying) Destroy(m_Mesh); else DestroyImmediate(m_Mesh);
    }
}
