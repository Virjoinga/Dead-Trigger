using UnityEngine;

internal class EffectUtils
{
	public static void ShowRenderers(MeshRenderer[] List)
	{
		foreach (MeshRenderer meshRenderer in List)
		{
			if (meshRenderer != null)
			{
				meshRenderer.enabled = true;
			}
		}
	}

	public static void HideRenderers(MeshRenderer[] List)
	{
		foreach (MeshRenderer meshRenderer in List)
		{
			if (meshRenderer != null)
			{
				meshRenderer.enabled = false;
			}
		}
	}

	public static bool ActivateParticles(ParticleSystem[] List)
	{
		bool result = false;
		foreach (ParticleSystem particleSystem in List)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
				result = true;
			}
		}
		return result;
	}

	public static bool AreParticlesFinished(ParticleSystem[] List)
	{
		bool flag = false;
		foreach (ParticleSystem particleSystem in List)
		{
			if (particleSystem != null)
			{
				flag |= particleSystem.IsAlive();
			}
		}
		return !flag;
	}
}
