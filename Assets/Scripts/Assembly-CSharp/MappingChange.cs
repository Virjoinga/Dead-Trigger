using UnityEngine;

public class MappingChange : MonoBehaviour
{
	public enum Type
	{
		BasedOnValue = 0
	}

	public float Value;

	public Type MappingChangeType;

	public Vector2 Tiling1;

	public Vector2 Offset1;

	public Vector2 Tiling2;

	public Vector2 Offset2;

	public Color TextureColor = Color.red;

	private MeshRenderer m_Mesh;

	private Color m_TextureColor;

	private float m_Value;

	private void Awake()
	{
		m_Mesh = base.gameObject.GetComponent<MeshRenderer>();
		if (m_Mesh == null)
		{
			Debug.LogWarning("Can't find mesh renderer on object: " + base.gameObject.name);
		}
	}

	private void Start()
	{
		m_Value = Value;
		ApplyMappingChange();
		m_TextureColor = TextureColor;
		ApplyColorChange();
	}

	private void Update()
	{
		if (!Mathf.Approximately(m_Value, Value))
		{
			m_Value = Value;
			ApplyMappingChange();
		}
		if (m_TextureColor != TextureColor)
		{
			m_TextureColor = TextureColor;
			ApplyColorChange();
		}
	}

	private void ApplyMappingChange()
	{
		float num = 0f;
		if (MappingChangeType == Type.BasedOnValue)
		{
			num = Mathf.Clamp(Value, 0f, 1f);
		}
		m_Mesh.material.mainTextureScale = Tiling1 * num + Tiling2 * (1f - num);
		m_Mesh.material.mainTextureOffset = Offset1 * num + Offset2 * (1f - num);
	}

	private void ApplyColorChange()
	{
		m_Mesh.material.color = m_TextureColor;
	}
}
