using System.Collections.Generic;
using UnityEngine;

public class PlayerControlStates
{
	public class MoveState
	{
		public bool Enabled = true;

		public Vector3 Direction = default(Vector3);

		public float Force;

		public void ZeroInput()
		{
			Direction = Vector3.zero;
			Force = 0f;
		}
	}

	public class ViewState
	{
		private const int MaxSmooth = 5;

		public bool Enabled = true;

		public float YawAdd;

		public float PitchAdd;

		private List<Vector2> Values = new List<Vector2>(5);

		public void ZeroInput(bool clearSmooth = false)
		{
			YawAdd = 0f;
			PitchAdd = 0f;
			if (clearSmooth)
			{
				Values.Clear();
			}
		}

		public void SetNewRotation(float Yaw, float Pitch, bool smooth = false)
		{
			if (smooth)
			{
				if (Values.Count == 5)
				{
					Values.RemoveAt(0);
				}
				Values.Add(new Vector2(Pitch, Yaw));
				Vector2 vector = default(Vector2);
				int num = 0;
				for (int i = 0; i < Values.Count; i++)
				{
					vector += Values[i] * i;
					num += i + 1;
				}
				vector /= (float)num;
				YawAdd = vector.y;
				PitchAdd = ((!GuiOptions.invertYAxis) ? vector.x : (0f - vector.x));
			}
			else
			{
				YawAdd = Yaw;
				PitchAdd = ((!GuiOptions.invertYAxis) ? Pitch : (0f - Pitch));
			}
		}
	}

	public delegate void ButtonDelegate();

	public delegate void ObjectUseDelegate(InteractionObject obj);

	public delegate void TouchUpdate_Delegate(Vector2 pos, Vector2 delta);

	public delegate void WeaponDelegate(E_WeaponID weapon);

	public delegate void GadgetDelegate(E_ItemID item);

	public MoveState Move = new MoveState();

	public ViewState View = new ViewState();

	public bool ActionsEnabled = true;

	public bool Fire;

	public bool Use;

	public ButtonDelegate FireDownDelegate;

	public ButtonDelegate FireUpDelegate;

	public ButtonDelegate UseDelegate;

	public ButtonDelegate ReloadDelegate;

	public ButtonDelegate IronSightDelegate;

	public ObjectUseDelegate UseObjectDelegate;

	public TouchUpdate_Delegate TouchUpdateDelegate;

	public WeaponDelegate ChangeWeaponDelegate;

	public GadgetDelegate UseGadgetDelegate;

	public Transform _Temp;

	public Transform MainCameraTransfom;

	public PlayerControlsTouch TouchControls;

	public PlayerControlsPC PCControls;

	private PlayerControlsGamepad GamepadControls;

	private PlayerControlsXperia XperiaControls;

	public void Start()
	{
		GameObject gameObject = new GameObject();
		_Temp = gameObject.transform;
		MainCameraTransfom = Camera.main.transform;
		TouchControls = new PlayerControlsTouch(this);
		TouchControls.Start();
		if (Game.Instance.IsXperiaPlay)
		{
			XperiaControls = new PlayerControlsXperia(this);
		}
		else
		{
			GamepadControls = new PlayerControlsGamepad(this);
		}
	}

	public void SwitchToUseMode()
	{
		Move.Enabled = true;
	}

	public void SwitchToCombatMode()
	{
		Move.Enabled = true;
	}

	public void DisableInput()
	{
		Reset();
		Move.Enabled = false;
		ActionsEnabled = false;
	}

	public void EnableInput()
	{
		SwitchToCombatMode();
		Move.Enabled = true;
		ActionsEnabled = true;
	}

	public void DisableView()
	{
		View.Enabled = false;
	}

	public void EnableView()
	{
		View.Enabled = true;
	}

	public void Reset()
	{
		Fire = false;
		Move.Direction = Vector3.zero;
		Move.Force = 0f;
		if (TouchControls != null)
		{
			TouchControls.Reset();
		}
	}

	public void Update()
	{
		Move.ZeroInput();
		View.ZeroInput();
		Use = false;
		if (Game.Instance.GameState == E_GameState.Game)
		{
			if (PCControls != null)
			{
				PCControls.Update();
			}
			if (XperiaControls != null)
			{
				XperiaControls.Update();
			}
			bool flag = Input.GetKeyDown("escape") && TouchControls != null && Time.timeSinceLevelLoad > TouchControls.LastTouchControlTime + 0.25f;
			bool flag2 = MFGuiManager.Instance.FadeState == MFGuiManager.E_Fading.None;
			if (flag && Game.Instance.GameState == E_GameState.Game && !GuiHUD.Instance.IsHidden && flag2)
			{
				GuiHUD.Instance.SwitchToIngameMenu();
			}
			if (GamepadControls != null)
			{
				GamepadControls.Update();
			}
			if (TouchControls != null)
			{
				TouchControls.Update();
			}
		}
	}
}
