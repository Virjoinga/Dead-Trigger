// Unity 2017.4.40f1 — place in Assets/Editor/LegacyNavMeshLinks.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;
using System.IO;

public static class LegacyNavMeshLinks
{
    private const float SampleDistance = 2.0f;

    private static OffMeshLink[] GetSceneLinks()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        OffMeshLink[] all = Resources.FindObjectsOfTypeAll<OffMeshLink>();
        System.Collections.Generic.List<OffMeshLink> sceneLinks = new System.Collections.Generic.List<OffMeshLink>();

        foreach (OffMeshLink link in all)
        {
            if (link != null && !EditorUtility.IsPersistent(link) && link.gameObject.scene == scene)
                sceneLinks.Add(link);
        }

        return sceneLinks.ToArray();
    }

    [MenuItem("Tools/Navigation/Validate Legacy Off-Mesh Links")]
    private static void Validate()
    {
        OffMeshLink[] links = GetSceneLinks();
        NavMeshTriangulation navigation = NavMesh.CalculateTriangulation();
        bool hasNavMesh = navigation.vertices != null && navigation.vertices.Length > 0;
        int missing = 0;
        int distant = 0;
        int bidirectional = 0;
        int oneWay = 0;

        foreach (OffMeshLink link in links)
        {
            if (link.biDirectional) bidirectional++;
            else oneWay++;

            if (link.startTransform == null || link.endTransform == null)
            {
                missing++;
                Debug.LogWarning("Missing start or end transform: " + link.name, link);
                continue;
            }

            if (!hasNavMesh) continue;

            NavMeshHit hit;
            bool startFound = NavMesh.SamplePosition(link.startTransform.position, out hit, SampleDistance, NavMesh.AllAreas);
            bool endFound = NavMesh.SamplePosition(link.endTransform.position, out hit, SampleDistance, NavMesh.AllAreas);

            if (!startFound || !endFound)
            {
                distant++;
                Debug.LogWarning("Off-mesh link endpoint was not near baked NavMesh: " + link.name + " (start=" + startFound + ", end=" + endFound + ")", link);
            }
        }

        Debug.Log("Legacy link check: " + links.Length + " links (" + bidirectional + " bidirectional, " + oneWay + " one-way), " + missing + " missing endpoint references, " + distant + " links with endpoints outside " + SampleDistance + "m sample radius. Baked NavMesh present: " + hasNavMesh);

        if (!hasNavMesh)
            Debug.LogWarning("No active baked NavMesh was found. This check verified link references but cannot test endpoint proximity until a NavMesh is baked/loaded. NavMesh.SamplePosition proximity does not prove a link is traversable.");
    }

    [MenuItem("Tools/Navigation/Restore Legacy Off-Mesh Links From CSV")]
    private static void RestoreFromCsv()
    {
        OffMeshLink[] existing = GetSceneLinks();
        if (existing.Length > 0)
        {
            EditorUtility.DisplayDialog("Links already exist", "This scene already contains " + existing.Length + " OffMeshLink components. Restore is disabled to prevent duplicate links. Use Validate instead.", "OK");
            return;
        }

        string path = EditorUtility.OpenFilePanel("Select recovered scene_links.csv", "", "csv");
        if (string.IsNullOrEmpty(path)) return;

        string[] lines = File.ReadAllLines(path);
        if (lines.Length < 2 || !lines[0].StartsWith("component_file_id,name,gameobject_file_id,"))
        {
            EditorUtility.DisplayDialog("Unexpected CSV", "The selected file isn't the recovered scene_links.csv.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Restore legacy links", "Create " + (lines.Length - 1) + " replacement OffMeshLink objects in this scene? This won't reconstruct their original parent hierarchy. You can undo the operation.", "Restore", "Cancel"))
            return;

        GameObject root = new GameObject("Recovered OffMeshLinks");
        Undo.RegisterCreatedObjectUndo(root, "Restore Legacy OffMeshLinks");
        int restored = 0;

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]) || lines[i].Trim().Length == 0) continue;
            string[] cells = lines[i].Split(',');
            if (cells.Length != 17)
            {
                Debug.LogWarning("Skipped CSV row " + (i + 1) + ": expected 17 columns.");
                continue;
            }

            Vector3 start;
            Vector3 end;
            if (!TryReadVector(cells, 5, out start) || !TryReadVector(cells, 8, out end))
            {
                Debug.LogWarning("Skipped CSV row " + (i + 1) + ": invalid link coordinates.");
                continue;
            }

            GameObject holder = new GameObject(cells[1]);
            Undo.RegisterCreatedObjectUndo(holder, "Restore Legacy OffMeshLink");
            holder.transform.SetParent(root.transform, false);

            GameObject startObject = new GameObject("Start");
            Undo.RegisterCreatedObjectUndo(startObject, "Restore Legacy Link Start");
            startObject.transform.SetParent(holder.transform, false);
            startObject.transform.position = start;

            GameObject endObject = new GameObject("End");
            Undo.RegisterCreatedObjectUndo(endObject, "Restore Legacy Link End");
            endObject.transform.SetParent(holder.transform, false);
            endObject.transform.position = end;

            OffMeshLink link = Undo.AddComponent<OffMeshLink>(holder);
            link.startTransform = startObject.transform;
            link.endTransform = endObject.transform;
            link.biDirectional = bool.Parse(cells[11]);
            link.enabled = bool.Parse(cells[12]);
            link.activated = bool.Parse(cells[13]);
            link.area = int.Parse(cells[14], CultureInfo.InvariantCulture);
            link.costOverride = float.Parse(cells[15], CultureInfo.InvariantCulture);
            link.autoUpdatePositions = bool.Parse(cells[16]);
            restored++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Reconstructed " + restored + " OffMeshLinks from CSV. Validate and test their traversal after baking the NavMesh.");
    }

    private static bool TryReadVector(string[] cells, int index, out Vector3 result)
    {
        result = Vector3.zero;
        float x, y, z;
        if (!float.TryParse(cells[index], NumberStyles.Float, CultureInfo.InvariantCulture, out x)) return false;
        if (!float.TryParse(cells[index + 1], NumberStyles.Float, CultureInfo.InvariantCulture, out y)) return false;
        if (!float.TryParse(cells[index + 2], NumberStyles.Float, CultureInfo.InvariantCulture, out z)) return false;
        result = new Vector3(x, y, z);
        return true;
    }
}
