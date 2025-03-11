#define DEBUG
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Image Effects/MADFINGER explosion FX")]
public class MFExplosionPostFX : ImageEffectBase
{
	private class WaveEmitter
	{
		public Vector2 m_Center;

		public float m_Amplitude;

		public float m_Frequency;

		public float m_Speed;

		public float m_DistAtt;

		public float m_Time;

		public float m_Duration;

		public int m_SlotIdx;
	}

	public struct S_WaveParams
	{
		public float m_Amplitude;

		public float m_Freq;

		public float m_Speed;

		public float m_Duration;

		public float m_Radius;

		public float m_Delay;
	}

	private int m_ScreenGridXRes = 30;

	private int m_ScreenGridYRes = 25;

	private MeshFilter m_MeshFilter;

	private MeshRenderer m_MeshRenderer;

	private Mesh m_Mesh;

	private int m_MaxWaves = 4;

	private bool m_InitOK;

	private bool m_PrevDbgBtnState;

	private GameObject m_GameObj;

	public float m_WaveAmplitude = 0.3f;

	public float m_WaveFreq = 20f;

	public float m_WaveSpeed = 1.4f;

	public float m_WaveDuration = 1.5f;

	public float m_WaveMaxRadius = 1f;

	public float m_ColorizationIntensity = 1.5f;

	public float m_TimeScale = 1f;

	public Color m_Color0 = Color.white;

	public Color m_Color1 = new Color(0.5f, 0.3f, 0f, 1f);

	public static MFExplosionPostFX Instance;

	private List<WaveEmitter> m_ActiveWaves = new List<WaveEmitter>();

	private Stack<int> m_FreeWaveEmitterSlots = new Stack<int>();

	private void Awake()
	{
		if (Instance == null)
		{
			m_InitOK = DoInit();
			Instance = this;
		}
		else
		{
			Debug.LogWarning("There should be only one instance of MFExplosionPostFX attached to camera");
		}
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	private void LateUpdate()
	{
		UpdateEmitters();
	}

	public void Reset()
	{
		m_ActiveWaves.Clear();
		m_FreeWaveEmitterSlots.Clear();
		for (int num = m_MaxWaves - 1; num >= 0; num--)
		{
			m_FreeWaveEmitterSlots.Push(num);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!m_InitOK)
		{
			Graphics.Blit(source, destination);
			Debug.LogError("SlowTime PostFX subsystem not initialized correctly");
			return;
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = destination;
		Vector4 zero = Vector4.zero;
		Vector4 zero2 = Vector4.zero;
		zero.x = 0.5f / (float)source.width;
		zero.y = 0.5f / (float)source.height;
		zero.z = 1f;
		zero.w = (float)source.height / (float)source.width;
		zero2.x = m_ColorizationIntensity;
		for (int i = 0; i < m_MaxWaves; i++)
		{
			SetWaveShaderParams(i, Vector4.zero, Vector4.zero);
		}
		base.material.SetVector("_Params", zero2);
		base.material.SetVector("_Color0", m_Color0);
		base.material.SetVector("_Color1", m_Color1);
		base.material.mainTexture = source;
		base.material.SetVector("_UVOffsAndAspectScale", zero);
		foreach (WaveEmitter activeWave in m_ActiveWaves)
		{
			SetupWaveShaderParams(activeWave);
		}
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
		shader = Shader.Find("MADFINGER/PostFX/ExplosionFX");
		if (!shader)
		{
			Debug.LogError("Unable to get ExplosionFX shader");
		}
		DebugUtils.Assert(shader);
		if (!InitMeshes())
		{
			return false;
		}
		Reset();
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

	private void UpdateEmitters()
	{
		float num = Time.deltaTime * m_TimeScale;
		for (int num2 = m_ActiveWaves.Count - 1; num2 >= 0; num2--)
		{
			WaveEmitter waveEmitter = m_ActiveWaves[num2];
			waveEmitter.m_Time += num;
			if (waveEmitter.m_Time > waveEmitter.m_Duration)
			{
				m_FreeWaveEmitterSlots.Push(waveEmitter.m_SlotIdx);
				m_ActiveWaves.RemoveAt(num2);
			}
		}
	}

	private void SetWaveShaderParams(int slotIdx, Vector4 paramSet0, Vector4 paramSet1)
	{
		switch (slotIdx)
		{
		case 0:
			base.material.SetVector("_Wave0ParamSet0", paramSet0);
			base.material.SetVector("_Wave0ParamSet1", paramSet1);
			break;
		case 1:
			base.material.SetVector("_Wave1ParamSet0", paramSet0);
			base.material.SetVector("_Wave1ParamSet1", paramSet1);
			break;
		case 2:
			base.material.SetVector("_Wave2ParamSet0", paramSet0);
			base.material.SetVector("_Wave2ParamSet1", paramSet1);
			break;
		case 3:
			base.material.SetVector("_Wave3ParamSet0", paramSet0);
			base.material.SetVector("_Wave3ParamSet1", paramSet1);
			break;
		default:
			DebugUtils.Assert(false);
			break;
		}
	}

	private void SetupWaveShaderParams(WaveEmitter emitter)
	{
		Vector4 zero = Vector4.zero;
		Vector4 zero2 = Vector4.zero;
		zero.x = emitter.m_Center.x;
		zero.y = emitter.m_Center.y;
		zero.z = emitter.m_Amplitude;
		zero.w = emitter.m_Frequency;
		zero2.x = emitter.m_DistAtt;
		zero2.y = emitter.m_Speed;
		zero2.z = emitter.m_Time;
		zero2.w = 1f;
		SetWaveShaderParams(emitter.m_SlotIdx, zero, zero2);
	}

	public void EmitGrenadeExplosionWave(Vector2 normScreenPos, S_WaveParams waveParams)
	{
		if (m_FreeWaveEmitterSlots.Count == 0)
		{
			Debug.LogWarning("Out of free wave-emitter slots");
			return;
		}
		int slotIdx = m_FreeWaveEmitterSlots.Pop();
		WaveEmitter waveEmitter = new WaveEmitter();
		waveEmitter.m_Center = normScreenPos;
		waveEmitter.m_Amplitude = waveParams.m_Amplitude;
		waveEmitter.m_Frequency = waveParams.m_Freq;
		waveEmitter.m_Speed = waveParams.m_Speed;
		waveEmitter.m_Time = 0f - waveParams.m_Delay;
		waveEmitter.m_Duration = waveParams.m_Duration + waveParams.m_Delay;
		waveEmitter.m_DistAtt = 1f / waveParams.m_Radius;
		waveEmitter.m_SlotIdx = slotIdx;
		m_ActiveWaves.Add(waveEmitter);
	}

	public int NumActiveEffects()
	{
		return m_ActiveWaves.Count;
	}

	private void DbgEmitWave()
	{
		Debug.Log("aaaaaaaaaaaa");
		bool button = Input.GetButton("Fire1");
		if (button && !m_PrevDbgBtnState)
		{
			Vector2 vector = default(Vector2);
			vector.x = 0.5f;
			vector.y = 0.5f;
			S_WaveParams waveParams = default(S_WaveParams);
			waveParams.m_Amplitude = m_WaveAmplitude;
			waveParams.m_Duration = m_WaveDuration;
			waveParams.m_Freq = m_WaveFreq;
			waveParams.m_Radius = m_WaveMaxRadius;
			waveParams.m_Speed = m_WaveSpeed;
			waveParams.m_Delay = 0f;
			Debug.Log("EmitGrenadeExplosion " + vector);
			EmitGrenadeExplosionWave(vector, waveParams);
		}
		m_PrevDbgBtnState = button;
	}
}
