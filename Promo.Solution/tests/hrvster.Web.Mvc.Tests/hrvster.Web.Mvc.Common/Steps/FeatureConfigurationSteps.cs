using TestUtilities;

namespace hrvster.Web.Mvc.Common.Steps
{
    public static class FeatureConfigurationSteps
    {
        public static TFeature GiveACleanSlate<TFeature>(this IWebFeature<TFeature> feature)
            where TFeature : class
        {
            feature.Client.DefaultRequestHeaders.Clear();
            feature.DeleteAllTableData();

            return feature as TFeature;
        }
    }
}
