using UnityEngine;

public class CameraStateFPV : CameraState
{
	private Transform Transform;

	private Transform DefaultLookat;

	private GameObject Offset;

	private Transform OffsetTransform;

	public CameraStateFPV(AgentHuman owner)
		: base(owner)
	{
		Transform = new GameObject("FpvCameraDummy").transform;
	}

	public override Transform GetCameraWorldTransform()
	{
		return Owner.TransformEye;
	}

	public override Transform GetCameraFPVTransform()
	{
		Transform.position = Owner.TransformEye.position;
		Transform.rotation = Quaternion.Lerp(Transform.rotation, Owner.TransformEye.rotation, 28f * Time.deltaTime);
		return Transform;
	}

	public override void Activate(Transform t)
	{
		base.Activate(t);
		Transform.position = Owner.TransformEye.position;
		Transform.rotation = Owner.TransformEye.rotation;
	}

	public override void Deactivate()
	{
		base.Deactivate();
	}
}
