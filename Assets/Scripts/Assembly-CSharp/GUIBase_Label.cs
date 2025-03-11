using System.Collections;
using UnityEngine;

[AddComponentMenu("GUI/Widgets/Label")]
public class GUIBase_Label : GUIBase_Callback
{
	public int m_TextID;

	private int m_TextIDGenerated;

	[SerializeField]
	private string m_Text;

	[SerializeField]
	private string m_TextDyn;

	private string m_TextGenerated = string.Empty;

	[SerializeField]
	public string m_FontName = "Default";

	private GUIBase_FontBase m_FontXXX;

	public TextAnchor m_AnchorPoint = TextAnchor.MiddleCenter;

	private int m_AnchorPointGen = -1;

	public TextAlignment m_Alignment;

	private int m_AlignmentGen = -1;

	private GUIBase_Widget m_Widget;

	[SerializeField]
	private float m_LineSpace2;

	private float m_LineSpaceGenerated;

	private Vector2 m_LineSize;

	private bool m_RegenerationNeeded = true;

	public bool m_UseFontExGen;

	public string text
	{
		get
		{
			return (m_TextID <= 0 || m_TextDyn == null) ? m_Text : m_TextDyn;
		}
	}

	public bool isValid
	{
		get
		{
			return text != null && text != string.Empty && (bool)font;
		}
	}

	public Vector2 textSize
	{
		get
		{
			return GetTextSize();
		}
	}

	public Vector2 lineSize
	{
		get
		{
			return m_LineSize;
		}
	}

	public float lineSpace
	{
		get
		{
			return m_LineSpace2 * 0.01f;
		}
	}

	public TextAlignment alignment
	{
		get
		{
			return m_Alignment;
		}
	}

	public GUIBase_Widget Widget
	{
		get
		{
			return m_Widget;
		}
	}

	public bool useFontEx
	{
		get
		{
			return font != null && font is GUIBase_FontEx;
		}
	}

	public GUIBase_FontBase font
	{
		get
		{
			if (m_FontXXX == null)
			{
				m_FontXXX = MFFontManager.GetFont(m_FontName);
			}
			return m_FontXXX;
		}
	}

	public Texture2D fontTexture
	{
		get
		{
			if ((bool)font && (bool)font.fontMaterial)
			{
				return (Texture2D)font.fontMaterial.mainTexture;
			}
			return null;
		}
	}

	public void Start()
	{
		m_Widget = GetComponent<GUIBase_Widget>();
		m_Widget.m_TextScaleFix_HACK = true;
		m_Widget.RegisterUpdateDelegate(RegenerateSprites);
		m_Widget.RegisterCallback(this, 1);
		m_TextDyn = null;
		m_TextIDGenerated = 0;
		m_TextGenerated = string.Empty;
		m_AnchorPointGen = -1;
		m_AlignmentGen = -1;
		m_LineSpaceGenerated = 0f;
		m_RegenerationNeeded = true;
		if (m_TextID != 0)
		{
			m_TextDyn = TextDatabase.instance[m_TextID];
		}
	}

	public override bool Callback(E_CallbackType type)
	{
		if (type == E_CallbackType.E_CT_INIT)
		{
			m_Widget.HACK_Label_RemoveSprite();
			m_Widget.ChangeMaterial(font.fontMaterial);
			m_RegenerationNeeded = true;
			return true;
		}
		return false;
	}

