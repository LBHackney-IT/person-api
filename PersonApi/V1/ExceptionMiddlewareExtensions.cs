using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PersonApi.V1.Controllers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1
{
    // TODO: This should go in a common NuGet package...

    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    await HandleExceptions(context/*, logger*/).ConfigureAwait(false);
                });
            });
        }

        public static async Task HandleExceptions(HttpContext context)
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

                Trace.TraceError($"Request failed. {contextFeature.Error?.Message}");
            }

            var correlationId = context.Request.Headers[Constants.CorrelationId].FirstOrDefault();
            var exceptionResult = new ExceptionResult(message, context.TraceIdentifier,
                correlationId, context.Response.StatusCode);
            await context.Response.WriteAsync(exceptionResult.ToString()).ConfigureAwait(false);
        }
    }

    public class ExceptionResult
    {
        public const int DefaultStatusCode = 500;

        public ExceptionResult(string message, string traceId, string correlationId, int statusCode = DefaultStatusCode)
        {
            Message = message;
            TraceId = traceId;
            CorrelationId = correlationId;
            StatusCode = statusCode;
        }

        public string Message { get; private set; }
        public string TraceId { get; private set; }
        public string CorrelationId { get; private set; }
        public int StatusCode { get; private set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, _settings);
        }

        static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
