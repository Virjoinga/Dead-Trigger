using UnityEngine;

public class FPSCounter
{
	private int m_FrameCounter;

	private float m_TimeAccum;

	private readonly float m_UpdateInterval;

	public float Value { get; private set; }

	public FPSCounter(float UpdateInterval)
	{
		Value = 0f;
		m_FrameCounter = 0;
		m_TimeAccum = 0f;
		m_UpdateInterval = UpdateInterval;
	}

	public void Reset()
	{
		Value = 0f;
		m_FrameCounter = 0;
		m_TimeAccum = 0f;
	}

	public void Update(float DeltaTime)
	{
		m_FrameCounter++;
		m_TimeAccum += DeltaTime;
		if (m_TimeAccum >= m_UpdateInterval)
		{
			Value = (float)m_FrameCounter / m_TimeAccum;
			m_FrameCounter = 0;
			m_TimeAccum = 0f;
		}
	}
}
public class FpsCounter : MonoBehaviour
{
}
