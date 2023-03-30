using AutoMapper;
using hrvster.ApplicationServices.MatchGames;
using hrvster.Core.Authentication;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class MatchPrizesController : Controller
	{
		private readonly ILogger<MatchPrizesController> _logger;
		private readonly IMatchPrizeService _matchPrizeService;
		private readonly IMatchGamesService _matchGameService;
		private readonly IMatchImageService _matchImageService;
		private readonly IToastNotification _notificationService;
		private readonly IMapper _mapper;

		public MatchPrizesController(
			ILoggerFactory loggerFactory,
			IMatchPrizeService matchPrizeService,
			IMatchGamesService matchGamesService,
			IMatchImageService matchImageService,
			IToastNotification notificationService,
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<MatchPrizesController>();
			_matchPrizeService = matchPrizeService;
			_matchGameService = matchGamesService;
			_matchImageService = matchImageService;
			_notificationService = notificationService;
			_mapper = mapper;
		}

		[HttpGet]
		[Route("/matchprizes/matchgame/{Uid:Guid}/create")]
		public async Task<IActionResult> Create(
			Guid Uid, string actionName, string controllerName, CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);
				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var matchImages = await _matchImageService.GetForMatchGameAsync(Uid, cancellationToken);

			var model = new CreateMatchPrize
			{
				MatchGameName = matchGame.Name,
				MatchGameUid = matchGame.Uid,
				MatchImages = _mapper.Map<List<MatchImage>>(matchImages),
				ControllerName = controllerName ?? "MatchGames",
				ActionName = actionName ?? "View"
			};

			return View(model);
		}

		[HttpPost]
		[Route("/matchprizes/matchgame/{Uid:Guid}/create")]
		public async Task<IActionResult> Create(
			Guid Uid, string actionName, string controllerName, CreateMatchPrize form, CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);
				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			if (ModelState.IsValid)
			{
				_notificationService.AddSuccessToastMessage("Save now.");

				await _matchPrizeService.CreateAsync(Uid, form.Name, form.Description, form.Value, form.Quantity, form.ImageUid, cancellationToken);

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var matchImages = await _matchImageService.GetForMatchGameAsync(Uid, cancellationToken);

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			var model = new CreateMatchPrize
			{
				MatchGameName = matchGame.Name,
				MatchGameUid = matchGame.Uid,
				MatchImages = _mapper.Map<List<MatchImage>>(matchImages),
				ControllerName = controllerName ?? "MatchGames",
				ActionName = actionName ?? "View"
			};

			return View(model);
		}

		[HttpGet]
		[Route("/matchprizes/{prizeUid:Guid}/matchgame/{Uid:Guid}/edit")]
		public async Task<IActionResult> Update(
			Guid prizeUid, Guid Uid, string controllerName, string actionName, CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);
				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var matchPrize = await _matchPrizeService.GetPrizeAsync(prizeUid, cancellationToken);
			if (matchPrize == null)
			{
				_logger.LogWarning("Could not find the match prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Could not find the prize.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var model = await BuildUpdateMatchPrize(
				prizeUid, Uid, controllerName, actionName, matchGame, matchPrize, cancellationToken);

			return View(model);
		}
		

		[HttpPost]
		[Route("/matchprizes/{prizeUid:Guid}/matchgame/{Uid:Guid}/edit")]
		public async Task<IActionResult> Update(
			Guid prizeUid, 
			Guid Uid, 
			string controllerName, 
			string actionName,
			UpdateMatchPrize form,
			CancellationToken cancellationToken)
		{
			var matchGame = await _matchGameService.GetMatchGameAsync(Uid, cancellationToken);
			if (matchGame == null)
			{
				_logger.LogWarning("Could not find the match game with uid {uid}", Uid);
				_notificationService.AddWarningToastMessage("Could not find the match game.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			var matchPrize = await _matchPrizeService.GetPrizeAsync(prizeUid, cancellationToken);
			if (matchPrize == null)
			{
				_logger.LogWarning("Could not find the match prize with uid {prizeUid}", prizeUid);

				_notificationService.AddWarningToastMessage("Could not find the prize.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			if(ModelState.IsValid)
			{
				_logger.LogInformation("Updating the match prize with uid {matchPrizeUid} and name {prizeName}",
					prizeUid,
					matchPrize.Name);
				
				await _matchPrizeService.UpdateAsync(
					prizeUid, 
					Uid, 
					form.Name, 
					form.Description, 
					form.Quantity, 
					form.Value, 
					form.ImageUid, 
					cancellationToken);

				_notificationService.AddSuccessToastMessage("Match prize updated successfully.");

				_logger.LogInformation("Match prize with uid {matchPrizeUid} and name {prizeName} updated successfully.",
					prizeUid,
					matchPrize.Name);

				return RedirectToAction(actionName ?? "View", controllerName ?? "MatchGames", new { Uid = Uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction(
				"Update", 
				"MatchPrizes", 
				new { 
					prizeUid = prizeUid, 
					Uid = Uid, 
					actionName = actionName, 
					controllerName = controllerName 
				});
		}

		private async Task<UpdateMatchPrize> BuildUpdateMatchPrize(
			Guid prizeUid,
			Guid Uid,
			string controllerName,
			string actionName,
			Games.Match.Models.MatchGame matchGame,
			Games.Match.Models.MatchPrize matchPrize,
			CancellationToken cancellationToken)
		{
			var matchImages = await _matchImageService.GetForMatchGameAsync(Uid, cancellationToken);
			var matchPrizes = await _matchPrizeService.GetAllForMatchGameAsync(Uid, cancellationToken);
			var matchImagesUpdated = matchImages
				.Where(i => !matchPrizes.Any(p => p.Image.Uid == i.Uid)).ToList();
			matchImagesUpdated.Add(matchImages.FirstOrDefault(mi => mi.Uid == matchPrizes.FirstOrDefault(c => c.Uid == prizeUid).Image.Uid));

			var model = new UpdateMatchPrize
			{
				Description = matchPrize.Description,
				Name = matchPrize.Name,
				MatchPrizeUid = prizeUid,
				Quantity = matchPrize.Quantity,
				Value = matchPrize.Value,
				ImageUid = matchPrize.Image.Uid,
				MatchGameName = matchGame.Name,
				MatchGameUid = matchGame.Uid,
				MatchImages = _mapper.Map<List<MatchImage>>(matchImagesUpdated),
				ControllerName = controllerName ?? "MatchGames",
				ActionName = actionName ?? "View",
				PreviousQuantity = matchPrize.Quantity
			};
			return model;
		}
	}
}
