using Newtonsoft.Json;
using System;

namespace hrvster.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
		public static String AsJson(this object o) => JsonConvert.SerializeObject(o);
	}
}
