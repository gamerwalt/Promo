using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promo.Clients.External;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class CampaignsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CampaignsController(
			IIdentityClient identityClient,
			IMediator mediator)
		{
			_mediator = mediator;
		}
	}
}
