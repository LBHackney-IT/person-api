using Microsoft.AspNetCore.Builder;

namespace Hackney.Core.Testing.PactBroker
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UsePactBroker(this IApplicationBuilder app, string audience)
        {
            return app.UseMiddleware<ProviderStateMiddleware>(app.ApplicationServices)
                      .UseMiddleware<AuthorizationTokenReplacementMiddleware>(audience);
        }
    }
}
