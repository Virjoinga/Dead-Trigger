using UnityEngine;

[AddComponentMenu("Interaction/Destroyable Object")]
public class DestroyObject : MonoBehaviour
{
	public float Health;

	private float DefaultHealth;

	public AudioSource Sound;

	protected bool Active = true;

	private GameObject GameObject;

	public bool IsActive
	{
		get
		{
			return Active;
		}
	}

	private void Start()
	{
		GameObject = base.gameObject;
		DefaultHealth = Health;
	}

	private void OnProjectileHit(Projectile projectile)
	{
		if (Active)
		{
			Health -= projectile.Damage();
			if (Health < 0f)
			{
				Break();
			}
		}
	}

	public void OnReceiveRangeDamage(Agent attacker, float damage, Vector3 impuls, E_WeaponType weaponType)
	{
		if (Active)
		{
			Health -= damage;
			if (Health < 0f)
			{
				Break();
			}
		}
	}

	public virtual void Break()
	{
		Active = false;
		if ((bool)Sound)
		{
			Sound.Play();
		}
		GameObject.GetComponent<Renderer>().enabled = false;
		GameObject.GetComponent<Collider>().enabled = false;
	}

	public virtual void Reset()
	{
		Health = DefaultHealth;
		Active = true;
		GameObject.GetComponent<Renderer>().enabled = true;
		GameObject.GetComponent<Collider>().enabled = true;
	}

	public void Enable()
	{
		GameObject._SetActiveRecursively(true);
	}

	public void Disable()
	{
		GameObject._SetActiveRecursively(false);
	}
}
