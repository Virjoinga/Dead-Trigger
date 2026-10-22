using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

// Isolated batch-project test. Never opens or rewrites a gameplay scene.
public static class ValidateRecoveredNavMeshes
{
    [Serializable] public class Probe
    {
        public Vector3 start;
        public Vector3 end;
        public bool crossTile;
        public string label;
    }
    [Serializable] public class Link
    {
        public string id;
        public Vector3 start;
        public Vector3 end;
        public bool bidirectional;
        public int area;
        public Vector3 bakedStart;
        public Vector3 bakedEnd;
        public bool sourceAttached;
    }
    [Serializable] public class Manifest
    {
        public string source;
        public int expectedTriangles;
        public Probe[] probes;
        public Link[] links;
        public Vector3[] surfaceSamples;
    }
    [Serializable] public class Result
    {
        public string asset;
        public string error;
        public int expectedTriangles;
        public int triangles;
        public int samples;
        public int samplesFound;
        public int detailedSamples;
        public int detailedFound;
        public int tightDetailedFound;
        public float maxSampleDistance;
        public int adjacentPaths;
        public int adjacentComplete;
        public int portals;
        public int portalsComplete;
        public int linkPaths;
        public int linksComplete;
        public int rawLinkPaths;
        public int rawLinksComplete;
        public int sourceUnattachedLinks;
        public int repairedSceneLinks;
        public List<string> linkWarnings = new List<string>();
        public bool passed;
        public List<string> failures = new List<string>();
    }
    [Serializable] public class Report
    {
        public string unityVersion;
        public List<Result> results = new List<Result>();
    }
    [Serializable] public class Cases
    {
        public string[] assets;
    }

    public static void Run()
    {
        if (Application.unityVersion != "2017.4.40f1" || !File.Exists("NavMeshRecoveryHarness.marker"))
            throw new InvalidOperationException("Run only in the isolated marked Unity 2017.4.40f1 harness.");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        string[] assets = JsonUtility.FromJson<Cases>(File.ReadAllText("RecoveryCases.json")).assets;
        Array.Sort(assets, StringComparer.Ordinal);
        Report report = new Report();
        report.unityVersion = Application.unityVersion;
        bool allPassed = assets.Length > 0;
        foreach (string asset in assets)
        {
            Result result = Test(asset.Replace('\\', '/'));
            report.results.Add(result);
            allPassed &= result.passed;
            Debug.Log(result.asset + ": samples=" + result.samplesFound + "/" + result.samples + ", adjacency=" + result.adjacentComplete + "/" + result.adjacentPaths + ", portals=" + result.portalsComplete + "/" + result.portals + ", links=" + result.linksComplete + "/" + result.linkPaths + ", passed=" + result.passed + " " + result.error);
        }
        File.WriteAllText("RecoveryBatchResults.json", JsonUtility.ToJson(report, true));
        if (!allPassed) throw new InvalidOperationException("Some recovery cases failed; inspect RecoveryBatchResults.json");
    }

