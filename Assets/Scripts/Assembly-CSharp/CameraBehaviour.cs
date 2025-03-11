using UnityEngine;

public abstract class CameraBehaviour : MonoBehaviour
{
	public abstract Transform GetCameraWorldTransform();

	public abstract Transform GetCameraFPVTransform();

	public abstract void Activate(SpawnPoint spawn);
}
