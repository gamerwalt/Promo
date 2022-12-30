using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreateSweepsController : Controller
	{
		private readonly ILogger<CreateSweepsController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly SweepstakeConfiguration _sweepstakeConfiguration;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public CreateSweepsController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IToastNotification notificationService,
			SweepstakeConfiguration sweepstakeConfiguration,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepsController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_sweepstakeConfiguration = sweepstakeConfiguration;
			_sessionManager = sessionManager;
		}

		[HttpGet]
		public async Task<IActionResult> Create([FromQuery] Guid campaignUid)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_logger.LogWarning("Could not find the campaign with uid {campaignUid}", campaignUid);

				_notificationService.AddWarningToastMessage($"Could not find promotion with uid: {campaignUid}.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			if(campaign.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var wizardSweepUid = HttpContext.Session.GetString(SessionConstants.WizardSweepUid);

			if (!string.IsNullOrWhiteSpace(wizardSweepUid))
				return RedirectToAction("View", new { sweepUid = wizardSweepUid });

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_logger.LogWarning("Campaign with {campaignUid} is currently not active or will not be in progress in the future.", campaignUid);

				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			var createSweep = GetCreateSweepModel(campaign);

			return View(createSweep);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromQuery] Guid campaignUid, Models.CreateSweep form)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_logger.LogWarning("Could not find the campaign with uid {campaignUid}", campaignUid);

				_notificationService.AddWarningToastMessage($"Could not find promotion with uid: {campaignUid}.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_logger.LogWarning("Campaign with {campaignUid} is currently not active or will not be in progress in the future.", campaignUid);

				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			if (ModelState.IsValid)
			{
				var sweepUid = Guid.NewGuid();
				await _mediator.Send(new CreateSweepRequest(
					sweepUid,
					form.Name,
					form.StartDate,
					form.EndDate,
					form.DrawDate,
					campaignUid,
					_hrvsterUser.UserUid,
					_hrvsterUser.WindowsTimeZone,
					_hrvsterUser.IsDaylight));

				_notificationService.AddSuccessToastMessage("Sweepstake created successfully.");

				return RedirectToAction("Create", "CreateEntryRules", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating a sweepstake. ClassName: {className}. Method Name: {action}. Message: {message}",
				nameof(CreateSweepsController),
				nameof(Create),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.Campaign = campaign;

			return View(form);
		}

		[HttpGet]
		[Route("/CreateSweeps/View/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			HttpContext.Session.SetString(SessionConstants.WizardSweepUid, sweepUid.ToString());

			var model = new UpdateSweepstake
			{
				SweepUid = sweepUid,
				CampaignUid = sweep.Campaign.CampaignUid,
				Name = sweep.Name,
				StartDate = sweep.StartDate,
				EndDate = sweep.EndDate,
				DrawDate = sweep.DrawDate.GetValueOrDefault()
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateSweeps/View/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid, UpdateSweepstake form)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			if(ModelState.IsValid)
			{
				if(IsFormDirty(form, sweep))
				{
					await _mediator.Send(new UpdateSweepstakeRequest(sweep, sweepUid, form.Name, form.StartDate, form.EndDate, form.DrawDate, _hrvsterUser.WindowsTimeZone, _hrvsterUser.IsDaylight));

					_notificationService.AddSuccessToastMessage("Sweep updated successfully.");
				}

				return RedirectToAction("Create", "CreateEntryRules", new { sweepUid = sweepUid });
			}
			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating a sweepstake. ClassName: {className}. Method Name: {action}. Message: {message}",
				nameof(CreateSweepsController),
				nameof(View),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			return View(form);
		}

		private bool IsFormDirty(UpdateSweepstake form, Sweep sweep)
		{
			return form.Name != sweep.Name ||
				form.StartDate != sweep.StartDate ||
				form.EndDate != sweep.EndDate ||
				form.DrawDate != sweep.DrawDate;
		}

		private Models.CreateSweep GetCreateSweepModel(Campaign campaign)
		{
			var createSweep = new Models.CreateSweep();
			createSweep.Campaign = campaign;
			createSweep.StartDate = campaign.StartDate.AddDays(1);
			createSweep.EndDate = campaign.EndDate.AddDays(-_sweepstakeConfiguration.EndDateDaysAfterStartDate);
			createSweep.DrawDate = campaign.EndDate.AddDays(-_sweepstakeConfiguration.DrawDateDaysAfterEndDate);
			createSweep.Name = $"{campaign.CampaignName}-Sweepstakes";
			return createSweep;
		}
	}
}
