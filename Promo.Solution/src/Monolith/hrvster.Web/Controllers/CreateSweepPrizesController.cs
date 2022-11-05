using AutoMapper;
using hrvster.Core;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreateSweepPrizesController : Controller
	{
		private readonly ILogger<CreateSweepPrizesController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly HrvsterUser _hrvsterUser;
		private readonly PrizePositionFinder _prizePositionFinder;

		public CreateSweepPrizesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			IToastNotification notificationService,
			HrvsterUser hrvsterUser,
			PrizePositionFinder prizePositionFinder)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepPrizesController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
			_prizePositionFinder = prizePositionFinder;
		}

		[HttpGet]
		[Route("/CreateSweepPrizes/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			//do we already have all the prizes we need?
			//if we do, go to view for updates
			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			var drawRules = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			var prizesCount = prizes.ToList();

			_logger.LogInformation("There are {prizesCount} prizes already created for sweep {sweepName}", prizesCount.Count, sweep.Name);

			if (prizesCount.Count > 1)
			{
				if (prizesCount.Count == drawRules.WinnerCount)
				{
					_notificationService.AddInfoToastMessage("We have all the prizes we need. You can update them at this point or remove them.");

					return RedirectToAction("View", new { sweepUid = sweepUid });
				}
			}

			var totalRemainingPrizes = drawRules.WinnerCount - prizesCount.Count;

			_notificationService.AddInfoToastMessage($"There are {totalRemainingPrizes} prizes we need to create.");

			var model = new CreateSweepPrize
			{
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid,
				SweepId = sweep.SweepId,
				Position = _prizePositionFinder.GetNextPrizePosition(drawRules.WinnerCount, prizes),
				Prizes = _mapper.Map<IEnumerable<Prize>>(prizes)
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateSweepPrizes/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid, CreateSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			var drawRules = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			var prizesList = prizes.ToList();

			if (ModelState.IsValid && prizesList.Count < drawRules.WinnerCount)
			{
				await _mediator.Send(new CreateASweepPrizeRequest(sweepUid, form.Position, form.PrizeName, form.PrizeDescription, form.MonetaryValue));

				_notificationService.AddSuccessToastMessage("Prize created successfully.");

				return RedirectToAction("Create", new { sweepUid = sweepUid });
			}
			else if (prizesList.Count >= drawRules.WinnerCount)
			{
				_notificationService.AddWarningToastMessage("Unable to continue. It seems we have all the prizes we need.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from updating the Draw Rule for sweepstake {sweepName}. ClassName: {className}. Method Name: {action}. Message: {message}",
				sweep.Name,
				nameof(CreateDrawRulesController),
				nameof(View),
				messages);

			_notificationService.AddErrorToastMessage(messages);
			form.Prizes = _mapper.Map<IEnumerable<Prize>>(prizes);
			form.Position = prizesList.Count() + 1;
			form.SweepName = sweep.Name;
			
			return View(form);
		}

		[HttpGet]
		[Route("/CreateSweepPrizes/View/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			var drawRules = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));

			if (prizes.Count() == 0)
				return RedirectToAction("Create", new { sweepUid = sweepUid });

			var model = new ViewSweepPrizes
			{
				Prizes = prizes.OrderBy(c => c.PrizePosition),
				Sweep = sweep,
				WinnerCount = drawRules.WinnerCount
			};

			return View(model);
		}

		[HttpGet]
		[Route("/CreateSweepPrizes/EditPrize/Sweep/{sweepUid:Guid}/Prize/{prizeUid:Guid}")]
		public async Task<IActionResult> EditPrize(Guid sweepUid, Guid prizeUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			if(!prizes.Any(c => c.PrizeUid == prizeUid))
			{
				_logger.LogWarning("Could not find the prize with uid {prizeUid} for sweeps {sweepName} {sweepUid} you were trying to edit.",
					prizeUid,
					sweep.Name,
					sweep.SweepUid);

				_notificationService.AddWarningToastMessage("Could not find the prize you were trying to edit.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			var prize = prizes.FirstOrDefault(p => p.PrizeUid == prizeUid);

			var model = new EditSweepPrize
			{
				CampaignUid = sweep.Campaign.CampaignUid,
				Prize = _mapper.Map<Prize>(prize),
				PrizeUid = prize.PrizeUid,
				PrizeName = prize.PrizeName,
				PrizeDescription = prize.PrizeDescription,
				MonetaryValue = prize.PrizeValue,
				Sweep = sweep,
				SweepId = sweep.SweepId,
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateSweepPrizes/EditPrize/Sweep/{sweepUid:Guid}/Prize/{prizeUid:Guid}")]
		public async Task<IActionResult> EditPrize(Guid sweepUid, Guid prizeUid, EditSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			if (!prizes.Any(c => c.PrizeUid == prizeUid))
			{
				_logger.LogWarning("Could not find the prize with uid {prizeUid} for sweeps {sweepName} {sweepUid} you were trying to edit.",
					prizeUid,
					sweep.Name,
					sweep.SweepUid);

				_notificationService.AddWarningToastMessage("Could not find the prize you were trying to edit.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			await _mediator.Send(new UpdateSweepPrize(sweepUid, prizeUid, form.PrizeName, form.PrizeDescription, form.MonetaryValue));

			_notificationService.AddSuccessToastMessage("Prize updated successfully.");

			return RedirectToAction("View", new { sweepUid = sweepUid });
		}

		[HttpGet]
		[Route("/CreateSweepPrizes/DeletePrize/Sweep/{sweepUid:Guid}/Prize/{prizeUid:Guid}")]
		public async Task<IActionResult> DeletePrize(Guid sweepUid, Guid prizeUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			if (!prizes.Any(c => c.PrizeUid == prizeUid))
			{
				_logger.LogWarning("Could not find the prize with uid {prizeUid} for sweeps {sweepName} {sweepUid} you were trying to edit.",
					prizeUid,
					sweep.Name,
					sweep.SweepUid);

				_notificationService.AddWarningToastMessage("Could not find the prize you were trying to edit.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			var prize = prizes.FirstOrDefault(p => p.PrizeUid == prizeUid);

			var model = new DeleteSweepPrize
			{
				CampaignUid = sweep.Campaign.CampaignUid,
				Prize = _mapper.Map<Prize>(prize),
				PrizeUid = prize.PrizeUid,
				PrizeName = prize.PrizeName,
				PrizeDescription = prize.PrizeDescription,
				MonetaryValue = prize.PrizeValue,
				Sweep = sweep,
				SweepId = sweep.SweepId,
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateSweepPrizes/DeletePrize/Sweep/{sweepUid:Guid}/Prize/{prizeUid:Guid}")]
		public async Task<IActionResult> DeletePrize(Guid sweepUid, Guid prizeUid, DeleteSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			if (!prizes.Any(c => c.PrizeUid == prizeUid))
			{
				_logger.LogWarning("Could not find the prize with uid {prizeUid} for sweeps {sweepName} {sweepUid} you were trying to edit.",
					prizeUid,
					sweep.Name,
					sweep.SweepUid);

				_notificationService.AddWarningToastMessage("Could not find the prize you were trying to edit.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			await _mediator.Send(new DeleteSweepPrizeRequest(prizeUid));

			_notificationService.AddSuccessToastMessage("Prize deleted successfully.");

			return RedirectToAction("View", new { sweepUid = sweepUid });
		}
	}
}
