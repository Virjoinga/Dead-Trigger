using System;
using UnityEngine;

public class CityCamera : MonoBehaviour
{
	public struct Line
	{
		public float x1;

		public float y1;

		public float x2;

		public float y2;

		public void Shorten(float byPixels)
		{
			float num = x1 - x2;
			float num2 = y1 - y2;
			float num3 = Mathf.Abs(num) + Mathf.Abs(num2);
			if (!Mathf.Approximately(num3, 0f))
			{
				float num4 = Mathf.Abs(num / num3);
				x1 = x2 + num + byPixels * num4 * (float)((!(num > 0f)) ? 1 : (-1));
				y1 = y2 + num2 + byPixels * (1f - num4) * (float)((!(num2 > 0f)) ? 1 : (-1));
			}
		}
	}

	public delegate void CameraMoveEvent(Vector3 pos);

	private Vector3 finishingMove;

	private bool finishingMoveActive;

	private float finishingTimeActual;

	private float finishingTimeMax = 1f;

	private Vector3 lastDragMove;

	private bool userMovement;

	private CameraMoveEvent m_MoveEvent;

	private Vector3 requestedPos;

	public Vector3 position
	{
		get
		{
			return Camera.main.transform.position;
		}
	}

	public void RegisterMoveEvent(CameraMoveEvent listener)
	{
		m_MoveEvent = listener;
	}

	public Vector3 WorldToScreenPoint(Vector3 point)
	{
		return Camera.main.WorldToScreenPoint(point);
	}

	public Vector3 ScreenToWorldPoint(Vector3 point)
	{
		return Camera.main.ScreenToWorldPoint(point);
	}

	public static bool DoLineAndRectangleIntersect(ref Line l1, ref Line rect1, ref Line rect2, ref Line rect3, ref Line rect4, out Vector3 ptIntersection)
	{
		if (DoLinesIntersect(ref l1, ref rect1, out ptIntersection))
		{
			return true;
		}
		if (DoLinesIntersect(ref l1, ref rect2, out ptIntersection))
		{
			return true;
		}
		if (DoLinesIntersect(ref l1, ref rect3, out ptIntersection))
		{
			return true;
		}
		if (DoLinesIntersect(ref l1, ref rect4, out ptIntersection))
		{
			return true;
		}
		return false;
	}

	public static bool DoLinesIntersect(ref Line l1, ref Line l2, out Vector3 ptIntersection)
	{
		float num = (l2.y2 - l2.y1) * (l1.x2 - l1.x1) - (l2.x2 - l2.x1) * (l1.y2 - l1.y1);
		float num2 = (l2.x2 - l2.x1) * (l1.y1 - l2.y1) - (l2.y2 - l2.y1) * (l1.x1 - l2.x1);
		float num3 = (l1.x2 - l1.x1) * (l1.y1 - l2.y1) - (l1.y2 - l1.y1) * (l1.x1 - l2.x1);
		if (num == 0f)
		{
			ptIntersection = Vector3.zero;
			return false;
		}
		float num4 = num2 / num;
		float num5 = num3 / num;
		if (num4 >= 0f && num4 <= 1f && num5 >= 0f && num5 <= 1f)
		{
			ptIntersection = default(Vector3);
			ptIntersection.x = l1.x1 + num4 * (l1.x2 - l1.x1);
			ptIntersection.y = l1.y1 + num4 * (l1.y2 - l1.y1);
			ptIntersection.z = 0f;
			return true;
		}
		ptIntersection = Vector3.zero;
		return false;
	}