    private static bool Complete(Vector3 a, Vector3 b)
    {
        NavMeshHit start;
        NavMeshHit end;
        NavMeshPath path = new NavMeshPath();
        return NavMesh.SamplePosition(a, out start, 2f, NavMesh.AllAreas) && NavMesh.SamplePosition(b, out end, 2f, NavMesh.AllAreas) && NavMesh.CalculatePath(start.position, end.position, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete;
    }

    private static Result Test(string asset)
    {
        Result result = new Result();
        result.asset = asset;
        try
        {
            Manifest manifest = JsonUtility.FromJson<Manifest>(File.ReadAllText(Path.ChangeExtension(asset, ".recovery.json")));
            result.expectedTriangles = manifest.expectedTriangles;
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            NavMeshData data = AssetDatabase.LoadAssetAtPath<NavMeshData>(asset);
            if (data == null) throw new InvalidOperationException("Could not import NavMeshData");
            AttachSceneNavMesh.Attach(data);
            string scenePath = "Assets/RecoveryBatchTest.unity";
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), scenePath);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.OpenScene(scenePath);
            NavMeshTriangulation mesh = NavMesh.CalculateTriangulation();
            result.triangles = mesh.indices.Length / 3;
            for (int i = 0; i < result.triangles; i++)
            {
                Vector3 center = (mesh.vertices[mesh.indices[i * 3]] + mesh.vertices[mesh.indices[i * 3 + 1]] + mesh.vertices[mesh.indices[i * 3 + 2]]) / 3f;
                result.samples++;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(center, out hit, 2f, NavMesh.AllAreas))
                {
                    result.samplesFound++;
                    result.maxSampleDistance = Mathf.Max(result.maxSampleDistance, Vector3.Distance(center, hit.position));
                }
            }
            foreach (Probe probe in manifest.probes)
            {
                result.adjacentPaths++;
                if (probe.crossTile) result.portals++;
                if (Complete(probe.start, probe.end))
                {
                    result.adjacentComplete++;
                    if (probe.crossTile) result.portalsComplete++;
                }
                else result.failures.Add("Adjacent " + probe.label);
            }
            foreach (Vector3 sample in manifest.surfaceSamples)
            {
                result.detailedSamples++;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(sample, out hit, 0.1f, NavMesh.AllAreas)) result.tightDetailedFound++;
                if (NavMesh.SamplePosition(sample, out hit, 0.25f, NavMesh.AllAreas)) result.detailedFound++;
                else if (result.failures.Count < 20)
                {
                    bool nearby = NavMesh.SamplePosition(sample, out hit, 20f, NavMesh.AllAreas);
                    result.failures.Add("Detailed sample " + (result.detailedSamples - 1) + " " + sample + ": wide-radius hit=" + nearby + " " + hit.position + ", distance=" + Vector3.Distance(sample, hit.position));
                }
            }
            foreach (Link row in manifest.links)
            {
                GameObject holder = new GameObject(row.id);
                holder.SetActive(false);
                GameObject start = new GameObject("start");
                GameObject end = new GameObject("end");
                start.transform.SetParent(holder.transform);
                end.transform.SetParent(holder.transform);
                start.transform.position = row.start;
                end.transform.position = row.end;
                OffMeshLink link = holder.AddComponent<OffMeshLink>();
                link.startTransform = start.transform;
                link.endTransform = end.transform;
                link.biDirectional = row.bidirectional;
                link.area = row.area;
                link.activated = true;
                link.autoUpdatePositions = false;
                holder.SetActive(true);
            }
            // Reopen with scene-native data and serialized links, without a loader.
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), scenePath);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.OpenScene(scenePath);
            HashSet<string> failedRaw = new HashSet<string>();
            foreach (Link row in manifest.links)
            {
                result.rawLinkPaths++;
                if (Complete(row.start, row.end)) result.rawLinksComplete++;
                else
                {
                    failedRaw.Add(row.id);
                    result.linkWarnings.Add("Original endpoint route unavailable: " + row.id);
                }
                if (row.bidirectional)
                {
                    result.rawLinkPaths++;
                    if (Complete(row.end, row.start)) result.rawLinksComplete++;
                    else result.linkWarnings.Add("Original reverse route unavailable: " + row.id);
                }
                if (!row.sourceAttached)
                {
                    result.sourceUnattachedLinks++;
                    result.linkWarnings.Add("Unattached in original baked runtime graph: " + row.id);
                }
            }
            foreach (Link row in manifest.links)
            {
                GameObject holder = GameObject.Find(row.id);
                OffMeshLink link = holder.GetComponent<OffMeshLink>();
                if (row.sourceAttached)
                {
                    if (failedRaw.Contains(row.id))
                    {
                        Selection.activeGameObject = holder;
                        RestoreBakedLinkEndpoints.Restore();
                        if (Vector3.Distance(link.startTransform.position, row.bakedStart) > 0.0001f || Vector3.Distance(link.endTransform.position, row.bakedEnd) > 0.0001f)
                            throw new InvalidOperationException("Baked endpoint editor repair failed: " + row.id);
                        result.repairedSceneLinks++;
                    }
                    else
                    {
                        link.startTransform.position = row.bakedStart;
                        link.endTransform.position = row.bakedEnd;
                    }
                    link.UpdatePositions();
                }
            }
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), scenePath);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.OpenScene(scenePath);
            foreach (Link row in manifest.links)
            {
                if (!row.sourceAttached) continue;
                result.linkPaths++;
                if (Complete(row.bakedStart, row.bakedEnd)) result.linksComplete++;
                else
                {
                    NavMeshHit a;
                    NavMeshHit b;
                    bool foundA = NavMesh.SamplePosition(row.start, out a, 2f, NavMesh.AllAreas);
                    bool foundB = NavMesh.SamplePosition(row.end, out b, 2f, NavMesh.AllAreas);
                    result.failures.Add("Link " + row.id + ": endpoints " + foundA + "/" + foundB + ", distances " + Vector3.Distance(a.position, row.start) + "/" + Vector3.Distance(b.position, row.end) + ", hits " + a.position + "/" + b.position);
                }
                if (row.bidirectional)
                {
                    result.linkPaths++;
                    if (Complete(row.bakedEnd, row.bakedStart)) result.linksComplete++;
                    else result.failures.Add("Reverse link " + row.id);
                }
            }
            result.passed = result.triangles == result.expectedTriangles && result.detailedFound == result.detailedSamples && result.adjacentComplete == result.adjacentPaths && result.linksComplete == result.linkPaths;
        }
        catch (Exception error)
        {
            result.error = error.ToString();
        }
        finally
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }
        return result;
    }
}
