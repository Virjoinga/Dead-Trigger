using UnityEngine;

[AddComponentMenu("Game/NES/PlayOnEnable")]
public class PlayOnEnable : MonoBehaviour
{
	public bool PlayAnimation;

	public bool PlayAudio;

	public bool PlayParticle;

	private void OnEnable()
	{
		if (PlayAnimation && (bool)base.GetComponent<Animation>())
		{
			base.GetComponent<Animation>().Play();
		}
		if (PlayAudio && (bool)base.GetComponent<AudioSource>())
		{
			base.GetComponent<AudioSource>().Play();
		}
		if (PlayParticle && (bool)base.GetComponent<ParticleSystem>())
		{
			base.GetComponent<ParticleSystem>().Play();
		}
	}

	private void OnDisable()
	{
		if (PlayAnimation && (bool)base.GetComponent<Animation>())
		{
			base.GetComponent<Animation>().Stop();
		}
		if (PlayAudio && (bool)base.GetComponent<AudioSource>())
		{
			base.GetComponent<AudioSource>().Stop();
		}
		if (PlayParticle && (bool)base.GetComponent<ParticleSystem>())
		{
			base.GetComponent<ParticleSystem>().Stop();
		}
	}
}
