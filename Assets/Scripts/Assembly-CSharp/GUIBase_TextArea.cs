using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GUI/Widgets/TextArea")]
public class GUIBase_TextArea : GUIBase_Callback
{
	public enum HorizontalTextAlignment
	{
		Left = 0,
		Center = 1,
		Right = 2,
		Justify = 3
	}

	public class TextLine
	{
		public int m_StartIndex;

		public int m_EndIndex;

		public Vector2 m_Size = Vector2.zero;

		public Vector2 m_Offset = Vector2.zero;

		public bool m_EndOfParagraph;

		public int m_NumOfSpaces;

		public float m_SpaceWidth;
	}

	[SerializeField]
	private int m_TextID;

	[SerializeField]
	private string m_Text;

	[SerializeField]
	private HorizontalTextAlignment m_Alignment;

	[SerializeField]
	private float m_LineSpace;

	[SerializeField]
	private string m_FontName = "NewFont";

	[SerializeField]
	private Vector2 m_TextScale = Vector2.one;

	private bool m_RegenerateSprites = true;

	private GUIBase_Widget m_Widget;

	private GUIBase_FontEx m_Font;

	public GUIBase_Widget Widget
	{
		get
		{
			return m_Widget;
		}
	}

	public string text
	{
		get
		{
			return m_Text;
		}
	}

	public float lineSpace
	{
		get
		{
			return m_LineSpace;
		}
	}

	public HorizontalTextAlignment alignment
	{
		get
		{
			return m_Alignment;
		}
	}

	public GUIBase_FontBase font
	{
		get
		{
			return m_Font;
		}
	}

	public Texture2D fontTexture
	{
		get
		{
			return (!m_Font || !m_Font.fontMaterial) ? null : ((Texture2D)m_Font.fontMaterial.mainTexture);
		}
	}

	public void Start()
	{
		m_Widget = GetComponent<GUIBase_Widget>();
		m_Widget.RegisterUpdateDelegate(RegenerateSprites);
		m_Widget.RegisterCallback(this, 1);
		m_Widget.m_TextScaleFix_HACK = true;
		if (m_TextID <= 0 && string.IsNullOrEmpty(m_Text))
		{
			m_Text = "This-is-temporary-test-for-testing-TextArea-auto-wrap. This is tooooooo long line. \n\nThis is temporary test for testing TextArea auto wrap. This is tooooooo long line. This is temporary test for testing TextArea auto wrap. This is tooooooo long line.";
		}
		if (m_Font == null)
		{
			m_Font = MFFontManager.GetFont(m_FontName) as GUIBase_FontEx;
		}
	}

	public override bool Callback(E_CallbackType type)
	{
		if (type == E_CallbackType.E_CT_INIT)
		{
			m_Widget.HACK_Label_RemoveSprite();
			m_Widget.ChangeMaterial(m_Font.fontMaterial);
			return true;
		}
		return false;
	}

	private void RegenerateSprites()
	{
		if (!m_RegenerateSprites || !m_Widget.IsVisible())
		{
			return;
		}
		m_RegenerateSprites = false;
		m_Widget.HACK_Label_RemoveSprite();
		if (m_Font == null)
		{
			m_Font = MFFontManager.GetFont(m_FontName) as GUIBase_FontEx;
			if (m_Font == null)
			{
				Debug.LogError(base.gameObject.GetFullName() + " Can't load font with name " + m_FontName);
				return;
			}
		}
		if (m_TextID > 0)
		{
			m_Text = TextDatabase.instance[m_TextID];
		}
		if (text == null || text.Length <= 0)
		{
			return;
		}
		Vector3 lossyScale = base.gameObject.transform.lossyScale;
		lossyScale = Vector3.one;
		Vector2 vector = new Vector2(m_Widget.GetOrigPos().x - m_Widget.GetWidth() * 0.5f * lossyScale.x, m_Widget.GetOrigPos().y - m_Widget.GetHeight() * 0.5f * lossyScale.y);
		Vector2 vector2 = vector;
		float inMaxlineWidth = m_Widget.GetWidth() * lossyScale.x;
		lossyScale.x = m_TextScale.x;
		lossyScale.y = m_TextScale.y;
		List<TextLine> lines = GetLines(text, m_Font, alignment, inMaxlineWidth, lossyScale, lineSpace);
		if (lines == null || lines.Count <= 0)
		{
			return;
		}
		foreach (TextLine item in lines)
		{
			vector2 = vector + item.m_Offset;
			for (int i = item.m_StartIndex; i < item.m_EndIndex; i++)
			{
				switch (text[i])
				{
				case '\n':
					Debug.LogWarning("function GetLines doesn't work correctly");
					continue;
				case ' ':
					vector2.x += item.m_SpaceWidth;
					continue;
				}
				float outWidth;
				Rect outSprite;
				Rect outTexUV;
				if (m_Font.GetCharDescription(text[i], out outWidth, out outSprite, out outTexUV, false, false, false))
				{
					Vector2 vector3 = new Vector2(outSprite.width * lossyScale.x, outSprite.height * lossyScale.y);
					Vector2 centerSpritePos = vector2 + new Vector2(outSprite.center.x * lossyScale.x, 0f);
					m_Widget.AddSprite(centerSpritePos, vector3.x, vector3.y, lossyScale.x, lossyScale.y, 0f, (int)outTexUV.x, (int)(outTexUV.y + outTexUV.height), (int)outTexUV.width, (int)outTexUV.height);
					vector2.x += outWidth * lossyScale.x;
				}
			}
		}
		m_Widget.SetModify();
	}

