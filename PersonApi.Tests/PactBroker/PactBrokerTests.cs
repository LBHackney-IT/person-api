using Microsoft.Extensions.Configuration;
using PactNet;
using PactNet.Infrastructure.Outputters;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PersonApi.Tests.PactBroker
{
    [Collection("Pact Broker collection")]
    public class PactBrokerTests
    {
        private readonly string _serverUri;
        private readonly IConfiguration _configuration;
        private readonly PactVerifierConfig _pactVerifierConfig;

        public PactBrokerTests(PactBrokerFixture testFixture, ITestOutputHelper output)
        {
            _configuration = testFixture.Configuration;
            _serverUri = testFixture.ServerUri;
            _pactVerifierConfig = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(output)
                }
            };
        }

        [Fact]
        public void EnsureTheApiHonoursPactWithConsumer()
        {
            var user = _configuration.GetValue<string>("pact-broker-user");
            var pwd = _configuration.GetValue<string>("pact-broker-user-password");
            var pactUriOptions = new PactUriOptions().SetBasicAuthentication(user, pwd);

            var name = _configuration.GetValue<string>("pact-broker-provider-name");
            var path = _configuration.GetValue<string>("pact-broker-path");

            IPactVerifier pactVerifier = new PactVerifier(_pactVerifierConfig);
            pactVerifier
                .ServiceProvider(name, _serverUri)
                .PactBroker(path, pactUriOptions)
                .ProviderState(_serverUri + "/provider-states")
                .Verify();
        }
    }
}
