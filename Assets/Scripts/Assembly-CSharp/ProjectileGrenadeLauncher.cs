using UnityEngine;

[AddComponentMenu("Weapons/ProjectileGrenadeLauncher")]
public class ProjectileGrenadeLauncher : Projectile
{
	[SerializeField]
	private GrenadeTrail m_Trail;

	protected bool Processed;

	protected float Timer;

	private Renderer Render;

	private ProjectileGrenadeFrag m_Grenade;

	private Item.InitData m_Data = new Item.InitData();

	public override void Awake()
	{
		base.Awake();
		m_Grenade = GetComponentInChildren<ProjectileGrenadeFrag>();
	}

	private Vector3 GetTrailPos()
	{
		Transform transform = m_Grenade.transform;
		Vector3 position = transform.position;
		float sqrMagnitude = (position - base.Transform.position).sqrMagnitude;
		sqrMagnitude = Mathf.Clamp(1f - sqrMagnitude / 25f, 0f, 1f);
		return position + (sqrMagnitude * transform.forward - 0.1f * sqrMagnitude * transform.up + 0.1f * sqrMagnitude * transform.right);
	}

	public override void ProjectileInit(Vector3 pos, Vector3 dir, ProjectileInitSettings inSettings)
	{
		base.ProjectileInit(pos, dir, inSettings);
		Timer = 0f;
		Processed = false;
		m_Data.Owner = inSettings.Agent;
		m_Data.Pos = pos;
		m_Data.Dir = dir;
		m_Grenade.Init(m_Data, inSettings);
		if (m_Trail != null)
		{
			m_Trail.gameObject.SetActive(false);
		}
		if (m_Trail != null)
		{
			m_Trail.InitTrail(GetTrailPos());
			m_Trail.gameObject.SetActive(true);
		}
		Render = GetComponentInChildren<Renderer>();
		if ((bool)Render)
		{
			Render.enabled = true;
		}
	}

	public override void ProjectileUpdate(float deltaTime)
	{
		deltaTime = TimeManager.Instance.GetRealDeltaTime();
		Timer += deltaTime;
		if (!Processed)
		{
			if (m_Grenade.Finished || m_Grenade.Stuck)
			{
				Processed = true;
				Timer = 0f;
				m_Trail.FadeOut();
			}
			if (m_Trail != null)
			{
				m_Trail.UpdateAddTrailPos(GetTrailPos());
			}
		}
	}

	public override void ProjectileDeinit()
	{
	}

	public override bool IsFinished()
	{
		if (Processed && m_Grenade.Finished && Timer > 0.7f)
		{
			return true;
		}
		return false;
	}
}
