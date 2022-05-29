using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class WatchTypeProviderController : Controller
	{
		private readonly ILogger<WatchTypeProviderController> _logger;
		private readonly IMediator _mediator;

		public WatchTypeProviderController(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<WatchTypeProviderController>();
			_mediator = mediator;
		}

		public async Task<IActionResult> Select(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var watchTypeProviders = await _mediator.Send(new GetWatchTypeProvidersRequest());

			var model = new SelectWatchTypeProvider()
			{
				Providers = watchTypeProviders.Select(a => new SelectListItem()
				{
					Text = a.WatchTypeProviderName,
					Value = a.WatchTypeProviderId.ToString()
				}),
				Sweep = sweep,
				Campaign = sweep.Campaign
			};

			return View(model);
		}
	}
}
