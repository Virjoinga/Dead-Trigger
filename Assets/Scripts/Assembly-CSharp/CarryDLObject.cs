using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarryDLObject : MonoBehaviour
{
	private enum State
	{
		INIT = 0,
		PICKED = 1,
		FINISHED = 2,
		FINISHED_INIT = 3
	}

	public delegate void ObjectOperation(CarryDLObject obj);

	public int Instances = 1;

	public AudioClip PickUpSound;

	public AudioClip PutDownSound;

	private State m_State;

	private ObjectOperation m_PickUp;

	private ObjectOperation m_PutDown;

	private bool m_Activated;

	private CarryDLObjectTrigger m_PickUpTrigger;

	private CarryDLObjectTrigger m_PutDownTrigger;

	private int m_Instances;

	private GameObject m_PickUp_Box;

	private GameObject m_PutDown_Target;

	private GameObject m_PutDown_Box;

	public void Activate(ObjectOperation pickUp, ObjectOperation putDown)
	{
		if (m_State == State.FINISHED)
		{
			m_State = State.FINISHED_INIT;
		}
		else
		{
			m_State = State.INIT;
		}
		m_Activated = true;
		m_PickUp = pickUp;
		m_PutDown = putDown;
		m_PickUpTrigger.gameObject.SetActive(true);
		m_PutDownTrigger.gameObject.SetActive(true);
		ApplyState();
	}

	public void Deactivate()
	{
		m_State = State.INIT;
		m_Activated = false;
		m_PickUp = null;
		m_PutDown = null;
		m_PickUpTrigger.gameObject.SetActive(false);
		m_PutDownTrigger.gameObject.SetActive(false);
		ApplyState();
	}

	public GameObject Source()
	{
		return m_PickUp_Box;
	}

	public GameObject Target()
	{
		return m_PutDown_Target;
	}

	public int UsableInstances()
	{
		return m_Instances;
	}

	private void Awake()
	{
		CarryDLObjectTrigger[] componentsInChildren = base.gameObject.GetComponentsInChildren<CarryDLObjectTrigger>();
		if (componentsInChildren.Length != 2)
		{
			Debug.LogWarning("CarryDLObject name: " + base.name + " -  expected 2 CarryDLObjectTrigger child objects, found " + componentsInChildren.Length);
		}
		if (componentsInChildren[0].name == "Source")
		{
			m_PickUpTrigger = componentsInChildren[0];
			m_PutDownTrigger = componentsInChildren[1];
		}
		else
		{
			m_PickUpTrigger = componentsInChildren[1];
			m_PutDownTrigger = componentsInChildren[0];
		}
		m_PickUpTrigger.m_TriggerActivated = PickUp;
		m_PutDownTrigger.m_TriggerActivated = PutDown;
		m_Instances = Instances;
		foreach (Transform item in m_PickUpTrigger.transform)
		{
			if (item.gameObject.name == "Box")
			{
				m_PickUp_Box = item.gameObject;
			}
		}
		foreach (Transform item2 in m_PutDownTrigger.transform)
		{
			if (item2.gameObject.name == "Target")
			{
				m_PutDown_Target = item2.gameObject;
			}
			else if (item2.gameObject.name == "Box")
			{
				m_PutDown_Box = item2.gameObject;
			}
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void PickUp()
	{
		if (m_State == State.INIT || m_State == State.FINISHED_INIT)
		{
			m_State = State.PICKED;
			base.GetComponent<AudioSource>().clip = PickUpSound;
			base.GetComponent<AudioSource>().Play();
			ApplyState();
			if (m_PickUp != null)
			{
				m_PickUp(this);
			}
		}
	}

	private void PutDown()
	{
		if (m_State == State.PICKED)
		{
			m_State = State.FINISHED;
			m_Instances--;
			base.GetComponent<AudioSource>().clip = PutDownSound;
			base.GetComponent<AudioSource>().Play();
			ApplyState();
			if (m_PutDown != null)
			{
				m_PutDown(this);
			}
		}
	}

	private void ApplyState()
	{
		if (!m_Activated)
		{
			m_PickUp_Box._SetActiveRecursively(false);
			m_PutDown_Box.SetActive(false);
			m_PutDown_Target.SetActive(false);
			return;
		}
		switch (m_State)
		{
		case State.INIT:
			m_PickUp_Box._SetActiveRecursively(true);
			m_PutDown_Box.SetActive(false);
			m_PutDown_Target.SetActive(false);
			break;
		case State.PICKED:
			m_PickUp_Box._SetActiveRecursively(false);
			m_PutDown_Box.SetActive(false);
			m_PutDown_Target.SetActive(true);
			break;
		case State.FINISHED:
			m_PickUp_Box._SetActiveRecursively(false);
			m_PutDown_Box.SetActive(true);
			m_PutDown_Target.SetActive(false);
			break;
		case State.FINISHED_INIT:
			m_PickUp_Box._SetActiveRecursively(true);
			m_PutDown_Box.SetActive(true);
			m_PutDown_Target.SetActive(false);
			break;
		}
	}
}
