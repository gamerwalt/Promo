using AutoMapper;
using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Extensions.LogExtensions;
using hrvster.Core.Authentication;
using hrvster.Models;
using hrvster.Template1.Web.Models;
using hrvster.Template1.Web.Models.Query;
using hrvster.Template1.Web.Validators;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Infrastructure.Customizations;
using hrvster.Templates.Models.Extensions;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace hrvster.Template1.Web.Controllers;

public class MembersController : Controller
{
	private readonly ILogger<MembersController> _logger;
	private readonly IGameService _gameService;
	private readonly IMapper _mapper;
	private readonly IMemberRepository _memberRepository;
	private readonly IMemberProfileRepository _memberProfileRepository;
	private readonly ISessionManager _sessionManager;

	public MembersController(
		ILoggerFactory loggerFactory,
		IGameService gameService,
		IMapper mapper,
		IMemberRepository memberRepository,
		IMemberProfileRepository memberProfileRepository,
		ISessionManager sessionManager)
	{
		_logger = loggerFactory.CreateLogger<MembersController>();
		_gameService = gameService;
		_mapper = mapper;
		_memberRepository = memberRepository;
		_memberProfileRepository = memberProfileRepository;
		_sessionManager = sessionManager;
	}

	[Authorize]
	[HttpGet("Members/Create")]
	public async Task<IActionResult> CreateMember([FromQuery] MemberCreationQuery query)
	{
		var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
		if (string.IsNullOrWhiteSpace(accountUidString))
		{
			_logger.LogWarning("AccountUid is empty!");
			return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
		}

		var accountUid = Guid.Parse(accountUidString);
		if (accountUid == Guid.Empty)
		{
			_logger.LogWarning("AccountUid is empty!");
			return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
		}

		using (_logger.CreateMemberPageLogScope(query.RuleUid, query.GameUid, query.ExternalClient, accountUid))
		{
			if (!IsValidGameSession(query.GameType))
			{
				_logger.LogWarning("Game type empty!");
				return ErrorRedirect("Game type empty!", StatusCodes.Status400BadRequest);
			}

			var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(query.GameType), query.GameUid);

			if (!IsGameActive(gameDetails))
			{
				_logger.LogWarning("Either game has not started or it has ended.");

				return ErrorRedirect("Either game has not started or it has ended.", StatusCodes.Status400BadRequest);
			}

			//does this user exist?
			//if this user exists, then we need to redirect to the game
			var principal = User.Identity;
			var externalIdClaim = User.Claims.Where(c => c.Type == HrvsterClaimTypes.ExternalProfileId).FirstOrDefault();
			var externalId = externalIdClaim?.Value;

			if(externalId != null)
			{
				var memberProfile = await _memberProfileRepository.GetByExternalProfileIdAsync(accountUid, externalId);
				if (memberProfile != null)
				{
					_logger.LogInformation("Member already exists. Returning to play the game.");

					return RedirectToGame(query.GameType, query.GameUid, query.RuleUid, gameDetails.CampaignUid, query.ExternalClient);
				}
			}

			return View(new CreateMember
			{
				GameUid = query.GameUid,
				RuleUid = query.RuleUid,
				ExternalClient = query.ExternalClient,
				GameName = gameDetails.Name,
				GameStartDate = gameDetails.StartDate,
				GameEndDate = gameDetails.EndDate,
				GameType = query.GameType
			});
		}
	}

	[Authorize]
	[HttpPost("Members/Create")]
	public async Task<IActionResult> CreateMember([FromQuery] MemberCreationQuery query, CreateMember form)
	{
		_logger.LogInformation("Posting for Create Member.");
		var gameDetails = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(query.GameType), query.GameUid);

		var validator = new CreateMemberValidator();
		var result = validator.Validate(form);

		if (result.IsValid)
		{
			var accountUidString = _sessionManager.GetValue(SessionConstants.AccountUid);
			if (string.IsNullOrWhiteSpace(accountUidString))
			{
				_logger.LogWarning("AccountUid is empty!");
				return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
			}

			var accountUid = Guid.Parse(accountUidString);
			if (accountUid == Guid.Empty)
			{
				_logger.LogWarning("AccountUid is empty!");
				return ErrorRedirect("AccountUid is empty!", StatusCodes.Status400BadRequest);
			}

			using (_logger.CreateMemberPagePostLogScope(query.RuleUid, query.GameUid, query.ExternalClient, accountUid))
			{
				if (!IsValidGameSession(query.GameType))
				{
					_logger.LogWarning("Game type empty!");
					return ErrorRedirect("Game type empty!", StatusCodes.Status400BadRequest);
				}

				if (!IsGameActive(gameDetails))
				{
					_logger.LogWarning("Either game has not started or it has ended.");
					return ErrorRedirect("Either game has not started or it has ended.", StatusCodes.Status400BadRequest);
				}

				if (ModelState.IsValid)
				{
					var memberProfile = User.Claims.ToMemberProfile();
					var newMember = _mapper.Map<Member>(form);

					await CreateNewMember(accountUid, newMember, memberProfile);

					return RedirectToGame(query.GameType, query.GameUid, query.RuleUid, gameDetails.CampaignUid, query.ExternalClient);
				}

				return View(form);
			}
		}

		foreach(var error in result.Errors)
		{
			ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
		}
		
		return View(new CreateMember
		{
			GameUid = query.GameUid,
			RuleUid = query.RuleUid,
			ExternalClient = query.ExternalClient,
			GameName = gameDetails.Name,
			GameStartDate = gameDetails.StartDate,
			GameEndDate = gameDetails.EndDate,
			GameType = query.GameType
		});
	}

	private static bool IsGameActive(GameDetail gameDetails)
	{
		return true;
		//return DateTime.UtcNow <= gameDetails.EndDateUtc && DateTime.UtcNow >= gameDetails.StartDateUtc;
	}

	private static bool IsValidGameSession(string gameType)
	{
		return !string.IsNullOrWhiteSpace(gameType);
	}

	private async Task CreateNewMember(Guid accountUid, Member newMember, MemberProfile memberProfile)
	{
		_logger.LogInformation("Creating new member.");

		newMember.Uid = Guid.NewGuid();
		memberProfile.Uid = Guid.NewGuid();
		memberProfile.Member = newMember;
		memberProfile.ProfileType = ProfileType.Twitter;

		await _memberRepository.CreateMemberAsync(accountUid, newMember);
		await _memberProfileRepository.CreateMemberProfileAsync(accountUid, memberProfile);

		_logger.LogInformation("New member created.");
	}

	private IActionResult RedirectToGame(string gameType, Guid gameUid, Guid ruleUid, Guid campaignUid, string externalClient)
	{
		var controllerName = gameType == GameTypesEnum.Sweeps.ToString()
			? "Sweepstakes"
			: "Pick3Game";

		var actionName = "PrePlay";

		return RedirectToAction(actionName, controllerName, new { gameUid, ruleUid, externalClient, campaignUid });
	}

	private IActionResult ErrorRedirect(string reason, int statusCode)
	{
		return RedirectToActionPermanent(
			"HandleErrorCode",
			"Error",
			new { reason, statusCode });
	}
}


