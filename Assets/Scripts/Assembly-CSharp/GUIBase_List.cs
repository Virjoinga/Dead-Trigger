using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GUI/Widgets/Widgets List")]
public class GUIBase_List : GUIBase_Callback
{
	public GUIBase_Widget m_FirstListLine;

	public int m_NumOfLines = 1;

	public Vector2 m_LinesOffset;

	private GUIBase_Widget m_Widget;

	private List<GUIBase_Widget> m_Lines = new List<GUIBase_Widget>();

	public GUIBase_Widget Widget
	{
		get
		{
			return m_Widget;
		}
	}

	public int numOfLines
	{
		get
		{
			return m_Lines.Count;
		}
	}

	private void Start()
	{
		m_Widget = GetComponent<GUIBase_Widget>();
		if (m_FirstListLine != null && m_NumOfLines > 1)
		{
			InitializeChilds();
		}
	}

	public GUIBase_Widget GetWidgetOnLine(int inLineIndex)
	{
		if (inLineIndex >= 0 && inLineIndex < m_Lines.Count)
		{
			return m_Lines[inLineIndex];
		}
		return null;
	}

	private void InitializeChilds()
	{
		Vector3 position = m_FirstListLine.transform.position;
		Quaternion rotation = m_FirstListLine.transform.rotation;
		Vector3 zero = Vector3.zero;
		m_Lines.Add(m_FirstListLine);
		for (int i = 1; i < m_NumOfLines; i++)
		{
			zero.x += m_LinesOffset.x;
			zero.y += m_LinesOffset.y;
			GUIBase_Widget gUIBase_Widget = Object.Instantiate(m_FirstListLine, position + zero, rotation) as GUIBase_Widget;
			gUIBase_Widget.transform.parent = m_FirstListLine.transform.parent;
			gUIBase_Widget.transform.localScale = m_FirstListLine.transform.localScale;
			gUIBase_Widget.transform.localPosition = m_FirstListLine.transform.localPosition + zero;
			gUIBase_Widget.gameObject.name = m_FirstListLine.name + " [ " + i + " ]";
			m_Lines.Add(gUIBase_Widget);
		}
	}
}
