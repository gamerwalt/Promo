using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Http;


namespace hrvster.Middleware
{
	public class SubdomainMiddleware
	{
		private readonly RequestDelegate _next;

		public SubdomainMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string? subdomain;

			if (context.Request.Host.Host.Equals("localhost"))
			{
				subdomain = "mtn";
			}
			else
			{
				var host = context.Request.Host.Host;
				var index = host.IndexOf(".");
				subdomain = (index < 0) ? null : host.Substring(0, index);
			}

			context.Items[Constants.ContextItemsSubdomain] = subdomain;

			await _next(context);
		}
	}
}