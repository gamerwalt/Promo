using hrvster.ApplicationServices.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace hrvster.Exceptions
{
	public class CustomExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomExceptionMiddleware> _logger;

		public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (NoMoreBoardForThisGameException ex)
			{
				_logger.LogError($"Caught exception: {ex.Message}");

				context.Response.Redirect("/Error/NoMoreBoardForThisGame");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong: {ex.Message}");

				context.Response.Redirect("/Error/500");
			}
		}
	}

}
