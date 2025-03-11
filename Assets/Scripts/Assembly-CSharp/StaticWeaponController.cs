using UnityEngine;

public class StaticWeaponController : MonoBehaviour
{
	private TargetingSystem m_AimSystem;

	private void Awake()
	{
		m_AimSystem = GetComponent<TargetingSystem>();
	}

	private void Update()
	{
		if (Player.Instance != null && Player.Instance.StatWpnController == this)
		{
			PlayerControlStates controls = Player.Instance.Controls;
			float num = 1f / 30f;
			float num2 = 0.025f;
			float num3 = Mathf.Clamp(controls.View.YawAdd * num, -1f, 1f);
			float num4 = Mathf.Clamp((0f - controls.View.PitchAdd) * num2, -1f, 1f);
			float num5 = num3 * num3 + num4 * num4;
			if (num3 * num3 / num5 < 0.1f)
			{
				num3 = 0f;
			}
			if (num4 * num4 / num5 < 0.1f)
			{
				num4 = 0f;
			}
			m_AimSystem.SetTargetDirByAngleDiffs(num3, num4);
		}
	}

	[NESAction]
	public void Activate()
	{
		if (Player.Instance != null && Player.Instance.StatWpnController != this)
		{
			Player.Instance.StatWpnController = this;
		}
	}

	[NESAction]
	public void Deactivate()
	{
		if (Player.Instance != null && Player.Instance.StatWpnController == this)
		{
			Player.Instance.StatWpnController = null;
		}
	}
}
