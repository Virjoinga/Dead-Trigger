using System.Collections;
using UnityEngine;

[AddComponentMenu("Interaction/Weapon Box")]
public class InteractionWeaponBox : InteractionObject
{
	public E_WeaponID Weapon;

	public GameObject WeaponVisual;

	public AudioClip SoundTake;

	public GameObject CutsceneCamera;

	public AnimationClip CameraAnim;

	public float FadeOutTime = 0.3f;

	public float FadeInTime = 0.3f;

	public AnimationClip AnimOpen;

	public AnimationClip AnimClose;

	public AudioClip SoundOpen;

	public AudioClip SoundClose;

	public float ParticleDelay;

	public override bool IsInteractionFinished { get; protected set; }

	private void Start()
	{
		Initialize();
		CutsceneCamera._SetActiveRecursively(false);
		Animation = base.GetComponent<Animation>();
	}

	private void OnDestroy()
	{
		WeaponVisual = null;
		SoundTake = null;
		CutsceneCamera = null;
		CameraAnim = null;
		AnimOpen = null;
		AnimClose = null;
		SoundOpen = null;
		SoundClose = null;
	}

	public override void DoInteraction()
	{
		base.InteractionObjectUsable = false;
		Disable();
		if ((bool)SoundTake)
		{
			Player.Instance.Owner.SoundPlay(SoundTake);
		}
		StartCoroutine(PlayCutscene());
	}

	private IEnumerator PlayCutscene()
	{
		yield return new WaitForSeconds(FadeOutTime * 0.5f);
		IsInteractionFinished = true;
	}

	public override void Reset()
	{
		if ((bool)WeaponVisual)
		{
			WeaponVisual._SetActiveRecursively(true);
		}
		CancelInvoke();
		StopAllCoroutines();
		base.Reset();
	}

	private void PlayParticle()
	{
	}
}
