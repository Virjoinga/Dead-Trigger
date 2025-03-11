using UnityEngine;

[AddComponentMenu("Weapons/AmmoBox")]
public class AmmoBox : MonoBehaviour
{
	public int Ammo;

	public E_WeaponID ForWeapon;

	private bool Scale;

	private float CurrentTime;

	public GameObject GameObject { get; private set; }

	public Transform Transform { get; private set; }

	public bool Dropped { get; private set; }

	public E_WeaponID cacheKey { get; set; }

	private void Awake()
	{
		Transform = base.transform;
		GameObject = base.gameObject;
		Dropped = false;
	}

	public void Reset()
	{
		GameObject._SetActiveRecursively(false);
		Dropped = false;
		base.enabled = false;
	}

	public void Disable()
	{
		GameObject._SetActiveRecursively(false);
		base.enabled = false;
	}

	public void Enable()
	{
		GameObject._SetActiveRecursively(true);
		base.enabled = true;
		Transform.localScale = Vector3.zero;
		Transform.rotation = Quaternion.identity;
		Scale = true;
		CurrentTime = 0f;
	}

	public void Enable(Vector3 pos)
	{
		Transform.position = pos;
		Dropped = true;
		Enable();
	}

	private void Update()
	{
		if (Scale)
		{
			CurrentTime += Time.deltaTime;
			if (CurrentTime >= 1f)
			{
				CurrentTime = 1f;
				Scale = false;
			}
			float num = Mathfx.Hermite(0f, 1f, CurrentTime);
			Transform.localScale = new Vector3(num, num, num);
		}
	}
}
