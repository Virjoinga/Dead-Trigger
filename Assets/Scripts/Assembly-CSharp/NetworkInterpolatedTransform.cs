using UnityEngine;

public class NetworkInterpolatedTransform : MonoBehaviour
{
	internal struct State
	{
		internal double timestamp;

		internal Vector3 pos;

		internal Quaternion rot;
	}

	public double interpolationBackTime = 0.1;

	private State[] m_BufferedState = new State[20];

	private int m_TimestampCount;

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 value = base.transform.localPosition;
			Quaternion value2 = base.transform.localRotation;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			return;
		}
		Vector3 value3 = Vector3.zero;
		Quaternion value4 = Quaternion.identity;
		stream.Serialize(ref value3);
		stream.Serialize(ref value4);
		for (int num = m_BufferedState.Length - 1; num >= 1; num--)
		{
			m_BufferedState[num] = m_BufferedState[num - 1];
		}
		State state = default(State);
		state.timestamp = info.timestamp;
		state.pos = value3;
		state.rot = value4;
		m_BufferedState[0] = state;
		m_TimestampCount = Mathf.Min(m_TimestampCount + 1, m_BufferedState.Length);
		for (int i = 0; i < m_TimestampCount - 1; i++)
		{
			if (m_BufferedState[i].timestamp < m_BufferedState[i + 1].timestamp)
			{
				Debug.Log("State inconsistent");
			}
		}
	}

	private void Update()
	{
		if (Network.isServer)
		{
			return;
		}
		Debug.Log("Updating network transform");
		double time = Network.time;
		double num = time - interpolationBackTime;
		if (m_BufferedState[0].timestamp > num)
		{
			for (int i = 0; i < m_TimestampCount; i++)
			{
				if (m_BufferedState[i].timestamp <= num || i == m_TimestampCount - 1)
				{
					State state = m_BufferedState[Mathf.Max(i - 1, 0)];
					State state2 = m_BufferedState[i];
					double num2 = state.timestamp - state2.timestamp;
					float t = 0f;
					if (num2 > 0.0001)
					{
						t = (float)((num - state2.timestamp) / num2);
					}
					base.transform.localPosition = Vector3.Lerp(state2.pos, state.pos, t);
					base.transform.localRotation = Quaternion.Slerp(state2.rot, state.rot, t);
					break;
				}
			}
		}
		else
		{
			State state3 = m_BufferedState[0];
			base.transform.localPosition = state3.pos;
			base.transform.localRotation = state3.rot;
		}
	}
}