	public void CenterCameraOn(Vector3 pos)
	{
		finishingMoveActive = false;
		Vector3 vector = ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 9.3f));
		userMovement = false;
		RequestCameraWorldMove(position + pos - vector);
	}

	public void MoveCameraCloserTo(Vector3 pos)
	{
		finishingMoveActive = false;
		Vector3 vector = ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 9.3f));
		Vector3 vector2 = pos - vector;
		float magnitude = vector2.magnitude;
		float num = 0f;
		if (Mathf.Abs(vector2.x) > 2f)
		{
			num = magnitude - 2f;
		}
		else if (Mathf.Abs(vector2.z) > (float)Screen.height / (float)Screen.width * 2f)
		{
			num = magnitude - (float)Screen.height / (float)Screen.width * 2f;
		}
		vector2.Normalize();
		userMovement = false;
		RequestCameraWorldMove(position + vector2 * num);
	}

	private void Start()
	{
		requestedPos = Camera.main.transform.position;
		CityInput cityInput = base.gameObject.GetComponent("CityInput") as CityInput;
		if ((bool)cityInput)
		{
			cityInput.RegisterDragEvent(DragEvent);
		}
	}

	private void DragEvent(Vector3 dragVect, bool finished)
	{
		userMovement = true;
		dragVect.y *= (float)(Screen.width / Screen.height) * 2f;
		float num = dragVect.magnitude / (float)Screen.height;
		if (num > 0f)
		{
			finishingMove = dragVect;
			finishingMove *= 100f;
			finishingTimeActual = 0f;
			finishingTimeMax = 0.3f + Mathf.Clamp(num, 0f, 1f);
		}
		if (finished)
		{
			float magnitude = finishingMove.magnitude;
			if (magnitude > (float)(6 * Screen.height))
			{
				finishingMove *= (float)(6 * Screen.height) / magnitude;
			}
			finishingTimeActual = 0f;
			RequestCameraMoveScreen(dragVect * 2f);
			finishingMoveActive = true;
			lastDragMove = Vector3.zero;
		}
		else
		{
			finishingMoveActive = false;
			Vector3 vector = dragVect * 4f;
			RequestCameraMoveScreen(lastDragMove = (vector + lastDragMove) / 2f);
		}
	}

	private void RequestCameraMoveScreen(Vector3 newDeltaScreenPos)
	{
		float num = (float)Screen.height / 1.5f;
		if (newDeltaScreenPos.magnitude > num)
		{
			newDeltaScreenPos.Normalize();
			newDeltaScreenPos *= num;
		}
		newDeltaScreenPos.z = 5f;
		Vector3 vector = ScreenToWorldPoint(new Vector3(0f, 0f, 5f));
		Vector3 vector2 = ScreenToWorldPoint(newDeltaScreenPos);
		Vector3 newPos = requestedPos + (vector2 - vector);
		RequestCameraWorldMove(newPos);
	}

	private void RequestCameraWorldMove(Vector3 newPos)
	{
		requestedPos = newPos;
		requestedPos.y = 5.7f;
		ClampCameraPos(ref requestedPos);
	}

	private void ClampCameraPos(ref Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, -4.4f, 6.1f);
		pos.z = Mathf.Clamp(pos.z, -5.5f, 1.2f);
	}

	private void SmoothCameraMove(float deltaTime)
	{
		if (!requestedPos.Equals(Camera.main.transform.position))
		{
			Vector3 vector = requestedPos - Camera.main.transform.position;
			float magnitude = vector.magnitude;
			float num = Time.deltaTime * (float)((!userMovement) ? 13 : 40);
			if (finishingMoveActive)
			{
				num *= Mathf.Cos((float)Math.PI / 2f * (finishingTimeActual / finishingTimeMax));
			}
			Vector3 pos = ((!(magnitude > num) || !(num > 0.01f)) ? requestedPos : (Camera.main.transform.position + vector / magnitude * num));
			ClampCameraPos(ref pos);
			Camera.main.transform.position = pos;
			if (m_MoveEvent != null)
			{
				m_MoveEvent(Camera.main.transform.position);
			}
		}
	}

	private void Update()
	{
		if (finishingMove.sqrMagnitude > 0.01f)
		{
			finishingTimeActual += Time.deltaTime;
			if (finishingTimeActual >= finishingTimeMax)
			{
				finishingTimeActual = finishingTimeMax;
				finishingMoveActive = false;
			}
			if (finishingMoveActive)
			{
				RequestCameraMoveScreen(finishingMove * (finishingTimeMax - finishingTimeActual) * Time.deltaTime);
			}
		}
		SmoothCameraMove(Time.deltaTime);
	}
}
