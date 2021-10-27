using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace PersonApi.Tests.PactBroker
{
    public class PactBrokerFixture : IDisposable
    {
        private readonly IHost _server;
        public string ServerUri { get; }

        public IAmazonDynamoDB DynamoDb { get; private set; }
        public IDynamoDBContext DynamoDbContext { get; private set; }
        public IAmazonSimpleNotificationService SimpleNotificationService { get; private set; }
        public IAmazonSQS AmazonSQS { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public PactBrokerFixture()
        {
            ServerUri = "http://localhost:9222";

            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("Localstack_SnsServiceUrl", "http://localhost:4566");

            EnsureEnvVarConfigured("pact-broker-user", "pact-broker-user");
            EnsureEnvVarConfigured("pact-broker-user-password", "YBZKGe2LV4RvQ5bN");
            EnsureEnvVarConfigured("pact-broker-path", "https://contract-testing-development.hackney.gov.uk/");
            EnsureEnvVarConfigured("pact-broker-provider-name", "Person API V1");

            _server = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(webBuilder =>
                              {
                                  webBuilder.UseUrls(ServerUri);
                                  webBuilder.ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                                            .UseStartup<TestStartup>()
                                            .ConfigureServices((ctx, services) =>
                                            {
                                                ConfigureServicesInternal(services);
                                            });
                              })
                              .Build();

            CreateSnsTopic(SimpleNotificationService);

            _server.Start();
        }

        private void ConfigureServicesInternal(IServiceCollection services)
        {
            var url = Environment.GetEnvironmentVariable("DynamoDb_LocalServiceUrl");
            var snsUrl = Environment.GetEnvironmentVariable("Localstack_SnsServiceUrl");
            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var clientConfig = new AmazonDynamoDBConfig { ServiceURL = url };
                return new AmazonDynamoDBClient(clientConfig);
            });
            services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
            {
                var clientConfig = new AmazonSimpleNotificationServiceConfig { ServiceURL = snsUrl };
                return new AmazonSimpleNotificationServiceClient(clientConfig);
            });

            services.ConfigureAws();

            var serviceProvider = services.BuildServiceProvider();
            DynamoDb = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
            DynamoDbContext = serviceProvider.GetRequiredService<IDynamoDBContext>();
            SimpleNotificationService = serviceProvider.GetRequiredService<IAmazonSimpleNotificationService>();
            AmazonSQS = new AmazonSQSClient(new AmazonSQSConfig() { ServiceURL = snsUrl });

            Configuration = serviceProvider.GetRequiredService<IConfiguration>();

            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;

            DynamoDbTables.EnsureTablesExist(DynamoDb, DynamoDbTables.Tables);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                ProviderStateMiddleware.Cleanup();
                _server.Dispose();
                _disposed = true;
            }
        }

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }

        private void CreateSnsTopic(IAmazonSimpleNotificationService simpleNotificationService)
        {
            var snsAttrs = new Dictionary<string, string>();
            snsAttrs.Add("fifo_topic", "true");
            snsAttrs.Add("content_based_deduplication", "true");

            var response = simpleNotificationService.CreateTopicAsync(new CreateTopicRequest
            {
                Name = "person",
                Attributes = snsAttrs
            }).Result;

            Environment.SetEnvironmentVariable("PERSON_SNS_ARN", response.TopicArn);
        }
    }

    [CollectionDefinition("Pact Broker collection", DisableParallelization = true)]
    public class PactBrokerCollection : ICollectionFixture<PactBrokerFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
