using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Core.Requests;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Rules.API.Controllers
{
	[ApiController]
	[Route("v1")]
	public class EntryRuleController : Controller
	{
		private readonly IMediator _mediator;

		public EntryRuleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		/// <summary>
		/// Add a new entry rule
		/// </summary>
		/// <param name="entryRule"></param>
		/// <returns></returns>
		[HttpPost("entryrule")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EntryRule))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Authorize]
		public async Task<ActionResult<Rule>> AddRule([FromBody] NewEntryRuleRequest entryRule)
		{
			var entryRuleAdded = await _mediator.Send(new CreateEntryRule(entryRule));

			return new OkObjectResult(entryRuleAdded);
		}

		/// <summary>
		/// Get entry rule
		/// </summary>
		/// <param name="campaignUid"></param>
		/// <param name="sweepUid"></param>
		/// <returns></returns>
		[HttpGet("campaign/{campaignUid:Guid}/sweeps/{sweepUid:Guid}/entryrule")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EntryRule))]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<EntryRule>> GetRuleByCampaignAndSweep([FromRoute] Guid campaignUid, [FromRoute] Guid sweepUid)
		{
			var entryRule = await _mediator.Send(new GetCampaignSweepEntryRule(campaignUid, sweepUid));

			return new OkObjectResult(entryRule);
		}
	}
}
