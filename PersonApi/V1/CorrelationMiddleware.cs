using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PersonApi.V1.Infrastructure
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers["x-correlation-id"].Count == 0)
            {
                context.Request.Headers["x-correlation-id"] = Guid.NewGuid().ToString();
            }
            if (_next != null)
                await _next.Invoke(context).ConfigureAwait(false);
        }


    }

    public static class CorrelationMiddlewareExtension
    {
        public static IApplicationBuilder UseCorrelation(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<CorrelationMiddleware>();
        }
    }

}
