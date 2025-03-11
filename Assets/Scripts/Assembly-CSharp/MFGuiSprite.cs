using UnityEngine;

[AddComponentMenu("")]
public class MFGuiSprite
{
	protected float m_width;

	protected float m_height;

	protected float m_depth;

	protected Vector2 m_lowerLeftUV;

	protected Vector2 m_UVDimensions;

	protected bool m_HasClient;

	protected MFGuiRenderer m_GuiRenderer;

	internal bool m_hidden___DoNotAccessExternally;

	protected Vector3[] meshVerts;

	public Matrix4x4 matrix;

	public Vector3 offset;

	public Color m_Color;

	public int index { get; private set; }

	public bool client
	{
		get
		{
			return m_HasClient;
		}
		set
		{
			m_HasClient = value;
		}
	}

	public Vector2 lowerLeftUV
	{
		get
		{
			return m_lowerLeftUV;
		}
		set
		{
			m_lowerLeftUV = value;
			m_GuiRenderer.UpdateUV(this);
		}
	}

	public Vector2 uvDimensions
	{
		get
		{
			return m_UVDimensions;
		}
		set
		{
			m_UVDimensions = value;
			m_GuiRenderer.UpdateUV(this);
		}
	}

	public float width
	{
		get
		{
			return m_width;
		}
	}

	public float height
	{
		get
		{
			return m_height;
		}
	}

	public bool hidden
	{
		get
		{
			return m_hidden___DoNotAccessExternally;
		}
		set
		{
			if (value != m_hidden___DoNotAccessExternally)
			{
				m_hidden___DoNotAccessExternally = value;
				if (value)
				{
					m_GuiRenderer.HideSprite(this);
				}
				else
				{
					m_GuiRenderer.ShowSprite(this);
				}
			}
		}
	}

	public MFGuiSprite(MFGuiRenderer inRenderer, int inIndex)
	{
		m_width = 0f;
		m_height = 0f;
		m_depth = 0f;
		m_HasClient = false;
		m_GuiRenderer = inRenderer;
		index = inIndex;
		m_Color = Color.black;
		offset = Vector3.zero;
	}

	~MFGuiSprite()
	{
		m_GuiRenderer = null;
	}

	public void SetSizeXY(float width, float height, float depth)
	{
		m_width = width;
		m_height = height;
		m_depth = depth;
		Vector3 v = offset + new Vector3((0f - m_width) / 2f, m_height / 2f, depth);
		Vector3 v2 = offset + new Vector3((0f - m_width) / 2f, (0f - m_height) / 2f, depth);
		Vector3 v3 = offset + new Vector3(m_width / 2f, (0f - m_height) / 2f, depth);
		Vector3 v4 = offset + new Vector3(m_width / 2f, m_height / 2f, depth);
		Transform(v, v2, v3, v4);
	}

	public void SetSizeXZ(float width, float height, float depth)
	{
		m_width = width;
		m_height = height;
		m_depth = depth;
		Vector3 v = offset + new Vector3((0f - m_width) / 2f, depth, m_height / 2f);
		Vector3 v2 = offset + new Vector3((0f - m_width) / 2f, depth, (0f - m_height) / 2f);
		Vector3 v3 = offset + new Vector3(m_width / 2f, depth, (0f - m_height) / 2f);
		Vector3 v4 = offset + new Vector3(m_width / 2f, depth, m_height / 2f);
		Transform(v, v2, v3, v4);
	}

	public void SetSizeYZ(float width, float height, float depth)
	{
		m_width = width;
		m_height = height;
		m_depth = depth;
		Vector3 v = offset + new Vector3(depth, m_height / 2f, (0f - m_width) / 2f);
		Vector3 v2 = offset + new Vector3(depth, (0f - m_height) / 2f, (0f - m_width) / 2f);
		Vector3 v3 = offset + new Vector3(depth, (0f - m_height) / 2f, m_width / 2f);
		Vector3 v4 = offset + new Vector3(depth, m_height / 2f, m_width / 2f);
		Transform(v, v2, v3, v4);
	}

	public void SetBuffers(Vector3[] v, Vector2[] uv)
	{
		meshVerts = v;
	}

	public void Transform(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
	{
		int num = 4 * index;
		meshVerts[num] = matrix.MultiplyPoint(v1);
		meshVerts[num + 1] = matrix.MultiplyPoint(v2);
		meshVerts[num + 2] = matrix.MultiplyPoint(v3);
		meshVerts[num + 3] = matrix.MultiplyPoint(v4);
		m_GuiRenderer.UpdatePositions();
	}

	public void UpdateVertices(MFGuiRenderer.SPRITE_PLANE inPlaen)
	{
		switch (inPlaen)
		{
		case MFGuiRenderer.SPRITE_PLANE.XY:
			SetSizeXY(m_width, m_height, m_depth);
			break;
		case MFGuiRenderer.SPRITE_PLANE.XZ:
			SetSizeXZ(m_width, m_height, m_depth);
			break;
		case MFGuiRenderer.SPRITE_PLANE.YZ:
			SetSizeYZ(m_width, m_height, m_depth);
			break;
		default:
			SetSizeXY(m_width, m_height, m_depth);
			break;
		}
	}

	public void SetColor(Color c)
	{
		m_Color = c;
		m_GuiRenderer.UpdateColors(this);
	}

	public void SetAlpha(float alpha)
	{
		m_Color.a = alpha;
		m_GuiRenderer.UpdateColors(this);
	}
}
