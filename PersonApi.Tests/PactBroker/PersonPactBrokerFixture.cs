using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Hackney.Core.DynamoDb;
using Hackney.Core.Sns;
using Hackney.Core.Testing.DynamoDb;
using Hackney.Core.Testing.PactBroker;
using Hackney.Core.Testing.Sns;
using Microsoft.Extensions.DependencyInjection;
using PersonApi.V1.Domain;
using System;

namespace PersonApi.Tests.PactBroker
{
    public class PersonPactBrokerFixture : PactBrokerFixture<PactBrokerTestStartup>
    {
        protected override void SetEnvironmentVariables()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("Sns_LocalMode", "true");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:4566");

            EnsureEnvVarConfigured(Constants.ENV_VAR_PACT_BROKER_USER, "pact-broker-user");
            EnsureEnvVarConfigured(Constants.ENV_VAR_PACT_BROKER_PATH, "https://contract-testing-development.hackney.gov.uk/");
            EnsureEnvVarConfigured(Constants.ENV_VAR_PACT_BROKER_PROVIDER_NAME, "Person API V1");

            base.SetEnvironmentVariables();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDynamoDB();
            services.ConfigureDynamoDbFixture();

            services.ConfigureSns();
            services.ConfigureSnsFixture();

            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

            base.ConfigureServices(services);
        }

        protected override void ConfigureFixture(IServiceProvider provider)
        {
            var dynamoDbFixture = provider.GetRequiredService<IDynamoDbFixture>();
            dynamoDbFixture.EnsureTablesExist(DynamoDbTables.Tables);

            var snsFixture = provider.GetRequiredService<ISnsFixture>();
            snsFixture.CreateSnsTopic<PersonSns>("person.fifo", "PERSON_SNS_ARN");

            base.ConfigureFixture(provider);
        }
    }
}
