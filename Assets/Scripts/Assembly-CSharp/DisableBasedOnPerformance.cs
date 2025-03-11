using UnityEngine;

[AddComponentMenu("Optimizations/Disable Based On Performance")]
public class DisableBasedOnPerformance : MonoBehaviour
{
	public DeviceInfo.Performance UseForDevicePerformance = DeviceInfo.Performance.Medium;

	private void Start()
	{
		switch (UseForDevicePerformance)
		{
		case DeviceInfo.Performance.Medium:
			if (DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Low)
			{
				base.gameObject.SetActive(false);
			}
			break;
		case DeviceInfo.Performance.High:
			if (DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Low || DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Medium)
			{
				base.gameObject.SetActive(false);
			}
			break;
		case DeviceInfo.Performance.UltraHigh:
			if (DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Low || DeviceInfo.PerformanceGrade == DeviceInfo.Performance.Medium || DeviceInfo.PerformanceGrade == DeviceInfo.Performance.High)
			{
				base.gameObject.SetActive(false);
			}
			break;
		}
	}
}
