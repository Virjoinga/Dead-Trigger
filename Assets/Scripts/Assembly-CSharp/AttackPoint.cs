using System;
using UnityEngine;

[AddComponentMenu("Entities/Attack Point")]
public class AttackPoint : MonoBehaviour
{
	[NonSerialized]
	public Agent RegisteredAgent;

	[NonSerialized]
	public Transform Transform;

	private void Awake()
	{
		Transform = base.transform;
	}
}
