using Hackney.Core.DynamoDb;
using Hackney.Core.Sns;
using Hackney.Core.Testing.DynamoDb;
using Hackney.Core.Testing.PactBroker;
using Hackney.Core.Testing.Sns;
using Microsoft.Extensions.DependencyInjection;
using PersonApi.V1.Domain;
using System;
using Xunit;

namespace PersonApi.Tests.PactBroker
{
    public class PersonPactBrokerFixture : PactBrokerFixture<PactBrokerTestStartup>
    {
        public IDynamoDbFixture DynamoDbFixture { get; private set; }
        public ISnsFixture SnsFixture { get; private set; }

        public PersonPactBrokerFixture()
        { }

        protected override void SetEnvironmentVariables()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("Sns_LocalMode", "true");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:4566");

            EnsureEnvVarConfigured("pact-broker-user", "pact-broker-user");
            EnsureEnvVarConfigured("pact-broker-user-password", "YBZKGe2LV4RvQ5bN");
            EnsureEnvVarConfigured("pact-broker-path", "https://contract-testing-development.hackney.gov.uk/");
            EnsureEnvVarConfigured("pact-broker-provider-name", "Person API V1");

            base.SetEnvironmentVariables();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDynamoDB();
            services.ConfigureDynamoDbFixture();

            services.ConfigureSns();
            services.ConfigureSnsFixture();

            base.ConfigureServices(services);
        }

        protected override void ConfigureFixture(IServiceProvider provider)
        {
            DynamoDbFixture = provider.GetRequiredService<IDynamoDbFixture>();
            DynamoDbFixture.EnsureTablesExist(DynamoDbTables.Tables);

            SnsFixture = provider.GetRequiredService<ISnsFixture>();
            SnsFixture.CreateSnsTopic<PersonSns>("person", "PERSON_SNS_ARN");

            base.ConfigureFixture(provider);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                if (DynamoDbFixture != null)
                    DynamoDbFixture.Dispose();
                if (SnsFixture != null)
                    SnsFixture.Dispose();

                base.Dispose(disposing);
            }
        }

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }
    }
}
