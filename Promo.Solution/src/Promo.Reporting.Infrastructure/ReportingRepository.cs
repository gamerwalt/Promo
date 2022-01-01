using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Reporting.Infrastructure
{
	public interface IReportingRepository
	{
		Task DoSomething();
	}

	public class ReportingRepository : IReportingRepository
	{
		public Task DoSomething()
		{
			return Task.CompletedTask;
		}
	}
}