	private void RegenerateSprites()
	{
		if (!IsDataRegenerationNeaded() || !m_Widget.IsVisible())
		{
			return;
		}
		m_Widget.HACK_Label_RemoveSprite();
		GenerateRunTimeData();
		if (text != null && text.Length > 0)
		{
			Vector3 lossyScale = base.gameObject.transform.lossyScale;
			lossyScale = Vector3.one;
			Texture texture = fontTexture;
			int num = ((!texture) ? 1 : texture.width);
			int num2 = ((!texture) ? 1 : texture.height);
			float num3 = m_LineSize.y * (float)num2;
			float x = m_LineSize.x;
			float num4 = m_Widget.GetWidth() / x;
			Vector3 inCursor = GetLeftUpPos(m_Widget.GetOrigPos());
			float x2 = inCursor.x;
			inCursor.y += num3 * lossyScale.y * 0.5f;
			bool flag = IsMultiline(text);
			if (flag)
			{
				inCursor = SetupCursorForTextAlign(inCursor, text, 0, m_Alignment, font, x2, x, num4 * lossyScale.x);
			}
			for (int i = 0; i < text.Length; i++)
			{
				int num5 = text[i];
				int num6 = num5;
				if (num6 == 10)
				{
					if (flag)
					{
						inCursor = SetupCursorForTextAlign(inCursor, text, i + 1, m_Alignment, font, x2, x, num4 * lossyScale.x);
					}
					else
					{
						inCursor.x = x2;
					}
					inCursor.y += (num3 + lineSpace * (float)num2) * lossyScale.y;
					m_Widget.AddSprite(new Vector2(inCursor.x, inCursor.y), 0f, 0f, lossyScale.x, lossyScale.y, 0f, 0, 0, 0, 0);
				}
				else if (!useFontEx)
				{
					Vector2 inTexPos = default(Vector2);
					Vector2 inTexSize = default(Vector2);
					GUIBase_Font gUIBase_Font = font as GUIBase_Font;
					float width;
					gUIBase_Font.GetCharDscr(num5, out width, ref inTexPos, ref inTexSize);
					width *= num4;
					int texU = (int)((float)num * inTexPos.x);
					int num7 = (int)((float)num2 * inTexPos.y);
					int texW = (int)((float)num * inTexSize.x);
					int num8 = (int)((float)num2 * inTexSize.y);
					inCursor.x += 0.5f * width * lossyScale.x;
					m_Widget.AddSprite(new Vector2(inCursor.x, inCursor.y), width, num3, lossyScale.x, lossyScale.y, 0f, texU, num7 + num8, texW, num8);
					inCursor.x += 0.5f * width * lossyScale.x;
				}
				else
				{
					GUIBase_FontEx gUIBase_FontEx = font as GUIBase_FontEx;
					float outWidth;
					Rect outSprite;
					Rect outTexUV;
					if (gUIBase_FontEx.GetCharDescription(text[i], out outWidth, out outSprite, out outTexUV, false, false))
					{
						outWidth *= num4;
						Vector2 vector = new Vector2(outSprite.width, outSprite.height * (float)num2);
						Vector2 centerSpritePos = new Vector2(inCursor.x, inCursor.y) + new Vector2((outSprite.x + outSprite.width) * 0.5f * lossyScale.x, 0f);
						m_Widget.AddSprite(centerSpritePos, vector.x, vector.y, lossyScale.x, lossyScale.y, 0f, (int)outTexUV.x, (int)(outTexUV.y + outTexUV.height), (int)outTexUV.width, (int)outTexUV.height);
						inCursor.x += outWidth * lossyScale.x;
					}
				}
			}
		}
		m_RegenerationNeeded = false;
		m_Widget.SetModify();
	}

	public void SetNewText(string inText)
	{
		if (m_TextID != 0 || !(inText == m_Text))
		{
			Clear();
			m_TextID = 0;
			m_TextIDGenerated = 0;
			if (inText != null)
			{
				m_Text = inText;
			}
			m_RegenerationNeeded = true;
		}
	}

	public void SetNewText(int inTextID)
	{
		if (m_TextID != inTextID)
		{
			Clear();
			m_TextID = inTextID;
			m_TextIDGenerated = 0;
			m_RegenerationNeeded = true;
		}
	}

	public string GetText()
	{
		if (m_TextID != 0)
		{
			return TextDatabase.instance[m_TextID];
		}
		return m_Text;
	}

