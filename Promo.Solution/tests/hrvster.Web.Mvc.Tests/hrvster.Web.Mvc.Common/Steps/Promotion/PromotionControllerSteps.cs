using hrvster.Core;
using hrvster.Web.Models;
using Microsoft.Net.Http.Headers;
using TestUtilities.Data;

namespace hrvster.Web.Mvc.Common.Steps.Promotion
{
    public static class PromotionControllerSteps
    {
        public static Feature WhenIloadThePromotionCreationPage(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync(RouteList.CreatePromotion)).Wait();

            return feature;
        }

        public static Feature WhenILoadThePromotionsPage(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync(RouteList.Promotions)).Wait();

            return feature;
        }

        public static Feature AndCreateAPromotionUsing(this Feature feature, CreatePromotion form)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, RouteList.CreatePromotion);
            postRequest.Headers.Add("Cookie",
                new CookieHeaderValue(
                    AntiforgeryTokenExtractor.AntiForgeryCookieName,
                    feature.AntiForgeryCookie).ToString());

            var model = new Dictionary<string, string>
            {
                { nameof(form.Name), form.Name },
                { nameof(form.Description), form.Description },
                { nameof(form.CampaignTypeUid), form.CampaignTypeUid.ToString() },
                { nameof(form.StartDate), form.StartDate.ToLongDateString() },
                { nameof(form.EndDate), form.EndDate.ToLongDateString() },
                { AntiforgeryTokenExtractor.AntiForgeryFieldName, feature.AntiForgeryToken }
            };
            postRequest.Content = new FormUrlEncodedContent(model);

            Task.Run(async () => feature.Response = await feature.Client.SendAsync(postRequest)).Wait();

            return feature;
        }
    }
}
