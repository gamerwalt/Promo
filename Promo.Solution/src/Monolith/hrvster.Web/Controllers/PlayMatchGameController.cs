using AutoMapper;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Reporting;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Extensions.ModelExtensions;
using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Reporting;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Web.Models.Plays;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
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
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchGameBoardService _gameBoardService;
		private readonly IMatchGameplayService _gameplayService;
		private readonly HarvesterSettingsOptions _harvesterSettings;
		private readonly IReportingService _reportingService;
		private readonly IMapper _mapper;

		public PlayMatchGameController(
			ILoggerFactory loggerFactory,
			SignatureUtility signatureUtility,
			IWatchTypeProvider watchTypeProvider,
			ISessionManager sessionManager,
			ISignatureRepository signatureRepository,
			IMatchGamesService matchGameService,
			IMatchGameBoardService gameBoardService,
			IMatchGameplayService gameplayService,
			HarvesterSettingsOptions harvesterSettings,
			IReportingService reportingService,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<PlayMatchGameController>();
			_signatureUtility = signatureUtility;
			_watchTypeProvider = watchTypeProvider;
			_sessionManager = sessionManager;
			_signatureRepository = signatureRepository;
			_matchGameService = matchGameService;
			_gameBoardService = gameBoardService;
			_gameplayService = gameplayService;
			_harvesterSettings = harvesterSettings;
			_reportingService = reportingService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("/PlayMatchGame/matchgame")]
		public async Task<IActionResult> Play(string signature, CancellationToken cancellationToken)
		{
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "Signature", signature }
			}))
			{
				_logger.LogInformation("Received a request to play a Match game with signature {signature}", signature);

				var signatureFromDb = await _signatureRepository.GetAsync(signature, cancellationToken);

				if (signatureFromDb == null) return RedirectToAction(nameof(SignatureFailed), new
				{
					Uid = Guid.NewGuid(),
					gameplayUid = Guid.NewGuid()
				});

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

			var signature = await _signatureRepository.GetAsync(signatureSession, userDetails.ScreenName, userDetails.ExternalProfileId);

			if (signature == null)
			{
				return RedirectToAction("SignatureFailed", new
				{
					Uid = Guid.NewGuid(),
					gameplayUid = Guid.NewGuid()
				});
			}

			if (signature.ExternalUserId != userDetails.ExternalProfileId ||
				signature.ExternalUserHandle != userDetails.ScreenName)
			{
				return RedirectToAction("SignatureFailed", new
				{
					Uid = Guid.NewGuid(),
					gameplayUid = Guid.NewGuid()
				});
			}

			if (DateTime.UtcNow > signature.ExpiryDate)
			{

				return RedirectToAction("SignatureExpired", new
				{
					Uid = Guid.NewGuid(),
					gameplayUid = Guid.NewGuid()
				});
			}

			_logger.LogInformation("Signature with uid {signatureUid} not expired.", signature.Uid);

			_logger.LogInformation("Verifying the signature with details received from Twitter");

			if (!VerifySignature(signature.GamePlayUid, userDetails, signatureSession))
			{
				return RedirectToAction("SignatureFailed", new
                {
                    Uid = Guid.NewGuid(),
                    gameplayUid = Guid.NewGuid()
                });
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
			_sessionManager.RemoveKeyValue(SessionConstants.Signature);
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", Uid },
				{ "GamePlayUid", gameplayUid }
			}))

				_logger.LogInformation("Verifying the game.");

			if (!await _matchGameService.GamePlayableAsync(Uid, cancellationToken))
			{
				return RedirectToAction("GameUnplayable", new
				{
					Uid = Uid,
					gameplayUid = gameplayUid
				});
			}

			_logger.LogInformation("Verifying the gameplay and the gameboard");
			if (!await _gameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, cancellationToken))
			{
				return RedirectToAction("BoardUnplayable", new
				{
					Uid = Uid,
					gameplayUid = gameplayUid
				});
			}

			var gameplay = await _gameplayService.GetAsync(gameplayUid, cancellationToken);
			var gameboard = await _gameBoardService.GetGameBoardAsync(gameplay.MatchBoard.Uid, cancellationToken);
			var tiles = await _gameBoardService.GetTilesAsync(gameboard.Uid, cancellationToken);
			if (tiles == null)
			{
				return RedirectToAction("NoTilesAvailable", new
				{
					Uid = Uid,
					gameplayUid = gameplayUid
				});
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
				return RedirectToAction("GameUnplayable", new
				{
					Uid = Uid,
					gameplayUid = gameplayUid
				});
			}

			_logger.LogInformation("Verifying the gameplay and the gameboard");

			if (!await _gameBoardService.IsBoardPlayableByGameplayAsync(gameplayUid, cancellationToken))
			{
				return RedirectToAction("BoardUnplayable", new
                {
                    Uid = Uid,
                    gameplayUid = gameplayUid
				});
			}

			if (ModelState.IsValid)
			{
				var screenName = _sessionManager.GetValue(SessionConstants.UserExternalScreenName);
				var externalId = _sessionManager.GetValue(SessionConstants.UserExternalId);

				await _reportingService.SaveMatchGameAsync(
					Uid, 
					gameplayUid, 
					form.BoardUid, 
					ExternalClientType.Twitter, 
					screenName,
					externalId,
					cancellationToken);

				return await ReturnWinOrLossAsync(Uid, gameplayUid, form, cancellationToken);
			}

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
				Tiles = GetMatchTiles(tiles),
				SelectedTiles = LoadTileStrings(gameplay.SelectedTiles)
			};

			return View(model);
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay{gameplayUid:Guid}/BoardUnplayable")]
		public async Task<IActionResult> BoardUnplayable()
		{
			return View();
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay{gameplayUid:Guid}/GameUnplayable")]
		public async Task<IActionResult> GameUnplayable()
		{
			return View();
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay{gameplayUid:Guid}/NoTilesAvailable")]
		public async Task<IActionResult> NoTilesAvailable()
		{
			return View();
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/gameplay{gameplayUid:Guid}/SignatureExpired")]
		public async Task<IActionResult> SignatureExpired()
		{
			return View();
		}

		[HttpGet]
		[Route("/PlayMatchGame/SignatureFailed")]
		public async Task<IActionResult> SignatureFailed()
		{
			return View();
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
			_sessionManager.SetKeyValue(SessionConstants.UserExternalId, userDetails.ExternalProfileId);
			_sessionManager.SetKeyValue(SessionConstants.UserExternalScreenName, userDetails.ScreenName);
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

		private async Task<IActionResult> ReturnWinOrLossAsync(
			Guid Uid, Guid gameplayUid, PlayMatchGame form, CancellationToken cancellationToken)
		{
			var screenName = _sessionManager.GetValue(SessionConstants.UserExternalScreenName);
			var signature = _sessionManager.GetValue(SessionConstants.Signature);

			if (await _gameplayService.IsAWinAsync(gameUid: Uid, gameplayUid, form.SelectedTiles, cancellationToken))
			{
				await SetGameplayAsWonAsync(gameplayUid, form, cancellationToken);

				if (!string.IsNullOrWhiteSpace(screenName) && !string.IsNullOrWhiteSpace(signature))
				{
					await _signatureRepository.SetAsCompletedAsync(
					signature,
					screenName,
					cancellationToken);
				}

				return RedirectToAction("MatchGameWin", new { Uid = Uid, gameplayUid = gameplayUid });
			}

			await SetGameplayAsLossAsync(gameplayUid, form, cancellationToken);

			if (!string.IsNullOrWhiteSpace(screenName) && !string.IsNullOrWhiteSpace(signature))
			{
				await _signatureRepository.SetAsCompletedAsync(
				signature,
				screenName,
				cancellationToken);
			}

			return RedirectToAction("MatchGameLoss", new { Uid = Uid, gameplayUid = gameplayUid });
		}

		private List<Models.MatchTile> GetMatchTiles(IEnumerable<MatchTile> tiles)
		{
			var matchTiles = new List<Models.MatchTile>();

			foreach (var tile in tiles)
			{
				matchTiles.Add(new Models.MatchTile
				{
					Uid = tile.Uid,
					ImageUrl = tile.Image.LocationUrl
				});
			}

			return matchTiles;
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
