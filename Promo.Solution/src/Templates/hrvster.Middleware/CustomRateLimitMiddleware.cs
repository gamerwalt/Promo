using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace hrvster.Templates.Middleware
{
	public class CustomRateLimitMiddleware : ClientRateLimitMiddleware
	{
		public CustomRateLimitMiddleware(RequestDelegate next, IProcessingStrategy processingStrategy, IOptions<ClientRateLimitOptions> options, IRateLimitCounterStore counterStore, IClientPolicyStore policyStore, IRateLimitConfiguration config, ILogger<ClientRateLimitMiddleware> logger)
			: base(next, processingStrategy, options, policyStore, config, logger)
		{
		}

		public override Task ReturnQuotaExceededResponse(HttpContext httpContext, RateLimitRule rule, string retryAfter)
		{
			httpContext.Response.StatusCode = 429;
			httpContext.Response.ContentType = "application/json";

			var message = new
			{
				Message = "You have exceeded the rate limit or have tried to play too many times in a short span. Please try again later.",
				RetryAfter = retryAfter
			};

			return httpContext.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(message));
		}
	}
}
