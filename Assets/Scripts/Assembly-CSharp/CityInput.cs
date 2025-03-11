using System.Collections.Generic;
using UnityEngine;

public class CityInput : MonoBehaviour
{
	public delegate void DragEvent(Vector3 dragVect, bool finished);

	public delegate void TouchEvent(Collider collider);

	private Vector3 MouseDragPos;

	private bool m_Touching;

	private bool m_IsDragging;

	private bool m_DraggingEnabled = true;

	private bool m_Disabled;

	private List<DragEvent> m_DragEvents;

	private List<TouchEvent> m_TouchEvents;

	public void RegisterDragEvent(DragEvent listener)
	{
		m_DragEvents.Add(listener);
	}

	public void RegisterTouchEvent(TouchEvent listener)
	{
		m_TouchEvents.Add(listener);
	}

	public void Enable(bool scrolling)
	{
		m_Disabled = false;
		m_DraggingEnabled = scrolling;
	}

	public void Disable()
	{
		m_Disabled = true;
		m_IsDragging = false;
		m_Touching = false;
	}

	public bool IsDisabled()
	{
		return m_Disabled;
	}

	private void Awake()
	{
		m_DragEvents = new List<DragEvent>();
		m_TouchEvents = new List<TouchEvent>();
	}

	private void Start()
	{
		m_IsDragging = false;
		m_Touching = false;
	}

	private void Update()
	{
		if (m_Disabled)
		{
			return;
		}
		if (m_Touching)
		{
			Vector3 dragVect = MouseDragPos - Input.mousePosition;
			float sqrMagnitude = dragVect.sqrMagnitude;
			if (!m_IsDragging && sqrMagnitude > (float)(Screen.width * Screen.height) / 1700f)
			{
				m_IsDragging = true;
			}
			if (m_IsDragging && m_DraggingEnabled)
			{
				foreach (DragEvent dragEvent in m_DragEvents)
				{
					dragEvent(dragVect, Input.GetMouseButtonUp(0));
				}
				MouseDragPos = Input.mousePosition;
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (!m_Touching)
			{
				m_Touching = true;
				MouseDragPos = Input.mousePosition;
			}
		}
		else
		{
			if (!Input.GetMouseButtonUp(0))
			{
				return;
			}
			if (m_Touching && !m_IsDragging)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (Physics.Raycast(ray, out hitInfo))
				{
					foreach (TouchEvent touchEvent in m_TouchEvents)
					{
						touchEvent(hitInfo.collider);
					}
				}
			}
			m_IsDragging = false;
			m_Touching = false;
		}
	}
}
