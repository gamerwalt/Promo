using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreatePromotionController : Controller
	{
		private readonly ILogger<CreatePromotionController> _logger;
		private readonly IToastNotification _notificationService;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMediator _mediator;

		public CreatePromotionController(
			ILoggerFactory loggerFactory,
			IToastNotification notificationService,
			IMediator mediator,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<CreatePromotionController>();
			_notificationService = notificationService;
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			var model = new CreatePromotion
			{
				CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList(),
				StartDate = DateTime.UtcNow,
				EndDate = DateTime.UtcNow.AddDays(30)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreatePromotion form)
		{
			if (ModelState.IsValid)
			{
				var promotionUid = Guid.NewGuid();
				await _mediator.Send(
					new CreatePromotionRequest
					(
						promotionUid,
						form.Name,
						form.Description,
						form.CampaignTypeUid,
						form.StartDate,
						form.EndDate,
						_hrvsterUser.UserUid
					));

				_notificationService.AddSuccessToastMessage("Promotion created successfully. Let's create a Game now.");

				return RedirectToAction("Create", "CreateSweeps", new { campaignUid = promotionUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating a promotion. ClassName: {className}. Method Name: {action}. Message: {message}",
				nameof(CreatePromotionController),
				nameof(Create),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());
			form.CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList();

			return View(form);
		}

		[HttpGet]
		[Route("/CreatePromotion/View/{promotionUid:Guid}")]
		public async Task<IActionResult> View(Guid promotionUid)
		{
			var campaign = await _mediator.Send(new GetCampaign(promotionUid));

			if (campaign == null)
			{
				_logger.LogWarning("Could not find the campaign with uid {campaignUid}", promotionUid);

				_notificationService.AddWarningToastMessage($"Could not find promotion with uid: {promotionUid}.");

				return RedirectToAction("View", "Promotions", new { campaignUid = promotionUid });
			}

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_logger.LogWarning("Campaign with {campaignUid} is currently not active or will not be in progress in the future.", promotionUid);

				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = promotionUid });
			}

			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			var model = new UpdatePromotion
			{
				CampaignName = campaign.CampaignName,
				CampaignUid = campaign.CampaignUid,
				Description = campaign.Description,
				StartDate = campaign.StartDate,
				EndDate = campaign.EndDate,
				CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList(),
				CampaignTypeUid = campaign.CampaignType.CampaignTypeUid
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreatePromotion/View/{promotionUid:Guid}")]
		public async Task<IActionResult> View(Guid promotionUid, UpdatePromotion form)
		{
			var campaign = await _mediator.Send(new GetCampaign(promotionUid));

			if (campaign == null)
			{
				_logger.LogWarning("Could not find the campaign with uid {campaignUid}", promotionUid);

				_notificationService.AddWarningToastMessage($"Could not find promotion with uid: {promotionUid}.");

				return RedirectToAction("View", "Promotions", new { campaignUid = promotionUid });
			}

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_logger.LogWarning("Campaign with {campaignUid} is currently not active or will not be in progress in the future.", promotionUid);

				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = promotionUid });
			}

			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			if(ModelState.IsValid)
			{
				if(IsFormDirty(campaign, form))
				{
					await _mediator.Send(new UpdatePromotionRequest(promotionUid, form.CampaignName, form.Description, form.CampaignTypeUid, form.StartDate, form.EndDate));

					_notificationService.AddSuccessToastMessage("Promotion updated successfully.");
				}

				return RedirectToAction("Create", "CreateSweeps", new { campaignUid = promotionUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating a promotion. ClassName: {className}. Method Name: {action}. Message: {message}",
				nameof(CreatePromotionController),
				nameof(View),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList();

			return View(form);
		}

		private bool IsFormDirty(Campaign campaign, UpdatePromotion form)
		{
			return campaign.CampaignName != form.CampaignName ||
				campaign.Description != form.Description ||
				campaign.CampaignType.CampaignTypeUid != form.CampaignTypeUid ||
				campaign.StartDate != form.StartDate ||
				campaign.EndDate != form.EndDate;
		}
	}
}
