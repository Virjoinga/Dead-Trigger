using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Effects/ReflectionCamera")]
public class ReflectionCam : MonoBehaviour
{
	public float m_ReflectionPlaneHeightOffs;

	public float m_ClipPlaneOffset = 0.05f;

	private void Awake()
	{
	}

	private void UpdateReflCamPos(Camera reflCam, Camera viewCam)
	{
		Vector3 position = viewCam.transform.position;
		Vector3 vector = new Vector3(0f, 1f, 0f);
		Vector3 vector2 = position + new Vector3(0f, m_ReflectionPlaneHeightOffs, 0f);
		Plane plane = new Plane(vector, vector2);
		Matrix4x4 matrix4x = CalcPlanarReflMatrix(new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance));
		reflCam.worldToCameraMatrix = viewCam.worldToCameraMatrix * matrix4x;
		reflCam.ResetProjectionMatrix();
		Vector4 clipPlane = CameraSpacePlane(reflCam, vector2, vector, 1f);
		Matrix4x4 projection = reflCam.projectionMatrix;
		CalculateObliqueMatrix(ref projection, clipPlane);
		reflCam.projectionMatrix = projection;
	}

	private static Matrix4x4 CalcPlanarReflMatrix(Vector4 p)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		int row;
		int row2 = (row = 0);
		int column;
		int column2 = (column = 0);
		float num = identity[row, column];
		identity[row2, column2] = num - 2f * p[0] * p[0];
		int row3 = (column = 1);
		int column3 = (row = 0);
		num = identity[column, row];
		identity[row3, column3] = num - 2f * p[1] * p[0];
		int row4 = (row = 2);
		int column4 = (column = 0);
		num = identity[row, column];
		identity[row4, column4] = num - 2f * p[2] * p[0];
		int row5 = (column = 3);
		int column5 = (row = 0);
		num = identity[column, row];
		identity[row5, column5] = num - 2f * p[3] * p[0];
		int row6 = (row = 0);
		int column6 = (column = 1);
		num = identity[row, column];
		identity[row6, column6] = num - 2f * p[0] * p[1];
		int row7 = (column = 1);
		int column7 = (row = 1);
		num = identity[column, row];
		identity[row7, column7] = num - 2f * p[1] * p[1];
		int row8 = (row = 2);
		int column8 = (column = 1);
		num = identity[row, column];
		identity[row8, column8] = num - 2f * p[2] * p[1];
		int row9 = (column = 3);
		int column9 = (row = 1);
		num = identity[column, row];
		identity[row9, column9] = num - 2f * p[3] * p[1];
		int row10 = (row = 0);
		int column10 = (column = 2);
		num = identity[row, column];
		identity[row10, column10] = num - 2f * p[0] * p[2];
		int row11 = (column = 1);
		int column11 = (row = 2);
		num = identity[column, row];
		identity[row11, column11] = num - 2f * p[1] * p[2];
		int row12 = (row = 2);
		int column12 = (column = 2);
		num = identity[row, column];
		identity[row12, column12] = num - 2f * p[2] * p[2];
		int row13 = (column = 3);
		int column13 = (row = 2);
		num = identity[column, row];
		identity[row13, column13] = num - 2f * p[3] * p[2];
		return identity.transpose;
	}

	private static float sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Vector3 v = pos + normal * m_ClipPlaneOffset;
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(v);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, 0f - Vector3.Dot(lhs, rhs));
	}

	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(sgn(clipPlane.x), sgn(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}

	private void OnPreRender()
	{
		GL.SetRevertBackfacing(true);
	}

	private void OnPostRender()
	{
		GL.SetRevertBackfacing(false);
	}

	private void OnRenderObject()
	{
		if (Application.isPlaying)
		{
			if ((bool)Camera.main)
			{
				Camera camera = base.gameObject.GetComponent<Camera>();
				UpdateReflCamPos(camera, Camera.main);
				Shader.SetGlobalMatrix("_GlobalReflViewProjTM", camera.projectionMatrix * camera.worldToCameraMatrix);
			}
		}
		else if ((bool)Camera.current)
		{
			Camera camera2 = base.gameObject.GetComponent<Camera>();
			UpdateReflCamPos(camera2, Camera.current);
			Shader.SetGlobalMatrix("_GlobalReflViewProjTM", camera2.projectionMatrix * camera2.worldToCameraMatrix);
		}
	}
}
