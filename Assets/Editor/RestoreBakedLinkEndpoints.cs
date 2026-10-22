using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// Optional repair for a scene link whose original transforms no longer match
// the snapped endpoints recorded in the legacy bake. Never runs automatically.
public static class RestoreBakedLinkEndpoints
{
    [Serializable] private class Link
    {
        public string id;
        public Vector3 start;
        public Vector3 end;
        public Vector3 bakedStart;
        public Vector3 bakedEnd;
        public bool sourceAttached;
    }
    [Serializable] private class Manifest
    {
        public Link[] links;
    }

    [MenuItem("Tools/Navigation/Restore Selected Link Baked Endpoints")]
    public static void Restore()
    {
        if (Application.isPlaying || EditorSceneManager.sceneCount != 1)
        {
            Debug.LogError("Exit Play mode and open only the target scene before restoring a link.");
            return;
        }
        OffMeshLink selected = Selection.activeGameObject == null ? null : Selection.activeGameObject.GetComponent<OffMeshLink>();
        if (selected == null || selected.gameObject.scene != SceneManager.GetActiveScene() || selected.startTransform == null || selected.endTransform == null)
        {
            Debug.LogError("Select an existing scene OffMeshLink with valid endpoint transforms.");
            return;
        }
        PropertyInfo property = typeof(UnityEditor.AI.NavMeshBuilder).GetProperty("navMeshSettingsObject", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        UnityEngine.Object target = property == null ? null : property.GetValue(null, null) as UnityEngine.Object;
        if (target == null) throw new InvalidOperationException("No scene NavMesh settings found.");
        SerializedProperty reference = new SerializedObject(target).FindProperty("m_NavMeshData");
        NavMeshData data = reference == null ? null : reference.objectReferenceValue as NavMeshData;
        string path = data == null ? "" : Path.ChangeExtension(AssetDatabase.GetAssetPath(data), ".recovery.json");
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogError("Attach a recovered asset with its .recovery.json sidecar to this scene first.");
            return;
        }
        Manifest manifest = JsonUtility.FromJson<Manifest>(File.ReadAllText(path));
        Link match = null;
        foreach (Link link in manifest.links)
        {
            if (Vector3.Distance(selected.startTransform.position, link.start) > 0.02f || Vector3.Distance(selected.endTransform.position, link.end) > 0.02f) continue;
            if (match != null)
            {
                Debug.LogError("Multiple original links match these endpoints; no changes made.");
                return;
            }
            match = link;
        }
        if (match == null || !match.sourceAttached)
        {
            Debug.LogError("No uniquely matched, fully attached original baked link was found. No changes made.");
            return;
        }
        Transform start = selected.startTransform;
        Transform end = selected.endTransform;
        if (start == end || start.childCount != 0 || end.childCount != 0 || start.GetComponents<Component>().Length != 1 || end.GetComponents<Component>().Length != 1)
        {
            Debug.LogError("Automatic repair only supports separate empty endpoint helper objects; no changes made.");
            return;
        }
        Undo.RecordObjects(new UnityEngine.Object[] { start, end }, "Restore Baked Link Endpoints");
        start.position = match.bakedStart;
        end.position = match.bakedEnd;
        foreach (OffMeshLink link in Resources.FindObjectsOfTypeAll<OffMeshLink>())
        {
            if (link != null && link.gameObject.scene == selected.gameObject.scene && link.isActiveAndEnabled && (link.startTransform == start || link.startTransform == end || link.endTransform == start || link.endTransform == end))
                link.UpdatePositions();
        }
        EditorSceneManager.MarkSceneDirty(selected.gameObject.scene);
        Debug.Log("Restored original baked endpoints for " + selected.name + " (" + match.id + "). Save and test traversal; animation references and link settings are unchanged.", selected);
    }
}
