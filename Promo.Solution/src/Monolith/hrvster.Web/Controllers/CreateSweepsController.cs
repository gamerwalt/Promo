using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class CreateSweepsController : Controller
	{
		private readonly ILogger<CreateSweepsController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly SweepstakeConfiguration _sweepstakeConfiguration;
		private readonly HrvsterUser _hrvsterUser;

		public CreateSweepsController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IToastNotification notificationService,
			SweepstakeConfiguration sweepstakeConfiguration,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepsController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_sweepstakeConfiguration = sweepstakeConfiguration;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		public async Task<IActionResult> Create([FromQuery] Guid campaignUid)
		{
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

			var createSweep = GetCreateSweepModel(campaign);

			//_notificationService.AddSuccessToastMessage("Promotion created successfully. Let's create a Game of Sweepstakes now.", NotificationService.DismissInSeconds);

			return View(createSweep);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromQuery] Guid campaignUid, Models.CreateSweep form)
		{
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
					_hrvsterUser.UserUid));

				_notificationService.AddSuccessToastMessage("Sweepstake created successfully.");

				return RedirectToAction("Create", "CreateSweepRules", new { sweepUid = sweepUid });
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
