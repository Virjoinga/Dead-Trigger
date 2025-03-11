using System.Collections.Generic;
using UnityEngine;

internal class GuiShopUpgradeSprite
{
	private GUIBase_Sprite m_RootSprite;

	private List<GUIBase_Sprite> m_Levels = new List<GUIBase_Sprite>();

	private int m_CurrentLevel;

	private bool m_On;

	public GuiShopUpgradeSprite(GUIBase_Sprite rootSprite)
	{
		m_RootSprite = rootSprite;
		int num = 3;
		for (int i = 1; i <= num; i++)
		{
			string text = "Level" + i;
			GUIBase_Sprite childSprite = GuiBaseUtils.GetChildSprite(m_RootSprite.Widget, text);
			if (!childSprite)
			{
				Debug.LogError("Sprite '" + text + "' not found under upgrade sprite: " + rootSprite.name);
			}
			m_Levels.Add(childSprite);
		}
	}

	private void SetLevel(int level)
	{
		if (level > m_Levels.Count)
		{
			Debug.LogError("Not enough sprites. Trying to set " + level + " sprites count: " + m_Levels.Count);
		}
		else
		{
			m_CurrentLevel = level;
			ShowLevelSprite();
		}
	}

	private void ShowLevelSprite()
	{
		for (int i = 0; i < m_Levels.Count; i++)
		{
			bool v = i + 1 == m_CurrentLevel && m_On;
			m_Levels[i].Widget.Show(v, true);
		}
	}

	public void Show(bool on, int level)
	{
		m_On = on;
		m_RootSprite.Widget.Show(on, true);
		SetLevel(level);
		ShowLevelSprite();
	}
}
