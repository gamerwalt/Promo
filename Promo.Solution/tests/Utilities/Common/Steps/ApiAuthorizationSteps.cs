using hrvster.Core.Authentication;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestUtilities.Common.Steps
{
    public static class ApiAuthorizationSteps
    {
        public static TFeature ApiAuthorization<TFeature>(
            this IApiFeature<TFeature> feature,
            string authToken)
            where TFeature : class
        {
            feature.Client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(authToken)
                ? null
                : new AuthenticationHeaderValue("Bearer", authToken);

            return feature as TFeature;
        }
    }

    public static class WebAuthorizationSteps
    {
        public static TFeature WebAuthorization<TFeature>(
            this IWebFeature<TFeature> feature,
            string token)
            where TFeature : class
        {
            feature.Client.DefaultRequestHeaders.Add(SessionConstants.UserToken, token);
            feature.Client.DefaultRequestHeaders.Add(SessionConstants.UserDisplayName, "DisplayName");

            return feature as TFeature;
        }
    }
}
