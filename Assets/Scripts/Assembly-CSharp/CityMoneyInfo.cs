using UnityEngine;

public class CityMoneyInfo : CitySiteInfo
{
	private float m_LastRewardTime;

	private Funds m_Reward;

	private float m_RewardIntervalMinutes;

	public CityMoneyInfo(Funds reward, float rewardIntervalMinutes)
	{
		m_Reward = reward;
		m_RewardIntervalMinutes = rewardIntervalMinutes;
	}

	public Funds GatherRevenue()
	{
		m_LastRewardTime = (float)TimeUtils.GetCurrentTimeInSeconds();
		Funds funds = new Funds(m_Reward);
		funds.Value = Mathf.RoundToInt(funds.Value);
		return funds;
	}

	public float SecondsToNextReward()
	{
		return SecondsToNextAction(m_LastRewardTime, m_RewardIntervalMinutes * 60f);
	}

	private float SecondsToNextAction(double lastAction, float actionCooldownInSeconds)
	{
		double currentTimeInSeconds = TimeUtils.GetCurrentTimeInSeconds();
		double num = currentTimeInSeconds - lastAction;
		if (num < 0.0)
		{
			num = 0.0;
		}
		else if (num > 356400.0)
		{
			num = 356400.0;
		}
		float num2 = Mathf.Round((float)num);
		num2 -= actionCooldownInSeconds;
		if (num2 >= 0f)
		{
			num2 = 0f;
		}
		else
		{
			num2 = 0f - num2;
			if (num2 > actionCooldownInSeconds)
			{
				num2 = actionCooldownInSeconds;
			}
		}
		return num2;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
