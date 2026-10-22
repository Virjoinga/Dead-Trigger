using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ValidateZombieShader
{
    [Serializable] public class Report
    {
        public string unityVersion, graphicsDevice;
        public int cases, baselineFailures, pixelsCompared;
        public float maxCandidateError, maxBaselineError;
        public bool productionPassSupported, passed;
    }
    public static void Run()
    {
        if (!File.Exists("ZombieShaderHarness.marker") || Application.unityVersion != "2017.4.40f1")
            throw new InvalidOperationException("Run only in the marked isolated Unity 2017 shader harness");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        foreach(string path in new string[]{"Assets/FlatNormal.png","Assets/TiltedNormal.png","Assets/ZombieNormal.png"})
        {
            TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType = TextureImporterType.NormalMap;
            importer.sRGBTexture = false;
            importer.textureCompression = TextureImporterCompression.Compressed;
            importer.SaveAndReimport();
        }
        Material candidate = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/Candidate.shader"));
        Material baseline = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/Baseline.shader"));
        Material reference = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/ZombieReference.shader"));
        Material production = new Material(AssetDatabase.LoadAssetAtPath<Shader>("Assets/Production.shader"));
        bool productionPassSupported = production.shader.isSupported && production.SetPass(0);
        if (!productionPassSupported) throw new InvalidOperationException("Production shader pass unsupported");
        if (!candidate.shader.isSupported || !baseline.shader.isSupported || !reference.shader.isSupported)
            throw new InvalidOperationException("Shader unsupported by test graphics device");
        Texture2D lookup = new Texture2D(64,64,TextureFormat.RGBA32,false,true);
        lookup.wrapMode = TextureWrapMode.Clamp;
        for(int y=0;y<64;y++) for(int x=0;x<64;x++) lookup.SetPixel(x,y,new Color(x/160f,y/160f,0.1f,(x+y)/320f));
        lookup.Apply();
        Texture2D albedo = new Texture2D(1,1,TextureFormat.RGBA32,false,true);
        albedo.SetPixel(0,0,new Color(0.7f,0.6f,0.5f,0.4f)); albedo.Apply();
        foreach(Material mat in new Material[]{candidate,baseline,reference})
        {
            mat.SetTexture("_MainTex",albedo); mat.SetTexture("_BRDFTex",lookup);
            mat.SetFloat("_LightProbesLightingAmount",0.7f);
            mat.SetVector("_SpecularStrength",new Vector4(0,0,0,1));
        }
        GameObject cameraObject = new GameObject("Test camera",typeof(Camera));
        Camera camera = cameraObject.GetComponent<Camera>();
        camera.transform.position = new Vector3(0,0,-4);
        camera.orthographic = true; camera.orthographicSize = 1.5f;
        camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = Color.black;
        camera.allowHDR = false; camera.allowMSAA = false;
        RenderTexture target = new RenderTexture(64,64,24,RenderTextureFormat.ARGB32,RenderTextureReadWrite.Linear);
        camera.targetTexture = target;
        GameObject surface = new GameObject("Test surface",typeof(MeshFilter),typeof(MeshRenderer));
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[]{new Vector3(-1,-1,0),new Vector3(1,-1,0),new Vector3(1,1,0),new Vector3(-1,1,0)};
        mesh.uv = new Vector2[]{Vector2.zero,Vector2.right,Vector2.one,Vector2.up};
        mesh.colors = new Color[]{Color.white,Color.white,Color.white,Color.white};
        mesh.triangles = new int[]{0,2,1,0,3,2};
        surface.GetComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer renderer = surface.GetComponent<MeshRenderer>();
        Report report = new Report();
        report.unityVersion = Application.unityVersion;
        report.graphicsDevice = SystemInfo.graphicsDeviceType + ": " + SystemInfo.graphicsDeviceName;
        report.productionPassSupported = productionPassSupported;
        foreach(string normalPath in new string[]{"Assets/FlatNormal.png","Assets/TiltedNormal.png","Assets/ZombieNormal.png"})
        foreach(float handedness in new float[]{1,-1})
        foreach(Vector3 angles in new Vector3[]{Vector3.zero,new Vector3(15,30,40),new Vector3(-20,-30,90)})
        {
            Vector3 n = new Vector3(0.3f,0.4f,-0.8660254f).normalized;
            Vector3 t = Vector3.Cross(Vector3.up,n).normalized;
            mesh.normals = new Vector3[]{n,n,n,n};
            Vector4 tangent = new Vector4(t.x,t.y,t.z,handedness);
            mesh.tangents = new Vector4[]{tangent,tangent,tangent,tangent};
            surface.transform.rotation = Quaternion.Euler(angles);
            foreach(Material mat in new Material[]{candidate,baseline,reference})
            {
                mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(normalPath));
                mat.SetVector("_FixtureSHAr",new Vector4(0.4f,0.2f,-0.3f,0.3f));
                mat.SetVector("_FixtureSHAg",new Vector4(0.2f,-0.3f,0.1f,0.3f));
                mat.SetVector("_FixtureSHAb",new Vector4(-0.1f,0.1f,-0.2f,0.3f));
                mat.SetVector("_FixtureSHBr",new Vector4(0.1f,0.2f,-0.1f,0.1f));
                mat.SetVector("_FixtureSHBg",new Vector4(0.2f,-0.1f,0.1f,0.1f));
                mat.SetVector("_FixtureSHBb",new Vector4(-0.1f,0.2f,0.1f,-0.1f));
                mat.SetVector("_FixtureSHC",new Vector4(0.1f,-0.1f,0.2f,0));
            }
            Color[] expected = Render(camera,renderer,reference);
            Color[] actual = Render(camera,renderer,candidate);
            Color[] old = Render(camera,renderer,baseline);
            float candidateError = 0, baselineError = 0;
            float referencePeak = 0;
            for(int y=24;y<40;y++) for(int x=24;x<40;x++)
            {
                int pixel = y*64+x;
                referencePeak = Mathf.Max(referencePeak,expected[pixel].maxColorComponent);
                for(int c=0;c<3;c++)
                {
                    candidateError = Mathf.Max(candidateError,Mathf.Abs(actual[pixel][c]-expected[pixel][c]));
                    baselineError = Mathf.Max(baselineError,Mathf.Abs(old[pixel][c]-expected[pixel][c]));
                }
                report.pixelsCompared++;
            }
            if(referencePeak < 0.05f) throw new InvalidOperationException("Empty reference render");
            report.maxCandidateError = Mathf.Max(report.maxCandidateError,candidateError);
            report.maxBaselineError = Mathf.Max(report.maxBaselineError,baselineError);
            if(baselineError > 0.02f) report.baselineFailures++;
            report.cases++;
        }
        report.passed = report.productionPassSupported && report.maxCandidateError <= 0.008f && report.baselineFailures > 0;
        File.WriteAllText("ZombieShaderValidation.json",JsonUtility.ToJson(report,true));
        if(!report.passed) throw new InvalidOperationException("GPU shader comparison failed; see report");
        Debug.Log("Zombie shader GPU comparison passed: " + JsonUtility.ToJson(report));
    }
    static Color[] Render(Camera camera,MeshRenderer renderer,Material material)
    {
        renderer.sharedMaterial = material;
        camera.Render();
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        Texture2D readback = new Texture2D(64,64,TextureFormat.RGBA32,false,true);
        readback.ReadPixels(new Rect(0,0,64,64),0,0); readback.Apply();
        Color[] pixels = readback.GetPixels();
        UnityEngine.Object.DestroyImmediate(readback);
        RenderTexture.active = previous;
        return pixels;
    }
}
