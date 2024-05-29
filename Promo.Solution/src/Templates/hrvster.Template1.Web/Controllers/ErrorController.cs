using hrvster.Template1.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Controllers
{
	public class ErrorController : Controller
	{
		private readonly ILogger<ErrorController> _logger;

		public ErrorController(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ErrorController>();
		}

		[HttpGet]
		public IActionResult NoMoreBoardForThisGame()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GameHasNotYetStarted()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GameHasEnded()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GameNotFound()
		{
			return View();
		}

		[HttpGet]
		public IActionResult GameBoardsNotGenerated()
		{
			return View();
		}

		[HttpGet]
		public IActionResult CampaignNotFound()
		{
			return View();
		}

		[HttpGet("HandleErrorCode")]
		public async Task<IActionResult> HandleErrorCode(string reason, int statusCode)
		{
			var errorMessage = string.Empty;

			switch (statusCode)
			{
				case 400:
					errorMessage = "Bad request";
					break;
				case 404:
					errorMessage = "Resource not found";
					break;
				case 500:
					errorMessage = "An error occurred on the server";
					break;
				default:
					errorMessage = "An unknown error occurred";
					break;
			}

			_logger.LogError("An error occurred. StatusCode: {statusCode}. Error Message: {ErrorMessage}, Reason: {Reason}",
				statusCode,
				errorMessage,
				reason);

			var referer = HttpContext.Request.Headers["Referer"].ToString();

			if (!string.IsNullOrEmpty(referer))
			{
				return View(statusCode);
			}

			if (statusCode == 0 && string.IsNullOrWhiteSpace(referer))
				return View("Exception");

			var model = new ErrorReason
			{
				Message = reason
			};

			return View(statusCode.ToString(), model);
		}

		[Route("Error")]
		public IActionResult HandleException()
		{
			var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (exceptionFeature != null)
			{
				var exceptionType = exceptionFeature.Error.GetType().Name;
				var exceptionMessage = exceptionFeature.Error.Message;

				_logger.LogError("An exception occurred. Exception: {nameOfException}. Message: {message}",
					exceptionType,
					exceptionMessage);
			}

			return View("Exception");
		}
	}
}
