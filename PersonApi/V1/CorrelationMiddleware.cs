using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PersonApi.V1.Controllers
{
    // TODO: This should go in a common NuGet package...

    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationIdHeader = context.Request.Headers.GetHeaderValue(Constants.CorrelationId);
            if (string.IsNullOrEmpty(correlationIdHeader)
                || !Guid.TryParse(correlationIdHeader, out Guid correlationId))
            {
                correlationId = Guid.NewGuid();
                context.Request.Headers.Add(Constants.CorrelationId, new StringValues(correlationId.ToString()));
            }

            CallContext.NewContext();
            CallContext.Current.SetValue(Constants.CorrelationId, correlationId);

            var userId = context.Request.Headers.GetHeaderValue(Constants.UserId);
            CallContext.Current.SetValue(Constants.UserId, userId);

            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(Constants.CorrelationId))
                {
                    context.Response.Headers.Add(Constants.CorrelationId, new StringValues(correlationId.ToString()));
                }

                return Task.CompletedTask;
            });
            context.Response.OnCompleted(() =>
            {
                CallContext.ClearCurrentContext();
                return Task.CompletedTask;
            });

            if (_next != null)
                await _next(context).ConfigureAwait(false);
        }
    }

    [ExcludeFromCodeCoverage]
    public static class CorrelationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationMiddleware>();
        }
    }

}
