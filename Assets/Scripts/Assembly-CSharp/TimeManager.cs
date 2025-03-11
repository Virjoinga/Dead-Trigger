using UnityEngine;

public class TimeManager : MonoBehaviour
{
	private static TimeManager m_Instance;

	private float m_DebugOverrideTimeScale = -1f;

	private float m_TimeSinceLevelLoad;

	private bool m_UseScreenEffect;

	private bool m_SlowTimeSndPlayed;

	private float m_TimeScale = 1f;

	private float m_TimeScaleStart;

	private float m_Timer = -1f;

	private float m_BlendIn;

	private float m_BlendOut;

	private float m_Duration;

	private float m_StoredTime = 1f;

	private bool m_IsPaused;

	private float m_OriginalFixedTimeDelta = 0.033f;

	public static TimeManager Instance
	{
		get
		{
			return m_Instance;
		}
	}

	public float TimeScale
	{
		get
		{
			return m_TimeScale;
		}
		set
		{
			SetTimeScaleInternal(value, 0f, 0f, 0f);
		}
	}

	public float timeSinceLevelLoad
	{
		get
		{
			return m_TimeSinceLevelLoad;
		}
	}

	private TimeManager()
	{
	}

	public void SetTimeScale(float scale, float blendin = 0f, float blendout = 0f, float duration = 0f, bool useScreenEffect = false)
	{
		m_UseScreenEffect = useScreenEffect;
		if (Mathf.Approximately(scale, 1f))
		{
			m_UseScreenEffect = false;
		}
		m_SlowTimeSndPlayed = false;
		if (m_UseScreenEffect)
		{
			BeginSlowTimeFx();
			if ((bool)Player.Instance && (bool)Player.Instance.Owner)
			{
				Player.Instance.Owner.SoundPlay(Player.Instance.SoundSetup.TimeWarpIn);
			}
		}
		SetTimeScaleInternal(scale, blendin, blendout, duration);
	}

	public void DebugOverrideTimeScale(float scale, bool set)
	{
		if (set)
		{
			m_DebugOverrideTimeScale = scale;
		}
		else
		{
			m_DebugOverrideTimeScale = -1f;
		}
	}

	public void PauseTime()
	{
		m_IsPaused = true;
		m_StoredTime = Time.timeScale;
		Time.timeScale = 0f;
	}

	public void UnpauseTime()
	{
		m_IsPaused = false;
		Time.timeScale = m_StoredTime;
	}

	public float GetRealDeltaTime()
	{
		if (m_IsPaused)
		{
			return 0f;
		}
		return (!(Time.timeScale > 0f)) ? 0f : (Time.deltaTime * (1f / Time.timeScale));
	}

	private void Awake()
	{
		m_Instance = this;
	}

	private void Update()
	{
		if (m_IsPaused)
		{
			return;
		}
		m_TimeSinceLevelLoad += GetRealDeltaTime();
		if (m_Timer >= 0f)
		{
			m_Timer += GetRealDeltaTime();
			float num = 0f;
			if (m_UseScreenEffect && !m_SlowTimeSndPlayed && m_Timer >= m_BlendIn + m_Duration + m_BlendOut - 1f)
			{
				m_SlowTimeSndPlayed = true;
				if ((bool)Player.Instance && (bool)Player.Instance.Owner)
				{
					Player.Instance.Owner.SoundPlay(Player.Instance.SoundSetup.TimeWarpOut);
				}
			}
			if (!(m_Timer > m_BlendIn + m_Duration))
			{
				num = ((!Mathf.Approximately(m_BlendIn, 0f)) ? (m_TimeScaleStart + (m_TimeScale - m_TimeScaleStart) * Mathf.Clamp(m_Timer / m_BlendIn, 0f, 1f)) : m_TimeScale);
			}
			else if (m_Duration == 0f)
			{
				num = m_TimeScale;
			}
			else
			{
				if (m_Timer >= m_BlendIn + m_Duration + m_BlendOut || Mathf.Approximately(m_BlendOut, 0f))
				{
					Time.timeScale = m_TimeScaleStart;
					m_Timer = -1f;
					return;
				}
				num = m_TimeScale + (m_TimeScaleStart - m_TimeScale) * Mathf.Clamp((m_Timer - m_BlendIn - m_Duration) / m_BlendOut, 0f, 1f);
			}
			Time.timeScale = num;
			Time.fixedDeltaTime = m_OriginalFixedTimeDelta * num;
		}
		if (m_DebugOverrideTimeScale >= 0f)
		{
			Time.timeScale = m_DebugOverrideTimeScale;
			Time.fixedDeltaTime = m_OriginalFixedTimeDelta * m_DebugOverrideTimeScale;
		}
		if ((bool)BloodFXManager.Instance)
		{
			if (m_UseScreenEffect)
			{
				float num2 = ((!(Time.timeScale > 0.95f)) ? 0.7f : 1f);
				BloodFXManager.Instance.SetSlomoEffectNormalized(Mathf.Clamp(Time.timeScale * num2, 0.05f, 0.8f));
			}
			else
			{
				BloodFXManager.Instance.SetSlomoEffectNormalized(1f);
			}
		}
	}

	private void SetTimeScaleInternal(float scale, float blendin = 0f, float blendout = 0f, float duration = 0f)
	{
		m_BlendIn = blendin;
		m_BlendOut = blendout;
		m_Duration = duration;
		if (m_IsPaused)
		{
			m_TimeScaleStart = m_StoredTime;
		}
		else if (m_Timer < 0f)
		{
			m_TimeScaleStart = Time.timeScale;
		}
		m_Timer = 0f;
		m_TimeScale = scale;
	}

	private void BeginSlowTimeFx()
	{
		Vector2 normScreenPos = default(Vector2);
		normScreenPos.x = 0.5f;
		normScreenPos.y = 0.5f;
		MFSlowTimeFX.S_WaveParams waveParams = default(MFSlowTimeFX.S_WaveParams);
		waveParams.m_Amplitude = 0.75f;
		waveParams.m_Duration = 2f;
		waveParams.m_Freq = 10f;
		waveParams.m_Radius = 1f;
		waveParams.m_Speed = 1.5f;
		waveParams.m_Delay = 0f;
		if ((bool)MFSlowTimeFX.Instance)
		{
			MFSlowTimeFX.Instance.m_TimeScale = 4f;
			MFSlowTimeFX.Instance.m_ColorizationIntensity = 0.25f;
			MFSlowTimeFX.Instance.EmitGrenadeExplosionWave(normScreenPos, waveParams);
		}
	}
}
