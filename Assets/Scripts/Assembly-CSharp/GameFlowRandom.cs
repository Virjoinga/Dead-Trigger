using UnityEngine;

[NESEvent(new string[] { "Var. A", "Var. B" })]
public class GameFlowRandom : MonoBehaviour
{
	private NESController m_NESController;

	[NESAction]
	public void Activate()
	{
		if ((bool)m_NESController)
		{
			if (Random.value >= 0.5f)
			{
				m_NESController.SendGameEvent(this, "Var. A");
				Debug.Log("Var. A");
			}
			else
			{
				m_NESController.SendGameEvent(this, "Var. B");
				Debug.Log("Var. B");
			}
		}
	}

	private void Awake()
	{
		m_NESController = base.gameObject.GetFirstComponentUpward<NESController>();
		if (!(m_NESController == null))
		{
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, 0.15f);
	}
}
