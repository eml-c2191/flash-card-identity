using Identity.API.Models.Validations;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace Identity.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        private const string ErrorLogMessageFormat = "Something went wrong: {0}";
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string errorMessage = string.Format(ErrorLogMessageFormat, contextFeature.Error.Message);
                        logger.LogError(contextFeature.Error.Message);
                        await context.Response
                            .WriteAsync(JsonConvert.SerializeObject(new ErrorDetail(context.Response.StatusCode, errorMessage)));
                    }
                });
            });
        }
    }
}
