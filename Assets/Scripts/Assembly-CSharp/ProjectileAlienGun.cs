#define DEBUG
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Weapons/ProjectileAlienGun")]
public class ProjectileAlienGun : ProjectileBullet
{
	[SerializeField]
	private ParticleSystem m_HitParticle;

	[SerializeField]
	private Color m_FadeoutColor;

	private ParticleSystem m_hitPtc;

	public Color FadeoutColor
	{
		get
		{
			return m_FadeoutColor;
		}
		private set
		{
			m_FadeoutColor = value;
		}
	}

	public override void ProjectileUpdate(float deltaTime)
	{
		Timer += deltaTime;
		if (Processed)
		{
			return;
		}
		DebugUtils.Assert(!IsFinished());
		DebugUtils.Assert(!Hit);
		if (base.Transform.localScale.z != 1f)
		{
			base.Transform.localScale = new Vector3(1f, 1f, Mathf.Min(1f, base.Transform.localScale.z + Time.deltaTime * 8f));
		}
		Vector3 position = base.Transform.position + base.Transform.forward * ProjectileBullet.ShotCollisionDist;
		int layersMask = ~(ObjectLayerMask.IgnoreRaycast | ObjectLayerMask.IgnoreBullets);
		List<HitInfo> list = HitDetection.RayCast(base.Transform.position, base.Transform.forward, ProjectileBullet.ShotCollisionDist, layersMask);
		foreach (HitInfo item in list)
		{
			RaycastHit data = item.data;
			if (data.transform == Settings.IgnoreTransform)
			{
				continue;
			}
			base.Transform.position = data.point;
			HitZone hitZone = item.hitZone;
			if ((bool)hitZone)
			{
				hitZone.OnProjectileHit(this);
			}
			else
			{
				data.transform.SendMessage("OnProjectileHit", this, SendMessageOptions.DontRequireReceiver);
			}
			Damage();
			if (base.ignoreThisHit)
			{
				base.ignoreThisHit = false;
				continue;
			}
			//FluidSurface component = data.collider.GetComponent<FluidSurface>();
			/*if (component != null)
			{
				component.AddDropletAtWorldPos(data.point, 0.3f, 0.15f);
			}
			if ((!data.collider.isTrigger || component != null) && m_HitParticle != null)
			{
				if ((bool)m_hitPtc)
				{
					Object.Destroy(m_hitPtc.gameObject);
				}
				m_hitPtc = Object.Instantiate(m_HitParticle) as ParticleSystem;
				m_hitPtc.transform.position = data.point + data.normal * 0.4f;
				m_hitPtc.Play();
				GenerateWaveFX();
			}*/
			if (data.collider.isTrigger)
			{
				continue;
			}
			position = data.point;
			Hit = true;
			if (base.GetComponent<Renderer>() != null)
			{
				base.GetComponent<Renderer>().enabled = false;
			}
			break;
		}
		if (base.Agent != null && base.Agent.IsPlayer)
		{
			Game.Instance.MissionResultData.RegisterShot(base.hitProcessed);
		}
		Processed = true;
		base.Transform.position = position;
	}

	private void GenerateWaveFX()
	{
		if (!(Camera.main == null) && DeviceInfo.GetDetectedPerformanceLevel() != 0)
		{
			float magnitude = (base.transform.position - Camera.main.transform.position).magnitude;
			float t = Mathf.Min(magnitude / 25f, 1f);
			MFExplosionPostFX.S_WaveParams waveParams = default(MFExplosionPostFX.S_WaveParams);
			waveParams.m_Amplitude = Mathf.Lerp(0.3f, 0.1f, t);
			waveParams.m_Duration = 3f;
			waveParams.m_Freq = 20f;
			waveParams.m_Speed = 1f;
			waveParams.m_Radius = Mathf.Lerp(0.25f, 0.1f, t);
			waveParams.m_Delay = 0f;
			CamExplosionFXMgr.Instance.SpawnExplosionWaveFX(base.transform.position, waveParams, 0f);
		}
	}

	public override void ProjectileDeinit()
	{
		base.ProjectileDeinit();
	}
}
