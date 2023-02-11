using AutoMapper;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchImagesController : Controller
	{
		private readonly ISessionManager _sessionManager;
		private readonly IMatchGamesService _matchGamesService;
		private readonly IMatchImageService _matchImageService;
		private readonly ICampaignService _campaignService;
		private readonly IMatchImageService _matchImagesService;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;

		private HrvsterUser _hrvsterUser;

		public MatchImagesController(
			ISessionManager sessionManager,
			IMatchGamesService matchGamesService,
			IMatchImageService matchImageService,
			ICampaignService campaignService,
			IMatchImageService matchImagesService,
			IMapper mapper,
			IToastNotification notificationService)
		{
			_sessionManager = sessionManager;
			_matchGamesService = matchGamesService;
			_matchImageService = matchImageService;
			_campaignService = campaignService;
			_matchImagesService = matchImagesService;
			_mapper = mapper;
			_notificationService = notificationService;
		}

		[HttpGet]
		[Route("/MatchImages/game/{matchGameUid:Guid}")]
		public async Task<IActionResult> Index(Guid matchGameUid, CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGame = await _matchGamesService.GetMatchGameAsync(matchGameUid, cancellationToken);
			if(matchGame == null)
			{
				_notificationService.AddErrorToastMessage("Unable to find Match Game.");

				return RedirectToAction("Index", "MatchGames");
			}

			var campaign = await _campaignService.GetCampaignAsync(matchGame.CampaignUid, cancellationToken);
			if((campaign == null || !campaign.WillBeInProgress()) && !campaign.IsActive())
			{
				_notificationService.AddWarningToastMessage("Could not find the campaign or the campaign is neither active nor will be in progress in the future.");

				return RedirectToAction("Index", "MatchGames");
			}

			if(campaign.Account.AccountUid != _hrvsterUser.UserUid)
			{
				return RedirectToAction("Index", "MatchGames");
			}

			var matchImages = await _matchImagesService.GetForMatchGameAsync(matchGameUid, cancellationToken);

			var model = new MatchGameImages
			{
				MatchImages = _mapper.Map<IEnumerable<MatchImage>>(matchImages),
				Campaign = campaign,
				MatchGame = _mapper.Map<MatchGame>(matchGame)
			};
			
			return View(model);
		}

		[HttpGet]
		[Route("/MatchImages/game/{matchGameUid:Guid}/create")]
		public async Task<IActionResult> Create(
			Guid matchGameUid, 
			string actionName, 
			string controllerName, 
			CancellationToken cancellationToken)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var matchGame = await _matchGamesService.GetMatchGameAsync(matchGameUid, cancellationToken);
			if (matchGame == null)
			{
				_notificationService.AddErrorToastMessage("Unable to find Match Game.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
			}

			var campaign = await _campaignService.GetCampaignAsync(matchGame.CampaignUid, cancellationToken);
			if ((campaign == null || !campaign.WillBeInProgress()) && !campaign.IsActive())
			{
				_notificationService.AddWarningToastMessage("Could not find the campaign or the campaign is neither active nor will be in progress in the future.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
			}

			if (campaign.Account.AccountUid != _hrvsterUser.UserUid)
			{
				return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
			}

			var model = new CreateImage
			{
				MatchGame = _mapper.Map<MatchGame>(matchGame),
				Campaign = campaign
			};

			return View(model);
		}

		[HttpPost]
		[Route("/MatchImages/game/{matchGameUid:Guid}/create")]
		public async Task<IActionResult> Create(
			Guid matchGameUid, 
			CreateImage form,
			string actionName,
			string controllerName,
			CancellationToken cancellationToken)
		{
			if (ModelState.IsValid)
			{
				_hrvsterUser = _sessionManager.GetUserInformation();

				var matchGame = await _matchGamesService.GetMatchGameAsync(matchGameUid, cancellationToken);
				if (matchGame == null)
				{
					_notificationService.AddErrorToastMessage("Unable to find Match Game.");

					return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
				}

				var campaign = await _campaignService.GetCampaignAsync(matchGame.CampaignUid, cancellationToken);
				if ((campaign == null || !campaign.WillBeInProgress()) && !campaign.IsActive())
				{
					_notificationService.AddWarningToastMessage("Could not find the campaign or the campaign is neither active nor will be in progress in the future.");

					return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
				}

				if (campaign.Account.AccountUid != _hrvsterUser.UserUid)
				{
					return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
				}

				if (!await _matchImageService.CreateImagesAsync(matchGame, form.Images, cancellationToken))
				{
					_notificationService.AddErrorToastMessage($"An error occurred trying to save the images for {matchGame.Name}");

					return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
				}

				_notificationService.AddSuccessToastMessage("Match image(s) saved successfully.");

				return RedirectToAction(actionName ?? "Index", controllerName ?? "MatchGames");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("Create", new { matchGameUid });
		}
	}
}
