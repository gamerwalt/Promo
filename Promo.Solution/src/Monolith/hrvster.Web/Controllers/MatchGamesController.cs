using AutoMapper;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.MatchGames;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.WebModels;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchGamesController : Controller
	{
		private readonly ILogger<MatchGamesController> _logger;
		private readonly IMatchGamesService _matchGamesService;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		private hrvster.Models.HrvsterUser _hrvsterUser;

		public MatchGamesController(
			ILoggerFactory loggerFactory,
			IMatchGamesService matchGamesService,
			IToastNotification notificationService,
			ISessionManager sessionManager,
			IMapper mapper,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<MatchGamesController>();
			_matchGamesService = matchGamesService;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
			_mapper = mapper;
			_mediator = mediator;
		}

		[HttpGet]
		[Route("/MatchGames")]
		public async Task<IActionResult> Index(CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGames = await _matchGamesService.GetAccountMatchGamesAsync(_hrvsterUser.UserUid, cancellationToken);

			return View(_mapper.Map<IEnumerable<MatchGame>>(matchGames));
		}

		[HttpGet]
		[Route("/MatchGames/Create")]
		public async Task<IActionResult> Create(CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));

			if(campaigns.Count == 0)
			{
				_notificationService.AddInfoToastMessage("You need a promotion before you continue.");

				return RedirectToAction("Index", "Promotions");
			}

			var anyActivePromotions = campaigns.Any(c => c.IsActive());
			var anyWillBeInProgress = campaigns.Any(c => c.WillBeInProgress());

			if (!anyActivePromotions && !anyWillBeInProgress)
			{
				_notificationService.AddErrorToastMessage(
					"None of your promotions are active. We need 1 active promotion to continue.");

				return RedirectToAction("Index");
			}

			var createNewMatchGame = new CreateMatchGame()
			{
				Promotions = campaigns
					.Where(p => p.IsActive() || p.WillBeInProgress())
					.Select(a => new SelectListItem()
					{
						Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
						Value = a.CampaignUid.ToString()
					}),
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddDays(30)
			};

			return View(createNewMatchGame);
		}

		[HttpPost]
		[Route("/MatchGames/Create")]
		public async Task<IActionResult> Create(CreateMatchGame form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if(ModelState.IsValid)
			{
				var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
				if(!campaigns.Any(c => c.CampaignUid == form.PromotionUid))
				{
					_notificationService.AddErrorToastMessage("An error occurred trying to add your match game. Please try again.");

					return RedirectToAction("create", form);
				}

				await _mediator.Send(new CreateMatchGameRequest
					(
					_hrvsterUser.UserUid, 
					form.PromotionUid, 
					form.Name, 
					form.Description, 
					form.NumberOfBoards, 
					form.WinnableBoards, 
					form.WinProbability, 
					form.StartDate, 
					form.EndDate, 
					_hrvsterUser.WindowsTimeZone, 
					_hrvsterUser.IsDaylight));

				return RedirectToAction("Index");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("create", form);
		}
	}
}
