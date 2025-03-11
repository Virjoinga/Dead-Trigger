using UnityEngine;

public class RepairDestructibleObject : MonoBehaviour
{
	private const float REPAIR_VALUE_PER_CALL = 0.0175f;

	private bool PlayerInside;

	private bool EnemyInside;

	private DestructibleObject m_Destructible;

	private void Awake()
	{
		m_Destructible = GameObjectUtils.GetFirstComponentUpward<DestructibleObject>(base.gameObject);
	}

	private void Start()
	{
		InvokeRepeating("Repair", 1f, 0.5f);
	}

	private void OnTriggerStay(Collider Other)
	{
		GameObject gameObject = Other.gameObject;
		ComponentPlayer component = gameObject.GetComponent<ComponentPlayer>();
		ComponentEnemy firstComponentUpward = GameObjectUtils.GetFirstComponentUpward<ComponentEnemy>(gameObject);
		if (component != null)
		{
			PlayerInside = true;
		}
		else if (firstComponentUpward != null && firstComponentUpward.Owner.IsAlive)
		{
			EnemyInside = true;
		}
	}

	private void Repair()
	{
		if (PlayerInside && !EnemyInside && m_Destructible.HealthCoef < 1f)
		{
			m_Destructible.HealthCoef = Mathf.Clamp(m_Destructible.HealthCoef + 0.0175f, 0f, 1f);
			GuiHUD.Instance.ShowRepairIndicator(m_Destructible.gameObject, true);
		}
		else
		{
			GuiHUD.Instance.ShowRepairIndicator(m_Destructible.gameObject, false);
		}
		PlayerInside = false;
		EnemyInside = false;
	}

	private void Update()
	{
	}
}
