using System.Collections.Generic;
using UnityEngine;

internal class GuiDragInput
{
	private const float maxTapDuration = 0.5f;

	private const float minPopupHoldDuration = 0.75f;

	private const float maxPopupMoveDistance = 50f;

	private const float maxTapMoveDistance = 50f;

	private const float frictionPerSec = 900f;

	private const float inertiaFixedUpdate = 0.1f;

	private const float frictionPerCent = 0.15f;

	private const float frictionDuration = 1.2f;

	private const float minFriction = 0.05f;

	private const float maxFriction = 0.25f;

	private const float frictionAccDuration = 1.5f;

	private const int maxSmooth = 10;

	public bool isHorizontal;

	private float m_Velocity;

	private float timeTouchPhaseEnded;

	private float touchBeginTime;

	private float touchBeginPos;

	private float m_LastUpdateTime;

	private bool notMovedSinceTouch;

	private int m_FingerId = -1;

	private Vector3 lastMouseDragPos;

	private float lastDragPos;

	private List<float> lastValues = new List<float>(10);

	private Rect m_ActiveArea;

	public float MinSpeed
	{
		get
		{
			return 300f;
		}
	}

	public bool tapEvent { get; private set; }

	public float tapEventPos { get; private set; }

	public bool isHolding { get; private set; }

	public float holdingPos { get; private set; }

	public Vector2 ScrollDelta { get; private set; }

	public bool IsDragging { get; private set; }

	public float MoveSpeed
	{
		get
		{
			return Mathf.Abs(m_Velocity);
		}
	}

	public GuiDragInput()
	{
		IsDragging = false;
	}

	public void SetActiveArea(Rect rect)
	{
		m_ActiveArea = rect;
	}

	public void ClearTapEvent()
	{
		tapEvent = false;
		tapEventPos = 0f;
	}

	private void AddInertia(float inr)
	{
		if (lastValues.Count == 10)
		{
			lastValues.RemoveAt(0);
		}
		lastValues.Add(inr);
	}

	private float GetInertia()
	{
		float num = 0f;
		for (int i = 0; i < lastValues.Count; i++)
		{
			num += lastValues[i] * (float)i / (float)lastValues.Count;
		}
		return num / (float)lastValues.Count;
	}

	public void Update()
	{
		ScrollDelta = Vector2.zero;
		if (Input.touchCount > 0)
		{
			TouchUpdate();
		}
		else if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
		{
			MouseUpdate();
		}
		else
		{
			UpdateVelocity();
		}
	}

