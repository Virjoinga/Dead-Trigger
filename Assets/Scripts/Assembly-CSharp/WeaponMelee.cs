using System;
using UnityEngine;

[AddComponentMenu("Weapons/Melee")]
public class WeaponMelee : WeaponBase
{
	private const float m_StartupTime = 0.35f;

	private bool m_Firing;

	private bool m_StartFire = true;

	private float m_FireTimer;

	private void Awake()
	{
	}

	public override bool FireStartupDone()
	{
		if (base.WeaponID != 0)
		{
			return true;
		}
		return !m_StartFire;
	}

	public override void Fire()
	{
		string empty = string.Empty;
		if ((bool)WeaponAnimsSetup.Fire)
		{
			empty = WeaponAnimsSetup.Fire.name;
		}
		if (base.WeaponID == E_WeaponID.None)
		{
			if (m_StartFire)
			{
				if (!m_Firing)
				{
					if ((bool)WeaponAnimsSetup.Fire)
					{
						if (Animation.IsPlaying(empty))
						{
							Animation.Stop(empty);
						}
						Animation.Play(empty);
					}
					PlaySoundFireStart();
				}
				m_Firing = true;
				return;
			}
			m_FireTimer = 0f;
		}
		if ((bool)WeaponAnimsSetup.Fire)
		{
			Animation[empty].speed = 1f;
		}
		m_Firing = true;
		base.Fire();
	}

	protected void Update()
	{
		if (((bool)Audio && !Audio.isPlaying) || (!m_Firing && !IsFiring() && !IsBusy() && Audio.isPlaying && !IsPlayingIdleSound()))
		{
			PlaySoundIdle();
		}
		if (!m_Firing)
		{
			return;
		}
		m_FireTimer += TimeManager.Instance.GetRealDeltaTime();
		if (m_FireTimer >= 0.35f)
		{
			m_StartFire = !m_StartFire;
			if (m_StartFire)
			{
				m_Firing = false;
				m_FireTimer = 0f;
			}
		}
		if (base.WeaponID == E_WeaponID.None && m_StartFire)
		{
			float speed = Mathf.Sin((float)Math.PI / 2f * Mathf.Clamp(m_FireTimer / 0.35f, 0f, 1f));
			if ((bool)WeaponAnimsSetup.Fire)
			{
				string text = WeaponAnimsSetup.Fire.name;
				Animation[text].speed = speed;
			}
		}
	}

	protected void PlaySoundFireStart()
	{
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		if (soundFire.Length <= 1)
		{
			return;
		}
		AudioClip audioClip = soundFire[1];
		Audio.loop = false;
		if (Audio.isPlaying)
		{
			Audio.Stop();
			if (Audio.clip != audioClip)
			{
				Audio.clip = audioClip;
			}
			Audio.Play();
		}
		else
		{
			Audio.clip = audioClip;
			Audio.Play();
		}
	}

	private bool IsPlayingIdleSound()
	{
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		if (soundFire.Length > 0)
		{
			AudioClip audioClip = soundFire[0];
			if (Audio.clip == audioClip)
			{
				return true;
			}
		}
		return false;
	}

	protected void PlaySoundIdle()
	{
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		if (soundFire.Length <= 0)
		{
			return;
		}
		AudioClip audioClip = soundFire[0];
		Audio.loop = true;
		if (Audio.isPlaying)
		{
			Audio.Stop();
			if (Audio.clip != audioClip)
			{
				Audio.clip = audioClip;
			}
			Audio.Play();
		}
		else
		{
			Audio.clip = audioClip;
			Audio.Play();
		}
	}

	private bool IsPlayingFireSound()
	{
		if (!Audio.isPlaying)
		{
			return false;
		}
		int num = 0;
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		num = ((base.WeaponID != 0) ? 1 : 2);
		for (int i = num; i < soundFire.Length; i++)
		{
			if (Audio.clip == soundFire[i])
			{
				return true;
			}
		}
		return false;
	}

	protected override void PlaySoundFire()
	{
		int num = 0;
		if (IsPlayingFireSound())
		{
			return;
		}
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		num = ((base.WeaponID != 0) ? 1 : 2);
		if (soundFire.Length <= num)
		{
			return;
		}
		AudioClip audioClip = soundFire[UnityEngine.Random.Range(num, soundFire.Length - num)];
		Audio.loop = true;
		if (Audio.isPlaying)
		{
			Audio.Stop();
			if (Audio.clip != audioClip)
			{
				Audio.clip = audioClip;
			}
			Audio.Play();
		}
		else
		{
			Audio.clip = audioClip;
			Audio.Play();
		}
	}
}
