using System.IO;
using UnityEngine;

public class ScreenShotMovie : MonoBehaviour
{
	public string m_FolderName = "ScreenshotFolder";

	public int m_FrameRate = 25;

	public Camera m_CaptureCamera;

	public int m_SuperSize;

	private string m_RealFolder = string.Empty;

	private bool m_Capture;

	private Animation[] m_AnimTargets;

	private void Start()
	{
		Time.captureFramerate = m_FrameRate;
		m_RealFolder = m_FolderName;
		int num = 1;
		while (Directory.Exists(m_RealFolder))
		{
			m_RealFolder = m_FolderName + num;
			num++;
		}
		Directory.CreateDirectory(m_RealFolder);
		m_AnimTargets = GetComponentsInChildren<Animation>();
		if (m_AnimTargets == null || m_AnimTargets.Length <= 0)
		{
			Debug.LogError("Can't find game objects with animation component for movie capture !!!");
		}
		else
		{
			Animation[] animTargets = m_AnimTargets;
			foreach (Animation animation in animTargets)
			{
				animation.playAutomatically = false;
				animation.Stop();
			}
		}
		if (m_CaptureCamera == null)
		{
			m_CaptureCamera = GetComponentInChildren<Camera>();
		}
		if (m_CaptureCamera == null)
		{
			Debug.LogWarning("Can't find camera for movie capture !!!");
		}
		else if (m_CaptureCamera.GetComponent<Animation>() == null)
		{
			Debug.LogWarning("movie capture camera dosn't have animation component assigned !!!");
			m_CaptureCamera = null;
		}
		else if (m_CaptureCamera.GetComponent<Animation>().clip == null)
		{
			Debug.LogWarning("movie capture camera animation component doesn't have animClip assigned !!!");
			m_CaptureCamera = null;
		}
		else
		{
			m_CaptureCamera.GetComponent<Animation>().playAutomatically = false;
			m_CaptureCamera.GetComponent<Animation>().Stop();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			m_Capture = !m_Capture;
			if (m_Capture)
			{
				StartCapture();
			}
		}
		if (m_Capture)
		{
			string filename = string.Format("{0}/{1:D04} shot.png", m_RealFolder, Time.frameCount);
			ScreenCapture.CaptureScreenshot(filename, m_SuperSize);
		}
	}

	private void OnGUI()
	{
		if (!m_Capture)
		{
			GUI.Label(new Rect(20f, 20f, 550f, 100f), "Press [SPACE] for Start/Stop capture...");
		}
	}

	private void StartCapture()
	{
		m_Capture = true;
		Animation[] animTargets = m_AnimTargets;
		foreach (Animation animation in animTargets)
		{
			animation.Play();
		}
		if (m_CaptureCamera != null)
		{
			m_CaptureCamera.GetComponent<Animation>().Play();
		}
	}
}
