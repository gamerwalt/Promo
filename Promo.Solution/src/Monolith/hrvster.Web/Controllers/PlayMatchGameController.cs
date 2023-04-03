using AutoMapper;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Common;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using hrvster.Web.Models.Plays;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
			_mapper = mapper;
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}")]
		public async Task<IActionResult> Play(Guid Uid, string signature, CancellationToken cancellationToken)
		{
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", Uid },
				{ "Signature", signature }
			}))
			{
				_logger.LogInformation("Received a request to play a Match game with uid {uid}", Uid);

				_logger.LogInformation("Requesting the user to logon to twitter.");

				_sessionManager.SetKeyValue(SessionConstants.GameUid, Uid.ToString());
				_sessionManager.SetKeyValue(SessionConstants.Signature, signature);

				var authorizationUrl = await _watchTypeProvider.GetAuthorizationUrl(Constants.TwitterProvider, Request.Scheme, Request.Host.Value, $"/PlayMatchGame/Validate{Constants.TwitterProvider}Auth");


				return new RedirectResult(authorizationUrl);
			}
		}

		public IActionResult FailedAuthValidation()
		{
			return View();
		}


		public async Task<IActionResult> ValidateTwitterAuth(Guid tweetinvi_auth_request_id, string oauth_token, string oauth_verifier)
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
			var gameUidSession = _sessionManager.GetValue(SessionConstants.GameUid);

			var signature = await _signatureRepository.GetAsync(
				gameUidSession.ToString(), 
				signatureSession, 
				userDetails.ScreenName);

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

			_logger.LogInformation("Verifying the signature with details received from Twitter");

			if (!VerifySignature(signature.Uid, userDetails, signatureSession, gameUidSession))
			{
				return RedirectToAction("SignatureFailed");
			}

			if (await SignatureIsExpiredAsync(gameUidSession, signatureSession, userDetails.ScreenName))
			{
				return RedirectToAction("SignatureExpired");
			}

			_sessionManager.SetKeyValue(SessionConstants.Signature, signatureSession);
			_sessionManager.SetKeyValue(SessionConstants.UserDetails, userDetails.ScreenName);

			//add another unique uid to the signature so the SignaturePlay isn't that vulnerable
			var newUid = Guid.NewGuid();
			//Update the previous signature with the new uid and update the signature itself
			var signatureToUpdate = await _signatureRepository.GetByUidAsync(signature.Uid);
			//Todo: Check if it exists, if not, Get out

			var UidToUpdate = signatureToUpdate.Uid;
			var idToUpdate = signatureToUpdate.Id;

			signatureToUpdate = signature;
			signatureToUpdate.Uid = newUid;
			signatureToUpdate.Signature = _signatureUtility.CreateSignature(
				newUid,
				Convert.ToInt64(signatureToUpdate.ExternalUserId),
				signatureToUpdate.ExternalUserHandle, 
				signatureToUpdate.GameUid);

			_sessionManager.SetKeyValue(SessionConstants.SignatureUid, newUid.ToString());

			await _signatureRepository.UpdateSignatureAsync(signatureToUpdate, UidToUpdate, idToUpdate);

			return RedirectToAction("SignaturePlay", new { Uid = gameUidSession, Sig = signatureToUpdate.Signature, externalId = userDetails.ExternalProfileId, screenName = userDetails.ScreenName });
		}

		[HttpGet]
		[Route("/PlayMatchGame/{Uid:Guid}/SignaturePlay/{Sig}/ScreenName/{screenName}/Id/{externalId}")]
		public async Task<IActionResult> SignaturePlay(
			Guid Uid, string Sig, string screenName, string externalId, CancellationToken cancellationToken)
		{
			using (_logger.BeginScope(new Dictionary<string, object>
			{
				{ "GameUid", Uid },
			}))

			_logger.LogInformation("Received a request play a match game.");

			var signatureUidInSession = _sessionManager.GetValue(SessionConstants.SignatureUid);

			var signature = await _signatureRepository.GetByUidAsync(Guid.Parse(signatureUidInSession));
			if (signature == null)
			{
				_notificationService.AddErrorToastMessage("Signature has expired.");

				return RedirectToAction("SignatureExpired");
			}

			if(signature.Signature != Sig)
			{
				_notificationService.AddErrorToastMessage("Cannot play.");

				return RedirectToAction("CannotPlay");
			}

			if (DateTime.UtcNow > signature.ExpiryDate)
			{
				_notificationService.AddErrorToastMessage("Signature has expired.");

				return RedirectToAction("SignatureExpired");
			}

			var numberOfPlays = signature.GamePlayCount;

			if (!_signatureUtility.VerifySignature(Guid.Parse(signatureUidInSession), Convert.ToInt64(externalId), screenName, Uid, Sig))
			{
				_notificationService.AddErrorToastMessage("Signature has failed.");

				return RedirectToAction("SignatureFailed");
			}

			if (await SignatureIsExpiredAsync(Uid.ToString(), Sig, screenName))
			{
				_notificationService.AddErrorToastMessage("Signature has expired.");

				return RedirectToAction("SignatureExpired");
			}

			var game = _matchGameService.GetMatchGame(Uid);
			if(game == null)
			{
				_notificationService.AddErrorToastMessage("Invalid game.");

				return RedirectToAction("InvalidGame");
			}

			if(DateTime.UtcNow > game.EndDateUtc || DateTime.UtcNow < game.StartDateUtc)
			{
				_notificationService.AddErrorToastMessage("Cannot play at this time.");

				return RedirectToAction("InvalidGameTime");
			}

			var board = await _gameBoardService.GetRandomBoardAsync(gameUid: Uid, cancellationToken);

			var model = new PlayMatchGame
			{
				GameUid = Uid,
				GameName = game.Name,
				Signature = Sig,
				NumberOfGamePlays = numberOfPlays,
				MatchBoard = _mapper.Map<MatchBoard>(board),
				SelectedTiles = new List<Guid>()
			};

			return View(model);
		}

		private async Task<bool> SignatureIsExpiredAsync(string gameUidSession, string signatureSession, string screenName)
		{
			var signature = await _signatureRepository.GetAsync(gameUidSession, signatureSession, screenName);

			if (signature == null) return true;

			return DateTime.UtcNow > signature.ExpiryDate;
		}

		private bool VerifySignature(
			Guid signatureUid, 
			UserProviderDetails userDetails, 
			string signatureSession, 
			string gameUidSession)
				=> _signatureUtility.VerifySignature(
					signatureUid,
					Convert.ToInt64(userDetails.ExternalProfileId),
					userDetails.ScreenName,
					Guid.Parse(gameUidSession),
					signatureSession);

	}
}
