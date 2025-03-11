using UnityEngine;

public class ShapeBox : Shape
{
	private Vector3 m_Center;

	private Vector3 m_Extents;

	private Vector3 m_ExtentsScaled;

	private float m_Scale = 1f;

	private Vector3[] m_Axis = new Vector3[6];

	public ShapeBox()
	{
	}

	public ShapeBox(Vector3 Extents)
	{
		m_Extents = Extents;
	}

	public void SetPos(Vector3 Pos)
	{
		m_Center = Pos;
	}

	public void SetRot(Quaternion Rot)
	{
		Matrix4x4 mat = Matrix.Create(Rot);
		Vector3 axisX = Matrix.GetAxisX(mat);
		Vector3 axisY = Matrix.GetAxisY(mat);
		Vector3 axisZ = Matrix.GetAxisZ(mat);
		m_Axis[0] = axisX;
		m_Axis[1] = axisY;
		m_Axis[2] = axisZ;
		m_Axis[3] = -axisX;
		m_Axis[4] = -axisY;
		m_Axis[5] = -axisZ;
	}

	public void SetFrame(Matrix4x4 Frame)
	{
		Vector3 axisX = Matrix.GetAxisX(Frame);
		Vector3 axisY = Matrix.GetAxisY(Frame);
		Vector3 axisZ = Matrix.GetAxisZ(Frame);
		m_Axis[0] = axisX;
		m_Axis[1] = axisY;
		m_Axis[2] = axisZ;
		m_Axis[3] = -axisX;
		m_Axis[4] = -axisY;
		m_Axis[5] = -axisZ;
		m_Center = Matrix.GetOrigin(Frame);
	}

	public void SetExtents(Vector3 Extents)
	{
		m_Extents = Extents;
		m_ExtentsScaled = Extents * m_Scale;
	}

	public void SetScale(float Scale)
	{
		m_Scale = Scale;
		m_ExtentsScaled = m_Extents * m_Scale;
	}

	public override int RayCast(Vector3 Origin, Vector3 Direction, HitInfo[] Hits)
	{
		Vector3 rhs = Origin - m_Center;
		float num = Vector3.Dot(m_Axis[0], rhs);
		float num2 = Vector3.Dot(m_Axis[0], Direction);
		float num3 = 1f / num2;
		float num4;
		int num5;
		float num6;
		int num7;
		if (num3 >= 0f)
		{
			num4 = (0f - m_ExtentsScaled.x - num) * num3;
			num5 = 3;
			num6 = (m_ExtentsScaled.x - num) * num3;
			num7 = 0;
		}
		else
		{
			num4 = (m_ExtentsScaled.x - num) * num3;
			num5 = 0;
			num6 = (0f - m_ExtentsScaled.x - num) * num3;
			num7 = 3;
		}
		num = Vector3.Dot(m_Axis[1], rhs);
		num2 = Vector3.Dot(m_Axis[1], Direction);
		num3 = 1f / num2;
		float num8;
		int num9;
		float num10;
		int num11;
		if (num3 >= 0f)
		{
			num8 = (0f - m_ExtentsScaled.y - num) * num3;
			num9 = 4;
			num10 = (m_ExtentsScaled.y - num) * num3;
			num11 = 1;
		}
		else
		{
			num8 = (m_ExtentsScaled.y - num) * num3;
			num9 = 1;
			num10 = (0f - m_ExtentsScaled.y - num) * num3;
			num11 = 4;
		}
		if (num6 < num8 || num4 > num10)
		{
			return 0;
		}
		if (num8 > num4)
		{
			num4 = num8;
			num5 = num9;
		}
		if (num10 < num6)
		{
			num6 = num10;
			num7 = num11;
		}
		num = Vector3.Dot(m_Axis[2], rhs);
		num2 = Vector3.Dot(m_Axis[2], Direction);
		num3 = 1f / num2;
		if (num3 >= 0f)
		{
			num8 = (0f - m_ExtentsScaled.z - num) * num3;
			num9 = 5;
			num10 = (m_ExtentsScaled.z - num) * num3;
			num11 = 2;
		}
		else
		{
			num8 = (m_ExtentsScaled.z - num) * num3;
			num9 = 2;
			num10 = (0f - m_ExtentsScaled.z - num) * num3;
			num11 = 5;
		}
		if (num6 < num8 || num4 > num10)
		{
			return 0;
		}
		if (num8 > num4)
		{
			num4 = num8;
			num5 = num9;
		}
		if (num10 < num6)
		{
			num6 = num10;
			num7 = num11;
		}
		int num12 = 0;
		if (num4 >= 0f)
		{
			Hits[num12].m_Point = Origin + Direction * num4;
			Hits[num12].m_Normal = m_Axis[num5];
			Hits[num12++].m_Time = num4;
		}
		if (num6 >= 0f)
		{
			Hits[num12].m_Point = Origin + Direction * num6;
			Hits[num12].m_Normal = m_Axis[num7];
			Hits[num12++].m_Time = num6;
		}
		return num12;
	}