	public void Clear()
	{
		m_TextID = 0;
		m_TextIDGenerated = -1;
		m_TextDyn = string.Empty;
		m_Text = string.Empty;
		m_RegenerationNeeded = true;
	}

	public static bool IsMultiline(string inText)
	{
		return inText.IndexOf('\n') >= 0;
	}

	private static float GetCurentTextLineWidth(string inText, int inStartIndex, GUIBase_FontBase inFont)
	{
		int num = inText.IndexOf('\n', inStartIndex);
		int num2 = ((num != -1) ? (num - inStartIndex) : (inText.Length - inStartIndex));
		if (num2 <= 0)
		{
			return -1f;
		}
		string subString = inText.Substring(inStartIndex, num2);
		return GetLineWidth(subString, inFont);
	}

	private static float GetLineWidth(string subString, GUIBase_FontBase inFont)
	{
		float num = 0f;
		GUIBase_FontEx gUIBase_FontEx = inFont as GUIBase_FontEx;
		GUIBase_Font gUIBase_Font = inFont as GUIBase_Font;
		if (gUIBase_FontEx != null)
		{
			for (int i = 0; i < subString.Length; i++)
			{
				num += gUIBase_FontEx.GetCharWidth(subString[i]);
			}
		}
		else
		{
			for (int j = 0; j < subString.Length; j++)
			{
				num += gUIBase_Font.GetCharWidth(subString[j]);
			}
		}
		return num;
	}

	private Vector2 GetTextSize()
	{
		Vector2 inTexPos = Vector2.zero;
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		int num = 1;
		float num2 = 0f;
		for (int i = 0; i < text.Length; i++)
		{
			int num3 = text[i];
			int num4 = num3;
			if (num4 == 10)
			{
				zero3.x = Mathf.Max(zero3.x, zero2.x);
				zero2.x = 0f;
				num++;
			}
			else if (!useFontEx)
			{
				zero = Vector2.zero;
				num2 = 0f;
				GUIBase_Font gUIBase_Font = font as GUIBase_Font;
				if (gUIBase_Font.GetCharDscr(num3, out num2, ref inTexPos, ref zero))
				{
					zero2.x += num2;
					zero2.y = Mathf.Max(zero2.y, zero.y);
				}
			}
			else
			{
				num2 = 0f;
				GUIBase_FontEx gUIBase_FontEx = font as GUIBase_FontEx;
				Rect outSprite;
				Rect outTexUV;
				if (gUIBase_FontEx.GetCharDescription(text[i], out num2, out outSprite, out outTexUV, true, false))
				{
					zero2.x += num2;
					zero2.y = Mathf.Max(zero2.y, outSprite.height);
				}
			}
		}
		m_LineSize = zero2;
		zero3.x = Mathf.Max(zero3.x, zero2.x);
		m_LineSize.x = zero3.x;
		zero3.y = zero2.y * (float)num + (float)(num - 1) * lineSpace;
		return zero3;
	}

	internal bool IsDataRegenerationNeaded()
	{
		if (m_RegenerationNeeded)
		{
			return true;
		}
		return false;
	}

	public bool GenerateRunTimeData()
	{
		bool flag = IsDataRegenerationNeaded();
		if (flag)
		{
			if (font == null)
			{
				Debug.LogWarning("GUIBase_Label have not a font assigned " + DebugUtils.GetFullName(base.gameObject));
			}
			else
			{
				GUIBase_Widget component = GetComponent<GUIBase_Widget>();
				m_TextDyn = null;
				m_TextIDGenerated = 0;
				if (m_TextID != 0)
				{
					m_TextDyn = TextDatabase.instance[m_TextID];
					m_TextIDGenerated = m_TextID;
				}
				Texture2D texture2D = fontTexture;
				float num = texture2D.height;
				Vector2 vector = GetTextSize();
				component.SetScreenSize(vector.x, vector.y * num);
				m_AnchorPointGen = (int)m_AnchorPoint;
				m_AlignmentGen = (int)m_Alignment;
				m_LineSpaceGenerated = m_LineSpace2;
				m_UseFontExGen = useFontEx;
			}
		}
		return flag;
	}

