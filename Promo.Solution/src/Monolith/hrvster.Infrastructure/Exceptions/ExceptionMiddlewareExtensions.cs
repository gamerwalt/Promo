using hrvster.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace hrvster.Infrastructure
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {

                        logger.LogError($"Something went wrong... : {contextFeature.Error}");

                        await context.Response.WriteAsync
                        (
							new ApiResponse(null, context.Response.StatusCode)
							{
								message = contextFeature.Error.Message
									.Replace(Environment.NewLine, "")
									.Replace("  -- :", "")
									.Replace(" Severity: Error", "")
									.Replace("Validation failed: ", "")
							}.ToString()
                        );
                    }
                });
            });
        }
    }
}
