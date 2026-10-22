using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public static class TestSceneLightProbes
{
    public static void Run()
    {
        if (!File.Exists("LightProbeRecoveryHarness.marker") || Application.unityVersion != "2017.4.40f1")
            throw new InvalidOperationException("Run only in the isolated Unity 2017.4.40f1 recovery harness.");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        string scenePath = "Assets/ProbeSceneTest.unity";
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
        LightProbes probes = AssetDatabase.LoadAssetAtPath<LightProbes>("Assets/RecoveredLightProbes/mall/LightProbes.asset");
        string text = File.ReadAllText("LightingTemplate.yaml");
        if (!text.StartsWith("%YAML")) throw new InvalidOperationException("Use an untouched text lighting template outside Assets");
        text = Regex.Replace(text, @"(?m)^  m_Scene:.*$", "  m_Scene: {fileID: 102900000, guid: " + AssetDatabase.AssetPathToGUID(scenePath) + ", type: 3}");
        Directory.CreateDirectory("Tools/LightProbeRecovery/LightingTemplates");
        File.WriteAllText("Tools/LightProbeRecovery/LightingTemplates/mall.yaml", text);
        AttachRecoveredLightProbes.Attach(probes);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        EditorSceneManager.OpenScene(scenePath);
        LightProbes loaded = LightmapSettings.lightProbes;
        Debug.Log("Native scene probes after reopen=" + (loaded == null ? -1 : loaded.count));
        if (loaded == null || loaded.count != probes.count) throw new InvalidOperationException("Native lighting scene reference did not load probes");
        if (loaded.cellCount != probes.cellCount) throw new InvalidOperationException("Native scene tetrahedron count differs");
        SphericalHarmonicsL2[] actual = loaded.bakedProbes;
        SphericalHarmonicsL2[] expected = probes.bakedProbes;
        for (int i = 0; i < expected.Length; i++)
        {
            if (loaded.positions[i] != probes.positions[i]) throw new InvalidOperationException("Native scene probe position differs");
            for (int c = 0; c < 3; c++)
                for (int k = 0; k < 9; k++)
                    if (actual[i][c, k] != expected[i][c, k]) throw new InvalidOperationException("Native scene stored coefficient differs");
        }
        SphericalHarmonicsL2 sh;
        LightProbes.GetInterpolatedProbe(probes.positions[0], null, out sh);
        float maxError = 0;
        for (int c = 0; c < 3; c++)
            for (int k = 0; k < 9; k++) maxError = Mathf.Max(maxError, Mathf.Abs(sh[c, k] - probes.bakedProbes[0][c, k]));
        if (maxError > 0.0001f) throw new InvalidOperationException("Native scene probe interpolation differs");
        File.WriteAllText("NativeProbeSceneResult.json", "{\"probeCount\":" + loaded.count + ",\"maxCoefficientError\":" + maxError.ToString("R", System.Globalization.CultureInfo.InvariantCulture) + ",\"passed\":true}");
    }
}
