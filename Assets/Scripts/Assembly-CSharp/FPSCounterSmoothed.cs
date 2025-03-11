using UnityEngine;

public class FPSCounterSmoothed
{
	private float[] m_Samples;

	private readonly int m_SamplesMaxNum;

	private int m_SamplesNum;

	private float m_SamplesSum;

	private int m_SampleIndex;

	public float Value { get; private set; }

	public FPSCounterSmoothed(int SmoothingSamplesCount)
	{
		Value = 0f;
		m_SamplesNum = 0;
		m_SamplesMaxNum = Mathf.Max(1, SmoothingSamplesCount);
		m_Samples = new float[m_SamplesMaxNum];
		m_SamplesSum = 0f;
		m_SampleIndex = 0;
	}

	public void Reset()
	{
		Value = 0f;
		m_SamplesSum = 0f;
		m_SamplesNum = 0;
		m_SampleIndex = 0;
	}

	public void Update(float DeltaTime)
	{
		m_SamplesSum -= m_Samples[m_SampleIndex];
		m_SamplesSum += DeltaTime;
		m_Samples[m_SampleIndex] = DeltaTime;
		m_SampleIndex++;
		if (m_SampleIndex > m_SamplesNum)
		{
			m_SamplesNum = m_SampleIndex;
		}
		if (m_SampleIndex == m_SamplesMaxNum)
		{
			m_SampleIndex = 0;
		}
		Value = (float)m_SamplesNum / m_SamplesSum;
	}
}
