using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Extensions.ModelExtensions;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreateTweetController : Controller
	{
		private readonly ILogger<CreateTweetController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly HrvsterUser _hrvsterUser;

		public CreateTweetController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			IToastNotification notificationService,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<CreateTweetController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		[Route("/CreateTweet/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepsUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepsUid));
			if(sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepsUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepsUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepsUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var model = new CreateSweepRuleTweet
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeDrawDate = sweep.DrawDate,
				SweepstakeName = sweep.Name,
				SweepstakeStartDate = sweep.StartDate,
				SweepstakeEndDate = sweep.EndDate
			};

			return View();
		}
	}
}
