using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CarryDLObjectTrigger : MonoBehaviour
{
	public delegate void TriggerActivated();

	public TriggerActivated m_TriggerActivated;

	private void Start()
	{
	}

	private void OnTriggerEnter(Collider Other)
	{
		GameObject gameObject = Other.gameObject;
		ComponentPlayer component = gameObject.GetComponent<ComponentPlayer>();
		if (component != null && m_TriggerActivated != null)
		{
			m_TriggerActivated();
		}
	}
}
