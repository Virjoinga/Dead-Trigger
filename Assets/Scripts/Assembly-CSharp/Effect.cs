using UnityEngine;

internal class Effect
{
	private readonly ResourceCache m_Cache;

	private readonly GameObject m_Object;

	private readonly float m_SpawnTime;

	private ParticleSystem[] m_Gfx;

	public Effect(GameObject Effect, ResourceCache Cache)
	{
		m_Object = Effect;
		m_Cache = Cache;
		m_SpawnTime = Time.timeSinceLevelLoad;
		m_Gfx = m_Object.GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] gfx = m_Gfx;
		foreach (ParticleSystem particleSystem in gfx)
		{
			particleSystem.Play();
		}
	}

	public bool IsActive()
	{
		bool flag = false;
		ParticleSystem[] gfx = m_Gfx;
		foreach (ParticleSystem particleSystem in gfx)
		{
			if (particleSystem.isPlaying || particleSystem.particleCount > 0)
			{
				flag = true;
				break;
			}
		}
		if (!flag || Time.timeSinceLevelLoad - m_SpawnTime > 10f)
		{
		}
		return flag;
	}

	public void ShutDown()
	{
		m_Object._SetActiveRecursively(false);
		m_Cache.Return(m_Object);
	}
}
