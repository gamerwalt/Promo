using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.MessageContracts
{
	public interface IRulesAddedRejected
	{
		List<Rule> Rules { get; }
		string Reason { get; }
	}
}
