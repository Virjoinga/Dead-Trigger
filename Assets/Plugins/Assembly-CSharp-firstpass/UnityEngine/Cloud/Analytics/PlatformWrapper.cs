namespace UnityEngine.Cloud.Analytics
{
	internal static class PlatformWrapper
	{
		public static IPlatformWrapper platform
		{
			get
			{
				return new AndroidWrapper();
			}
		}
	}
}
