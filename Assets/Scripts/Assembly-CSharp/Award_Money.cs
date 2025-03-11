using UnityEngine;

[AddComponentMenu("Entities/Award - Money")]
public class Award_Money : Award
{
	public int m_MoneyRangeMin = 8;

	public int m_MoneyRangeMax = 12;

	protected override void OnPickedUp()
	{
		if (Player.Instance != null)
		{
			int units = Random.Range(m_MoneyRangeMin, m_MoneyRangeMax + 1);
			Player.Instance.MoneyPickedUp(units, 3001050);
		}
	}
}
