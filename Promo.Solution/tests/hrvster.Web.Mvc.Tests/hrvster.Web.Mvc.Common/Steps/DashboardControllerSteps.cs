using TestUtilities;

namespace hrvster.Web.Mvc.Common.Steps
{
    public static class DashboardControllerSteps
    {
        public static Feature WhenILoadTheDashboardPage(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync($"/Dashboard")).Wait();

            return feature;
        }
    }
}
