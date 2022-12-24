using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Web.Mvc.Common.Steps.Sweeps
{
	public static class SweepsControllerSteps
	{
		public static Feature WhenILoadTheSweepsPage(this Feature feature)
		{
			Task.Run(async () => feature.Response = await feature.Client.GetAsync(RouteList.SweepsPage)).Wait();

			return feature;
		}
	}
}
