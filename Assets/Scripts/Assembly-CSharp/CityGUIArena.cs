using System.Collections;
using UnityEngine;

public class CityGUIArena
{
	private class ArenaInfo
	{
		public GUIBase_Button Accept;

		public GUIBase_Label Caption;

		public GUIBase_Label Score;

		public GUIBase_Widget Preview;

		public void Init(GUIBase_Widget parent)
		{
			Accept = GetChildByName<GUIBase_Button>(parent, "ButtonAccept");
			Caption = GetChildByName<GUIBase_Label>(parent, "Caption");
			Score = GetChildByName<GUIBase_Label>(parent, "Score");
			Preview = GetChildByName<GUIBase_Widget>(parent, "Preview");
		}
	}

	private class ArenaStart : CityBaseScreen
	{
		private GUIBase_Layout m_Dialog;

		private ArenaActivated m_ArenaDelegate;

		private Vector3 m_OrigPos;

		public ArenaStart()
			: base(CityScreen.ArenaStart)
		{
		}

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("ArenaStart");
			m_OrigPos = m_Dialog.transform.localPosition;
		}

		public void Show(ArenaActivated arena, GUIBase_Button.TouchDelegate close, CityArenaInfo arenaInfo)
		{
			base.Show(close);
			m_Dialog.StopAllCoroutines();
			m_ArenaDelegate = arena;
			GUIBase_Button.TouchDelegate[] array = new GUIBase_Button.TouchDelegate[4] { ArenaButton1, ArenaButton2, ArenaButton3, ArenaButton4 };
			ArenaInfo[] array2 = new ArenaInfo[array.Length];
			int num = 0;
			bool flag = Game.Instance.PlayerPersistentInfo.EquipList.Weapons.Count > 0;
			for (int i = 0; i < array.Length; i++)
			{
				GUIBase_Widget component = m_Dialog.GetWidget("Arena" + (i + 1)).GetComponent<GUIBase_Widget>();
				ArenaInfo arenaInfo2 = new ArenaInfo();
				arenaInfo2.Init(component);
				if (flag)
				{
					arenaInfo2.Accept.RegisterTouchDelegate(array[i]);
				}
				else
				{
					arenaInfo2.Accept.RegisterTouchDelegate(null);
				}
				num++;
				arenaInfo2.Caption.SetNewText(arenaInfo.arenas[i].name);
				arenaInfo2.Score.SetNewText(arenaInfo.HiScore(arenaInfo.arenas[i].arenaId).ToString());
				string text = "MissionPreviews/" + arenaInfo.arenas[i].levelPreview;
				GameObject gameObject = Resources.Load(text) as GameObject;
				if (gameObject == null)
				{
					Debug.LogWarning("Cant find prefab: " + text);
				}
				else
				{
					GUIBase_Widget component2 = gameObject.GetComponent<GUIBase_Widget>();
					if ((bool)component2)
					{
						arenaInfo2.Preview.CopyMaterialSettings(component2);
					}
				}
				array2[i] = arenaInfo2;
			}
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", close, null);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonCloseVirtual", close, null);
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			m_Dialog.StartCoroutine(CityGUIResources.ShowDialog(m_Dialog, m_OrigPos));
		}

		public void DisableControls(bool disable)
		{
			m_Dialog.EnableControls(!disable);
		}

		public override void Hide()
		{
			base.Hide();
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", null, null);
			m_Dialog.StopAllCoroutines();
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			m_Dialog.StartCoroutine(CityGUIResources.HideDialog(m_Dialog, m_OrigPos));
		}

		private void ArenaButton1()
		{
			m_ArenaDelegate(0);
		}

		private void ArenaButton2()
		{
			m_ArenaDelegate(1);
		}

		private void ArenaButton3()
		{
			m_ArenaDelegate(2);
		}

		private void ArenaButton4()
		{
			m_ArenaDelegate(3);
		}
	}

	private class ArenaResultDlg : CityBaseScreen
	{
		private GUIBase_Layout m_Dialog;

		private Vector3 m_OrigPos;

		private AnimatedNumber MissionSpentMoney;

		private AnimatedNumber HeadShotsMoney;

		private AnimatedNumber RemovedLimbsMoney;

		private AnimatedNumber DesintegrationsMoney;

		private AnimatedNumber MoneyFoundMoney;

		private AnimatedNumber MissionEarnedValue;

		private AnimatedNumber ProfitTotalMoney;

		public ArenaResultDlg()
			: base(CityScreen.ArenaResult)
		{
		}

		public void Init(GUIBase_Pivot pivot)
		{
			m_Dialog = pivot.GetLayout("ArenaResult");
			m_OrigPos = m_Dialog.transform.localPosition;
			MissionSpentMoney = new AnimatedNumber(m_Dialog.GetWidget("MoneySpentMoney").GetComponent<GUIBase_Label>());
			HeadShotsMoney = new AnimatedNumber(m_Dialog.GetWidget("HeadShotsMoney").GetComponent<GUIBase_Label>());
			RemovedLimbsMoney = new AnimatedNumber(m_Dialog.GetWidget("RemovedLimbsMoney").GetComponent<GUIBase_Label>());
			DesintegrationsMoney = new AnimatedNumber(m_Dialog.GetWidget("DesintegrationsMoney").GetComponent<GUIBase_Label>());
			MoneyFoundMoney = new AnimatedNumber(m_Dialog.GetWidget("MoneyFoundMoney").GetComponent<GUIBase_Label>());
			MissionEarnedValue = new AnimatedNumber(m_Dialog.GetWidget("MissionEarnedValue").GetComponent<GUIBase_Label>());
			ProfitTotalMoney = new AnimatedNumber(m_Dialog.GetWidget("ProfitTotalMoney").GetComponent<GUIBase_Label>());
		}

		public void Show(GUIBase_Button.TouchDelegate close, CityArenaInfo arenaInfo)
		{
			base.Show(close);
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", close, null);
			GUIBase_Label component = m_Dialog.GetWidget("Rank").GetComponent<GUIBase_Label>();
			string text = TextDatabase.instance[3000300];
			text = text.Replace("%d", arenaInfo.arenaRank.ToString());
			component.SetNewText(text);
			GUIBase_Label component2 = m_Dialog.GetWidget("Score").GetComponent<GUIBase_Label>();
			component2.SetNewText(arenaInfo.arenaScore.ToString());
			GUIBase_Label component3 = m_Dialog.GetWidget("Waves").GetComponent<GUIBase_Label>();
			component3.SetNewText(arenaInfo.arenaWaves.ToString());
			GUIBase_Sprite component4 = m_Dialog.GetWidget("BestGadgetSprite").GetComponent<GUIBase_Sprite>();
			E_ItemID favouriteItem = arenaInfo.missionResult.GetFavouriteItem();
			if (favouriteItem == E_ItemID.None)
			{
				component4.Widget.Show(false, true);
			}
			else
			{
				component4.Widget.CopyMaterialSettings(ItemSettingsManager.Instance.Get(favouriteItem).ShopWidget);
			}
			component4 = m_Dialog.GetWidget("BestWeaponSprite").GetComponent<GUIBase_Sprite>();
			E_WeaponID favouriteWeapon = arenaInfo.missionResult.GetFavouriteWeapon();
			if (favouriteWeapon == E_WeaponID.None)
			{
				component4.Widget.Show(false, true);
			}
			else
			{
				component4.Widget.CopyMaterialSettings(WeaponSettingsManager.Instance.Get(favouriteWeapon).ShopWidget);
			}
			MFGuiManager.Instance.ShowLayout(m_Dialog, true);
			m_Dialog.GetWidget("MissionEarnedXP").Show(false, false);
			m_Dialog.StopAllCoroutines();
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			m_Dialog.StartCoroutine(Show(arenaInfo));
		}

		public override void Hide()
		{
			base.Hide();
			GuiBaseUtils.RegisterButtonDelegate(m_Dialog, "ButtonClose", null, null);
			m_Dialog.StopAllCoroutines();
			m_Dialog.StartCoroutine(CityGUIResources.Reset(m_Dialog, m_OrigPos));
			CityManager.Instance.StopSound(CityManager.Sounds.GUI_NumberLoop);
			m_Dialog.StartCoroutine(CityGUIResources.HideDialog(m_Dialog, m_OrigPos));
		}

		private IEnumerator Show(CityArenaInfo arenaInfo)
		{
			yield return m_Dialog.StartCoroutine(CityGUIResources.ShowDialog(m_Dialog, m_OrigPos));
			float delay = 0.01f;
			GUIBase_Label label4 = m_Dialog.GetWidget("MissionTimeValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.GetMissionTime());
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("KilledZombiesValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.KilledZombies.ToString());
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("FireAccuracyValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.FireAccuracy + "%");
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("InjuredValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(Mathf.RoundToInt(arenaInfo.missionResult.HealthLost * 100f) + " %");
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("WastedMoneyValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.wastedMoney.ToString());
			yield return new WaitForSeconds(delay);
			MissionSpentMoney.Label.Widget.Show(true, true);
			m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, -arenaInfo.spentMoney, MissionSpentMoney, 1f, "$"));
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("HeadshotsValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.HeadShots.ToString());
			yield return new WaitForSeconds(delay);
			HeadShotsMoney.Label.Widget.Show(true, true);
			m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.headShotMoney, HeadShotsMoney, 1f, "$"));
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("RemovedLimbsValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.RemovedLimbs.ToString());
			yield return new WaitForSeconds(delay);
			RemovedLimbsMoney.Label.Widget.Show(true, true);
			m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.limbMoney, RemovedLimbsMoney, 1f, "$"));
			yield return new WaitForSeconds(delay);
			label4 = m_Dialog.GetWidget("DisintegrationsValue").GetComponent<GUIBase_Label>();
			label4.GetComponent<GUIBase_Widget>().Show(true, true);
			label4.SetNewText(arenaInfo.missionResult.Disintegrations.ToString());
			yield return new WaitForSeconds(delay);
			DesintegrationsMoney.Label.Widget.Show(true, true);
			m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.desintegrationMoney, DesintegrationsMoney, 1f, "$"));
			yield return new WaitForSeconds(delay);
			yield return new WaitForSeconds(delay);
			MoneyFoundMoney.Label.Widget.GetComponent<GUIBase_Widget>().Show(true, true);
			m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.foundMoney, MoneyFoundMoney, 1f, "$"));
			yield return new WaitForSeconds(0.5f);
			MissionEarnedValue.Label.Widget.Show(true, true);
			yield return m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.totalExperience, MissionEarnedValue, 1f, "xp"));
			ProfitTotalMoney.Label.Widget.Show(true, true);
			yield return m_Dialog.StartCoroutine(CityGUIResources.AnimateNumber(0, arenaInfo.totalMoney, ProfitTotalMoney, 1f, "$"));
		}
	}

	public delegate void ArenaActivated(int arenaIndex);

	private ArenaStart m_ArenaStart = new ArenaStart();

	private ArenaResultDlg m_ArenaResult = new ArenaResultDlg();

	private GUIBase_Pivot m_Pivot;

	public CityGUIArena()
	{
		Init();
	}

	public void Destroy()
	{
		DestroyInternal();
	}

	public void ShowArenaStart(ArenaActivated arena, GUIBase_Button.TouchDelegate close, CityArenaInfo arenaInfo)
	{
		m_ArenaStart.Show(arena, close, arenaInfo);
	}

	public void ArenaStartDisableControls()
	{
		m_ArenaStart.DisableControls(true);
	}

	public void HideArenaStart()
	{
		m_ArenaStart.Hide();
	}

	public void ShowArenaResult(GUIBase_Button.TouchDelegate close, CityArenaInfo arenaInfo)
	{
		m_ArenaResult.Show(close, arenaInfo);
	}

	public void HideArenaResult()
	{
		m_ArenaResult.Hide();
	}

	private void Init()
	{
		m_Pivot = MFGuiManager.Instance.GetPivot("CityScreens");
		m_ArenaStart.Init(m_Pivot);
		m_ArenaResult.Init(m_Pivot);
	}

	private void DestroyInternal()
	{
	}

	private static T GetChildByName<T>(GUIBase_Widget widget, string name) where T : Component
	{
		Transform transform = widget.transform.Find(name);
		return (!(transform != null)) ? ((T)null) : transform.GetComponent<T>();
	}
}
