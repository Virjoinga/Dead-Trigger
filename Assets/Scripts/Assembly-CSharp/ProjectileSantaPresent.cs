using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Weapons/ProjectileSantaPresent")]
public class ProjectileSantaPresent : Projectile, IHitZoneOwner
{
	private const float Delay = 0.3f;

	private int m_HitSoundNum;

	public Explosion m_Explosion;

	private bool m_Finished;

	private List<Vector3> Trajectory = new List<Vector3>();

	private Vector3 Velocity = new Vector3(0f, 0f, 0f);

	private new float Speed;

	private float Duration;

	private float HitTime;

	private Vector3 StartPos = new Vector3(0f, 0f, 0f);

	private float FlightTime;

	private ParticleSystem CurrentParticle;

	private float RotationValue = 50f;

	public void OnHitZoneProjectileHit(HitZone zone, Projectile projectile)
	{
		Explode(projectile.Agent);
	}

	public void OnHitZoneRangeDamage(HitZone zone, Agent attacker, float damage, Vector3 impulse, E_WeaponID weaponID, E_WeaponType weaponType)
	{
		if (damage > 50f)
		{
			Explode(attacker);
		}
	}

	public void OnHitZoneMeleeDamage(HitZone zone, MeleeDamageData Data)
	{
	}

	public override void Awake()
	{
		base.Awake();
		HitZone component = GetComponent<HitZone>();
		component.SetOwner(this);
	}

	private void OnDestroy()
	{
		m_Explosion = null;
		Object.Destroy(CurrentParticle);
		CurrentParticle = null;
	}

	public override void ProjectileInit(Vector3 pos, Vector3 dir, ProjectileInitSettings inSettings)
	{
		base.ProjectileInit(pos, dir, inSettings);
		m_Finished = false;
		ComputeTrajectoryLight(dir);
		base.Transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
		FlightTime = 0f;
		RotationValue = Random.Range(40, 90);
	}

	public override void ProjectileDeinit()
	{
	}

	public override void ProjectileUpdate(float deltaTime)
	{
		if (IsFinished())
		{
			return;
		}
		if (FlightTime >= 0f && FlightTime <= 0.3f)
		{
			base.Transform.localScale = Vector3.Lerp(base.Transform.localScale, Vector3.one, FlightTime / 0.3f);
		}
		if (FlightTime <= float.Epsilon)
		{
			FlightTime += deltaTime;
			return;
		}
		if ((HitTime > 0f && FlightTime >= HitTime) || FlightTime >= Duration)
		{
			Explode();
			return;
		}
		Vector3 position = base.Transform.position;
		base.Transform.position = Throw.ComputePosition(StartPos, Velocity, FlightTime);
		Vector3 vector = base.Transform.position - position;
		int layerMask = ~(ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.IgnorePlayer | ObjectLayerMask.IgnoreBullets | ObjectLayerMask.EnemyBox);
		RaycastHit hitInfo;
		if (Physics.Raycast(position, vector.normalized, out hitInfo, vector.magnitude, layerMask) && !hitInfo.collider.isTrigger)
		{
			HitTime = FlightTime + hitInfo.distance / Velocity.magnitude;
		}
		base.Transform.Rotate(Time.deltaTime * RotationValue, Time.deltaTime * RotationValue, Time.deltaTime * RotationValue);
		FlightTime += deltaTime;
	}

	public override bool IsFinished()
	{
		return m_Finished;
	}

	internal void Explode(Agent overrideAgent = null)
	{
		m_Finished = true;
		if (m_Explosion != null)
		{
			Explosion explosion = Mission.Instance.ExplosionCache.Get(m_Explosion, base.Transform.position, Quaternion.identity, new Transform[2] { base.transform, Settings.IgnoreTransform });
			explosion.causer = ((!(overrideAgent == null)) ? overrideAgent : base.Agent);
			explosion.damage = Settings.Damage;
			explosion.FromWeapon = E_WeaponID.SantaPresent;
			if (overrideAgent != null && overrideAgent.IsPlayer)
			{
				explosion.damage *= 1.5f;
				explosion.damageRadius *= 1.5f;
				explosion.damageMaxRadius *= 1.5f;
			}
		}
	}

	protected void PlayHitSound(Collision Coll)
	{
		SemanticMaterialManager.Instance.SpawnImpactEffect(Coll);
	}

	internal void OnCollisionStay(Collision Coll)
	{
	}

	internal void OnCollisionExit(Collision Coll)
	{
	}

	private void OnRenderObject()
	{
		DisplayTrajectory(0f);
		CapsuleCollider capsuleCollider = base.GetComponent<Collider>() as CapsuleCollider;
		if (!(capsuleCollider != null))
		{
		}
	}

	private void ComputeTrajectoryLight(Vector3 inDir)
	{
		Trajectory.Clear();
		Speed = 0f;
		Duration = 0f;
		HitTime = 0f;
		FlightTime = 0f;
		StartPos = base.Transform.position;
		Speed = Throw.ComputeMinSpeed(StartPos, Settings.Destination);
		if (Throw.ComputeVelocity(StartPos, Settings.Destination, Speed, ref Velocity))
		{
			Duration = Throw.ComputeDuration(StartPos, Settings.Destination, Velocity);
		}
		else
		{
			Velocity = inDir * Speed;
			float magnitude = (Settings.Destination - StartPos).magnitude;
			Duration = magnitude / Speed * 1.4142f;
		}
		Duration += 5f;
	}

	private void ComputeTrajectory(Vector3 inDir)
	{
		Trajectory.Clear();
		Speed = 0f;
		Duration = 0f;
		HitTime = 0f;
		FlightTime = 0f;
		StartPos = base.Transform.position;
		Speed = Throw.ComputeMinSpeed(StartPos, Settings.Destination);
		bool flag = Throw.ComputeVelocity(StartPos, Settings.Destination, Speed, ref Velocity);
		if (flag)
		{
			Duration = Throw.ComputeDuration(StartPos, Settings.Destination, Velocity);
			Throw.ComputeTrajectory(StartPos, Velocity, 4f, 0.1f, Trajectory);
		}
		else
		{
			Velocity = inDir * Speed;
			Duration = 1f;
			Trajectory.Add(StartPos);
			Trajectory.Add(Settings.Destination);
		}
		float num = 0f;
		int count = Trajectory.Count;
		int layermask = ~(1 << LayerMask.NameToLayer("Ignore Raycast"));
		for (int i = 0; i < count - 1; i++)
		{
			Vector3 vector = Trajectory[i];
			Vector3 vector2 = Trajectory[i + 1] - vector;
			float num2 = Vector3.Magnitude(vector2);
			num += num2;
			RaycastHit[] array = Physics.RaycastAll(vector, vector2 / num2, num2, layermask);
			RaycastHit[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				RaycastHit raycastHit = array2[j];
				if (!raycastHit.collider.isTrigger)
				{
					Trajectory.RemoveRange(i, count - i);
					Trajectory.Add(raycastHit.point);
					num -= num2;
					Duration = Throw.ComputeDuration(StartPos, raycastHit.point, Velocity);
					return;
				}
			}
		}
		if (!flag && num > 0f)
		{
			Duration = num / Speed * 1.4142f;
		}
		Duration += 5f;
	}

	public void DisplayTrajectory(float DisplayTime)
	{
		DebugDraw.DepthTest = true;
		DebugDraw.DisplayTime = DisplayTime;
		for (int i = 0; i < Trajectory.Count - 1; i++)
		{
			DebugDraw.Line(Color.red, Trajectory[i], Trajectory[i + 1]);
		}
		DebugDraw.DisplayTime = 0f;
	}
}
