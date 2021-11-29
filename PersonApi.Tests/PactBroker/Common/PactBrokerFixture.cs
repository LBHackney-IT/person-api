using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Microsoft.AspNetCore.Builder;
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
            ServerUri = "http://localhost:9222";

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
                                            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

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

            var user = Configuration.GetValue<string>("pact-broker-user");
            var pwd = Configuration.GetValue<string>("pact-broker-user-password");
            var pactUriOptions = new PactUriOptions().SetBasicAuthentication(user, pwd);

            var name = Configuration.GetValue<string>("pact-broker-provider-name");
            var path = Configuration.GetValue<string>("pact-broker-path");

            IPactVerifier pactVerifier = new PactVerifier(pactVerifierConfig);
            pactVerifier
                .ServiceProvider(name, ServerUri)
                .PactBroker(path, pactUriOptions)
                .ProviderState(ServerUri + "/provider-states")
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
                {
                    _server.StopAsync().GetAwaiter().GetResult();
                    _server.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
