using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1
{
    public class LoggingScopeMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingScopeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<LoggingScopeMiddleware> logger)
        {
            var correlationId = context.Request.Headers.GetHeaderValue(Constants.CorrelationId);
            var userId = context.Request.Headers.GetHeaderValue(Constants.UserId);

            using (logger.BeginScope("CorrelationId: {CorrelationId}; UserId: {UserId}", correlationId, userId))
            {
                if (_next != null)
                    await _next(context).ConfigureAwait(false);
            }
        }
    }

    public static class LoggingScopeMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingScope(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingScopeMiddleware>();
        }
    }
}
