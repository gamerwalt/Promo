using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	[Authorize]
	public class RulesController : ControllerBase
	{
		private readonly IMediator _mediator;

		public RulesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("rules")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<IEnumerable<Rule>>> AddRule([FromBody] List<NewRuleRequest> ruleRequests)
		{
			var rules = BuildRulesFromRequest(ruleRequests);

			var rulesAdded = await _mediator.Send(new AddCampaignRules(rules));

			return new OkObjectResult(rulesAdded);
		}

		private static List<Rule> BuildRulesFromRequest(List<NewRuleRequest> ruleRequests)
		{
			var rules = new List<Rule>();
			foreach (var rule in ruleRequests)
			{
				rules.Add(new Rule
				{
					RuleUid = rule.RuleUid,
					CampaignUid = rule.CampaignUid,
					SweepUid = rule.SweepUid,
					WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), rule.WatchTypeCode),
					WatchValue = rule.WatchValue,
					ActiveInd = rule.ActiveInd
				});
			}

			return rules;
		}

		[HttpGet("rules/hashtags")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<IEnumerable<Rule>>> GetRulesByHashtags([FromBody] List<string> hashTags)
		{
			var rules = await _mediator.Send(new GetRulesForHashTags(hashTags));

			return new OkObjectResult(rules);
		}

		[HttpGet("rule/{ruleUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Rule))]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<Rule>> GetRule([FromRoute] Guid ruleUid)
		{
			var rule = await _mediator.Send(new GetRule(ruleUid));

			return new OkObjectResult(rule);
		}

		[HttpGet("rules/{watchTypeCode}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Rule>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<IEnumerable<Rule>>> GetRules([FromRoute] string watchTypeCode)
		{
			var unaddedRules = await _mediator.Send(new GetRules(watchTypeCode));

			return new OkObjectResult(unaddedRules);
		}

		[HttpPost("rule/{ruleUid:Guid}/added")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Rule))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<Rule>> UpdateRuleAsAded([FromRoute] Guid ruleUid)
		{
			var rule = await _mediator.Send(new UpdateRuleAsAdded(ruleUid));

			return new OkObjectResult(rule);
		}
	}
}
