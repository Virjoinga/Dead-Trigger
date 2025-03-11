using UnityEngine;

public class CityHudUtility
{
	private float xLeft;

	private float xRight;

	private float yTop;

	private float yBottom;

	private Vector3 screenCamPos;

	private CityCamera.Line screenUp;

	private CityCamera.Line screenBottom;

	private CityCamera.Line screenLeft;

	private CityCamera.Line screenRight;

	public void InitResolution()
	{
		xLeft = 8f;
		xRight = Screen.width - 16;
		yTop = Screen.height - 68;
		yBottom = 8f;
		screenCamPos = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		screenUp.x1 = xLeft;
		screenUp.y1 = yTop;
		screenUp.x2 = xRight;
		screenUp.y2 = yTop;
		screenBottom.x1 = xLeft;
		screenBottom.y1 = yBottom;
		screenBottom.x2 = xRight;
		screenBottom.y2 = yBottom;
		screenLeft.x1 = xLeft;
		screenLeft.y1 = yBottom;
		screenLeft.x2 = xLeft;
		screenLeft.y2 = yTop;
		screenRight.x1 = xRight;
		screenRight.y1 = yBottom;
		screenRight.x2 = xRight;
		screenRight.y2 = yTop;
	}

	public float GetScreenBottom()
	{
		return Screen.height;
	}

	public bool FindIntersectionWithScreen(Vector3 iconScreenPos, out Vector3 intersect)
	{
		Vector3 vector = iconScreenPos;
		CityCamera.Line l = default(CityCamera.Line);
		l.x1 = vector.x;
		l.y1 = vector.y;
		l.x2 = screenCamPos.x;
		l.y2 = screenCamPos.y;
		CityCamera.Line rect = screenUp;
		rect.y1 -= (float)Screen.height * 0.075f;
		rect.y2 -= (float)Screen.height * 0.075f;
		if (CityCamera.DoLineAndRectangleIntersect(ref l, ref rect, ref screenBottom, ref screenLeft, ref screenRight, out intersect))
		{
			if (Mathf.FloorToInt(intersect.x) <= Mathf.FloorToInt(xLeft))
			{
				l.Shorten((float)Screen.width * 0.02f);
			}
			else if (Mathf.CeilToInt(intersect.x) >= Mathf.FloorToInt(xRight))
			{
				l.Shorten((float)Screen.width * 0.02f);
			}
			else if (Mathf.FloorToInt(intersect.y) <= Mathf.FloorToInt(yBottom))
			{
				l.Shorten((float)Screen.width * 0.12f);
			}
			else if (Mathf.CeilToInt(intersect.y) >= Mathf.FloorToInt(rect.y1))
			{
				l.Shorten((float)(-Screen.width) * 0.15f);
			}
			else
			{
				Debug.Log("WTF");
			}
		}
		if (CityCamera.DoLineAndRectangleIntersect(ref l, ref screenUp, ref screenBottom, ref screenLeft, ref screenRight, out intersect))
		{
			intersect -= screenCamPos;
			float magnitude = intersect.magnitude;
			intersect *= 1f - 12f / magnitude;
			intersect += screenCamPos;
			if (intersect.x < xLeft + 12f)
			{
				intersect.x = xLeft + 12f;
			}
			if (intersect.x > xRight - 12f)
			{
				intersect.x = xRight - 12f;
			}
			if (intersect.y < yBottom + 14f)
			{
				intersect.y = yBottom + 14f;
			}
			if (intersect.y > yTop - 14f)
			{
				intersect.y = yTop - 14f;
			}
			intersect.y = (float)Screen.height - intersect.y;
			return true;
		}
		return false;
	}
}
