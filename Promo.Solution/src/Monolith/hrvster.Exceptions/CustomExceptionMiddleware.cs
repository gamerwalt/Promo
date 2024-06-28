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
			catch(GameHasNotYetStartedException ex)
			{
				_logger.LogError($"Caught exception: {ex.Message}");

				context.Response.Redirect("/Error/GameHasNotYetStarted");
			}
			catch(GameHasEndedException ex)
			{
				_logger.LogError($"Caught exception: {ex.Message}");

				context.Response.Redirect("/Error/GameHasEnded");
			}
			catch(GameNotFoundException ex)
			{
				_logger.LogError("Caught a game not found exception.");

				context.Response.Redirect("/Error/GameNotFound");
			}
			catch(GameBoardsNotGeneratedException ex)
			{
				_logger.LogError("The gameboards have not been generated for this game.");

				context.Response.Redirect("/Error/GameBoardsNotGenerated");
			}
			catch(CampaignNotFoundException ex)
			{
				_logger.LogError("The campaign could not be found.");

				context.Response.Redirect("/Error/CampaignNotFound");
			}
			catch(UserHasExceededNumberOfPlaysException ex)
			{
				_logger.LogError("This user has played too much.");

				context.Response.Redirect("/Error/PlayedTooMuch");
			}
			catch(UserIsBlacklisted ex)
			{
				_logger.LogError("The user is blacklisted.");

				context.Response.Redirect("/Error/UserIsBlacklisted");
			}
			catch(GameBoardNoMorePlayableException ex)
			{
				_logger.LogError("Gameboard no more playable.");

				context.Response.Redirect("/Error/GameboardNoMorePlayable");
			}
			catch(BadGameBoardForGameException ex)
			{
				_logger.LogError("The gameboard selected does not belong to the game.");

				context.Response.Redirect("/Error/WrongGaemboardForGame");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong: {ex.Message}");
				_logger.LogError($"Stack Trace: {ex.StackTrace}");

				context.Response.Redirect("/Error/500");
			}
		}
	}

}
