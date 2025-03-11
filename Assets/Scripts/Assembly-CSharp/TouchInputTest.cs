using System.Collections.Generic;
using UnityEngine;

public class TouchInputTest : MonoBehaviour
{
	private Dictionary<int, bool> m_ActiveTouches = new Dictionary<int, bool>();

	public void Update()
	{
		int num = 100;
		while (--num > 0)
		{
			Debug.Log("This is only garbage to decrease FPS");
		}
		if (Input.touchCount == 0)
		{
			return;
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			Debug.Log(string.Concat("time: ", Time.timeSinceLevelLoad, " TouchPhase: ", touch.phase.ToString(), " | id=", touch.fingerId, ", pos=", touch.position, ", delta=", touch.deltaPosition));
			if (touch.phase == TouchPhase.Began)
			{
				bool value = false;
				if (m_ActiveTouches.TryGetValue(touch.fingerId, out value) && value)
				{
					Debug.Log(string.Concat("time: ", Time.timeSinceLevelLoad, " TouchPhase: TOUCH_ERROR | id=", touch.fingerId, ", pos=", touch.position, ", delta=", touch.deltaPosition, "====================================="));
				}
				m_ActiveTouches[touch.fingerId] = true;
			}
			else if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Stationary && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
			{
				m_ActiveTouches[touch.fingerId] = false;
			}
		}
	}
}
