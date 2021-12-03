using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hackney.Core.Testing.PactBroker
{
    public abstract class PactBrokerFixture<TStartup> : IDisposable where TStartup : class
    {
        protected readonly IHost _server;
        public string ServerUri { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected PactBrokerFixture()
        {
            ServerUri = Constants.DEFAULT_SERVER_URI;

            SetEnvironmentVariables();

            _server = Host.CreateDefaultBuilder()
                          .ConfigureWebHostDefaults(webBuilder =>
                          {
                              webBuilder.UseUrls(ServerUri);
                              webBuilder.ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                                        .UseStartup<TStartup>()
                                        .ConfigureServices((ctx, services) =>
                                        {
                                            ConfigureServices(services);

                                            var serviceProvider = services.BuildServiceProvider();

                                            Configuration = serviceProvider.GetRequiredService<IConfiguration>();
                                            ConfigureFixture(serviceProvider);
                                        });
                          })
                          .Build();

            _server.Start();
        }

        protected virtual void SetEnvironmentVariables() { }

        protected virtual void ConfigureServices(IServiceCollection services) { }

        protected virtual void ConfigureFixture(IServiceProvider provider) { }

        public void RunPactBrokerTest(IList<IOutput> outputters = null)
        {
            if (outputters is null) outputters = new List<IOutput>();
            if (!outputters.Any(x => x.GetType() == typeof(ConsoleOutput)))
                outputters.Add(new ConsoleOutput());

            var pactVerifierConfig = new PactVerifierConfig
            {
                Outputters = outputters
            };

            var user = Configuration.GetValue<string>(Constants.ENV_VAR_PACT_BROKER_USER);
            var pwd = Configuration.GetValue<string>(Constants.ENV_VAR_PACT_BROKER_USER_PASSWORD);
            var pactUriOptions = new PactUriOptions().SetBasicAuthentication(user, pwd);

            var name = Configuration.GetValue<string>(Constants.ENV_VAR_PACT_BROKER_PROVIDER_NAME);
            var path = Configuration.GetValue<string>(Constants.ENV_VAR_PACT_BROKER_PATH);

            IPactVerifier pactVerifier = new PactVerifier(pactVerifierConfig);
            pactVerifier
                .ServiceProvider(name, ServerUri)
                .PactBroker(path, pactUriOptions)
                .ProviderState(ServerUri + Constants.PROVIDER_STATES_ROUTE)
                .Verify();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (_server != null)
                    _server.Dispose();

                _disposed = true;
            }
        }

        protected void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }
    }
}