	public Vector3 GetLeftUpPos(Vector3 inRefPoint)
	{
		return GetLeftUpPos(m_Widget, inRefPoint, Vector3.one);
	}

	public Vector3 GetLeftUpPos(GUIBase_Widget inWidget)
	{
		return GetLeftUpPos(inWidget, base.transform.position, base.transform.lossyScale);
	}

	public Vector3 GetLeftUpPos(GUIBase_Widget inWidget, Vector3 inRefPoint, Vector3 inScale)
	{
		Vector3 vector = inScale;
		Vector3 vector2 = new Vector3(inWidget.GetWidth() * vector.x, inWidget.GetHeight() * vector.y, 0f);
		Vector3 result = inRefPoint;
		switch (m_AnchorPoint)
		{
		case TextAnchor.UpperLeft:
			result.x -= vector2.x * 0f;
			result.y -= vector2.y * 0f;
			break;
		case TextAnchor.UpperCenter:
			result.x -= vector2.x * 0.5f;
			result.y -= vector2.y * 0f;
			break;
		case TextAnchor.UpperRight:
			result.x -= vector2.x * 1f;
			result.y -= vector2.y * 0f;
			break;
		case TextAnchor.MiddleLeft:
			result.x -= vector2.x * 0f;
			result.y -= vector2.y * 0.5f;
			break;
		case TextAnchor.MiddleCenter:
			result.x -= vector2.x * 0.5f;
			result.y -= vector2.y * 0.5f;
			break;
		case TextAnchor.MiddleRight:
			result.x -= vector2.x * 1f;
			result.y -= vector2.y * 0.5f;
			break;
		case TextAnchor.LowerLeft:
			result.x -= vector2.x * 0f;
			result.y -= vector2.y * 1f;
			break;
		case TextAnchor.LowerCenter:
			result.x -= vector2.x * 0.5f;
			result.y -= vector2.y * 1f;
			break;
		case TextAnchor.LowerRight:
			result.x -= vector2.x * 1f;
			result.y -= vector2.y * 1f;
			break;
		}
		return result;
	}

	public static Vector3 SetupCursorForTextAlign(Vector3 inCursor, string inText, int inStartIndex, TextAlignment inAlignment, GUIBase_FontBase inFont, float inLineBegin, float inMaxlineWidth, float inScale)
	{
		Vector3 result = inCursor;
		result.x = inLineBegin;
		if (inAlignment != 0)
		{
			float curentTextLineWidth = GetCurentTextLineWidth(inText, inStartIndex, inFont);
			float num = (inMaxlineWidth - curentTextLineWidth) * inScale;
			switch (inAlignment)
			{
			case TextAlignment.Center:
				result.x += num * 0.5f;
				break;
			case TextAlignment.Right:
				result.x += num;
				break;
			}
		}
		return result;
	}

	public void OnLanguageChanged(string inNewLanguage)
	{
		if ("English.Old" == inNewLanguage)
		{
			ChangeFont(MFFontManager.GetFont("default"));
		}
		else
		{
			ChangeFont(MFFontManager.GetFont(m_FontName));
		}
	}

	private void ChangeFont(GUIBase_FontBase inNewFont)
	{
		m_FontXXX = inNewFont;
		m_RegenerationNeeded = true;
		m_Widget.HACK_Label_RemoveSprite();
		m_Widget.ChangeMaterial(m_FontXXX.fontMaterial);
	}

	private IEnumerator GenerateRandomText_Coroutine(int inMinSize, int inMaxSize)
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(1, 3));
			yield return new WaitForFixedUpdate();
			SetNewText(DebugUtils.GetRandomString(Random.Range(inMinSize, inMaxSize)));
		}
	}
}
