using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PersonApi.Tests.PactBroker
{
    public class AuthorizationTokenReplacementMiddleware
    {
        private const string Authorization = "Authorization";

        private readonly RequestDelegate _next;

        public AuthorizationTokenReplacementMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(Authorization))
            {
                // swap for a valid key
                string token = TokenGenerator.Generate();
                context.Request.Headers[Authorization] = $"Bearer {token}";
            }

            await _next(context).ConfigureAwait(false);
        }
    }
}
