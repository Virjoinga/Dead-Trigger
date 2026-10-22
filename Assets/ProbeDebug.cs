
using UnityEngine;
using UnityEngine.Rendering;

public class ProbeDebug : MonoBehaviour
{
    void Start()
    {
        Renderer r = GetComponentInChildren<Renderer>();
        LightProbes probes = LightmapSettings.lightProbes;

        Debug.Log("Light Probe Count: " + (probes != null ? probes.count : 0));

        if (r == null || probes == null || probes.count == 0)
            return;

        SphericalHarmonicsL2 sh;
        LightProbes.GetInterpolatedProbe(r.bounds.center, r, out sh);

        Color[] colors = new Color[1];
        sh.Evaluate(new Vector3[] { Vector3.up }, colors);

        Debug.Log("Light Probe Usage: " + r.lightProbeUsage);
        Debug.Log("Interpolated Probe Lighting: " + colors[0]);
    }
}
