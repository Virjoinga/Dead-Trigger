using UnityEngine;

public class HudIngameBuy : HudComponent
{
	private GUIBase_Layout m_Dialog;

	private Vector3 m_OrigPos;

	private GUIBase_Widget m_Parent;

	private bool m_ShowIngameBuy;

	private IngameBuy m_IngameBuyComponent;

	private GuiHUD.IngameBuyCallback m_Callback;

	private GUIBase_Label m_Description;

	private GUIBase_Label m_Money;

	private GUIBase_Label m_NotEnoughMoney;

	private GUIBase_Label m_Cost;

	private GUIBase_Widget m_Image;

	private GUIBase_Button m_BuyButton;

	private GUIBase_Label m_ButtonText;

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "IngameBuy";

	public override bool VisibleOnStart()
	{
		return true;
	}

	public override void Init()
	{
		GUIBase_Pivot pivot = MFGuiManager.Instance.GetPivot(s_PivotMainName);
		if (!pivot)
		{
			Debug.LogError("'" + s_PivotMainName + "' not found!!! Assert should come now");
			return;
		}
		m_Dialog = pivot.GetLayout(s_LayoutMainName);
		if (!m_Dialog)
		{
			Debug.LogError("'" + s_LayoutMainName + "' not found!!! Assert should come now");
			return;
		}
		m_OrigPos = m_Dialog.transform.position;
		m_Parent = m_Dialog.GetWidget("Parent").GetComponent<GUIBase_Widget>();
		m_Description = m_Dialog.GetWidget("Item_Description").GetComponent<GUIBase_Label>();
		m_Money = m_Dialog.GetWidget("Inventory_Money").GetComponent<GUIBase_Label>();
		m_NotEnoughMoney = m_Dialog.GetWidget("Item_NotEnoughMoney").GetComponent<GUIBase_Label>();
		m_Cost = m_Dialog.GetWidget("Item_Cost").GetComponent<GUIBase_Label>();
		m_Image = m_Dialog.GetWidget("Item_Picture").GetComponent<GUIBase_Widget>();
		m_BuyButton = m_Dialog.GetWidget("Button_Ok").GetComponent<GUIBase_Button>();
		m_ButtonText = m_Dialog.GetWidget("Button_Caption").GetComponent<GUIBase_Label>();
	}

	public override void Reset()
	{
	}

	public override void LateUpdate(float deltaTime)
	{
		base.LateUpdate(deltaTime);
		if (IsVisible() && m_ShowIngameBuy)
		{
		}
	}

	public override void LateUpdate200ms()
	{
		if (IsVisible() && m_ShowIngameBuy)
		{
			UpdateIngameBuyData(false);
		}
	}

	protected override void ShowWidgets(bool on)
	{
		if (m_ShowIngameBuy)
		{
			MFGuiManager.Instance.ShowLayout(m_Dialog, on);
			m_Parent.Show(on, true);
			if (on)
			{
				UpdateIngameBuyData(true);
			}
		}
		else
		{
			MFGuiManager.Instance.ShowLayout(m_Dialog, false);
		}
	}

	public void ShowIngameBuy(IngameBuy ingameBuy, GuiHUD.IngameBuyCallback closeCallback)
	{
		m_IngameBuyComponent = ingameBuy;
		m_Callback = closeCallback;
		GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "Button_Ok", CloseDialog, null);
		ShowIngameBuy(true);
	}

	public void HideIngameBuy()
	{
		ShowIngameBuy(false);
		m_IngameBuyComponent = null;
		m_Callback = null;
	}

	public void SimulateIngameBuyAccept()
	{
		CloseDialog();
	}

	private void CloseDialog()
	{
		if (m_Callback != null)
		{
			m_Callback(true);
		}
	}

	private void UpdateIngameBuyData(bool forceUpdate)
	{
		CanBuy canBuy = m_IngameBuyComponent.IsBuyPossible();
		if (canBuy == CanBuy.HealthFull || canBuy == CanBuy.AmmoFull)
		{
			if (canBuy == CanBuy.HealthFull)
			{
				m_Cost.SetNewText(3000450);
			}
			else
			{
				m_Cost.SetNewText(3000460);
			}
			if (m_Cost.transform.localScale.x > 1.0001f)
			{
				m_Cost.transform.localScale = Vector3.one;
			}
		}
		else
		{
			m_Cost.SetNewText(m_IngameBuyComponent.GetCost() + " $");
			if (m_Cost.transform.localScale.x < 1.2f)
			{
				m_Cost.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
			}
		}
		string text = TextDatabase.instance[3000430];
		text = text.Replace("%d", m_IngameBuyComponent.GetPPIMoney().ToString());
		m_Money.SetNewText(text);
		if (canBuy == CanBuy.Yes)
		{
			if (m_BuyButton.IsDisabled() || forceUpdate)
			{
				m_BuyButton.SetDisabled(false);
				m_ButtonText.Widget.m_Color = Color.white;
				m_NotEnoughMoney.Widget.Show(false, true);
			}
		}
		else if (!m_BuyButton.IsDisabled() || forceUpdate)
		{
			m_BuyButton.SetDisabled(true);
			m_ButtonText.Widget.m_Color = Color.gray;
			if (canBuy == CanBuy.NotEnoughMoney)
			{
				m_NotEnoughMoney.SetNewText(3000440);
				m_NotEnoughMoney.Widget.Show(true, true);
			}
			else
			{
				m_NotEnoughMoney.Widget.Show(false, true);
			}
		}
	}

	private void ShowIngameBuy(bool show)
	{
		m_ShowIngameBuy = show;
		if (!IsVisible())
		{
			return;
		}
		m_Dialog.StopAllCoroutines();
		m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
		if (show)
		{
			m_Dialog.StartCoroutine(CityGUIResources.InternalShowHideDialog(m_Dialog, m_OrigPos, 0.1f, true));
			m_Parent.Show(true, true);
		}
		else
		{
			m_Dialog.StartCoroutine(CityGUIResources.InternalShowHideDialog(m_Dialog, m_OrigPos, 0.15f, false));
		}
		if (show)
		{
			UpdateIngameBuyData(true);
			m_Image.CopyMaterialSettings(m_IngameBuyComponent.GetIcon());
			string text = TextDatabase.instance[m_IngameBuyComponent.Description()];
			int additionalDescription = m_IngameBuyComponent.GetAdditionalDescription();
			if (additionalDescription > 0)
			{
				text = text.Replace("%s", TextDatabase.instance[additionalDescription].ToString());
			}
			m_Description.SetNewText(text);
		}
	}

	private T GetChildByName<T>(GUIBase_Widget widget, string name) where T : Component
	{
		Transform transform = widget.transform.Find(name);
		return (!(transform != null)) ? ((T)null) : transform.GetComponent<T>();
	}
}
