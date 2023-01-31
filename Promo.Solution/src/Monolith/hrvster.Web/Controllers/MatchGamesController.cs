using AutoMapper;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.WebModels;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
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
					})
			};

			return View(createNewMatchGame);
		}

		[HttpPost]
		[Route("/MatchGames/Create")]
		public async Task<IActionResult> Create(CreateMatchGame form, CancellationToken cancellationToken)
		{
			return View();
		}
	}
}
