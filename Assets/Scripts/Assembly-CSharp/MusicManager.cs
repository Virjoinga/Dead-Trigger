using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[AddComponentMenu("Music + Sound/Music Manager [NES]")]
public class MusicManager : MonoBehaviour
{
	[Serializable]
	public class MusicEvent
	{
		public static string m_DefaultName = "[noname]";

		public string m_Name;

		public AudioClip m_Clip;

		public float m_FadeOutTime = 1f;

		public float m_FadeInTime = 1f;

		public float m_Volume = 1f;

		public MusicEvent(string name, AudioClip clip, float fadeoutTime, float fadeInTime, float volume)
		{
			m_Name = name;
			m_Clip = clip;
			m_FadeOutTime = fadeoutTime;
			m_FadeInTime = fadeInTime;
			m_Volume = volume;
		}

		public MusicEvent()
		{
			m_Name = m_DefaultName;
			m_Clip = null;
			m_FadeOutTime = 1f;
			m_FadeInTime = 1f;
			m_Volume = 1f;
		}
	}

	private const string defaultEventName = "default";

	public MusicEvent defaultEvent = new MusicEvent("default", null, 0f, 1f, 1f);

	public List<MusicEvent> musicEvents = new List<MusicEvent>();

	internal AudioSource Audio;

	internal float UnmodifiedVolume = 1f;

	internal float OptionsVolume = 1f;

	internal float FadeMusicVolume;

	public static MusicManager Instance;

	private void Awake()
	{
		Instance = this;
		Audio = base.GetComponent<AudioSource>();
	}

	private void Start()
	{
		PlayDefaultMusic();
	}

	private void OnDestroy()
	{
		if ((bool)Audio)
		{
			Audio.Stop();
		}
	}

	private void Update()
	{
		float num = ((!(Time.timeScale >= 1f)) ? Mathf.Max(Time.timeScale, 0.5f) : 1f);
		if (Time.deltaTime > float.Epsilon && Audio.pitch != num)
		{
			Audio.pitch = num;
		}
	}

	[NESAction(Argument1 = "GetAvailibleMusicEvents")]
	public void PlayMusic(string inMusicName)
	{
		if (string.IsNullOrEmpty(inMusicName))
		{
			Debug.LogError("Requested music is null");
			return;
		}
		if (inMusicName == "default")
		{
			PlayDefaultMusic();
			return;
		}
		MusicEvent musicEvent = musicEvents.Find((MusicEvent item) => item.m_Name == inMusicName);
		if (musicEvent == null)
		{
			Debug.LogError("Unknown music type " + inMusicName);
		}
		else
		{
			SetNewMusic(musicEvent);
		}
	}

	public void ApplyOptionsChange()
	{
		OptionsVolume = ((!GuiOptions.musicOn) ? 0f : GuiOptions.musicVolume);
		Audio.volume = UnmodifiedVolume * OptionsVolume;
		FadeMusicVolume = UnmodifiedVolume * OptionsVolume;
	}

	public void PlayDefaultMusic()
	{
		SetNewMusic(defaultEvent);
	}

	public void FadeOutMusic(float fadeOutTime)
	{
		StartCoroutine(SwitchMusic(null, 0f, fadeOutTime, 0f));
	}

	private string[] GetAvailibleMusicEvents()
	{
		List<string> list = new List<string>();
		list.Add(defaultEvent.m_Name);
		foreach (MusicEvent musicEvent in musicEvents)
		{
			if (musicEvent.m_Name != MusicEvent.m_DefaultName)
			{
				list.Add(musicEvent.m_Name);
			}
		}
		return list.ToArray();
	}

	internal void SetNewMusic(MusicEvent inMusic)
	{
		SetNewMusic(inMusic.m_Clip, inMusic.m_Volume, inMusic.m_FadeOutTime, inMusic.m_FadeInTime);
	}

	internal void SetNewMusic(AudioClip clip, float volume, float fadeOutTime, float fadeIntime)
	{
		UnmodifiedVolume = volume;
		StartCoroutine(SwitchMusic(clip, UnmodifiedVolume * OptionsVolume, fadeOutTime, fadeIntime));
	}

	internal IEnumerator SwitchMusic(AudioClip clip, float inMusicVolume, float fadeOutTime, float fadeIntime)
	{
		FadeMusicVolume = inMusicVolume;
		if (Audio.clip == clip)
		{
			yield break;
		}
		if (Audio.isPlaying)
		{
			if (fadeOutTime == 0f)
			{
				Audio.volume = 0f;
				Audio.Stop();
			}
			else
			{
				float maxVolume = Audio.volume;
				float volume2 = Audio.volume;
				while (volume2 > 0f)
				{
					volume2 -= 1f / fadeOutTime * Time.deltaTime * maxVolume;
					if (volume2 < 0f)
					{
						volume2 = 0f;
					}
					Audio.volume = volume2;
					yield return new WaitForEndOfFrame();
				}
				Audio.Stop();
				Audio.clip = null;
			}
		}
		yield return new WaitForEndOfFrame();
		if (!(clip != null))
		{
			yield break;
		}
		Audio.clip = clip;
		Audio.Play();
		if (fadeIntime == 0f)
		{
			Audio.volume = FadeMusicVolume;
			yield break;
		}
		float volume = 0f;
		while (volume < FadeMusicVolume)
		{
			volume += 1f / fadeIntime * Time.deltaTime * FadeMusicVolume;
			if (volume > FadeMusicVolume)
			{
				volume = FadeMusicVolume;
			}
			Audio.volume = volume;
			yield return new WaitForEndOfFrame();
		}
		Audio.volume = FadeMusicVolume;
	}
}
