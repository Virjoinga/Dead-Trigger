using UnityEngine;

public class HitInfo
{
	public RaycastHit data;

	public DummyCollider dummyCollider;

	public DummyColliderCollection dummyColliderCollection;

	public HitZone hitZone;

	public void CopyTo(HitInfo Dst)
	{
		Dst.data = data;
		Dst.dummyCollider = dummyCollider;
		Dst.dummyColliderCollection = dummyColliderCollection;
		Dst.hitZone = hitZone;
	}
}
