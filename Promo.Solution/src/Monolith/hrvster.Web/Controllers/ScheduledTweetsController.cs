using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class ScheduledTweetsController : Controller
	{
		private readonly ILogger<ScheduledTweetsController> _logger;
		private readonly IMediator _mediator;
		private readonly INotyfService _notificationService;
		private readonly NotificationServiceOptions _notificationServiceOptions;
		private readonly HrvsterUser _hrvsterUser;

		public ScheduledTweetsController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			INotyfService notificationService,
			NotificationServiceOptions notificationServiceOptions,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetsController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_notificationServiceOptions = notificationServiceOptions;
			_hrvsterUser = hrvsterUser;
		}

		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> View(Guid ruleUid, Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning(
					$"Could not find sweepstake with uid: {sweepUid}.",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning(
					$"Could not find rule with uid: {ruleUid} ",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction(actionName, controllerName);
			}

			if(rule.Sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var scheduledTweet = await _mediator.Send(new GetScheduledTweetRequest(rule));
			var scheduledImages = scheduledTweet.Images?.Select(item => new string(item.ImageLocation)).ToList();
			var model =  new UpdateScheduledSweepRuleTweet
			{
				RuleId = rule.RuleId,
				ScheduledUid = scheduledTweet.ScheduledUid,
				Text = scheduledTweet.Text,
				ScheduledTime = scheduledTweet.ScheduledTime,
				PreviousMediaId1 = scheduledImages.Count >= 1 ? scheduledTweet.Images[0].ScheduledImageId : 0,
				PreviousMediaId2 = scheduledImages.Count >= 2 ? scheduledTweet.Images[1].ScheduledImageId : 0,
				PreviousMediaId3 = scheduledImages.Count >= 3 ? scheduledTweet.Images[2].ScheduledImageId : 0,
				PreviousMediaId4 = scheduledImages.Count >= 4 ? scheduledTweet.Images[3].ScheduledImageId : 0,
				Sweepstake = sweep,
				Rule = rule,
				ScheduledId = scheduledTweet.ScheduledId,
				ScheduledTimeUtc = scheduledTweet.ScheduledTimeUtc,
				Status = scheduledTweet.Status,
				Images = scheduledImages,
				ActionName = actionName ?? "Index",
				ControllerName = controllerName ?? "Dashboard"
				
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(
			UpdateScheduledSweepRuleTweet form, 
			Guid scheduledUid, 
			Guid sweepUid, 
			Guid ruleUid, 
			string actionName, 
			string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning(
					$"Could not find sweepstake with uid: {sweepUid}.",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning(
					$"Could not find rule with uid: {ruleUid} ",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (rule.Sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var scheduledTweet = await _mediator.Send(new GetScheduledTweetRequest(rule));
			if(scheduledTweet == null)
			{
				_logger.LogError("Could not find the scheduled tweet for the rule {rule}", rule.ToJsonString());

				_notificationService.Warning($"Could not find the scheduled tweet.", _notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (!form.Text.Contains(rule.WatchValue))
			{
				_notificationService.Warning("Your tweet needs to contain the watch value.", _notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if(scheduledTweet.Status != ScheduledStatus.New)
			{
				_notificationService.Warning("You cannot make changes to this scheduled tweet again.", _notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new UpdateScheduledTweetRequest(_hrvsterUser.UserUid, rule, sweep, scheduledUid, form.Text, form.ScheduledTime, form.PreviousMediaId1, form.Media1, form.PreviousMediaId2, form.Media2, form.PreviousMediaId3, form.Media3, form.PreviousMediaId4, form.Media4));

				_notificationService.Success("Scheduled tweet updated successfully.", _notificationServiceOptions.DismissInSeconds);
			}

			return RedirectToAction("View", new
			{
				RuleUid = ruleUid,
				SweepUid = sweepUid,
				actionName = actionName,
				controllerName = controllerName
			});
		}

		[HttpGet]
		public async Task<IActionResult> DeleteScheduledRule(
			Guid sweepUid, 
			Guid ruleUid, 
			string controllerName, 
			string actionName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning(
					$"Could not find sweepstake with uid: {sweepUid}.",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning(
					$"Could not find rule with uid: {ruleUid} ",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");
			if (rule.Sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var scheduledTweet = await _mediator.Send(new GetScheduledTweetRequest(rule));
			if (scheduledTweet == null)
			{
				_logger.LogError("Could not find the scheduled tweet for the rule {rule}", rule.ToJsonString());

				_notificationService.Warning($"Could not find the scheduled tweet.", _notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}
			var scheduledImages = scheduledTweet.Images?.Select(item => new string(item.ImageLocation)).ToList();
			var model = new DeleteScheduledSweepRuleTweet
			{
				RuleId = rule.RuleId,
				ScheduledUid = scheduledTweet.ScheduledUid,
				Text = scheduledTweet.Text,
				ScheduledTime = scheduledTweet.ScheduledTime,
				PreviousMediaId1 = scheduledImages.Count >= 1 ? scheduledTweet.Images[0].ScheduledImageId : 0,
				PreviousMediaId2 = scheduledImages.Count >= 2 ? scheduledTweet.Images[1].ScheduledImageId : 0,
				PreviousMediaId3 = scheduledImages.Count >= 3 ? scheduledTweet.Images[2].ScheduledImageId : 0,
				PreviousMediaId4 = scheduledImages.Count >= 4 ? scheduledTweet.Images[3].ScheduledImageId : 0,
				Sweepstake = sweep,
				Rule = rule,
				ScheduledId = scheduledTweet.ScheduledId,
				ScheduledTimeUtc = scheduledTweet.ScheduledTimeUtc,
				Status = scheduledTweet.Status,
				Images = scheduledImages,
				ActionName = actionName ?? "Index",
				ControllerName = controllerName ?? "Dashboard"
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteScheduledRule(
			DeleteScheduledSweepRuleTweet form,
			Guid sweepUid,
			Guid ruleUid,
			string controllerName,
			string actionName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning(
					$"Could not find sweepstake with uid: {sweepUid}.",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning(
					$"Could not find rule with uid: {ruleUid} ",
					_notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");
			if (rule.Sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var scheduledTweet = await _mediator.Send(new GetScheduledTweetRequest(rule));
			if (scheduledTweet == null)
			{
				_logger.LogError("Could not find the scheduled tweet for the rule {rule}", rule.ToJsonString());

				_notificationService.Warning($"Could not find the scheduled tweet.", _notificationServiceOptions.DismissInSeconds);

				return RedirectToAction("View", new
				{
					RuleUid = ruleUid,
					SweepUid = sweepUid,
					actionName = actionName,
					controllerName = controllerName
				});
			}

			await _mediator.Send(new DeleteScheduledSweepRuleTweetRequest(form.ScheduledUid, ruleUid));

			_notificationService.Success("Scheduled tweet deleted successfully.", _notificationServiceOptions.DismissInSeconds);

			return RedirectToAction("View", "SweepRules", new { sweepUid = sweepUid, ruleUid = ruleUid });
		}
	}
}
