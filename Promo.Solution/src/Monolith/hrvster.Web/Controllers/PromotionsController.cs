using AutoMapper;
using hrvster.ApplicationServices.Payments;
using hrvster.Core;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class PromotionsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ISessionManager _sessionManager;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly IPaymentService _paymentService;

		private HrvsterUser _hrvsterUser;

		public PromotionsController(
			IMediator mediator, 
			ISessionManager sessionManager,
			IMapper mapper, 
			IToastNotification notificationService,
			IPaymentService paymentService)
		{
			_mediator = mediator;
			_sessionManager = sessionManager;
			_mapper = mapper;
			_notificationService = notificationService;
			_paymentService = paymentService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			var promotions = _mapper.Map<IEnumerable<Promotion>>(campaigns.OrderBy(c => c.EndDate));

			return View(promotions);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid campaignUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_notificationService.AddErrorToastMessage("Promotion not found.");

				return RedirectToAction("Index");
			}

			if (campaign.Account.AccountUid != _hrvsterUser.UserUid) return RedirectToAction("Index");

			var sweeps = await _mediator.Send(new GetSweepsForCampaignUid(campaignUid, _hrvsterUser.UserUid));
			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			var viewPromotion = new ViewPromotion
			{
				CampaignName = campaign.CampaignName,
				Description = campaign.Description,
				CampaignUid = campaign.CampaignUid,
				Sweepstakes = _mapper.Map<List<Sweepstake>>(sweeps),
				StartDate = campaign.StartDate,
				EndDate = campaign.EndDate,
				StartDateUtc = campaign.StartDateUtc,
				EndDateUtc = campaign.EndDateUtc,
				UpdatePromotion = new UpdatePromotion
				{
					CampaignUid = campaign.CampaignUid,
					CampaignName = campaign.CampaignName,
					Description = campaign.Description,
					StartDate = campaign.StartDate,
					EndDate = campaign.EndDate,
					CampaignTypeUid = campaign.CampaignType.CampaignTypeUid,
					CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList(),
				}
			};

			return View(viewPromotion);
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid campaignUid)
		{
			return RedirectToActionPermanent("View", new { campaignUid = campaignUid });
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromQuery] Guid campaignUid, ViewPromotion form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_notificationService.AddErrorToastMessage("Promotion not found.");

				return RedirectToAction("Index");
			}

			if (campaign.Account.AccountUid != _hrvsterUser.UserUid) return RedirectToAction("Index");

			var sweeps = await _mediator.Send(new GetSweepsForCampaignUid(campaignUid, _hrvsterUser.UserUid));
			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			if (ModelState.IsValid)
			{
				await _mediator.Send(new UpdatePromotionRequest(
					campaignUid,
					form.UpdatePromotion.CampaignName,
					form.UpdatePromotion.Description,
					form.UpdatePromotion.CampaignTypeUid,
					form.UpdatePromotion.StartDate,
					form.UpdatePromotion.EndDate,
					_hrvsterUser.WindowsTimeZone,
					_hrvsterUser.IsDaylight));

				_notificationService.AddSuccessToastMessage("Promotion updated.");

				GetPromotionDetails(form, campaign, sweeps, campaignTypes);

				return RedirectToAction("Update", new { campaignUid = campaignUid });
			}

			GetPromotionDetails(form, campaign, sweeps, campaignTypes);

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View("View", form);
		}

		private void GetPromotionDetails(ViewPromotion form, Campaign campaign, IEnumerable<Sweep> sweeps, IEnumerable<CampaignType> campaignTypes)
		{
			form.CampaignName = campaign.CampaignName;
			form.Description = campaign.Description;
			form.CampaignUid = campaign.CampaignUid;
			form.Sweepstakes = _mapper.Map<List<Sweepstake>>(sweeps);
			form.StartDate = campaign.StartDate;
			form.EndDate = campaign.EndDate;
			form.EndDateUtc = campaign.EndDateUtc;
			form.StartDateUtc = campaign.StartDateUtc;
			form.UpdatePromotion.CampaignTypes = campaignTypes
					.Where(a => a.IsActive)
					.Select(a => new SelectListItem()
					{
						Text = a.CampaignTypeName,
						Value = a.CampaignTypeUid.ToString()
					}).ToList();
		}

		[HttpGet]
		[Route("/Promotions/Create")]
		public async Task<IActionResult> Create()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());
			var startDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, _hrvsterUser.WindowsTimeZone);

			var createPromotion = new CreatePromotion()
			{
				StartDate = startDate,
				EndDate = startDate.AddDays(30)
			};
			createPromotion.CampaignTypes = campaignTypes
				.Where(a => a.IsActive)
				.Select(a => new SelectListItem()
				{
					Text = a.CampaignTypeName,
					Value = a.CampaignTypeUid.ToString()
				}).ToList();

			return View(createPromotion);
		}

		[HttpPost]
		[Route("/Promotions/Create")]
		public async Task<IActionResult> Create(CreatePromotion model)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if (ModelState.IsValid)
			{
				var newCampaignRequest = new NewCampaignRequest
				{
					Name = model.Name,
					Description = model.Description,
					CampaignTypeUid = model.CampaignTypeUid,
					StartDate = model.StartDate,
					EndDate = model.EndDate
				};

				var promotionUid = await _mediator.Send(new CreateCampaignRequest(newCampaignRequest, _hrvsterUser.UserUid, _hrvsterUser.WindowsTimeZone));

				if(promotionUid == Guid.Empty)
				{
					_notificationService.AddErrorToastMessage("Unable to create promotion. Please speak to support.");

					return RedirectToAction("index");
				}

				_notificationService.AddSuccessToastMessage(ToastMessages.PromotionCreatedSuccessfully);

				return RedirectToAction("View", new { campaignUid = promotionUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors("Update Promotion ", string.Empty));

			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			model.CampaignTypes = campaignTypes
				.Where(a => a.IsActive)
				.Select(a => new SelectListItem()
				{
					Text = a.CampaignTypeName,
					Value = a.CampaignTypeUid.ToString()
				}).ToList();

			return View(model);
		}
	}
}
