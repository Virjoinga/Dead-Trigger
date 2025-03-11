#define DEBUG
using System.Collections;
using UnityEngine;

[AddComponentMenu("Game/Door")]
[RequireComponent(typeof(Animation))]
public class Door : MonoBehaviour, IGameZoneChild, IGameZoneChild_AutoRegister
{
	private enum E_State
	{
		Close = 0,
		Open = 1
	}

	[SerializeField]
	private E_State m_InitState;

	[SerializeField]
	private AnimationClip m_OpenAnim;

	[SerializeField]
	private AudioClip m_OpenSound;

	[SerializeField]
	private AnimationClip m_CloseAnim;

	[SerializeField]
	private AudioClip m_CloseSound;

	private GameObject GameObject;

	private Animation Animation;

	private AudioSource Audio;

	private UnityEngine.AI.OffMeshLink[] OffMeshLinks;

	private E_State m_ActualState;

	private bool m_PingPongAnim;

	public bool IsActivatedWithGameZone()
	{
		return true;
	}

	private void Awake()
	{
		GameObject = base.gameObject;
		Animation = GameObject.GetComponent<Animation>();
		Audio = GameObject.GetComponent<AudioSource>();
		m_ActualState = m_InitState;
		if (m_OpenAnim == m_CloseAnim)
		{
			m_PingPongAnim = true;
			Animation[m_OpenAnim.name].wrapMode = WrapMode.Once;
		}
		OffMeshLinks = GetComponents<UnityEngine.AI.OffMeshLink>();
		SetOffMeshLinks(m_ActualState);
		CheckDataConsistency();
	}

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public void Reset()
	{
	}

	[NESAction]
	private void Open()
	{
		if (m_ActualState != E_State.Open)
		{
			StopAllCoroutines();
			StartCoroutine(Open_Coroutine());
		}
	}

	[NESAction]
	private void Close()
	{
		if (m_ActualState != 0)
		{
			StopAllCoroutines();
			StartCoroutine(Close_Coroutine());
		}
	}

	private IEnumerator Open_Coroutine()
	{
		if (m_OpenSound != null)
		{
			Audio.PlayOneShot(m_OpenSound);
		}
		if (m_PingPongAnim)
		{
			Animation[m_OpenAnim.name].speed = 1f;
		}
		Animation.Play(m_OpenAnim.name);
		float time = Animation[m_OpenAnim.name].length - 0.01f;
		yield return new WaitForSeconds(time / 2f);
		SetOffMeshLinks(E_State.Open);
		yield return new WaitForSeconds(time / 2f);
		m_ActualState = E_State.Open;
	}

	private IEnumerator Close_Coroutine()
	{
		if (m_CloseSound != null)
		{
			Audio.PlayOneShot(m_CloseSound);
		}
		if (m_PingPongAnim)
		{
			Animation[m_CloseAnim.name].time = Animation[m_CloseAnim.name].length;
			Animation[m_CloseAnim.name].speed = -1f;
		}
		Animation.Play(m_CloseAnim.name);
		float time = Animation[m_CloseAnim.name].length - 0.01f;
		SetOffMeshLinks(E_State.Close);
		yield return new WaitForSeconds(time);
		m_ActualState = E_State.Close;
	}

	private void SetOffMeshLinks(E_State state)
	{
		if (OffMeshLinks.Length != 0)
		{
			bool activated = state == E_State.Open;
			UnityEngine.AI.OffMeshLink[] offMeshLinks = OffMeshLinks;
			foreach (UnityEngine.AI.OffMeshLink offMeshLink in offMeshLinks)
			{
				offMeshLink.activated = activated;
			}
		}
	}

	public virtual void CheckDataConsistency()
	{
		DebugUtils.Assert(base.GetComponent<Animation>() != null);
		DebugUtils.Assert(m_OpenAnim != null);
		DebugUtils.Assert(m_CloseAnim != null);
	}

	private void OnDrawGizmos()
	{
		BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
		if (boxCollider != null)
		{
			Gizmos.color = new Color(1f, 1f, 0f, 0.35f);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawCube(boxCollider.center, boxCollider.size);
		}
	}
}
