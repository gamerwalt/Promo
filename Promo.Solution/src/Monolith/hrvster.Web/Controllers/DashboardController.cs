using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests;
using hrvster.Core.Subscriptions;
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
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public DashboardController(
			ILoggerFactory loggerFactory, 
			IMediator mediator, 
			IMapper mapper,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<DashboardController>();
			_mediator = mediator;
			_mapper = mapper;
			_sessionManager = sessionManager;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var tasks = new List<Task>();
			var accountTask = _mediator.Send(new GetAccountRequest(_hrvsterUser.UserUid));
			var promotionsTask = _mediator.Send(new GetAllCampaignsAndPropertiesRequest(_hrvsterUser.UserUid));

			tasks.Add(accountTask);
			tasks.Add(promotionsTask);

			await Task.WhenAll(tasks);

			var model = new Dashboard
			{
				Account = _mapper.Map<hrvster.Models.WebModels.Account>(accountTask.Result),
				Promotions = promotionsTask.Result,
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> GetTweets([FromQuery] string campaignUid)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

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
			_hrvsterUser = _sessionManager.SetUserInformation();

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
