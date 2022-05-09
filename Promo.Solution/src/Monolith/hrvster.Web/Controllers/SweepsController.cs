using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models.WebModels;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CreateSweep = hrvster.Web.Models.CreateSweep;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;

		public SweepsController(IMediator mediator, HrvsterUser hrvsterUser, IMapper mapper)
		{
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Create(Guid campaignUid)
		{
			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				ModelState.AddModelError("CustomError", "Campaign not found.");
				return RedirectToAction("Index");
			}

			var createSweep = new CreateSweep();
			createSweep.Campaign = campaign;
			createSweep.StartDate = campaign.StartDate.AddDays(1);
			createSweep.EndDate = campaign.EndDate.AddDays(-1);
			createSweep.DrawDate = campaign.EndDate.AddDays(10);
			createSweep.Name = $"{campaign.CampaignName}-Sweepstakes";

			return View(createSweep);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid campaignUid, CreateSweep model)
		{
			if(ModelState.IsValid)
			{
				var request = new NewSweepRequest()
				{
					AccountUid = _hrvsterUser.UserUid,
					CampaignUid = model.Campaign.CampaignUid,
					Name = model.Name,
					StartDate = model.StartDate,
					EndDate = model.EndDate,
					DrawDate = model.DrawDate
				};
				
				var sweep = await _mediator.Send(new Core.Requests.CreateSweep(request));

				if(sweep == null)
				{
					ModelState.AddModelError("CustomError", "Unable to create sweep. Please reach out to support.");

					return RedirectToAction("Create", new { campaignUid = model.Campaign.CampaignUid });
				}

				return RedirectToAction("View", new { sweepUid = sweep.SweepUid });
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));

			return View(_mapper.Map<Sweepstake>(sweep));
		}
	}
}
