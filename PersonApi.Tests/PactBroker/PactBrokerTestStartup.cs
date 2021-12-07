using Hackney.Core.Testing.PactBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PersonApi.Tests.PactBroker
{
    public class PactBrokerTestStartup
    {
        private readonly Startup _inner;

        public PactBrokerTestStartup(IConfiguration configuration)
        {
            _inner = new Startup(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPactBrokerHandler, PersonPactBrokerHandler>();
            _inner.ConfigureServices(services);
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UsePactBroker("person-api");
            Startup.Configure(app, env, logger);
        }
    }
}
