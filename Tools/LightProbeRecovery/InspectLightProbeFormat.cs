using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Reflection;

public static class InspectLightProbeFormat
{
    public static void InspectGeometryAPI()
    {
        foreach (Type type in new Type[] { typeof(LightProbes), typeof(Lightmapping), typeof(LightingDataAsset) })
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                string name = method.Name.ToLowerInvariant();
                if (name.Contains("probe") || name.Contains("tetra") || name.Contains("lightingdata")) Debug.Log(type.Name + ": " + method);
            }
            foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) Debug.Log(type.Name + " constructor: " + constructor);
        }
    }

    public static void InspectLightingFields()
    {
        LightingDataAsset data = AssetDatabase.LoadAssetAtPath<LightingDataAsset>("Assets/LightingTemplate.asset");
        SerializedObject serialized = new SerializedObject(data);
        SerializedProperty iterator = serialized.GetIterator();
        bool enter = true;
        while (iterator.Next(enter))
        {
            Debug.Log("Lighting data field " + iterator.propertyPath + " type=" + iterator.type);
            enter = false;
        }
        foreach (MethodInfo method in typeof(LightingDataAsset).GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) Debug.Log("LightingData method " + method);
    }
    private static string Values(SphericalHarmonicsL2 sh)
    {
        StringBuilder text = new StringBuilder();
        for (int c = 0; c < 3; c++)
        {
            text.Append(" channel " + c + ":");
            for (int k = 0; k < 9; k++) text.Append(" " + sh[c, k].ToString("R", System.Globalization.CultureInfo.InvariantCulture));
        }
        return text.ToString();
    }

    public static void Run()
    {
        SphericalHarmonicsL2 ambient = new SphericalHarmonicsL2();
        ambient.AddAmbientLight(new Color(1, 2, 3));
        Debug.Log("Ambient:" + Values(ambient));
        foreach (Vector3 axis in new Vector3[] { Vector3.right, Vector3.up, Vector3.forward })
        {
            SphericalHarmonicsL2 sh = new SphericalHarmonicsL2();
            sh.AddDirectionalLight(axis, Color.white, 1);
            Debug.Log("Direction " + axis + ":" + Values(sh));
        }
        LightProbes source = AssetDatabase.LoadAssetAtPath<LightProbes>("Assets/CurrentMall.asset");
        LightProbes clone = UnityEngine.Object.Instantiate(source);
        SphericalHarmonicsL2[] probes = clone.bakedProbes;
        for (int c = 0; c < 3; c++)
            for (int k = 0; k < 9; k++) probes[0][c, k] = c * 100 + k + 1;
        clone.bakedProbes = probes;
        AssetDatabase.CreateAsset(clone, "Assets/Sentinel.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Current mall count=" + source.count + ", cells=" + source.cellCount + ", first SH=" + Values(source.bakedProbes[0]));
    }
}
