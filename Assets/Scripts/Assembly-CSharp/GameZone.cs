using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GameZone : GameZoneBase
{
	public enum E_State
	{
		E_WAITING_FOR_START = 0,
		E_IN_PROGRESS = 1
	}

	private E_State State;

	private float TimeToUpdate;

	private void Awake()
	{
		GameObject = base.gameObject;
		CollectChilds(false);
	}

	private void Start()
	{
		Initialize();
		TimeToUpdate = Time.timeSinceLevelLoad + 0.2f;
	}

	public override void Enable()
	{
		Mission.Instance.CurrentGameZone = this;
		Game.Instance.Save_Save();
		Mission.Instance.Save_Save();
		Mission.Instance.LockPrevGameZone();
		base.Enable();
		State = E_State.E_IN_PROGRESS;
		EnableChilds();
	}

	public override void Disable()
	{
		DisableChilds();
		base.Disable();
	}

	public override void Reset()
	{
		State = E_State.E_WAITING_FOR_START;
		base.Reset();
		StopAllCoroutines();
		ResetChilds();
	}

	public void RegisterControllableObject(IGameZoneControledObject inObject)
	{
	}

	public void UnRegisterControllableObject(IGameZoneControledObject inObject)
	{
	}

	private void OnDrawGizmos()
	{
		BoxCollider boxCollider = GetComponent("BoxCollider") as BoxCollider;
		if (boxCollider != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
			Gizmos.matrix = Matrix4x4.identity;
		}
	}

	private void OnDrawGizmosSelected()
	{
		BoxCollider boxCollider = GetComponent("BoxCollider") as BoxCollider;
		if (boxCollider != null)
		{
			Gizmos.color = Color.white;
			Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
			Gizmos.matrix = Matrix4x4.identity;
		}
	}

	private void Update()
	{
		if (base.Enemies.Count == 0 || TimeToUpdate > Time.timeSinceLevelLoad)
		{
			return;
		}
		for (int i = 0; i < base.Enemies.Count; i++)
		{
			if (!base.Enemies[i].IsAlive)
			{
				base.Enemies.RemoveAt(i);
				break;
			}
		}
		TimeToUpdate = Time.timeSinceLevelLoad + 0.2f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (State == E_State.E_WAITING_FOR_START && !(Player.Instance == null) && !(other != Player.Instance.Owner.CharacterController))
		{
			Enable();
		}
	}

	private void OnTriggerExit(Collider other)
	{
	}
}
