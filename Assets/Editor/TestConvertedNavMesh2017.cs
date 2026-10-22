using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public static class TestConvertedNavMesh2017
{
    [MenuItem("Tools/Navigation/Test Selected Converted NavMesh")]
    private static void Test()
    {
        NavMeshData data = Selection.activeObject as NavMeshData;
        if (data == null)
        {
            Debug.LogError("Select the experimental NavMeshData asset in the Project window first.");
            return;
        }

        NavMeshDataInstance instance = NavMesh.AddNavMeshData(data);
        if (!instance.valid)
        {
            Debug.LogError("Unity could not add the selected NavMeshData. The experimental tile conversion is not compatible.");
            return;
        }

        try
        {
            Vector3[] points = new Vector3[]
            {
                new Vector3(-55.8f, 12.114561f, -33.245041f),
                new Vector3(-10.9f, 12.114561f, -33.245041f),
                new Vector3(-94.1f, 0.014561f, 29.688294f),
                new Vector3(-16.133331f, 0.014561f, 31.988295f)
            };

            int successes = 0;
            for (int i = 0; i < points.Length; i++)
            {
                NavMeshHit hit;
                bool found = NavMesh.SamplePosition(points[i], out hit, 1.0f, NavMesh.AllAreas);
                if (found) successes++;
                Debug.Log("Converted NavMesh tile " + i + ": " + (found ? "sample found at " + hit.position : "NO SAMPLE"));
            }

            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            Debug.Log("Converted NavMesh check: " + successes + "/4 tile samples found. Loaded triangulation contains " + triangulation.indices.Length / 3 + " triangles. Run in an otherwise empty scene so existing NavMeshes cannot affect the results.");
        }
        finally
        {
            instance.Remove();
        }
    }
}
