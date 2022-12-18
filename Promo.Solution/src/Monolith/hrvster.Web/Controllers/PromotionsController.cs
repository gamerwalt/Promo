using AutoMapper;
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
	//[ShouldBeSubscribed]
	public class PromotionsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;

		public PromotionsController(IMediator mediator, HrvsterUser hrvsterUser, IMapper mapper, IToastNotification notificationService)
		{
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			var promotions = _mapper.Map<IEnumerable<Promotion>>(campaigns.OrderBy(c => c.EndDate));

			return View(promotions);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid campaignUid)
		{
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
					form.UpdatePromotion.EndDate));

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
			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			var createPromotion = new CreatePromotion()
			{
				StartDate = DateTime.UtcNow,
				EndDate = DateTime.UtcNow
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

				var promotionUid = await _mediator.Send(new CreateCampaignRequest(newCampaignRequest, _hrvsterUser.UserUid));

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
