using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Models.WebModels.Sweeps;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepRulesController : Controller
	{
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;

		public SweepRulesController(IMediator mediator, HrvsterUser hrvsterUser, IMapper mapper)
		{
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweepRule = await _mediator.Send(new GetSweepRules(sweepUid));

			return View(sweepRule);
		}
	}
}
