using System;

namespace hrvster.Models.Extensions
{
	public static class UrlExtensions
	{
		public static Uri GenerateUrl(this Uri uri, string urlType)
		{
			return new Uri($"{uri}{urlType}");
		}
	}
}
