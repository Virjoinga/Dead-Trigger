using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDLObstacle : MonoBehaviour
{
	public enum E_State
	{
		Closed = 0,
		FullyOpen = 1,
		OpenForAI = 2
	}

	[SerializeField]
	private E_State m_InitState;

	[SerializeField]
	private AnimationClip m_OpenAnimUpper;

	[SerializeField]
	private AnimationClip m_OpenAnimLower;

	[SerializeField]
	private AudioClip m_OpenSoundUpper;

	[SerializeField]
	private AudioClip m_OpenSoundLower;

	private AnimationClip m_CloseAnimUpper;

	private AnimationClip m_CloseAnimLower;

	private AudioClip m_CloseSoundUpper;

	private AudioClip m_CloseSoundLower;

	private GameObject UpperMesh;

	private GameObject LowerMesh;

	private Animation AnimationUpper;

	private Animation AnimationLower;

	private AudioSource AudioUpper;

	private AudioSource AudioLower;

	private UnityEngine.AI.OffMeshLink[] OffMeshLinks;

	private ActionPoint[] ActionPoints;

	private E_State m_ActualState;

	private List<GameObject> m_Icons = new List<GameObject>();

	public bool IsActivatedWithGameZone()
	{
		return true;
	}

	private void FindIcons(Transform transform)
	{
		foreach (Transform item in transform)
		{
			if (item.name == "Icon")
			{
				m_Icons.Add(item.gameObject);
				item.gameObject._SetActiveRecursively(false);
			}
		}
	}

	private void Awake()
	{
		m_CloseAnimUpper = m_OpenAnimUpper;
		m_CloseAnimLower = m_OpenAnimLower;
		m_CloseSoundUpper = m_OpenSoundUpper;
		m_CloseSoundLower = m_OpenSoundLower;
		foreach (Transform item in base.transform)
		{
			if (item.name == "UpperMesh")
			{
				UpperMesh = item.gameObject;
				FindIcons(UpperMesh.transform);
				if ((bool)UpperMesh.gameObject.GetComponent<Renderer>())
				{
					UpperMesh.gameObject.GetComponent<Renderer>().enabled = true;
				}
			}
			else if (item.name == "LowerMesh")
			{
				LowerMesh = item.gameObject;
				LowerMesh.SetActive(true);
				FindIcons(LowerMesh.transform);
				if ((bool)LowerMesh.gameObject.GetComponent<Renderer>())
				{
					LowerMesh.gameObject.GetComponent<Renderer>().enabled = true;
				}
			}
			else if (item.name == "Icon")
			{
				m_Icons.Add(item.gameObject);
				item.gameObject._SetActiveRecursively(false);
			}
		}
		if ((bool)UpperMesh)
		{
			AnimationUpper = UpperMesh.GetComponent<Animation>();
			AudioUpper = UpperMesh.GetComponent<AudioSource>();
			AnimationUpper.AddClip(m_OpenAnimUpper, m_OpenAnimUpper.name);
			AnimationUpper.AddClip(m_CloseAnimUpper, m_CloseAnimUpper.name);
		}
		AnimationLower = LowerMesh.GetComponent<Animation>();
		AudioLower = LowerMesh.GetComponent<AudioSource>();
		AnimationLower.AddClip(m_OpenAnimLower, m_OpenAnimLower.name);
		AnimationLower.AddClip(m_CloseAnimLower, m_CloseAnimLower.name);
		m_ActualState = m_InitState;
		OffMeshLinks = GetComponentsInChildren<UnityEngine.AI.OffMeshLink>();
		ActionPoints = GetComponentsInChildren<ActionPoint>();
		SetOffMeshLinks(m_ActualState);
		CheckDataConsistency();
	}

	[NESAction]
	public void FullyOpen()
	{
		if (m_ActualState != E_State.FullyOpen)
		{
			StopAllCoroutines();
			StartCoroutine(FullyOpen_Coroutine());
		}
	}

	[NESAction]
	public void OpenForAi()
	{
		if (m_ActualState != E_State.OpenForAI)
		{
			StopAllCoroutines();
			StartCoroutine(OpenForAi_Coroutine());
		}
	}

	[NESAction]
	public void Close()
	{
		if (m_ActualState != 0)
		{
			StopAllCoroutines();
			StartCoroutine(Close_Coroutine());
		}
	}

	[NESAction]
	private void ShowIcons(bool show)
	{
		foreach (GameObject icon in m_Icons)
		{
			icon._SetActiveRecursively(show);
		}
	}

	private IEnumerator FullyOpen_Coroutine()
	{
		if (m_ActualState == E_State.Closed && (bool)UpperMesh)
		{
			if (m_OpenSoundUpper != null)
			{
				AudioUpper.PlayOneShot(m_OpenSoundUpper);
			}
			if (m_OpenAnimUpper == m_CloseAnimUpper)
			{
				AnimationUpper[m_OpenAnimUpper.name].speed = 1f;
			}
			AnimationUpper.Play(m_OpenAnimUpper.name);
		}
		if (m_OpenSoundLower != null)
		{
			AudioLower.PlayOneShot(m_OpenSoundLower);
		}
		if (m_OpenAnimLower == m_CloseAnimLower)
		{
			AnimationLower[m_OpenAnimLower.name].speed = 1f;
		}
		AnimationLower.Play(m_OpenAnimLower.name);
		float time = AnimationLower[m_OpenAnimLower.name].length - 0.01f;
		yield return new WaitForSeconds(time / 2f);
		SetOffMeshLinks(E_State.FullyOpen);
		SetActionPoints(E_State.FullyOpen);
		yield return new WaitForSeconds(time / 2f);
		m_ActualState = E_State.FullyOpen;
	}

	private IEnumerator OpenForAi_Coroutine()
	{
		float time = 0f;
		if (m_ActualState == E_State.Closed && (bool)UpperMesh)
		{
			if (m_OpenSoundUpper != null)
			{
				AudioUpper.PlayOneShot(m_OpenSoundUpper);
			}
			if (m_OpenAnimUpper == m_CloseAnimUpper)
			{
				AnimationUpper[m_OpenAnimUpper.name].speed = 1f;
			}
			AnimationUpper.Play(m_OpenAnimUpper.name);
			time = AnimationUpper[m_OpenAnimUpper.name].length - 0.01f;
		}
		else if (m_ActualState == E_State.FullyOpen)
		{
			if (m_CloseSoundLower != null)
			{
				AudioLower.PlayOneShot(m_CloseSoundLower);
			}
			if (m_OpenAnimLower == m_CloseAnimLower)
			{
				AnimationLower[m_CloseAnimLower.name].time = AnimationLower[m_CloseAnimLower.name].length;
				AnimationLower[m_CloseAnimLower.name].speed = -1f;
			}
			AnimationLower.Play(m_CloseAnimLower.name);
			time = AnimationLower[m_CloseAnimLower.name].length - 0.01f;
		}
		yield return new WaitForSeconds(time / 2f);
		SetOffMeshLinks(E_State.OpenForAI);
		SetActionPoints(E_State.OpenForAI);
		yield return new WaitForSeconds(time / 2f);
		m_ActualState = E_State.OpenForAI;
	}

	private IEnumerator Close_Coroutine()
	{
		float time = 0f;
		if ((bool)UpperMesh)
		{
			if (m_OpenSoundUpper != null)
			{
				AudioUpper.PlayOneShot(m_CloseSoundUpper);
			}
			if (m_OpenAnimUpper == m_CloseAnimUpper)
			{
				AnimationUpper[m_CloseAnimUpper.name].time = AnimationUpper[m_CloseAnimUpper.name].length;
				AnimationUpper[m_CloseAnimUpper.name].speed = -1f;
			}
			AnimationUpper.Play(m_CloseAnimUpper.name);
			time = AnimationUpper[m_CloseAnimUpper.name].length - 0.01f;
		}
		if (m_ActualState == E_State.FullyOpen)
		{
			if (m_CloseSoundLower != null)
			{
				AudioLower.PlayOneShot(m_CloseSoundLower);
			}
			if (m_OpenAnimLower == m_CloseAnimLower)
			{
				AnimationLower[m_CloseAnimLower.name].time = AnimationLower[m_CloseAnimLower.name].length;
				AnimationLower[m_CloseAnimLower.name].speed = -1f;
			}
			AnimationLower.Play(m_CloseAnimLower.name);
			time = AnimationLower[m_CloseAnimLower.name].length - 0.01f;
		}
		SetOffMeshLinks(E_State.Closed);
		SetActionPoints(E_State.Closed);
		yield return new WaitForSeconds(time);
		m_ActualState = E_State.Closed;
	}

	private void SetOffMeshLinks(E_State state)
	{
		if (OffMeshLinks.Length != 0)
		{
			bool activated = state != E_State.Closed;
			UnityEngine.AI.OffMeshLink[] offMeshLinks = OffMeshLinks;
			foreach (UnityEngine.AI.OffMeshLink offMeshLink in offMeshLinks)
			{
				offMeshLink.activated = activated;
			}
		}
	}

	private void SetActionPoints(E_State state)
	{
		if (ActionPoints.Length != 0)
		{
			bool flag = state == E_State.OpenForAI;
			ActionPoint[] actionPoints = ActionPoints;
			foreach (ActionPoint actionPoint in actionPoints)
			{
				actionPoint.enabled = flag;
			}
		}
	}

	public virtual void CheckDataConsistency()
	{
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

	public void Enable()
	{
	}

	public void Disable()
	{
	}

	public void Reset()
	{
	}
}
