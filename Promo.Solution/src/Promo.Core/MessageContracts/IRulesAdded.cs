using Promo.Core.Models;
using System.Collections.Generic;

namespace Promo.Core.MessageContracts
{
	public interface IRulesAdded
	{
		IList<Rule> Rules { get; }
	}
}
