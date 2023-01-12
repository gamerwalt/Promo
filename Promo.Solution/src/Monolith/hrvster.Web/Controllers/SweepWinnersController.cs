using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class SweepWinnersController : Controller
	{
		private readonly ILogger<SweepWinnersController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private hrvster.Models.HrvsterUser _hrvsterUser;

		public SweepWinnersController(ILoggerFactory loggerFactory, IMediator mediator, IToastNotification notificationService, ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<SweepWinnersController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}

		[HttpGet]
		[Route("/sweeps/{sweepUid:Guid}/winners/{sweepWinnerUid:Guid}/prize/{prizeUid:Guid}/promote-backup")]
		public async Task<IActionResult> PromoteBackup(
			Guid sweepUid,
			Guid sweepWinnerUid,
			Guid prizeUid,
			string actionName,
			string controllerName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogInformation("Unable to locate the sweepstake with uid {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage("Unable to locate the sweepstake.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			if (_hrvsterUser.UserUid != sweep.Account.AccountUid)
			{
				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var sweepWinner = await _mediator.Send(new GetSweepWinnerRequest(sweepWinnerUid));
			if (sweepWinner == null || sweepWinner.FulfilledInd)
			{
				_logger.LogInformation("Unable to locate the sweep winner with SweepWinnerUid {sweepWinnerUid}", sweepWinnerUid);

				_notificationService.AddWarningToastMessage("Unable to locate the winner or the winner has been fulfilled.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				_logger.LogInformation("Unable to locate the prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Unable to locate the prize.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var model = new PromoteBackupDetails
			{
				Sweepstake = sweep,
				SweepWinner = sweepWinner,
				Prize = prize,
				ReturnAction = actionName,
				ReturnController = controllerName,
			};

			return View(model);
		}

		[HttpPost]
		[Route("/sweeps/{sweepUid:Guid}/winners/{sweepWinnerUid:Guid}/prize/{prizeUid:Guid}/promote-backup")]
		public async Task<IActionResult> PromoteBackup(
			PromoteBackupDetails form,
			Guid sweepUid,
			Guid sweepWinnerUid,
			Guid prizeUid,
			string actionName,
			string controllerName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogInformation("Unable to locate the sweepstake with uid {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage("Unable to locate the sweepstake.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			if (_hrvsterUser.UserUid != sweep.Account.AccountUid)
			{
				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var sweepWinner = await _mediator.Send(new GetSweepWinnerRequest(sweepWinnerUid));
			if (sweepWinner == null || sweepWinner.FulfilledInd)
			{
				_logger.LogInformation("Unable to locate the sweep winner with SweepWinnerUid {sweepWinnerUid}", sweepWinnerUid);

				_notificationService.AddWarningToastMessage("Unable to locate the winner or the winner has been fulfilled.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				_logger.LogInformation("Unable to locate the prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Unable to locate the prize.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}
			
			if(!ModelState.IsValid)
			{
				_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

				return RedirectToAction("PromoteBackup", new
				{
					sweepUid = sweepUid,
					sweepWinnerUid = sweepWinnerUid,
					prizeUid = prizeUid,
					actionName = actionName,
					controllerName = controllerName,
				});
			}

			await _mediator.Send(new PromoteNextBackupWinnerRequest(sweep, prize, sweepWinner, form.Reason));

			_logger.LogInformation("Promote next backup winner successfull");

			_notificationService.AddSuccessToastMessage("Next winner promoted.");

			return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
		}

		[HttpGet]
		[Route("/sweeps/{sweepUid:Guid}/winners/{sweepWinnerUid:Guid}/prizes/{prizeUid}/fulfill")]
		public async Task<IActionResult> FulfillWinner(
			Guid sweepUid, Guid sweepWinnerUid, Guid prizeUid, string actionName, string controllerName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogInformation("Unable to locate the sweepstake with uid {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage("Unable to locate the sweepstake.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			if(_hrvsterUser.UserUid != sweep.Account.AccountUid)
			{
				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}
				
			var sweepWinner = await _mediator.Send(new GetSweepWinnerRequest(sweepWinnerUid));
			if(sweepWinner == null || sweepWinner.FulfilledInd)
			{
				_logger.LogInformation("Unable to locate the sweep winner with SweepWinnerUid {sweepWinnerUid}", sweepWinnerUid);

				_notificationService.AddWarningToastMessage("Unable to locate the winner or the winner has been fulfilled.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if(prize == null)
			{
				_logger.LogInformation("Unable to locate the prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Unable to locate the prize.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var model = new FulfillWinnerDetails
			{
				Sweepstake = sweep,
				SweepWinner = sweepWinner,
				Prize = prize,
				ReturnAction = actionName,
				ReturnController = controllerName,
			};

			return View(model);
		}

		[HttpPost]
		[Route("/sweeps/{sweepUid:Guid}/winners/{sweepWinnerUid:Guid}/prizes/{prizeUid}/fulfill")]
		public async Task<IActionResult> FulfillWinner(
			FulfillWinnerDetails form, 
			Guid sweepUid, 
			Guid sweepWinnerUid, 
			Guid prizeUid, 
			string actionName, 
			string controllerName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogInformation("Unable to locate the sweepstake with uid {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage("Unable to locate the sweepstake.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			if (_hrvsterUser.UserUid != sweep.Account.AccountUid)
			{
				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var sweepWinner = await _mediator.Send(new GetSweepWinnerRequest(sweepWinnerUid));
			if (sweepWinner == null || sweepWinner.FulfilledInd)
			{
				_logger.LogInformation("Unable to locate the sweep winner with SweepWinnerUid {sweepWinnerUid}", sweepWinnerUid);

				_notificationService.AddWarningToastMessage("Unable to locate the winner or the winner has been fulfilled.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				_logger.LogInformation("Unable to locate the prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Unable to locate the prize.");

				return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
			}

			await _mediator.Send(new MarkWinnerAsFulfilledRequest(sweep, prize, sweepWinner));

			_logger.LogInformation("Sweep winner {userIdentifier} has been fulfilled with the prize {prizeName} for the sweepstakes {sweepName}.",
				sweepWinner.UserIdentifier,
				prize.PrizeName,
				sweep.Name);

			_notificationService.AddSuccessToastMessage($"Winner {sweepWinner.UserIdentifier} has been fulfilled with the prize {prize.PrizeName}.");

			return RedirectToAction(
					string.IsNullOrWhiteSpace(actionName) ? "View" : actionName,
					string.IsNullOrWhiteSpace(controllerName) ? "Sweeps" : controllerName,
					new { sweepUid = sweepUid });
		}
	}
}
