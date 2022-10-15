using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class DashboardController : Controller
	{
		private readonly ILogger<DashboardController> _logger;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		public DashboardController(ILoggerFactory loggerFactory, HrvsterUser hrvsterUser, IMediator mediator, IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<DashboardController>();
			_hrvsterUser = hrvsterUser;
			_mediator = mediator;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			var account = await _mediator.Send(new GetAccountRequest(_hrvsterUser.UserUid));

			var promotions = await _mediator.Send(new GetAllCampaignsAndPropertiesRequest(_hrvsterUser.UserUid));

			var model = new Dashboard
			{
				Account = _mapper.Map<hrvster.Models.WebModels.Account>(account),
				Promotions = promotions
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetTweets([FromQuery] string campaignUid)
		{
			_logger.LogInformation("Request to retrieve tweets and entries.");

			var promotions = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			if(!promotions.Any(c => c.CampaignUid.Equals(Guid.Parse(campaignUid))))
			{
				return new NotFoundObjectResult(new { message = "Data not found."});
			}

			var result = await _mediator.Send(new GetTweetsHarvestedRequest(_hrvsterUser.UserUid, Guid.Parse(campaignUid)));

			var jsonResult = BuildJsonResult(result);

			return new OkObjectResult(jsonResult);
		}

		[HttpGet]
		public async Task<IActionResult> GetSweepEntries([FromQuery] string campaignUid)
		{
			_logger.LogInformation("Request to retrieve tweets and entries.");

			var promotions = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			if (!promotions.Any(c => c.CampaignUid.Equals(Guid.Parse(campaignUid))))
			{
				return new NotFoundObjectResult(new { message = "Data not found." });
			}

			var result = await _mediator.Send(new GetSweepEntriesByCampaignRequest(Guid.Parse(campaignUid), _hrvsterUser.UserUid));

			var jsonResult = BuildJsonResult(result);

			return new OkObjectResult(jsonResult);
		}

		private string BuildJsonResult(IEnumerable<SweepEntryChart> result)
		{
			var newResult = from r in result
							group r by r.SweepName
							into sweepGroup
							select new
							{
								name = sweepGroup.Key,
								data = from d in sweepGroup
									   group d by d.Datepart.Date
									   into datePartGroup
									   select new
									   {
										   y = datePartGroup.Sum(x => x.Count),
										   x = datePartGroup.Key
									   }
							};

			return JsonConvert.SerializeObject(newResult);
		}

		private string BuildJsonResult(IEnumerable<HarvestedTweet> result)
		{
			var newResult = from r in result
							group r by r.SweepName
							into sweepGroup
							select new
							{
								name = sweepGroup.Key,
								data = from d in sweepGroup
									   group d by d.Datepart.Date
									   into datePartGroup
									   select new
									   {
										   y = datePartGroup.Sum(x => x.Count),
										   x = datePartGroup.Key
									   }
							};

			return JsonConvert.SerializeObject(newResult);
		}
	}
}
