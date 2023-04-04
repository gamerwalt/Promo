using AutoMapper;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Web.Models.Plays;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class PlayMatchGameController : Controller
	{
		private readonly ILogger<PlayMatchGameController> _logger;
		private readonly SignatureUtility _signatureUtility;
		private readonly IWatchTypeProvider _watchTypeProvider;
		private readonly ISessionManager _sessionManager;
		private readonly ISignatureRepository _signatureRepository;
		private readonly IToastNotification _notificationService;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchGameBoardService _gameBoardService;
		private readonly IMatchGameplayService _gameplayService;
		private readonly IMapper _mapper;

		public PlayMatchGameController(
			ILoggerFactory loggerFactory,
			SignatureUtility signatureUtility,
			IWatchTypeProvider watchTypeProvider,
			ISessionManager sessionManager,
			ISignatureRepository signatureRepository,
			IToastNotification notificationService,
			IMatchGamesService matchGameService,
			IMatchGameBoardService gameBoardService,
			IMatchGameplayService gameplayService,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<PlayMatchGameController>();
			_signatureUtility = signatureUtility;
			_watchTypeProvider = watchTypeProvider;
			_sessionManager = sessionManager;
			_signatureRepository = signatureRepository;
			_notificationService = notificationService;
			_matchGameService = matchGameService;
			_gameBoardService = gameBoardService;
			_gameplayService = gameplayService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("/PlayMatchGame")]
		public async Task<IActionResult> Play(string signature, CancellationToken cancellationToken)
		{
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "Signature", signature }
			}))
			{
				_logger.LogInformation("Received a request to play a Match game with signature {signature}", signature);

				_logger.LogInformation("Requesting the user to logon to twitter.");

				_sessionManager.SetKeyValue(SessionConstants.Signature, signature);

				var authorizationUrl = await _watchTypeProvider.GetAuthorizationUrl(Constants.TwitterProvider, Request.Scheme, Request.Host.Value, $"/PlayMatchGame/Validate{Constants.TwitterProvider}Auth");

				return new RedirectResult(authorizationUrl);
			}
		}

		public IActionResult FailedAuthValidation()
		{
			return View();
		}


		public async Task<IActionResult> ValidateTwitterAuth(
			Guid tweetinvi_auth_request_id,
			string oauth_token,
			string oauth_verifier)
		{
			_logger.LogInformation("Validation of twitter auth successful.");

			if (Request.Query["denied"].Count > 0)
			{
				return RedirectToAction("FailedAuthValidation");
			}

			_logger.LogInformation("Getting user details from the system.");

			var userDetails = await _watchTypeProvider.IsValidAuthorization(Constants.TwitterProvider, Request.QueryString.Value);

			if (userDetails == null ||
				string.IsNullOrWhiteSpace(userDetails.ExternalProfileId) ||
					string.IsNullOrWhiteSpace(userDetails.ScreenName))
			{
				_logger.LogError("Could not get the user details.");

				return RedirectToAction("FailedAuthValidation");
			}

			var signatureSession = _sessionManager.GetValue(SessionConstants.Signature);

			var signature = await _signatureRepository.GetAsync(signatureSession, userDetails.ScreenName);

			if (signature == null)
			{
				_notificationService.AddErrorToastMessage("Signature has expired.");

				return RedirectToAction("SignatureExpired");
			}

			if (DateTime.UtcNow > signature.ExpiryDate)
			{
				_notificationService.AddErrorToastMessage("Signature has expired.");

				return RedirectToAction("SignatureExpired");
			}

			_logger.LogInformation("Signature with uid {signatureUid} not expired.", signature.Uid);

			_logger.LogInformation("Verifying the signature with details received from Twitter");

			if (!VerifySignature(signature.GamePlayUid, userDetails, signatureSession))
			{
				return RedirectToAction("SignatureFailed");
			}

			_logger.LogInformation("Signature verified for signature with uid {signatureUid}", signature.Uid);

			SetSessionVariables(userDetails, signature);

			_logger.LogInformation("Generating new game play uid.");

			return RedirectToAction(nameof(SignaturePlay), new { Uid = signature.GameUid, gameplayUid = signature.GamePlayUid });
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> SignaturePlay(Guid Uid, Guid gameplayUid, CancellationToken cancellationToken)
		{
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", Uid },
				{ "GamePlayUid", gameplayUid }
			}))

			_logger.LogInformation("Verifying the game.");

			if (!await _matchGameService.GamePlayableAsync(Uid, cancellationToken))
			{
				_notificationService.AddErrorToastMessage("Unable to play the game or the game doesn't exist.");

				return RedirectToAction("GameUnplayable");
			}

			_logger.LogInformation("Verifying the gameplay and the gameboard");
			if (!await _gameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, cancellationToken))
			{
				_notificationService.AddErrorToastMessage("Unable to play this game.");

				return RedirectToAction("BoardUnplayable");
			}

			var gameplay = await _gameplayService.GetAsync(gameplayUid, cancellationToken);
			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);
			if(tiles == null)
			{
				_notificationService.AddErrorToastMessage("Unable to play the game at this time. Please contact support. Rest assurred, we have been notified about this issue.");

				//Raise an event here with medaitor to contact support

				return RedirectToAction("NotTilesAvailable");
			}

			var model = new PlayMatchGame
			{
				GameUid = gameboard.MatchGame.Uid,
				GameName = gameboard.MatchGame.Name,
				MatchBoard = _mapper.Map<Models.Plays.MatchBoard>(gameboard),
				Tiles = LoadTiles(tiles)
			};

			return View(model);
		}

		[HttpPost]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> SignaturePlay(
			Guid Uid,
			Guid gameplayUid,
			PlayMatchGame form,
			CancellationToken cancellationToken)
		{
			// verify the game
			//Verify if it's not yet game end date
			//verify the gameboard is reserved and is not played
			//Verify the gameplay and if it has been played
			//if played, redirect out.

			//if we get here...
			//Once played and posted, we commit as played for the gameplay as well as the gameplay and put in the 
			//gameplay table if won or not. 
			//also put in the selected tiles (uid/id)

			return new OkObjectResult("Time to play");
		}

		private List<Guid> LoadTiles(IEnumerable<MatchTile> tiles)
		{
			var tileList = new List<Guid>();

			foreach (var tile in tiles)
			{
				tileList.Add(tile.Uid);
			}

			return tileList;
		}

		private void SetSessionVariables(UserProviderDetails userDetails, SignatureModel signature)
		{
			_sessionManager.SetKeyValue(SessionConstants.UserDetails, userDetails.ScreenName);
			_sessionManager.SetKeyValue(SessionConstants.GameplayUid, signature.GamePlayUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.GameUid, signature.GameUid.ToString());
			_sessionManager.SetKeyValue(SessionConstants.Signature, signature.Signature);
		}

		private bool VerifySignature(
			Guid gameplayUid,
			UserProviderDetails userDetails,
			string signatureSession)
				=> _signatureUtility.VerifySignature(
					gameplayUid,
					Convert.ToInt64(userDetails.ExternalProfileId),
					userDetails.ScreenName,
					signatureSession);

	}
}
