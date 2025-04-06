using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsXperia
{
	private class ViewJoypad
	{
		public int FingerID = -1;

		public Vector2 Center;

		public Vector2 LastTouchPosition;

		public bool FirstDelta;

		private List<Vector2> DeltaPositions = new List<Vector2>();

		private int FilterPositions = 5;

		public bool On
		{
			get
			{
				return FingerID != -1;
			}
		}

		public void SetCenter(Vector2 center)
		{
			Center = center;
			LastTouchPosition = center;
			FirstDelta = true;
			DeltaPositions.Clear();
		}

		public void AddDelta(Vector2 delta)
		{
			if (DeltaPositions.Count == FilterPositions)
			{
				DeltaPositions.RemoveAt(0);
			}
			DeltaPositions.Add(delta);
		}

		public Vector2 GetDelta()
		{
			if (DeltaPositions.Count == 0)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			foreach (Vector2 deltaPosition in DeltaPositions)
			{
				zero += deltaPosition;
			}
			return zero / DeltaPositions.Count;
		}
	}

	public bool Enabled = true;

	private PlayerControlStates States;

	private ViewJoypad ViewJoystick = new ViewJoypad();

	private float ViewSensitivityX
	{
		get
		{
			return 2f * GuiOptions.sensitivity;
		}
	}

	private float ViewSensitivityY
	{
		get
		{
			return 1.5f * GuiOptions.sensitivity;
		}
	}

	public PlayerControlsXperia(PlayerControlStates inStates)
	{
		States = inStates;
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		angle %= 360f;
		if (angle >= -360f && angle <= 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	private void UpdateMove()
	{
		if (Game.Instance.IsXperiaPlay)
		{
			Vector2 joydir = Vector2.zero;
			GetTouchStickJoystick(ref joydir, true);
			float num = Mathf.Clamp(joydir.magnitude, 0f, 1f);
			if (num > 0.001f)
			{
				States.Move.Direction.x = joydir.x;
				States.Move.Direction.z = joydir.y;
				States.Move.Direction.Normalize();
				States._Temp.eulerAngles = new Vector3(0f, Player.Instance.Owner.Transform.rotation.eulerAngles.y, 0f);
				States.Move.Direction = States._Temp.TransformDirection(States.Move.Direction);
				States.Move.Force = num;
			}
		}
	}

	private void UpdateView()
	{
		if (Game.Instance.IsXperiaPlay)
		{
			UpdateViewBySlidePad();
		}
	}

	private void UpdateViewBySlidePad()
	{
#if UNITY_ANDROID
		if (!AndroidInput.secondaryTouchEnabled)
		{
			return;
		}
		int touchCountSecondary = AndroidInput.touchCountSecondary;
		for (int i = 0; i < touchCountSecondary; i++)
		{
			Touch secondaryTouch = AndroidInput.GetSecondaryTouch(i);
			if (secondaryTouch.phase == TouchPhase.Began)
			{
				ViewTouchBegin(secondaryTouch);
			}
			else if (secondaryTouch.phase == TouchPhase.Moved || secondaryTouch.phase == TouchPhase.Stationary)
			{
				ViewTouchUpdate(secondaryTouch);
			}
			else if (secondaryTouch.phase == TouchPhase.Ended || secondaryTouch.phase == TouchPhase.Canceled)
			{
				ViewTouchEnd(secondaryTouch);
			}
		}
#endif
	}

	private bool InsideRightPad(Touch touch)
	{
#if UNITY_ANDROID
		Vector2 vector = new Vector2(786f, 180f);
		Vector2 vector2 = new Vector2(touch.position.x - vector.x, touch.position.y - vector.y);
		if (touch.position.x > (float)(AndroidInput.secondaryTouchWidth / 2) && vector2.sqrMagnitude < 40000f)
		{
			return true;
		}
#endif
        return false;
	}

	private void ViewTouchBegin(Touch touch)
	{
		if (States.View.Enabled && !ViewJoystick.On && InsideRightPad(touch))
		{
			ViewJoystick.FingerID = touch.fingerId;
			ViewJoystick.SetCenter(touch.position);
			States.View.ZeroInput();
		}
	}

	private void ViewTouchUpdate(Touch touch)
	{
		if (States.View.Enabled && ViewJoystick.FingerID == touch.fingerId)
		{
			Vector2 delta = touch.position - ViewJoystick.LastTouchPosition;
			if (ViewJoystick.FirstDelta)
			{
				delta *= 0.25f;
			}
			ViewJoystick.AddDelta(delta);
			Vector2 delta2 = ViewJoystick.GetDelta();
			ViewJoystick.LastTouchPosition = touch.position;
			if (delta.magnitude > 0.01f)
			{
				ViewJoystick.FirstDelta = false;
				float num = 0.225f;
				float num2 = 0.15f;
				float num3 = delta2.x * num;
				float num4 = delta2.y * num2;
				float sensitivity = GuiOptions.sensitivity;
				num3 *= sensitivity;
				num4 *= sensitivity;
				States.View.SetNewRotation(num3, num4);
			}
		}
	}

	private void ViewTouchEnd(Touch touch)
	{
		if (ViewJoystick.FingerID == touch.fingerId)
		{
			ViewJoystick.FingerID = -1;
		}
	}

	private void UpdateActions()
	{
		if (!Game.Instance.IsXperiaPlay)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.RightShift))
		{
			if (Player.Instance.InUseMode)
			{
				States.UseDelegate();
			}
			else
			{
				States.FireDownDelegate();
			}
		}
		else if (Input.GetKeyUp(KeyCode.RightShift) && !Player.Instance.InUseMode)
		{
			States.FireUpDelegate();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			States.ReloadDelegate();
		}
		if (Input.GetKeyDown(KeyCode.Return) && Game.Instance.GameState != E_GameState.IngameMenu && !GuiHUD.Instance.IsHidden && MFGuiManager.Instance.FadeState == MFGuiManager.E_Fading.None && (bool)GuiIngameMenu.Instance)
		{
			GuiHUD.Instance.SwitchToIngameMenu();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			ChangeWeaponPrev();
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			ChangeWeaponNext();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			States.IronSightDelegate();
		}
		if (Input.GetKeyDown("joystick button 1"))
		{
			UseGadget(0);
		}
		else if (Input.GetKeyDown("joystick button 0"))
		{
			UseGadget(1);
		}
		else if (Input.GetKeyDown("joystick button 3"))
		{
			UseGadget(2);
		}
		else if (Input.GetKeyDown("joystick button 2"))
		{
			UseGadget(3);
		}
	}

	private void UpdateActionsAlways()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			GuiHUD.Instance.SimulateIngameAction();
		}
	}

	private void UseGadget(int index)
	{
		Debug.Log("UseGadget: " + index);
		E_ItemID e_ItemID = GuiHUD.Instance.ItemID(index);
		if (e_ItemID != 0)
		{
			States.UseGadgetDelegate(e_ItemID);
		}
	}

	public void Update()
	{
		if (Enabled && !(GuiHUD.Instance == null) && !GuiHUD.Instance.IsHidden)
		{
			if (States.Move.Enabled)
			{
				UpdateMove();
			}
			if (States.View.Enabled)
			{
				UpdateView();
			}
			if (States.ActionsEnabled)
			{
				UpdateActions();
			}
			UpdateActionsAlways();
		}
	}

	private void ChangeWeapon(E_WeaponID type)
	{
		if (CanChangeToWeapon(type) && (bool)GuiHUD.Instance && (bool)Player.Instance && (bool)Player.Instance.Owner && (bool)Player.Instance.Owner.WeaponComponent && Player.Instance.CanChangeWeapon())
		{
			States.ChangeWeaponDelegate(type);
			if ((bool)Player.Instance.Owner.WeaponComponent.GetWeapon(type))
			{
				GuiHUD.Instance.OnCurrentWeaponChanged(type);
			}
		}
	}

	private bool CanChangeToWeapon(E_WeaponID type)
	{
		ComponentWeapons weaponComponent = Player.Instance.Owner.WeaponComponent;
		WeaponBase weapon = weaponComponent.GetWeapon(type);
		if (weapon == null || (weapon.ClipAmmo <= 0 && weapon.WeaponAmmo <= 0) || weaponComponent.CurrentWeapon == type)
		{
			return false;
		}
		return true;
	}

	private void ChangeWeaponPrev()
	{
		if (!Player.Instance.CanChangeWeapon() || !Player.Instance.Owner.WeaponComponent || !Player.Instance.Owner.WeaponComponent.GetCurrentWeapon())
		{
			return;
		}
		E_WeaponID weaponID = Player.Instance.Owner.WeaponComponent.GetCurrentWeapon().WeaponID;
		int num = (int)weaponID;
		do
		{
			num = ((num > 0) ? (num - 1) : 25);
			if (CanChangeToWeapon((E_WeaponID)num))
			{
				ChangeWeapon((E_WeaponID)num);
				break;
			}
		}
		while (num != (int)weaponID);
	}

	private void ChangeWeaponNext()
	{
		if (!Player.Instance.CanChangeWeapon() || !Player.Instance.Owner.WeaponComponent || !Player.Instance.Owner.WeaponComponent.GetCurrentWeapon())
		{
			return;
		}
		E_WeaponID weaponID = Player.Instance.Owner.WeaponComponent.GetCurrentWeapon().WeaponID;
		int num = (int)weaponID;
		do
		{
			num = (num + 1) % 26;
			if (CanChangeToWeapon((E_WeaponID)num))
			{
				ChangeWeapon((E_WeaponID)num);
				break;
			}
		}
		while (num != (int)weaponID);
	}

	public void GetTouchStickJoystick(ref Vector2 joydir, bool left)
	{
#if UNITY_ANDROID
		if (!AndroidInput.secondaryTouchEnabled)
		{
			return;
		}
		Vector2 vector = ((!left) ? new Vector2(786f, 180f) : new Vector2(180f, 180f));
		int touchCountSecondary = AndroidInput.touchCountSecondary;
		for (int i = 0; i < touchCountSecondary; i++)
		{
			Touch secondaryTouch = AndroidInput.GetSecondaryTouch(i);
			Vector2 joydir2 = new Vector2(secondaryTouch.position.x - vector.x, secondaryTouch.position.y - vector.y);
			if (((left && secondaryTouch.position.x < (float)(AndroidInput.secondaryTouchWidth / 2)) || (!left && secondaryTouch.position.x > (float)(AndroidInput.secondaryTouchWidth / 2))) && joydir2.sqrMagnitude < 40000f)
			{
				if (left)
				{
					ComputeJoyVector(ref joydir2, 30f, 100f, 1f);
				}
				else
				{
					ComputeJoyVector(ref joydir2, 10f, 100f, 1.5f);
				}
				joydir = joydir2;
				if (left)
				{
					joydir.y = 0f - joydir.y;
				}
			}
		}
#endif
	}

	private static void ComputeJoyVector(ref Vector2 joydir, float deadzone, float padRadius, float clamp)
	{
		if (joydir.x > 0f - deadzone && joydir.x < deadzone)
		{
			joydir.x = 0f;
		}
		if (joydir.y > 0f - deadzone && joydir.y < deadzone)
		{
			joydir.y = 0f;
		}
		joydir /= padRadius;
		joydir.x = Mathf.Clamp(joydir.x, 0f - clamp, clamp);
		joydir.y = Mathf.Clamp(joydir.y, 0f - clamp, clamp);
		joydir.x = EaseJoyValue(joydir.x);
		joydir.y = EaseJoyValue(joydir.y);
		joydir.x = DropSmallJoyValue(joydir.x);
		joydir.y = DropSmallJoyValue(joydir.y);
	}

	private static float EaseJoyValue(float val)
	{
		if (val >= 0f && val <= 1f)
		{
			return Mathfx.Hermite(0f, 1f, val);
		}
		if (val < 0f && val >= -1f)
		{
			return Mathfx.Hermite(0f, -1f, 0f - val);
		}
		return val;
	}

	private static float DropSmallJoyValue(float val)
	{
		if ((val > 0f && val < 0.01f) || (val < 0f && val > -0.01f))
		{
			return 0f;
		}
		return val;
	}
}
