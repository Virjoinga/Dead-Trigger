using UnityEngine;

[NESEvent(new string[] { "OnActivate" })]
public class GameFlowNode : MonoBehaviour
{
	private NESController m_NESController;

	[NESAction]
	public void Activate()
	{
		if ((bool)m_NESController)
		{
			m_NESController.SendGameEvent(this, "OnActivate");
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
		Gizmos.DrawSphere(base.transform.position, 0.35f);
	}
}
