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
	public class MatchImageController : Controller
	{
		private readonly ISessionManager _sessionManager;
		private readonly IMatchGamesService _matchGamesService;
		private readonly ICampaignService _campaignService;
		private readonly IMatchImageService _matchImagesService;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;

		private HrvsterUser _hrvsterUser;

		public MatchImageController(
			ISessionManager sessionManager,
			IMatchGamesService matchGamesService,
			ICampaignService campaignService,
			IMatchImageService matchImagesService,
			IMapper mapper,
			IToastNotification notificationService)
		{
			_sessionManager = sessionManager;
			_matchGamesService = matchGamesService;
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

			var matchImages = await _matchImagesService.GetByMatchGameUid(matchGameUid, cancellationToken);

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
		public async Task<IActionResult> Create(Guid matchGameUid, CancellationToken cancellationToken)
		{
			return new OkObjectResult(matchGameUid);
		}
	}
}
