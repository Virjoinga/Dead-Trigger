using System.Collections;

namespace Prime31
{
	public static class JsonExtensions
	{
		public static string toJson(this IDictionary obj)
		{
			return Json.encode(obj);
		}
	}
}
