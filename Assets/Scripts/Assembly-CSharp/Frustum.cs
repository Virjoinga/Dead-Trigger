#define DEBUG
using System;
using UnityEngine;

public static class Frustum
{
	private enum E_ProjMode
	{
		Perspective = 0,
		Orthographic = 1
	}

	private enum E_ClipPlane
	{
		Right = 0,
		Left = 1,
		Top = 2,
		Bottom = 3,
		Near = 4,
		Far = 5
	}

	private static Vector3 m_Pos;

	private static Vector3 m_Dir;

	private static Vector3 m_Up;

	private static Vector3 m_Right;

	private static float m_Near;

	private static float m_Far;

	private static float m_Ratio;

	private static float m_FOV;

	private static float m_InvCosX;

	private static float m_InvCosY;

	private static float m_TanY;

	private static float m_OrthoSize;

	private static E_ProjMode m_ProjMode;

	private static Plane[] m_ClipPlanes = new Plane[6];

	private static bool m_ClipPlanesValid;

	public static bool UpdateClippingPlanes { get; set; }

	public static void Setup(Camera Cam, float Far = -1f)
	{
		Matrix4x4 localToWorldMatrix = Cam.transform.localToWorldMatrix;
		m_Pos = Matrix.GetOrigin(localToWorldMatrix);
		m_Dir = Matrix.GetAxis(localToWorldMatrix, 2);
		m_Up = Matrix.GetAxis(localToWorldMatrix, 1);
		m_Right = Matrix.GetAxis(localToWorldMatrix, 0);
		m_Near = Cam.nearClipPlane;
		m_Far = ((!(Far > m_Near)) ? Cam.farClipPlane : Far);
		m_FOV = Cam.fieldOfView;
		m_Ratio = Cam.aspect;
		if (Cam.orthographic)
		{
			m_OrthoSize = Cam.orthographicSize;
			m_ProjMode = E_ProjMode.Orthographic;
		}
		else
		{
			m_OrthoSize = 0f;
			m_ProjMode = E_ProjMode.Perspective;
			float f = (float)Math.PI / 180f * m_FOV * 0.5f;
			m_TanY = Mathf.Tan(f);
		}
		m_ClipPlanesValid = false;
		if (UpdateClippingPlanes)
		{
			SetupClippingPlanes(Cam.projectionMatrix * Cam.worldToCameraMatrix);
		}
	}

	public static void SetupPersp(Vector3 Pos, Vector3 Dir, Vector3 Up, Vector3 Right, float Near, float Far, float Ratio, float FOV)
	{
		m_Pos = Pos;
		m_Dir = Dir;
		m_Up = Up;
		m_Right = Right;
		m_Near = Near;
		m_Far = Far;
		m_FOV = FOV;
		m_Ratio = Ratio;
		m_OrthoSize = 0f;
		m_ProjMode = E_ProjMode.Perspective;
		float f = (float)Math.PI / 180f * m_FOV * 0.5f;
		m_TanY = Mathf.Tan(f);
		m_ClipPlanesValid = false;
		if (UpdateClippingPlanes)
		{
			Matrix4x4 matrix4x = Matrix4x4.Perspective(m_FOV, m_Ratio, m_Near, m_Far);
			Matrix4x4 inverse = Matrix.Create(m_Pos, m_Right, m_Up, -m_Dir).inverse;
			SetupClippingPlanes(matrix4x * inverse);
		}
	}

