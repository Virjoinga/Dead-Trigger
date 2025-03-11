#define DEBUG
using System;
using System.Collections.Generic;
using LRUCache;
using UnityEngine;

[AddComponentMenu("Image Effects/MADFINGER screen space vertex grid FX")]
public class MFScreenSpaceVertexGridFX : ImageEffectBase
{
	private struct S_CacheRec
	{
		public bool m_IsBlocked;

		public float m_QueryTime;
	}

	private struct S_Flashbang
	{
		public Color m_Color;

		public float m_StartTime;

		public float m_Intensity;

		public float m_Duration;
	}

	private const int MAX_GLOWS = 4;

	private int m_ScreenGridXRes = 30;

	private int m_ScreenGridYRes = 25;

	private MeshFilter m_MeshFilter;

	private MeshRenderer m_MeshRenderer;

	private Mesh m_Mesh;

	private bool m_InitOK;

	private GameObject m_GameObj;

	private ScreenSpaceGlowEmitter[] m_ActiveGlows = new ScreenSpaceGlowEmitter[4];

	private int m_NumActiveGlows;

	private Vector4 m_GlowsIntensityMask;

	public float m_DirFadeoutStrength = 80f;

	public float m_MaxVisQueryResultAge = 0.25f;

	public float m_FlashbangPeakTime = 0.5f;

	public float m_FlashbangDuration = 6f;

	private List<S_Flashbang> m_Flashbangs = new List<S_Flashbang>();

	private LRUCache<int, S_CacheRec> m_GlowsVisCache = new LRUCache<int, S_CacheRec>(128);

	private bool m_PrevDbgBtnState;

	public static MFScreenSpaceVertexGridFX Instance;

