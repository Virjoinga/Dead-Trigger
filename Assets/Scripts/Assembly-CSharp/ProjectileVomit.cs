using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Weapons/ProjectileVomit")]
public class ProjectileVomit : Projectile, IImportantObject
{
	private int m_HitSoundNum;

	public int m_HitSoundLimit = 3;

	public Explosion m_Explosion;

	public ParticleSystem ParticleFly;

	public ParticleSystem ParticleHit;

	public GameObject ObjectFly;

	private AudioSource Audio;

	private bool m_Finished;

	private List<Vector3> Trajectory = new List<Vector3>();

	private Vector3 Velocity = new Vector3(0f, 0f, 0f);

	private new float Speed;

	private float Duration;

	private float HitTime;

	private Vector3 StartPos = new Vector3(0f, 0f, 0f);

	private float FlightTime;

	private ParticleSystem CurrentParticle;

	private GameObject FlyingObject;

	public override void Awake()
	{
		base.Awake();
		Audio = GetComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		m_Explosion = null;
		if (CurrentParticle != null)
		{
			Object.Destroy(CurrentParticle);
			CurrentParticle = null;
		}
		if (FlyingObject != null)
		{
			Object.Destroy(FlyingObject);
			FlyingObject = null;
		}
	}

	public override void ProjectileInit(Vector3 pos, Vector3 dir, ProjectileInitSettings inSettings)
	{
		base.ProjectileInit(pos, dir, inSettings);
		m_Finished = false;
		ComputeTrajectoryLight(dir);
		StartCoroutine(_PlayFlyParticle(0.3f, pos));
		FlightTime = -0.3f;
	}

	private IEnumerator _PlayFlyParticle(float delay, Vector3 pos)
	{
		yield return new WaitForSeconds(delay);
		if (ObjectFly == null)
		{
			if (ParticleFly != null)
			{
				CurrentParticle = Object.Instantiate(ParticleFly) as ParticleSystem;
				CurrentParticle.transform.parent = base.Transform;
				CurrentParticle.transform.localPosition = new Vector3(0f, 0f, 0f);
				CurrentParticle.Play();
			}
		}
		else
		{
			FlyingObject = Object.Instantiate(ObjectFly) as GameObject;
			FlyingObject.transform.parent = base.Transform;
			FlyingObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			FlyingObject.SetActive(true);
		}
		if ((bool)Audio)
		{
			Audio.Play();
		}
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
		if (FlightTime < 0f)
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
		int layerMask = ~(ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.IgnoreBullets);
		RaycastHit hitInfo;
		if (Physics.Raycast(position, vector.normalized, out hitInfo, vector.magnitude, layerMask) && !hitInfo.collider.isTrigger)
		{
			HitTime = FlightTime + hitInfo.distance / Velocity.magnitude;
		}
		FlightTime += deltaTime;
	}

	public override bool IsFinished()
	{
		return m_Finished;
	}

	internal void Explode()
	{
		m_Finished = true;
		if (CurrentParticle != null)
		{
			CurrentParticle.Stop();
			CurrentParticle.Clear();
			Object.Destroy(CurrentParticle);
		}
		else if (FlyingObject != null)
		{
			FlyingObject.SetActive(false);
			Object.Destroy(FlyingObject);
		}
		if ((bool)ParticleHit)
		{
			CurrentParticle = Object.Instantiate(ParticleHit) as ParticleSystem;
			CurrentParticle.transform.position = base.Transform.position;
			CurrentParticle.Play();
		}
		if (m_Explosion != null)
		{
			Explosion explosion = Mission.Instance.ExplosionCache.Get(m_Explosion, base.Transform.position, Quaternion.identity, new Transform[2] { base.transform, Settings.IgnoreTransform });
			explosion.causer = base.Agent;
			explosion.damage = Settings.Damage;
			explosion.FromWeapon = ((base.ProjectileType != E_ProjectileType.VomitGreen) ? E_WeaponID.VomitRed : E_WeaponID.VomitGreen);
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

	public E_ImportantObjectType GetImportantObjectType()
	{
		return E_ImportantObjectType.Vomit;
	}

	public GameObject GetGameObject()
	{
		return base.GameObject;
	}
}
