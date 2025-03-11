using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InteractionTargetState
{
	[Serializable]
	public class GameEvent
	{
		public string Name;

		public GameEvents.E_State State;

		public GameEvent(string name, GameEvents.E_State state)
		{
			Name = name;
			State = state;
		}
	}

	[Serializable]
	public class InteractionParticle
	{
		public float Delay;

		public float Life;

		public Transform Parent;
	}

	[Serializable]
	public class InteractionSound
	{
		public AudioSource Audio;

		public float Delay;

		public float Life;

		public Transform Parent;

		public InteractionSound()
		{
		}

		public InteractionSound(AudioSource audio, float delay, float life, Transform parent)
		{
			Audio = audio;
			Delay = delay;
			Life = life;
			Parent = parent;
		}

		~InteractionSound()
		{
			Audio = null;
			Parent = null;
		}
	}

	public List<GameEvent> GameEvents = new List<GameEvent>();

	public List<InteractionParticle> Emitters = new List<InteractionParticle>();

	public List<InteractionSound> Sounds = new List<InteractionSound>();

	public AnimationClip AnimationClip;

	public Animation Animation;

	public AnimationClip CameraAnimation;

	public Camera Camera;

	private GameObject GameObject;

	~InteractionTargetState()
	{
		AnimationClip = null;
		Animation = null;
		CameraAnimation = null;
		Camera = null;
		GameObject = null;
		GameEvents.Clear();
		Emitters.Clear();
		Sounds.Clear();
	}

	public void Initialize(GameObject go)
	{
		GameObject = go;
		if (Camera != null)
		{
			Camera.gameObject._SetActiveRecursively(false);
		}
	}

	public void Enable()
	{
		foreach (GameEvent gameEvent in GameEvents)
		{
			GameBlackboard.Instance.GameEvents.AddEventChangeHandler(gameEvent.Name, EventHandler);
		}
	}

	public void Disable()
	{
		foreach (GameEvent gameEvent in GameEvents)
		{
			GameBlackboard.Instance.GameEvents.RemoveEventChangeHandler(gameEvent.Name, EventHandler);
		}
	}

	public void Reset()
	{
		foreach (InteractionSound sound in Sounds)
		{
			sound.Audio.Stop();
		}
	}

	public void EventHandler(string name, GameEvents.E_State state)
	{
		foreach (GameEvent gameEvent in GameEvents)
		{
			if (GameBlackboard.Instance.GameEvents.GetState(gameEvent.Name) != gameEvent.State)
			{
				return;
			}
		}
		InteractionStart();
	}

	protected void InteractionStart()
	{
		if ((bool)Camera && (bool)CameraAnimation)
		{
			Mission.Instance.StartCoroutine(PlayCameraAnim());
		}
		if ((bool)Animation && (bool)AnimationClip)
		{
			AnimationState animationState = Animation[AnimationClip.name];
			if (!animationState)
			{
				Debug.LogError(string.Concat(GameObject, " has no animation '", AnimationClip.name, "'"));
			}
			Animation[AnimationClip.name].speed = 1f;
			if (Animation.IsPlaying(AnimationClip.name))
			{
				Animation[AnimationClip.name].time = 0f;
			}
			else
			{
				Animation.Play(AnimationClip.name, PlayMode.StopAll);
			}
		}
		int num = 0;
		while (Emitters != null && num < Emitters.Count)
		{
			num++;
		}
		int num2 = 0;
		while (Sounds != null && num2 < Sounds.Count)
		{
			Mission.Instance.StartCoroutine(SoundRun(Sounds[num2].Audio, Sounds[num2].Delay));
			Mission.Instance.StartCoroutine(SoundStop(Sounds[num2].Audio, Sounds[num2].Delay + Sounds[num2].Life));
			if ((bool)Sounds[num2].Parent)
			{
				Sounds[num2].Audio.transform.parent = Sounds[num2].Parent;
			}
			num2++;
		}
	}

	protected void InteractionStartEx()
	{
		if ((bool)Animation && (bool)AnimationClip)
		{
			Animation[AnimationClip.name].speed = 10000f;
			Animation.Play(AnimationClip.name, PlayMode.StopAll);
			Debug.Log(string.Concat(GameObject, " play anim ", AnimationClip.name));
		}
	}

	private IEnumerator SoundRun(AudioSource audio, float delay)
	{
		yield return new WaitForSeconds(delay);
		audio.Play();
	}

	private IEnumerator SoundStop(AudioSource audio, float delay)
	{
		yield return new WaitForSeconds(delay);
		audio.Stop();
	}

	private IEnumerator PlayCameraAnim()
	{
		if (AnimationClip != null)
		{
			yield return new WaitForSeconds(Animation[AnimationClip.name].length);
		}
		Player.Instance.StopMove(true);
		MFGuiManager.Instance.FadeOut(0.2f);
		yield return new WaitForSeconds(0.2f);
		Camera old = Camera.main;
		old.gameObject._SetActiveRecursively(false);
		GuiHUD.Instance.Hide();
		Camera.gameObject._SetActiveRecursively(true);
		Camera.GetComponent<Animation>().Play(CameraAnimation.name);
		MFGuiManager.Instance.FadeIn(0.2f);
		yield return new WaitForSeconds(0.2f);
		yield return new WaitForSeconds(Camera.GetComponent<Animation>()[CameraAnimation.name].length);
		MFGuiManager.Instance.FadeOut(0.2f);
		yield return new WaitForSeconds(0.2f);
		Camera.gameObject._SetActiveRecursively(false);
		old.gameObject._SetActiveRecursively(true);
		MFGuiManager.Instance.FadeIn(0.2f);
		Player.Instance.StopMove(false);
		GuiHUD.Instance.Show();
	}
}
