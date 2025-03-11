using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Effects/Screen space glow")]
[ExecuteInEditMode]
public class ScreenSpaceGlowEmitter : MonoBehaviour
{
	public enum GlowType
	{
		Point = 0,
		Spot = 1
	}

	public GlowType m_GlowType;

	public Color m_Color = new Color(1f, 1f, 1f, 1f);

	public float m_Intensity = 1f;

	public float m_Radius = 5f;

	public float m_MaxVisDist = 30f;

	public float m_ConeAngle = 30f;

	public int m_InstanceID = -1;

	public LayerMask m_ColLayerMask = -1;

	public static HashSet<ScreenSpaceGlowEmitter> ms_Instances = new HashSet<ScreenSpaceGlowEmitter>();

	private static int ms_InstCnt = 0;

	private void Awake()
	{
		m_InstanceID = ms_InstCnt++;
		ms_Instances.Add(this);
	}

	private void OnDestroy()
	{
		ms_Instances.Remove(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.position, 0.25f);
	}
}
