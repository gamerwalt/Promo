using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Options;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Campaign = hrvster.Models.Campaign;
using CreateSweep = hrvster.Web.Models.CreateSweep;
using Winner = hrvster.Web.Models.Winner;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepsController : Controller
	{
		private readonly ILogger<SweepsController> _logger;
		private readonly IMediator _mediator;
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly SweepstakeConfiguration _sweepstakeConfiguration;

		public SweepsController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			hrvster.Models.HrvsterUser hrvsterUser,
			IMapper mapper,
			IToastNotification notificationService,
			SweepstakeConfiguration sweepstakeConfiguration)
		{
			_logger = loggerFactory.CreateLogger<SweepsController>();
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
			_notificationService = notificationService;
			_sweepstakeConfiguration = sweepstakeConfiguration;
		}

		public async Task<IActionResult> Index()
		{
			var sweeps = await _mediator.Send(new GetSweepForAccountUidRequest(_hrvsterUser.UserUid));

			return View(sweeps);
		}

		[HttpGet]
		public async Task<IActionResult> CreateNew()
		{
			var promotions = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));
			var anyActivePromotions = promotions.Any(c => c.IsActive());
			var anyWillBeInProgress = promotions.Any(c => c.WillBeInProgress());

			if (!anyActivePromotions && !anyWillBeInProgress)
			{
				_notificationService.AddErrorToastMessage(
					"None of your promotions are active. We need 1 active promotion to continue.");

				return RedirectToAction("Index");
			}

			var model = BuildCreateNewForm(promotions);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNew(CreateNewSweep form)
		{
			var promotions = await _mediator.Send(new GetAllCampaignsForAccount(_hrvsterUser.UserUid));

			var anyActivePromotions = promotions.Any(c => c.IsActive());
			var anyWillBeInProgress = promotions.Any(c => c.WillBeInProgress());

			if (!anyActivePromotions && !anyWillBeInProgress)
			{
				_notificationService.AddErrorToastMessage("None of your promotions are active. We need 1 active promotion to continue.");

				return RedirectToAction("Index");
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new CreateSweepRequest(form.Name, form.StartDate, form.EndDate, form.DrawDate, form.Campaign.CampaignUid, _hrvsterUser.UserUid, _hrvsterUser.WindowsTimeZone));

				_notificationService.AddSuccessToastMessage("Sweepstake created successfully.");

				return RedirectToAction("Index");
			}

			form.Promotions = promotions
				.Where(p => p.IsActive() || p.WillBeInProgress())
				.Select(a => new SelectListItem()
				{
					Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
					Value = a.CampaignUid.ToString()
				});

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View(form);
		}

		[HttpGet]
		public async Task<IActionResult> Create(Guid campaignUid)
		{
			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_notificationService.AddWarningToastMessage($"Could not find campaign with uid: {campaignUid}.");

				return RedirectToAction("Index");
			}

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			var createSweep = new CreateSweep();
			createSweep.Campaign = campaign;
			createSweep.StartDate = campaign.StartDate.AddDays(1);
			createSweep.EndDate = campaign.EndDate.AddDays(-_sweepstakeConfiguration.EndDateDaysAfterStartDate);
			createSweep.DrawDate = campaign.EndDate.AddDays(-_sweepstakeConfiguration.DrawDateDaysAfterEndDate);
			createSweep.Name = $"{campaign.CampaignName}-Sweepstakes";

			return View(createSweep);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromQuery] Guid campaignUid, CreateSweep form)
		{
			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			if (campaign == null)
			{
				_notificationService.AddWarningToastMessage($"Could not find campaign with uid: {campaignUid}.");

				return RedirectToAction("Index");
			}

			if (!campaign.IsActive() && !campaign.WillBeInProgress())
			{
				_notificationService.AddWarningToastMessage("Promotion is not active.");

				return RedirectToAction("View", "Promotions", new { campaignUid = campaignUid });
			}

			if (ModelState.IsValid)
			{
				var request = new NewSweepRequest()
				{
					AccountUid = _hrvsterUser.UserUid,
					CampaignUid = form.Campaign.CampaignUid,
					Name = form.Name,
					StartDate = form.StartDate,
					EndDate = form.EndDate,
					DrawDate = form.DrawDate
				};

				var sweep = await _mediator.Send(new Core.Requests.CreateSweep(request, _hrvsterUser.WindowsTimeZone));

				if (sweep == null)
				{
					_notificationService.AddWarningToastMessage($"Unable to create sweep. Please reach out to support.");

					return RedirectToAction("Create", new { campaignUid = form.Campaign.CampaignUid });
				}

				_notificationService.AddSuccessToastMessage("Sweepstake created successfully.");

				return RedirectToAction("View", new { sweepUid = sweep.SweepUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			form.Campaign = campaign;

			return View(form);
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null) return RedirectToAction("Index");
			if (sweep.Account.AccountUid != _hrvsterUser.UserUid) return RedirectToAction("Index");

			var sweepAndRules = await _mediator.Send(new GetSweepstakeAndRulesRequests(sweepUid));
			if (sweepAndRules == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index");
			}

			var tasks = new List<Task>();
			var entriesTask = _mediator.Send(new GetSweepEntriesRequest(sweepUid));
			var prizesTask = _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));
			var winnersTask = _mediator.Send(new GetSweepWinners(sweepUid));

			tasks.Add(entriesTask);
			tasks.Add(prizesTask);
			tasks.Add(winnersTask);
			await Task.WhenAll(tasks);
			var model = BuildViewSweepstake(sweepAndRules, entriesTask, prizesTask, winnersTask);

			return View(model);
		}

		private ViewSweepstake BuildViewSweepstake(SweepstakeAndRules sweepAndRulles, Task<IEnumerable<hrvster.Models.SweepEntry>> entriesTask, Task<SweepPrize> prizesTask, Task<IEnumerable<hrvster.Models.SweepWinner>> winnersTask)
		{
			return new ViewSweepstake()
			{
				Sweepstake = sweepAndRulles.Sweepstake,
				Rules = sweepAndRulles.Rules,
				DrawRule = sweepAndRulles.DrawRule,
				EntryRule = sweepAndRulles.EntryRule,
				SweepEntries = _mapper.Map<IEnumerable<Models.SweepEntry>>(entriesTask.Result),
				Prizes = _mapper.Map<IEnumerable<Prize>>(prizesTask.Result.Prizes),
				Winners = _mapper.Map<IEnumerable<Winner>>(winnersTask.Result),
				UpdateSweepstake = new UpdateSweepstake
				{
					CampaignUid = sweepAndRulles.Sweepstake.Campaign.CampaignUid,
					SweepUid = sweepAndRulles.Sweepstake.SweepUid,
					Name = sweepAndRulles.Sweepstake.Name,
					StartDate = sweepAndRulles.Sweepstake.StartDate,
					EndDate = sweepAndRulles.Sweepstake.EndDate,
					DrawDate = sweepAndRulles.Sweepstake.DrawDate.GetValueOrDefault()
				}
			};
		}

		[HttpPost]
		public async Task<IActionResult> View(ViewSweepstake form, Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null) return RedirectToAction("Index");
			if (sweep.Account.AccountUid != _hrvsterUser.UserUid) return RedirectToAction("Index");

			var sweepAndRulles = await _mediator.Send(new GetSweepstakeAndRulesRequests(sweepUid));
			if (sweepAndRulles == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index");
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new UpdateSweepstakeRequest(
					sweep,
					sweepUid,
					form.UpdateSweepstake.Name,
					form.UpdateSweepstake.StartDate,
					form.UpdateSweepstake.EndDate,
					form.UpdateSweepstake.DrawDate,
					_hrvsterUser.WindowsTimeZone));

				_notificationService.AddSuccessToastMessage("Sweepstake updated successfully.");

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			var tasks = new List<Task>();
			var entriesTask = _mediator.Send(new GetSweepEntriesRequest(sweepUid));
			var prizesTask = _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));
			var winnersTask = _mediator.Send(new GetSweepWinners(sweepUid));

			tasks.Add(entriesTask);
			tasks.Add(prizesTask);
			tasks.Add(winnersTask);
			await Task.WhenAll(tasks);
			var model = BuildViewSweepstake(sweepAndRulles, entriesTask, prizesTask, winnersTask);

			return View(model);
		}

		private CreateNewSweep BuildCreateNewForm(List<Campaign> promotions)
		{
			var startDate = DateTime.Now;
			var endDate = startDate.AddDays(-_sweepstakeConfiguration.EndDateDaysAfterStartDate);
			var drawDate = endDate.AddDays(-_sweepstakeConfiguration.DrawDateDaysAfterEndDate);

			var model = new CreateNewSweep
			{
				Promotions = promotions
					.Where(p => p.IsActive() || p.WillBeInProgress())
					.Select(a => new SelectListItem()
					{
						Text = $"{a.CampaignName} - {a.StartDate.ToString("d")} to {a.EndDate.ToString("d")}",
						Value = a.CampaignUid.ToString()
					}),
				StartDate = startDate,
				EndDate = endDate,
				DrawDate = drawDate
			};
			return model;
		}
	}
}
