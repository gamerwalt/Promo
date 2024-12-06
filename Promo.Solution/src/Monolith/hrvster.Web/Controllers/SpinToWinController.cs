using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.SpinToWinGame;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models.SpinToWin;
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
	public class SpinToWinController : Controller
	{
		private readonly ISessionManager _sessionManager;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;

		public SpinToWinController(
			ISessionManager sessionManager, 
			IMediator mediator,
			IToastNotification notificationService)
		{
			_sessionManager = sessionManager;
			_mediator = mediator;
			_notificationService = notificationService;
		}

		[HttpGet]
		[Route("/SpinToWin")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[Route("/SpinToWin/Create")]
		public async Task<IActionResult> Create(CancellationToken cancellationToken)
		{
			var _hrvsterUser = _sessionManager.GetUserInformation();

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

			var createNewSpinToWin = BuildCreateGameModel(campaigns);
			ModelState.Remove("PromotionUid");

			return View(createNewSpinToWin);
		}

		[HttpPost]
		[Route("/SpinToWin/Create")]
		public async Task<IActionResult> Create(CreateSpinToWinGame form,  CancellationToken cancellationToken)
		{
			var _hrvsterUser = _sessionManager.GetUserInformation();

			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));

			if(ModelState.IsValid)
			{
				var gameUid = Guid.NewGuid();

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

				if(await _mediator.Send(BuildCreateSpinGameRequest(gameUid, form)))
				{
					return RedirectToAction("Index");
				}

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

		private CreateSpinToWinGameRequest BuildCreateSpinGameRequest(Guid gameUid, CreateSpinToWinGame form)
		{
			return new CreateSpinToWinGameRequest
			(
				gameUid,
				form.Name,
				form.Description,
				form.StartDate,
				form.EndDate,
				form.PieGenerateDate,
				form.AcceptTerms,
				form.NumberOfSpins,
				form.MaximumWinForPhysicalPrizes,
				form.MaximumWinForPointPrizes,
				form.PointsForFreePlay,
				form.PromotionUid,
				form.SpreadMainPrizesPerDay
			);
		}


		private static CreateSpinToWinGame BuildCreateGameModel(List<Campaign> campaigns)
			=> new CreateSpinToWinGame()
			{
				Promotions = campaigns
					.Where(p => p.IsActive() || p.WillBeInProgress())
					.Select(a => new SelectListItem()
					{
						Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
						Value = a.CampaignUid.ToString()
					}),
				StartDate = DateTime.Now.AddDays(1),
				EndDate = DateTime.Now.AddDays(30),
				PieGenerateDate = DateTime.Now
			};
	}
}
