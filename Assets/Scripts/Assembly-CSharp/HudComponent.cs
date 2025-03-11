public abstract class HudComponent
{
	public enum EnableLayer
	{
		IngameMenu = 0,
		Contest = 1,
		Controls = 2,
		Tutorial = 3,
		Last = 4
	}

	private bool[] m_Enabled = new bool[4];

	private bool m_Visible;

	protected HudComponent()
	{
		for (int i = 0; i < m_Enabled.Length; i++)
		{
			m_Enabled[i] = true;
		}
	}

	public abstract void Init();

	public abstract void Reset();

	public virtual void OnDestroy()
	{
	}

	public abstract bool VisibleOnStart();

	public virtual void LateUpdate(float deltaTime)
	{
	}

	public virtual void LateUpdate100ms()
	{
	}

	public virtual void LateUpdate200ms()
	{
	}

	public virtual void HandleAction(AgentAction a)
	{
	}

	public virtual void StoreControlsOrigPositions()
	{
	}

	public virtual void UpdateControlsPosition()
	{
	}

	public virtual bool Enable(EnableLayer layer, bool enable)
	{
		bool flag = IsEnabled();
		m_Enabled[(int)layer] = enable;
		bool flag2 = IsEnabled();
		if (flag == flag2)
		{
			return false;
		}
		if (flag2)
		{
			ShowWidgets(m_Visible);
		}
		else
		{
			ShowWidgets(false);
		}
		return true;
	}

	public bool IsEnabled()
	{
		bool flag = true;
		bool[] enabled = m_Enabled;
		foreach (bool flag2 in enabled)
		{
			flag = flag && flag2;
		}
		return flag;
	}

	public void Show(bool on)
	{
		m_Visible = on;
		if (IsEnabled())
		{
			ShowWidgets(on);
		}
	}

	public bool IsVisible()
	{
		return m_Visible;
	}

	protected abstract void ShowWidgets(bool on);
}
