using UnityEngine;

internal class GuiShopFunds
{
	private GUIBase_Sprite m_RootSprite;

	private GUIBase_Label m_FundsLabel;

	private GUIBase_Sprite m_SpriteGold;

	private GUIBase_Sprite m_SpriteMoney;

	public GuiShopFunds(GUIBase_Sprite rootSprite)
	{
		m_RootSprite = rootSprite;
		m_FundsLabel = GuiBaseUtils.GetChildLabel(m_RootSprite.Widget, "Funds_Label");
		m_SpriteGold = GuiBaseUtils.GetChildSprite(m_RootSprite.Widget, "Gold_Sprite");
		m_SpriteMoney = GuiBaseUtils.GetChildSprite(m_RootSprite.Widget, "Money_Sprite");
	}

	public void SetValue(int cost, bool gold, bool plusSign = false)
	{
		Debug.Log("Funds: " + cost + " gold: " + gold);
		string newText = ((!plusSign) ? cost.ToString() : ("+" + cost));
		m_FundsLabel.SetNewText(newText);
		m_SpriteGold.Widget.Show(gold, false);
		m_SpriteMoney.Widget.Show(!gold, false);
	}

	public void Show(bool on)
	{
		m_RootSprite.Widget.Show(on, true);
	}
}
