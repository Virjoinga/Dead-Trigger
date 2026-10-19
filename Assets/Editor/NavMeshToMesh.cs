
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public static class NavMeshToMesh
{
    [MenuItem("Tools/Navigation/Export NavMesh To Mesh")]
    private static void Export()
    {
        NavMeshTriangulation data = NavMesh.CalculateTriangulation();

        if (data.vertices == null || data.vertices.Length == 0)
        {
            Debug.LogError("No active NavMesh was found.");
            return;
        }

        string path = EditorUtility.SaveFilePanelInProject("Save NavMesh Mesh", "NavMeshGeometry", "asset", "Select where to save the mesh.");

        if (string.IsNullOrEmpty(path))
            return;

        Mesh mesh = new Mesh();
        mesh.name = "NavMeshGeometry";
        mesh.indexFormat = data.vertices.Length > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16;
        mesh.vertices = data.vertices;

        int[] triangles = (int[])data.indices.Clone();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 a = data.vertices[triangles[i]];
            Vector3 b = data.vertices[triangles[i + 1]];
            Vector3 c = data.vertices[triangles[i + 2]];

            if (Vector3.Cross(b - a, c - a).y < 0f)
            {
                int temp = triangles[i + 1];
                triangles[i + 1] = triangles[i + 2];
                triangles[i + 2] = temp;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        AssetDatabase.CreateAsset(mesh, path);

        GameObject obj = new GameObject("NavMesh Bake Source");
        Undo.RegisterCreatedObjectUndo(obj, "Create NavMesh Bake Source");

        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        obj.AddComponent<MeshRenderer>();

        GameObjectUtility.SetStaticEditorFlags(obj, StaticEditorFlags.NavigationStatic);

        Selection.activeGameObject = obj;
        EditorSceneManager.MarkSceneDirty(obj.scene);

        Debug.Log("Exported NavMesh: " + mesh.vertexCount + " vertices, " + triangles.Length / 3 + " triangles.");
    }
}
