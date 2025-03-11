using System;
using UnityEngine;

public class Rotator
{
	private const float ErrorTolerance = 0.004363323f;

	private float m_Current;

	private float m_Target;

	private bool m_AngleLimited;

	private float m_AngleLimitMin;

	private float m_AngleLimitMax;

	private float m_Speed;

	private bool m_Active;

	public float Angle
	{
		get
		{
			return m_Current;
		}
		set
		{
			SetCurrent(value);
		}
	}

	public float AbsDiff
	{
		get
		{
			return Mathf.Abs(m_Target - m_Current);
		}
	}

	public float TargetAngle
	{
		set
		{
			SetTarget(value);
		}
	}

	public float Speed
	{
		get
		{
			return m_Speed;
		}
	}

	public bool IsActive
	{
		get
		{
			return m_Active;
		}
	}

	public Rotator(float Angle, float Range, float Speed)
	{
		m_Current = MathUtils.SanitizeRadians(Angle);
		m_Target = m_Current;
		m_Active = false;
		m_Speed = Speed;
		if (Range >= 0f && Range < 360f)
		{
			m_AngleLimited = true;
			m_AngleLimitMin = m_Current - Range * 0.5f;
			m_AngleLimitMax = m_Current + Range * 0.5f;
		}
		else
		{
			m_AngleLimited = false;
		}
	}

	public void Update(float DeltaTime)
	{
		if (m_Active)
		{
			float num = m_Speed * DeltaTime;
			float num2 = Mathf.Clamp(m_Target - m_Current, 0f - num, num);
			m_Current += num2;
			m_Active = Mathf.Abs(m_Target - m_Current) > 0.004363323f;
		}
	}

	private void SetCurrent(float Angle)
	{
		m_Current = FixAngle(Angle);
		m_Active = Mathf.Abs(m_Target - m_Current) > 0.004363323f;
	}

	private void SetTarget(float Angle)
	{
		m_Target = FixAngle(Angle);
		m_Active = Mathf.Abs(m_Target - m_Current) > 0.004363323f;
	}

	private float FixAngle(float Angle)
	{
		if (m_AngleLimited)
		{
			Angle = Mathf.Clamp(Angle, m_AngleLimitMin, m_AngleLimitMax);
		}
		while (Angle - m_Current > (float)Math.PI)
		{
			Angle -= (float)Math.PI * 2f;
		}
		while (m_Current - Angle > (float)Math.PI)
		{
			Angle += (float)Math.PI * 2f;
		}
		return Angle;
	}
}
