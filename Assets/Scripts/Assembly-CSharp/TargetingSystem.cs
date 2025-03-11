using System;
using UnityEngine;

[AddComponentMenu("Game/Targeting System")]
public class TargetingSystem : MonoBehaviour
{
	public enum E_Dir
	{
		Current = 0,
		Initial = 1,
		MinH = 2,
		MaxH = 3,
		MinV = 4,
		MaxV = 5
	}

	private enum E_State
	{
		Idle = 0,
		Starting = 1,
		InProgress = 2,
		Stopping = 3
	}

	[Serializable]
	public class PartsSettings
	{
		public Transform m_Base;

		public Transform m_Hinge;
	}

	[Serializable]
	public class RotationSettings
	{
		public float m_HRange = 140f;

		public float m_HSpeed = 40f;

		public float m_VRange = 80f;

		public float m_VSpeed = 30f;
	}

	[Serializable]
	public class SoundsSettings
	{
		public AudioSource m_Source;

		public AudioClip m_RotStart;

		public AudioClip m_RotLoop;

		public AudioClip m_RotStop;
	}

	private const float InToleranceThreshold = (float)Math.PI / 180f;

	public PartsSettings m_PartsSettings = new PartsSettings();

	public RotationSettings m_RotSettings = new RotationSettings();

	public SoundsSettings m_SndSettings = new SoundsSettings();

	private E_State m_State;

	private bool m_InTolerance;

	private float m_TimerInTolerance;

	private float m_TimerNotInTolerance;

	private Rotator m_HMotor;

	private Rotator m_VMotor;

	private Vector3[] m_Dir = new Vector3[6];

	public Vector3 Dir
	{
		get
		{
			return m_Dir[0];
		}
	}

	public bool DirInTolerance
	{
		get
		{
			return m_InTolerance;
		}
	}

	public Vector3 Origin
	{
		get
		{
			return m_PartsSettings.m_Hinge.position;
		}
	}

	private void Start()
	{
		if (m_SndSettings.m_Source != null)
		{
			m_SndSettings.m_Source = base.GetComponent<AudioSource>();
			m_SndSettings.m_Source.playOnAwake = false;
			m_SndSettings.m_Source.loop = true;
		}
		m_State = E_State.Idle;
		m_InTolerance = true;
		m_TimerInTolerance = 0f;
		m_TimerNotInTolerance = 0f;
		ComputeDirs();
		float AngleH = 0f;
		float AngleV = 0f;
		Vector3 dir = GetDir(E_Dir.Current);
		MathUtils.VectorToAngles(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, dir, ref AngleH, ref AngleV);
		m_HMotor = new Rotator(AngleH, m_RotSettings.m_HRange * ((float)Math.PI / 180f), m_RotSettings.m_HSpeed * ((float)Math.PI / 180f));
		m_VMotor = new Rotator(AngleV, m_RotSettings.m_VRange * ((float)Math.PI / 180f), m_RotSettings.m_VSpeed * ((float)Math.PI / 180f));
	}

	private void OnDestroy()
	{
		m_HMotor = null;
		m_VMotor = null;
	}

	private void Update()
	{
		m_InTolerance = Mathf.Max(m_HMotor.AbsDiff, m_VMotor.AbsDiff) <= (float)Math.PI / 180f;
		bool flag = false;
		if (m_InTolerance)
		{
			m_TimerInTolerance += Time.deltaTime;
			m_TimerNotInTolerance = 0f;
			flag = m_TimerInTolerance <= 0.1f;
			flag &= m_SndSettings.m_Source == null || m_SndSettings.m_Source.isPlaying;
		}
		else
		{
			m_TimerInTolerance = 0f;
			m_TimerNotInTolerance += Time.deltaTime;
			flag = m_TimerNotInTolerance >= 0.1f;
		}
		m_HMotor.Update(Time.deltaTime);
		m_VMotor.Update(Time.deltaTime);
		Vector3 vector = MathUtils.AnglesToVector(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, m_HMotor.Angle, m_VMotor.Angle);
		m_PartsSettings.m_Hinge.rotation = Quaternion.LookRotation(vector, m_PartsSettings.m_Base.up);
		m_Dir[0] = vector;
		E_State state = m_State;
		if (flag)
		{
			if (m_State == E_State.Idle || m_State == E_State.Stopping)
			{
				ChangeState(E_State.Starting);
			}
		}
		else if (m_State == E_State.Starting || m_State == E_State.InProgress)
		{
			ChangeState(E_State.Stopping);
		}
		if (state == m_State && (m_SndSettings.m_Source == null || !m_SndSettings.m_Source.isPlaying))
		{
			if (m_State == E_State.Starting)
			{
				ChangeState(E_State.InProgress);
			}
			else if (m_State == E_State.Stopping)
			{
				ChangeState(E_State.Idle);
			}
		}
	}

