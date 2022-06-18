using Newtonsoft.Json;

namespace hrvster.Extensions
{
	public static class ObjectExtensions
	{
		public static string ToJsonString<T>(this T obj) where T : class
		{
			return JsonConvert.SerializeObject(obj);
		}
	}
}
