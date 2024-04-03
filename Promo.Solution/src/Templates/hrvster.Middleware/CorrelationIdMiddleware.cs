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
			string correlationId;

			// Check if the incoming request has a Correlation ID
			if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var headerCorrelationId))
			{
				correlationId = Guid.NewGuid().ToString();
			}
			else
			{
				correlationId = headerCorrelationId;
			}

			// Set or update the Correlation ID in HttpContext.Items
			context.Items["CorrelationId"] = correlationId;

			// Append or update the Correlation ID in the response header
			context.Response.OnStarting(() =>
			{
				context.Response.Headers[CorrelationIdHeaderName] = correlationId;
				return Task.CompletedTask;
			});

			// Check if LogContext already contains a CorrelationId
			if (context.Items.ContainsKey("LogContextCorrelationIdSet"))
			{
				// The CorrelationId is already set in LogContext for this request
			}
			else
			{
				// Mark that we're adding the CorrelationId to the LogContext
				context.Items["LogContextCorrelationIdSet"] = true;

				// Enrich the log context with the Correlation ID
				using (LogContext.PushProperty("CorrelationId", correlationId))
				{
					correlationContext.CorrelationId = Guid.Parse(correlationId);

					// Continue processing
					await _next(context);
				}
			}
		}

	}
}
