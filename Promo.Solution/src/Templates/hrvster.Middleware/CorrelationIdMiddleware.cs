using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace hrvster.Templates.Middleware
{
	public class CorrelationIdMiddleware
	{
		private const string CorrelationIdHeaderName = "X-Correlation-ID";
		private readonly RequestDelegate _next;

		public CorrelationIdMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, ICorrelationContextAccessor correlationContext)
		{
			// Check if the incoming request has a Correlation ID
			if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId))
			{
				// If not, create a new Correlation ID
				correlationId = Guid.NewGuid().ToString();
			}

			// Set the Correlation ID to the HttpContext, so you can use it later in the request
			context.Items["CorrelationId"] = correlationId;

			// Append the Correlation ID to the response header
			context.Response.OnStarting(() =>
			{
				context.Response.Headers.TryAdd(CorrelationIdHeaderName, new[] { (string)correlationId });
				return Task.CompletedTask;
			});

			// Enrich the log context with the Correlation ID
			using (LogContext.PushProperty("CorrelationId", correlationId))
			{
				correlationContext.CorrelationId = Guid.Parse(correlationId.ToString());

				// Continue processing
				await _next(context);
			}
		}
	}
}