	public void SetNewText(string inText)
	{
		if (!(inText == m_Text))
		{
			m_TextID = 0;
			m_Text = inText;
			m_RegenerateSprites = true;
		}
	}

	public void SetNewText(int inTextID)
	{
		if (m_TextID != inTextID)
		{
			m_TextID = inTextID;
			m_Text = string.Empty;
			m_RegenerateSprites = true;
		}
	}

	public void Clear()
	{
		m_TextID = 0;
		m_Text = string.Empty;
		m_RegenerateSprites = true;
	}

	public static List<TextLine> GetLines(string inText, GUIBase_FontEx inFont, HorizontalTextAlignment inAlignment, float inMaxlineWidth, Vector3 inScale, float inLineSpacePct)
	{
		if (string.IsNullOrEmpty(inText))
		{
			return null;
		}
		if (inMaxlineWidth <= 0f)
		{
			return null;
		}
		if (inScale == Vector3.zero)
		{
			return null;
		}
		List<TextLine> list = new List<TextLine>();
		TextLine textLine = null;
		int num = -1;
		float x = 0f;
		float num2 = inFont.GetCharWidth(32) * inScale.x;
		float num3 = 0f;
		float fontHeight = inFont.GetFontHeight();
		for (int i = 0; i < inText.Length; i++)
		{
			if (textLine == null)
			{
				textLine = new TextLine();
				textLine.m_StartIndex = i;
				textLine.m_SpaceWidth = num2;
				num = -1;
				x = 0f;
			}
			int num4 = inText[i];
			switch (num4)
			{
			case 10:
				textLine.m_EndIndex = i;
				textLine.m_EndOfParagraph = true;
				list.Add(textLine);
				textLine = null;
				continue;
			case 32:
				num = i;
				x = textLine.m_Size.x;
				textLine.m_Size.x += num2;
				textLine.m_NumOfSpaces++;
				continue;
			}
			num3 = inFont.GetCharWidth(num4) * inScale.x;
			if (textLine.m_Size.x + num3 > inMaxlineWidth)
			{
				if (num >= 0)
				{
					textLine.m_NumOfSpaces--;
					textLine.m_Size.x = x;
					textLine.m_EndIndex = num;
					i = num;
				}
				else
				{
					textLine.m_EndIndex = i;
					i--;
				}
				textLine.m_EndOfParagraph = false;
				if (textLine.m_EndIndex - textLine.m_StartIndex < 1)
				{
					Debug.LogWarning("Can't generate line for character: " + (char)num4);
				}
				else
				{
					list.Add(textLine);
				}
				textLine = null;
			}
			else
			{
				textLine.m_Size.x += num3;
			}
		}
		if (textLine != null)
		{
			textLine.m_EndIndex = inText.Length;
			textLine.m_EndOfParagraph = true;
			if (textLine.m_EndIndex - textLine.m_StartIndex == 0)
			{
				Debug.LogWarning("Empty line");
			}
			else
			{
				list.Add(textLine);
			}
		}
		float num5 = 0f;
		foreach (TextLine item in list)
		{
			TrimSpaces(inText, item, num2);
			if (item.m_EndIndex - item.m_StartIndex == 0)
			{
				continue;
			}
			float num6 = inMaxlineWidth - item.m_Size.x;
			switch (inAlignment)
			{
			case HorizontalTextAlignment.Left:
				item.m_Offset.x = 0f;
				break;
			case HorizontalTextAlignment.Center:
				item.m_Offset.x = num6 * 0.5f;
				break;
			case HorizontalTextAlignment.Right:
				item.m_Offset.x = num6;
				break;
			case HorizontalTextAlignment.Justify:
				if (!item.m_EndOfParagraph && item.m_NumOfSpaces > 0)
				{
					item.m_SpaceWidth += num6 / (float)item.m_NumOfSpaces;
				}
				break;
			default:
				Debug.LogError("Unknown Horizontal text alignment !!!! " + inAlignment);
				break;
			}
			num5 += 0.5f * fontHeight * inScale.y;
			item.m_Offset.y = num5;
			num5 += 0.5f * fontHeight * inScale.y;
			num5 += inLineSpacePct * fontHeight * inScale.y;
		}
		return list;
	}

	private static int TrimSpaces(string inText, TextLine inLine, float inSpaceWidth)
	{
		int num = 0;
		for (int i = inLine.m_StartIndex; i < inLine.m_EndIndex && inText[i] == ' '; i++)
		{
			inLine.m_StartIndex++;
			num++;
		}
		int num2 = inLine.m_EndIndex - 1;
		while (num2 >= inLine.m_StartIndex && inText[num2] == ' ')
		{
			inLine.m_EndIndex--;
			num++;
			num2--;
		}
		inLine.m_Size.x -= (float)num * inSpaceWidth;
		inLine.m_NumOfSpaces -= num;
		return num;
	}

	public void OnLanguageChanged(string inNewLanguage)
	{
		GUIBase_FontEx gUIBase_FontEx = null;
		gUIBase_FontEx = ((!("English.Old" == inNewLanguage)) ? (MFFontManager.GetFont(m_FontName) as GUIBase_FontEx) : (MFFontManager.GetFont(m_FontName, SystemLanguage.Korean) as GUIBase_FontEx));
		if (gUIBase_FontEx != m_Font)
		{
			m_Font = gUIBase_FontEx;
			m_Widget.HACK_Label_RemoveSprite();
			m_Widget.ChangeMaterial(m_Font.fontMaterial);
			m_RegenerateSprites = true;
		}
	}
}
