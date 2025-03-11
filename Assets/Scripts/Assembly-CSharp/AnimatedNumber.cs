using UnityEngine;

public class AnimatedNumber
{
	private GUIBase_Label m_Label;

	private Vector3 m_OrigScale;

	public Vector3 OrigScale
	{
		get
		{
			return m_OrigScale;
		}
	}

	public GUIBase_Label Label
	{
		get
		{
			return m_Label;
		}
	}

	public AnimatedNumber(GUIBase_Label label)
	{
		m_Label = label;
		m_OrigScale = label.transform.localScale;
	}

	public void RestoreOrigScale()
	{
		m_Label.transform.localScale = m_OrigScale;
	}
}