	private void TouchUpdate()
	{
		int num = -1;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			if (m_FingerId == -1 && touch.phase == TouchPhase.Began)
			{
				if (m_ActiveArea.Contains(touch.position))
				{
					num = i;
					m_FingerId = touch.fingerId;
					break;
				}
			}
			else if (touch.fingerId == m_FingerId)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			Touch touch2 = Input.GetTouch(num);
			float num2 = ((!isHorizontal) ? (0f - touch2.position.y) : touch2.position.x);
			float deltaPos = num2 - lastDragPos;
			lastDragPos = num2;
			if (touch2.phase == TouchPhase.Began)
			{
				OnDragBegin(num2);
			}
			else if (touch2.phase == TouchPhase.Canceled)
			{
				OnDragCancel();
				m_FingerId = -1;
			}
			else if (touch2.phase == TouchPhase.Moved)
			{
				OnDragMove(deltaPos, touch2.deltaTime);
			}
			else if (touch2.phase == TouchPhase.Ended)
			{
				OnDragEnd(deltaPos, touch2.deltaTime, num2);
				m_FingerId = -1;
			}
			else if (touch2.phase == TouchPhase.Stationary)
			{
				OnTouchUpdate(num2);
			}
		}
	}

	private void CleanTouch()
	{
		IsDragging = false;
		touchBeginPos = 0f;
		touchBeginTime = 0f;
		isHolding = false;
		notMovedSinceTouch = false;
		holdingPos = 0f;
		lastDragPos = 0f;
	}

	private void OnDragBegin(float startPos)
	{
		m_Velocity = 0f;
		touchBeginPos = startPos;
		touchBeginTime = Time.time;
		lastDragPos = startPos;
		notMovedSinceTouch = true;
		IsDragging = true;
	}

	private void OnTouchUpdate(float curPos)
	{
		if (notMovedSinceTouch)
		{
			notMovedSinceTouch = Mathf.Abs(touchBeginPos - curPos) < 50f;
		}
		if (IsDragging && Time.time > touchBeginTime + 0.75f && notMovedSinceTouch)
		{
			isHolding = true;
			holdingPos = curPos;
		}
	}

	private void OnDragMove(float deltaPos, float deltaTime)
	{
		if (isHorizontal)
		{
			ScrollDelta = new Vector2(deltaPos, 0f);
		}
		else
		{
			ScrollDelta = new Vector2(0f, deltaPos);
		}
		if (deltaTime >= 0.0001f)
		{
			float inr = deltaPos / deltaTime;
			AddInertia(inr);
		}
	}

	private void OnDragEnd(float deltaPos, float deltaTime, float curPos)
	{
		timeTouchPhaseEnded = Time.time;
		if (deltaTime >= 0.0001f)
		{
			float inr = deltaPos / deltaTime;
			AddInertia(inr);
		}
		if (Time.time < touchBeginTime + 0.5f && Mathf.Abs(curPos - touchBeginPos) < 50f)
		{
			tapEvent = true;
			tapEventPos = curPos;
		}
		else
		{
			m_Velocity = GetInertia();
		}
		lastValues.Clear();
		CleanTouch();
	}

	public bool HasMomentum()
	{
		return MoveSpeed > MinSpeed;
	}

	private void OnDragCancel()
	{
		CleanTouch();
	}

	private void MouseUpdate()
	{
		float num = ((!isHorizontal) ? (0f - Input.mousePosition.y) : Input.mousePosition.x);
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 point = Input.mousePosition;
			if (m_ActiveArea.Contains(point))
			{
				lastMouseDragPos = Input.mousePosition;
				OnDragBegin(num);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Vector3 vector = Input.mousePosition - lastMouseDragPos;
			float deltaPos = ((!isHorizontal) ? (0f - vector.y) : vector.x);
			OnDragEnd(deltaPos, Time.deltaTime, num);
			lastMouseDragPos = Input.mousePosition;
		}
		else if (Input.GetMouseButton(0))
		{
			Vector3 vector2 = Input.mousePosition - lastMouseDragPos;
			float num2 = ((!isHorizontal) ? (0f - vector2.y) : vector2.x);
			if (Mathf.Abs(num2) > 0.001f)
			{
				OnDragMove(num2, Time.deltaTime);
			}
			else
			{
				OnTouchUpdate(num);
			}
			lastMouseDragPos = Input.mousePosition;
		}
	}

	private float GetFriction()
	{
		float num = Time.time - timeTouchPhaseEnded;
		float t = Mathf.Clamp(num / 1.2f, 0f, 1f);
		return Mathf.Lerp(0.05f, 0.25f, t);
	}

	private void UpdateVelocity()
	{
		float num = Time.time - m_LastUpdateTime;
		while (num > 0.1f)
		{
			num -= 0.1f;
			UpdateFrictionProportional();
		}
		m_LastUpdateTime = Time.time - num;
		float num2 = m_Velocity * Time.deltaTime;
		if (isHorizontal)
		{
			ScrollDelta = new Vector2(num2, 0f);
		}
		else
		{
			ScrollDelta = new Vector2(0f, num2);
		}
	}

	private void UpdateFrictionProportional()
	{
		m_Velocity -= m_Velocity * GetFriction();
	}

	private void UpdateFrictionConstant(float inDeltaTime)
	{
		if (m_Velocity == 0f)
		{
			return;
		}
		float num = 900f * inDeltaTime;
		if (m_Velocity > 0f)
		{
			m_Velocity -= num;
			if (m_Velocity < 0f)
			{
				m_Velocity = 0f;
			}
		}
		else
		{
			m_Velocity += num;
			if (m_Velocity > 0f)
			{
				m_Velocity = 0f;
			}
		}
	}
}
