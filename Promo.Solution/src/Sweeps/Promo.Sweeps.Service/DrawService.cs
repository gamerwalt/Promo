using MediatR;
using Promo.Sweeps.Core.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Draw.Service
{
	public interface IDrawService
	{
		Task DrawAvailableSweepsAsync(CancellationToken token);
	}

	public class DrawService : IDrawService
	{
		private readonly IMediator _mediator;

		public DrawService(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task DrawAvailableSweepsAsync(CancellationToken token)
		{
			//1. Get the sweeps and the draw rules that have a draw now or that has passed and has not been drawn
			var sweepDrawRules = await _mediator.Send(new GetSweepDrawRules(), token);

			//2. Perform a draw on each of the entries for the sweeps
			foreach(var sweepDrawRule in sweepDrawRules)
			{

			}

			//3. Save the winners and backups
		}
	}
}
