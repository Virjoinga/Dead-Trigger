using UnityEngine;

public class ComponentBody : MonoBehaviour
{
	private AgentActionRotate ActionRotate;

	private AgentHuman Owner;

	public Transform RightHand;

	public Transform LeftHand;

	private float RotationDiff;

	private bool SmoothRotation = true;

	private void Start()
	{
		Owner = GetComponent<AgentHuman>();
		RotationDiff = 0f;
	}

	private void Update()
	{
		if (!Owner.IsAlive || Time.timeScale == 0f)
		{
			return;
		}
		float rotationDiffLimit = GetRotationDiffLimit();
		float num = Owner.Transform.rotation.eulerAngles.y - Owner.BlackBoard.Desires.Rotation.eulerAngles.y;
		if (num < 0.001f && num > -0.001f)
		{
			num = 0f;
		}
		if (num != 0f && ActionRotate == null)
		{
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			RotationDiff += Mathf.Abs(num);
			if (RotationDiff >= rotationDiffLimit)
			{
				if (!Owner.IsPlayer && !Owner.BlackBoard.ActionPointOn)
				{
					ActionRotate = AgentActionFactory.Create(AgentActionFactory.E_Type.Rotate) as AgentActionRotate;
					if (Owner.NavMeshAgent.updateRotation)
					{
						ActionRotate.Rotation = ((num < 0f) ? E_RotationType.Left : E_RotationType.Right);
					}
					else
					{
						ActionRotate.Rotation = ((num > 0f) ? E_RotationType.Left : E_RotationType.Right);
					}
					ActionRotate.Angle = RotationDiff;
					Owner.BlackBoard.ActionAdd(ActionRotate);
				}
				RotationDiff = 0f;
			}
		}
		if (!Owner.IsPlayer && Owner.NavMeshAgent.updateRotation && !Owner.IsActionPointOn)
		{
			Owner.BlackBoard.Desires.Rotation = Owner.Transform.rotation;
		}
		else if (num != 0f && Time.deltaTime > float.Epsilon)
		{
			float rotationSpeed = GetRotationSpeed(Mathf.Abs(num));
			Quaternion quaternion = default(Quaternion);
			quaternion.eulerAngles = new Vector3(0f, Owner.BlackBoard.Desires.Rotation.eulerAngles.y, 0f);
			Owner.Transform.rotation = ((!SmoothRotation) ? quaternion : Quaternion.Lerp(Owner.Transform.rotation, quaternion, rotationSpeed * Time.deltaTime));
		}
		UpdateAiming();
		if (ActionRotate != null && !ActionRotate.IsActive())
		{
			RotationDiff = 0f;
			ActionRotate = null;
		}
	}

	private float GetRotationSpeed(float angle)
	{
		if (Owner.IsActionPointOn)
		{
			return 3f;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.Contest).GetBool())
		{
			return 7f;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Stand)
		{
			if (angle < 60f)
			{
				return 5f;
			}
			if (angle < 120f)
			{
				return 7f;
			}
			return 8f;
		}
		return 10f;
	}

	private float GetRotationDiffLimit()
	{
		if (Owner.IsActionPointOn)
		{
			return 5f;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.Contest).GetBool())
		{
			return 1f;
		}
		if (Owner.WorldState.GetWSProperty(E_PropKey.BodyPose).GetBodyPose() == E_BodyPose.Stand)
		{
			return 20f;
		}
		return 60f;
	}

	private void LateUpdate()
	{
		if (RightHand != null && !SmoothRotation)
		{
			RightHand.rotation = Owner.BlackBoard.Desires.Rotation;
		}
	}

	public void EnableSmoothRotation(bool enable)
	{
		SmoothRotation = enable;
	}

	private void UpdateAiming()
	{
		Owner.BlackBoard.FireDir = Owner.BlackBoard.Desires.FireDirection;
	}
}
