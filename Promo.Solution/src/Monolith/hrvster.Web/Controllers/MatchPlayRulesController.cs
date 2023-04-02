using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchPlayRulesController : Controller
	{
		private readonly ILogger<MatchPlayRulesController> _logger;
		private readonly IMatchPlayRulesService _service;
		private readonly IMatchGamesService _matchGameService;
		private readonly IToastNotification _notificationService;

		public MatchPlayRulesController(
			ILoggerFactory loggerFactory,
			IMatchPlayRulesService service,
			IMatchGamesService matchGamesService,
			IToastNotification notificationService)
		{
			_logger = loggerFactory.CreateLogger<MatchPlayRulesController>();
			_service = service;
			_matchGameService = matchGamesService;
			_notificationService = notificationService;
		}

		[HttpGet]
		[Route("/matchgame/{Uid:Guid}/play/rules/create")]
		public async Task<IActionResult> Create(
			Guid Uid, string controllerName, string actionName, CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);

				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var model = new CreateMatchPlayRule
			{
				GameName = matchGame.Name,
				GameUid = matchGame.Uid,
				ControllerName = controllerName,
				ActionName = actionName,
				PlayPerUser = 1,
				PlayPerTweet = 1
			};

			return View(model);
		}

		[HttpPost]
		[Route("/matchgame/{Uid:Guid}/play/rules/create")]
		public async Task<IActionResult> Create(
			Guid Uid, string controllerName, string actionName, CreateMatchPlayRule form, CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);

				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			if(ModelState.IsValid)
			{
				await _service.CreateAsync(Uid, form.PlayPerUser, form.PlayPerTweet, cancellationToken);

				_notificationService.AddSuccessToastMessage("Play rules created successfully.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View(form);
		}
	}
}
