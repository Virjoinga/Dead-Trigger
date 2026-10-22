using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyLinkDiagnostics
{
    [MenuItem("Tools/Navigation/Diagnose Selected Enemy Link")]
    public static void Diagnose()
    {
        if (!Application.isPlaying || Selection.activeGameObject == null)
        {
            Debug.LogWarning("In Play mode, select an enemy that is stopped at a link.");
            return;
        }
        NavMeshAgent agent = Selection.activeGameObject.GetComponentInParent<NavMeshAgent>();
        if (agent == null) agent = Selection.activeGameObject.GetComponentInChildren<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning("Selected object has no NavMeshAgent in its parents or children.");
            return;
        }
        StringBuilder message = new StringBuilder("Enemy link diagnostic: " + agent.name);
        message.Append("\nAgent enabled=" + agent.enabled + ", on NavMesh=" + agent.isOnNavMesh + ", position=" + agent.transform.position);
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            Debug.LogWarning(message.ToString(), agent);
            return;
        }
        message.Append("\nOn link=" + agent.isOnOffMeshLink + ", auto traverse=" + agent.autoTraverseOffMeshLink + ", update position=" + agent.updatePosition + ", stopped=" + agent.isStopped);
        message.Append("\nPath pending=" + agent.pathPending + ", has path=" + agent.hasPath + ", status=" + agent.pathStatus + ", remaining=" + agent.remainingDistance);
        message.Append("\nSpeed=" + agent.speed + ", velocity=" + agent.velocity + ", base offset=" + agent.baseOffset + ", area mask=" + agent.areaMask + ", agent type=" + agent.agentTypeID);
        AgentHuman owner = agent.GetComponent<AgentHuman>();
        AnimComponent animation = agent.GetComponent<AnimComponent>();
        if (owner != null) message.Append("\nActionPointOn=" + owner.IsActionPointOn);
        if (animation != null) message.Append("\nAnimation state=" + (animation.CurrentAnimState == null ? "none" : animation.CurrentAnimState.GetType().Name));
        OffMeshLinkData data = agent.isOnOffMeshLink ? agent.currentOffMeshLinkData : agent.nextOffMeshLinkData;
        message.Append("\nLink data valid=" + data.valid + ", activated=" + data.activated);
        if (data.valid)
        {
            message.Append("\nLink start=" + data.startPos + ", end=" + data.endPos + ", distance to start=" + Vector3.Distance(agent.transform.position, data.startPos));
            OffMeshLink link = data.offMeshLink;
            if (link != null)
            {
                message.Append("\nLink=" + link.name + ", enabled=" + link.isActiveAndEnabled + ", activated=" + link.activated + ", occupied=" + link.occupied + ", area=" + link.area);
                ActionPoint point = link.GetComponent<ActionPoint>();
                message.Append("\nActionPoint present=" + (point != null));
                if (point != null)
                {
                    message.Append(", enabled=" + point.enabled);
                    message.Append("\nWalk clip=" + ClipInfo(point.m_AnimMove) + ", run clip=" + ClipInfo(point.m_AnimRun));
                }
            }
        }
        Debug.Log(message.ToString(), agent);
    }

    private static string ClipInfo(ActionPoint.AnimData animation)
    {
        if (animation == null) return "missing animation settings";
        if (animation.m_Anim == null) return "MISSING CLIP (speed=" + animation.m_MoveSpeed + ")";
        return animation.m_Anim.name + " (length=" + animation.m_Anim.length + ", speed=" + animation.m_MoveSpeed + ")";
    }
}
