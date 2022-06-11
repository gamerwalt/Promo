using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepPrizesController : Controller
	{
		private readonly ILogger<SweepPrizesController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;

		public SweepPrizesController(
			ILoggerFactory loggerFactory, 
			IMediator mediator,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<SweepPrizesController>();
			_mediator = mediator;
			_mapper = mapper;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var model = await _mediator.Send(new GetSweepPrizes(sweepUid));

			return View(_mapper.Map<IEnumerable<ViewSweepPrize>>(model));
		}
	}
}