	public override int SphereCast(Vector3 Origin, Vector3 Direction, float Radius, HitInfo[] Hits)
	{
		Radius *= 0.9f;
		m_ExtentsScaled.x += Radius;
		m_ExtentsScaled.y += Radius;
		m_ExtentsScaled.z += Radius;
		int num = RayCast(Origin, Direction, Hits);
		m_ExtentsScaled.x -= Radius;
		m_ExtentsScaled.y -= Radius;
		m_ExtentsScaled.z -= Radius;
		for (int i = 0; i < num; i++)
		{
			SnapToBox(ref Hits[i]);
		}
		return num;
	}

	private void SnapToBox(ref HitInfo Hit)
	{
		Vector3 point = Hit.m_Point;
		Vector3 rhs = point - m_Center;
		float num = Vector3.Dot(m_Axis[0], rhs);
		if (num > m_ExtentsScaled.x)
		{
			num -= m_ExtentsScaled.x;
			point -= num * m_Axis[0];
		}
		else if (num < 0f - m_ExtentsScaled.x)
		{
			num += m_ExtentsScaled.x;
			point -= num * m_Axis[0];
		}
		num = Vector3.Dot(m_Axis[1], rhs);
		if (num > m_ExtentsScaled.y)
		{
			num -= m_ExtentsScaled.y;
			point -= num * m_Axis[1];
		}
		else if (num < 0f - m_ExtentsScaled.y)
		{
			num += m_ExtentsScaled.y;
			point -= num * m_Axis[1];
		}
		num = Vector3.Dot(m_Axis[2], rhs);
		if (num > m_ExtentsScaled.z)
		{
			num -= m_ExtentsScaled.z;
			point -= num * m_Axis[2];
		}
		else if (num < 0f - m_ExtentsScaled.z)
		{
			num += m_ExtentsScaled.z;
			point -= num * m_Axis[2];
		}
		Hit.m_Point = point;
	}

	public override void UpdateBBox()
	{
		Vector3 vector = m_ExtentsScaled.x * m_Axis[0];
		Vector3 vector2 = m_ExtentsScaled.y * m_Axis[1];
		Vector3 vector3 = m_ExtentsScaled.z * m_Axis[2];
		float num = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
		float num2 = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
		float num3 = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
		m_BBox[0] = m_Center.x - num;
		m_BBox[1] = m_Center.y - num2;
		m_BBox[2] = m_Center.z - num3;
		m_BBox[3] = m_Center.x + num;
		m_BBox[4] = m_Center.y + num2;
		m_BBox[5] = m_Center.z + num3;
	}

	public void Draw(Color Col)
	{
		Matrix4x4 local2World = Matrix.Create(m_Center, m_Axis[0], m_Axis[1], m_Axis[2]);
		DebugDraw.Box(Col, 2f * m_ExtentsScaled, local2World);
	}

	public void Draw(Color Col, Color BoundsCol)
	{
		DebugDraw.Box(BoundsCol, new Vector3(m_BBox[0], m_BBox[1], m_BBox[2]), new Vector3(m_BBox[3], m_BBox[4], m_BBox[5]));
		Draw(Col);
	}
}
