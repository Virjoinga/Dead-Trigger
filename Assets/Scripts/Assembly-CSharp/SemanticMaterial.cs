using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SemanticMaterial
{
	[Serializable]
	public class ProjectileImpact
	{
		public List<AudioClip> m_Sfx;

		public GameObject m_Gfx;

		internal ResourceCache m_GfxCache;

		public int m_ExpectedNum = 8;
	}

	[SerializeField]
	private E_SemanticMaterialID m_ID;

	[SerializeField]
	public PhysicMaterial m_PhyMaterial;

	[SerializeField]
	public ProjectileImpact m_ProjectileImpact = new ProjectileImpact();

	[SerializeField]
	public List<AudioClip> m_GrenadeImpacts = new List<AudioClip>();

	[SerializeField]
	public AudioClip m_GadgetsSticking;

	[SerializeField]
	public List<AudioClip> m_PlayerFootsteps = new List<AudioClip>();

	public E_SemanticMaterialID ID
	{
		get
		{
			return m_ID;
		}
	}

	public SemanticMaterial(E_SemanticMaterialID MatID)
	{
		m_ID = MatID;
		m_ProjectileImpact.m_Sfx = new List<AudioClip>();
		m_ProjectileImpact.m_Sfx.Add(null);
		m_GrenadeImpacts.Add(null);
		m_PlayerFootsteps.Add(null);
	}

	public ProjectileImpact GetProjectileImpactData(E_ProjectileType ProjType)
	{
		return m_ProjectileImpact;
	}
}
