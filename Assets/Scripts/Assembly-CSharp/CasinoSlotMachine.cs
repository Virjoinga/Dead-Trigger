using System.Collections;
using UnityEngine;

public class CasinoSlotMachine : MonoBehaviour
{
	public enum Symbols
	{
		First = 0,
		Small1 = 0,
		Small2 = 1,
		Small3 = 2,
		Small4 = 3,
		Small5 = 4,
		Medium1 = 5,
		Medium2 = 6,
		Big1 = 7,
		Jackpot = 8,
		Last = 8
	}

	private class Slot
	{
		private MeshRenderer m_Mesh;

		public Slot(GameObject symbol)
		{
			m_Mesh = symbol.GetComponent<MeshRenderer>();
		}

		public void SetSymbol(Symbols symbol)
		{
			Vector2 mainTextureOffset = default(Vector2);
			switch (symbol)
			{
			case Symbols.First:
				mainTextureOffset.x = -0.666f;
				mainTextureOffset.y = 0.666f;
				break;
			case Symbols.Small2:
				mainTextureOffset.x = -0.666f;
				mainTextureOffset.y = 0.333f;
				break;
			case Symbols.Small3:
				mainTextureOffset.x = -0.666f;
				mainTextureOffset.y = 0f;
				break;
			case Symbols.Small4:
				mainTextureOffset.x = -0.333f;
				mainTextureOffset.y = 0.666f;
				break;
			case Symbols.Small5:
				mainTextureOffset.x = -0.333f;
				mainTextureOffset.y = 0.333f;
				break;
			case Symbols.Medium1:
				mainTextureOffset.x = -0.333f;
				mainTextureOffset.y = 0f;
				break;
			case Symbols.Big1:
				mainTextureOffset.x = 0f;
				mainTextureOffset.y = 0.666f;
				break;
			case Symbols.Medium2:
				mainTextureOffset.x = 0f;
				mainTextureOffset.y = 0.333f;
				break;
			case Symbols.Jackpot:
				mainTextureOffset.x = 0f;
				mainTextureOffset.y = 0f;
				break;
			}
			m_Mesh.material.mainTextureOffset = mainTextureOffset;
		}
	}

	public delegate void ShowReward();

	public AudioClip m_SpinWin;

	public AudioClip m_SpinLost;

	public ShowReward m_RewardCallback;

	private Slot[] m_LeftSlots = new Slot[3];

	private Slot[] m_CenterSlots = new Slot[3];

	private Slot[] m_RightSlots = new Slot[3];

	private Animation m_SlotMachineAnim;

	private Animation m_WinAnim;

	private Animation m_CameraAnim;

	private bool m_CameraAnimationRun;

	private GameObject[] m_BlurSlots = new GameObject[3];

	private float m_BusyTime;

	private float m_RewardTime;

	private float m_WinAnimTime;

	public void Reset()
	{
		StopAllCoroutines();
		m_SlotMachineAnim.Stop();
		m_WinAnim.Stop();
		base.GetComponent<AudioSource>().Stop();
		m_BusyTime = 0f;
		m_RewardTime = 0f;
		m_WinAnimTime = 0f;
		SetRandomMachineState();
		GameObject[] blurSlots = m_BlurSlots;
		foreach (GameObject gameObject in blurSlots)
		{
			gameObject.SetActive(false);
		}
	}

	public bool IsBusy()
	{
		return m_BusyTime > Time.timeSinceLevelLoad || (m_RewardTime > 1E-05f && m_RewardTime > Time.timeSinceLevelLoad);
	}

	public void SpinRandomSymbol(bool win)
	{
		Spin((Symbols)Random.Range(0, 9), win);
	}

	public void Spin(Symbols symbol, bool win)
	{
		m_BusyTime = Time.timeSinceLevelLoad;
		m_RewardTime = 0f;
		if (win)
		{
			m_BusyTime += m_SpinWin.length;
			m_WinAnimTime = Time.timeSinceLevelLoad + m_SlotMachineAnim.clip.length;
			m_RewardTime = Time.timeSinceLevelLoad + m_SlotMachineAnim.clip.length + 0.5f;
		}
		else
		{
			m_WinAnimTime = 0f;
			m_BusyTime += m_SpinLost.length;
		}
		if (!m_CameraAnimationRun)
		{
			m_CameraAnimationRun = true;
			m_CameraAnim.Play();
		}
		m_WinAnim.Stop();
		m_SlotMachineAnim.Rewind();
		m_SlotMachineAnim.Play();
		if (win)
		{
			base.GetComponent<AudioSource>().PlayOneShot(m_SpinWin);
		}
		else
		{
			base.GetComponent<AudioSource>().PlayOneShot(m_SpinLost);
		}
		StartCoroutine(ControlSpin(symbol, win));
	}

