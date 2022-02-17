using Promo.Core.Models;
using System.Collections.Generic;

namespace Promo.Core.MessageContracts
{
	public interface IRulesAddedAccepted
	{
		List<Rule> Rules { get; }
	}
}
