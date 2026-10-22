using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class RecoveredNavMeshLoader : MonoBehaviour
{
    public NavMeshData data;
    private NavMeshDataInstance instance;

    private void OnEnable()
    {
        if (data == null || instance.valid) return;

        instance = NavMesh.AddNavMeshData(data);
        if (instance.valid) RefreshSceneLinks();
    }

    private void Start()
    {
        // All scene objects have finished their initial enable callbacks now.
        if (instance.valid) RefreshSceneLinks();
    }

    [ContextMenu("Refresh Existing Scene Links")]
    public void RefreshSceneLinks()
    {
        if (!instance.valid) return;

        OffMeshLink[] links = Resources.FindObjectsOfTypeAll<OffMeshLink>();
        foreach (OffMeshLink link in links)
        {
            if (link != null && link.gameObject.scene == gameObject.scene && link.isActiveAndEnabled && link.startTransform != null && link.endTransform != null)
            {
                // Legacy links have autoUpdatePositions disabled. Reconnect
                // them after the mesh becomes available without moving endpoints.
                link.UpdatePositions();
            }
        }
    }

    private void OnDisable()
    {
        if (instance.valid)
            instance.Remove();
        instance = new NavMeshDataInstance();
    }
}
