using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class PromotionsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;
		private readonly INotyfService _notificationService;

		public PromotionsController(IMediator mediator, HrvsterUser hrvsterUser, IMapper mapper, INotyfService notificationService)
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
			var promotions = _mapper.Map<IEnumerable<Promotion>>(campaigns);
			
			return View(promotions);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid campaignUid)
		{
			var campaign = await _mediator.Send(new GetCampaign(campaignUid));
			var sweeps = await _mediator.Send(new GetSweepsForCampaignUid(campaignUid, _hrvsterUser.UserUid));

			var promotion = _mapper.Map<Promotion>(campaign);
			promotion.Sweepstakes = _mapper.Map<IEnumerable<Sweepstake>>(sweeps);

			return View(promotion);
		}

		[HttpGet]
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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreatePromotion model)
		{
			if(ModelState.IsValid)
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

				return RedirectToAction("View", new { campaignUid = promotionUid });
			}

			var errors = ModelState.Select(c => c.Value.Errors)
				.Where(c => c.Count > 0)
				.ToList();

			_notificationService.Error(BuildMessageFromErrors(errors));

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

		private string BuildMessageFromErrors(List<ModelErrorCollection> errors)
		{
			var sb = new StringBuilder();
			foreach (var error in errors)
			{
				sb.Append(error.FirstOrDefault()?.ErrorMessage);
			}

			return sb.ToString();
		}
	}
}