	private static void SetupClippingPlanes(Matrix4x4 WorldToProjection)
	{
		m_ClipPlanesValid = true;
		Vector3 vector = default(Vector3);
		vector.x = WorldToProjection.m30 - WorldToProjection.m00;
		vector.y = WorldToProjection.m31 - WorldToProjection.m01;
		vector.z = WorldToProjection.m32 - WorldToProjection.m02;
		float num = WorldToProjection.m33 - WorldToProjection.m03;
		float num2 = 1f / vector.magnitude;
		m_ClipPlanes[0].normal = vector * num2;
		m_ClipPlanes[0].distance = num * num2;
		vector.x = WorldToProjection.m30 + WorldToProjection.m00;
		vector.y = WorldToProjection.m31 + WorldToProjection.m01;
		vector.z = WorldToProjection.m32 + WorldToProjection.m02;
		num = WorldToProjection.m33 + WorldToProjection.m03;
		num2 = 1f / vector.magnitude;
		m_ClipPlanes[1].normal = vector * num2;
		m_ClipPlanes[1].distance = num * num2;
		vector.x = WorldToProjection.m30 - WorldToProjection.m10;
		vector.y = WorldToProjection.m31 - WorldToProjection.m11;
		vector.z = WorldToProjection.m32 - WorldToProjection.m12;
		num = WorldToProjection.m33 - WorldToProjection.m13;
		num2 = 1f / vector.magnitude;
		m_ClipPlanes[2].normal = vector * num2;
		m_ClipPlanes[2].distance = num * num2;
		vector.x = WorldToProjection.m30 + WorldToProjection.m10;
		vector.y = WorldToProjection.m31 + WorldToProjection.m11;
		vector.z = WorldToProjection.m32 + WorldToProjection.m12;
		num = WorldToProjection.m33 + WorldToProjection.m13;
		num2 = 1f / vector.magnitude;
		m_ClipPlanes[3].normal = vector * num2;
		m_ClipPlanes[3].distance = num * num2;
		vector.x = WorldToProjection.m30 - WorldToProjection.m20;
		vector.y = WorldToProjection.m31 - WorldToProjection.m21;
		vector.z = WorldToProjection.m32 - WorldToProjection.m22;
		num = WorldToProjection.m33 - WorldToProjection.m23;
		num2 = 1f / vector.magnitude;
		m_ClipPlanes[4].normal = vector * num2;
		m_ClipPlanes[4].distance = num * num2;
		vector.x = WorldToProjection.m30 + WorldToProjection.m20;
		vector.y = WorldToProjection.m31 + WorldToProjection.m21;
		vector.z = WorldToProjection.m32 + WorldToProjection.m22;
		num = WorldToProjection.m33 + WorldToProjection.m23;
		num2 = 1f / vector.magnitude;
		m_ClipPlanes[5].normal = vector * num2;
		m_ClipPlanes[5].distance = num * num2;
	}

	public static bool IsInside(Vector3 Point)
	{
		Vector3 lhs = Point - m_Pos;
		float num = Vector3.Dot(lhs, m_Dir);
		if (num < m_Near || num > m_Far)
		{
			return false;
		}
		float num2 = ((m_ProjMode != 0) ? m_OrthoSize : (num * m_TanY));
		float num3 = Vector3.Dot(lhs, m_Up);
		if (num3 < 0f - num2 || num3 >= num2)
		{
			return false;
		}
		num2 *= m_Ratio;
		float num4 = Vector3.Dot(lhs, m_Right);
		return num4 >= 0f - num2 && num4 <= num2;
	}

	public static bool IsInside(Vector3 BoxMin, Vector3 BoxMax)
	{
		DebugUtils.Assert(m_ClipPlanesValid);
		Vector3 rhs = default(Vector3);
		for (int i = 0; i < 6; i++)
		{
			Vector3 normal = m_ClipPlanes[i].normal;
			float distance = m_ClipPlanes[i].distance;
			rhs.x = ((!(normal.x < 0f)) ? BoxMax.x : BoxMin.x);
			rhs.y = ((!(normal.y < 0f)) ? BoxMax.y : BoxMin.y);
			rhs.z = ((!(normal.z < 0f)) ? BoxMax.z : BoxMin.z);
			float num = Vector3.Dot(normal, rhs) + distance;
			if (num < 0f)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsInside(Vector3 SphereCenter, float SphereRadius)
	{
		DebugUtils.Assert(m_ClipPlanesValid);
		for (int i = 0; i < 6; i++)
		{
			float distanceToPoint = m_ClipPlanes[i].GetDistanceToPoint(SphereCenter);
			if (distanceToPoint < 0f - SphereRadius)
			{
				return false;
			}
		}
		return true;
	}

	public static void Draw(Color Col)
	{
	}
}