	private void ChangeState(E_State NewState)
	{
		if (m_SndSettings.m_Source != null)
		{
			float num = 0f;
			if (m_SndSettings.m_Source.isPlaying && m_State != E_State.InProgress)
			{
				num = 1f - m_SndSettings.m_Source.time / m_SndSettings.m_Source.clip.length;
			}
			m_SndSettings.m_Source.clip = null;
			switch (NewState)
			{
			case E_State.Starting:
				if (m_SndSettings.m_RotStart != null)
				{
					m_SndSettings.m_Source.clip = m_SndSettings.m_RotStart;
					m_SndSettings.m_Source.time = num * m_SndSettings.m_Source.clip.length;
					m_SndSettings.m_Source.loop = false;
				}
				break;
			case E_State.InProgress:
				m_SndSettings.m_Source.clip = m_SndSettings.m_RotLoop;
				m_SndSettings.m_Source.time = 0f;
				m_SndSettings.m_Source.loop = true;
				break;
			case E_State.Stopping:
				if (m_SndSettings.m_RotStop != null)
				{
					m_SndSettings.m_Source.clip = m_SndSettings.m_RotStop;
					m_SndSettings.m_Source.time = num * m_SndSettings.m_Source.clip.length;
					m_SndSettings.m_Source.loop = false;
				}
				break;
			}
			m_SndSettings.m_Source.Play();
		}
		m_State = NewState;
	}

	public Vector3 GetDir(E_Dir Dir)
	{
		return m_Dir[(int)Dir];
	}

	public void SetCurrentDir(Vector3 Dir, bool ResetTargetDir)
	{
		float AngleH = 0f;
		float AngleV = 0f;
		MathUtils.VectorToAngles(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, Dir, ref AngleH, ref AngleV);
		m_HMotor.Angle = AngleH;
		m_VMotor.Angle = AngleV;
		if (ResetTargetDir)
		{
			m_HMotor.TargetAngle = AngleH;
			m_VMotor.TargetAngle = AngleV;
		}
		m_Dir[0] = Dir;
	}

	public void SetTargetDir(Vector3 Dir)
	{
		float AngleH = 0f;
		float AngleV = 0f;
		MathUtils.VectorToAngles(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, Dir, ref AngleH, ref AngleV);
		m_HMotor.TargetAngle = AngleH;
		m_VMotor.TargetAngle = AngleV;
	}

	public void SetTargetDirByPos(Vector3 Pos)
	{
		float AngleH = 0f;
		float AngleV = 0f;
		Vector3 vec = Vector3.Normalize(Pos - Origin);
		MathUtils.VectorToAngles(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, vec, ref AngleH, ref AngleV);
		m_HMotor.TargetAngle = AngleH;
		m_VMotor.TargetAngle = AngleV;
	}

	public void SetTargetDirByAngleDiffs(float HChange, float VChange)
	{
		m_HMotor.TargetAngle = m_HMotor.Angle + HChange * ((float)Math.PI / 180f);
		m_VMotor.TargetAngle = m_VMotor.Angle + VChange * ((float)Math.PI / 180f);
	}

	public void ComputeDirs()
	{
		Vector3 forward = m_PartsSettings.m_Hinge.forward;
		m_Dir[0] = forward;
		m_Dir[1] = forward;
		m_Dir[2] = Vector3.zero;
		m_Dir[3] = Vector3.zero;
		m_Dir[4] = Vector3.zero;
		m_Dir[5] = Vector3.zero;
		if (m_RotSettings.m_HRange >= 0f && m_RotSettings.m_HRange < 360f)
		{
			float num = m_RotSettings.m_HRange * 0.5f * ((float)Math.PI / 180f);
			m_Dir[2] = MathUtils.AnglesToVector(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, 0f - num, 0f);
			m_Dir[3] = MathUtils.AnglesToVector(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, num, 0f);
		}
		if (m_RotSettings.m_VRange >= 0f && m_RotSettings.m_VRange < 360f)
		{
			float num2 = m_RotSettings.m_VRange * 0.5f * ((float)Math.PI / 180f);
			m_Dir[4] = MathUtils.AnglesToVector(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, 0f, 0f - num2);
			m_Dir[5] = MathUtils.AnglesToVector(m_PartsSettings.m_Base.forward, m_PartsSettings.m_Base.up, 0f, num2);
		}
	}
}
