using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Extensions.ModelExtensions;
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
	}
}
