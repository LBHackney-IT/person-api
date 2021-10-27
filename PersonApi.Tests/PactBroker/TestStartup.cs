using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PersonApi.Tests.PactBroker
{
    public class TestStartup
    {
        private readonly Startup _inner;

        public TestStartup(IConfiguration configuration)
        {
            _inner = new Startup(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _inner.ConfigureServices(services);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseMiddleware<ProviderStateMiddleware>(app.ApplicationServices)
               .UseMiddleware<AuthorizationTokenReplacementMiddleware>();

            Startup.Configure(app, env, logger);
        }
    }
}
