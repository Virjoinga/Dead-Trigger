using UnityEngine;

[AddComponentMenu("Miscellaneous/Play Sound From Animations")]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public class PlaySoundFromAnimations : MonoBehaviour
{
	public void AnimNotify_PlayOneShot(AudioClip inClip)
	{
		base.GetComponent<AudioSource>().PlayOneShot(inClip);
	}
}
