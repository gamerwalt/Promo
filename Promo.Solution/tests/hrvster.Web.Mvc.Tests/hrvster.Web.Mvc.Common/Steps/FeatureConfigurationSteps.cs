using hrvster.Core;
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

        public static Feature AndExtractTheAntiForgeryValues(this Feature feature)
        {
            feature.AntiForgeryToken = AntiforgeryTokenExtractor.ExtractAntiforgeryToken(feature.Response);

            feature.AntiForgeryCookie = AntiforgeryTokenExtractor.ExtractAntiforgeryCookieValue(feature.Response);

            return feature;
        }
    }
}
