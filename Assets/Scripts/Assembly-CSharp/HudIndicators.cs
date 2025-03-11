using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudIndicators : HudComponent
{
	private class MoneyInfo
	{
		private GUIBase_Widget m_Parent;

		private GUIBase_Label m_Number;

		private int CurrentMoney;

		private int FinalMoney;

		private float m_Progress;

		public MoneyInfo(GUIBase_Layout layout, int moneyInit)
		{
			m_Parent = layout.GetWidget("Money");
			m_Number = layout.GetWidget("MoneyText").GetComponent<GUIBase_Label>();
			FinalMoney = (CurrentMoney = moneyInit);
			m_Number.SetNewText(FinalMoney.ToString());
			m_Progress = 0f;
		}

		public void Enable(bool enable)
		{
			m_Parent.Show(enable, true);
		}

		public void MoneyChanges()
		{
			m_Progress = 0f;
			FinalMoney = Game.Instance.PlayerPersistentInfo.roundMoney;
		}

		public void Update()
		{
			if (CurrentMoney != FinalMoney)
			{
				m_Progress = Mathf.Min(m_Progress + TimeManager.Instance.GetRealDeltaTime() * 4f, 1f);
				CurrentMoney = Mathf.FloorToInt(Mathf.Lerp(CurrentMoney, FinalMoney, m_Progress));
				m_Number.SetNewText(CurrentMoney.ToString());
			}
		}
	}

	private class HealthAnimator
	{
		private const float decreaseTimeLimit = 0.4f;

		private const float increaseTimeLimit = 0.4f;

		private float decreaseTimer = 1000f;

		private float increaseTimer = 1000f;

		private Vector3 origPosition1;

		private Vector3 origPosition2;

		private GUIBase_Sprite sprite1;

		private GUIBase_Sprite sprite2;

		public HealthAnimator(GUIBase_Sprite inSprite1, GUIBase_Sprite inSprite2)
		{
			sprite1 = inSprite1;
			sprite2 = inSprite2;
			origPosition1 = sprite1.transform.localPosition;
			if ((bool)sprite2)
			{
				origPosition2 = sprite2.transform.localPosition;
			}
		}

		public void DecreaseHealthAnimation()
		{
			decreaseTimer = 0f;
		}

		public void IncreaseHealthAnimation()
		{
			increaseTimer = 0f;
		}

		public void SetPartialHealthEffect(float ratio)
		{
			sprite2.Widget.m_FadeAlpha = ratio;
			sprite2.Widget.m_Color = Color.white * ratio;
		}

		public bool IsPartialHealthEffectActive()
		{
			return sprite2.Widget.m_FadeAlpha < 0.99f;
		}

		public void Update(float time)
		{
			if (increaseTimer < 0.4f)
			{
				float num = increaseTimer / 0.4f;
				float num2 = Mathf.Sin((float)Math.PI * num);
				Vector3 localScale = new Vector3(1f + num2 * 0.33f, 1f + num2 * 0.33f, 1f + num2 * 0.33f);
				sprite1.transform.localScale = localScale;
				sprite2.transform.localScale = localScale;
				if (increaseTimer < 0.2f)
				{
					increaseTimer += 2f * time;
				}
				else
				{
					increaseTimer += 0.5f * time;
				}
				increaseTimer = Mathf.Clamp(increaseTimer, 0f, 0.41f);
				if (increaseTimer >= 0.4f)
				{
					localScale = new Vector3(1f, 1f, 1f);
					sprite1.transform.localScale = localScale;
					sprite2.transform.localScale = localScale;
				}
			}
			if (decreaseTimer < 0.4f)
			{
				float num3 = (float)Screen.width * 0.015f * Mathf.Sin((float)Math.PI * (decreaseTimer * 13f / 0.4f));
				sprite1.transform.localPosition = new Vector3(origPosition1.x + num3, origPosition1.y + num3 / 2f, origPosition1.y);
				sprite2.transform.localPosition = new Vector3(origPosition2.x + num3, origPosition2.y + num3 / 2f, origPosition2.y);
				decreaseTimer = Mathf.Clamp(decreaseTimer + time, 0f, 0.41f);
				if (decreaseTimer >= 0.4f)
				{
					sprite1.transform.localPosition = origPosition1;
					sprite2.transform.localPosition = origPosition2;
				}
			}
		}
	}

	private class ObjectHP
	{
		public GUIBase_Label m_Text;

		public GUIBase_Widget m_Parent;

		public bool highlight;

		public GameObject obj;

		private bool visible;

		private bool repairing;

		public bool used
		{
			get
			{
				return obj != null;
			}
		}

		public void Repair(bool repair)
		{
			repairing = repair;
		}

		public bool IsRepairing()
		{
			return repairing;
		}

		public void Use(GameObject inObj)
		{
			obj = inObj;
			Show();
		}

		public void Free()
		{
			obj = null;
			Hide();
		}

		public void Show()
		{
			m_Parent.Show(true, true);
			visible = true;
		}

		public void Hide(bool setVisibilityFlag = true)
		{
			m_Parent.Show(false, true);
			if (setVisibilityFlag)
			{
				visible = false;
			}
		}

		public bool IsVisible()
		{
			return visible;
		}
	}

	private const float CritHealthBlendIn = 0.75f;

	private const float CritHealthShowHold = 0.1f;

	private const float CritHealthBlendOut = 0.75f;

	private const float CritHealthHideHold = 0.1f;

	private const float HitHealthBlendIn = 0.3f;

	private const float HitHealthShowHold = 0.25f;

	private const float HitHealthBlendOut = 0.5f;

	private const float HitHealthHideHold = 0.1f;

	private static Color red = new Color(1f, 0f, 0f);

	private static Color normal = new Color(0.488f, 0.71f, 0.078f);

	private List<ObjectHP> ObjectHPs = new List<ObjectHP>();

	private MoneyInfo Money;

	private bool m_HealthInitialized;

	private int m_ActualHealthSegments;

	private float m_ActualHealthSegmentsFloat;

	private GUIBase_Widget m_HealthParent;

	private GUIBase_Widget m_HealthText;

	private GUIBase_Sprite[] m_Health;

	private GUIBase_Sprite[] m_HealthFull;

	private GUIBase_Widget m_HealthCritical;

	private float m_HealthCriticalMaxAlpha;

	private float m_HealthCriticalTimer;

	private float m_HealthHitTimer = -1f;

	private HealthAnimator[] m_HealthAnimators;

	private GUIBase_Widget m_CounterParent;

	private GUIBase_Label m_Counter;

	private E_MissionType m_MissionType = E_MissionType.None;

	private GUIBase_Widget m_KillAllZombies;

	private GUIBase_Widget m_ProtectObjects;

	private GUIBase_Widget m_TimeDefense;

	private GUIBase_Widget m_CarryObjects;

	private GUIBase_Widget m_RepairIndicator;

	private bool m_RepairIndicatorShow;

	private bool m_CounterShow;

	private GUIBase_Widget m_CarryObjectParent;

	private bool m_CarryObjectShow;

	private string s_PivotMainName = "MainHUD";

	private string s_LayoutMainName = "HUD_Layout";

	private string s_HealthParent = "Health";

	private string s_HealthName = "Health";

	private string s_HealthTextName = "HealthText";

	private string s_HealthFullName = "HealthFull";

	private string s_HealthCriticalName = "CriticalIndicator";

	private string s_CounterParent = "Counter";

	private string s_CarryObjectParent = "CarryObject";

	private string s_ObjectHPParent1 = "Shield1";

	private string s_ObjectHPParent2 = "Shield2";

	private string s_ObjectHPText1 = "Shield1Text";

	private string s_ObjectHPText2 = "Shield2Text";

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
		GUIBase_Layout layout = pivot.GetLayout(s_LayoutMainName);
		if (!layout)
		{
			Debug.LogError("'" + s_LayoutMainName + "' not found!!! Assert should come now");
			return;
		}
		m_HealthParent = layout.GetWidget(s_HealthParent).GetComponent<GUIBase_Widget>();
		m_HealthText = layout.GetWidget(s_HealthTextName).GetComponent<GUIBase_Widget>();
		m_HealthCritical = layout.GetWidget(s_HealthCriticalName).GetComponent<GUIBase_Widget>();
		m_Health = new GUIBase_Sprite[8];
		m_HealthFull = new GUIBase_Sprite[8];
		m_HealthAnimators = new HealthAnimator[8];
		for (int i = 0; i < 8; i++)
		{
			m_Health[i] = layout.GetWidget(s_HealthName + i).GetComponent<GUIBase_Sprite>();
			m_HealthFull[i] = layout.GetWidget(s_HealthFullName + i).GetComponent<GUIBase_Sprite>();
			m_Health[i].Widget.Show(false, true);
			m_HealthFull[i].Widget.Show(false, true);
		}
		m_HealthInitialized = false;
		m_CounterParent = layout.GetWidget(s_CounterParent).GetComponent<GUIBase_Widget>();
		m_Counter = m_CounterParent.GetComponentInChildren<GUIBase_Label>();
		m_KillAllZombies = layout.GetWidget("KillAllZombies").GetComponent<GUIBase_Widget>();
		m_TimeDefense = layout.GetWidget("TimeDefense").GetComponent<GUIBase_Widget>();
		m_ProtectObjects = layout.GetWidget("ProtectObjects").GetComponent<GUIBase_Widget>();
		m_CarryObjects = layout.GetWidget("CarryObjects").GetComponent<GUIBase_Widget>();
		m_RepairIndicator = layout.GetWidget("Repair").GetComponent<GUIBase_Widget>();
		SetCounter(0);
		m_CarryObjectParent = layout.GetWidget(s_CarryObjectParent).GetComponent<GUIBase_Widget>();
		ObjectHP objectHP = new ObjectHP();
		objectHP.m_Parent = layout.GetWidget(s_ObjectHPParent1).GetComponent<GUIBase_Widget>();
		objectHP.m_Text = layout.GetWidget(s_ObjectHPText1).GetComponent<GUIBase_Label>();
		ObjectHPs.Add(objectHP);
		objectHP = new ObjectHP();
		objectHP.m_Parent = layout.GetWidget(s_ObjectHPParent2).GetComponent<GUIBase_Widget>();
		objectHP.m_Text = layout.GetWidget(s_ObjectHPText2).GetComponent<GUIBase_Label>();
		ObjectHPs.Add(objectHP);
		Money = new MoneyInfo(layout, 0);
		PlayerPersistantInfo playerPersistentInfo = Game.Instance.PlayerPersistentInfo;
		playerPersistentInfo.OnRoundMoneyChanged = (PlayerPersistantInfo.PersistenInfoChanged)Delegate.Combine(playerPersistentInfo.OnRoundMoneyChanged, new PlayerPersistantInfo.PersistenInfoChanged(Money.MoneyChanges));
	}

	public override void Reset()
	{
		m_ActualHealthSegments = 0;
		m_ActualHealthSegmentsFloat = 0f;
	}

	public override void LateUpdate(float deltaTime)
	{
		bool flag = false;
		if (!m_HealthInitialized)
		{
			InitHealth();
			flag = true;
		}
		base.LateUpdate(deltaTime);
		if (!IsVisible() && !flag)
		{
			return;
		}
		UpdateHealth(flag);
		UpdateCriticalHealth(deltaTime);
		HealthAnimator[] healthAnimators = m_HealthAnimators;
		foreach (HealthAnimator healthAnimator in healthAnimators)
		{
			if (healthAnimator != null)
			{
				healthAnimator.Update(deltaTime);
			}
		}
		Money.Update();
	}

	protected override void ShowWidgets(bool on)
	{
		m_HealthParent.Show(on, !on);
		if (on)
		{
			UpdateHealth(true);
			UpdateCriticalHealth(0.01f);
		}
		if (m_RepairIndicatorShow)
		{
			m_RepairIndicator.Show(on, true);
		}
		else
		{
			m_RepairIndicator.Show(false, true);
		}
		m_HealthText.Show(on, true);
		if (m_CounterShow)
		{
			if (on)
			{
				ShowCounter(true);
			}
			else
			{
				m_CounterParent.Show(false, true);
			}
		}
		else
		{
			m_CounterParent.Show(false, true);
		}
		if (m_CarryObjectShow)
		{
			m_CarryObjectParent.Show(on, true);
		}
		else
		{
			m_CarryObjectParent.Show(false, true);
		}
		foreach (ObjectHP objectHP in ObjectHPs)
		{
			if (objectHP.used)
			{
				if (on && objectHP.IsVisible())
				{
					objectHP.Show();
				}
				else if (!on)
				{
					objectHP.Hide(false);
				}
			}
		}
		Money.Enable(on);
	}

	public void SetMissionType(E_MissionType missionType)
	{
		m_MissionType = missionType;
	}

	public void SetCounter(int val)
	{
		m_Counter.SetNewText(val.ToString());
	}

	public void SetCounterTime(float seconds)
	{
		m_Counter.SetNewText(FormatTime(seconds));
	}

	public void ShowCounter(bool show)
	{
		m_CounterParent.Show(show, true);
		m_KillAllZombies.Show(false, true);
		m_TimeDefense.Show(false, true);
		m_ProtectObjects.Show(false, true);
		m_CarryObjects.Show(false, true);
		if (show)
		{
			switch (m_MissionType)
			{
			case E_MissionType.KillZombies:
				m_KillAllZombies.Show(true, true);
				break;
			case E_MissionType.TimeDefense:
				m_TimeDefense.Show(true, true);
				break;
			case E_MissionType.ProtectObjects:
				m_ProtectObjects.Show(true, true);
				break;
			case E_MissionType.CarryResources:
				m_CarryObjects.Show(true, true);
				break;
			}
		}
		m_CounterShow = show;
	}

	public void ShowCarryObjectIcon(bool show)
	{
		m_CarryObjectParent.Show(show, true);
		m_CarryObjectShow = show;
	}

	public void RegisterObjectHP(GameObject obj)
	{
		foreach (ObjectHP objectHP in ObjectHPs)
		{
			if (!objectHP.used)
			{
				objectHP.Use(obj);
				return;
			}
		}
		Debug.LogWarning("RegisterObjectHP - can't find free sprite for object: " + obj.name);
	}

	public void UnregisterObjectHP(GameObject obj)
	{
		foreach (ObjectHP objectHP in ObjectHPs)
		{
			if (objectHP.obj == obj)
			{
				objectHP.m_Parent.StopAllCoroutines();
				objectHP.Free();
				return;
			}
		}
		Debug.LogWarning("UnregisterObjectH - can't find object: " + obj.name);
	}

	public void SetObjectHP(GameObject obj, float HP, bool highlight)
	{
		ObjectHP objectHP = null;
		foreach (ObjectHP objectHP2 in ObjectHPs)
		{
			if (objectHP2.obj == obj)
			{
				objectHP = objectHP2;
				break;
			}
		}
		if (objectHP == null)
		{
			Debug.LogWarning("Unregistered 'ObjectHP' object: " + obj.name);
			return;
		}
		objectHP.m_Text.SetNewText(Mathf.CeilToInt(HP * 100f) + "%");
		if (!highlight)
		{
			objectHP.m_Parent.m_Color = normal;
		}
		if (objectHP.highlight != highlight)
		{
			objectHP.highlight = highlight;
			if (!highlight)
			{
				objectHP.m_Parent.StopAllCoroutines();
			}
			else
			{
				objectHP.m_Parent.StartCoroutine(HighlightObject(objectHP.m_Parent));
			}
		}
	}

	public void ShowRepairIndicator(GameObject obj, bool show)
	{
		bool flag = false;
		foreach (ObjectHP objectHP in ObjectHPs)
		{
			if (objectHP.used)
			{
				if (objectHP.obj == obj)
				{
					objectHP.Repair(show);
				}
				if (objectHP.IsRepairing())
				{
					flag = true;
				}
			}
		}
		if (flag && !m_RepairIndicatorShow)
		{
			m_RepairIndicator.Show(true, true);
			m_RepairIndicatorShow = true;
			m_RepairIndicator.StartCoroutine(HighlightObject2(m_RepairIndicator));
		}
		else if (!flag && m_RepairIndicatorShow)
		{
			m_RepairIndicator.StopAllCoroutines();
			m_RepairIndicator.Show(false, true);
			m_RepairIndicatorShow = false;
		}
	}

	private IEnumerator HighlightObject(GUIBase_Widget sprite)
	{
		while (true)
		{
			sprite.m_Color = red;
			yield return new WaitForSeconds(0.13f);
			sprite.m_Color = normal;
			yield return new WaitForSeconds(0.1f);
		}
	}

	private IEnumerator HighlightObject2(GUIBase_Widget sprite)
	{
		while (true)
		{
			sprite.m_FadeAlpha = 0.33f;
			yield return new WaitForSeconds(0.25f);
			sprite.m_FadeAlpha = 0.66f;
			yield return new WaitForSeconds(0.25f);
		}
	}

	private int GetMaxHealthSegments()
	{
		float f = Player.Instance.Owner.BlackBoard.RealMaxHealth / Player.HealthSegment;
		return Mathf.CeilToInt(f);
	}

	private float GetActualHealthSegmentsFloat()
	{
		return Player.Instance.Owner.BlackBoard.Health / Player.HealthSegment;
	}

	private int GetActualHealthSegments()
	{
		return Mathf.CeilToInt(GetActualHealthSegmentsFloat());
	}

	private void InitHealth()
	{
		if ((bool)Player.Instance && (bool)Player.Instance.Owner)
		{
			int maxHealthSegments = GetMaxHealthSegments();
			Vector3 vector = m_Health[1].transform.localPosition - m_Health[0].transform.localPosition;
			Vector3 localPosition = m_Health[0].transform.localPosition;
			for (int i = 0; i < maxHealthSegments; i++)
			{
				m_Health[i].Widget.Show(true, true);
				m_HealthAnimators[i] = new HealthAnimator(m_Health[i], m_HealthFull[i]);
				localPosition += vector;
			}
			m_ActualHealthSegments = maxHealthSegments;
			m_ActualHealthSegmentsFloat = GetActualHealthSegmentsFloat();
			m_HealthInitialized = true;
		}
	}

	private void UpdateCriticalHealth(float deltaTime)
	{
		if (!Player.Instance || !Player.Instance.Owner || !IsVisible())
		{
			return;
		}
		float num = GetActualHealthSegments();
		if (num > 2.1f && m_HealthHitTimer < 0f)
		{
			m_HealthCritical.Show(false, false);
			return;
		}
		if (!m_HealthCritical.IsVisible())
		{
			m_HealthCritical.Show(true, false);
		}
		if (m_HealthHitTimer >= 0f)
		{
			m_HealthCriticalMaxAlpha = 0.7f;
		}
		else if (num > 1.1f)
		{
			m_HealthCriticalMaxAlpha = 0.35f;
		}
		else
		{
			m_HealthCriticalMaxAlpha = 0.5f;
		}
		float num2 = 0f;
		if (m_HealthHitTimer >= 0f)
		{
			m_HealthHitTimer += deltaTime;
			m_HealthCriticalTimer = 0f;
			num2 = ComputeAlpha(ref m_HealthHitTimer, m_HealthCriticalMaxAlpha, 0.3f, 0.25f, 0.5f, 0.1f, true);
		}
		else
		{
			m_HealthCriticalTimer += deltaTime;
			num2 = ComputeAlpha(ref m_HealthCriticalTimer, m_HealthCriticalMaxAlpha, 0.75f, 0.1f, 0.75f, 0.1f, false);
		}
		m_HealthCritical.m_FadeAlpha = num2;
	}

	private float ComputeAlpha(ref float timer, float maxAlpha, float blendIn, float showHold, float blendOut, float hideHold, bool onlyOnce)
	{
		float result = 0f;
		if (timer > blendIn)
		{
			if (timer <= blendIn + showHold)
			{
				result = maxAlpha;
			}
			else if (timer <= blendIn + showHold + blendOut)
			{
				result = maxAlpha * (1f - (timer - blendIn - showHold) / blendOut);
			}
			else if (timer <= blendIn + showHold + blendOut + hideHold)
			{
				result = 0f;
			}
			else if (onlyOnce)
			{
				timer = -1f;
			}
			else
			{
				timer -= blendIn + showHold + blendOut + hideHold;
			}
		}
		if (timer <= blendIn)
		{
			result = maxAlpha * (timer / blendIn);
		}
		return result;
	}

	private void UpdateHealth(bool forced = false)
	{
		if (!m_HealthInitialized)
		{
			return;
		}
		int maxHealthSegments = GetMaxHealthSegments();
		int actualHealthSegments = GetActualHealthSegments();
		float num = GetActualHealthSegmentsFloat() - m_ActualHealthSegmentsFloat;
		if ((Mathf.Abs(num) >= 0.01f || actualHealthSegments != m_ActualHealthSegments) ? true : false)
		{
			if (num < 0f)
			{
				for (int i = 0; i < maxHealthSegments; i++)
				{
					m_HealthAnimators[i].DecreaseHealthAnimation();
				}
				m_HealthHitTimer = 0f;
			}
			float partialHealthEffect = Mathf.Abs(1f - (float)actualHealthSegments + GetActualHealthSegmentsFloat());
			for (int j = 0; j < maxHealthSegments; j++)
			{
				if (num < 0f && j == actualHealthSegments - 1)
				{
					m_HealthAnimators[j].SetPartialHealthEffect(partialHealthEffect);
					continue;
				}
				if (m_HealthAnimators[j].IsPartialHealthEffectActive())
				{
					m_HealthAnimators[j].IncreaseHealthAnimation();
				}
				m_HealthAnimators[j].SetPartialHealthEffect(1f);
			}
		}
		m_ActualHealthSegmentsFloat = GetActualHealthSegmentsFloat();
		if (!forced && actualHealthSegments == m_ActualHealthSegments)
		{
			return;
		}
		if (actualHealthSegments > m_ActualHealthSegments)
		{
			for (int k = m_ActualHealthSegments; k < actualHealthSegments; k++)
			{
				m_HealthAnimators[k].IncreaseHealthAnimation();
			}
		}
		for (int l = 0; l < actualHealthSegments; l++)
		{
			m_Health[l].Widget.Show(true, true);
		}
		for (int m = actualHealthSegments; m < maxHealthSegments; m++)
		{
			m_Health[m].Widget.Show(true, false);
			m_HealthFull[m].Widget.Show(false, true);
		}
		m_ActualHealthSegments = actualHealthSegments;
	}

	private string FormatTime(float timeInSeconds)
	{
		int num = Mathf.FloorToInt(timeInSeconds / 60f);
		int num2 = Mathf.RoundToInt(timeInSeconds) % 60;
		if (num2 < 10)
		{
			return num + ":0" + num2;
		}
		return num + ":" + num2;
	}
}
