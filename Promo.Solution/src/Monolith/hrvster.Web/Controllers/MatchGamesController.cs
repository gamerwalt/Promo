using AutoMapper;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.MatchGames;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using hrvster.Web.Models.Plays;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchGamesController : Controller
	{
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchImageService _matchImageService;
		private readonly IMatchRulesService _matchRuleService;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchPlayRulesService _playRulesService;
		private readonly IMatchGameplayService _gameplayService;

		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		private HrvsterUser _hrvsterUser;

		public MatchGamesController(
			IMatchGamesService matchGamesService,
			IMatchImageService matchImageService,
			IMatchRulesService matchRulesService,
			IMatchPrizeService matchPrizeService,
			IMatchPlayRulesService playRulesService,
			IMatchGameplayService gamePlayService,
			IToastNotification notificationService,
			ISessionManager sessionManager,
			IMapper mapper,
			IMediator mediator)
		{
			_matchGamesService = matchGamesService;
			_matchImageService = matchImageService;
			_matchRuleService = matchRulesService;
			_matchPrizeService = matchPrizeService;
			_playRulesService = playRulesService;
			_gameplayService = gamePlayService;
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
		public async Task<IActionResult> Create(Guid campaignUid, CancellationToken cancellationToken)
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

			var createNewMatchGame = BuildCreateGameModel(campaigns, campaignUid);
			ModelState.Remove("PromotionUid");

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
						form.GenerateBoardDate,
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
		[Route("/MatchGames/{Uid:Guid}/View")]
		public async Task<IActionResult> View(Guid Uid, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGameTask = _matchGamesService.GetMatchGameAsync(Uid, cancellationToken);
			var campaignsTask = _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			var imagesTask = _matchImageService.GetForMatchGameAsync(Uid, cancellationToken);
			var matchRulesTask = _matchRuleService.GetAsync(Uid, cancellationToken);
			var matchPrizeTask = _matchPrizeService.GetAllForMatchGameAsync(Uid, cancellationToken);
			var playRulesTask = _playRulesService.GetAllForMatchGameAsync(Uid, cancellationToken);
			var gamePlaysTask = _gameplayService.GetAllGamePlays(Uid, cancellationToken);

			var tasks = new List<Task>
			{
				matchGameTask,
				campaignsTask,
				imagesTask,
				matchRulesTask,
				matchPrizeTask,
				playRulesTask,
				gamePlaysTask
			};

			await Task.WhenAll(tasks);

			var matchGame = matchGameTask.Result;
			var campaigns = campaignsTask.Result;
			var images = imagesTask.Result;
			var rules = matchRulesTask.Result;
			var prizes = matchPrizeTask.Result;
			var playRules = playRulesTask.Result;
			var gamePlays = gamePlaysTask.Result;

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

			var model = BuildMatchGameDetails(matchGame, images, rules, prizes, playRules, gamePlays, campaigns);

			return View(model);
		}

		[HttpPost]
		[Route("/MatchGames/{Uid:Guid}/View")]
		public async Task<IActionResult> View(Guid Uid, MatchGameDetails form, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGame = await _matchGamesService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_notificationService.AddWarningToastMessage("Unable to locate the Match Game.");

				return RedirectToAction("Index");
			}

			if(ModelState.IsValid)
			{
				_notificationService.AddSuccessToastMessage("Match game updated successfully.");

				matchGame.Name = form.MatchGame.Name;
				matchGame.Description = form.MatchGame.Description;
				matchGame.NumberOfBoards = form.MatchGame.NumberOfBoards;
				matchGame.NumberOfWinnableBoards = form.MatchGame.WinnableBoards;
				matchGame.WinProbability = form.MatchGame.WinProbability;
				matchGame.StartDate = form.MatchGame.StartDate;
				matchGame.EndDate = form.MatchGame.EndDate;

				await _matchGamesService.UpdateMatchGameAsync(
					matchGame, 
					_hrvsterUser.WindowsTimeZone, 
					_hrvsterUser.IsDaylight, cancellationToken);

				return RedirectToAction("View", new { Uid = Uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("View", new { Uid = Uid });
		}

		[HttpPost]
		[Route("/MatchGames/{Uid:Guid}/Gameplay/{gamePlayUid:Guid}/Prizes/{prizeUid:Guid}/fulfill")]
		public async Task<IActionResult> Fulfill(Guid Uid, Guid gamePlayUid, Guid prizeUid, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGame = await _matchGamesService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_notificationService.AddWarningToastMessage("Unable to locate the Match Game.");

				return RedirectToAction("View", new { Uid });
			}

			var gamePlay = await _gameplayService.GetAsync(gamePlayUid, cancellationToken);
			if (gamePlay == null)
			{
				_notificationService.AddWarningToastMessage("Unable to locate the Game Play.");

				return RedirectToAction("View", new { Uid });
			}

			if(prizeUid != gamePlay.PrizeUid)
			{
				_notificationService.AddWarningToastMessage("Prize information is wrong.");

				return RedirectToAction("View", new { Uid });
			}

			if(ModelState.IsValid)
			{
				await _gameplayService.FulfillPrizeAsync(gamePlay, prizeUid, cancellationToken);
			}

			return RedirectToAction("View", new { Uid });
		}

		private MatchGameDetails BuildMatchGameDetails(
			Games.Match.Models.MatchGame matchGame,
			IEnumerable<Games.Match.Models.MatchImage> images,
			IEnumerable<Rule> rules,
			IEnumerable<Games.Match.Models.MatchPrize> prizes,
			Games.Match.Models.MatchPlayRule playRules,
			IEnumerable<Games.Match.Models.MatchPlay> gamePlays,
			List<Campaign> campaigns)
				=> new MatchGameDetails
				{
					Generated = matchGame.HasBeenGenerated,
					ReadyToBeGenerated = matchGame.ReadyToBeGenerated,
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
						EndDate = matchGame.EndDate,
						IsGenerated = matchGame.HasBeenGenerated,
						GenerateBoardDate = matchGame.GenerateBoardDate
					},
					MatchImages = _mapper.Map<List<MatchImage>>(images),
					MatchRules = _mapper.Map<List<MatchRule>>(rules),
					MatchPrizes = _mapper.Map<List<MatchPrize>>(prizes),
					MatchPlayRule = _mapper.Map<MatchPlayRule>(playRules),
					MatchPlays = _mapper.Map<IEnumerable<MatchPlay>>(gamePlays)
				};

		private static CreateMatchGame BuildCreateGameModel(List<Campaign> campaigns, Guid campaignUid)
			=> new CreateMatchGame()
			{
				Promotions = campaigns
					.Where(p => p.IsActive() || p.WillBeInProgress())
					.Select(a => new SelectListItem()
					{
						Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
						Value = a.CampaignUid.ToString(),
						Selected = a.CampaignUid == campaignUid
					}),
				StartDate = DateTime.Now.AddDays(1),
				EndDate = DateTime.Now.AddDays(30),
				GenerateBoardDate = DateTime.Now
			};
	}
}
