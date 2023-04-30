using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchRulesController : Controller
	{
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchRulesService _matchRuleService;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public MatchRulesController(
			IMatchGamesService matchGamesService,
			IMatchRulesService matchRulesService,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_matchGamesService = matchGamesService;
			_matchRuleService = matchRulesService;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}

		[HttpGet]
		[Route("/MatchRules/{ruleUid:Guid}/game/{gameUid:Guid}/delete")]
		public async Task<IActionResult> Delete(
			Guid gameUid, 
			Guid ruleUid, 
			string actionName, 
			string controllerName, 
			CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var game = await _matchGamesService.GetMatchGameAsync(gameUid, cancellationToken);
			if(game == null)
			{
				_notificationService.AddErrorToastMessage("Unable to locate game.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "Dashboard", new
				{
					Uid = gameUid
				});
			}

			var rule = await _matchRuleService.GetRuleAsync(ruleUid, cancellationToken);
			if(rule == null)
			{
				_notificationService.AddErrorToastMessage("Unable to locate rule.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "Dashboard", new
				{
					Uid = gameUid
				});
			}

			//Todo: Check if gameplay exists for this rule

			var model = new ViewMatchRule
			{
				ExternalId = rule.ExternalId,
				ExternalClient = rule.ExternalClientType.ToString(),
				GameName = rule.Game.Name,
				Hashtag = rule.WatchValue,
				ReturnAction = actionName,
				ReturnController = controllerName,
				DeleteMatchRule = new DeleteMatchRule
				{
					RuleUid = ruleUid,
					GameUid = gameUid,
					ExternalClientType = rule.ExternalClientType.ToString(),
					ExternalId = rule.ExternalId
				}
			};

			return View(model);
		}

		[HttpPost]
		[Route("/MatchRules/{ruleUid:Guid}/game/{gameUid:Guid}/delete")]
		public async Task<IActionResult> Delete(
			Guid gameUid,
			Guid ruleUid,
			string actionName,
			string controllerName,
			ViewMatchRule form,
			CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var game = await _matchGamesService.GetMatchGameAsync(gameUid, cancellationToken);
			if (game == null)
			{
				_notificationService.AddErrorToastMessage("Unable to locate game.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "Dashboard", new
				{
					Uid = gameUid
				});
			}

			var rule = await _matchRuleService.GetRuleAsync(ruleUid, cancellationToken);
			if (rule == null)
			{
				_notificationService.AddErrorToastMessage("Unable to locate rule.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "Dashboard", new
				{
					Uid = gameUid
				});
			}

			//Todo: Check if gameplay exists for this rule

			if (ModelState.IsValid)
			{
				await _matchGamesService.DeleteGameRuleAsync(
					form.DeleteMatchRule.RuleUid, 
					form.DeleteMatchRule.GameUid, 
					_hrvsterUser.UserUid,
					Enum.Parse<ExternalClientType>(form.DeleteMatchRule.ExternalClientType), 
					form.DeleteMatchRule.ExternalId,
					cancellationToken);

				_notificationService.AddSuccessToastMessage("Rule and post deleted successfully.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "Dashboard", new
				{
					uid = gameUid
				});
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("Delete", new
			{
				ruleUid = ruleUid,
				gameUid = gameUid,
				actionName = actionName,
				controllerName = controllerName
			});
		}
	}
}
