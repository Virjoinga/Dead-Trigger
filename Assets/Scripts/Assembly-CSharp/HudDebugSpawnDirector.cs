using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Debug/Spawn monitor")]
public class HudDebugSpawnDirector : MonoBehaviour
{
	public Material Mat;

	public float StartLeft;

	public float StartDown;

	public float Width = 0.2f;

	public float Height = 0.1f;

	private List<float> Values = new List<float>();

	public int MaxValues = 100;

	private Vector3 StartPos;

	private float Step;

	public static HudDebugSpawnDirector Instance;

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		StartPos = new Vector3(StartLeft, StartDown, 0f);
		Step = Width / (float)MaxValues;
	}

	public void AddIntensity(float intensity)
	{
		if (Values.Count == MaxValues)
		{
			Values.RemoveAt(0);
		}
		Mathf.Clamp(intensity, 0f, 1f);
		Values.Add(intensity);
	}

	private void OnPostRender()
	{
		if (!Mat)
		{
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		GL.PushMatrix();
		Mat.SetPass(0);
		GL.LoadOrtho();
		DrawBorder();
		DrawGraph();
		GL.PopMatrix();
	}

	private void DrawBorder()
	{
		GL.Begin(1);
		GL.Color(Color.red);
		GL.Vertex(StartPos + new Vector3(0f, Height, 0f));
		GL.Vertex(StartPos);
		GL.Vertex(StartPos);
		GL.Vertex(StartPos + new Vector3(Width, 0f, 0f));
		GL.End();
	}

	private void DrawGraph()
	{
		GL.Begin(1);
		GL.Color(Color.red);
		int num = 0;
		foreach (float value in Values)
		{
			float num2 = value;
			num++;
			if (num2 > 0.8f)
			{
				GL.Color(Color.red);
			}
			else if ((double)num2 < 0.2)
			{
				GL.Color(Color.green);
			}
			else
			{
				GL.Color(Color.white);
			}
			GL.Vertex(StartPos + new Vector3(Step * (float)num, 0f, 0f));
			GL.Vertex(StartPos + new Vector3(Step * (float)num, num2 * Height, 0f));
		}
		GL.End();
	}
}
