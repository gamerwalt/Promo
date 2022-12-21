using hrvster.Core;
using hrvster.Web.Models;
using Microsoft.Net.Http.Headers;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Web.Mvc.Common.Steps.Authentication
{
    public static class RegisterSteps
    {
        public static Feature WhenILoadTheSignUpPage(this Feature feature)
        {
            Task.Run(async () => feature.Response = await feature.Client.GetAsync(RouteList.SignUp)).Wait();

            return feature;
        }

        public static Feature WhenISignUpUsing(this Feature feature, Register form)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, RouteList.SignUp);
            postRequest.Headers.Add("Cookie", new CookieHeaderValue(AntiforgeryTokenExtractor.AntiForgeryCookieName, feature.AntiForgeryCookie).ToString());

            var model = new Dictionary<string, string>
            {
                { nameof(form.EmailAddress), form.EmailAddress },
                { nameof(form.UserName), form.UserName },
                { nameof(form.PhoneNumber), form.PhoneNumber },
                { nameof(form.Password), form.Password },
                { nameof(form.FirstName), form.FirstName },
                { nameof(form.LastName), form.LastName },
                { nameof(form.AcceptedTerms), form.AcceptedTerms.ToString() },
                { AntiforgeryTokenExtractor.AntiForgeryFieldName, feature.AntiForgeryToken }
            };
            postRequest.Content = new FormUrlEncodedContent(model);

            Task.Run(async () => feature.Response = await feature.Client.SendAsync(postRequest)).Wait();

            return feature;
        }
    }
}