	private void Awake()
	{
		Instance = this;
		m_InitOK = DoInit();
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	public void InternalUpdate()
	{
		SelectActiveGlows();
	}

	public int NumActiveGlows()
	{
		return m_NumActiveGlows;
	}

	public bool AnyEffectActive()
	{
		return m_NumActiveGlows > 0 || m_Flashbangs.Count > 0;
	}

	private void SelectActiveGlows()
	{
		m_NumActiveGlows = 0;
		if (Camera.main == null)
		{
			return;
		}
		Camera main = Camera.main;
		Vector3 position = main.transform.position;
		S_CacheRec retVal = default(S_CacheRec);
		float time = Time.time;
		foreach (ScreenSpaceGlowEmitter ms_Instance in ScreenSpaceGlowEmitter.ms_Instances)
		{
			Vector3 position2 = ms_Instance.transform.position;
			if (Vector3.Distance(position, position2) > ms_Instance.m_MaxVisDist)
			{
				continue;
			}
			Vector3 vector = main.WorldToViewportPoint(position2);
			if (vector.z < 0f || vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
			{
				continue;
			}
			if (m_NumActiveGlows >= 4)
			{
				break;
			}
			bool flag = false;
			bool flag2 = true;
			if (m_GlowsVisCache.get(ms_Instance.m_InstanceID, ref retVal))
			{
				float num = time - retVal.m_QueryTime;
				DebugUtils.Assert(num >= 0f);
				if (num <= m_MaxVisQueryResultAge)
				{
					flag = retVal.m_IsBlocked;
					flag2 = false;
				}
			}
			if (flag2)
			{
				Vector3 direction = position2 - position;
				flag = (retVal.m_IsBlocked = Physics.Raycast(position, direction, 1f, ms_Instance.m_ColLayerMask));
				retVal.m_QueryTime = time;
				m_GlowsVisCache.add(ms_Instance.m_InstanceID, retVal);
			}
			if (!flag)
			{
				Vector3 lhs = position - position2;
				Vector3 forward = ms_Instance.transform.forward;
				float magnitude = lhs.magnitude;
				lhs.Normalize();
				float num2 = RemapValue(Mathf.Clamp01(Vector3.Dot(lhs, forward)), Mathf.Cos(ms_Instance.m_ConeAngle * ((float)Math.PI / 180f) / 2f), 1f, 0f, 1f);
				float num3 = Mathf.Clamp01(magnitude / ms_Instance.m_MaxVisDist);
				float num4 = num2 * (1f - num3 * num3);
				if (num4 > 0.001f)
				{
					m_ActiveGlows[m_NumActiveGlows++] = ms_Instance;
				}
			}
		}
	}

	private void SetGlowShaderParams(int glowIdx, ScreenSpaceGlowEmitter glowInfo, Vector3 camDir, Vector3 camPos)
	{
		Vector3 forward = glowInfo.transform.forward;
		Vector3 position = glowInfo.transform.position;
		Vector4 v = position;
		Vector4 v2 = glowInfo.m_Color * glowInfo.m_Intensity * Mathf.Pow(Mathf.Clamp01(Vector3.Dot(-camDir, forward)), m_DirFadeoutStrength);
		Matrix4x4 zero = Matrix4x4.zero;
		v.w = glowInfo.m_Radius;
		Vector3 lhs = camPos - position;
		float magnitude = lhs.magnitude;
		lhs.Normalize();
		float num = RemapValue(Mathf.Clamp01(Vector3.Dot(lhs, forward)), Mathf.Cos(glowInfo.m_ConeAngle * ((float)Math.PI / 180f) / 2f), 1f, 0f, 1f);
		float num2 = Mathf.Clamp01(magnitude / glowInfo.m_MaxVisDist);
		m_GlowsIntensityMask[glowIdx] = num * (1f - num2 * num2);
		zero.SetRow(0, v);
		zero.SetRow(1, v2);
		zero.SetRow(2, forward);
		switch (glowIdx)
		{
		case 0:
			base.material.SetMatrix("_Glow0Params", zero);
			break;
		case 1:
			base.material.SetMatrix("_Glow1Params", zero);
			break;
		case 2:
			base.material.SetMatrix("_Glow2Params", zero);
			break;
		case 3:
			base.material.SetMatrix("_Glow3Params", zero);
			break;
		default:
			DebugUtils.Assert(false);
			break;
		}
	}

	private void SetupActiveGlowsShaderParams(Vector3 camDir, Vector3 camPos)
	{
		for (int i = 0; i < m_NumActiveGlows; i++)
		{
			SetGlowShaderParams(i, m_ActiveGlows[i], camDir, camPos);
		}
	}

	private void ResetMaterialGlowParams()
	{
		base.material.SetMatrix("_Glow0Params", Matrix4x4.zero);
		base.material.SetMatrix("_Glow1Params", Matrix4x4.zero);
		base.material.SetMatrix("_Glow2Params", Matrix4x4.zero);
		base.material.SetMatrix("_Glow3Params", Matrix4x4.zero);
		m_GlowsIntensityMask = Vector3.zero;
	}

	private float Impulse(float k, float x)
	{
		float num = k * x;
		return Mathf.Clamp01(num * Mathf.Exp(1f - num));
	}

	private float FlashBangFunc(float t, float tPeak, float tDuration)
	{
		float num = tDuration * 0.25f;
		float num2 = 1f / tPeak;
		float num3 = -1f / (tDuration - num);
		float a = t * num2;
		float b = tDuration / (tDuration - num) + t * num3;
		return Mathf.Clamp01(Mathf.Min(a, b));
	}

	private Vector4 CalcGlobalColor()
	{
		Vector4 zero = Vector4.zero;
		int count = m_Flashbangs.Count;
		float time = Time.time;
		for (int num = count - 1; num >= 0; num--)
		{
			S_Flashbang s_Flashbang = m_Flashbangs[num];
			float num2 = time - s_Flashbang.m_StartTime;
			float num3 = FlashBangFunc(num2, m_FlashbangPeakTime, s_Flashbang.m_Duration) * s_Flashbang.m_Intensity;
			if (num2 > m_FlashbangDuration)
			{
				m_Flashbangs.RemoveAt(num);
			}
			else
			{
				zero.x += num3 * s_Flashbang.m_Color.r;
				zero.y += num3 * s_Flashbang.m_Color.g;
				zero.z += num3 * s_Flashbang.m_Color.b;
			}
		}
		return zero;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!m_InitOK || !Camera.main)
		{
			Graphics.Blit(source, destination);
			Debug.LogError("Screen space vertex grid FX not initialized correctly");
			return;
		}
		Camera main = Camera.main;
		Vector3 camDir = -main.cameraToWorldMatrix.GetRow(2);
		Vector3 position = main.transform.position;
		ResetMaterialGlowParams();
		SetupActiveGlowsShaderParams(camDir, position);
		Matrix4x4 matrix4x = main.projectionMatrix * main.worldToCameraMatrix;
		Vector4 vector = CalcGlobalColor();
		vector.w = ((source.texelSize.y < 0f) ? 1 : 0);
		base.material.SetMatrix("_UnprojectTM", matrix4x.inverse);
		base.material.SetVector("_GlowsIntensityMask", m_GlowsIntensityMask);
		base.material.SetVector("_GlobalColor", vector);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = destination;
		base.material.mainTexture = source;
		if (base.material.SetPass(0))
		{
			Graphics.DrawMeshNow(m_Mesh, Matrix4x4.identity);
		}
		else
		{
			Debug.LogError("Unable to set material pass");
		}
		RenderTexture.active = active;
	}

	private bool DoInit()
	{
		m_GameObj = new GameObject();
		shader = Shader.Find("MADFINGER/PostFX/ScreenSpaceLightFX");
		if (!shader)
		{
			Debug.LogError("Unable to get ScreenSpaceLightFX shader");
		}
		DebugUtils.Assert(shader);
		if (!InitMeshes())
		{
			return false;
		}
		m_GameObj.SetActive(false);
		return true;
	}

	private bool InitMeshes()
	{
		DebugUtils.Assert(m_ScreenGridXRes > 1);
		DebugUtils.Assert(m_ScreenGridYRes > 1);
		m_GameObj.AddComponent<MeshFilter>();
		m_GameObj.AddComponent<MeshRenderer>();
		m_MeshFilter = (MeshFilter)m_GameObj.GetComponent(typeof(MeshFilter));
		m_MeshRenderer = (MeshRenderer)m_GameObj.GetComponent(typeof(MeshRenderer));
		DebugUtils.Assert(m_MeshFilter);
		DebugUtils.Assert(m_MeshRenderer);
		m_MeshRenderer.GetComponent<Renderer>().material = base.material;
		m_MeshRenderer.GetComponent<Renderer>().enabled = true;
		m_MeshRenderer.GetComponent<Renderer>().castShadows = false;
		m_MeshRenderer.GetComponent<Renderer>().receiveShadows = false;
		m_Mesh = m_MeshFilter.mesh;
		int num = m_ScreenGridXRes * m_ScreenGridYRes;
		int num2 = (m_ScreenGridXRes - 1) * (m_ScreenGridYRes - 1) * 2;
		Vector3[] array = new Vector3[num];
		Vector2[] array2 = new Vector2[num];
		int[] array3 = new int[num2 * 3];
		for (int i = 0; i < m_ScreenGridYRes; i++)
		{
			for (int j = 0; j < m_ScreenGridXRes; j++)
			{
				int num3 = i * m_ScreenGridXRes + j;
				array[num3].x = (float)j / (float)(m_ScreenGridXRes - 1);
				array[num3].y = (float)i / (float)(m_ScreenGridYRes - 1);
				array[num3].z = 0f;
				array2[num3].x = array[num3].x;
				array2[num3].y = array[num3].y;
			}
		}
		int num4 = 0;
		for (int k = 0; k < m_ScreenGridYRes - 1; k++)
		{
			for (int l = 0; l < m_ScreenGridXRes - 1; l++)
			{
				int num5 = l + k * m_ScreenGridXRes;
				int num6 = l + 1 + k * m_ScreenGridXRes;
				int num7 = l + 1 + (k + 1) * m_ScreenGridXRes;
				int num8 = l + (k + 1) * m_ScreenGridXRes;
				array3[num4++] = num8;
				array3[num4++] = num6;
				array3[num4++] = num5;
				array3[num4++] = num8;
				array3[num4++] = num7;
				array3[num4++] = num6;
			}
		}
		m_Mesh.vertices = array;
		m_Mesh.uv = array2;
		m_Mesh.triangles = array3;
		m_Mesh.name = "screenspace grid";
		return true;
	}

	private float RemapValue(float v, float r0, float r1, float p0, float p1)
	{
		v = Mathf.Max(v, r0);
		v = Mathf.Min(v, r1);
		return p0 + (p1 - p0) * (v - r0) / (r1 - r0);
	}

	public void SpawnFlashbang(Color col, float intensity)
	{
		S_Flashbang item = default(S_Flashbang);
		item.m_Color = col;
		item.m_Intensity = intensity;
		item.m_StartTime = Time.time;
		item.m_Duration = m_FlashbangDuration;
		m_Flashbangs.Add(item);
	}

	public void SpawnFlashbang(Color col, float intensity, float duration)
	{
		S_Flashbang item = default(S_Flashbang);
		item.m_Color = col;
		item.m_Intensity = intensity;
		item.m_StartTime = Time.time;
		item.m_Duration = duration;
		m_Flashbangs.Add(item);
	}

	public int NumActiveFlashbangs()
	{
		return m_Flashbangs.Count;
	}

	private void DbgSpawnFlashbang()
	{
		bool button = Input.GetButton("Fire1");
		if (button && !m_PrevDbgBtnState)
		{
			SpawnFlashbang(Color.white, 1f);
		}
		m_PrevDbgBtnState = button;
	}
}
