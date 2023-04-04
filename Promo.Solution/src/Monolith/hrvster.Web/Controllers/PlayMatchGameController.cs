using AutoMapper;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Extensions.ModelExtensions;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Web.Models.Plays;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using NToastNotify.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		private readonly HarvesterSettingsOptions _harvesterSettings;
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
			HarvesterSettingsOptions harvesterSettings,
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
			_harvesterSettings = harvesterSettings;
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
				GameplayUid = gameplay.Uid,
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
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", Uid },
				{ "GamePlayUid", gameplayUid }
			}))

			_logger.LogInformation("Posted gameplay with the following. {Content}", form.ToJsonString());
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

			if (ModelState.IsValid)
			{
				return await ReturnWinOrLossAsync(Uid, gameplayUid, form, cancellationToken);
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return Redirect(_harvesterSettings.HrvsterUrl);
		}

		[HttpGet]
		[Route("/PlayMatchGame/win/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> MatchGameWin(
			Guid Uid,
			Guid gameplayUid,
			CancellationToken cancellationToken)
		{
			var gameplay = await _gameplayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && !gameplay.WinIndicator)
			{
				_notificationService.AddWarningToastMessage("Unable to locate your win. Something went wrong.");

				return Redirect(_harvesterSettings.HrvsterUrl);
			}

			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);
			
			var model = new GameplayWin
			{
				GameplayUid = gameplay.Uid,
				GameUid = gameboard.MatchGame.Uid,
				GameName = gameboard.MatchGame.Name,
				MatchBoard = _mapper.Map<Models.Plays.MatchBoard>(gameboard),
				Tiles = GetMatchTiles(tiles),
				SelectedTiles = LoadTileStrings(gameplay.SelectedTiles)
			};

			return View(model);
		}

		private List<Models.MatchTile> GetMatchTiles(IEnumerable<MatchTile> tiles)
		{
			var matchTiles = new List<Models.MatchTile>();

			foreach(var tile in tiles)
			{
				matchTiles.Add(new Models.MatchTile
				{
					Uid = tile.Uid,
					ImageUrl = tile.Prize.Image.LocationUrl
				});
			}

			return matchTiles;
		}

		[HttpGet]
		[Route("/PlayMatchGame/loss/{Uid:Guid}/gameplay/{gameplayUid:Guid}")]
		public async Task<IActionResult> MatchGameLoss(
			Guid Uid,
			Guid gameplayUid,
			CancellationToken cancellationToken)
		{
			var gameplay = await _gameplayService.GetAsync(gameplayUid, cancellationToken);

			if (!gameplay.PlayedIndicator && !gameplay.MatchBoard.PlayedIndicator && gameplay.WinIndicator)
			{
				_notificationService.AddWarningToastMessage("Unable to locate your win. Something went wrong.");

				return Redirect(_harvesterSettings.HrvsterUrl);
			}

			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);

			var model = new GameplayLoss
			{
				GameplayUid = gameplay.Uid,
				GameUid = gameboard.MatchGame.Uid,
				GameName = gameboard.MatchGame.Name,
				MatchBoard = _mapper.Map<Models.Plays.MatchBoard>(gameboard),
				Tiles = LoadTiles(tiles),
				SelectedTiles = LoadTileStrings(gameplay.SelectedTiles)
			};

			return View(model);
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

		private List<Guid> LoadTileStrings(string tileUids)
		{
			var tiles = new List<Guid>();

			var tileList = tileUids.Split(',').ToList();

			foreach (var tileUid in tileList)
			{
				tiles.Add(Guid.Parse(tileUid));
			}

			return tiles;
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

		private async Task<IActionResult> ReturnWinOrLossAsync(Guid Uid, Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			if (await _gameplayService.IsAWinAsync(gameUid: Uid, gameplayUid, form.SelectedTiles, cancellationToken))
			{
				await SetGameplayAsWonAsync(gameplayUid, form, cancellationToken);

				return RedirectToAction("MatchGameWin", new { Uid = Uid, gameplayUid = gameplayUid });
			}

			await SetGameplayAsLossAsync(gameplayUid, form, cancellationToken);

			return RedirectToAction("MatchGameLoss", new { Uid = Uid, gameplayUid = gameplayUid });
		}

		private async Task SetGameplayAsWonAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was won with the tiles {selectedTiles}",
				gameplayUid,
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a win.");

			await _gameplayService.MarkAsPlayedAndWinAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		}

		private async Task SetGameplayAsLossAsync(Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			_logger.LogInformation("The gameplay with uid {gameplayUid} was lost with the tiles {selectedTiles}",
				gameplayUid, 
				form.SelectedTiles);

			_logger.LogInformation("Setting the gameplay as a loss.");

			await _gameplayService.MarkAsPlayedAndLostAsync(gameplayUid, form.SelectedTiles, cancellationToken);
		}

	}
}
