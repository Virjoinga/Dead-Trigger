using UnityEngine;

public class MenuController
{
	private class Button
	{
		public bool Pressed;

		public bool FirstQuery;

		public float LastRelaxPos;

		public float LastPressedPos;

		public void Reset()
		{
			Pressed = false;
			FirstQuery = true;
			LastRelaxPos = 0f;
			LastPressedPos = 0f;
		}
	}

	private enum E_Dir
	{
		LEFT = 0,
		RIGHT = 1,
		UP = 2,
		DOWN = 3
	}

	private Button[] Buttons = new Button[4]
	{
		new Button(),
		new Button(),
		new Button(),
		new Button()
	};

	private float DeadZone = 0.1f;

	private float MoveDelta = 0.2f;

	public MenuController()
	{
		for (int i = 0; i < Buttons.Length; i++)
		{
			Buttons[i].Reset();
		}
	}

	public void Update()
	{
		Button button = Buttons[0];
		Button button2 = Buttons[1];
		float axisRaw = Input.GetAxisRaw("HorizontalMove");
		if (!button2.Pressed && axisRaw > button2.LastRelaxPos + MoveDelta)
		{
			button2.Pressed = true;
			button2.LastPressedPos = axisRaw;
		}
		else if (button2.Pressed && axisRaw < button2.LastPressedPos - MoveDelta)
		{
			button2.Reset();
			button2.LastRelaxPos = axisRaw;
		}
		else if (!button.Pressed && axisRaw < button.LastRelaxPos - MoveDelta)
		{
			button.Pressed = true;
			button.LastPressedPos = axisRaw;
		}
		else if (button.Pressed && axisRaw > button.LastPressedPos + MoveDelta)
		{
			button.Reset();
			button.LastRelaxPos = axisRaw;
		}
		if (button2.Pressed)
		{
			if (button2.LastPressedPos < axisRaw)
			{
				button2.LastPressedPos = axisRaw;
			}
		}
		else if (button2.LastRelaxPos > axisRaw)
		{
			if (axisRaw > DeadZone)
			{
				button2.LastRelaxPos = axisRaw;
			}
			else
			{
				button2.LastRelaxPos = DeadZone;
			}
		}
		if (button.Pressed)
		{
			if (button.LastPressedPos > axisRaw)
			{
				button.LastPressedPos = axisRaw;
			}
		}
		else if (button.LastRelaxPos < axisRaw)
		{
			if (axisRaw < 0f - DeadZone)
			{
				button.LastRelaxPos = axisRaw;
			}
			else
			{
				button.LastRelaxPos = 0f - DeadZone;
			}
		}
		Button button3 = Buttons[3];
		Button button4 = Buttons[2];
		float axisRaw2 = Input.GetAxisRaw("VerticalMove");
		if (!button4.Pressed && axisRaw2 > button4.LastRelaxPos + MoveDelta)
		{
			button4.Pressed = true;
			button4.LastPressedPos = axisRaw2;
		}
		else if (button4.Pressed && axisRaw2 < button4.LastPressedPos - MoveDelta)
		{
			button4.Reset();
			button4.LastRelaxPos = axisRaw2;
		}
		else if (!button3.Pressed && axisRaw2 < button3.LastRelaxPos - MoveDelta)
		{
			button3.Pressed = true;
			button3.LastPressedPos = axisRaw2;
		}
		else if (button3.Pressed && axisRaw2 > button3.LastPressedPos + MoveDelta)
		{
			button3.Reset();
			button3.LastRelaxPos = axisRaw2;
		}
		if (button4.Pressed)
		{
			if (button4.LastPressedPos < axisRaw2)
			{
				button4.LastPressedPos = axisRaw2;
			}
		}
		else if (button4.LastRelaxPos > axisRaw2)
		{
			if (axisRaw2 > DeadZone)
			{
				button4.LastRelaxPos = axisRaw2;
			}
			else
			{
				button4.LastRelaxPos = DeadZone;
			}
		}
		if (button3.Pressed)
		{
			if (button3.LastPressedPos > axisRaw2)
			{
				button3.LastPressedPos = axisRaw2;
			}
		}
		else if (button3.LastRelaxPos < axisRaw2)
		{
			if (axisRaw2 < 0f - DeadZone)
			{
				button3.LastRelaxPos = axisRaw2;
			}
			else
			{
				button3.LastRelaxPos = 0f - DeadZone;
			}
		}
	}

	public bool PressedLeft()
	{
		if (Buttons[0].Pressed && Buttons[0].FirstQuery)
		{
			Buttons[0].FirstQuery = false;
			return true;
		}
		return false;
	}

	public bool PressedRight()
	{
		if (Buttons[1].Pressed && Buttons[1].FirstQuery)
		{
			Buttons[1].FirstQuery = false;
			return true;
		}
		return false;
	}

	public bool PressedUp()
	{
		if (Buttons[2].Pressed && Buttons[2].FirstQuery)
		{
			Buttons[2].FirstQuery = false;
			return true;
		}
		return false;
	}

	public bool PressedDown()
	{
		if (Buttons[3].Pressed && Buttons[3].FirstQuery)
		{
			Buttons[3].FirstQuery = false;
			return true;
		}
		return false;
	}

	public bool PressedOk()
	{
		if (PlayerControlsGamepad.ButtonDown(PlayerControlsGamepad.E_Input.Fire) || Input.GetKeyDown("1") || Input.GetKeyDown("8"))
		{
			return true;
		}
		return false;
	}

	public bool PressedBack()
	{
		if (Input.GetKeyDown("escape"))
		{
			return true;
		}
		return false;
	}
}
