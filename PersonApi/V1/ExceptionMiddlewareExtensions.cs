using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Controllers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PersonApi.V1
{
    // TODO: This should go in a common NuGet package...

    public static class ExceptionMiddlewareExtensions
    {
        [ExcludeFromCodeCoverage]
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, IApiLogger logger)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    await HandleExceptions(context, logger).ConfigureAwait(false);
                });
            });
        }

        public static async Task HandleExceptions(HttpContext context, IApiLogger logger)
        {
            context.Response.ContentType = "application/json";
            string message = "Internal Server Error.";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                switch (contextFeature.Error)
                {
                    // TODO: Handle our specific errors here.

                    case Exception ex:
                        message = ex.Message;
                        break;
                    default:
                        break;
                }

                logger.Log(LogLevel.Error, "Exception thrown", contextFeature.Error);
            }

            var correlationId = context.Request.Headers.GetHeaderValue(Constants.CorrelationId);
            var exceptionResult = new ExceptionResult(message, context.TraceIdentifier,
                correlationId, context.Response.StatusCode);
            await context.Response.WriteAsync(exceptionResult.ToString()).ConfigureAwait(false);
        }
    }
}
