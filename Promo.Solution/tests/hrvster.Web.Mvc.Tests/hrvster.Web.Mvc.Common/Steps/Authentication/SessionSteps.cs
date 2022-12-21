using hrvster.Core;
using hrvster.Core.Requests.WebRequests;
using Microsoft.Net.Http.Headers;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Web.Mvc.Common.Steps.Authentication
{
    public static class SessionSteps
    {
        public static Feature WhenISignInUsing(this Feature feature, SigninForm form)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Session/SignIn");
            postRequest.Headers.Add("Cookie",
                new CookieHeaderValue(
                    AntiforgeryTokenExtractor.AntiForgeryCookieName,
                    feature.AntiForgeryCookie).ToString());

            var model = new Dictionary<string, string>
            {
                { nameof(form.Email), form.Email },
                { nameof(form.Password), form.Password},
                { AntiforgeryTokenExtractor.AntiForgeryFieldName, feature.AntiForgeryToken }
            };
            postRequest.Content = new FormUrlEncodedContent(model);

            Task.Run(async () => feature.Response = await feature.Client.SendAsync(postRequest)).Wait();

            return feature;
        }

        public static Feature WhenILoadTheSignInForm(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync($"/Session/SignIn")).Wait();

            return feature;
        }

        public static Feature WhenISignOut(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.PostAsync(RouteList.SignOut, null)).Wait();

            return feature;
        }
    }
}
