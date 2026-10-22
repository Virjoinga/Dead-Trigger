using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class ValidateLightProbeRecovery
{
    [Serializable] public class Probe { public Vector3 position; public float[] legacyCoefficients; }
    [Serializable] public class Tetra { public int[] indices; }
    [Serializable] public class Manifest { public string level; public int probeCount; public int cellCount; public Probe[] probes; public Tetra[] tetrahedra; public double[] basisFactors; }
    [Serializable] public class Cases { public string[] assets; }
    [Serializable] public class Result
    {
        public string level;
        public int probes;
        public int cells;
        public int coefficientsChecked;
        public int probeQueries;
        public int probeQueriesPassed;
        public int interiorQueries;
        public int interiorQueriesPassed;
        public int tightInteriorQueriesPassed;
        public int degenerateCells;
        public float maxPositionError;
        public float maxStoredCoefficientError;
        public float maxProbeRelativeError;
        public float maxInteriorRelativeError;
        public bool passed;
        public string error;
        public List<string> probeFailures = new List<string>();
        public List<string> interiorFailures = new List<string>();
    }
    [Serializable] public class Report { public string unityVersion; public bool basisCalibrationPassed; public List<Result> results = new List<Result>(); }

    private static readonly double[] Factors = new double[] { 0.28209479177387814, -0.32573500793527993, 0.32573500793527993, -0.32573500793527993, 0.2731371076480198, -0.2731371076480198, 0.07884789131313001, -0.2731371076480198, 0.1365685538240099 };

    private static SphericalHarmonicsL2 Converted(float[] legacy)
    {
        SphericalHarmonicsL2 sh = new SphericalHarmonicsL2();
        for (int c = 0; c < 3; c++)
            for (int k = 0; k < 9; k++) sh[c, k] = (float)(legacy[k * 3 + c] * Factors[k]);
        return sh;
    }

    private static float RelativeError(SphericalHarmonicsL2 a, SphericalHarmonicsL2 b)
    {
        float error = 0;
        for (int c = 0; c < 3; c++)
            for (int k = 0; k < 9; k++) error = Mathf.Max(error, Mathf.Abs(a[c, k] - b[c, k]) / (1 + Mathf.Abs(b[c, k])));
        return error;
    }

    private static void CalibrateBasis()
    {
        // Independent reference: the legacy Unity API's real-SH construction,
        // checked against Unity 2017's native AddAmbient/DirectionalLight APIs.
        Color color = new Color(0.2f, 0.4f, 0.8f);
        float[] legacy = new float[27];
        for (int c = 0; c < 3; c++) legacy[c] = color[c] * (float)(2 * Math.Sqrt(Math.PI));
        SphericalHarmonicsL2 native = new SphericalHarmonicsL2();
        native.AddAmbientLight(color);
        if (RelativeError(Converted(legacy), native) > 0.000001f) throw new InvalidOperationException("Legacy ambient basis calibration failed");
        foreach (Vector3 direction in new Vector3[] { Vector3.right, Vector3.up, Vector3.forward, new Vector3(1, 2, 3).normalized, new Vector3(-2, 3, -1).normalized })
        {
            double x = direction.x, y = direction.y, z = direction.z;
            double[] basis = new double[] { 1 / (2 * Math.Sqrt(Math.PI)), -y * Math.Sqrt(3 / Math.PI) / 2,
                z * Math.Sqrt(3 / Math.PI) / 2, -x * Math.Sqrt(3 / Math.PI) / 2,
                x * y * Math.Sqrt(15 / Math.PI) / 2, -y * z * Math.Sqrt(15 / Math.PI) / 2,
                (3 * z * z - 1) * Math.Sqrt(5 / Math.PI) / 4,
                -x * z * Math.Sqrt(15 / Math.PI) / 2, (x * x - y * y) * Math.Sqrt(15 / Math.PI) / 4 };
            for (int c = 0; c < 3; c++)
                for (int k = 0; k < 9; k++) legacy[k * 3 + c] = (float)(basis[k] * color[c] * 0.65 * 2 * (16 * Math.PI / 17));
            native.Clear();
            native.AddDirectionalLight(direction, color, 0.65f);
            if (RelativeError(Converted(legacy), native) > 0.000001f) throw new InvalidOperationException("Legacy directional basis calibration failed: " + direction);
        }
    }

    public static void Run()
    {
        if (Application.unityVersion != "2017.4.40f1" || !File.Exists("LightProbeRecoveryHarness.marker"))
            throw new InvalidOperationException("Run only in the marked isolated Unity 2017.4.40f1 project");
        Report report = new Report();
        report.unityVersion = Application.unityVersion;
        CalibrateBasis();
        report.basisCalibrationPassed = true;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = Color.black;
        Cases cases = JsonUtility.FromJson<Cases>(File.ReadAllText("LightProbeCases.json"));
        bool allPassed = cases.assets.Length > 0;
        foreach (string asset in cases.assets)
        {
            Result result = Test(asset);
            report.results.Add(result);
            allPassed &= result.passed;
            Debug.Log(result.level + ": stored coefficients=" + result.coefficientsChecked + ", probe queries=" + result.probeQueriesPassed + "/" + result.probeQueries + ", interior queries=" + result.interiorQueriesPassed + "/" + result.interiorQueries + ", passed=" + result.passed + " " + result.error);
        }
        File.WriteAllText("LightProbeRecoveryResults.json", JsonUtility.ToJson(report, true));
        if (!allPassed) throw new InvalidOperationException("LightProbe validation failed; inspect LightProbeRecoveryResults.json");
    }

    private static Result Test(string asset)
    {
        Result result = new Result();
        try
        {
            Manifest source = JsonUtility.FromJson<Manifest>(File.ReadAllText(Path.ChangeExtension(asset, ".recovery.json")));
            result.level = source.level;
            LightProbes probes = AssetDatabase.LoadAssetAtPath<LightProbes>(asset);
            if (probes == null) throw new InvalidOperationException("Could not import probe asset");
            result.probes = probes.count;
            result.cells = probes.cellCount;
            if (probes.count != source.probeCount || probes.cellCount != source.cellCount) throw new InvalidOperationException("Probe or tetrahedron count changed");
            SphericalHarmonicsL2[] actual = probes.bakedProbes;
            Vector3[] positions = probes.positions;
            SphericalHarmonicsL2[] expected = new SphericalHarmonicsL2[source.probes.Length];
            for (int i = 0; i < expected.Length; i++)
            {
                expected[i] = Converted(source.probes[i].legacyCoefficients);
                result.maxPositionError = Mathf.Max(result.maxPositionError, Vector3.Distance(positions[i], source.probes[i].position));
                for (int c = 0; c < 3; c++)
                    for (int k = 0; k < 9; k++)
                    {
                        result.coefficientsChecked++;
                        result.maxStoredCoefficientError = Mathf.Max(result.maxStoredCoefficientError, Mathf.Abs(actual[i][c, k] - expected[i][c, k]));
                    }
            }
            LightmapSettings.lightProbes = probes;
            for (int i = 0; i < expected.Length; i++)
            {
                SphericalHarmonicsL2 interpolated;
                LightProbes.GetInterpolatedProbe(positions[i], null, out interpolated);
                float error = RelativeError(interpolated, expected[i]);
                result.probeQueries++;
                result.maxProbeRelativeError = Mathf.Max(result.maxProbeRelativeError, error);
                if (error < 0.0001f) result.probeQueriesPassed++;
                else
                {
                    result.probeFailures.Add(i + " at " + positions[i] + ": error=" + error + ", constant=" + interpolated[0, 0] + "/" + interpolated[1, 0] + "/" + interpolated[2, 0] + ", expected=" + expected[i][0, 0] + "/" + expected[i][1, 0] + "/" + expected[i][2, 0]);
                }
            }
            foreach (Tetra tetra in source.tetrahedra)
            {
                int[] t = tetra.indices;
                if (t[0] < 0 || t[1] < 0 || t[2] < 0 || t[3] < 0) continue;
                Vector3 a = positions[t[0]], b = positions[t[1]], c = positions[t[2]], d = positions[t[3]];
                float determinant = Vector3.Dot(b - a, Vector3.Cross(c - a, d - a));
                if (Mathf.Abs(determinant) < 0.000001f) { result.degenerateCells++; continue; }
                Vector3 center = (a + b + c + d) / 4;
                SphericalHarmonicsL2 target = (expected[t[0]] + expected[t[1]] + expected[t[2]] + expected[t[3]]) * 0.25f;
                SphericalHarmonicsL2 interpolated;
                LightProbes.GetInterpolatedProbe(center, null, out interpolated);
                float error = RelativeError(interpolated, target);
                result.interiorQueries++;
                result.maxInteriorRelativeError = Mathf.Max(result.maxInteriorRelativeError, error);
                if (error < 0.0001f) result.tightInteriorQueriesPassed++;
                if (error < 0.001f) result.interiorQueriesPassed++;
                else result.interiorFailures.Add(string.Join(",", Array.ConvertAll(t, delegate(int n) { return n.ToString(); })) + ": error=" + error);
            }
            result.passed = result.maxPositionError == 0 && result.maxStoredCoefficientError < 0.000001f && result.probeQueriesPassed == result.probeQueries && result.interiorQueriesPassed == result.interiorQueries;
        }
        catch (Exception error) { result.error = error.ToString(); }
        finally { LightmapSettings.lightProbes = null; }
        return result;
    }
}