	public IEnumerator ControlSpin(Symbols symbol, bool win)
	{
		yield return new WaitForSeconds(0.12f);
		GameObject[] blurSlots = m_BlurSlots;
		foreach (GameObject obj in blurSlots)
		{
			obj.SetActive(true);
		}
		yield return new WaitForSeconds(0.5f);
		SetMachineState(symbol, win);
	}

	private void SetMachineState(Symbols symbol, bool win)
	{
		Symbols symbols;
		Symbols symbols2;
		Symbols symbols3;
		if (win)
		{
			symbols = symbol;
			symbols2 = symbol;
			symbols3 = symbol;
		}
		else
		{
			do
			{
				symbols = (Symbols)Random.Range(0, 9);
				symbols2 = (Symbols)Random.Range(0, 9);
				symbols3 = (Symbols)Random.Range(0, 9);
			}
			while (symbols == symbols2 && symbols2 == symbols3);
		}
		m_LeftSlots[1].SetSymbol(symbols);
		m_CenterSlots[1].SetSymbol(symbols2);
		m_RightSlots[1].SetSymbol(symbols3);
		Symbols symbol2 = ((symbols != 0) ? (symbols - 1) : Symbols.Jackpot);
		Symbols symbol3 = ((symbols != Symbols.Jackpot) ? (symbols + 1) : Symbols.First);
		m_LeftSlots[0].SetSymbol(symbol2);
		m_LeftSlots[2].SetSymbol(symbol3);
		int num = (int)(symbols2 - 4);
		if (num < 0)
		{
			num += 8;
		}
		symbol2 = (Symbols)num;
		int num2 = (int)(symbols2 + 2);
		if (num2 > 8)
		{
			num2 = 0 + num2 - 8;
		}
		symbol3 = (Symbols)num2;
		m_CenterSlots[0].SetSymbol(symbol2);
		m_CenterSlots[2].SetSymbol(symbol3);
		num = (int)(symbols3 + 3);
		if (num > 8)
		{
			num = 0 + num - 8;
		}
		symbol2 = (Symbols)num;
		num2 = (int)(symbols3 - 3);
		if (num2 < 0)
		{
			num2 += 8;
		}
		symbol3 = (Symbols)num2;
		m_RightSlots[0].SetSymbol(symbol2);
		m_RightSlots[2].SetSymbol(symbol3);
	}

	public void SetRandomMachineState()
	{
		SetMachineState((Symbols)Random.Range(0, 9), false);
	}

	private void Awake()
	{
		m_LeftSlots[0] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/A_01").gameObject);
		m_LeftSlots[1] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/A_02").gameObject);
		m_LeftSlots[2] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/A_03").gameObject);
		m_CenterSlots[0] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/B_01").gameObject);
		m_CenterSlots[1] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/B_02").gameObject);
		m_CenterSlots[2] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/B_03").gameObject);
		m_RightSlots[0] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/C_01").gameObject);
		m_RightSlots[1] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/C_02").gameObject);
		m_RightSlots[2] = new Slot(base.gameObject.transform.Find("slot_machine_SYMBOLS/C_03").gameObject);
		SetRandomMachineState();
		m_SlotMachineAnim = base.gameObject.transform.Find("slot_machine_SYMBOLS").gameObject.GetComponent<Animation>();
		m_WinAnim = base.gameObject.transform.Find("slot_machine_blikacka").gameObject.GetComponent<Animation>();
		m_BlurSlots[0] = base.gameObject.transform.Find("slot_machine_SYMBOLS/X_BLUR_A").gameObject;
		m_BlurSlots[1] = base.gameObject.transform.Find("slot_machine_SYMBOLS/X_BLUR_B").gameObject;
		m_BlurSlots[2] = base.gameObject.transform.Find("slot_machine_SYMBOLS/X_BLUR_C").gameObject;
		m_CameraAnim = GameObject.Find("Casino_Camera_Helper").GetComponentInChildren<Animation>();
	}

	private void Update()
	{
		if (m_WinAnimTime > 0f && m_WinAnimTime < Time.timeSinceLevelLoad)
		{
			m_WinAnimTime = 0f;
			m_WinAnim.Play();
		}
		if (m_RewardTime > 1E-05f && m_RewardTime <= Time.timeSinceLevelLoad)
		{
			m_RewardTime = 0f;
			if (m_RewardCallback != null)
			{
				m_RewardCallback();
			}
		}
	}
}
