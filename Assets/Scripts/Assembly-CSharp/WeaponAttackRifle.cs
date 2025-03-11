using System;
using UnityEngine;

[AddComponentMenu("Weapons/AttackRifle")]
public class WeaponAttackRifle : WeaponBase
{
	private const float m_StartupTime = 0.75f;

	private bool m_Firing;

	private bool m_StartFire = true;

	private float m_FireTimer;

	private void Awake()
	{
	}

	public override bool FireStartupDone()
	{
		if (base.WeaponID != E_WeaponID.Minigun)
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
		if (base.WeaponID == E_WeaponID.Minigun)
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
		if (!m_Firing)
		{
			return;
		}
		m_FireTimer += TimeManager.Instance.GetRealDeltaTime();
		if (m_FireTimer >= 0.75f)
		{
			m_StartFire = !m_StartFire;
			if (m_StartFire)
			{
				m_Firing = false;
				m_FireTimer = 0f;
			}
		}
		if (base.WeaponID == E_WeaponID.Minigun && m_StartFire)
		{
			float speed = Mathf.Sin((float)Math.PI / 2f * Mathf.Clamp(m_FireTimer / 0.75f, 0f, 1f));
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
		if (soundFire.Length <= 0)
		{
			return;
		}
		AudioClip audioClip = soundFire[0];
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

	protected override void PlaySoundFire()
	{
		int num = 0;
		AudioClip[] soundFire = SoundsSetup.SoundFire;
		if (base.WeaponID == E_WeaponID.Minigun)
		{
			num = 1;
		}
		if (soundFire.Length <= num)
		{
			return;
		}
		AudioClip audioClip = soundFire[UnityEngine.Random.Range(num, soundFire.Length - num)];
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
