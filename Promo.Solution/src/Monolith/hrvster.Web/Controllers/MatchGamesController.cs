using AutoMapper;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.MatchGames;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchImageService _matchImageService;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		private HrvsterUser _hrvsterUser;

		public MatchGamesController(
			IMatchGamesService matchGamesService,
			IMatchImageService matchImageService,
			IToastNotification notificationService,
			ISessionManager sessionManager,
			IMapper mapper,
			IMediator mediator)
		{
			_matchGamesService = matchGamesService;
			_matchImageService = matchImageService;
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

			if (campaigns.Count == 0)
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

			var createNewMatchGame = BuildCreateGameModel(campaigns);

			return View(createNewMatchGame);
		}

		[HttpPost]
		[Route("/MatchGames/Create")]
		public async Task<IActionResult> Create(CreateMatchGame form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));

			if (ModelState.IsValid)
			{
				var matchGameUid = Guid.NewGuid();
				if (campaigns.Count == 0)
				{
					_notificationService.AddInfoToastMessage("You need a promotion before you continue.");

					return RedirectToAction("Index", "Promotions");
				}

				if (!campaigns.Any(c => c.CampaignUid == form.PromotionUid))
				{
					_notificationService.AddErrorToastMessage("An error occurred trying to add your match game. Please try again.");

					return RedirectToAction("create", form);
				}

				var anyActivePromotions = campaigns.Any(c => c.IsActive());
				var anyWillBeInProgress = campaigns.Any(c => c.WillBeInProgress());

				if (!anyActivePromotions && !anyWillBeInProgress)
				{
					_notificationService.AddErrorToastMessage(
						"None of your promotions are active. We need 1 active promotion to continue.");

					return RedirectToAction("Index");
				}

				if (await _mediator.Send(new CreateMatchGameRequest
					(
						matchGameUid,
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
						_hrvsterUser.IsDaylight)
					))
					return RedirectToAction("Index");

				form.Promotions = campaigns
								.Where(p => p.IsActive() || p.WillBeInProgress())
								.Select(a => new SelectListItem()
								{
									Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
									Value = a.CampaignUid.ToString()
								});

				return View("create", form);
			}

			form.Promotions = campaigns
				.Where(p => p.IsActive() || p.WillBeInProgress())
				.Select(a => new SelectListItem()
				{
					Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
					Value = a.CampaignUid.ToString()
				});

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View("create", form);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid Uid, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGameTask = _matchGamesService.GetMatchGameAsync(Uid, cancellationToken);
			var campaignsTask = _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			var imagesTask = _matchImageService.GetForMatchGameAsync(Uid, cancellationToken);
			//var matchRulesTask = _matchRulesService

			var tasks = new List<Task>();
			tasks.Add(matchGameTask);
			tasks.Add(campaignsTask);
			tasks.Add(imagesTask);

			await Task.WhenAll(tasks);

			var matchGame = matchGameTask.Result;
			var campaigns = campaignsTask.Result;
			var images = imagesTask.Result;

			if (matchGame == null)
			{
				_notificationService.AddWarningToastMessage("Unable to locate the Match Game.");

				return RedirectToAction("Index");
			}

			if (campaigns.Count == 0)
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

			var model = BuildMatchGameDetails(matchGame, images, campaigns);

			return View(model);
		}

		private MatchGameDetails BuildMatchGameDetails(
			Games.Match.Models.MatchGame matchGame,
			IEnumerable<Games.Match.Models.MatchImage> images,
			List<Campaign> campaigns)
				=> new MatchGameDetails
				{
					MatchGame = new UpdateMatchGame
					{
						Promotions = campaigns
							.Where(p => p.IsActive() || p.WillBeInProgress())
							.Select(a => new SelectListItem()
							{
								Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
								Value = a.CampaignUid.ToString()
							}),
						PromotionUid = matchGame.CampaignUid,
						MatchGameUid = matchGame.Uid,
						Name = matchGame.Name,
						Description = matchGame.Description,
						NumberOfBoards = matchGame.NumberOfBoards,
						WinnableBoards = matchGame.NumberOfWinnableBoards,
						WinProbability = matchGame.WinProbability,
						StartDate = matchGame.StartDate,
						EndDate = matchGame.EndDate
					},
					MatchImages = _mapper.Map<List<MatchImage>>(images)
				};

		private static CreateMatchGame BuildCreateGameModel(List<Campaign> campaigns)
			=> new CreateMatchGame()
			{
				Promotions = campaigns
									.Where(p => p.IsActive() || p.WillBeInProgress())
									.Select(a => new SelectListItem()
									{
										Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
										Value = a.CampaignUid.ToString()
									}),
				StartDate = DateTime.Now.AddDays(1),
				EndDate = DateTime.Now.AddDays(30)
			};
	}
}
